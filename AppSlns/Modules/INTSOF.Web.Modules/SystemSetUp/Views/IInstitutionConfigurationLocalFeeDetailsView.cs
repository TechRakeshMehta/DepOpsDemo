using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.SystemSetUp;

namespace CoreWeb.SystemSetUp.Views
{
    public interface IInstitutionConfigurationLocalFeeDetailsView
    {
        IInstitutionConfigurationLocalFeeDetailsView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 DeptProgramMappingID { get; set; }
        Int32 PackageID { get; set; }
        /// <summary>
        /// Gets and set List of ServiceItemFeeType
        /// </summary>
        List<LocalFeeRecordsInfo> ListServiceItemFeeRecord
        {
            set;
            get;
        }

        Int32 PackageServiceItemFeeID
        {
            get;
            set;
        }

        String FeeItemName
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



