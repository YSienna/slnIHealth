using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TContactText
    {
        public int FContactTextId { get; set; }
        public int? FCoachContactId { get; set; }
        public bool? FIsCoach { get; set; }
        public string FContactText { get; set; }
        public string FContactTextTime { get; set; }

        public virtual TCoachContact FCoachContact { get; set; }
    }
}
