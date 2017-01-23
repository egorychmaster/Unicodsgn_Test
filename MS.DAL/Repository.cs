using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MS.Entity.Entity;
using MS.Entity.Interfaces;

namespace MS.DAL
{
    public class Repository : IRepository
    {
        string connect = string.Empty;
//        private readonly MessageContext db;

        #region : constructor
        private Repository() {}

        public Repository(string connect, bool initDatabase = false)
        {
            this.connect = connect;
//            db = new MessageContext(connect);

            if(initDatabase)
                Database.SetInitializer(new DbInitializer());
        }
        #endregion : constructor

        public IEnumerable<Message> Messages()
//            public IQueryable<Message> Messages()
        {
            using (MessageContext db = new MessageContext(connect))
            {
                return db.Messages.ToList();
            }
        }

        public void MessageAdd(Message message)
        {
            using (MessageContext db = new MessageContext(connect))
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }
        }

        public void MessageEdit(Message message)
        {
            using (MessageContext db = new MessageContext(connect))
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

//        public void Dispose()
//        {
//            db.Dispose();
//        }
    }
}
