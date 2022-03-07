using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using my_movies_backend.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_movies_backend
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
            // get database connection string from DB_CONNECTION_STRING env variable
            String envDbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (String.IsNullOrEmpty(envDbConnectionString))
            {
                envDbConnectionString = null;
            }

            // get database type from DB_TYPE env variable
            String envDbType = Environment.GetEnvironmentVariable("DB_TYPE");
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
