using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xTremeShop.Models
{
    public class MobileApp
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Downloads { get; set; }
        public float Rating { get; set; }
        public string Category { get; set; }
        public byte[] AppIcon { get; set; }

        public ICollection<LibraryApps> LibraryApps { get; set; }
    }
}
