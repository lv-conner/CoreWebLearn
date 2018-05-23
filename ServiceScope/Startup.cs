using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WcfService;

namespace ServiceScope
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<INameService, NameServiceClient>();
            services.AddTransient<ITransient, Transient>();
            services.AddScoped<IScope, Scope>();
            services.AddSingleton<ISingleton, Singleton>();

            var service = services.BuildServiceProvider().GetService<INameService>();
            var hello = service.HelloAsync("tim").GetAwaiter().GetResult();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseScopeTest();
            app.UseScopeMiddleware();
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
