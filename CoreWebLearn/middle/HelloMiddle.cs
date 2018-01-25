using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWebLearn
{
    public class HelloMiddle
    {
        public RequestDelegate _next;
        public string _content;
        public string _contentType;
        public IGetName _getName;
        public HelloMiddle(RequestDelegate next,IGetName getName,string content,string contentType)
        {
            _next = next;
            _getName = getName;
            _content = content;
            _contentType = contentType;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.ContentType = _contentType;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var byteContent = Encoding.GetEncoding("GB2312").GetBytes(_content + _getName.Name);
            await context.Response.Body.WriteAsync(byteContent, 0, byteContent.Length);
        }
    }
}
