using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MS.WEB.Models
{
    public class MessageView
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Текст сообщения")]
        public string Text { get; set; }

        [Display(Name = "Отправить на email")]
        public bool Email { get; set; }

        [Display(Name = "Отправить sms")]
        public bool Sms { get; set; }

        [Display(Name = "Отправить Push")]
        public bool Push { get; set; }
    }
}