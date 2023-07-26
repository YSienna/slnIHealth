using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TCoachContact
    {
        public TCoachContact()
        {
            TContactTexts = new HashSet<TContactText>();
            TCourses = new HashSet<TCourse>();
        }

        public int FCoachContactId { get; set; }
        public int? FMemberId { get; set; }
        public int? FCoachId { get; set; }
        public string FContactDate { get; set; }
        public int? FStatusNumber { get; set; }
        public int? FAvailableTimeNum { get; set; }

        public virtual TAvailableTime FAvailableTimeNumNavigation { get; set; }
        public virtual TCoach FCoach { get; set; }
        public virtual TMember FMember { get; set; }
        public virtual TStatus FStatusNumberNavigation { get; set; }
        public virtual ICollection<TContactText> TContactTexts { get; set; }
        public virtual ICollection<TCourse> TCourses { get; set; }
    }
}
