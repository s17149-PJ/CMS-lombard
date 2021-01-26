using Lombard_00.Controllers.Tranzit;
using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
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
                IDb.DbInstance.AddTUser(new TUser()
                {
                    Id = 1,
                    Nick = "totaly admin",
                    Name = "Totaly",
                    Surname = "Admin",
                    Password = "AdminGodDamit",
                    ValidUnitl = DateTime.Now.AddDays(1),
                    Token = "0",
                    Roles = new List<TRole>(){
                        new TRole()
                        {
                            Id = 1,
                            Name = "Admin"
                        }
                    }
                });
                IDb.DbInstance.AddTUser(new TUser()
                {
                    Id = 2,
                    Nick = "user",
                    Name = "Not",
                    Surname = "Hacker",
                    Password = "12345",
                    Roles = new List<TRole>(){
                        new TRole()
                        {
                            Id = 2,
                            Name = "User"
                        }
                    }
                });

                for (int i = 2; i < 10; i++)
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

                var tagA = new TTag() { Name = "material" };
                var tagB = new TTag() { Name = "expensive" };
                IDb.DbInstance.AddTItem(new TItem()
                {
                    Name = "test0",
                    Description = "Nullam fringilla dignissim elementum. Pellentesque posuere ipsum metus. Nullam aliquet arcu eget luctus fermentum. Aenean sagittis consequat eros, et ullamcorper libero viverra nec. Vestibulum vel magna id mi pellentesque porttitor sed ac orci. Proin ut commodo ligula. Donec commodo, est hendrerit hendrerit cursus, neque purus ullamcorper tellus, quis rutrum ligula urna non lacus. Nulla at dictum dolor. Aenean non rutrum turpis. Maecenas ut erat metus. Sed eget egestas est, vel interdum enim. Aliquam nec eleifend dolor.",
                    ImageMetaData = "https://cutt.ly/ijDjtHh",
                    FinallizationDateTime = DateTime.Now.AddSeconds(86400),
                    Tags = new List<TTag>() { tagA, tagB }
                },
                new TUser() { Id = 1 }, 
                100);//pending

                IDb.DbInstance.AddTItem(new TItem()
                {
                    Name = "test1",
                    Description = "Morbi vel ornare quam. Duis sodales gravida elit, at bibendum dolor mollis eget. Pellentesque hendrerit, felis id faucibus consectetur, nibh dolor iaculis sem, quis luctus mauris urna eget tortor. Nullam mattis vestibulum risus quis tristique. Proin non justo tempus, viverra nisl nec, vulputate nunc. Quisque sit amet ligula at velit posuere bibendum a ac elit. Sed venenatis eros a erat eleifend faucibus at id nunc. Phasellus massa orci, malesuada sed lacinia in, pellentesque vel mi. Nunc id leo a lectus ultricies sollicitudin at vel nisi. Integer mauris dolor, mollis at mi nec, fringilla bibendum nisi. Praesent felis nisi, facilisis sit amet ex quis, finibus aliquam tellus. In nec erat tincidunt, mollis mi non, aliquet libero. Etiam ullamcorper faucibus tortor quis dignissim. Nulla commodo tempor enim at suscipit. Nullam sit amet est quis turpis malesuada pharetra.",
                    ImageMetaData = "https://cutt.ly/3jDjiHm",
                    FinallizationDateTime = DateTime.Now.AddSeconds(86400),
                    Tags = new List<TTag>() { tagA }
                },
                new TUser() { Id = 2 },
                100);//pending

                //IDb.DbInstance.AddTUserItemBid()
            }

            var db = IDb.DbInstance;

            //var usr = db.FindTUser(1);

            
            //var tmp = db.TItems.Select(e=> new TokenItem(e, db));
            var tt = 2;

            int x = 2;
            //tests
            //var users = IDb.DbInstance.TUsers;
            //var items = IDb.DbInstance.TItems;

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
