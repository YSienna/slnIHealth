using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TOrderDetail
    {
        public int FOrderDetailsId { get; set; }
        public int FOrderId { get; set; }
        public int FProductId { get; set; }
        public int FQuantity { get; set; }
        public decimal FUnitprice { get; set; }
        public int FDiscountId { get; set; }

        public virtual TDiscount FDiscount { get; set; }
        public virtual TOrder FOrder { get; set; }
        public virtual TProduct FProduct { get; set; }
    }
}
