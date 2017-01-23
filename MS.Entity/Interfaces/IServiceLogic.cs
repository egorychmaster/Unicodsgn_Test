using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MS.Entity.Entity;

namespace MS.Entity.Interfaces
{
    public interface IServiceLogic
    {
        /// <summary>
        /// Помещает сообщение в очередь отправки сообщений
        /// </summary>
        bool SendMessage(Message mess);

        /// <summary> Получить очередь сообщений </summary>
        List<Message> Queue();

        /// <summary>
        /// Проверяет очередь сообщений и отправляет одно из сообщений
        /// </summary>
        bool QueueStep();
    }
}
