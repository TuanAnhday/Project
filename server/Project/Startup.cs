using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Project.Domain.Contracts.Responsitoties;
using Project.Domain.Settings;
using Project.Infrastructure.Data;
using Project.Infrastructure.Data.Models;
using Project.Infrastructure.Data.Repositories;
using Project.Infrastructure.Utils.Helpers;
using Project.Utils;
using Project.Utils.Filters;

namespace Project
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get;}

        public void ConfigureServices(IServiceCollection services)
        {
            var coreConnectionString = Configuration.GetConnectionString("Project");
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddDbContext<ProjectDbContext>(options =>
            {
                options.UseMySql(
                    coreConnectionString,
                    ServerVersion.AutoDetect(coreConnectionString)
                    );
            });
            services.AddIdentityCore<UseDataModel>(options =>
            {
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
            })
                .AddEntityFrameworkStores<ProjectDbContext>()
                .AddDefaultTokenProviders();

            services.AddControllers(options => { options.Filters.Add<HttpResponseExceptionFilter>(); });
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Project", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                });
            });
            #region Service Injection

            #region IRepositories
            services.AddScoped<IUserRepository, UserRepository>();
            #endregion

            services.AddScoped<Authenticator>();

            services.AddHttpContextAccessor();
            #endregion

            services.AddSwaggerGenNewtonsoftSupport();
            services.AddAutoMapper(c => c.AddProfile<MappingProfile>());
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseCors(builder => builder
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithOrigins(Configuration.GetSection("AllowedOrigins").Value.Split(","))
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        );
        }

    }
}
