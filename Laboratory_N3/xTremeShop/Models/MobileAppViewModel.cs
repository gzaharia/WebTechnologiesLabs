using System;
using System.Collections.Generic;

namespace xTremeShop.Models
{
    public class MobileAppViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Downloads { get; set; }
        public float Rating { get; set; }
        public string Category { get; set; }
        public byte[] AppIcon { get; set; }
        public bool FullAccess { get; set; }

        public ICollection<LibraryApps> LibraryApps { get; set; }
    }
}
