using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TCandidate
    {
        public int FCandidateId { get; set; }
        public int? FMemberId { get; set; }
        public int? FCoachId { get; set; }

        public virtual TCoach FCoach { get; set; }
        public virtual TMember FMember { get; set; }
    }
}
