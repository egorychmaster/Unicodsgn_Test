using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MS.Entity.Entity;

namespace MS.DAL
{
        public class DbInitializer : DropCreateDatabaseAlways<MessageContext>
    {
            protected override void Seed(MessageContext db)
            {
                db.Messages.Add(new Message { Text = "Первое сообщение 1", IsEmailSend = true, State = StateType.ToSend });
                db.Messages.Add(new Message { Text = "Второе сообщение 2", IsSmsSend = true, State = StateType.ToSend });
                db.Messages.Add(new Message { Text = "Третье сообщение 3", IsPushSend = true, State = StateType.ToSend });
    
                base.Seed(db);
            }
        }
}
