using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CNewsCommentViewModel
    {

        public CNewsCommentViewModel()
        {
            _comment = new TNewsComment();
        }
        private TNewsComment _comment;
        public int FNewsCommentId { get; set; }
        public int FNewsId { get; set; }
        public int? FMemberId { get; set; }
        public string FNewsReply { get; set; }
        public TMember MemberName { get; set; }
        public TNews GetNews { get; set; }
    }
}
