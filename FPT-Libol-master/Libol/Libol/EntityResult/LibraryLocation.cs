using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libol.EntityResult
{
    public class LibraryLocation
    {
        public Library lib { get; set; }
        public List<Location> locs { get; set; }

        public LibraryLocation(Library lib, List<Location> locs)
        {
            this.lib = lib;
            this.locs = locs;
        }
    }
}