using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LPSFS.SYSX.WEB.SysXExceptionModel
{
    public interface ISysXMessageFormattor
    {
        #region ApplicationInformation
        string ApplicationDomain { get; set; }
        string TrustLevel { get; set; }
        string ApplicationVirtualPath { get; set; }
        string AppliactionPath { get; set; }
        string MachineName { get; set; }
        #endregion

        #region Events
        string EventCode { get; set; }
        string EventMessage { get; set; }
        string EventTime { get; set; }
        string EventSequence { get; set; }
        string EventOccurrence { get; set; }
        string EventDetailCode { get; set; }
        #endregion


        #region ProcessInformation
        string ProcessId { get; set; }
        string ProcessName { get; set; }
        string AccountName { get; set; }
        #endregion

        #region ExceptionInformation
        string ExceptionType { get; set; }
        string ExceptionMessage { get; set; }
        string Severity { get; set; }
        #endregion

        #region RequestInformation
        string RequestUrl { get; set; }
        string RequestPath { get; set; }
        string UserHostAddress { get; set; }
        string IsAuthenticated { get; set; }
        string AuthenticationType { get; set; }
        string ThreadAccountName { get; set; }
        #endregion

        #region ThreadInformation
        string ThreadId { get; set; }
        
        string IsImpersonating { get; set; }
        string StackTrace { get; set; }
        #endregion

        StringBuilder Format(Exception ex);
    }
}
