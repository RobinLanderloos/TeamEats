using System.Diagnostics;
using TeamEats.Client.Pages;
using TeamEats.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeamEats.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

// add .env file to configuration
var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddJsonFile(".env", optional: true, reloadOnChange: true)
    .Build();

var connString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "team-eats.db");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite($"Data Source='{connString}'"));
// builder.Services.AddIdentityApiEndpoints<ApplicationUser>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityCore<ApplicationUser>()
	.AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
	.AddOpenIdConnect("EntraExternalId", "EntraExternalId", options =>
	{
		options.ClientId = builder.Configuration["EntraExternalId:ClientId"];
		options.ClientSecret = builder.Configuration["EntraExternalId:ClientSecret"];
		options.Authority = builder.Configuration["EntraExternalId:Authority"];
		options.ResponseType = "code";
		options.UsePkce = true;
		options.MapInboundClaims = false;
		options.CallbackPath = "/signin-oidc";
	});

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

// app.MapIdentityApi<ApplicationUser>();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveWebAssemblyRenderMode()
	.AddAdditionalAssemblies(typeof(TeamEats.Client._Imports).Assembly);

app.Run();