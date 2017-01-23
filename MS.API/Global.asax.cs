using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MS.BLL;
using MS.Entity.Interfaces;
using MS.DAL;

namespace MS.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public IRepository repository { get; set; }


        protected void Application_Start()
        {
            //            Database.SetInitializer(new MessageDbInitializer());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            repository = new Repository("ConnectionStringDb", false);
            Task.Factory.StartNew(mailErrorThread, repository);
        }


        Action<object> mailErrorThread = (object obj) =>
        {
            IRepository repository = (IRepository)obj;

            while (true)
            {
                try
                {
                    // throw new Exception("Ошибка отправки");
                    var manager = new ManagerMessages(repository, false);

                    while (true)
                    {
                        if (!manager.QueueStep())
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log.Error("Mail.Send: ", ex);
                }
                 Thread.Sleep(60000);
            }
        };

    }
}
