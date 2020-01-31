using INTSOF.UI.Contract.BkgOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    /// <summary>
    /// Stores data related to the Supplement Background Order
    /// </summary>
    [Serializable]
    public class SupplementOrderCart
    {
        /// <summary>
        /// Primary key of the Order table
        /// </summary>
        public Int32 MasterOrderId { get; set; }

        public List<SupplementOrderData> lstSupplementOrderData { get; set; }

        /// <summary>
        /// ID of the OrderPackageServiceGroup, for which the Supplement Order is being placed. 
        /// </summary>
        public Int32 OrdPkgSvcGroupId { get; set; }

        /// <summary>
        /// Parent Screen Name
        /// </summary>
        public String ParentScreen { get; set; }

        //New Change 21072016
        public List<SupplementServiceItemCustomForm> lstCustomFormLst { get; set; }
        public List<Int32> LstSupplementServiceId { get; set; }
    }

    [Serializable]
    public class SupplementOrderData
    {
        /// <summary>
        /// Primary Key of ams.BackgroundService table i.e. BSE_ID 
        /// </summary>
        public Int32 BkgServiceId { get; set; }

        /// <summary>
        ///  Primary Key of ams.PackageServiceItem i.e. PSI_ID
        /// </summary>
        public Int32 PackageSvcItemId { get; set; }

        /// <summary>
        /// BSAD_ID - PK of ams.BkgSvcAttributeGroup in Tenant database
        /// </summary>
        public Int32 BkgSvcAttributeGroupId { get; set; }

        /// <summary>
        /// Id of Particular Instance of a repeatable Group
        /// </summary>
        public Int32 InstanceId { get; set; }

        public Int32 CustomFormId { get; set; }

        /// <summary>
        /// Actual data of the Forms, entered by the applicant
        /// KEY - BAGM_ID - PK of ams.BkgAttributeGroupMapping in client database
        /// VALUE - Attribute Value
        /// </summary>
        public Dictionary<Int32, String> FormData { get; set; }

        /// <summary>
        /// KEY - BPS_ID - PK of ams.BkgPackageService in client database
        /// </summary>
        public Int32 PackageServiceId { get; set; }
    }
}

