using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TStatus
    {
        public TStatus()
        {
            TCoachContacts = new HashSet<TCoachContact>();
            TCoaches = new HashSet<TCoach>();
            TCourses = new HashSet<TCourse>();
            TOrders = new HashSet<TOrder>();
            TProblems = new HashSet<TProblem>();
            TReservations = new HashSet<TReservation>();
        }

        public int FStatusId { get; set; }
        public int FStatusNumber { get; set; }
        public string FStatus { get; set; }

        public virtual ICollection<TCoachContact> TCoachContacts { get; set; }
        public virtual ICollection<TCoach> TCoaches { get; set; }
        public virtual ICollection<TCourse> TCourses { get; set; }
        public virtual ICollection<TOrder> TOrders { get; set; }
        public virtual ICollection<TProblem> TProblems { get; set; }
        public virtual ICollection<TReservation> TReservations { get; set; }
    }
}
