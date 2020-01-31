#region Namespaces

#region SystemDefined

using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System.Collections.Generic;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public class OnlinePaymentSubmissionPresenter : Presenter<IOnlinePaymentSubmissionView>
    {
        #region Variables

        #region Private Variables



        #endregion

        #region Public Variables



        #endregion

        #endregion

        #region Properties

        #region Private Properties



        #endregion

        #region Public Properties



        #endregion

        #endregion

        #region Methods

        #region Public Methods

        public override void OnViewInitialized()
        {
            View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
            View.OnlinePaymentTransactionDetails = ComplianceDataManager.GetPaymentTransactionDetails(View.InvoiceNumber, View.TenantId);
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetNextPagePathByOrderStageID(ApplicantOrderCart applicantOrderCart)
        {
            View.NextPagePath = ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.OnlinePaymentSubmission);
        }

        /// <summary>
        /// Method to get Organization User data bases on user ID
        /// </summary>
        /// <param name="CurrentLoggedInUserId"></param>
        public void getOrganizationUserDetails(int CurrentLoggedInUserId)
        {
            View.OrganizationUserData = ComplianceDataManager.GetOrganizationUserDetailByOrganizationUserId(CurrentLoggedInUserId);

        }

        #endregion

        #region Private Methods


        #endregion

        #endregion

      
    }
}





