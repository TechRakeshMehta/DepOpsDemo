using System.ServiceProcess;

namespace DataFeedAPIService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new DataFeed() 
            };
            ServiceBase.Run(ServicesToRun);
            //DataFeedService.StartDataFeedService();
        }
    }
}
