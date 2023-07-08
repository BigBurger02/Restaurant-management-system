using IdentityServer;
using Serilog;
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.CreateBootstrapLogger();

Log.Information("Starting up");

try
{
	var builder = WebApplication.CreateBuilder(args);

	builder.Host.UseSerilog((ctx, lc) => lc
		.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
		.Enrich.FromLogContext()
		.ReadFrom.Configuration(ctx.Configuration));

	// Azure key vault config
	string kvURL = builder.Configuration.GetValue<string>("keyVaultConfig:KVUrl")!;
	string tenantID = builder.Configuration.GetValue<string>("keyVaultConfig:TenantID")!;
	string clientID = builder.Configuration.GetValue<string>("keyVaultConfig:ClientID")!;
	string clientSecret = builder.Configuration.GetValue<string>("keyVaultConfig:ClientSecret")!;
	builder.Configuration.AddAzureKeyVault(
		new Uri(kvURL),
		new ClientSecretCredential(tenantID, clientID, clientSecret),
		new AzureKeyVaultConfigurationOptions());

	var app = builder
		.ConfigureServices()
		.ConfigurePipeline();

	if (args.Contains("/seed"))
	{
		Log.Information("Seeding database...");
		SeedData.EnsureSeedData(app, "12345");
		Log.Information("Done seeding database. Exiting.");
		return;
	}

	app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "HostAbortedException") // HostAbortedException thrown because of /seed argument
{
	Log.Fatal(ex, "Unhandled exception");
}
finally
{
	Log.Information("Shut down complete");
	Log.CloseAndFlush();
}