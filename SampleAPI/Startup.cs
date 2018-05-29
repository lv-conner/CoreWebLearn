using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace SampleAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "SampleAPI", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "SampleAPI.XML");
                options.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 配置中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });

            //// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            //// specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    //c.RoutePrefix = "swagger/ui";
            //    c.SwaggerEndpoint("v1/swagger.json", "SampleAPI");
            //});
            app.UseBeforeMiddleware();
            app.UseMvc();
            app.UseAfterMiddleware();
        }
    }
}
