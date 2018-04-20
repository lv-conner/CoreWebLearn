using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using WebSocketLearn.Services;

/// <summary>
/// 回显中间件，将使用WebSocket发送的消息加上Ok后回发到客户端。
/// </summary>
namespace WebSocketLearn.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class EchoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger logger;
        public EchoMiddleware(RequestDelegate next,ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<EchoMiddleware>();
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IHostingEnvironment hostingEnvironment, IWebSocketEncoding socketEncoding)
        {
            var fe = httpContext.Features.Get<IHttpUpgradeFeature>();
            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                //升级协议，建立链接。返回代表此链接的websocket对象。
                var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
                var buffer = new byte[4*1024];
                List<byte> msgArr = new List<byte>();
                //开始侦听socket，获取消息。此时可以向客户端发送消息。
                //接收到消息后，消息将传入到buffer中。
                //WebSocketResult 消息接收结果。
                //result.CloseStatus 关闭状态
                //result.CloseStatusDescription 关闭状态描述
                //result.Count 接收的消息长度
                //result.EndOfMessage 是否已经接受完毕
                //result.MessageType 消息类型 Text(文字，默认UTF-8编码),Binary(二进制),Close(关闭消息);
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                //当关闭状态没有值时：
                while(!result.CloseStatus.HasValue)
                {
                    msgArr.Clear();
                    //循环接收消息。
                    while (!result.EndOfMessage)
                    {
                        msgArr.AddRange(new ArraySegment<byte>(buffer, 0, result.Count));
                        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    }
                    msgArr.AddRange(new ArraySegment<byte>(buffer, 0, result.Count));
                    switch (result.MessageType)
                    {
                        case System.Net.WebSockets.WebSocketMessageType.Binary:
                            try
                            {
                                var fileName = Path.Combine(hostingEnvironment.WebRootPath, Guid.NewGuid().ToString() + ".png");
                                using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
                                {
                                    await fileStream.WriteAsync(msgArr.ToArray(), 0, msgArr.Count());
                                }
                            }
                            catch(FileNotFoundException fileNotFound)
                            {
                                await webSocket.SendAsync(new ArraySegment<byte>(socketEncoding.Encode(fileNotFound.Message)), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                            catch(InvalidOperationException inEx)
                            {
                                await webSocket.SendAsync(new ArraySegment<byte>(socketEncoding.Encode(inEx.Message)), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                            catch(Exception e)
                            {
                                await webSocket.SendAsync(new ArraySegment<byte>(socketEncoding.Encode(e.Message)), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                            break;
                        case System.Net.WebSockets.WebSocketMessageType.Close:
                            //关闭链接。
                            await webSocket.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "客户端关闭", CancellationToken.None);
                            break;
                        case System.Net.WebSockets.WebSocketMessageType.Text:
                            //获取客户端发送的消息。消息编码，采用UTF8
                            var message = socketEncoding.Decode(msgArr.ToArray());
                            logger.LogInformation($"{ "Received message is \t" + message }");
                            var res =socketEncoding.Encode("OK \t" + message);
                            //回显数据
                            await webSocket.SendAsync(new ArraySegment<byte>(res,0,res.Count()), System.Net.WebSockets.WebSocketMessageType.Text, result.EndOfMessage, CancellationToken.None);
                            break;
                        default:
                            break;
                    }
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer),CancellationToken.None);
                }

            }
            else
            {
                await _next(httpContext);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class EchoMiddlewareExtensions
    {
        public static IApplicationBuilder UseEchoMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EchoMiddleware>();
        }
    }
}
