using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IManageDownloadPdfView
    {
        #region Properties

        String FileIdentifier
        {
            get;
            set;
        }
        Int32 TenantID
        {
            get;
            set;
        }
        Int32 PackageID
        {
            get;
            set;
        }
        String PageHTML
        {
            get;
            set;
        }
        Int32 CurrentLoggedInUserID
        {
            get;
        }
        String PackageName
        {
            get;
            set;
        }
        #endregion
    }
}




