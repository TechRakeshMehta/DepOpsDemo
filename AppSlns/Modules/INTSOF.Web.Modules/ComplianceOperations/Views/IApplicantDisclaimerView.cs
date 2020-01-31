using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IApplicantDisclaimerView
    {
        Int32 CurrentLoggedInUserID
        {
            get;
        }
        Int32 TenantId
        {
            get;
        }
        String PackageName
        {
            get;
            set;
        }
        Int32 DPP_ID
        {
            get;
            set;
        }  
        
        /// <summary>
        /// Used to identify the current Order Request Type i.e. New order, Change subscription etc.
        /// </summary>
        String OrderType
        {
            get;
            set;
        }

        #region UAT-4592
        Int32 DisclaimerDocumentSystemDocumentID
        {
            get;
            set;
        }        

        Boolean SystemDocumentIsDeleted
        {
            get;
            set;
        }

        String DocumentPath
        {
            get;
            set;
        }
        #endregion
    }
}
