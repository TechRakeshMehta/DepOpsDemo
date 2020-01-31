using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public interface IVendorServiceMappingView
    {
        String ErrorMessage
        {
            get;
            set;
        }

        List<VendorServiceMappingContract> VendorServiceMappingList
        {
            get;
            set;
        }

        Int32 VendorServiceMappingID
        {
            get;
            set;
        }

        List<BackgroundService> BackgroundServiceList
        {
            get;
            set;
        }

        List<ExternalVendor> ExternalVendorList
        {
            get;
            set;
        }

        List<ExternalBkgSvc> ExternalBkgServiceList
        {
            get;
            set;
        }

        Int32 VendorID
        {
            get;
            set;
        }

        VendorServiceMappingContract ViewContract
        {
            get;
        }        
    }
}
