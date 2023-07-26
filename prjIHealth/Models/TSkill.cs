using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TSkill
    {
        public TSkill()
        {
            TCoachSkills = new HashSet<TCoachSkill>();
        }

        public int FSkillId { get; set; }
        public string FSkillName { get; set; }
        public string FSkillDescription { get; set; }
        public string FSkillImage { get; set; }

        public virtual ICollection<TCoachSkill> TCoachSkills { get; set; }
    }
}
