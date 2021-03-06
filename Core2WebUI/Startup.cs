﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Core2WebUI.Entities.Identity;
using Core2WebUI.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Core2WebUI.Core.Culture;
using Core2WebUI.Core.Utills;
using Core2WebUI.Core.Hmac;
using Core2WebUI.Core.RabbitMQ;
using Core2WebUI.Entities.Log;
using Core2WebUI.Middlewares.Exceptions;
using Core2WebUI.Middlewares;
using System.IO;
using System.Net;
using Core2WebUI.Core.Exceptions.Custom;

namespace Core3WebUI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            /*services.Configure<RequestLocalizationOptions>(options =>
           {
               CultureInfo[] supportedCultures = new[]
                   {
                   new CultureInfo("en-us"),
                   new CultureInfo("en-AU"),
                   new CultureInfo("en-GB"),
                   new CultureInfo("en"),
                   new CultureInfo("es-ES"),
                   new CultureInfo("es-MX"),
                   new CultureInfo("es"),
                   new CultureInfo("fr-fr"),
                   new CultureInfo("fr"),
                   new CultureInfo("tr"),
               };

               options.DefaultRequestCulture = new RequestCulture("es-ES");
               options.SupportedCultures = supportedCultures;
               options.SupportedUICultures = supportedCultures;
               options.RequestCultureProviders = new List<IRequestCultureProvider>
                   {
                       new RouteDataRequestCultureProvider(),
                       new QueryStringRequestCultureProvider(),
                       new CookieRequestCultureProvider()
                   };
           });*/
            //services.AddLocalization();

            try
            {
                //Identity dccontext ayarları(postgreSQL için ayarlanıyor)
                services.AddDbContext<CustomIdentityDbContext>(options =>
                             options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
                );
            }
            catch(Exception ex)
            {
                throw new IdentityManagerException(Convert.ToInt32(HttpStatusCode.Unauthorized), ex);
            }


            try {
                // Identity ayarları
                services.AddIdentity<CustomIdentityUser, CustomIdentityRole>()
                        .AddEntityFrameworkStores<CustomIdentityDbContext>()
                        .AddDefaultTokenProviders();
            }
            catch (Exception ex)
            {
                throw new IdentityManagerException(Convert.ToInt32(HttpStatusCode.Unauthorized), ex);
            }
            

            //HTTP Cookie ayarları
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Acc/Login";
                options.LogoutPath = "/Acc/Logout";
                options.AccessDeniedPath = "/Acc/AccesDenied";
                /*options.Cookie = new CookieBuilder()
                {
                    HttpOnly = true,
                    Name = ".Miya.Security.Cookie",
                    Path = "/",
                    SameSite = SameSiteMode.Lax,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest,
                    Expiration = TimeSpan.FromMinutes(2)
                };*/
            });

            try
            {
                // redis ayarları
                services.AddDistributedRedisCache(options =>
                {
                    options.InstanceName = Configuration.GetConnectionString("RedisInstanceName");
                    options.Configuration = Configuration.GetConnectionString("RedisServer");
                }
                );
            } catch(Exception ex)
            {
                throw new IdentityManagerException(Convert.ToInt32(HttpStatusCode.Unauthorized), ex);
            }
            
            
            // session ayarları
            services.AddSession(options =>
            {
               
               options.IdleTimeout = TimeSpan.FromMinutes(20);
               options.Cookie = new CookieBuilder
                {
                   
                    //Expiration = TimeSpan.FromMinutes(2),
                    HttpOnly = true,
                    //Domain = "http://localhost/9082/",
                    SecurePolicy = CookieSecurePolicy.SameAsRequest,
                    SameSite = SameSiteMode.Lax,
                    Name = ".Miya.Security.Cookie",
                    Path = "/",
                    
                };
            });

            // Add detection services container and device resolver service.
            services.AddDetection()
                    .AddDevice();

            //culture localizer ayarları
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();

            services.AddScoped<AjaxSessionTimeOutAttribute>();
            services.AddScoped<HmacTokenGeneratorAttribute>();
            services.AddScoped<PageEntryLogRabbitMQAttribute>();

            //services.AddScoped<ISessionService, SessionService>();
            //services.AddSingleton<HmacServiceManagerBase, HmacServiceManager>();
            services.AddTransient<RemoteAddressFinder, RemoteAddressFinder>();
            services.AddTransient<HmacServiceManagerBase, HmacServiceManager>();
            services.AddTransient<PageEntryLogPublisher, PageEntryLogPublisher>();
            services.AddSingleton<PageAccessLogModel, PageAccessLogModel>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpStatusCodeExceptionMiddleware();
                app.UseExceptionHandlingMiddleware();
            } else
            {
                app.UseHttpStatusCodeExceptionMiddleware();
                app.UseExceptionHandlingMiddleware();
            }

            // session
            app.UseSession();

            try {
                // Identity
                app.UseAuthentication();
            } catch(Exception ex)
            {
                throw new IdentityManagerException(Convert.ToInt32(HttpStatusCode.Unauthorized), ex);
            }


            // route globalizasyon cultureInfo ayarlar
            app.UseRouter(routes =>
            {
                routes.MapMiddlewareRoute("{culture?}/{*mvcRoute}", _app =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("es"),
                        new CultureInfo("en-US"),
                        new CultureInfo("en-AU"),
                        new CultureInfo("en-GB"),
                        new CultureInfo("en"),
                        new CultureInfo("es-ES"),
                        new CultureInfo("es-MX"),
                        new CultureInfo("fr-FR"),
                        new CultureInfo("fr"),
                        new CultureInfo("tr-TR"),
                        //new CultureInfo("ru-RU"),
                    };

                    var requestLocalizationOptions = new RequestLocalizationOptions
                    {
                        DefaultRequestCulture = new RequestCulture("es-MX"),
                        SupportedCultures = supportedCultures,
                        SupportedUICultures = supportedCultures
                    };
                    requestLocalizationOptions.RequestCultureProviders = new List<IRequestCultureProvider>
                    {
                        new RouteDataRequestCultureProvider(),
                        new QueryStringRequestCultureProvider(),
                        new CookieRequestCultureProvider(),
                    };

                    /*requestLocalizationOptions.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
                    requestLocalizationOptions.RequestCultureProviders.Insert(1, new RouteDataRequestCultureProvider());
                    requestLocalizationOptions.RequestCultureProviders.Insert(2, new CookieRequestCultureProvider());*/
                    _app.UseRequestLocalization(requestLocalizationOptions);

                    _app.UseMvc(mvcRoutes =>
                    {
                        mvcRoutes.MapRoute(
                            name: "default",
                            //template: "{culture=tr-TR}/{controller=Home}/{action=Index}/{id?}");
                            template: "{culture?}/{controller=Home}/{action=Index}/{id?}");
                        mvcRoutes.MapRoute(
                        name: "default_route",
                        template: "{controller}/{action}/{id?}",
                        defaults: new { controller = "Home", action = "Index" });
                        
                    });
                });

            });

            //var localOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            //app.UseRequestLocalization(localOptions.Value);

           /* app.UseMvc(routes=> {
                routes.MapRoute(
                    name: "default",
                    template: "{culture?}/{controller=home}/{action=index}/{id?}");
                });*/
            
        }
    }
}
