using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IRequiredDocumentationLoaderView
    {
        Int32? HierarchyNodeID { get; set; }

        Int32? ServiceID { get; set; }

        List<SystemDocument> AdditionalDocuments
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
        List<SysDocumentFieldMappingContract> AdditionalDocumentList
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

        //Boolean IsSubscriptionExist
        //{
        //    get;
        //    set;
        //}

        //UAT-3745
        List<SystemDocBkgSvcMapping> lstServiceDocBkgSvcMapping
        {
            get;
            set;
        }

        List<SystemDocBkgSvcMapping> ServiceDocBkgSvcMappingList
        {
            get;
            set;
        }
    }
}
