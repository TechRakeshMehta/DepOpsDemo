using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract;

namespace CoreWeb.BkgSetup.Views
{
    public interface ISetUpAttributeForAttributeGroupView
    {

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 BkgPackageSvcId
        {
            get;
            set;
        }

        Int32 AttributeGroupId
        {
            get;
        }
        Int32 BackgroundPackageId
        {
            get;
        }

        Boolean IsServiceSystemDefined
        {
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        String ErrorMessage { get; set; }

        String SuccessMessage { get; set; }

        String InfoMessage { get; set; }

        List<AttributeDataSecurityClient> AttributeList
        {
            set;
        }
        List<Int32> MappedAttributeIds
        {
            get;
            set;
        }
        List<AttributeSetupContract> MappedAttributeList
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

        Int32 SelectedAttributeId
        {
            get;
            set;
        }

        List<lkpSvcAttributeDataType> listAttributeDataType
        {
            set;
        }
        Boolean IsRequired
        {
            get;
        }

        Boolean IsDisplay
        {
            get;
        }
        Boolean IsHiddenFromUI
        {
            get;
        }
    }
}
