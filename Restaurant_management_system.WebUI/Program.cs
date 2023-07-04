using System.Globalization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Logging.AzureAppServices;

using Restaurant_management_system.Core.MailAggregate;
using Restaurant_management_system.Core.Interfaces;
using Restaurant_management_system.Core.Services;
using Restaurant_management_system.Infrastructure;
using Restaurant_management_system.Infrastructure.Data;
using Restaurant_management_system.Infrastructure.Data.Authorization;

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

// Azure key vault config
string kvURL = builder.Configuration.GetValue<string>("keyVaultConfig:KVUrl")!;
string tenantID = builder.Configuration.GetValue<string>("keyVaultConfig:TenantID")!;
string clientID = builder.Configuration.GetValue<string>("keyVaultConfig:ClientID")!;
string clientSecret = builder.Configuration.GetValue<string>("keyVaultConfig:ClientSecret")!;
builder.Configuration.AddAzureKeyVault(
	new Uri(kvURL),
	new ClientSecretCredential(tenantID, clientID, clientSecret),
	new AzureKeyVaultConfigurationOptions());

builder.Services.AddControllersWithViews();

// DB
if (builder.Environment.IsDevelopment())
	builder.Services.AddDatabaseDeveloperPageExceptionFilter();
string? AuthConnectionString = builder.Configuration.GetConnectionString("AuthContext") ?? throw new InvalidOperationException("Connection string 'AuthContext' not found.");
string? DataConnectionString = builder.Configuration.GetConnectionString("DataConnectionString") ?? throw new InvalidOperationException("Connection string 'DataConnectionString' not found.");
builder.Services.AddDbContext(AuthConnectionString!);
builder.Services.AddDbContext<RestaurantContext>(options =>
	options.UseSqlServer(DataConnectionString));

// Mail
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

// API
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

// Authentication
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
	// Password settings
	options.Password.RequireDigit = true;
	options.Password.RequireUppercase = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequiredLength = 6;
	options.Password.RequiredUniqueChars = 0;

	// Lockout settings
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.MaxFailedAccessAttempts = 7;
	options.Lockout.AllowedForNewUsers = true;

	// User settings
	options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
	options.User.RequireUniqueEmail = true;
});
builder.Services.AddAuthentication()
	.AddGoogle(googleOptions =>
	{
		googleOptions.ClientId = builder.Configuration.GetValue<string>("Authentication:Google:ClientId")!;
		googleOptions.ClientSecret = builder.Configuration.GetValue<string>("Authentication:Google:ClientSecret")!;
	})
	.AddGitHub(githubOptions =>
	{
		githubOptions.ClientId = builder.Configuration.GetValue<string>("Authentication:Github:ClientId")!;
		githubOptions.ClientSecret = builder.Configuration.GetValue<string>("Authentication:Github:ClientSecret")!;
	})
	.AddMicrosoftAccount(microsoftOptions =>
	{
		microsoftOptions.ClientId = builder.Configuration.GetValue<string>("Authentication:Microsoft:ClientId")!;
		microsoftOptions.ClientSecret = builder.Configuration.GetValue<string>("Authentication:Microsoft:ClientSecret")!;
	})
	.AddTwitter(twitterOptions =>
	{
		twitterOptions.ConsumerKey = builder.Configuration.GetValue<string>("Authentication:Twitter:ClientId")!;
		twitterOptions.ConsumerSecret = builder.Configuration.GetValue<string>("Authentication:Twitter:ClientSecret")!;
	});
builder.Services.ConfigureApplicationCookie(options =>
{
	options.Cookie.HttpOnly = true;
	options.ExpireTimeSpan = TimeSpan.FromHours(24);

	options.LoginPath = "/Identity/Account/Login";
	options.AccessDeniedPath = "/Identity/Account/AccessDenied";
	options.SlidingExpiration = true;
});

// Authorization
builder.Services.AddAuthorization(options =>
{
	options.FallbackPolicy = new AuthorizationPolicyBuilder()
		.RequireAuthenticatedUser()
		.Build();
});
builder.Services.AddSingleton<IAuthorizationHandler, AdministratorsAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, WaitersAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, CooksAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ChefAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, GuestAuthorizationHandler>();

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
//    var authContext = services.GetRequiredService<ApplicationDbContext>();
//    var dataContext = services.GetRequiredService<RestaurantContext>();
//    authContext.Database.Migrate();
//    await AuthDbInitializer.Initialize(services, builder.Configuration.GetValue<string>("TestUserPassword")!);
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
app.MapRazorPages();

app.Run();