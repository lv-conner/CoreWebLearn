using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;

namespace Basic
{
    public class PocketGo
    {
        //55221-b7c0099f2fffc528da4b254b
        public static async void Authentication()
        {
            var clientHandler = new CHandler();
            HttpClient client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri("https://getpocket.com/v3/oauth/request");
            var obj = new { consumer_key = "55221-b7c0099f2fffc528da4b254b", redirect_uri = "http://localhost:8569" };
            StringContent content = new StringContent(JsonConvert.SerializeObject(obj),Encoding.UTF8,"application/json");
            var parameters = new Dictionary<string, string>();
            parameters.Add("consumer_key", "55221-b7c0099f2fffc528da4b254b");
            parameters.Add("redirect_uri", "http://localhost:8569");
            var urlContent = new FormUrlEncodedContent(parameters);
            var response = await client.PostAsync("", content);
            //response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseText);
        }


        public class CHandler: HttpClientHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.TryAddWithoutValidation("X-Accept", "application/json");
                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}
