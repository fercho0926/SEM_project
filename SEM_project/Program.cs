using SEM_project.Data;
using SEM_project.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

var connectionString =
    //"Server=P200583946\\SQLEXPRESS;Database=Sem_Database;Trusted_Connection=True;MultipleActiveResultSets=true; TrustServerCertificate=True";
    "Server=EN2010480;Database=Sem_Database;Trusted_Connection=True;MultipleActiveResultSets=true; TrustServerCertificate=True";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IMailService, MailService>();


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