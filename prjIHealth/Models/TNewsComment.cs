using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TNewsComment
    {
        public int FNewsCommentId { get; set; }
        public int FNewsId { get; set; }
        public int? FMemberId { get; set; }
        public string FNewsReply { get; set; }

        public virtual TMember FMember { get; set; }
        public virtual TNews FNews { get; set; }
    }
}
