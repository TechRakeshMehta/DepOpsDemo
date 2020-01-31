using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.PlacementMatching;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;

namespace CoreWeb.PlacementMatching.Views
{
    public interface ISharedCustomAttributesView
    {
        Int32 CurrentLoggedInUserID { get; }
        List<SharedCustomAttributesContract> lstSharedCustomAttributes { get; set; }
        SharedCustomAttributesContract SharedCustomAttributes { get; set; }
        Int32 SelectSharedCustomAttributeID { get; set; }
        Int32 SelectSharedCustomAttributeMappingID { get; set; }
        List<lkpCustomAttributeDataType> lstAttributeDataType { get; set; }
        List<lkpSharedCustomAttributeUseType> lstAttributeUseType { get; set; }
        List<AgencyHierarchyContract> lstAgencyRootNodes { get; set; }
        Int32 SelectedAgencyRootNodeID { get; set; }
        Guid UserId { get; }
        Boolean IsAgencyUserLoggedIn { get; }
    }
}
