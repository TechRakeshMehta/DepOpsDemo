using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.SystemSetUp.Views
{
    public interface IInstitutionConfigurationGlobalFeeDetailsView
    {
        Int32 SelectedFeeItemId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and set List of ServiceItemFeeType
        /// </summary>
        List<ServiceFeeItemRecordContract> ListServiceItemFeeRecordContract
        {
            set;
            get;
        }

        String FeeTypeCode
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }

        Int32 DeptProgramMappingID
        {
            get;
            set;
        }

        Int32 PackageID
        {
            get;
            set;
        }

        Int32 PackageHierarchyID
        {
            get;
            set;
        }
    }
}
