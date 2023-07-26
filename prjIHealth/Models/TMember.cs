using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace prjIHealth.Models
{
    public partial class TMember
    {
        public TMember()
        {
            TBodyRecords = new HashSet<TBodyRecord>();
            TCalorieIntakes = new HashSet<TCalorieIntake>();
            TCandidates = new HashSet<TCandidate>();
            TCoachContacts = new HashSet<TCoachContact>();
            TCoachRates = new HashSet<TCoachRate>();
            TCoaches = new HashSet<TCoach>();
            TNews = new HashSet<TNews>();
            TNewsComments = new HashSet<TNewsComment>();
            TOrders = new HashSet<TOrder>();
            TProblems = new HashSet<TProblem>();
            TReplies = new HashSet<TReply>();
            TTrackLists = new HashSet<TTrackList>();
        }

        public int FMemberId { get; set; }
        [DisplayName("姓名")]
        public string FMemberName { get; set; }
        [Required][DisplayName("密碼")]
        public string FPassword { get; set; }
        [DisplayName("生日")]
        public string FBirthday { get; set; }
        [DisplayName("生理性別")]
        public bool? FGender { get; set; }
        [DisplayName("照片")]
        public string FPicturePath { get; set; }
        [Required][DisplayName("使用者名稱")]
        public string FUserName { get; set; }
        [DisplayName("地址")]
        public string FAddress { get; set; }
        [DisplayName("電話")]
        public string FPhone { get; set; }
       [Required] [DisplayName("電子信箱")]
        public string FEmail { get; set; }
        [DisplayName("註冊日期")]
        public string FRegisterDate { get; set; }
        [DisplayName("權限名稱")]
        public int? FAuthorityId { get; set; }
        [DisplayName("會員狀態")]
        public bool? FDisabled { get; set; }
        [DisplayName("備註")]
        public string FRemarks { get; set; }

        public virtual TAuthority FAuthority { get; set; }
        public virtual ICollection<TBodyRecord> TBodyRecords { get; set; }
        public virtual ICollection<TCalorieIntake> TCalorieIntakes { get; set; }
        public virtual ICollection<TCandidate> TCandidates { get; set; }
        public virtual ICollection<TCoachContact> TCoachContacts { get; set; }
        public virtual ICollection<TCoachRate> TCoachRates { get; set; }
        public virtual ICollection<TCoach> TCoaches { get; set; }
        public virtual ICollection<TNews> TNews { get; set; }
        public virtual ICollection<TNewsComment> TNewsComments { get; set; }
        public virtual ICollection<TOrder> TOrders { get; set; }
        public virtual ICollection<TProblem> TProblems { get; set; }
        public virtual ICollection<TReply> TReplies { get; set; }
        public virtual ICollection<TTrackList> TTrackLists { get; set; }
    }
}
