using PlantifyControlPanel.Helper;
using PlantifyControlPanel.Profiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PlantifyApp.Repository.Identity;
using PlantifyApp.Core.Models;
using PlantifyApp.Apis.Extension;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace PlantifyControlPanel
{
    public  class Program
    {
        private static async Task CreateRolesandUsers(IdentityConnection context ,RoleManager<IdentityRole> RoleManager,UserManager<ApplicationUser> userManager)
        {
            // Creating Admin Role and default user. 
            var result = await RoleManager.Roles.FirstOrDefaultAsync(r=>r.Name=="Admin");
            if (result == null)
            {
                // Create an admin role. 
                var Role = new IdentityRole();
                Role.Name = "Admin";
                await RoleManager.CreateAsync(Role);

                // Create an admin user who will maintain the website 
                var User = new ApplicationUser()
                {
                    DisplayName = "admin",
                    Email = "admin@gmail.com",
                    UserName = "admin",
                    PhoneNumber = "1234567890"

                };

                var CheckUser = await userManager.CreateAsync(User, "P@asswo0rd");  

                if (CheckUser.Succeeded)
                {
                  await userManager.AddToRoleAsync(User,"Admin");

                }
            }

            // Create PowerUser role.    
            var UserRole = await RoleManager.FindByNameAsync("User");
            if (UserRole ==null)
            {
                var Role = new IdentityRole();
                Role.Name = "User";
               await  RoleManager.CreateAsync(Role);
            }
        }




        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // DBConnection
            builder.Services.AddDbContext<IdentityConnection>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("defaultconnection"));
            });

            // AutoMapper
            builder.Services.AddAutoMapper(m => m.AddProfile(new UserProfile()));


            // Identity 
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityConnection>().AddDefaultTokenProviders();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                


            }).AddCookie(options =>
            {
                options.Cookie.Name = "Cookie"; 
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
            });

            builder.Services.AddSession(options =>
            {
                
                options.IdleTimeout = TimeSpan.FromMinutes(20);
               
            });

            // MailKit
            builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));
            builder.Services.AddScoped<IEmailSetting, EmailSetting>();

      



                var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
        
            app.UseRouting();
            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {

                // Get required services
                var context = scope.ServiceProvider.GetRequiredService<IdentityConnection>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // Create roles and users
                await CreateRolesandUsers(context, roleManager, userManager);
            }


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Admin}/{action=Login}/{id?}");
            });

            app.Run();
        }
    }
}
