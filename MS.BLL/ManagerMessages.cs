using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Monads.NET;
using MS.Entity.Entity;
using MS.Entity.Interfaces;

namespace MS.BLL
{
    public class ManagerMessages
    {
        private readonly IRepository Repository = null;
        //        private IRepository Repository { get; set; }

        /// <summary> Если очередь уже проинициализирована первым запуском </summary>
        private static bool isGeneralQueueInitialize = false;
        /// <summary> Генеральная очередь - для потоков отправки </summary>
        private static Queue<Message> generalQueue = new Queue<Message>();

        /// <summary> Лист очереди для внешнего чтения </summary>
        private static List<Message> generalQueueList = null;
        public static List<Message> GeneralQueueList { get { return generalQueueList; } }

        /// <summary> Очередь потока обслуживающего повторную(предыдущие отправки с ошибками) отправку </summary>
        private static Queue<Message> errorQueue = new Queue<Message>();

        private bool isUseGeneralQueue { get; set; }

        #region : constructor
        private ManagerMessages() { }
        public ManagerMessages(IRepository repository, bool isUseGeneralQueue = true)
        {
            this.isUseGeneralQueue = isUseGeneralQueue;
            this.Repository = repository;

            if (isUseGeneralQueue) // Обслуживаем генеральную очередь
            {
                object one = new object();
                lock (one)
                {
                    // Инициализируем генеральную очередь при первой загрузке из БД
                    if (!isGeneralQueueInitialize)
                    {
                        // Загрузка из БД генеральной очереди
                        var messages = repository.Messages()
                            .Where(x => x.State == StateType.ToSend)
                            .OrderBy(x => x.Id).ToList();
                        foreach (var mess in messages)
                        {
                            generalQueue.Enqueue(mess);
                        }

                        // В UI не показываем успешные
                        generalQueueList = messages.Where(x => x.State != StateType.Success).ToList();
                        isGeneralQueueInitialize = true;
                    }
                }
            }
            else
            {// Для потока-таймер повторной отправки
                // Загрузка из БД ошибочной очереди
                var messages = repository.Messages()
                    .Where(x => x.State == StateType.Error)
                    .OrderBy(x => x.Id).ToList();
                foreach (var mess in messages)
                {
                    errorQueue.Enqueue(mess);
                }
            }
        }
        #endregion : constructor

        /// <summary>
        /// Помещает сообщение в очередь отправки сообщений
        /// </summary>
        public static void SendMessage(Message mess)
        {
            generalQueue.Enqueue(mess);
            generalQueueList.Add(mess);
        }

        /// <summary> 
        /// Проверяет очередь сообщений и отправляет одно из сообщений 
        /// </summary>
        public bool QueueStep()
        {
            Message message = null;
            if (isUseGeneralQueue) // Обслуживаем генеральную очередь
            {
                if (generalQueue.Count == 0)
                    return false;

                message = generalQueue.Dequeue();
            }
            else
            {// Для потока-таймер повторной отправки
                if (errorQueue.Count == 0)
                    return false;

                message = errorQueue.Dequeue();
            }

            if (message == null)
                return false;


            // send to mail
            var managerMail = new ManagerMail();
            message = managerMail.Send(message);


            // update status db
            if (message.IsEmailSend == false && message.IsSmsSend == false && message.IsPushSend == false)
            {// success
                message.State = StateType.Success;
                if (Repository == null)
                    return false;
                Repository.MessageEdit(message);
                // В UI не показываем эту запись
                generalQueueList = generalQueueList.Where(x => x.Id != message.Id).ToList();
                //                generalQueueList = generalQueueList.Where(x => x.State != StateType.Success).ToList();
                return true;
            }
            else
            {// error
                message.State = StateType.Error;
//                if (Repository == null)
//                    return false;
                Repository.MessageEdit(message);
                // В UI не показываем успешные
//                generalQueueList = generalQueueList.Where(x => x.Id != message.Id).ToList();
                return false;
            }
        }

    }
}
