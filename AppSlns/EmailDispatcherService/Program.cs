using System.ServiceProcess;

namespace EmailDispatcherService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //         ServiceBase[] ServicesToRun;
            //         ServicesToRun = new ServiceBase[] 
            //{ 
            //	new EmailDispatcherService() 
            //};
            //         ServiceBase.Run(ServicesToRun);
            CategoryComplianceRqd.NotificationUpcomingNonComplianceRequiredCategoryAction();
            ScheduleAction.SendMailForNonComplianceCategories();
        }

//        static void Main()
//        {
//#if(!DEBUG)
//            ServiceBase[] ServicesToRun;
//            ServicesToRun = new ServiceBase[] 
//            { 
//                new EmailDispatcherService() 
//            };
//            ServiceBase.Run(ServicesToRun);
//#else
//            EmailDispatcherService obj = new EmailDispatcherService();
//            obj.complianceTimer_Elapsed(null, null);
//#endif
//        }
    }
}
