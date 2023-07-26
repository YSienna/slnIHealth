using Microsoft.AspNetCore.Http;
using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjiHealth.ViewModels
{
    public class CLoginViewModel
    {
        private  TMember _tmember;
        public CLoginViewModel() { _tmember = new TMember(); }
        public TMember Member { get { return _tmember; } set {_tmember=value; } }
        public int fMemberId { get { return _tmember.FMemberId; } set { _tmember.FMemberId = value; } }
        public string fUserName { get { return _tmember.FUserName; } set { _tmember.FUserName = value; } }
        [DataType(DataType.Password)]
        public string fPassword { get { return _tmember.FPassword; } set { _tmember.FPassword = value; } }
        public string fEmail { get { return _tmember.FEmail; } set { _tmember.FEmail = value; } }
        public string fPhone { get { return _tmember.FPhone; } set { _tmember.FPhone = value; } }
        [DisplayName("姓名")]
        public string fMemberName { get { return _tmember.FMemberName; } set { _tmember.FMemberName = value; } }
        public string fBirthday { get { return _tmember.FBirthday; } set { _tmember.FBirthday = value; } }
        public string fAddress { get { return _tmember.FAddress; } set { _tmember.FAddress = value; } }
        public string fRemarks { get { return _tmember.FRemarks; } set { _tmember.FRemarks = value; } }
        public int? FAuthorityId { get { return _tmember.FAuthorityId; } set { _tmember.FAuthorityId = value; } }
        public bool? FDisabled { get { return _tmember.FDisabled; } set { _tmember.FDisabled= value; } }
        public bool? FGender { get { return _tmember.FGender; } set { _tmember.FGender = value; } }
        public string fPicturePath { get { return _tmember.FPicturePath; } set { _tmember.FPicturePath = value; } }


        public IFormFile photo { get; set; }

        public int fAuthorityId {  get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
        public string firstPassword { get; set; }
        public string  confirmPassword { get; set; }

    }
}
