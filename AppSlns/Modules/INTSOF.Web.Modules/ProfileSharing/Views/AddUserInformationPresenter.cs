using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.SharedDataEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

namespace CoreWeb.ProfileSharing.Views
{
    public class AddUserInformationPresenter : Presenter<IAddUserInformationView>
    {
        public void SaveUser()
        {
            SharedUserReviewQueue sharedUserReviewQueue = new SharedUserReviewQueue();
            sharedUserReviewQueue.SURQ_FirstName = View.FirstName;
            sharedUserReviewQueue.SURQ_LastName = View.LastName;
            sharedUserReviewQueue.SURQ_EmailId = View.EmailAddress;
            sharedUserReviewQueue.SURQ_Phone = View.Phone;
            sharedUserReviewQueue.SURQ_Title = View.Title;
            sharedUserReviewQueue.SURQ_Note = View.Note;
            sharedUserReviewQueue.SURQ_AgencyId = View.AgencyID;
            sharedUserReviewQueue.SURQ_TenantId = View.TenantID;
            sharedUserReviewQueue.SURQ_IsDeleted = false;
            sharedUserReviewQueue.SURQ_CreatedById = View.CurrentLoggedInUserId;
            sharedUserReviewQueue.SURQ_CreatedOn = DateTime.Now;
            sharedUserReviewQueue.SURQ_StatusId = GetSharedUserReviewStatusID();

            View.SaveStatus = ProfileSharingManager.SaveSharedUserForReview(sharedUserReviewQueue);
        }

        private int GetSharedUserReviewStatusID()
        {
            String code = SharedUserReviewStatus.NEW.GetStringValue();
            return ProfileSharingManager.GetSharedUserReviewStatusType().Where(cond => cond.SURS_Code.ToLower() == code.ToLower() && !cond.SURS_IsDeleted)
                                                                        .Select(col => col.SURS_ID).FirstOrDefault();
        }

        public List<Int32> AnyAgencyUserExists(int clientID, string agencyIds)
        {
            return ProfileSharingManager.AnyAgencyUserExists(clientID, agencyIds);
        }

        public List<ClinicalRotationAgencyContract> GetAgenciesMappedWithRotation(Int32 selectedTenantID, Int32 rotationId)
        {
            return ClinicalRotationManager.GetAgenciesMappedWithRotation(selectedTenantID, rotationId);
        }

    }
}
