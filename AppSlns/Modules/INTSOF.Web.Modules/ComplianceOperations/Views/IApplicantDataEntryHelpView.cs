using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IApplicantDataEntryHelpView
    {
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
    }
}




