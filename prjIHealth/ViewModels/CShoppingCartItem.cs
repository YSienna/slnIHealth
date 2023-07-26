using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CShoppingCartItem
    {
        public int productId { get; set; }
        [DisplayName("購買量")]
        public int count { get; set; }
        [DisplayName("單價")]
        public decimal price { get; set; }

        public decimal discount { get; set; }

        public decimal 小計 { get { return (this.count * this.price)-this.discount; } }
        public TProduct product { get; set; }
        public  int discountID { get; set; }

        //訂單用屬性
        [Required]
        public int FPaymentCategoryId { get; set; }
        public string FDate { get; set; }
        public int FMemberId { get; set; }
        [Required]
        public string FAddress { get; set; }
        [Required]
        public string FContact { get; set; }
        [Required]
        public string FPhone { get; set; }
        public string FRemarks { get; set; }
        public int FStatusNumber { get; set; }
    }
}
