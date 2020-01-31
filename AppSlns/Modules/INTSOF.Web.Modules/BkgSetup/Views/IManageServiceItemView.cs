#region NameSpaces

#region system defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Project Specific
using Entity.ClientEntity;
#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public interface IManageServiceItemView
    {
        #region Service Item
        Int32 TenantId
        {
            get;
            set;
        }
        String ServiceItemName
        {
            get;
            set;
        }
        String ServiceItemDescription
        {
            get;
            set;
        }

        Int32 ServiceItemTypeId
        {
            get;
            set;
        }

        String ServiceItemLabel
        {
            get;
            set;
        }

        List<PackageServiceItem> ListPackageServiceItem
        {
            get;
            set;
        }
        List<lkpServiceItemType> ListServiceItemType
        {
            get;
            set;
        }

        Int32 PSI_ID
        {
            get;
            set;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }
        String InfoMessage
        {
            get;
            set;
        }

        Int32 BkgPackageSvcId
        {
            get;
            set;
        }

        Int32 BackgroundServiceId
        {
            get;
            set;
        }
        Int32 AttributeGroupId
        {
            get;
            set;
        }
        Int32 GlobalFeeItemId
        {
            get;
            set;
        }
        Int32? ParentServiceItemId
        {
            get;
            set;
        }

        List<BkgSvcAttributeGroup> ListAttributeGroup
        {
            get;
            set;
        }
        List<PackageServiceItem> ListParentServiceItem
        {
            get;
            set;
        }

        List<Entity.PackageServiceItemFee> GlobalPackageServiceFeeItemList
        {
            get;
            set;
        }

        Boolean IsRequired
        {
            get;
            set;
        }

        Boolean IsSupplemental
        {
            get;
            set;
        }

        Decimal? AdditinalOccurencePrice
        {
            get;
            set;
        }

        Int32 ParentNodeId
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32? QuantityIncluded
        {
            get;
        }
        Int32 QuantityGroup
        {
            get;
        }
        Int32 ServiceItemPriceTypeId
        {
            get;
        }

        Int32? MinOccurrences
        {
            get;
        }

        Int32? MaxOccurrences
        {
            get;
        }
        #endregion
    }
}
