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
using Microsoft.Extensions.Caching.Redis;

namespace WebSocketLearn
{   
    public class Common
    {
        public static List<System.Net.WebSockets.WebSocket> webSocketsList = new List<System.Net.WebSockets.WebSocket>();
    }
    public class Startup
    {
        private readonly IHostingEnvironment env;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.env = env;
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
            services.AddSession();


            //内存缓存，仅限与本机。内部使用IMemoryCache 实现。
            services.AddDistributedMemoryCache();
            //分布式redis缓存；
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "localhost";
                //InstanceName 实例名称的设置将导致设置到redis中的每个条目的key值添加InstanceName的前缀。
                options.InstanceName = env.ApplicationName;
            });
            //使用Sql server作为分布式缓存，需要注意在项目文件中添加一下条目
            //  <DotNetCliToolReference Include="Microsoft.Extensions.Caching.SqlConfig.Tools" Version="2.0.0" />
            //默认使用Nuget package manager添加该引用生成的是
            //  <PackageReference Include="Microsoft.Extensions.Caching.SqlConfig.Tools" Version="2.0.0" />
            //作为包引用添加到项目引用是 dotnet 命令接口将不能检测其中添加的cli tool,因此执行dotnet sql-cache create命令会找不到该命令。
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = "Server=PRCNMG1L0311;initial catalog=CacheDb;user id=sa;password=Root@admin";
                options.SchemaName = "dbo";
                options.TableName = "SqlCacheTable";
            });
            //Timespan时间格式：0.00：00：00从string转换。
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
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
            app.UseSession();
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
