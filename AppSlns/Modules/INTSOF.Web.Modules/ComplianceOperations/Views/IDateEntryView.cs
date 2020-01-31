using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IDateEntryView
    {
        /// <summary>
        /// Id of the selected Tenant
        /// </summary>
        Int32 TenantId { get; set; }

        /// <summary>
        /// Id of the selected Subscription
        /// </summary>
        Int32 PkgSubId { get; set; }

        /// <summary>
        /// OrganizationUserId of the Applicant to whom the current document belongs to
        /// </summary>
        Int32 ApplicantId { get; set; }

        /// <summary>
        /// Data to be stored for the Form
        /// </summary>
        AdminDataEntrySaveContract SaveContract
        {
            get;
            set;
        }

        /// <summary>
        /// Id of the Current logged-in used
        /// </summary>
        Int32 CurrentUserId
        {
            get;
        }

        /// Id of the selected Document i.e.  PK of the ApplicantDocument table. 
        /// </summary>
        Int32 DocumentId
        {
            get;
            set;
        }

        /// <summary>
        ///  Document Status. 
        /// </summary>
        String DocumentStatus
        {
            get;
            set;
        }

        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
        }

        DataEntryQueueContract NextRecord
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the Context of the current view
        /// </summary>
        IDateEntryView CurrentViewContext
        {
            get;
        }

        /// <summary>
        ///  
        /// </summary>
        List<AdminDataEntryUIContract> UIContract
        {
            get;
            set;
        }

        /// <summary>
        /// Get no of impacted item count
        /// </summary>
        Int32 ImpactedItemCnt
        {
            get;
            set;
        }

        /// <summary>
        /// Get FDEQ_ID (Flat data entry docId)
        /// </summary>
        Int32 FDEQ_ID
        {
            get;
            set;
        }

        #region UAT-1608:
        /// <summary>
        /// Data to be stored for itesmSeries
        /// </summary>
        AdminDataEntrySaveContract ItemSeriesSaveContract
        {
            get;
            set;
        }

        String ErrorMessage
        {
            set;
        }
        String SuccessMessage
        {
            set;
        }
        #endregion

        #region Production Issue: Data Entry[26/12/2016]
        Int32 SelectedDiscardReasonId
        {
            get;

        }

        String DiscardReasonText
        {
            get;


        }

        List<Entity.ClientEntity.lkpDocumentDiscardReason> LstDocumentDiscradReason
        {
            set;

        }

        String AdditionalNotes
        {
            get;
        }

        /// <summary>
        /// Get Need to send email on document discard
        /// </summary>
        Boolean IsDiscardDocumentEmailNeedToSend
        {
            get;
            set;
        }

        #endregion

        Dictionary<Int32, Int32> lstRuleVoilatedItem
        {
            get;
            set;
        }
        //UAT 2695
        String DocumentName
        {
            get;
            set;
        }
        #region UAT-2742
        //UAT-2742
        List<Int32> lstPkgSubForDocDiscard
        {
            get;
            set;
        }
        Boolean IsAnySubsForSaveAndDone
        {
            get;
            set;
        }
        #endregion
    }
}
