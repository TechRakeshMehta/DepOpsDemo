using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public interface IDisclosureAndReleaseView
    {
        //List<String> State { get; set; }

        //List<String> Country { get; set; }

        Int32? HierarchyNodeID { get; set; }

        Int32? ServiceID { get; set; }

        List<Entity.ClientEntity.InstHierarchyRegulatoryEntityMappingDetails> RegulatoryNodeType
        {
            get;
            set;
        }

        String RegulatoryNodeIDs
        {
            get;
            set;
        }

        List<SystemDocument> DandRDocuments
        {
            get;
            set;
        }

        List<Int32> lstServiceIds
        {
            get;
            set;
        }

        String BkgServiceIds
        {
            get;
            set;
        }

        Int32 TenantID
        {
            get;
            set;
        }

        Dictionary<Int32, String> DictAttributeGroupIDs
        {
            get;
            set;
        }

        List<SysDocumentFieldMappingContract> DocumentAttributeMappingList
        {
            get;
            set;
        }
        List<SysDocumentFieldMappingContract> BkgDisclosureDocumentList
        {
            get;
            set;
        }

        List<SysDocumentFieldMappingContract> LstSpecialFields
        {
            get;
            set;
        }

        String PackageName
        {
            get;
            set;
        }
        Int32 CurrentLoggedInUserID
        {
            get;
        }

        Boolean RestrictOrderFlow
        {
            get;
            set;
        }
    }
}
