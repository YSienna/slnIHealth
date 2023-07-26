using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TAvailableTime
    {
        public TAvailableTime()
        {
            TCoachAvailableTimes = new HashSet<TCoachAvailableTime>();
            TCoachContacts = new HashSet<TCoachContact>();
            TCourses = new HashSet<TCourse>();
        }

        public int FAvailableTimeId { get; set; }
        public string FAvailableTime { get; set; }
        public int? FAvailableTimeNum { get; set; }

        public virtual ICollection<TCoachAvailableTime> TCoachAvailableTimes { get; set; }
        public virtual ICollection<TCoachContact> TCoachContacts { get; set; }
        public virtual ICollection<TCourse> TCourses { get; set; }
    }
}
