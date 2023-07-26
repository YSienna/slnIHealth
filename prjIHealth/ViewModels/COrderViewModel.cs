using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class COrderViewModel
    {
        public COrderViewModel()
        {
            _prod = new TOrder();
        }
        private TOrder _prod;
        //public List<COrderViewModel> odlist { get; set; }
        public TOrder order
        {
            get { return _prod; }
            set { _prod = value; }
        }
        [DisplayName("訂單編號")]
        public int FOrderId
        {
            get { return _prod.FOrderId; }
            set { _prod.FOrderId = value; }
        }
        [DisplayName("付款方式")]
        public int FPaymentCategoryId
        {
            get { return _prod.FPaymentCategoryId; }
            set { _prod.FPaymentCategoryId = value; }
        }
        [DisplayName("日期")]
        public string FDate
        {
            get { return _prod.FDate; }
            set { _prod.FDate = value; }
        }
        [DisplayName("會員")]
        public int FMemberId
        {
            get { return _prod.FMemberId; }
            set { _prod.FMemberId = value; }
        }
        [DisplayName("地址")]
        public string FAddress
        {
            get { return _prod.FAddress; }
            set { _prod.FAddress = value; }
        }
        [DisplayName("收件人")]
        public string FContact
        {
            get { return _prod.FContact; }
            set { _prod.FContact = value; }
        }
        [DisplayName("電話")]
        public string FPhone
        {
            get { return _prod.FPhone; }
            set { _prod.FPhone = value; }
        }
        [DisplayName("備註")]
        public string FRemarks
        {
            get { return _prod.FRemarks; }
            set { _prod.FRemarks = value; }
        }
        [DisplayName("狀態")]
        public int FStatusNumber
        {
            get { return _prod.FStatusNumber; }
            set { _prod.FStatusNumber = value; }
        }
        [DisplayName("付款方式")]
        public TPaymentCategory fPayment { get; set; }
        [DisplayName("狀態")]
        public TStatus fstatus { get; set; }
        [DisplayName("會員")]
        public TMember fmember { get; set; }
    }
}
