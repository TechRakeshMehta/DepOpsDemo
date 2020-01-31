using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IDownloadPdfView
    {
        String FilePath
        {
            get;
            set;
        }
        String PdfFileName
        {
            get;
            set;
        }
        Int32 CurrentLoggedInUserID
        {
            get;
        }

        Int32 TenantID
        {
            get;
            set;
        }
        bool IsFromModifyShipping
        {
            get;
            set;
        }
        List<Int32> OrderIDs
        {
            get;
            set;
        }

        Int32 OrgUsrID { get; }
    }
}




