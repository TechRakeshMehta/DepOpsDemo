using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.RepoManagers
{
   public class CBIBillingStatusManagers
    {
       public static List<CBIBillingStatusContract> GetCBIBillingStatus(CustomPagingArgsContract customPagingArgsContract, CBIBillingStatusContract CBIBillingStatusContract)
       {
           try
           {
               return BALUtils.GetFingerPrintClientRepoInstance(CBIBillingStatusContract.TenantId).GetCBIBillingStatus(customPagingArgsContract, CBIBillingStatusContract);
           }
           catch (SysXException ex)
           {
               BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
               throw ex;
           }
           catch (Exception ex)
           {
               BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
               throw (new SysXException(ex.Message, ex));
           }
       }

       public static Boolean SaveCBIBillingStatus(CBIBillingStatusContract CBIBillingStatusContract, int currentLoggedInUserId)
       {
           try
           {
               return BALUtils.GetFingerPrintClientRepoInstance(CBIBillingStatusContract.TenantId).SaveCBIBillingStatus(CBIBillingStatusContract, currentLoggedInUserId);
           }
           catch (SysXException ex)
           {
               BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
               throw ex;
           }
           catch (Exception ex)
           {
               BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
               throw (new SysXException(ex.Message, ex));
           }
       }
    }
}
