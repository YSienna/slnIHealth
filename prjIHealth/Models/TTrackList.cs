using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TTrackList
    {
        public int FTrackId { get; set; }
        public int FProductId { get; set; }
        public int FMemberId { get; set; }

        public virtual TMember FMember { get; set; }
        public virtual TProduct FProduct { get; set; }
    }
}
