using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.Models
{
    public class Message
    {
        [Required][DisplayName("使用者帳戶")]
        public string fUserName{ get; set; }
        [Required]
        [DisplayName("聊天內容")]
        public string text { get; set; }
        public DateTime When { get; set; }
    }
   
}
