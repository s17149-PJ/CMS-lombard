using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace Lombard_00
{
    public class Startup
    {
        private void TestingStuff()
        {
            //restart db if you want / need
            if (IDb.DbInstance.TUsers.Count == 0)
            {
                IDb.DbInstance.AddTRole(new TRole()
                {
                    Id = 1,
                    Name = "Admin"
                });
                IDb.DbInstance.AddTRole(new TRole()
                {
                    Id = 2,
                    Name = "User"
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
                IDb.DbInstance.AddTUser(new TUser()
                {
                    Id = 2,
                    Nick = "user",
                    Name = "Not",
                    Surname = "Hacker",
                    Password = "12345"
                });

                for (int i = 2; i < 20; i++)
                {
                    IDb.DbInstance.AddTUser(new TUser()
                    {
                        Id = i,
                        Nick = GetNewToken(),
                        Name = GetNewToken(),
                        Surname = GetNewToken(),
                        Password = GetNewToken()
                    });
                }

                IDb.DbInstance.AddTItem(new TItem()
                {
                    Name = "test0",
                    Description = "test0",
                    ImageMetaData = "test",
                    FinallizationDateTime = DateTime.Now,
                    StartingBid = new TUserItemBid()
                    {
                        CreatedOn = DateTime.Now,
                        User = IDb.DbInstance.TUsers.ToArray()[0]
                    }
                });//pending
                IDb.DbInstance.AddTItem(new TItem()
                {
                    Name = "test1",
                    Description = "test1",
                    ImageMetaData = "test",
                    FinallizationDateTime = DateTime.Now.AddDays(-1),
                    StartingBid = new TUserItemBid()
                    {
                        CreatedOn = DateTime.Now.AddDays(-1),
                        User = IDb.DbInstance.TUsers.ToArray()[0]
                    }
                });//broken
                IDb.DbInstance.AddTItem(new TItem()
                {
                    Name = "test2",
                    Description = "test2",
                    ImageMetaData = "test",
                    FinallizationDateTime = DateTime.Now.AddDays(-2),
                    StartingBid = new TUserItemBid()
                    {
                        CreatedOn = DateTime.Now.AddDays(-2),
                        User = IDb.DbInstance.TUsers.ToArray()[0],
                    },
                    WinningBid = new TUserItemBid()
                    {
                        CreatedOn = DateTime.Now.AddDays(-1),
                        User = IDb.DbInstance.TUsers.ToArray()[1],
                        Money = 100
                    }
                });//done

            }

#pragma warning disable CS0219 // The variable 'x' is assigned but its value is never used
            int x = 2;
#pragma warning restore CS0219 // The variable 'x' is assigned but its value is never used
            //tests
            {

            }
            var list = IDb.DbInstance.TItems;

            x = 2;
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
