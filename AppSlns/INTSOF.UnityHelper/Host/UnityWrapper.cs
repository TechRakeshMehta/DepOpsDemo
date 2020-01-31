using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Configuration;
using System.ServiceModel.Activation;


//using INTSOF.NewQueues.Interfaces.BusinessFacadeInterface.WorkOrderService;


namespace INTSOF.UnityHelper.Host
{
    public class UnityWrapper : ServiceHostFactory
    {
        private readonly IUnityContainer _serviceContainer;

        public UnityWrapper()
        {
        }

        public UnityWrapper(String configSectionName)
        {
            _serviceContainer = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection(configSectionName); //QueueContainer
            section.Configure(_serviceContainer);
        }

        /// <summary>
        /// This method provide the instance of class which implements the interface (T) on basis of its config repoName setting 
        /// </summary>
        /// <param name="repoName">config repoName setting</param>
        /// <returns>instance of class</returns>
        public T GetUnityObject<T>(string repoName)
        {
            //if (tenantId > 0)
            //    _serviceContainer.RegisterType<T>(new InjectionConstructor(tenantId));
            T obj = _serviceContainer.Resolve<T>(repoName);
            return obj;
        }
    }
}
