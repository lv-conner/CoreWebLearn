using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Features;
using WebSocketLearn.Middleware;
using WebSocketLearn.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.FileProviders;

namespace WebSocketLearn
{
    public class Common
    {
        public static List<System.Net.WebSockets.WebSocket> webSocketsList = new List<System.Net.WebSockets.WebSocket>();
    }
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
            services.AddMvc();
            services.AddAntiforgery();
            services.AddSingleton<IWebSocketEncoding, WebSocketEncoding>();
            services.Configure<WebSocketOptions>(Configuration.GetSection("WebSocketOptions"));
            
            //Timespan时间格式：0.00：00：00从string转换。
            var options = services.BuildServiceProvider().GetService<IOptions<WebSocketOptions>>();
            IFileProvider fileProvider = services.BuildServiceProvider().GetService<IFileProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //FileServerOptions options = new FileServerOptions()
            //{
            //    EnableDirectoryBrowsing = true,
            //};
            //app.UseFileServer(options);
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseWebSockets();
            app.UseEchoMiddleware();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.Run(async context =>
           {
               //直接输出中文会导致乱码，因为直接直接回应文字，最后将转换成html,但是由于相应报头中没有Content-type，因此浏览器不能正确识别字符类型，导致中文显示乱码。
               context.Response.Headers["Content-Type"] = "text/html; charset=utf-8";
               await context.Response.WriteAsync("错误");
           });
        }
    }
}
