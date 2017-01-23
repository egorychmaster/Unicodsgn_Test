using MS.Entity.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using MS.Entity.Entity;

namespace MS.BLL
{
    public class ServiceLogic : IServiceLogic
    {
        [Dependency]
        public IRepository Repository { get; set; }


        /// <summary>
        /// Помещает сообщение в очередь отправки сообщений
        /// </summary>
        public bool SendMessage(Message mess)
        {
            try
            {
                mess.State = StateType.ToSend;
                Repository.MessageAdd(mess);

                if (ManagerMessages.GeneralQueueList == null)
                {// Если объекта с генеральной очередью в памяти не было - продгружаем из БД
                    new ManagerMessages(Repository);
                }
                else
                {
                    ManagerMessages.SendMessage(mess);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log.Error("ServiceLogic.SendMessage: ", ex);
                throw;
            }
        }

        /// <summary> Получить очередь сообщений </summary>
        public List<Message> Queue()
        {
            try
            {
                if (ManagerMessages.GeneralQueueList == null)
                {// Если объекта с генеральной очередью в памяти не было - продгружаем из БД
                    new ManagerMessages(Repository);
                }
                
                return ManagerMessages.GeneralQueueList;
            }
            catch (Exception ex)
            {
                Logger.Log.Error("ServiceLogic.Queue: ", ex);
                throw;
            }
        }
        
        /// <summary> Проверяет очередь сообщений и отправляет одно из сообщений </summary>
        public bool QueueStep()
        {
            try
            {
                // next step
                var manager = new ManagerMessages(Repository);
                var res = manager.QueueStep();

                return res;
            }
            catch (Exception ex)
            {
                Logger.Log.Error("ServiceLogic.QueueStep: ", ex);
                throw;
            }
        }

        
    }
}
