using System;
using System.Collections.Generic;

namespace Entity.ExternalVendorContracts
{
    [Serializable]
    public class EvUpdateOrderContract
    {
        #region OrderProperties

        public String AccountNumber
        {
            get;
            set;
        }

        public String LoginName
        {
            get;
            set;
        }

        public String Password
        {
            get;
            set;
        }

        public Int32 BusinessOwnerID
        {
            get;
            set;
        }

        public Int32 BkgOrderID
        {
            get;
            set;
        }

        public String BkgOrderVendorProfileID
        {
            get;
            set;
        }

        public String OrderPreviousStatusTypeCode
        {
            get;
            set;
        }

        public String OrderNewStatusTypeCode
        {
            get;
            set;
        }

        public Int32? OrderStatusTypeID
        {
            get;
            set;
        }

        /* commented Code For [UAT-844]
        public Boolean NeedsFirstReview
        {
            get;
            set;
        }

       
        public Int16? PackageSupplementalTypeID
        {
            get;
            set;
        }

        public String PackageSupplementalTypeCode
        {
            get;
            set;
        }*/

        public Boolean BkgOrderFlaggedInd
        {
            get;
            set;
        }

        public Int32 ExternalVendorBkgOrderDetailID
        {
            get;
            set;
        }

        #endregion

        public List<EvUpdateOrderItemContract> EvUpdateOrderItemContract
        {
            get;
            set;
        }

        //Commented as per ticket UAT-1244 (As for each External Vendor Order there is going to be only one svcGroup)
        //public List<EvUpdateOrderPackageSvcGroup> EvUpdateOrderPackageSvcGroup
        //{
        //    get;
        //    set;
        //}

        public EvUpdateOrderPackageSvcGroup EvUpdateOrderPackageSvcGroup
        {
            get;
            set;
        }

        public VendorResponse VendorResponse
        {
            get;
            set;
        }

        //UAT-1244
        public String VendorProfileStatus
        {
            get;
            set;
        }

        //UAT-844:Order Review Enhanchments
        public Boolean IsAutoReviewComplete
        {
            get;
            set;
        }
    }

    [Serializable]
    public class EvUpdateOrderItemContract
    {
        public String ExternalBackgroundServiceCode
        {
            get;
            set;
        }

        public Int32 BkgOrderPackageSvcLineItemID
        {
            get;
            set;
        }

        public String ExternalVendorOrderID
        {
            get;
            set;
        }

        public DateTime? DateCompleted
        {
            get;
            set;
        }

        public String ResultText
        {
            get;
            set;
        }

        public String ResultXML
        {
            get;
            set;
        }

        public Boolean SvcLineItemFlaggedInd
        {
            get;
            set;
        }

        public Int32 OrderLineItemResultStatusID
        {
            get;
            set;
        }

        public Int32 ExtVendorBkgOrderLineItemDetailID
        {
            get;
            set;
        }

        public VendorResponse VendorResponse
        {
            get;
            set;
        }

        public Int32 BkgOrderPackageServiceGroupID
        {
            get;
            set;
        }
    }

    [Serializable]
    public class EvUpdateOrderPackageSvcGroup
    {
        //UAT-844:Order Review enhancements

        public Int32 BkgSvcGroupID
        {
            get;
            set;
        }

        public Int32 BkgOrderPackageServiceGroupID
        {
            get;
            set;
        }

        public Boolean FirstReviewTrigger
        {
            get;
            set;
        }

        public Boolean SecondReviewTrigger
        {
            get;
            set;
        }

        public String BkgSvcGroupName
        {
            get;
            set;
        }

        public String ServiceGroupPreviousReviewStatusCode
        {
            get;
            set;
        }

        public String ServiceGroupStatusCode
        {
            get;
            set;
        }

        public Boolean ServiceGroupFlaggedInd
        {
            get;
            set;
        }

        public String ServiceGroupNewReviewStatusCode
        {
            get;
            set;
        }

        public Int16? PackageSupplementalTypeID
        {
            get;
            set;
        }

        public String PackageSupplementalTypeCode
        {
            get;
            set;
        }

        public Boolean NeedsFirstReview
        {
            get;
            set;
        }
    }
}
