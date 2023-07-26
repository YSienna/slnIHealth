using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TCoachRate
    {
        public int FRateId { get; set; }
        public int? FMemberId { get; set; }
        public int? FCoachId { get; set; }
        public int? FRateStar { get; set; }
        public string FRateText { get; set; }
        public string FRateDate { get; set; }
        public bool? FVisible { get; set; }

        public virtual TCoach FCoach { get; set; }
        public virtual TMember FMember { get; set; }
    }
}
