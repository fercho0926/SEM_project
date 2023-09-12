using Microsoft.AspNetCore.Builder;
using SEM_project.Data;
using SEM_project.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);
//builder.Configuration.AddSecretsManager(
//    configurator: options =>
//    {
//        options.ConfigureSecretsManagerConfig(new AmazonSecretsManagerConfig());
//        options.PollingInterval = TimeSpan.FromSeconds(10);
//        options.SecretFilter = entry => entry.Name.Contains("Database");
//    });

//var connectionString = builder.Configuration.GetValue<string>("Dev_DatabaseConection");
var connectionString =
    "Server=EN2010480;Database=Sem_Database;Trusted_Connection=True;MultipleActiveResultSets=true; TrustServerCertificate=True";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddControllersWithViews();

// Services dependencies
//builder.Services.AddTransient<IDocumentService, DocumentService>();
builder.Services.AddTransient<IMailService, MailService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute().RequireAuthorization(); });


app.MapControllerRoute(
    name: "default",
    pattern: "{area:identity}/{controller=account}/{action=login}");
app.MapRazorPages();

app.Run();