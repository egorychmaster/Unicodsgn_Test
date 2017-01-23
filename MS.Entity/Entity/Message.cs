//using MS.Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Entity.Entity
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
//        public MessageType MessageType { get; set; }
        public bool IsEmailSend { get; set; }
        public bool IsSmsSend { get; set; }
        public bool IsPushSend { get; set; }
        /// <summary> Если сообщение отправлено </summary>
        public StateType State { get; set; }
    }
}
