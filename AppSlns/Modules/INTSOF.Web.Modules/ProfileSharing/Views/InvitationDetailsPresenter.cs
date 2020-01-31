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

using INTSOF.Utils;
using Business.RepoManagers;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.UI.Contract.ComplianceOperation;
using Entity.ClientEntity;
using System.Data;
using System.Xml.Linq;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

#endregion

#endregion

namespace CoreWeb.ProfileSharing.Views
{
    public class InvitationDetailsPresenter : Presenter<IInvitationDetailsView>
    {
        public override void OnViewInitialized()
        {
            //Get all tenants and filter them w.r.t Invitations
            List<Entity.ClientEntity.Tenant> tenants = ComplianceDataManager.getClientTenant();
            var profileSharingInvitations = ProfileSharingManager.GetInvitationsByInviteeOrgUserID(View.CurrentLoggedInUserId);

            //List<Int32> sharedUserTenantIDs = new List<Int32>();
            //if ((View.SharedUserTypeCodes.Contains(OrganizationUserType.Instructor.GetStringValue())
            //    || View.SharedUserTypeCodes.Contains(OrganizationUserType.Preceptor.GetStringValue())))
            //{
            //    sharedUserTenantIDs = GetSharedUserTenantIds();
            //    View.lstTenant = tenants.Where(x => sharedUserTenantIDs.Contains(x.TenantID)).ToList();
            //}
            //else
            //{
            //    if (profileSharingInvitations.IsNotNull())
            //    {
            //        var tenantIDs = profileSharingInvitations.Select(x => x.PSI_TenantID).Distinct().ToList();
            //        View.lstTenant = tenants.Where(x => tenantIDs.Contains(x.TenantID)).ToList();
            //    }
            //}

            if (profileSharingInvitations.IsNotNull())
            {
                var tenantIDs = profileSharingInvitations.Select(x => x.PSI_TenantID).Distinct().ToList();
                View.lstTenant = tenants.Where(x => tenantIDs.Contains(x.TenantID)).ToList();
            }

            //Get InviteeTypes
            View.lstInviteeType = LookupManager.GetSharedDBLookUpData<lkpInvitationSource>().Where(x => x.IsDeleted == false)
                    .Select(cond => new LookupContract()
                    {
                        Name = cond.InvitationSourceType,
                        Code = cond.Code
                    }).OrderBy(x => x.Name).ToList();


            GetRotationReviewStatus();
        }

        public override void OnViewLoaded()
        {

        }

        public String GetFormattedPhoneNumber(String unformattedPhoneNumber)
        {
            return ApplicationDataManager.GetFormattedPhoneNumber(unformattedPhoneNumber);
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
        /// To get Client Id
        /// </summary>
        //private Int32 ClientId
        //{
        //    get
        //    {
        //        if (IsDefaultTenant)
        //            return View.SelectedTenantId;
        //        return View.TenantId;
        //    }
        //}

        public void PerformSearch()
        {
            //UAT-1702: On the Agency User's Other tab, we should allow the user to multi select from a list of Institutions
            //if (View.SelectedTenantId == 0)
            //{
            //    View.lstInvitationQueue = new List<InvitationDataContract>();
            //}
            if (View.lstSelectedTenants.IsNullOrEmpty())
            {
                View.lstInvitationQueue = new List<InvitationDataContract>();
            }
            else
            {
                InvitationSearchContract searchContract = new InvitationSearchContract();
                searchContract.Name = String.IsNullOrEmpty(View.InviteeNameSearch) ? null : View.InviteeNameSearch;
                searchContract.EmailAddress = String.IsNullOrEmpty(View.EmailAddressSearch) ? null : View.EmailAddressSearch;
                searchContract.Phone = String.IsNullOrEmpty(View.PhoneNumberSearch) ? null : View.PhoneNumberSearch;
                //searchContract.TenantID = View.SelectedTenantId;
                searchContract.Notes = String.IsNullOrEmpty(View.NotesSearch) ? null : View.NotesSearch.ToFormatApostrophe();

                searchContract.LstInviteTypeCode = View.SelectedInviteeTypeCode.Count == 0 ? null : View.SelectedInviteeTypeCode;
                //searchContract.SelectedItemIDs = GetXMLString(searchContract.SelectedItemIDList);
                searchContract.CurrentLoggedInUserID = View.CurrentLoggedInUserId;

                searchContract.ExpirationDateFrom = View.ExpirationDateFrom;
                searchContract.ExpirationDateTo = View.ExpirationDateTo;
                searchContract.InvitationDateFrom = View.InvitationDateFrom;
                searchContract.InvitationDateTo = View.InvitationDateTo;
                searchContract.LastViewedDateFrom = View.LastViewedDateFrom;
                searchContract.LastViewedDateTo = View.LastViewedDateTo;
                searchContract.SelectedReviewStatusCode = View.SelectedReviewStatusCode;
                //UAT-1702: On the Agency User's Other tab, we should allow the user to multi select from a list of Institutions
                searchContract.TenantDetailList = View.lstSelectedTenants;
                var lstSelectedTenants = View.lstSelectedTenants.Select(x => x.TenantID).ToList();
                searchContract.TenantIDs = String.Join(",", lstSelectedTenants);
                //UAT-1641:As an Agency User, I should be able to be linked to multiple agencies
                searchContract.AgencyIdList = View.lstSelectedAgencyIds;

                try
                {
                    View.GridCustomPaging.DefaultSortExpression = "ExpirationDateOrderBy";

                    if (View.GridCustomPaging.SortExpression.IsNullOrEmpty())
                        View.GridCustomPaging.SortDirectionDescending = false;
                    View.SetSearchContract = searchContract;

                    //View.lstInvitationQueue = ProfileSharingManager.GetInvitationData(ClientId, searchContract, View.GridCustomPaging);
                    View.lstInvitationQueue = ProfileSharingManager.GetInvitationData(searchContract, View.GridCustomPaging);
                    if (View.lstInvitationQueue.IsNotNull() && View.lstInvitationQueue.Count > 0)
                    {
                        //View.VirtualPageCount = View.GridCustomPaging.VirtualPageCount;
                        if (View.lstInvitationQueue[0].TotalCount > 0)
                        {
                            View.VirtualPageCount = View.lstInvitationQueue[0].TotalCount;
                        }
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                    }
                    else
                    {
                        View.VirtualPageCount = 0;
                        View.CurrentPageIndex = 1;
                    }
                }
                catch (Exception e)
                {
                    View.lstInvitationQueue = new List<InvitationDataContract>();
                    throw e;
                }
            }
        }

        public void GetApplicantInviteDocuments(List<InvitationIDsContract> selectedInvitationIDsList, Int32 tenantID)
        {
            List<InvitationDocumentContract> documentToExport = new List<InvitationDocumentContract>();
            //View.SelectedTenantId
            var applicantInvitationDocList = ProfileSharingManager.GetApplicantInviteDocuments(tenantID 
                                                                                               , selectedInvitationIDsList.Where(cond => cond.IsInvitationSourceApplicant)
                                                                                               .ToList());
            var adminInvitationDocList = ProfileSharingManager.GetClientInviteDocuments(tenantID
                                                                                        , selectedInvitationIDsList.Where(cond => !cond.IsInvitationSourceApplicant)
                                                                                        .ToList());

            if (applicantInvitationDocList.IsNotNull())
            {
                documentToExport.AddRange(applicantInvitationDocList);
            }

            if (adminInvitationDocList.IsNotNull())
            {
                documentToExport.AddRange(adminInvitationDocList);
            }

            View.DocumentListToExport = documentToExport;
        }

        private List<InvitationDocumentContract> AssignValuesForPassportReport(DataTable dtTable)
        {
            IEnumerable<DataRow> rows = dtTable.AsEnumerable();
            return rows.Select(x => new InvitationDocumentContract
            {
                ProfileSharingInvitationID = Convert.ToInt32(x["ProfileSharingInvitationID"]),
                ComplianceCategoryID = x["ComplianceCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(x["ComplianceCategoryID"]),
                CategoryName = Convert.ToString(x["CategoryName"]),
                PackageSubscriptionID = x["PackageSubscriptionID"] == DBNull.Value ? 0 : Convert.ToInt32(x["PackageSubscriptionID"]),
            }).ToList();
        }

        /// <summary>
        /// Get passport report data
        /// </summary>
        /// <param name="invitationIdsforPassportReport"></param>
        public void GetPassportReportData(List<InvitationIDsContract> invitationIDsContract, Int32 tenantID)
        {
            //View.PassportReportData = AssignValuesToPassportReportDataModel(ProfileSharingManager.GetPassportReportData(View.SelectedTenantId, invitationIDsContract));
            View.PassportReportData = AssignValuesToPassportReportDataModel(ProfileSharingManager.GetPassportReportData(tenantID, invitationIDsContract));
        }

        public void GetAttestationDocumentData(List<InvitationIDsContract> invitationIDsContract)
        {
            View.AttestationDocumentData = ProfileSharingManager.GetAttestationDocumentData(invitationIDsContract);
        }

        private List<InvitationDocumentContract> AssignValuesToPassportReportDataModel(DataTable dtTable)
        {
            IEnumerable<DataRow> rows = dtTable.AsEnumerable();
            return rows.Select(x => new InvitationDocumentContract
            {
                ProfileSharingInvitationID = x["ProfileSharingInvitationID"] == DBNull.Value ? 0 : Convert.ToInt32(x["ProfileSharingInvitationID"]),
                PackageSubscriptionID = x["PackageSubscriptionID"] == DBNull.Value ? 0 : Convert.ToInt32(x["PackageSubscriptionID"]),
                ComplianceCategoryID = x["ComplianceCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(x["ComplianceCategoryID"]),
                CategoryName = Convert.ToString(x["CategoryName"]),
                SnapshotID = x["SnapshotID"] == DBNull.Value ? 0 : Convert.ToInt32(x["SnapshotID"]),
                IsInvitationSourceApplicant = Convert.ToBoolean(x["IsInvitationSourceApplicant"]),
                Name = Convert.ToString(x["FirstName"]) + " " + Convert.ToString(x["LastName"]),
                CompliancePackageID = x["CompliancePackageID"] == DBNull.Value ? 0 : Convert.ToInt32(x["CompliancePackageID"]),
                PackageName = Convert.ToString(x["PackageName"])
            }).ToList();
        }

        #region Rotations

        private ClinicalRotationProxy _clientRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        #endregion

        public void GetRotationReviewStatus()
        {
            List<Entity.SharedDataEntity.lkpSharedUserInvitationReviewStatu> lstSharedUserInvitationReviewStatus = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpSharedUserInvitationReviewStatu>().Where(cond => !cond.SUIRS_IsDeleted).ToList();

            View.lstInvitationReviewStatus = lstSharedUserInvitationReviewStatus.Select(col => new SharedUserInvitationReviewStatusContract
            {
                Code = col.SUIRS_Code,
                Description = col.SUIRS_Description,
                IsDeleted = col.SUIRS_IsDeleted,
                Name = col.SUIRS_Name,
                ReviewStatusID = col.SUIRS_ID
            }).ToList();
        }

        public bool SaveUpdateReviewStatus()
        {
            List<Int32> lstSelectedInvitationIds = View.SelectedInvitationIds.Select(cond => cond.Key).ToList();
            return ProfileSharingManager.SaveUpdateSharedUserInvitationReviewStatus(lstSelectedInvitationIds, View.CurrentLoggedInUserId, View.CurrentLoggedInUserId, View.SelectedReviewStatusID);
        }

        public Boolean UpdateViewRemaining(Dictionary<Int32, String> selectedInvitationIds)
        {
            //List<Int32> lstSelectedInvitationIds = View.SelectedInvitationIds.Select(cond => cond.Key).ToList();
            List<Int32> lstSelectedInvitationIds = selectedInvitationIds.Select(cond => cond.Key).ToList();
            return ProfileSharingManager.UpdateViewRemaining(lstSelectedInvitationIds, View.CurrentLoggedInUserId);
        }

        #region UAT-1641:As an Agency User, I should be able to be linked to multiple agencies
        public void GetAgencyList()
        {
            List<INTSOF.ServiceDataContracts.Modules.Common.AgencyDetailContract> lstAgencyTemp = new List<INTSOF.ServiceDataContracts.Modules.Common.AgencyDetailContract>();
            
            if (!View.lstSelectedTenants.IsNullOrEmpty())
            {
                List<Int32> selectedTenantIDs = View.lstSelectedTenants.Select(x => x.TenantID).ToList();
                lstAgencyTemp = ProfileSharingManager.GetInstitutionMappedAgency(selectedTenantIDs, View.UserID, true, View.CurrentLoggedInUserId);
            }
            View.lstAgency = lstAgencyTemp;
        }
        #endregion

        #region UAT-1844:

        public Boolean IsNotApprovedReviewStatusSelected(Int32 reviewStatusId)
        {
            Boolean IsNotApprovedReviewStatusSelected=false;
            List<Entity.SharedDataEntity.lkpSharedUserInvitationReviewStatu> lstSharedUserInvitationReviewStatus = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpSharedUserInvitationReviewStatu>().ToList();
            if (!lstSharedUserInvitationReviewStatus.IsNullOrEmpty())
            {
                Entity.SharedDataEntity.lkpSharedUserInvitationReviewStatu sharedUserInvReviewStatus_NotApproved = lstSharedUserInvitationReviewStatus.FirstOrDefault(x => x.SUIRS_ID == reviewStatusId && !x.SUIRS_IsDeleted);
                if (!sharedUserInvReviewStatus_NotApproved.IsNullOrEmpty() 
                    && String.Compare(sharedUserInvReviewStatus_NotApproved.SUIRS_Code, SharedUserInvitationReviewStatus.NOT_APPROVED.GetStringValue(), true) == 0)
                {
                    IsNotApprovedReviewStatusSelected = true;
                }
            }
            return IsNotApprovedReviewStatusSelected;
        }
        #endregion
    }
}
