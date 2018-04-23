using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace xTremeShop.ViewModels
{
    public class MobileAppCreateViewModel
    {
        [Required]
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Downloads { get; set; }
        public float Rating { get; set; }
        [Required]
        public string Category { get; set; }

        public IFormFile AppIcon { get; set; }
    }
}
