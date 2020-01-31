using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.Mobility;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
namespace CoreWeb.Mobility.Views
{
    public class MobilityApprovalQueuePresenter : Presenter<IMobilityApprovalQueueView>
    {

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetTenants();
        }
        /// <summary>
        /// To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            List<Entity.Tenant> lstTemp = SecurityManager.GetTenants(SortByName, false, clientCode);
            lstTemp.Insert(0, new Entity.Tenant { TenantID = 0, TenantName = "--Select--" });
            View.lstTenant = lstTemp;
        }

        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
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
        /// <summary>
        /// Get the Applicant Mobility Node Transition deatils
        /// </summary>
        public void GetApplicantNodeTransitionStatus()
        {
            try
            {
                if (ClientId != 0 && ClientId.IsNotNull())
                {
                    List<Int32> defaultSelectedStatusIdList = new List<Int32>();
                    if (!(View.SelectedApplicantMobilityStatusIds.Count > 0))
                    {
                        List<Int16> tempList = LookupManager.GetLookUpData<Entity.ClientEntity.lkpApplicantMobilityStatu>(ClientId).Where(cond => cond.IsDeleted == false).Select(cnd => cnd.ApplicantMobilityStatusID).ToList();
                        //List<Int16> tempList=MobilityManager.GetApplicantMobilityStatusList(ClientId).Select(cond => cond.ApplicantMobilityStatusID).ToList();
                        foreach(int id in tempList)
                        {
                            defaultSelectedStatusIdList.Add(id);
                        }
                    }

                    MobilitySearchDataContract mobilitySearchDataContract = new MobilitySearchDataContract();
                    if ((View.SelectedApplicantMobilityStatusIds.Count > 0))
                    {
                        mobilitySearchDataContract.Status = GetXMLString(View.SelectedApplicantMobilityStatusIds);
                    }
                    else
                    {
                        mobilitySearchDataContract.Status = GetXMLString(defaultSelectedStatusIdList);
                    }
                    //mobilitySearchDataContract.UserGroupXMLString = GetXMLString(View.SelectedUserGroupIds);
                    if (View.SelectedUserGroupId != AppConsts.NONE && View.SelectedUserGroupId.IsNotNull())
                    {
                        mobilitySearchDataContract.UserGroupID = View.SelectedUserGroupId;
                    }
                    mobilitySearchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.FirstName) ? null : View.FirstName;
                    mobilitySearchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.LastName) ? null : View.LastName;
                    if (View.SourceNodeIds.IsNullOrEmpty())
                    {
                        mobilitySearchDataContract.SourceNodeIds = Convert.ToString(View.SourceNodeIds);
                    }
                    if (!View.TransitionDate.IsNullOrEmpty() && View.TransitionDate != DateTime.MinValue)
                    {
                        mobilitySearchDataContract.TransitionDate = View.TransitionDate;
                    }
                    else
                    {
                        mobilitySearchDataContract.TransitionDate = null;
                    }
                    View.ApplicantNodeTransitionStatus = MobilityManager.GetApplicantNodeTransitionStatus(ClientId, View.GridCustomPaging, mobilitySearchDataContract);
                    if (View.ApplicantNodeTransitionStatus.IsNotNull() && View.ApplicantNodeTransitionStatus.Count > 0)
                    {
                        if (View.ApplicantNodeTransitionStatus[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.ApplicantNodeTransitionStatus[0].TotalCount;
                        }
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                    }
                    else
                    {
                        View.VirtualRecordCount = 0;
                        View.CurrentPageIndex = 1;
                    }
                }
                else
                {
                    View.ApplicantNodeTransitionStatus = new List<ApplicantTransitionStatus>();
                }
            }
            catch (Exception e)
            {
                View.ApplicantNodeTransitionStatus = new List<ApplicantTransitionStatus>();
                throw e;
            }
        }
        /// <summary>
        /// update the MobilityNodeTransition status based on the status
        /// </summary>
        /// <param name="mobilityNodeTransitionLists"></param>
        /// <param name="approvalStatus"></param>
        /// <returns></returns>
        public Boolean UpdateNodeTransitionStatus(Dictionary<Int32, Boolean> mobilityNodeTransitionLists, Int16 approvalStatus)
        {
            List<Int32> mobilityNodeTransitionIds = new List<Int32>();
            mobilityNodeTransitionIds = mobilityNodeTransitionLists.Where(cond => cond.Value).Select(select => select.Key).ToList();
            if (mobilityNodeTransitionIds.Count > 0)
            {
                return MobilityManager.UpdateNodeTransitionStatus(ClientId, mobilityNodeTransitionIds, View.CurrentLoggedInUserId, approvalStatus);
            }
            return false;
        }
        /// <summary>
        /// Get the Approval Status based on the code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Int16 GetApplicantMobilityStatus(String code)
        {
            return LookupManager.GetLookUpData<Entity.ClientEntity.lkpApplicantMobilityStatu>(ClientId).FirstOrDefault(cond => cond.IsDeleted == false && cond.Code==code).ApplicantMobilityStatusID;
            //return MobilityManager.GetApplicantMobilityStatus(ClientId, code);
        }

        /// <summary>
        /// Method to get the Applicant mobility status list.
        /// </summary>
        public void GetApplicantStatusList()
        {

            //View.LstApplicantMobilityStatus = MobilityManager.GetApplicantMobilityStatusList(ClientId);
            View.LstApplicantMobilityStatus = LookupManager.GetLookUpData<Entity.ClientEntity.lkpApplicantMobilityStatu>(ClientId).Where(cond=>cond.IsDeleted==false).ToList();
        }

        /// <summary>
        /// To get Admin Program Study
        /// </summary>
        public void GetAllUserGroups()
        {
            List<UserGroup> tempList = new List<UserGroup>();
            if (ClientId != AppConsts.NONE)
            {
                tempList = ComplianceSetupManager.GetAllUserGroup(ClientId).OrderBy(ex=>ex.UG_Name).ToList();
                tempList.Insert(0, new UserGroup { UG_ID = 0, UG_Name = "--Select--" });
                View.lstUserGroup = tempList;
            }
            else
            {
                tempList.Insert(0, new UserGroup { UG_ID = 0, UG_Name = "--Select--" });
                View.lstUserGroup = tempList;
            }
        }

        #region Private Method

        /// <summary>
        /// Get XML String For Status List
        /// </summary>
        /// <param name="listOfIds">listOfIds</param>
        /// <returns>String</returns>
        private String GetXMLString(List<Int32> listOfIds)
        {
            if (listOfIds.IsNotNull() && listOfIds.Count > 0)
            {
                StringBuilder IdString = new StringBuilder();
                foreach (Int32 id in listOfIds)
                {
                    IdString.Append("<Root><Value>" + id.ToString() + "</Value></Root>");
                }

                return IdString.ToString();
            }
            return null;
        }

        #endregion
    }
}




