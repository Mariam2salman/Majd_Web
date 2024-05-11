using Majd_Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Majd_Web.Models;
using Majd_Web.Repository;
using Microsoft.AspNetCore.Identity.UI.Services;
using Majd_Web.Utilty;
using Majd_Web.Utility;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDBContext>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IExperienceRepository, ExperienceRepository>();

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
//{
//    options.SignIn.RequireConfirmedAccount = false;
//    options.Password.RequiredUniqueChars = 0;
//    options.Password.RequireUppercase = false;
//    options.Password.RequiredLength = 8;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireLowercase = false;
//});

builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});


builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapRazorPages();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
await SeedRolesAsync(app.Services);

app.Run();
async Task SeedRolesAsync(IServiceProvider services)
{
    using (var scope = services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new[] { "Admin", "Employer", "Alumni" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
    using (var scope = services.CreateScope())
    {
        var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        string email = "admin@gmail.com";
        string password = "$123456Aa";
        if (await UserManager.FindByEmailAsync(email) == null)
        {
            var user = new ApplicationUser();
            user.UserName = email;
            user.Email = email;

            await UserManager.CreateAsync(user, password);
            await UserManager.AddToRoleAsync(user, "Admin");
        }

    }
}