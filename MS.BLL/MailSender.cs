using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MS.BLL
{
    public class MailSender
    {
        /* Пример вызова
         * SendMail("smtp.gmail.com", "mymail@gmail.com", "myPassword", "yourmail@gmail.com", "Тема письма", "Тело письма", "C:\\1.txt");
         */

        // unicodsgn@gmail.com

        /// <summary>
        /// Отправка письма на почтовый ящик C# mail send
        /// </summary>
        /// <param name="smtpServer">Имя SMTP-сервера (smtp.gmail.com)</param>
        /// <param name="from">Адрес отправителя (mymail@gmail.com)</param>
        /// <param name="password">пароль к почтовому ящику отправителя</param>
        /// <param name="mailto">Адрес получателя (yourmail@gmail.com)</param>
        /// <param name="caption">Тема письма</param>
        /// <param name="message">Сообщение</param>
        /// <param name="attachFile">Присоединенный файл</param>
        public void SendMail(string smtpServer, string from, string password, 
            string mailto, string caption, string message, string attachFile = null)
        {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(new MailAddress(mailto));
                mail.Subject = caption;
                mail.Body = message;
                if (!string.IsNullOrEmpty(attachFile))
                    mail.Attachments.Add(new Attachment(attachFile));
                SmtpClient client = new SmtpClient();
                client.Host = smtpServer;
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from.Split('@')[0], password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                mail.Dispose();
        }
    }
}
