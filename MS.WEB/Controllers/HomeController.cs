using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MS.Entity.Entity;
using MS.WEB.Models;
using MS.WEB.Properties;
using Newtonsoft.Json;
using RestSharp;

namespace MS.WEB.Controllers
{
    public class HomeController : Controller
    {
        private static IMapper mapper;

        public HomeController()
        {
            CreateMapper();
        }

        /// <summary>
        /// Помещает сообщение в очередь отправки сообщений
        /// </summary>
        /// <returns></returns>
        public ActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendMessage(MessageView mess)
        {
             
            if (ModelState.IsValid && !(mess.Email == false && mess.Sms == false && mess.Push == false))
            {

                var messDTO = mapper.Map<MessageView, Message>(mess);

                var baseUrl = Settings.Default.MSEndpointAddress;
                var client = new RestClient(baseUrl);
                var request = new RestRequest("SendMessage", Method.POST) {RequestFormat = DataFormat.Json};
                request.AddHeader("Accept", "application/json");

                string jsonToSend = JsonConvert.SerializeObject(messDTO);
                request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                var response = client.Execute(request);

                bool isSuccess = false;
                Boolean.TryParse(response.Content, out isSuccess);

                if (isSuccess)
                {
                    ViewBag.Mess = "Сообщение успешно поставлено в очередь.";
                }
                else
                {
                    ViewBag.Mess = "Ошибка отправки.";
                }

                return View("SendMessageComplete");
            }

            return View(mess);
        }

        /// <summary>
        /// Получить очередь сообщений
        /// </summary>
        /// <returns></returns>
        public ActionResult Queue(int page = 1)
        {
            var baseUrl = Settings.Default.MSEndpointAddress;
            var client = new RestClient(baseUrl);
            var request = new RestRequest("Queue", Method.GET);
            var response = client.Execute(request);
            var messageList = JsonConvert.DeserializeObject<List<Message>>(response.Content);
            var QueueList = mapper.Map<List<Message>, List<QueueView>>(messageList);

            int pageSize = 5; 
            IEnumerable<QueueView> phonesPerPages = QueueList.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = QueueList.Count };
            IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Queues = phonesPerPages };

            return View(ivm);
        }

        /// <summary>
        /// Проверяет очередь сообщений и отправляет одно из сообщений
        /// </summary>
        /// <returns></returns>
        public ActionResult QueueStep(bool step = false)
        {
            if (step)
            {// Отправка
                var baseUrl = Settings.Default.MSEndpointAddress;
                var client = new RestClient(baseUrl);
                var request = new RestRequest("QueueStep", Method.GET);

                bool result = false;
                Boolean.TryParse(client.Execute(request).Content, out result);

                ViewBag.NextStep = true;
                ViewBag.Result = result;
            }

            return View();
        }

        private static void CreateMapper()
        {
            if (mapper != null)
                return;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Message, QueueView>()
                // .ForMember(x => x.IsEmailSend, y => y.Ignore())
                ;

                cfg.CreateMap<MessageView, Message>()
                .ForMember(dest => dest.IsEmailSend, opt => opt.MapFrom(m => m.Email))
                .ForMember(dest => dest.IsSmsSend, opt => opt.MapFrom(m => m.Sms))
                .ForMember(dest => dest.IsPushSend, opt => opt.MapFrom(m => m.Push))
                .ForMember(x => x.State, y => y.Ignore())
                ;
            });
            config.AssertConfigurationIsValid();
            mapper = config.CreateMapper();
        }
    }
}