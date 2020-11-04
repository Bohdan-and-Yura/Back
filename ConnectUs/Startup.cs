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

            // var appSettingsSection = Configuration.GetSection("AppSettings");
            //services.Configure<AuthOptions>(appSettingsSection);
            AuthOptions authOptions = new AuthOptions();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    //ValidateIssuer = false,
                    ValidateAudience = false,
                    //ValidateLifetime = false,
                };
            });

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IMeetupService, MeetupService>();
            services.AddScoped<IUserService, UserService>();

            services.AddDbContext<BaseDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddIdentity<User, IdentityRole>()
                   .AddEntityFrameworkStores<BaseDbContext>()
                    .AddDefaultTokenProviders();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddMemoryCache();
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

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

            // custom jwt auth middleware


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            mapper.ConfigurationProvider.AssertConfigurationIsValid();

        }
    }
}
