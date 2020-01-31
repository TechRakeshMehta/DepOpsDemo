using System;
using System.Collections.Specialized;
using System.Xml;
namespace INTSOF.AuthNet.Business
{
    public interface IGateway {
        string ApiLogin { get; set; }
        string TransactionKey { get; set; }
		IGatewayResponse Send (IGatewayRequest request);
        IGatewayResponse Send(IGatewayRequest request, string description);
    }
}
