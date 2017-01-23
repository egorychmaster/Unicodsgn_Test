using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using MS.Entity.Entity;

namespace MS.BLL
{
    public class ManagerMail
    {
        public ManagerMail()
        {
            Logger.InitLogger();
        }

        public Message Send(Message mess)
        {
            var tasks = new List<Task<bool>>();

            Task<bool> emailTask = null;
            Task<bool> smsTask = null;
            Task<bool> pushTask = null;

            if (mess.IsEmailSend)
            {
                var mailInfo = new MailInfo()
                {
                    MailTo = System.Configuration.ConfigurationManager.AppSettings["SendToEmail"],
                    Caption = "type: " + MessageType.Email.ConvertEnumToString(),
                    Text = mess.Text,
                    MessageType = MessageType.Email
                };
                emailTask = Task.Factory.StartNew(SendFunc, mailInfo);
                tasks.Add(emailTask);
            }
            if (mess.IsSmsSend)
            {
                var mailInfo = new MailInfo()
                {
                    MailTo = System.Configuration.ConfigurationManager.AppSettings["SendToSms"],
                    Caption = "type: " + MessageType.Sms.ConvertEnumToString(),
                    Text = mess.Text,
                    MessageType = MessageType.Sms
                };
                smsTask = Task.Factory.StartNew(SendFunc, mailInfo);
                tasks.Add(smsTask);
            }
            if (mess.IsPushSend)
            {
                var mailInfo = new MailInfo()
                {
                    MailTo = System.Configuration.ConfigurationManager.AppSettings["SendToPush"],
                    Caption = "type: " + MessageType.Push.ConvertEnumToString(),
                    Text = mess.Text,
                    MessageType = MessageType.Push
                };
                pushTask = Task.Factory.StartNew(SendFunc, mailInfo);
                tasks.Add(pushTask);
            }

            Task.WaitAll(tasks.ToArray());

            mess.IsEmailSend = !emailTask?.Result ?? false;
            mess.IsSmsSend = !smsTask?.Result ?? false;
            mess.IsPushSend = !pushTask?.Result ?? false;

            return mess;
        }

        Func<object, bool> SendFunc = (object obj) =>
        {
            try
            {
//                 throw new Exception("Ошибка отправки");

                MailInfo mailInfo = (MailInfo)obj;

                MailSender mail = new MailSender();
                string password = "Qwerty12";
                mail.SendMail("smtp.gmail.com", "unicodsgn@gmail.com", password, mailInfo.MailTo, mailInfo.Caption, mailInfo.Text);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Mail.Send: ", ex);
                return false;
            }
        };
    }

    public enum MessageType
    {
        None = 0,
        Email = 1,
        Sms = 2,
        Push = 3,
    }

    class MailInfo
    {
        /// <summary> Элктронный ящик получателя </summary>
        public string MailTo { get; set; }
        /// <summary> Тема письма </summary>
        public string Caption { get; set; }

        string text = string.Empty;
        /// <summary>
        /// С указанием в тексте письма какой это тип сообщения и текста сообщения
        /// </summary>
        public string Text
        {
            get
            {
                string fullText = string.Empty;

                switch (MessageType)
                {
                    case MessageType.Email:
                        fullText = "Email\n";
                        break;
                    case MessageType.Sms:
                        fullText = "Sms\n";
                        break;
                    case MessageType.Push:
                        fullText = "Push\n";
                        break;
                }
                return fullText + text;
            }
            set { text = value; }
        }

        public MessageType MessageType { get; set; }
    }
}
