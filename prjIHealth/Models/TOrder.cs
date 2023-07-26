using System;
using System.Collections.Generic;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TOrder
    {
        public TOrder()
        {
            TOrderDetails = new HashSet<TOrderDetail>();
        }

        public int FOrderId { get; set; }
        public int FPaymentCategoryId { get; set; }
        public string FDate { get; set; }
        public int FMemberId { get; set; }
        public string FAddress { get; set; }
        public string FContact { get; set; }
        public string FPhone { get; set; }
        public string FRemarks { get; set; }
        public int FStatusNumber { get; set; }

        public virtual TMember FMember { get; set; }
        public virtual TPaymentCategory FPaymentCategory { get; set; }
        public virtual TStatus FStatusNumberNavigation { get; set; }
        public virtual ICollection<TOrderDetail> TOrderDetails { get; set; }
    }
}
