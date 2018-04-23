using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xTremeShop.Models
{
    public class UserLibrary
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<LibraryApps> LibraryApps { get; set; }
    }

    public class LibraryApps
    {
        public int LibaryId { get; set; }
        public UserLibrary Library { get; set; }

        public int AppId { get; set; }
        public MobileApp App { get; set; }
    }
}
