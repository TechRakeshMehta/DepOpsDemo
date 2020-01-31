using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IDataEntryDocumentDiscardReasonView
    {
        /// <summary>
        /// Get FDEQ_ID (Flat data entry docId)
        /// </summary>
        Int32 FDEQ_ID
        {
            get;
            set;
        }

        Int32 DocumentId
        {
            get;
            set;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        List<lkpDocumentDiscardReason> LstDocumentDiscradReason
        {
            set;
        }
        Int32 SelectedDiscardReasonId { get; }

        Int32 CurrentLoggedInUserID
        {
            get;
        }

        String AdditionalNotes { get; }
        String DiscardReasonText
        {
            get;
        }
            
    }
}
