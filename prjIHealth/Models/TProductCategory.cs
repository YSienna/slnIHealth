using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TProductCategory
    {
        public TProductCategory()
        {
            TProducts = new HashSet<TProduct>();
        }

        public int FCategoryId { get; set; }
        public string FCategoryName { get; set; }
        public string FDescription { get; set; }
        public string FImage { get; set; }

        public virtual ICollection<TProduct> TProducts { get; set; }
    }
}
