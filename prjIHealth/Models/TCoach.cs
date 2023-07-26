using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TCoach
    {
        public TCoach()
        {
            TCandidates = new HashSet<TCandidate>();
            TCoachAvailableTimes = new HashSet<TCoachAvailableTime>();
            TCoachContacts = new HashSet<TCoachContact>();
            TCoachExperiences = new HashSet<TCoachExperience>();
            TCoachLicenses = new HashSet<TCoachLicense>();
            TCoachRates = new HashSet<TCoachRate>();
            TCoachSkills = new HashSet<TCoachSkill>();
        }

        public int FCoachId { get; set; }
        public string FCoachName { get; set; }
        public int? FMemberId { get; set; }
        public int? FCityId { get; set; }
        public string FCoachImage { get; set; }
        public int? FCoachFee { get; set; }
        public string FCoachDescription { get; set; }
        public string FApplyDate { get; set; }
        public int? FStatusNumber { get; set; }
        public bool? FVisible { get; set; }
        public int? FCourseCount { get; set; }
        public string FSlogan { get; set; }

        public virtual TCity FCity { get; set; }
        public virtual TMember FMember { get; set; }
        public virtual TStatus FStatusNumberNavigation { get; set; }
        public virtual ICollection<TCandidate> TCandidates { get; set; }
        public virtual ICollection<TCoachAvailableTime> TCoachAvailableTimes { get; set; }
        public virtual ICollection<TCoachContact> TCoachContacts { get; set; }
        public virtual ICollection<TCoachExperience> TCoachExperiences { get; set; }
        public virtual ICollection<TCoachLicense> TCoachLicenses { get; set; }
        public virtual ICollection<TCoachRate> TCoachRates { get; set; }
        public virtual ICollection<TCoachSkill> TCoachSkills { get; set; }
    }
}
