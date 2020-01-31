#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract;
#endregion


namespace CoreWeb.BkgSetup.Views
{
    public interface ISetUpAttributeGroupForServicesView
    {
        Int32 TenantId
        {
            get;
            set;
        }
        Int32 BackgroundServiceId
        {
            get;
        }
        Int32 BackgroundServiceGroupId
        {
            get;
        }

        Int32 BackgroundPackageId
        {
            get;
        }

        List<AttributeSetupContract> MappedAttributeGroupList
        {
            get;
            set;
        }
        String ServiceName
        {
            get;
            set;

        }
        String DisplayName
        {
            get;
            set;

        }

        String Notes
        {
            get;
            set;

        }

        Int32? PkgCount
        {
            get;
            set;
        }

        Int32? ResidenceDuration
        {
            get;
            set;
        }

        Int32? MinOccurrences
        {
            get;
            set;
        }

        Int32? MaxOccurrences
        {
            get;
            set;
        }

        Boolean SendDocsToStudent
        {
            get;
            set;
        }

        Boolean IsSupplemental
        {
            get;
            set;
        }

        Boolean IgnoreRHOnSupplement
        {
            get;
            set;
        }


        BkgPackageSvc CurrentBkgPackageSvc
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        String ErrorMessage { get; set; }

        String SuccessMessage { get; set; }

        String InfoMessage { get; set; }

        Boolean IsReportable
        {
            get;
            set;
        }

        //UAT-3109
        String AMERNumber
        {
            get;
            set;
        }
    }
}
