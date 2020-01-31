using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using Business.RepoManagers;
using Entity;
using System.Linq;

namespace CoreWeb.Messaging.Views
{
    public class AddressLookupPresenter : Presenter<IAddressLookupView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IMessagingController _controller;
        // public AddressLookupPresenter([CreateNew] IMessagingController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        // TODO: Handle other view events and set state in the view


        /// <summary>
        /// Retrieves Users for composing the new message
        /// </summary>
        public void RetrieveUsers(Int32 organizationUserId, lkpCommunicationTypeContext communicationTypeContext)
        {
            Entity.CustomPagingArgs customPagingArgs = new Entity.CustomPagingArgs();
            View.GridUsersCustomPaging.DefaultSortExpression = "OrganizationUserID";
            View.GridUsersCustomPaging.SecondarySortExpression = ", UserID";

            IQueryable<OrganizationUser> organizationUsersList = MessageManager.RetrieveUsers(View.CurrentViewContext.SelectedOrganizationUserIds, organizationUserId, communicationTypeContext, View.CurrentViewContext.SelectedTenantId, View.CurrentViewContext.SelectedProgramId);
            View.OrganizationUsers = customPagingArgs.ApplyFilterOrSort(organizationUsersList, View.GridUsersCustomPaging);

            View.VirtualPageCount = View.GridUsersCustomPaging.VirtualPageCount;
            View.CurrentPageIndex = View.GridUsersCustomPaging.CurrentPageIndex;
        }

        public void GetMessagingGroups(Int32 organizationUserId, Boolean isApplicant)
        {
            Entity.CustomPagingArgs customPagingArgs = new Entity.CustomPagingArgs();
            View.GridMessagingGroupCustomPaging.DefaultSortExpression = "ID";
            View.GridMessagingGroupCustomPaging.SecondarySortExpression = ", Type";

            IQueryable<vw_ListOfUsers> messagingGroupList = MessageManager.GetMessagingGroups(organizationUserId);

            // UAT 891 
            if (isApplicant)
            {
                // 'IsInternalMsgEnabled' will be 1 for the Groups andusers which have 'IsInternalMsgEnabled' = 1 in security OrganizationUser
                messagingGroupList = messagingGroupList.Where(mgl => mgl.IsInternalMsgEnabled == AppConsts.ONE);
            }

            View.MessagingGroups = customPagingArgs.ApplyFilterOrSort(messagingGroupList, View.GridMessagingGroupCustomPaging);

            View.VirtualPageCount = View.GridMessagingGroupCustomPaging.VirtualPageCount;
            View.CurrentPageIndex = View.GridMessagingGroupCustomPaging.CurrentPageIndex;
        }

        public OrganizationUser GetOrganizationUser(Int32 organizationUserId)
        {
            return SecurityManager.GetOrganizationUser(organizationUserId);
        }

    }
}




