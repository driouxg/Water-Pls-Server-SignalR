using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRTest.DataAccess;
using SignalRTest.Hubs;
using System;
using System.Linq;
using SignalRTest.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using SignalRTest.Services;
using SignalRTest.Domain.Entity;
using SignalRTest.Services.Impl;
using Swashbuckle.AspNetCore.Swagger;

namespace SignalRTest
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public IHostingEnvironment Environment { get; set; }
        private readonly ILogger _logger;

        public Startup(IHostingEnvironment env, ILogger<Startup> logger)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            Environment = env;

            _logger = logger;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info { Title = "Water Pls", Version = "v1.0" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme() { In = "header", Description = "Please insert JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
            });
            // End Swagger

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR();

            // CORS
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://localhost:3000")
                    .AllowCredentials();
            }));


            services.AddDbContext<WaterDbContext>
                (options => options.UseSqlServer(Configuration["WATER_PLS_DB"]));

            services.AddSingleton<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            IdentityModelEventSource.ShowPII = true;
            services.AddAuthentication(options =>
            {
                // Identity made Cookie authentication the default.
                // However, we want JWT Bearer Auth to be the default.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Configure JWT Bearer Auth to expect our security key
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        LifetimeValidator = (before, expires, token, param) =>
                        {
                            return expires > DateTime.UtcNow;
                        },
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateActor = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["SIGNING_KEY"]))
                    };

                // We have to hook the OnMessageReceived event in order to
                // allow the JWT authentication handler to read the access
                // token from the query string when a WebSocket or 
                // Server-Sent Events request comes in.
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        string accessToken = context.Request.Query["access_token"];
                        _logger.LogInformation("access token is: " + accessToken);

                        string authToken = context.Request?.Headers?["Authorization"];
                        if (!string.IsNullOrEmpty(authToken) && authToken.StartsWith("Bearer "))
                        {
                            authToken = authToken.Substring(7);
                        }
                        _logger.LogInformation("auth token is: " + authToken);

                        // If the request is for one of our endpoints.
                        var path = context.HttpContext.Request.Path;

                        if ((!string.IsNullOrEmpty(accessToken) || !string.IsNullOrEmpty(authToken)) &&
                            ((path.StartsWithSegments("/requestWaterHub") ||
                            path.StartsWithSegments("/donateWaterHub") ||
                            path.StartsWithSegments("/api"))
                            ))
                        {
                            _logger.LogInformation("access token is null or empty: " + string.IsNullOrEmpty(accessToken));
                            context.Token = string.IsNullOrEmpty(accessToken) ? authToken : accessToken;
                            _logger.LogInformation("context token is: " + context.Token);
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // ASP.NET Core 2.2 Authorization and Identity
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Stores.MaxLengthForKeys = 128;
                })
                .AddEntityFrameworkStores<WaterDbContext>()
                .AddDefaultTokenProviders();

            // Change to use Name as the user identifier for SignalR
            // WARNING: This requires that the source of your JWT token 
            // ensures that the Name claim is unique!
            // If the Name claim isn't unique, users could receive messages 
            // intended for a different user!
            services.AddSingleton<IUserIdProvider, ApplicationUserIdProvider>();    // <----- This originally was services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
            services.AddScoped<ITokenManagerService, TokenManagerService>();

            services.Configure<IdentityOptions>(options =>
            {
                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;

                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });

            //            services.ConfigureApplicationCookie(options =>
            //            {
            //                // Cookie settings
            //                options.Cookie.HttpOnly = true;
            //                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            //                options.Cookie.Name = "MySuperDuperCookie";
            //
            //                options.LoginPath = "/api/Users/login";
            //                //options.AccessDeniedPath = "/api/not-found";
            //                options.SlidingExpiration = true;
            //            });
            // End of ASP.NET Core 2.2 Authorization and Identity

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, WaterDbContext dbContext, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            dbContext.Database.Migrate();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                //                app.UseSwaggerUI(c =>
                //                {
                //                    c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "API V1.0");
                //                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // ASP.NET Identity Authentication
            app.UseAuthentication();

            app.UseCookiePolicy();
            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<RequestWaterHub>("/requestWaterHub");
                routes.MapHub<DonateWaterHub>("/donateWaterHub");
            });

            var roleCreator = new RoleCreator(dbContext, userManager, roleManager, Configuration);
            roleCreator.Initialize().Wait();

            app.UseMvc();
        }
    }
}
