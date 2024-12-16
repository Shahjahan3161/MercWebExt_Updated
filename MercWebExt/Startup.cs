//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Threading.Tasks;
//using DinkToPdf;
//using DinkToPdf.Contracts;
//using MercWebExt.Helpers;
//using MercWebExt.Services;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.AspNetCore.SpaServices.Extensions;
//using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
//using MercWebExt.Models.DataBase;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection.Extensions;

//namespace MercWebExt
//{
//    public class Startup
//    {
//        public Startup(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//        public IConfiguration Configuration { get; }

//        public void ConfigureServices(IServiceCollection services)
//        {
//            var connectionString = Configuration.GetConnectionString("DefaultConnection");
//            services.AddDbContext<DatabaseContext>(options =>
//                options.UseSqlServer(connectionString));

//            //services.AddTransient<ContextService>();
//            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();


//            // services.AddAuthentication(options =>
//            // {
//            //     options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//            //     options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//            //     options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//            // })
//            //.AddCookie();

//            services.AddCors(options =>
//            {
//                options.AddPolicy(MyAllowSpecificOrigins,
//                builder =>
//                {
//                    builder.AllowAnyOrigin()
//                         .AllowAnyHeader()
//                         .AllowAnyMethod();
//                });
//            });

//            services.AddControllersWithViews(); // Updated from AddMvc() for better clarity and future compatibility
//            services.AddDistributedMemoryCache();
//            services.AddSession();

//            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
//            services.AddTransient<IEmailSender, EmailSender>();

//            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

//        }

//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) // Updated parameter type for consistency with newer versions
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//            }
//            else
//            {
//                app.UseExceptionHandler("/Home/Error");
//                app.UseHsts();
//            }

//            app.UseHttpsRedirection();
//            app.UseStaticFiles();
//            app.UseCookiePolicy();

//            app.UseRouting(); // Added to ensure proper routing setup

//            app.UseCors(MyAllowSpecificOrigins);

//            app.UseAuthentication(); // Moved here to follow UseRouting()

//            app.UseAuthorization(); // Added for completeness, typically follows UseAuthentication()

//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapControllerRoute(
//                    name: "default",
//                    pattern: "{controller=Home}/{action=Index}/{id?}");
//            });

//            // Configure SPA middleware
//            app.UseSpa(spa =>
//            {
//                spa.Options.SourcePath = "ClientApp"; // Adjust this path according to your project structure

//                if (env.IsDevelopment())
//                {
//                    spa.UseReactDevelopmentServer(npmScript: "start"); // Use the appropriate npm script for your SPA
//                }
//            });
//        }
//    }
//}
