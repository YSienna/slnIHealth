using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TNewsCategory
    {
        public TNewsCategory()
        {
            TNews = new HashSet<TNews>();
        }

        public int FNewsCategoryId { get; set; }
        public string FCategoryName { get; set; }

        public virtual ICollection<TNews> TNews { get; set; }
    }
}
