using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LPSDesktop.Logging.Interfaces;

namespace LPSFS.SYSX.WEB.SysXLoggerModel
{
    class SysXLoggingClient : ILoggingClient
    {
        public ILogger Logger { get; set; }

        public string TypeKey { get; set; }

        public string TypeName{get; set;}
    }
}
