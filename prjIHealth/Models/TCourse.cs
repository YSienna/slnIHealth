using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TCourse
    {
        public TCourse()
        {
            TReservations = new HashSet<TReservation>();
        }

        public int FCourseId { get; set; }
        public int? FCoachContactId { get; set; }
        public string FCourseName { get; set; }
        public int? FCourseTotal { get; set; }
        public int? FStatusNumber { get; set; }
        public bool? FVisible { get; set; }
        public int? FAvailableTimeNum { get; set; }

        public virtual TAvailableTime FAvailableTimeNumNavigation { get; set; }
        public virtual TCoachContact FCoachContact { get; set; }
        public virtual TStatus FStatusNumberNavigation { get; set; }
        public virtual ICollection<TReservation> TReservations { get; set; }
    }
}
