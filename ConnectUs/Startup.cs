using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectUs.Domain.Entities;
using ConnectUs.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AutoMapper;
using ConnectUs.Domain.IRepositories;
using ConnectUs.Infrastructure.Repositories;
using ConnectUs.Domain.Helpers;
using ConnectUs.Domain.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using ConnectUs.Web.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Cors;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.HttpOverrides;

namespace ConnectUs
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

            services.AddCors();

            services.AddSwaggerGen();
            #region automapper

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(AutoMapperProfile));

            var mapperConfig = new MapperConfiguration(mc =>
            {

                mc.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            #endregion

            #region cookies configure
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.Always;
            });

            

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                //options.Cookie.Expiration = TimeSpan.FromMinutes(AuthOptions.LIFETIME);
                options.ExpireTimeSpan = TimeSpan.FromMinutes(AuthOptions.LIFETIME);
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.Cookie.SameSite = SameSiteMode.None;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.ISSUER,

                    ValidateAudience = true,
                    ValidAudience = AuthOptions.AUDIENCE,
                    ValidateLifetime = true,

                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            });

            #endregion
            services.AddDbContext<BaseDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            
            services.AddTransient<IAccountService, AccountService>();
            services.AddScoped<IMeetupService, MeetupService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMeetupAdminService, MeetupAdminService>();

            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<BaseDbContext>()
                    .AddDefaultTokenProviders();

            services.AddMemoryCache();
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            }); app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true) // allow any origin
               .AllowCredentials()
               ); //
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
            // custom jwt auth middleware


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            mapper.ConfigurationProvider.AssertConfigurationIsValid();

        }
    }
}
