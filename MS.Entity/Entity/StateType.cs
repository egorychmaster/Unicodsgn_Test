using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Entity.Entity
{
    /// <summary>
    /// Состояние сообщения
    /// </summary>
    public enum StateType
    {
        None = 0,
        ToSend = 1, // К отправке готово
        Success = 2, // Отправлено
        Error = 3 // Отправка завершилась ошибкой
    }
}
