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
    public interface IVendorServiceAttributeMappingView
    {
        List<VendorServiceAttributeMappingContract> VendorServiceAttributeMappingList
        {
            get;
            set;
        }

        Int32 VendorServiceMappingID
        {
            get;
            set;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        Int32 VendorServiceFieldID
        {
            get;
            set;
        }

        List<ExternalServiceAttribute> ExternalServiceAttributeList
        {
            get;
            set;
        }

        List<InternalServiceAttribute> InternalServiceAttributeList
        {
            get;
            set;
        }

        VendorServiceAttributeMappingContract ViewContract
        {
            get;
        }

        List<Entity.lkpFormatType> FormatType
        {
            get;
            set;
        }
    }
}
