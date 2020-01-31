using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IPackageSelectionForDataEntryView
    {
        /// <summary>
        /// This will contain Aplicant TenantID of the type, based on the scenarios
        /// 1. If the current document had only one subscription, this will contain the TenantID of the 'Next Record'
        /// 2. If the current document had multiple subscriptions, then it will have the TenantID of the 'same document record'. 
        /// In that case, NextRecordTenantId and NextRecordApplicantId are used to get the data of the Next record in queue. 
        /// </summary>
        Int32 SelectedTenantID { get; set; }

        /// <summary>
        /// This will contain Aplicant OrganizationID of the type, based on the scenarios
        /// 1. If the current document had only one subscription, this will contain the ApplicantID of the 'Next Record'
        /// 2. If the current document had multiple subscriptions, then it will have the ApplicantID of the 'same document record'. 
        /// In that case, NextRecordTenantId and NextRecordApplicantId are used to get the data of the Next record in queue. 
        /// </summary>
        Int32 ApplicantOrganizationUserID { get; set; }

        List<PackageSubscriptionForDataEntry> lstPackageSubscription
        {
            set;
            get;
        }

        /// <summary>
        /// Represents the property to decide whether the Same document subscriptions are 
        /// being displayed when the user has Selected 'Save & Done' from dEtails screen.
        /// This will be false 
        /// 1. When the screen is opened from the Queue 
        /// 2. There was only one susbcription for the Document which was opened 
        /// 3. There were multiple susbcriptions for the Document which was opened but user selected 'Go to Next Document'
        /// </summary>
        Boolean IsSameDocument
        {
            get;
            set;
        }

        /// <summary>
        ///  Represents the SubscriptionId of the applicant for whom multiple subscriptions exist.
        ///  Will be available, only - When the 'Save and Done' is clicked from the Details screen and Applicant has more subscriptions apart from current one.
        /// </summary>
        Int32 CurrentSubscriptionId
        {
            get;
            set;
        }

        /// <summary>
        /// Will be available only from the Details screen and that too if the document had multiple subscriptions
        /// and user selects 'Save and Done'
        /// </summary>
        Int32 NextRecordTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Will be available only from the Details screen and that too if the document had multiple subscriptions
        /// and user selects 'Save and Done'
        /// </summary>
        Int32 NextRecordApplicantId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the status of the document to be updated in the database
        /// </summary>
        String DocumentStatus
        {
            get;
            set;
        }

        /// <summary>
        /// ÒrganizationUserID of the CurrentLogged in user
        /// </summary>
        Int32 CurrentUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        Int32 DocumentId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the 'FlatDataEntryQueue' PK for the Document which was opened.
        /// Will always contain the FdeqId of the current document
        /// </summary>
        Int32 FDEQId
        {
            get;
            set;
        }

        /// <summary>
        /// Data to track the Data entry Productivity
        /// </summary>
        DataEntryTrackingContract DataEntryTimeTracking
        {
            get;
            set;
        }

        /// <summary>
        /// DocumentStatusID to be used for time tracking
        /// </summary>
        short DocumentStatusId
        {
            get;
            set;
        }
        Boolean IsDiscardDocument
        {
            get;
            set;
        }
    }
}
