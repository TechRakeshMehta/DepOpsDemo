using INTSOF.Utils;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace INTSOF.ServiceDataContracts.Core
{
    public static class RequestData
    {
        [ThreadStatic]
        public static UserContext ActiveUser;
    }
    public class MessageInspector : IDispatchMessageInspector, IClientMessageInspector
    {
        #region IDispatchMessageInspector

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            //Retrieve Inbound Object from Request 
            //If needed can be implemented 
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            //Retrieve Inbound Object from Request 
            var header = request.Headers.GetHeader<UserContext>("UserContext", "s");
            if (header != null)
            {
                OperationContext.Current.IncomingMessageProperties.Add("UserContext", header);
            }
            return null;
        }

        #endregion


        #region IClientMessageInspector

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            //Instantiate new HeaderObject with values from ClientContext; 
            if (RequestData.ActiveUser != null)
            {
                var typedHeader = new MessageHeader<UserContext>(RequestData.ActiveUser);
                var untypedHeader = typedHeader.GetUntypedHeader("ActiveUser", "s");
                request.Headers.Add(untypedHeader);
            }
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            //Instantiate new HeaderObject with values from ClientContext; 
            //if needed can be implemented 
        }

        #endregion
    }
}

