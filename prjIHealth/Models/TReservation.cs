using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TReservation
    {
        public int FReservationId { get; set; }
        public int? FCourseId { get; set; }
        public string FCourseTime { get; set; }
        public int? FStatusNumber { get; set; }

        public virtual TCourse FCourse { get; set; }
        public virtual TStatus FStatusNumberNavigation { get; set; }
    }
}
