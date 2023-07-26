using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TProduct
    {
        public TProduct()
        {
            TOrderDetails = new HashSet<TOrderDetail>();
            TProductsImages = new HashSet<TProductsImage>();
            TTrackLists = new HashSet<TTrackList>();
        }

        public int FProductId { get; set; }
        public string FProductName { get; set; }
        public int FCategoryId { get; set; }
        public decimal FUnitprice { get; set; }
        public string FDescription { get; set; }
        public bool FVisible { get; set; }
        public string FCoverImage { get; set; }

        public virtual TProductCategory FCategory { get; set; }
        public virtual ICollection<TOrderDetail> TOrderDetails { get; set; }
        public virtual ICollection<TProductsImage> TProductsImages { get; set; }
        public virtual ICollection<TTrackList> TTrackLists { get; set; }
    }
}
