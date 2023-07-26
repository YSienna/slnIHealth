using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using prjiHealth.Models;
using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjiHealth.ViewModels
{
    public class CProblemViewModel
    {
        public CProblemViewModel()
        {
            _prob = new TProblem();
        }
        private TProblem _prob;
        public TProblem product
        {
            get { return _prob; }
            set { _prob = value; }
        }

        public int FProblemId {
            get { return _prob.FProblemId; }
            set { _prob.FProblemId = value; }
             }
        [DisplayName("問題時間")]
        public string FProblemTime
        {
            get { return _prob.FProblemTime; }
            set { _prob.FProblemTime = value; }
        }
        public int FProblemCategoryId
        {
            get { return _prob.FProblemCategoryId; }
            set { _prob.FProblemCategoryId = value; }
        }
        [DisplayName("問題內容")]
        public string FProblemContent
        {
            get { return _prob.FProblemContent; }
            set { _prob.FProblemContent = value; }
        }
        public string FFilePath
        {
            get { return _prob.FFilePath; }
            set { _prob.FFilePath = value; }
        }
        
        public int FMemberId
        {
            get { return _prob.FMemberId; }
            set { _prob.FMemberId = value; }
        }
    
        public int? FOrderId
        {
            get { return _prob.FOrderId; }
            set { _prob.FOrderId = value; }
        }
        [DisplayName("Email")]
        public string FEmail
        {
            get { return _prob.FEmail; }
            set { _prob.FEmail = value; }
        }
        [DisplayName("連絡電話")]
        public string FContactPhone
        {
            get { return _prob.FContactPhone; }
            set { _prob.FContactPhone = value; }
        }
        [BindRequired]
        public int FStatusNumber
        {
            get { return _prob.FStatusNumber; }
            set { _prob.FStatusNumber = value; }
        }
        public byte[] FPicturePath
        {
            get { return _prob.FPicturePath; }
            set { _prob.FPicturePath = value; }
        }
     
        public TProblemCategroie FProblemCategory { get; set; }
      
        public TStatus Status { get; set; }

        public IFormFile photo { get; set; }

        //reply用屬性
        public int FReplyId { get; set; }
        public string FReplyTime { get; set; }
        [Required]
        public string FReplyContent { get; set; }
        public int? FReplierId { get; set; }
        [BindRequired]
        public string FReplyType { get; set; }
    }
}
