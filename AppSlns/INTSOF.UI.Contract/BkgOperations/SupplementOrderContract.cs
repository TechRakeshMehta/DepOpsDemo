using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    /// <summary>
    /// Contract to pass data between the layers, for the supplement order
    /// </summary>
    public class SupplementOrderContract
    {
        public List<SupplementOrderServices> lstSupplementOrderData { get; set; }

        /// <summary>
        /// Order Addition type from lkpOrderAdditionType
        /// </summary>
        public Int32 OrderAdditionTypeId { get; set; }

        /// <summary>
        /// Order Status Id from lkpOrderStatus, in case status is to be set as Paid, when total line item price is 0.
        /// </summary>
        public Int32 OrderStatusIdPaid { get; set; }


        /// <summary>
        /// Id of the user creating the order
        /// </summary>
        public Int32 CreatedById { get; set; }

        public Int16 SvcLineItemDispatchStatusId { get; set; }

        /// <summary>
        /// Primary key of lkpOrderLineItemStatu. Value is based on the Code type
        /// </summary>
        public Int32 OrderLineItemStatusId { get; set; }

        /// <summary>
        /// Primary key of ams.lkpOrderStatusType. Value is based on the Code type
        /// </summary>
        public Int32 BkgOrderStatusTypeId { get; set; }

        /// <summary>
        /// Primarky key of the Order table i.e. OrderID
        /// </summary>
        public Int32 OrderId { get; set; }

        /// <summary>
        /// Primary Key of the ams.lkpEventHistory i.e. EH_ID
        /// </summary>
        public Int32 BkgOrderEventHistoryId { get; set; }

        /// <summary>
        /// Primary Key of the ams.lkpBkgSvcGrpReviewStatusType table
        /// </summary>
        public Int32 BkgSvcGrpReviewStatusTypeId { get; set; }

        /// <summary>
        /// Lookup Text of the Review Status Type.
        /// Used to store the Event history data.
        /// </summary>
        public String BkgSvcGrpReviewStatusType { get; set; }

        /// <summary>
        /// Primary Key of the ams.lkpBkgSvcGrptatusType table
        /// </summary>
        public Int32 BkgSvcGrpStatusTypeId { get; set; }

        /// <summary>
        /// Lookup Text of the Status Type.
        /// Used to store the Event history data.
        /// </summary>
        public String BkgSvcGrpStatusType { get; set; }

        /// <summary>
        /// Primary Key of the ams.BkgorderPackageSvcGroup table i.e. OPSG_ID.
        /// Used to update the Review Status of the Service group to FirstReviewCompleted after supplement Order
        /// </summary>
        public Int32 OrderPkgSvcGroupId  { get; set; }
    }
}
