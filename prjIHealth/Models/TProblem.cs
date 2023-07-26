using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TProblem
    {
        public TProblem()
        {
            TReplies = new HashSet<TReply>();
        }

        public int FProblemId { get; set; }
        public string FProblemTime { get; set; }
        public int FProblemCategoryId { get; set; }
        public string FProblemContent { get; set; }
        public string FFilePath { get; set; }
        public byte[] FPicturePath { get; set; }
        public int FMemberId { get; set; }
        public int? FOrderId { get; set; }
        public string FEmail { get; set; }
        public string FContactPhone { get; set; }
        public int FStatusNumber { get; set; }

        public virtual TMember FMember { get; set; }
        public virtual TProblemCategroie FProblemCategory { get; set; }
        public virtual TStatus FStatusNumberNavigation { get; set; }
        public virtual ICollection<TReply> TReplies { get; set; }
    }
}
