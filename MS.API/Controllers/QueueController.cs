using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Practices.Unity;
using MS.API.Repositories;
using MS.DAL;
using MS.Entity.Entity;
using MS.Entity.Interfaces;

namespace MS.API.Controllers
{
    public class QueueController : ApiController
    {
        [Dependency]
        public IRepository Repository { get; set; }

        [Dependency]
        public IServiceLogic ServiceLogic { get; set; }

        /// <summary>
        /// Помещает сообщение в очередь отправки сообщений
        /// </summary>
        [HttpPost, Route("SendMessage")]
//        public IEnumerable<Message> SendMessage (Message mess)
        public bool SendMessage (Message mess)
        {
            var res = ServiceLogic.SendMessage(mess);
            return res;
        }

        /// <summary> Получить очередь сообщений </summary>
        [HttpGet, Route("Queue")]
        public List<Message> Queue()
        {
            var res = ServiceLogic.Queue();
            return res;
        }

        /// <summary>
        /// Проверяет очередь сообщений и отправляет одно из сообщений
        /// </summary>
        [HttpGet, Route("QueueStep")]
        public bool QueueStep ()
        {
            var res = ServiceLogic.QueueStep();
            return res;
        }

    }
}
