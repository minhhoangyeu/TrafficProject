using Hangfire;
using Hangfire.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Traffic.Api.Filters;
using Traffic.Api.Logger;
using Traffic.Application.AutoMapper;
using Traffic.Application.Implementation;
using Traffic.Application.Interfaces;
using System;
using System.IO.Compression;
using Traffic.Data;
using Traffic.Data.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Traffic.Api.Middlewares;
using FluentValidation.AspNetCore;
using Traffic.Application.Models.User;
using FluentValidation;
using Traffic.Utilities.Helpers;
using Microsoft.AspNetCore.Http;

namespace Traffic
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
            EnviromentConfig.Config(Configuration);

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TrafficContext>(options =>
            {
                options.UseSqlServer(connectionString,
                            x => x.MigrationsAssembly("Traffic.Data"));
            });
            // Config CORS
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            // Set default request culture
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-US");
            });

            // Register DI
            services.AddTransient<DbInitializer>();
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICampaignHistoryService, CampaignHistoryService>();
            services.AddTransient<ICampaignService, CampaignService>();
            services.AddTransient<IUserCampaignConfigService, UserCampaignConfigService>();
            services.AddTransient<IUserCampaignService, UserCampaignService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddTransient<IFileStorageService, FileStorageService>();
            services.AddHttpContextAccessor();
            services.AddControllers()
              .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());

            #region jwt
            // Configure Authentication
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JwtConfig:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["JwtConfig:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtConfig:SecretKey"]))
                };
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion

            // AutoMapper
            services.AddSingleton(AutoMapperConfig.RegisterMappings().CreateMapper());

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Traffic API", Version = "v2" });
                c.EnableAnnotations();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Access Token Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddResponseCompression();
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(CustomExceptionFilter));
            });

            // Logger
            LogProvider.SetCurrentLogProvider(new NoLoggingProvider());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Traffic API v1"));

            app.UseCors("CorsPolicy");

            app.UseResponseCompression();

            app.UseRequestLocalization();
            // custom jwt auth middleware
            //app.UseMiddleware<JwtMiddleware>();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
