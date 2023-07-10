using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;

using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Logging.AzureAppServices;

using Restaurant_management_system.Core.MailAggregate;
using Restaurant_management_system.Core.Interfaces;
using Restaurant_management_system.Core.Services;
using Restaurant_management_system.Infrastructure;
using Restaurant_management_system.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Logs
builder.Logging
	.ClearProviders()
	.AddConsole()
	.AddAzureWebAppDiagnostics()
	.SetMinimumLevel(LogLevel.Warning); // if not specified in appsettings.json
builder.Services.Configure<AzureFileLoggerOptions>(options =>
{
	options.FileName = "azure-diagnostics-";
	options.FileSizeLimit = 50 * 1024;
	options.RetainedFileCountLimit = 5;
});
builder.Services.Configure<AzureBlobLoggerOptions>(options =>
{
	options.BlobName = "log.txt";
});
builder.Logging.AddApplicationInsights(
	configureTelemetryConfiguration: (config) =>
		config.ConnectionString = builder.Configuration.GetConnectionString("AppInsights") ?? throw new InvalidOperationException("Connection string 'AppInsights' not found."),
	configureApplicationInsightsLoggerOptions: (options) => { }
);
IdentityModelEventSource.ShowPII = true;

// Azure key vault config
string kvURL = builder.Configuration.GetValue<string>("keyVaultConfig:KVUrl")!;
string tenantID = builder.Configuration.GetValue<string>("keyVaultConfig:TenantID")!;
string clientID = builder.Configuration.GetValue<string>("keyVaultConfig:ClientID")!;
string clientSecret = builder.Configuration.GetValue<string>("keyVaultConfig:ClientSecret")!;
builder.Configuration.AddAzureKeyVault(
	new Uri(kvURL),
	new ClientSecretCredential(tenantID, clientID, clientSecret),
	new AzureKeyVaultConfigurationOptions());

// DB
//if (builder.Environment.IsDevelopment())
//	builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
string? DataConnectionString = builder.Configuration.GetConnectionString("DataConnectionString") ?? throw new InvalidOperationException("Connection string 'DataConnectionString' not found.");
builder.Services.AddDbContext<RestaurantContext>(options =>
	options.UseSqlServer(DataConnectionString));

// Mail
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMyEmailSender, SmtpEmailSender>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Restaurant Management System API",
		Version = "v1",
		Description = "An API to manage orders in restaurant"
	});
	c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
	// Sort controllers in /swagger/index.html
	SwaggerControllerOrder<ControllerBase> swaggerControllerOrder = new SwaggerControllerOrder<ControllerBase>(Assembly.GetEntryAssembly()!);
	c.OrderActionsBy((apiDesc) => $"{swaggerControllerOrder.SortKey(apiDesc.ActionDescriptor.RouteValues["controller"]!)}");
});

// Auth
builder.Services.AddAuthentication("Bearer")
	.AddJwtBearer("Bearer", options =>
	{
		options.Authority = "https://" + builder.Configuration.GetValue<string>("Authority:IdentityServer:Uri"); // https://localhost:9003
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateAudience = false
		};
	});
builder.Services.AddAuthorization(options =>
	options.AddPolicy("ApiScope", policy =>
	{
		policy.RequireAuthenticatedUser();
		policy.RequireClaim("scope", "api");
	})
);

// Localization
builder.Services.AddLocalization(options =>
	options.ResourcesPath = "Resources");
builder.Services.AddMvc()
	.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
	.AddDataAnnotationsLocalization();
var supportedCultures = new[]
{
	new CultureInfo("en"),
	new CultureInfo("de"),
	new CultureInfo("uk")
};
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	options.DefaultRequestCulture = new RequestCulture("uk");
	options.SetDefaultCulture("uk");
	options.SupportedCultures = supportedCultures;
	options.SupportedUICultures = supportedCultures;
});

// CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("CORS", corsPolicyBuilder => corsPolicyBuilder
		.AllowAnyOrigin()
		.AllowAnyMethod()
		.AllowAnyHeader());
});

builder.Services.AddControllersWithViews();

var app = builder.Build();





if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

// DB init
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var dataContext = services.GetRequiredService<RestaurantContext>();
//    DbInitializer.Initialize(dataContext);
//}

app.UseRouting();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseCors("CORS");

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API V1"));

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.MapControllerRoute(
	name: "swagger",
	pattern: "swagger/index.html");
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();
app.MapRazorPages();

app.Run();