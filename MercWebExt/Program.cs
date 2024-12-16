using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using MercWebExt.Helpers;
using MercWebExt.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SpaServices.Extensions;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using MercWebExt.Models.DataBase;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MercWebExt.Data.Services;

namespace MercWebExt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuration
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(connectionString));

            // Add HttpContextAccessor
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add services
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("_myAllowSpecificOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                         .AllowAnyHeader()
                         .AllowAnyMethod();
                });
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            builder.Services.AddScoped<IContextService, ContextService>();
            builder.Services.AddScoped<PdfService>();


            var app = builder.Build();

            // Request pipeline configuration
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseCors("_myAllowSpecificOrigins");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp"; // Adjust this path according to your project structure
                if (app.Environment.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start"); // Use the appropriate npm script for your SPA
                }
            });

            app.Run();
        }
    }
}
