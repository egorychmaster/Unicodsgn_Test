using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MS.Entity.Entity;

namespace MS.WEB.Models
{
    public class QueueView
    {
        public int Id { get; set; }
        [Display(Name = "Текст сообщения")]
        public string Text { get; set; }
        [Display(Name = "Email")]
        public bool IsEmailSend { get; set; }
        [Display(Name = "Sms")]
        public bool IsSmsSend { get; set; }
        [Display(Name = "Push")]
        public bool IsPushSend { get; set; }
        [Display(Name = "Состояние сообщения")]
        public StateType State { get; set; }
    }
}