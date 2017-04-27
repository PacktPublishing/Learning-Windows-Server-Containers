using System;
using System.ComponentModel.DataAnnotations;

namespace MusicStore.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        public string CartId { get; set; }
        public int AlbumId { get; set; }
        public int Count { get; set; }

        public DateTime DateCreated { get; set; } 

        public virtual Album Album { get; set; }
    }
}