using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace Lombard_00
{
    public class Startup
    {
        private void TestingStuff() {
            if (IDb.DbInstance.TUsers.Count == 0) {
                IDb.DbInstance.AddTUserRole(new TUserRole() { 
                    Id = 0,
                    Name = "Admin"
                });
                IDb.DbInstance.AddTUserRole(new TUserRole()
                {
                    Id = 1,
                    Name = "User"
                });

                IDb.DbInstance.AddTUser(new TUser() { 
                    Nick = "totaly admin",
                    Name = "Totaly",
                    Surname = "Admin",
                    Password = "AdminGodDamit",
                    Roles = new List<TUserRole>() { IDb.DbInstance.TUserRoles[0] }
                });
                IDb.DbInstance.AddTUser(new TUser()
                {
                    Nick = "user",
                    Name = "Not",
                    Surname = "Hacker",
                    Password = "12345",
                    Roles = new List<TUserRole>() { IDb.DbInstance.TUserRoles[1] }
                });
            }
            var value = IDb.DbInstance.TUsers;

            int x = 2 + 3;
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            TestingStuff();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
