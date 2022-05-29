using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quiz_Application.Services;
using Quiz_Application.Services.Repository;
using Quiz_Application.Web.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using DinkToPdf.Contracts;
using DinkToPdf;
using Quiz_Application.Services.Entities;
using Microsoft.AspNetCore.Identity;
using Business.Mapping;
using Quiz_Application.Services.Utilities.Identity;

namespace Quiz_Application.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("QuizDBConnection");
            services.AddServices();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddDbContext<QuizDBContext>(options => options.UseSqlServer(connection));
            services.AddIdentity<Candidate, IdentityRole>().AddEntityFrameworkStores<QuizDBContext>().AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".Vacation.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };
            });
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession();
            services.AddSingleton<BasicAuthentication>();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<Candidate> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {

                endpoints.MapAreaControllerRoute(
                   name: "Admin",
                   areaName: "admin",
                   pattern: "admin/{controller=Home}/{action=Index}"
           );

                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area:exists}/{controller}/{action}"
            );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Auth}/{action=Login}/{id?}");
            });
            SeedIdentity.Seed(userManager, roleManager, Configuration).Wait();
        }
    }
}
