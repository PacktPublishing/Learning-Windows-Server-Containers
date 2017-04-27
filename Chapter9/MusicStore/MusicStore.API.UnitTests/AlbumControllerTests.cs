using Microsoft.Extensions.Options;
using Moq;
using MusicStore.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Caching.Distributed;
using MusicStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MusicStore.API.UnitTests
{
    public class AlbumControllerTests
    {
        private Mock<IOptions<AppSettings>> _options;
        private AlbumController _albumController;
        private Mock<IDistributedCache> _distributedCache;
        private MusicStoreContext _musicStoreContext;
        private AppSettings _appSettings;

        public AlbumControllerTests()
        {
            //Configuring In Memory Store for Unit Tests
            var optionsBuilder = new DbContextOptionsBuilder<MusicStoreContext>();
            optionsBuilder.UseInMemoryDatabase();
            
            //Create Data Context
            _musicStoreContext = new MusicStoreContext(optionsBuilder.Options);

            //Mocking dependencies
            _appSettings = new AppSettings() { CacheDbResults = true };

            _options = new Mock<IOptions<AppSettings>>();
            _options.SetupGet(p => p.Value).Returns(_appSettings);

            _distributedCache = new Mock<IDistributedCache>();
            _albumController = new AlbumController(_options.Object, _distributedCache.Object, _musicStoreContext);

        }

        [Fact]
        public void Test_GetAlbums()
        {
            //Arrange
            var albums = new List<Album>() { new Album
                {
                    Title = "The Best Of The Men At Work",
                    Genre = SampleData.Genres["Pop"],
                    Price = 8.99M,
                    Artist = SampleData.Artists["Men At Work"],
                    AlbumArtUrl = string.Empty
                }};
            _musicStoreContext.Albums.AddRange(albums);
            _musicStoreContext.SaveChanges();
            _distributedCache.Setup(c => c.Set("topselling",
                Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(albums, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                })),
                (new Mock<DistributedCacheEntryOptions>()).Object));

            //Act
            var topalbums = _albumController.Get();

            //Assert
            Assert.True(topalbums.Result.Count == 1);
        }

        [Fact]
        public void Test_GetById()
        {
            //Arrange
            var albums = new List<Album>() { new Album
                {
                    AlbumId = 2,
                    Title = "...And Justice For All",
                    Genre = SampleData.Genres["Metal"],
                    Price = 8.99M,
                    Artist = SampleData.Artists["Metallica"],
                    AlbumArtUrl = string.Empty
                }};
            //To Make sure same album is not added before
            _musicStoreContext.Albums.AddRange(albums);
            _musicStoreContext.SaveChanges();
            _distributedCache.Setup(c => c.Get("album_2")).Returns(new byte[] { });
            _distributedCache.Setup(c => c.Set("album_2",
                    Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(albums[0], new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    })),
                    (new Mock<DistributedCacheEntryOptions>()).Object));
            
            //Act
            var album = _albumController.GetById(2);

            //Assert
            Assert.True(album != null);
        }

        [Fact]
        public void Test_GetCachedAlbums()
        {
            //Arrange
            var albums = new List<Album>() { new Album
                {
                    Title = "The Best Of The Men At Work",
                    Genre = SampleData.Genres["Pop"],
                    Price = 8.99M,
                    Artist = SampleData.Artists["Men At Work"],
                    AlbumArtUrl = string.Empty
                }};
            _distributedCache.Setup(c => c.Get("topselling")).Returns(
                Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(albums, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                })));

            //Act
            var topalbums = _albumController.Get();

            //Assert
            Assert.True(topalbums.Result.Count == 1);
        }

        [Fact]
        public void Test_GetByIdFromCache()
        {
            //Arrange
            var albums = new List<Album>() { new Album
                {
                    AlbumId = 2,
                    Title = "...And Justice For All",
                    Genre = SampleData.Genres["Metal"],
                    Price = 8.99M,
                    Artist = SampleData.Artists["Metallica"],
                    AlbumArtUrl = string.Empty
                }};
            _distributedCache.Setup(c => c.Get("album_2")).Returns(
                Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(albums[0], new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                })));

            //Act
            var album = _albumController.GetById(1);

            //Assert
            Assert.True(album != null);
        }
    }
}
