using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vega.Controllers;
using Vega.Core;
using Vega.Core.Models;
using Vega.Persistence;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace Vega
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();
            
            Configuration = Configuration;
        }

        public IConfiguration Configuration { get; }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPhotoRepository, PhotoRepository>();

            services.Configure<PhotoSettings>(Configuration.GetSection("PhotoSettings"));

            services.AddScoped<IVehicleRepository, VehicleRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IPhotoService, PhotoService>();

            services.AddTransient<IPhotoStorage, FileSystemPhotoStorage>();

            services.AddCors
                (c => 
                c.AddPolicy(
                    "AllowOrigin", 
                    options => 
                        options.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()));

            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<VegaDbContext>(options => options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; database=vega; Trusted_Connection=True;"));

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.RequireAdminRole,
                    policy => policy.RequireClaim("https://vega.com/roles", "Admin"));
            });

            // 1. Add Authentication Services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://vegaclient.us.auth0.com/";
                options.Audience = "https://api.vega.com";
            });

            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
