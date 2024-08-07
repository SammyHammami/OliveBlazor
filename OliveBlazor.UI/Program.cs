using Blazored.Toast;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OliveBlazor.Core.Application;
using OliveBlazor.Core.Application.Services;
using OliveBlazor.Core.Domain.Security;
using OliveBlazor.Infrastructure;
using OliveBlazor.Infrastructure.Data;
using OliveBlazor.Infrastructure.Indentity;
using OliveBlazor.Infrastructure.Indentity.Permissions;
using OliveBlazor.UI.Areas.Identity;
using OliveBlazor.UI.Data;
using OliveBlazor.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<UserIdentity>>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IErrorService, ErrorService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<WeatherForecastService>();

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero; // Immediate validation
});



builder.Services.AddIdentity<UserIdentity, RoleIdentity>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
    }) // Registering Identity services
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders(); //



builder.Services.AddSingleton<IAuthorizationPolicyProvider, EnumPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, EnumRequirementHandler>();
builder.Services.AddBlazoredToast();


builder.Services.AddAuthorization(options =>
{
    foreach (var perm in Enum.GetValues(typeof(Permissions)).Cast<Permissions>())
    {
        options.AddPolicy(perm.ToString(), policy => policy.RequireClaim("Permission", perm.ToString()));
    }
});




builder.Services.AddInfrastructure(builder.Configuration)
    .AddApplicationServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Add this section at the end of the Program.cs before app.Run();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var userManager = services.GetRequiredService<UserManager<UserIdentity>>();
        var roleManager = services.GetRequiredService<RoleManager<RoleIdentity>>();
        await SeedData.Initialize(services, userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();


app.Run();
