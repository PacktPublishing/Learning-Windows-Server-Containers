using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MusicStore.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace MusicStore.API.Controllers
{
    [Route("api/[controller]")]
    public class AlbumController : Controller
    {
        IDistributedCache _cache;
        MusicStoreContext _dbContext;
        private readonly AppSettings _appSettings;

        public AlbumController(
            IOptions<AppSettings> options,
            [FromServices] IDistributedCache cache,
            [FromServices] MusicStoreContext dbContext
            )
        {
            _appSettings = options.Value;
            _cache = cache;
            _dbContext = dbContext;
        }

        // GET api/albums
        [HttpGet]
        public async Task<List<Album>> Get()
        {
            var cacheKey = "topselling";
            List<Album> albums = null;
            var cachedalbums = _cache.Get(cacheKey);
            if (cachedalbums == null || cachedalbums.Length == 0)
            {
                //Get albums from database
                albums = await GetTopSellingAlbumsAsync(_dbContext, 10);
                if (albums != null && albums.Count > 0)
                {
                    if (_appSettings.CacheDbResults)
                    {
                        // Refresh it every 10 minutes.
                        _cache.Set(
                            cacheKey,
                            Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(albums, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })),
                            new DistributedCacheEntryOptions()
                            {
                                AbsoluteExpiration =
                            DateTimeOffset.UtcNow.AddMinutes(10)
                            }
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
            return albums;
        }

        [HttpGet("{id:int}")]
        public async Task<Album> GetById(int id)
        {
            var cacheKey = string.Format("album_{0}", id);
            Album album = null;
            var cachedAlbum = _cache.Get(cacheKey);
            if (cachedAlbum == null || cachedAlbum.Length == 0)
            {
                album = await _dbContext.Albums
                                .Where(a => a.AlbumId == id)
                                .Include(a => a.Artist)
                                .Include(a => a.Genre)
                                .FirstOrDefaultAsync();

                if (album != null)
                {
                    if (_appSettings.CacheDbResults)
                    {
                        //Remove it from cache if not retrieved in last 10 minutes
                        _cache.Set(
                            cacheKey,
                             Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(album, new JsonSerializerSettings()
                             {
                                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                             })),
                            new DistributedCacheEntryOptions()
                            {
                                AbsoluteExpiration =
                            DateTimeOffset.UtcNow.AddMinutes(10)
                            });
                    }
                }
            }
            else
            {
                album = JsonConvert.
                    DeserializeObject<Album>(System.Text.Encoding.UTF8.GetString(cachedAlbum));
            }
            return album;
        }

        private async Task<List<Album>> GetTopSellingAlbumsAsync(MusicStoreContext dbContext, int count)
        {
            // Group the order details by album and return
            // the albums with the highest count
            return await dbContext.Albums
                .OrderByDescending(a => a.OrderDetails.Count)
                .Take(count)
                .ToListAsync();
        }
    }
}
