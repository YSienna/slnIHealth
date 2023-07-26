using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TReply
    {
        public int FReplyId { get; set; }
        public int FProblemId { get; set; }
        public string FReplyTime { get; set; }
        public string FReplyContent { get; set; }
        public int? FReplierId { get; set; }
        public string FReplyType { get; set; }

        public virtual TProblem FProblem { get; set; }
        public virtual TMember FReplier { get; set; }
    }
}
