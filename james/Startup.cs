using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using james.Formatters;
using james.Db;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using james.Repository;

namespace james
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

            services.AddDbContext<ColorsContext>(opt =>
                opt.UseMySql("Server=localhost;User=colors;Password=colors;Database=colors",
                   mysqlOptions =>
                         {
                             mysqlOptions.ServerVersion(new Version(5, 7, 21), ServerType.MySql);
                             mysqlOptions.EnableRetryOnFailure(
                                 maxRetryCount: 10,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null
                             );
                         }
                   ));

            services.AddMvc(
                options =>
                {
                    options.InputFormatters.Clear();
                    options.OutputFormatters.Clear();
                    // Definim quins són els formatters a fer servir
                    options.InputFormatters.Add(new BondCompactBinaryInputFormatter());
                    options.OutputFormatters.Add(new BondCompactBinaryOutputFormatter());
                }
            ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<IColorsRepository, ColorsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
