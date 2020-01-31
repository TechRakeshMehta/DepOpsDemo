using Microsoft.Practices.Unity;
using System;
using System.ServiceModel;


namespace INTSOF.UnityHelper.Host
{
    internal class QueueServiceHost : ServiceHost
    {
        private readonly IUnityContainer _serviceContainer;
        //private ILogger queueServiceBehaviorLogger = SysXLoggerService.GetInstance().GetLogger();

        public QueueServiceHost (Type serviceType, IUnityContainer serviceContainer, params Uri [] baseAddresses)
            : base (serviceType, baseAddresses)
        {
            if (INTSOF.Utils.Extensions.IsNull (serviceContainer))
                //queueServiceBehaviorLogger.Error ("Queue Container - Incorrect Parameter Value (Null)", new ArgumentNullException ("Service Component Error Source Guid" + _serviceComponentGuid.ToString ()));
                throw new ArgumentNullException ("Queue Container - Incorrect Parameter Value (Null)");
            this._serviceContainer = serviceContainer;
        }

        protected override void OnOpening ()
        {
            if (INTSOF.Utils.Extensions.IsNull (Description.Behaviors.Find<QueueServiceBehavior> ()))
                Description.Behaviors.Add (new QueueServiceBehavior (_serviceContainer));
            base.OnOpening ();
        }
    }
}
