using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TBodyRecord
    {
        public int FBodyRecordId { get; set; }
        public int? FMemberId { get; set; }
        public string FRecordDate { get; set; }
        public double? FWorkload { get; set; }
        public double? FHeight { get; set; }
        public double? FWeight { get; set; }

        public virtual TMember FMember { get; set; }
    }
}
