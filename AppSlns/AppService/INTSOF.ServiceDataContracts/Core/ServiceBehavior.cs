
using INTSOF.ServiceDataContracts.Core;
using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace INTSOF.ServiceDataContracts.Modules
{
    public class ServiceBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint,
        BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint,
        ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new MessageInspector());
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint,
        EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(
        ServiceEndpoint endpoint)
        {
        }

        #endregion

        public override Type BehaviorType
        {
            get
            {
                return typeof(ServiceBehavior);
            }
        }

        protected override object CreateBehavior()
        {
            return new ServiceBehavior();
        }
    }

    public class InspectorBehaviorExtension : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new ServiceBehavior();
        }

        public override Type BehaviorType
        {
            get { return typeof(ServiceBehavior); }
        }
    }

}
