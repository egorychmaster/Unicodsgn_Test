using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MS.Entity.Entity;

namespace MS.Entity.Interfaces
{
    public interface IRepository// : IDisposable
    {
        IEnumerable<Message> Messages();
//        IQueryable<Message> Messages();

        void MessageEdit(Message message);

        void MessageAdd(Message message);
    }
}
