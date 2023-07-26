using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class COrderDetailViewModel
    {
        public COrderDetailViewModel()
        {
            _prod = new TOrderDetail();
        }
        private TOrderDetail _prod;
        public TOrderDetail orderdetail
        {
            get { return _prod; }
            set { _prod = value; }
        }
        public int FOrderDetailsId
        {
            get { return _prod.FOrderDetailsId; }
            set { _prod.FOrderDetailsId = value; }
        }
        public int FOrderId
        {
            get { return _prod.FOrderId; }
            set { _prod.FOrderId = value; }
        }
        [DisplayName("產品")]
        public int FProductId
        {
            get { return _prod.FProductId; }
            set { _prod.FProductId = value; }
        }
        [DisplayName("數量")]
        public int FQuantity
        {
            get { return _prod.FQuantity; }
            set { _prod.FQuantity = value; }
        }
        [DisplayName("價錢")]
        public decimal FUnitprice
        {
            get { return _prod.FUnitprice; }
            set { _prod.FUnitprice = value; }
        }
        [DisplayName("折扣")]
        public int FDiscountId
        {
            get { return _prod.FDiscountId; }
            set { _prod.FDiscountId = value; }
        }

        public TProduct fproduct { get; set; }
        public TDiscount fdiscount { get; set; }
        public TOrder forder { get; set; }
        public TPaymentCategory pay { get; set; }
        public TStatus sta { get; set; }
    }
}
