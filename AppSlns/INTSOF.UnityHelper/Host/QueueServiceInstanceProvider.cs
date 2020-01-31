using INTSOF.Utils;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;


namespace INTSOF.UnityHelper.Host
{
    public class QueueInstanceProvider : IInstanceProvider
    {
        private readonly Type _serviceType;
        private readonly IUnityContainer _serviceContainer;
        //private Guid _serviceComponentGuid = new Guid ();
        //private ILogger queueServiceBehaviorLogger = SysXLoggerService.GetInstance().GetLogger();

        public QueueInstanceProvider(IUnityContainer serviceContainer, Type serviceType)
        {
            if (Extensions.IsNull (serviceContainer))
                //queueServiceBehaviorLogger.Error ("Queue Container - Incorrect Parameter Value (Null)", new ArgumentNullException ("Service Component Error Source Guid" + _serviceComponentGuid.ToString ()));
                throw new ArgumentNullException ("Queue Container - Incorrect Parameter Value (Null)");

            if (Extensions.IsNull(serviceType))
                //queueServiceBehaviorLogger.Error ("Queue Service Type - Incorrect Parameter Value (Null)", new ArgumentNullException ("Service Component Error Source Guid" + _serviceComponentGuid.ToString ()));
                throw new ArgumentNullException ("Queue Service Type - Incorrect Parameter Value (Null)");

            this._serviceContainer = serviceContainer;
            this._serviceType = serviceType;
        }

        #region IInstanceProvider Implementation

        public object GetInstance(InstanceContext serviceInstanceContext, Message serviceMessage)
        {
            Object serviceInstance = _serviceContainer.Resolve(_serviceType);
            if (Extensions.IsNull (serviceInstance))
                //queueServiceBehaviorLogger.Error ("Queue Service Type - Incorrect Service Type Configuration", new ConfigurationErrorsException ("Service Component Error Source Guid" + _serviceComponentGuid.ToString ()));
                throw new ConfigurationErrorsException ("Queue Service Type - Incorrect Service Type Configuration");

            return _serviceContainer.Resolve(_serviceType);
        }

        public object GetInstance(InstanceContext serviceInstanceContext)
        {
            return GetInstance(serviceInstanceContext, null);
        }

        public void ReleaseInstance(InstanceContext serviceInstanceContext, object serviceInstance)
        {
            _serviceContainer.Teardown (serviceInstance);
        }

        #endregion

    }
}
