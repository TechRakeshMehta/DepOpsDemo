using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace INTSOF.Utils.SPI.PDFConversion
{
    [ServiceContract]
    public interface IPDFConversion
    {
        [OperationContract]
        byte[] GeneratePDF(string url);
    }
}
