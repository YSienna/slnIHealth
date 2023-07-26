using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TPaymentCategory
    {
        public TPaymentCategory()
        {
            TOrders = new HashSet<TOrder>();
        }

        public int FPaymentCategoryId { get; set; }
        public string FPaymentCategory { get; set; }

        public virtual ICollection<TOrder> TOrders { get; set; }
    }
}
