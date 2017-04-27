//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;

//namespace MusicStore.Models
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class HttpClientContext
//    {
//        public static HttpClient client = new HttpClient();

//        /// <summary>
//        /// 
//        /// </summary>
//        public HttpClientContext()
//        {
//            client.BaseAddress = new Uri(Microsoft.Extensions.Configuration);
//            client.DefaultRequestHeaders.Accept.Clear();
//            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//        }
//    }
//}
