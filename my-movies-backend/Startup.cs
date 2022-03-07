using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using my_movies_backend.Data;
using System;

namespace my_movies_backend
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // allow Cross-Origin Requests from origins listed in CORS_ORIGINS env variable
            string envCorsOrigins = Environment.GetEnvironmentVariable("CORS_ORIGINS");
            if (!String.IsNullOrEmpty(envCorsOrigins))
            {
                string[] origins = envCorsOrigins.Split(',');
                services.AddCors(options =>
                {
                    options.AddPolicy(name: MyAllowSpecificOrigins,
                        builder =>
                        {
                            builder.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
                        });
                });
            }

            // get database connection string from DB_CONNECTION_STRING env variable
            string envDbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (String.IsNullOrEmpty(envDbConnectionString))
            {
                envDbConnectionString = null;
            }

            // get database type from DB_TYPE env variable
            string envDbType = Environment.GetEnvironmentVariable("DB_TYPE");
            switch (envDbType)
            {
                case "MY_SQL": // use MySQL
                    services.AddDbContext<MoviesContext>(options => options.UseMySql(envDbConnectionString ?? Configuration.GetConnectionString("MySQLConnection")));
                    break;
                default: // use SQL Server
                    services.AddDbContext<MoviesContext>(options => options.UseSqlServer(envDbConnectionString ?? Configuration.GetConnectionString("DefaultConnection")));
                    break;
            }

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
