using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TDiscount
    {
        public TDiscount()
        {
            TOrderDetails = new HashSet<TOrderDetail>();
        }

        public int FDiscountId { get; set; }
        public string FDiscountCode { get; set; }
        public string FDiscretion { get; set; }
        public string FDiscountValue { get; set; }

        public virtual ICollection<TOrderDetail> TOrderDetails { get; set; }
    }
}
