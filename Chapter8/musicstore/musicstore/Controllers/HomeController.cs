using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MusicStore.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppSettings _appSettings;
        private IHostingEnvironment _environment;
        private HttpClient _client;

        public HomeController(IOptions<AppSettings> options, IHostingEnvironment environment, HttpClient httpClient)
        {
            _appSettings = options.Value;
            _environment = environment;
            _client = httpClient;
        }

        //
        // GET: /Index/
        public async Task<IActionResult> Index()
        {
            // Get most popular albums
            IEnumerable<Album> albums = await _client.GetAsync("/api/album").Result
                                            .Content.ReadAsAsync<IEnumerable<Album>>();
            //try get top selling albums from API
            return View(albums);
        }

        // GET : /Details
        public async Task<IActionResult> Details(
           [FromServices] IMemoryCache cache,
           int id)
        {
            Album album = await _client.GetAsync(string.Format("/api/album/{0}", id)).Result
                                 .Content.ReadAsAsync<Album>();
            if (album == null)
            {
                return NotFound();
            }
            return View(album);
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }

        public IActionResult StatusCodePage()
        {
            return View("~/Views/Shared/StatusCodePage.cshtml");
        }

        public IActionResult AccessDenied()
        {
            return View("~/Views/Shared/AccessDenied.cshtml");
        }

    }
}