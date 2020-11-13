using Lombard_00.Controllers;
using Lombard_00.Controllers.Tranzit;
using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace Lombard_00
{
    public class Startup
    {
        private void TestingStuff()
        {
            //restart db if you want / need
            if (true)
            {
                IDb.DbInstance.VoidOut();
                IDb.DbInstance.AddTRole(new TRole()
                {
                    Id = 1,
                    Name = "Admin"
                });
                IDb.DbInstance.AddTUser(new TUser()
                {
                    Id = 1,
                    Nick = "totaly admin",
                    Name = "Totaly",
                    Surname = "Admin",
                    Password = "AdminGodDamit",
                    ValidUnitl = DateTime.Now.AddDays(1),
                    Token = "0"
                });
                IDb.DbInstance.AddTUserRole(new TUserRole()
                {
                    User = IDb.DbInstance.TUsers[1],//admin
                    Role = IDb.DbInstance.TRoles[1]//admin
                });

                IDb.DbInstance.AddTRole(new TRole()
                {
                    Id = 2,
                    Name = "User"
                });

                IDb.DbInstance.AddTUser(new TUser()
                {
                    Id = 2,
                    Nick = "user",
                    Name = "Not",
                    Surname = "Hacker",
                    Password = "12345"
                });
                IDb.DbInstance.AddTUserRole(new TUserRole()
                {
                    User = IDb.DbInstance.TUsers[2],//user
                    Role = IDb.DbInstance.TRoles[2]//user
                });

                for (int i = 3; i < 20; i++)
                {
                    IDb.DbInstance.AddTUser(new TUser()
                    {
                        Id = i,
                        Nick = GetNewToken(),
                        Name = GetNewToken(),
                        Surname = GetNewToken(),
                        Password = GetNewToken()
                    });
                    IDb.DbInstance.AddTUserRole(new TUserRole()
                    {
                        User = IDb.DbInstance.TUsers[i],//user
                        Role = IDb.DbInstance.TRoles[2]//user
                    });
                }

                var citem = new CItem();

                citem.ItemAdd(1,"0",new TokenItem() { 
                    Name = "test item 0",
                    Description = "yes",
                });
                citem.ItemAdd(1, "0", new TokenItem()
                {
                    Name = "test item 1",
                    Description = "yes",
                });
                citem.ItemAdd(1, "0", new TokenItem()
                {
                    Name = "test item 2",
                    Description = "yes",
                });

                //tests
                {
                    //citem.ItemDelete(0, "0", new TokenItem() { Id = 0 });


                }
            }
        }
        private string GetNewToken()
        {
            var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var resultToken = new string(
               Enumerable.Repeat(allChar, 6)
               .Select(token => token[random.Next(token.Length)]).ToArray());

            return resultToken.ToString();
        }//done


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
