using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;

namespace MusicStore.API.FunctionalTests
{
    public class AlbumControllerTests
    {
        [Fact]
        public async void Test_GetAlbums()
        {
            string retValue = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://musicstorestag.southeastasia.cloudapp.azure.com:81/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/album");
                if (response.IsSuccessStatusCode)
                {
                    retValue = await response.Content.ReadAsStringAsync();
                }
            }
            Assert.NotEmpty(retValue);
            Assert.Contains("The Best Of The Men At Work", retValue);
        }

    }
}
