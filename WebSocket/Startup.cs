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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
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
            //app.Use(async (context, next) =>
            //{
            //    if (context.WebSockets.IsWebSocketRequest)
            //    {
            //        //此处进行升级协议。并建立链接。返回代表链接的套接字。
            //        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            //        Common.webSocketsList.Add(webSocket);
            //        //以下开始侦听并回显接收到的数据
            //        await Echo(context, webSocket, loggerFactory.CreateLogger("Echo"));
            //    }
            //    else
            //    {
            //        await next();
            //    }
            //});

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private async Task Echo(HttpContext context, System.Net.WebSockets.WebSocket webSocket, ILogger logger)
        {
            //缓冲区
            var buffer = new byte[1024 * 4];
            //使用WebSocket接收信息。
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            LogFrame(logger, result, buffer);
            //如果消息不是关闭消息则进行循环侦听。
            while (!result.CloseStatus.HasValue)
            {
                // If the client send "ServerClose", then they want a server-originated close to occur
                string content = "<<binary>>";
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    content = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    if (content.Equals("ServerClose"))
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing from Server", CancellationToken.None);
                        logger.LogDebug($"Sent Frame Close: {WebSocketCloseStatus.NormalClosure} Closing from Server");
                        return;
                    }
                    else if (content.Equals("ServerAbort"))
                    {
                        context.Abort();
                    }
                }
                foreach (var item in Common.webSocketsList)
                {
                    if(item.State != WebSocketState.Open)
                    {
                        continue;
                    }
                    Console.WriteLine("Enter message");
                    var message = Console.ReadLine();
                    var bytes = Encoding.UTF8.GetBytes(message);
                    await item.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Count()), WebSocketMessageType.Text, true, CancellationToken.None);
                    await item.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                }

                //await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                logger.LogDebug($"Sent Frame {result.MessageType}: Len={result.Count}, Fin={result.EndOfMessage}: {content}");

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                LogFrame(logger, result, buffer);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        private void LogFrame(ILogger logger, WebSocketReceiveResult frame, byte[] buffer)
        {
            var close = frame.CloseStatus != null;
            string message;
            if (close)
            {
                message = $"Close: {frame.CloseStatus.Value} {frame.CloseStatusDescription}";
            }
            else
            {
                string content = "<<binary>>";
                if (frame.MessageType == WebSocketMessageType.Text)
                {
                    content = Encoding.UTF8.GetString(buffer, 0, frame.Count);
                }
                message = $"{frame.MessageType}: Len={frame.Count}, Fin={frame.EndOfMessage}: {content}";
            }
            logger.LogDebug("Received Frame " + message);
        }
    }
}
