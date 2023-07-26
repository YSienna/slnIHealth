using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TNews
    {
        public TNews()
        {
            TNewsComments = new HashSet<TNewsComment>();
            TNewsImages = new HashSet<TNewsImage>();
        }

        public int FNewsId { get; set; }
        public string FTitle { get; set; }
        public string FNewsDate { get; set; }
        public string FContent { get; set; }
        public string FThumbnailPath { get; set; }
        public int FNewsCategoryId { get; set; }
        public int? FViews { get; set; }
        public string FVideoUrl { get; set; }
        public int? FMemberId { get; set; }

        public virtual TMember FMember { get; set; }
        public virtual TNewsCategory FNewsCategory { get; set; }
        public virtual ICollection<TNewsComment> TNewsComments { get; set; }
        public virtual ICollection<TNewsImage> TNewsImages { get; set; }
    }
}
