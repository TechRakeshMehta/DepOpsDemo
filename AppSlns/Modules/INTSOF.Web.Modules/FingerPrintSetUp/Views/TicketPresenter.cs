using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.UI.Contract.TicketsCentre;
using Entity.ClientEntity;
using INTSOF.UI.Contract.Templates;
using INTSOF.UI.Contract.FingerPrintSetup;

namespace CoreWeb.TicketsCentre.Views
{
    public class TicketPresenter : Presenter<ITicketView>
    {
        public override void OnViewLoaded()
        {

        }
        public void GetDropDownsData()
        {

            List<Entity.Tenant> listTenant = SecurityManager.GetListOfTenantWithLocationService(); //TicketsCentreManager.GetClientList(View.loggedInUserClientID);
            listTenant.Insert(0, new Entity.Tenant { TenantID = 0, TenantName = "--SELECT--" });
            View.lstClients = listTenant;

            GetServices();

            //List<lkpTicketSeverity> listSeverity = TicketsCentreManager.GetTicketSeverityList(View.SelectedClientID);
            //listSeverity.Insert(0, new lkpTicketSeverity { TicketSeverityID = 0, Name = "--SELECT--" });
            //View.lstSeverity = listSeverity;

            GetSeverity();

            List<ClientUsers> listAssignToUsers = TicketsCentreManager.GetClientUsers(View.CurrentLoggedInUserId, View.loggedInUserClientID, View.SelectedClientID, View.LOBId);
            listAssignToUsers.Insert(0, new ClientUsers { UserId = 0, UserName = "--SELECT--" });
            View.lstAssignToUsers = listAssignToUsers;


            //View.lstServiceSteps = TicketsCentreManager.GetServiceStepsList(View.ClientServiceMappingID, View.SelectedClientID, View.WorkItemID, View.Source);
            GetServiceSteps();

            //View.lstStatus = TicketsCentreManager.GetTicketStatusList(View.SelectedClientID);
            GetStatus();

            View.lstSendTo = TicketsCentreManager.GetClientUsers(View.CurrentLoggedInUserId, View.loggedInUserClientID, View.SelectedClientID, View.LOBId);

            GetLocationList();
            GetTicketType();
        }
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetClientUsers()
        {
            List<ClientUsers> listAssignToUsers = TicketsCentreManager.GetClientUsers(View.CurrentLoggedInUserId, View.loggedInUserClientID, View.SelectedClientID, View.LOBId);
            View.lstSendTo = TicketsCentreManager.GetClientUsers(View.CurrentLoggedInUserId, View.loggedInUserClientID, View.SelectedClientID, View.LOBId);
            listAssignToUsers.Insert(0, new ClientUsers { UserId = 0, UserName = "--SELECT--" });
            View.lstAssignToUsers = listAssignToUsers;

        }

        public Int32 AddTicket(TicketsContract ticketContract, string screenName)
        {
            return TicketsCentreManager.AddTicket(View.SelectedClientID, ticketContract, View.CurrentLoggedInUserId, screenName);
        }

        public void GetServiceList()
        {
            List<ClientSrvcMappingContract> listServices = TicketsCentreManager.GetServiceList(View.SelectedClientID, View.LOBId);
            listServices.Insert(0, new ClientSrvcMappingContract { ClientSrvcMappingID = 0, ServcieName = "--SELECT--" });
            View.lstServices = listServices;
        }
        public void GetServiceStepsList()
        {
            View.lstServiceSteps = TicketsCentreManager.GetServiceStepsList(View.ClientServiceMappingID, View.SelectedClientID, View.WorkItemID, View.Source);
        }
        public void BindTicketData()
        {
            View.TicketDetails = TicketsCentreManager.GetTicketDetailById(View.SelectedClientID, View.TicketId);
        }

        public Int32 GetDocumentTypeID()
        {
            return TicketsCentreManager.GetDocumentType(View.SelectedClientID);
        }
        public Int32 GetEntityTypeID()
        {
            return TicketsCentreManager.GetEntityType(View.SelectedClientID);
        }

        public Boolean DeleteDocument(Int32 documentID)
        {
            return TicketsCentreManager.DeleteDocument(View.SelectedClientID, View.TicketId, documentID, View.loggedInUserClientID);
        }
        //public void SendEmail(TicketsContract ticketContract, int ticketID, string actionName)
        //{
        //    TicketsCentreManager.SendEmail(ticketContract, ticketID, actionName);
        //}
        //public void UpdateWorkItemStatus(int WorkItemID)
        //{
        //    TicketsCentreManager.UpdateWorkItemStatus(WorkItemID, View.CurrentLoggedInUserId);
        //}

        public void GetClients()
        {
            List<Entity.Tenant> listTenant = SecurityManager.GetListOfTenantWithLocationService(); //TicketsCentreManager.GetClientList(View.loggedInUserClientID);
            //listTenant.Insert(0, new Entity.Tenant { TenantID = 0, TenantName = "--SELECT--" });
            View.lstClients = listTenant;
        }
        public void GetServices()
        {
            List<ClientSrvcMappingContract> listServices = TicketsCentreManager.GetServiceList(View.SelectedClientID, View.LOBId);
            listServices.Insert(0, new ClientSrvcMappingContract { ClientSrvcMappingID = 0, ServcieName = "--SELECT--" });
            View.lstServices = listServices;
        }
        public void GetServiceSteps()
        {
            View.lstServiceSteps = TicketsCentreManager.GetServiceStepsList(View.ClientServiceMappingID, View.SelectedClientID, View.WorkItemID, View.Source);
        }
        public void GetAssignedToUsers()
        {
            List<ClientUsers> listAssignToUsers = TicketsCentreManager.GetClientUsers(View.CurrentLoggedInUserId, View.loggedInUserClientID, View.SelectedClientID, View.LOBId);
            listAssignToUsers.Insert(0, new ClientUsers { UserId = 0, UserName = "--SELECT--" });
            View.lstAssignToUsers = listAssignToUsers;
        }
        public void GetSendToUsers()
        {
            View.lstSendTo = TicketsCentreManager.GetClientUsers(View.CurrentLoggedInUserId, View.loggedInUserClientID, View.SelectedClientID, View.LOBId);
        }
        public void GetSeverity()
        {
            List<lkpTicketSeverity> listSeverity = TicketsCentreManager.GetTicketSeverityList(View.SelectedClientID);
            listSeverity.Insert(0, new lkpTicketSeverity { TicketSeverityID = 0, Name = "--SELECT--" });
            View.lstSeverity = listSeverity;
        }
        public void GetStatus()
        {
            View.lstStatus = TicketsCentreManager.GetTicketStatusList(View.SelectedClientID);
        }

        public void GetLocationList()
        {
            List<LocationContract> lstLocations = new List<LocationContract>();
            lstLocations = FingerPrintSetUpManager.GetFingerprintLocations(View.CurrentLoggedInUserId, View.IsEnroller, true).ToList();
            lstLocations.Insert(0, new LocationContract { LocationID = 0, LocationName = "--SELECT--" });
            View.lstAvailableLocations = lstLocations;
        }

        public void GetTicketType()
        {
            List<lkpTicketIssueType> lstTicketIssueType = TicketsCentreManager.GetTicketIssueTypeList(View.SelectedClientID);
            lstTicketIssueType.Insert(0, new lkpTicketIssueType { TicketIssueTypeID = 0, Name = "--SELECT--" });
            View.lstTicketType = lstTicketIssueType;
        }

        //public void CheckIfUserIsEnroller()
        //{
        //    View.IsEnroller = SecurityManager.CheckIfUserIsEnroller(View.CurrentLoggedInUser_Guid);
        //}
    }
}
