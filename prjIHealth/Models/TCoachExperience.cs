using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TCoachExperience
    {
        public int FExperienceId { get; set; }
        public int? FCoachId { get; set; }
        public string FExperience { get; set; }

        public virtual TCoach FCoach { get; set; }
    }
}
