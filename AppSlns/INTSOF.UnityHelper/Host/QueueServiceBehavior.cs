using INTSOF.Logger.factory;
using INTSOF.Utils;
using Microsoft.Practices.Unity;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;


namespace INTSOF.UnityHelper.Host
{
    internal class QueueServiceBehavior : IServiceBehavior
    {
        private readonly IUnityContainer _serviceContainer;
        //private Guid _serviceComponentGuid = new Guid();
        private INTSOF.Logger.ILogger queueServiceBehaviorLogger = SysXLoggerFactory.GetInstance().GetLogger();
        public QueueServiceBehavior (IUnityContainer serviceContainer)
        {
            if (Extensions.IsNull (serviceContainer))
                //queueServiceBehaviorLogger.Error("Queue Service Behavior - Incorrect Parameter Value (Null)", new ArgumentNullException ("Service Component Error Source Guid" + _serviceComponentGuid.ToString()));
                throw new ArgumentNullException ("Queue Service Behavior - Incorrect Parameter Value (Null)");
             
            _serviceContainer = serviceContainer;
        }

        #region Service behavior Implementation

        public void AddBindingParameters (ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, 
                                            Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            throw new NotImplementedException ();
        }

        public void ApplyDispatchBehavior (ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            if (Extensions.IsNull (serviceDescription))
                //queueServiceBehaviorLogger.Error("Queue Service Description - Incorrect Parameter Value (Null)", new ArgumentNullException ("Service Component Error Source Guid" + _serviceComponentGuid.ToString()));
                throw new ArgumentNullException ("Queue Service Description - Incorrect Parameter Value (Null)");

            if (Extensions.IsNull(serviceHostBase))
               //queueServiceBehaviorLogger.Error("Queue Service Host - Incorrect Parameter Value (Null)", new ArgumentNullException ("Service Component Error Source Guid" + _serviceComponentGuid.ToString()));
                throw new ArgumentNullException ("Queue Service Description - Incorrect Parameter Value (Null)");
           
            for (Int32 dispatcherIndex = 0; dispatcherIndex < serviceHostBase.ChannelDispatchers.Count; dispatcherIndex++)
            {
                ChannelDispatcherBase dispatcher = serviceHostBase.ChannelDispatchers[dispatcherIndex];
                ChannelDispatcher channelDispatcher = (ChannelDispatcher)dispatcher;

                for (Int32 endpointIndex = 0; endpointIndex < channelDispatcher.Endpoints.Count; endpointIndex++)
                {
                    EndpointDispatcher endpointDispatcher = channelDispatcher.Endpoints [endpointIndex];
                    endpointDispatcher.DispatchRuntime.InstanceProvider = new QueueInstanceProvider (_serviceContainer, serviceDescription.ServiceType);
                }
            }
        }

        public void Validate (ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            throw new NotImplementedException ();
        }

        #endregion
    }
}
