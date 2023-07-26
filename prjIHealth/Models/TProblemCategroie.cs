using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TProblemCategroie
    {
        public TProblemCategroie()
        {
            TProblems = new HashSet<TProblem>();
        }

        public int FProblemCategoryId { get; set; }
        public string FProblemCategory { get; set; }

        public virtual ICollection<TProblem> TProblems { get; set; }
    }
}
