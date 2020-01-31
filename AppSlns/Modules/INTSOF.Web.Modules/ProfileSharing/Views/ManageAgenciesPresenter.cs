#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using Entity.SharedDataEntity;

#endregion

#endregion

namespace CoreWeb.ProfileSharing.Views
{
    public class ManageAgenciesPresenter : Presenter<IManageAgenciesView>
    {
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads

        }

        public override void OnViewLoaded()
        {

        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        public void GetAgencies()
        {

            String agencyIDs = View.LstSelectedAgencyIDs.IsNullOrEmpty() ? null : View.LstSelectedAgencyIDs;
            List<ManageAgencyContract> tmpAgency = ProfileSharingManager.GetAgencyDetail(View.TenantId, agencyIDs);
            //Commented code UAT-2640
            //if (!IsAdminLoggedIn())
            //{
            //    List<Int32> lstAgencyForClient = ProfileSharingManager.GetAllAgencyForOrgUser(View.TenantId, View.CurrentLoggedInUserId).Select(sel => sel.AG_ID).ToList();
            //    tmpAgency = tmpAgency.Where(cond => lstAgencyForClient.Contains(cond.AgencyID.Value)).ToList();
            //}
            List<Int32> ListAgencyIDs = tmpAgency.Select(x => x.AgencyID).ToList();
            View.lstAgencyInstitutions = ProfileSharingManager.GetAgencyInstitutionForAgencies(View.TenantId, ListAgencyIDs);
            TranslateToContract(tmpAgency, IsDefaultTenant);


        }



        public void GetTenants()
        {
            View.lstTenant = ComplianceDataManager.getClientTenant();
            View.lstTenantID = View.lstTenant.Select(x => x.TenantID).ToList();
        }

        public void SaveAgency()
        {
            View.AgencyData.CreatedByTenantID = View.TenantId;
            if (!IsNPINumberExist(View.AgencyData.NpiNumber))
            {
                if (IsDefaultTenant)
                {
                    View.AgencyData.SearchStatusID = GetAgencySearchStatusIDByCode(AgencySearchStausTypes.AVAILABLE.GetStringValue());
                    View.CurrentSelectedTenantIDs = new List<Int32>();
                }
                else
                {
                    View.AgencyData.SearchStatusID = GetAgencySearchStatusIDByCode(AgencySearchStausTypes.NOT_REVIEWED.GetStringValue());
                }

                Tuple<Int32, Dictionary<Int32, Int32>, Int32> result = ProfileSharingManager.SaveAgencies(View.AgencyData, View.CurrentSelectedTenantIDs);
                //Tuple<Int32, Dictionary<Int32, Int32>, Int32> result = ProfileSharingManager.SaveAgencies(View.AgencyData, new List<Int32>());
                Int32 savedAgencyID = result.Item1;
                //UAT-2639
                Int32 agencyHierarchyId = result.Item3;
                if (savedAgencyID > AppConsts.NONE)
                {
                    Boolean isSavedSucces = true;
                    //UAT-2640:
                    if (!View.AgencyData.IsAdmin)
                    {
                        View.AgencyData.AgencyProfileSharePermission.AgencyHierarchyID = agencyHierarchyId;
                        ProfileSharingManager.SaveAgencyHirInstNodeMappingForClientAdmin(View.TenantId, View.AgencyData.AgencyProfileSharePermission, View.CurrentLoggedInUserId,
                                                                                         savedAgencyID, AppConsts.NONE, View.AgencyData.IsAdmin);
                    }
                    //Commented Code For UAT-2640
                    //foreach (AgencyHierarchyContract agencyHierarchyContract in View.AgencyData.LstAgencyHierarchy)
                    //{
                    //    if (!View.AgencyData.IsAdmin)
                    //    {
                    //        agencyHierarchyContract.AgencyHierarchyID = agencyHierarchyId;
                    //    }

                    //   Int32 agencyInstitutionId= Convert.ToInt32(result.Item2.Where(f => f.Key == agencyHierarchyContract.TenantID).Select(f=>f.Value).FirstOrDefault());
                    //    isSavedSucces = ProfileSharingManager.SaveAgencyHierarchyMapping(agencyHierarchyContract.TenantID,
                    //        agencyHierarchyContract, View.CurrentLoggedInUserId, savedAgencyID, agencyInstitutionId);
                    //}
                    if (isSavedSucces)
                    {
                        //UAT- 2631 Digestion Process
                        List<Int32> lstAgencyHierarchyID = ProfileSharingManager.GetAgencyHierarchyIDsByAgencyID(savedAgencyID);
                        if (!lstAgencyHierarchyID.IsNullOrEmpty())
                        {
                            AgencyHierarchyManager.CallDigestionProcess(String.Join(",", lstAgencyHierarchyID), AppConsts.CHANGE_TYPE_ALL, View.CurrentLoggedInUserId);
                        }
                        View.SuccessMessage = AppConsts.AG_SAVED_SUCCESS_MSG;
                    }
                    else
                    {
                        View.ErrorMessage = AppConsts.AG_SAVED_HIERARCHY_ERROR_MSG;
                    }

                }
                else
                {
                    View.ErrorMessage = AppConsts.AG_SAVED_ERROR_MSG;
                }
            }
            else
            {
                View.ErrorMessage = AppConsts.AG_NPINUMBER_EXIST_MSG;
            }
        }

        public Int32 GetAgencySearchStatusIDByCode(String statusCode)
        {
            List<Entity.SharedDataEntity.lkpAgencySearchStatu> lkpAgencySearchStatusType = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpAgencySearchStatu>();
            return lkpAgencySearchStatusType.Where(cond => cond.SS_Code == statusCode && !cond.SS_IsDeleted).Select(x => x.SS_ID).FirstOrDefault();
        }

        public void UpdateAgency()
        {
            List<Int32> tenantIDs_Added = View.CurrentSelectedTenantIDs.Except(View.PrevSelectedTenantIDs).ToList();
            List<Int32> tenantIDs_Removed = View.PrevSelectedTenantIDs.Except(View.CurrentSelectedTenantIDs).ToList();

            View.AgencyData.CreatedByTenantID = View.TenantId;
            if (!IsNPINumberExist(View.AgencyData.NpiNumber))
            {
                if (IsDefaultTenant)
                {
                    View.AgencyData.SearchStatusID = GetAgencySearchStatusIDByCode(AgencySearchStausTypes.AVAILABLE.GetStringValue());
                }
                else
                {
                    View.AgencyData.SearchStatusID = GetAgencySearchStatusIDByCode(AgencySearchStausTypes.NOT_REVIEWED.GetStringValue());
                }

                Tuple<String, Dictionary<Int32, Int32>> result = ProfileSharingManager.UpdateAgencies(View.AgencyData, new List<Int32>(), new List<Int32>());
                if (result.Item1 == AppConsts.AG_UPDATED_SUCCESS_MSG)
                {
                    Boolean isSuccess = true;
                    //Commented Code For UAT-2640
                    //foreach (Int32 tenantID in tenantIDs_Removed)
                    //{
                    //    Int32 agencyInstitutionId = Convert.ToInt32(result.Item2.Where(f => f.Key == tenantID).Select(f => f.Value).FirstOrDefault());
                    //    isSuccess = ProfileSharingManager.DeleteAgencyHierarchyMappings(tenantID, View.AgencyData.AgencyID, View.AgencyData.LoggedInUserID, agencyInstitutionId);
                    //    //UAT-2529Delete Agency Permission also

                    //}
                    //foreach (AgencyHierarchyContract agencyHierarchyContract in View.AgencyData.LstAgencyHierarchy)
                    //{
                    //    Int32 agencyInstitutionId = Convert.ToInt32(result.Item2.Where(f => f.Key == agencyHierarchyContract.TenantID).Select(f => f.Value).FirstOrDefault());
                    //    isSuccess = ProfileSharingManager.SaveAgencyHierarchyMapping(agencyHierarchyContract.TenantID, agencyHierarchyContract, View.AgencyData.LoggedInUserID, View.AgencyData.AgencyID, agencyInstitutionId);
                    //    isSuccess = true;
                    //}
                    if (isSuccess)
                    {
                        //UAT- 2631 Digestion Process
                        List<Int32> lstAgencyHierarchyID = ProfileSharingManager.GetAgencyHierarchyIDsByAgencyID(View.AgencyID);
                        if (!lstAgencyHierarchyID.IsNullOrEmpty())
                        {
                            AgencyHierarchyManager.CallDigestionProcess(String.Join(",", lstAgencyHierarchyID), AppConsts.CHANGE_TYPE_ALL, View.CurrentLoggedInUserId);
                        }

                        View.SuccessMessage = AppConsts.AG_UPDATED_SUCCESS_MSG;
                    }
                    else
                    {
                        View.ErrorMessage = AppConsts.AG_UPDATED_HIERARCHY_ERROR_MSG;
                    }
                }
                else
                {
                    View.ErrorMessage = result.Item1;
                }
            }
            else
            {
                View.ErrorMessage = AppConsts.AG_NPINUMBER_EXIST_MSG;
            }
        }

        public void DeleteAgency()
        {
            //UAT- 2631 Digestion Process
            List<Int32> lstAgencyHierarchyID = ProfileSharingManager.GetAgencyHierarchyIDsByAgencyID(View.AgencyID);

            String status = ProfileSharingManager.DeleteAgency(View.AgencyData);
            if (status == AppConsts.AG_DELETED_SUCCESS_MSG)
            {
                //UAT- 2631 Digestion Process
                if (!lstAgencyHierarchyID.IsNullOrEmpty())
                {
                    AgencyHierarchyManager.CallDigestionProcess(String.Join(",", lstAgencyHierarchyID), AppConsts.CHANGE_TYPE_ALL, View.CurrentLoggedInUserId);
                }

                View.SuccessMessage = status;
            }
            else if (status == AppConsts.AG_DELETED_INFO_MSG || status == AppConsts.AG_DELETION_CR_ASSOCIATED_MSG)
            {
                View.ErrorMessage = status;
            }
            else
            {
                View.ErrorMessage = status;
            }
        }

        #region Private Methods
        private void TranslateToContract(List<ManageAgencyContract> tmpAgency, Boolean IsAdmin)
        {
            List<AgencyContract> _tmplst = new List<AgencyContract>();
            foreach (ManageAgencyContract _agency in tmpAgency)
            {
                AgencyContract _agContract = new AgencyContract();
                _agContract.AgencyID = _agency.AgencyID;
                _agContract.Name = _agency.Name;
                //UAT-2640:
                _agContract.Label = _agency.LABEL;
                _agContract.Description = _agency.Description.IsNullOrEmpty() ? String.Empty : _agency.Description;
                _agContract.Address = _agency.Address.IsNullOrEmpty() ? String.Empty : _agency.Address;
                _agContract.SharingStatusCode = _agency.SharingStatusCode;

                _agContract.NpiNumber = _agency.NpiNumber.IsNullOrEmpty() ? String.Empty : _agency.NpiNumber;
                if (_agency.ZipCodeID.IsNotNull())
                {
                    _agContract.ZipCodeID = _agency.ZipCodeID.HasValue ? _agency.ZipCodeID.Value : 0;
                }
                _agContract.FullAddress = _agency.FullAddress.IsNullOrEmpty() ? String.Empty : _agency.FullAddress;

                if (IsAdmin)
                {
                    _agContract.TenantName = GetTenantName(_agency.AgencyID);
                    _agContract.AgencyHierarchyLabel = _agency.AgencyHierarchyLabel;
                    _agContract.AgencyHierarchyRootNodeLabel = _agency.AgencyHierarchyRootNodeLabel;
                }
                else
                {
                    _agContract.TenantName = GetCurrentTenantName();
                }

                _agContract.AttestationReportText = _agency.AttestationReportText;
                _tmplst.Add(_agContract);
            }
            View.ListAgencies = _tmplst;
        }

        private string GetTenantName(Int32 AG_ID)
        {
            List<Int32?> lstTenantIDs = View.lstAgencyInstitutions.Where(x => x.AGI_AgencyID == AG_ID).Select(cond => cond.AGI_TenantID).ToList();
            return GetInstitutionName(lstTenantIDs);
        }

        private static string GetInstitutionName(List<Int32?> lstTenantIDs)
        {
            var lstTenant = ComplianceDataManager.getClientTenant();
            return String.Join(",", lstTenant.Where(x => lstTenantIDs.Contains(x.TenantID)).Select(col => col.TenantName));
        }

        private String GetCurrentTenantName()
        {
            return ComplianceDataManager.GetTenantList(SecurityManager.DefaultTenantID, true).Where(col => col.TenantID == View.TenantId).FirstOrDefault().TenantName;
        }

        private Boolean IsNPINumberExist(String npiNumber)
        {
            if (npiNumber != String.Empty)
            {
                if (View.OldNPINumber != npiNumber)
                {
                    return ProfileSharingManager.IsNPINumberExist(npiNumber);
                }
            }
            return false;
        }

        #endregion

        public void CheckTenantsNeedToDisable(int AG_ID)
        {
            View.TenantIDsToDisable = ProfileSharingManager.CheckTenantsNeedToDisable(View.TenantId, AG_ID);
        }

        public usp_GetAgencyDetailByAgencyID_Result GetAgencyDetailByAgencyID(Int32 agencyID)
        {
            return ProfileSharingManager.GetAgencyDetailByAgencyID(agencyID);
        }

        public void AssociateAgencyWithInstitution()
        {
            Tuple<String, Int32> result = ProfileSharingManager.SaveAgencyInstitutionMapping(View.agencyInstitution);
            String status = result.Item1;
            Boolean isSavedSucces = true;
            if (status == AppConsts.AG_SAVED_SUCCESS_MSG)
            {
                AgencyHierarchyContract agencyHierarchyContract = View.LstAgencyHierarchy.FirstOrDefault(col => col.TenantID == View.agencyInstitution.AGI_TenantID);
                if (!agencyHierarchyContract.IsNullOrEmpty())
                {
                    isSavedSucces = ProfileSharingManager.SaveAgencyHierarchyMapping(agencyHierarchyContract.TenantID,
                        agencyHierarchyContract, View.CurrentLoggedInUserId, View.agencyInstitution.AGI_AgencyID.Value, result.Item2);

                }
                if (isSavedSucces)
                {
                    View.SuccessMessage = AppConsts.AG_SAVED_SUCCESS_MSG;
                }
                else
                {
                    View.ErrorMessage = AppConsts.AG_SAVED_HIERARCHY_ERROR_MSG;
                }
            }
            else
            {
                View.ErrorMessage = status;
            }
        }

        public bool IsAgencyAssociateWithInstitution()
        {
            return ProfileSharingManager.IsAgencyAssociateWithInstitution(View.InstitutionID, View.AgencyID);
        }

        public void GetInstituteHierarchyForSelectedAgency(Dictionary<Int32, String> lstSelectedTenants)
        {
            List<AgencyHierarchyContract> lstAgencyHierarchy = new List<AgencyHierarchyContract>();

            foreach (Int32 tenantID in lstSelectedTenants.Keys)
            {
                List<Entity.ClientEntity.AgencyHierarchyMapping> lstAgencyHierarchyMapping =
                                                  ProfileSharingManager.GetInstituteHierarchyForSelectedAgency(View.AgencyID, tenantID);
                AgencyHierarchyContract agencyHierarchyContract = new AgencyHierarchyContract();
                agencyHierarchyContract.TenantID = tenantID;
                agencyHierarchyContract.TenantName = lstSelectedTenants[tenantID];
                if (!lstAgencyHierarchyMapping.IsNullOrEmpty())
                {
                    agencyHierarchyContract.Hierarchies = String.Join(",", lstAgencyHierarchyMapping.DistinctBy(col => col.DeptProgramMapping.DPM_ID)
                                                                                .Where(cond => !cond.DeptProgramMapping.DPM_IsDeleted)
                                                                                .Select(col => col.DeptProgramMapping.DPM_Label));
                    agencyHierarchyContract.HierarchyIDs = String.Join(",", lstAgencyHierarchyMapping.DistinctBy(col => col.DeptProgramMapping.DPM_ID)
                                                                                .Where(cond => !cond.DeptProgramMapping.DPM_IsDeleted)
                                                                                .Select(col => col.DeptProgramMapping.DPM_ID));
                }
                #region UAT-2529
                Tuple<Boolean, Boolean> agencyInstitutionPermission =
                                                  ProfileSharingManager.GetAgencyInstitutePermissionForSelectedAgency(View.AgencyID, tenantID);
                agencyHierarchyContract.IsAdmin = agencyInstitutionPermission.Item2;
                agencyHierarchyContract.IsStudent = agencyInstitutionPermission.Item1;
                #endregion

                lstAgencyHierarchy.Add(agencyHierarchyContract);
            }
            View.LstAgencyHierarchy = lstAgencyHierarchy;
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsAdminLoggedIn()
        {
            return (SecurityManager.DefaultTenantID == View.TenantId);
        }

        public void GetAgencyPermissionTypeID(String code)
        {
            View.AgencyPermissionTypeId = ProfileSharingManager.GetAgencyPermissionTypeID(code);
        }

        public void GetAgencyPermissionAccessTypeID(String code)
        {
            View.AgencyPermissionAccessTypeId = ProfileSharingManager.GetAgencyPermissionAccessTypeID(code);
        }

        public void GetAgencypermisionByAgencyID(int agencyID)
        {
            View.DicAgencyPermissions = ProfileSharingManager.GetAgencyPermisionByAgencyID(agencyID);
        }
        //UAT-2181:
        public void AssignTenantToAgency(Int32 SelectedTenantID)
        {
            Boolean isSavedSucces = true;
            if (View.lstSelectedAgencyIDs.IsNullOrEmpty())
            {
                View.ErrorMessage = "Please select Agency(s).";
            }
            else
            {
                isSavedSucces = ProfileSharingManager.AssignTenantToAgency(SelectedTenantID, View.lstSelectedAgencyIDs, View.CurrentLoggedInUserId);

                if (isSavedSucces)
                {
                    View.SuccessMessage = AppConsts.AG_SAVED_TENANT_SUCCESS_MSG;
                }
                else
                {
                    View.ErrorMessage = AppConsts.AG_NOT_SAVED_SUCCESS_MSG;
                }
            }
        }

        //UAT-2639:

        public void GetClientAdminRootNode()
        {
            View.ClientAdminRootNode = ProfileSharingManager.GetClientAdminRootNode(View.TenantId);
        }

        public void GetAgencyInstitutionData(Int32 agencyId)
        {
            View.AgHierarchyProfilePermission = ProfileSharingManager.GetAgencyHierarchyProfileSharePermission(View.TenantId, agencyId);
        }

        /// <summary>
        /// UAT 2821 Agency formatted attestations (uploaded document rather than our attestation)
        /// </summary>
        /// <param name="agencyID"></param>
        public void GetAgencyAttestationFormPermission(String agencyID)
        {
            View.AttestationPermissionForAgency = ProfileSharingManager.GetAttestationReportTextForAgency(agencyID.ToString()).FirstOrDefault();
        }
    }
}
