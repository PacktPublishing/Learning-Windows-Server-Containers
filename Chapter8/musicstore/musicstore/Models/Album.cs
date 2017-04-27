using System;
using System.Collections.Generic;

namespace MusicStore.Models
{
    public class Album
    {
        public int AlbumId { get; set; }

        public int GenreId { get; set; }

        public int ArtistId { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string AlbumArtUrl { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }

        public DateTime Created { get; set; }

        /// <summary>
        /// TODO: Temporary hack to populate the orderdetails until EF does this automatically. 
        /// </summary>
        public Album()
        {
            OrderDetails = new List<OrderDetail>();
            Created = DateTime.UtcNow;
        }
    }
}