using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using INTSOF.Utils;

namespace INTSOF.Complio.API.Core
{
    public enum ExceptionType
    {
        SqlExceptionType = 1000,
        TimeOutException = 1001,
        XmlException = 1002,
        ReportServerSoapException = 1003,
        SqlCustomExceptionType = 50000
    }

    public enum EntityType
    {
        [StringValue("AAAA")]
        GetUserData,
        [StringValue("AAAB")]
        GetTrackingPackageByOrderId,
        [StringValue("AAAC")]
        GetScreeningPackagesByOrderIds
    }

}