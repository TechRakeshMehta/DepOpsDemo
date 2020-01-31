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
    public interface IEditServiceItemDetailView
    {
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

        //String ServiceItemLabel
        //{
        //    get;
        //    set;
        //}

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
            set;
        }
        List<PackageServiceItem> ListParentServiceItem
        {
            set;
        }

        List<Entity.PackageServiceItemFee> GlobalPackageServiceFeeItemList
        {
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

        Int32 BPHM_Id
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
            set;
        }
        Int32? QuantityGroupId
        {
            get;
            set;
        }
        Int32 ServiceItemPriceTypeId
        {
            get;
            set;
        }

        List<lkpServiceItemPriceType> ServiceItemPriceTypes
        {
            set;
        }

        List<PackageServiceItem> QuantityGroups
        {
            set;
            get;
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

        Boolean ifQuantityGrpEditable
        {
            get;
            set;
        }
        Boolean IsStateSearchRuleExists
        {
            get;
            set;
        }
    }
}
