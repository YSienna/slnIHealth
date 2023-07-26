using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TRegion
    {
        public int FRegionId { get; set; }
        public string FRegion { get; set; }
        public string Postal { get; set; }
        public int? FCityId { get; set; }

        public virtual TCity FCity { get; set; }
    }
}
