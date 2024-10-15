
using Blog.Data.Configurations;
using Blog.Data.Context;
using Blog.Web.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionSqLite") ?? throw new InvalidOperationException("Connection string 'DefaultConnectionSqLite' not found.");

    builder.Services.AddDbContext<MeuDbContext>(options =>
        options.UseSqlite(connectionString));

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));
}
else
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    
    builder.Services.AddDbContext<MeuDbContext>(options =>
     options.UseSqlServer(connectionString));

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}

builder.Services.AddHttpContextAccessor();




builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>() 
                .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllersWithViews();

builder.Services.ResolveDependencies();

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

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.UseDbMigrationHelper();

app.Run();
