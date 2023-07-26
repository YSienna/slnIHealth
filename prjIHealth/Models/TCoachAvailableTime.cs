using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TCoachAvailableTime
    {
        public int FCoachTimeId { get; set; }
        public int? FCoachId { get; set; }
        public int? FAvailableTimeId { get; set; }

        public virtual TAvailableTime FAvailableTime { get; set; }
        public virtual TCoach FCoach { get; set; }
    }
}
