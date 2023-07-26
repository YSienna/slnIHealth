using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TCoachSkill
    {
        public int FCoachSkillId { get; set; }
        public int? FCoachId { get; set; }
        public int? FSkillId { get; set; }

        public virtual TCoach FCoach { get; set; }
        public virtual TSkill FSkill { get; set; }
    }
}
