using CoreWeb.Tickets.Views;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.TicketsCentre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract.FingerPrintSetup;

namespace CoreWeb.Tickets
{
    public class ManageTicketsPresenter : Presenter<IManageTicketsView>
    {
        public override void OnViewLoaded()
        {
            //CheckIfUserIsEnroller();
            GetClientList();
            GetSeverity();
            GetStatus();
            GetTicketType();
            GetLocationList();
            //GetClientServicesList();
        }
        private void GetClientList()
        {
            View.lstTenant = SecurityManager.GetListOfTenantWithLocationService();
        }
        //Commented BY [SS]
        //private void GetClientServicesList()
        //{
        //    View.lstServices = ServiceStepManager.GetClientServices(View.SelectedClientID, View.LobId);
        //    //ServiceStepManager.GetAllClientServicesMapping()
        //    //ServiceStepManager.GetClientServicesStepMappingByService()
        //}
        public void GetTickets()
        {
            Int32 clientID = View.loggedInUserClientID;
            Int64 workItemID = AppConsts.NONE;
            //if (String.Compare(View.QueueCode, ScreenName.ManageWorkQueue.GetStringValue(), true) == AppConsts.NONE)
            //{
            //    clientID = View.SelectedClientID;
            //    workItemID = View.WorkItemID;
            //}
            View.TicketSearchContract.IsEnroller = View.IsEnroller;

            List<TicketsContract> lstTempTickets = TicketsCentreManager.GetTickets(View.SelectedTenantID, View.LobId, View.CustomPagingContract, View.ViewAllTickets,
                                               View.SelectedTenantID, View.CurrentLoggedInUserId, workItemID, View.IsEncryptionApplied, View.TicketSearchContract);

            if (!lstTempTickets.IsNullOrEmpty())
            {
                View.lstTickets = lstTempTickets;
            }
            else
            { View.lstTickets = new List<TicketsContract>(); }
            View.VirtualPageCount = View.CustomPagingContract.VirtualPageCount;
            View.CurrentPageIndex = View.CustomPagingContract.CurrentPageIndex;
        }
        public Boolean DeleteTicket()
        {
            return TicketsCentreManager.DeleteTicket(View.SelectedTenantID, View.TicketIssueID, View.CurrentLoggedInUserId);
        }
        public bool AddEditTickets()
        {
            return true;
        }
        public void BindAssignToUsers()
        {
            //View.lstAssignToUsers = TicketsCentreManager.GetClientUsers(View.CurrentLoggedInUserId, View.loggedInUserClientID, View.SelectedClientID);
        }

        public void GetSeverity()
        {
            List<lkpTicketSeverity> listSeverity = TicketsCentreManager.GetTicketSeverityList(View.SelectedTenantID);
            listSeverity.Insert(0, new lkpTicketSeverity { TicketSeverityID = 0, Name = "--SELECT--" });
            View.lstSeverity = listSeverity;
        }

        public void GetStatus()
        {
            List<lkpTicketStatu> lstTicketStatus = TicketsCentreManager.GetTicketStatusList(View.SelectedTenantID);
            lstTicketStatus.Insert(0, new lkpTicketStatu { TicketStatusID = 0, Name = "--SELECT--" });
            View.lstTicketStatus = lstTicketStatus;
        }

        public void GetTicketType()
        {
            List<lkpTicketIssueType> lstTicketIssueType = TicketsCentreManager.GetTicketIssueTypeList(View.SelectedTenantID);
            lstTicketIssueType.Insert(0, new lkpTicketIssueType { TicketIssueTypeID = 0, Name = "--SELECT--" });
            View.lstTicketType = lstTicketIssueType;
        }

        public void GetLocationList()
        {
            List<LocationContract> lstLocations = new List<LocationContract>();
            View.lstAvailableLocations = FingerPrintSetUpManager.GetFingerprintLocations(View.CurrentLoggedInUserId, View.IsEnroller, true).ToList();
        }


        //public void CheckIfUserIsEnroller()
        //{
        //    View.IsEnroller = SecurityManager.CheckIfUserIsEnroller(View.CurrentLoggedInUser_Guid);
        //}

    }
}
