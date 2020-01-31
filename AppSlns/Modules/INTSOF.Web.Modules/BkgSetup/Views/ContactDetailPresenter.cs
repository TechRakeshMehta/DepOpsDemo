using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using Entity.ClientEntity;

namespace CoreWeb.BkgSetup.Views
{
    public class ContactDetailPresenter : Presenter<IContactDetailView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// To save/insert NodeDeadline
        /// </summary>
        ///// <param name="nodeDeadline"></param>
        public Boolean SaveContact()
        {
            InstitutionContact institutionContactDetail = new InstitutionContact();
            institutionContactDetail.ICO_FirstName = View.FirstName;
            institutionContactDetail.ICO_LastName = View.Lastname;
            institutionContactDetail.ICO_Title = View.Title;
            institutionContactDetail.ICO_PrimaryPhone = View.PrimaryPhone;
            institutionContactDetail.ICO_PrimaryEmailAddress = View.PrimaryEmailAddress;
            institutionContactDetail.ICO_Address1 = View.Address1;
            institutionContactDetail.ICO_Address2 = View.Address2;
            institutionContactDetail.ICO_ZipCodeID = View.ZipCodeId;
            institutionContactDetail.ICO_IsDeleted = false;
            institutionContactDetail.ICO_ModifiedOn = DateTime.Now;
            institutionContactDetail.ICO_ModifiedByID = View.CurrentLoggedInUserId;
            return BackgroundSetupManager.UpdateContact(institutionContactDetail, View.InstitutionContactId, View.SelectedTenantId, View.HierarchyNodeID, true);
        }

        /// <summary>
        /// Get the Contact record from the InstitutionContact Table
        /// </summary>
        public void GetContactData()
        {
            InstitutionContact institutionContact = BackgroundSetupManager.GetInstitutionContactList(View.SelectedTenantId, View.InstitutionContactId);
            if (institutionContact != null)
            {
                View.FirstName = institutionContact.ICO_FirstName;
                View.Lastname = institutionContact.ICO_LastName;
                View.Title = institutionContact.ICO_Title;
                View.PrimaryPhone = institutionContact.ICO_PrimaryPhone;
                View.PrimaryEmailAddress = institutionContact.ICO_PrimaryEmailAddress;
                View.Address1 = institutionContact.ICO_Address1;
                View.Address2 = institutionContact.ICO_Address2;
                View.ZipCodeId = institutionContact.ICO_ZipCodeID;
            }
        }
    }
}
