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

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppSettings _appSettings;
        private IHostingEnvironment _environment;

        public HomeController(IOptions<AppSettings> options, IHostingEnvironment environment)
        {
            _appSettings = options.Value;
            _environment = environment;
        }

        //
        // GET: /Home/
        public async Task<IActionResult> Index(
            [FromServices] MusicStoreContext dbContext,
            [FromServices] IDistributedCache cache)
        {
            // Get most popular albums
            var cacheKey = "topselling";
            List<Album> albums = null;
            //try get top selling albums from cache
            var cachedalbums = cache.Get(cacheKey);
            if (cachedalbums == null)
            {
                //Get albums from database
                albums = await GetTopSellingAlbumsAsync(dbContext, 6);
                if (albums != null && albums.Count > 0)
                {
                    if (_appSettings.CacheDbResults)
                    {
                        // Refresh it every 10 minutes.
                        cache.Set(
                            cacheKey,
                            Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(albums)),
                            new DistributedCacheEntryOptions() { AbsoluteExpiration = 
                            DateTimeOffset.UtcNow.AddMinutes(10) }
                        );
                    }
                }
            }
            else
            {
                //deserialize cached albums
                albums = JsonConvert.
                    DeserializeObject<List<Album>>(System.Text.Encoding.UTF8.GetString(cachedalbums));
            }
            return View(albums);
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

        private Task<List<Album>> GetTopSellingAlbumsAsync(MusicStoreContext dbContext, int count)
        {
            // Group the order details by album and return
            // the albums with the highest count

            return dbContext.Albums
                .OrderByDescending(a => a.OrderDetails.Count)
                .Take(count)
                .ToListAsync();
        }
    }
}