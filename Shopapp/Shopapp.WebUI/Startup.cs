using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shopapp.Bussiness.Abstract;
using Shopapp.Bussiness.Concrete;
using Shopapp.DataAccess.Abstarct;
using Shopapp.DataAccess.Concrete.EFCore;
using Shopapp.WebUI.Identity;
using Shopapp.WebUI.Middlewares;
using Shopapp.WebUI.EmailServices;
using System;
using Shopapp.Business.Concrete;
using Shopapp.DataAccess.Concrete.EfCore;

namespace Shopapp.WebUI
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationIdentityDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            services.Configure<IdentityOptions>(options => {
                //password
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength= 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;


                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = false;//mail onay API hatalý ondan devre dýþý býraktým
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = "Shopapp.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };
            });
            services.AddScoped<IProductDal, EFCoreProductDal>();
            services.AddScoped<ICategoryDal, EFCoreCategoryDal>();
            services.AddScoped<ICartDal, EFCoreCartDal>();
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<ICartService, CartManager>();
            services.AddScoped<IOrderService, OrderManager>();
            services.AddScoped<IOrderDal, EfCoreOrderDal>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedDatabase.Seed();
            }
            app.UseStaticFiles();
            app.CustomStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
                
                endpoints.MapControllerRoute(
                    name : "adminProducts ", 
                    pattern: "admin/products/",
                    defaults: new { controller = "Admin", action ="ProductList" });
                
                endpoints.MapControllerRoute(
                    name : "orders ", 
                    pattern: "cart/orders/",
                    defaults: new { controller = "Cart", action ="GetOrders" });
                
                endpoints.MapControllerRoute(
                    name : "cart ", 
                    pattern: "cart",
                    defaults: new { controller = "Cart", action ="Index" });

                endpoints.MapControllerRoute(
                    name : "adminProducts ", 
                    pattern: "Admin/products/{id?}",
                    defaults: new { controller = "Admin", action ="EditProduct" });

                endpoints.MapControllerRoute(
                    name: "adminCategories ",
                    pattern: "Admin/EditCategory/{id?}",
                    defaults: new { controller = "Admin", action = "EditCategory" });

                endpoints.MapControllerRoute(
                    name: "products ",
                    pattern: "products/{category?}",
                    defaults: new { controller = "Shop", action = "List" });
            });
            SeedIdentity.Seed(userManager, roleManager, Configuration).Wait();
        }
    }
}
