using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MS.Entity.Entity;

namespace MS.DAL
{
    public class MessageContext : DbContext
    {
        public MessageContext(string connect) : base(connect)
        { }

        public DbSet<Message> Messages { get; set; }
    }
}
