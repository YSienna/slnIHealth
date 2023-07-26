using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TCity
    {
        public TCity()
        {
            TCoaches = new HashSet<TCoach>();
            TRegions = new HashSet<TRegion>();
        }

        public int FCityId { get; set; }
        public string FCityName { get; set; }
        public int? FCityOrder { get; set; }

        public virtual ICollection<TCoach> TCoaches { get; set; }
        public virtual ICollection<TRegion> TRegions { get; set; }
    }
}
