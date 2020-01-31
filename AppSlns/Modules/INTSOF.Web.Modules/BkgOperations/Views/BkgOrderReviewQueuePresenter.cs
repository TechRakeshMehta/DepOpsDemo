using System;
using System.Collections.Generic;
using System.Linq;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public class BkgOrderReviewQueuePresenter : Presenter<IBkgOrderReviewQueueView>
    {

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            if (Business.RepoManagers.SecurityManager.DefaultTenantID == View.TenantId)
            {
                View.IsAdminUser = true;
            }
            else
            {
                View.IsAdminUser = false;
            }
            GetTenants();
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        /// <summary>
        /// To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.LstTenant = SecurityManager.GetTenants(SortByName, false, clientCode);
        }

        /// <summary>
        /// To get Tenant Id
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// To get Client Id
        /// </summary>
        private Int32 ClientId
        {
            get
            {
                if (IsDefaultTenant)
                    return View.SelectedTenantId;
                return View.TenantId;
            }
        }


        public void GetAllReviewCriterias()
        {
            if (ClientId == 0)
                View.LstReviewCriterias = new List<BkgReviewCriteria>();
            else
                View.LstReviewCriterias = BackgroundProcessOrderManager.GetAllReviewCriterias(ClientId).ToList().OrderBy(x => x.BRC_Name).ToList();//UAT sort dropdowns by Name;
        }

        public void GetAllSvcGrpReviewStatuses()
        {
            if (ClientId == 0)
                View.LstSvcGrpReviewStatus = new List<lkpBkgSvcGrpReviewStatusType>();
            else
                View.LstSvcGrpReviewStatus = BackgroundProcessOrderManager.GetAllSvcGrpReviewStatuses(ClientId).ToList();
        }

        public void GetAllSvcGrpStatuses()
        {
            if (ClientId == 0)
                View.LstSvcGrpStatus = new List<lkpBkgSvcGrpStatusType>();
            else
                View.LstSvcGrpStatus = BackgroundProcessOrderManager.GetAllSvcGrpStatuses(ClientId).ToList();
        }


        /// <summary>
        /// To perform search
        /// </summary>
        public void PerformSearch()
        {
            if (ClientId == 0)
            {
                View.BkgOrderReviewQueueData = new List<BkgOrderReviewQueueContract>();
            }
            else
            {
                BkgOrderReviewQueueContract bkgOrderReviewQueueContract = new BkgOrderReviewQueueContract();
                bkgOrderReviewQueueContract.ClientID = ClientId;
                bkgOrderReviewQueueContract.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
                bkgOrderReviewQueueContract.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
                if (!View.OrderNumber.IsNullOrEmpty())
                {
                    bkgOrderReviewQueueContract.OrderNumber = View.OrderNumber;
                }
                if (View.SvcGrpReviewStatusTypeIDs.IsNotNull() && View.SvcGrpReviewStatusTypeIDs.Count > AppConsts.NONE)
                {
                    bkgOrderReviewQueueContract.SvcGrpReviewStatusTypeIDs = View.SvcGrpReviewStatusTypeIDs;
                }

                if (View.SvcGrpStatusTypeID.IsNotNull() && View.SvcGrpStatusTypeID > AppConsts.NONE)
                {
                    bkgOrderReviewQueueContract.SvcGrpStatusTypeID = View.SvcGrpStatusTypeID;
                }

                if (View.SelectedReviewCriteriaId.IsNotNull() && View.SelectedReviewCriteriaId > AppConsts.NONE)
                {
                    bkgOrderReviewQueueContract.SelectedReviewCriteriaId = View.SelectedReviewCriteriaId;
                }
                if (View.OrderFromDate.IsNotNull() || View.OrderToDate.IsNotNull())
                {
                    if (View.OrderFromDate.IsNullOrEmpty())
                    {
                        View.OrderFromDate = DateTime.Now;
                    }
                    if (View.OrderToDate.IsNullOrEmpty())
                    {
                        View.OrderToDate = DateTime.Now;
                    }
                    bkgOrderReviewQueueContract.OrderFromDate = View.OrderFromDate;
                    bkgOrderReviewQueueContract.OrderToDate = View.OrderToDate;
                }

                if (View.SvcGrpUpdatedFromDate.IsNotNull() || View.SvcGrpUpdatedToDate.IsNotNull())
                {
                    if (View.SvcGrpUpdatedToDate.IsNullOrEmpty())
                    {
                        View.SvcGrpUpdatedToDate = DateTime.Now;
                    }
                    if (View.SvcGrpUpdatedFromDate.IsNullOrEmpty())
                    {
                        View.SvcGrpUpdatedFromDate = DateTime.Now;
                    }
                    bkgOrderReviewQueueContract.SvcGrpUpdatedFromDate = View.SvcGrpUpdatedFromDate;
                    bkgOrderReviewQueueContract.SvcGrpUpdatedToDate = View.SvcGrpUpdatedToDate;
                }


                if (View.CurrentLoggedInUserId.IsNotNull() && View.CurrentLoggedInUserId > AppConsts.NONE)
                {
                    bkgOrderReviewQueueContract.LoggedInUserId = IsDefaultTenant ? (Int32?)null : View.CurrentLoggedInUserId;
                }
                //changed related to UAT-1683
                if (!View.SelectedArchiveStateCode.IsNullOrEmpty())
                {
                    bkgOrderReviewQueueContract.SelectedArchiveStateCode = View.SelectedArchiveStateCode;
                }
                //if (View.TargetHierarchyNodeId.IsNotNull() && View.TargetHierarchyNodeId > AppConsts.NONE)
                //{
                //    bkgOrderReviewQueueContract.DeptProgramMappingID = View.TargetHierarchyNodeId;
                //}
                //changed related to UAT-2304
                if (!View.SelectedSvcGrpReviewType.IsNullOrEmpty())
                {
                    bkgOrderReviewQueueContract.SvcGrpReviewType = View.SelectedSvcGrpReviewType;
                }
                if (!View.CurrentPageIndex.IsNullOrEmpty())
                {
                    bkgOrderReviewQueueContract.CurrentPageIndex = View.CurrentPageIndex;
                }

                bkgOrderReviewQueueContract.DeptProgramMappingIDs = View.TargetHierarchyNodeIds;

                View.GridCustomPaging.DefaultSortExpression = QueueConstants.ORDER_REVIEW_QUEUE_DEFAULT_SORTING_FIELDS;
                View.GridCustomPaging.SecondarySortExpression = QueueConstants.ORDER_REVIEW_QUEUE_DEFAULT_SORTING_FIELDS;
                bkgOrderReviewQueueContract.GridCustomPagingArguments = View.GridCustomPaging;
                View.SetBkgOrderReviewQueueContract = bkgOrderReviewQueueContract;
                //changed related to UAT-2304
                if (!bkgOrderReviewQueueContract.SvcGrpReviewType.IsNullOrEmpty() 
                    && String.Compare(bkgOrderReviewQueueContract.SvcGrpReviewType, SvcGrpReviewType.ALL.GetStringValue(), true) == AppConsts.NONE)
                {
                    bkgOrderReviewQueueContract.SvcGrpReviewType = null;
                }

                View.BkgOrderReviewQueueData = BackgroundProcessOrderManager.GetBkgOrderReviewQueueData(ClientId, bkgOrderReviewQueueContract, View.GridCustomPaging);
                if (View.BkgOrderReviewQueueData.IsNotNull() && View.BkgOrderReviewQueueData.Count > 0)
                {
                    if (View.BkgOrderReviewQueueData[0].TotalCount > 0)
                    {
                        View.VirtualRecordCount = View.BkgOrderReviewQueueData[0].TotalCount;
                    }
                    View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                }
                else
                {
                    View.VirtualRecordCount = 0;
                    View.CurrentPageIndex = 1;
                }
            }
        }

        public Int32? GetSvcGroupReviewStatusTypeIdByCode(String reviewStatusTypeCode)
        {
            return BackgroundProcessOrderManager.GetSvcGroupReviewStatusTypeIdByCode(View.SelectedTenantId, reviewStatusTypeCode);
        }

        /// <summary>
        /// Gets the list of Archive State.
        /// </summary>
        public void GetArchiveStateList()
        {
            View.lstArchiveState = ComplianceDataManager.GetArchiveStateList(View.SelectedTenantId);
        }
    }
}

