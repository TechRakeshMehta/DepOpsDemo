using Business.RepoManagers;
using Entity.ClientEntity;
using Entity.SharedDataEntity;
using INTSOF.Contracts;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.ServiceUtil;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.UI.Contract.Templates;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;


namespace CoreWeb.ProfileSharing.Views
{
    public class RequirementSharesPresenter : Presenter<IRequirementShares>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        /// <summary>
        /// On View Initialized Event
        /// </summary>
        public override void OnViewInitialized()
        {
            List<TenantDetailContract> tenants = GetTenants();
            List<Int32> sharedUserTenantIDs = new List<Int32>();
            sharedUserTenantIDs = GetSharedUserTenantIds();
            var tenantList = tenants.Where(x => sharedUserTenantIDs.Contains(x.TenantID)).ToList();
            View.lstTenants = tenantList;
            var tenantID = tenants.FirstOrDefault().TenantID;
            GetWeekDays(tenantID);
            GetRotationReviewStatus(tenantID);
            GetRotationStatus(tenantID);
            GetInviteeTypes();
            GetInvitationReviewStatus();
            GetAgencyList();
            GetInvitationArchiveStateList(); //UAT-3470
        }

        private void GetInviteeTypes()
        {
            View.lstInviteeType = LookupManager.GetSharedDBLookUpData<lkpInvitationSource>().Where(x => x.IsDeleted == false)
                   .Select(cond => new LookupContract()
                   {
                       Name = cond.InvitationSourceType,
                       Code = cond.Code
                   }).OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// On View Loaded Event
        /// </summary>
        public override void OnViewLoaded()
        {

        }

        public void GetRotationDetail()
        {
            List<RequirementSharesDataContract> resultData;
            //UAT-2183
            string tenantIds = string.Empty;
            if (!View.IsControlVisible)
            {
                if (View.lstSelectedTenants.IsNullOrEmpty())
                {
                    //by default all tenants When no tenant is checked.
                    tenantIds = String.Join(",", View.lstTenants.Select(n => n.TenantID).ToList());
                }
                else
                {
                    tenantIds = String.Join(",", View.lstSelectedTenants.Select(n => n.TenantID).ToList());
                }
            }
            else
            {
                //UAT-2183
                if (View.IsReturnFromStudentDetailScren && View.lstSelectedTenants.IsNullOrEmpty())
                {
                    tenantIds = String.Join(",", View.lstTenants.Select(x => x.TenantID).ToList());
                }
                else
                {
                    tenantIds = String.Join(",", View.lstSelectedTenants.Select(n => n.TenantID).ToList());
                }
            }
            //UAT-2998
            if (tenantIds.IsNullOrEmpty() && !View.StrSelectedTenantIds.IsNullOrEmpty())
            {
                tenantIds = View.StrSelectedTenantIds;
            }

            StringBuilder customAttributes = new StringBuilder();
            if (!HttpContext.Current.Session["dicCustomAttributes"].IsNullOrEmpty())
            {
                Dictionary<Int32, string> dicCustomAttributes = new Dictionary<int, string>();
                dicCustomAttributes = (Dictionary<Int32, string>)HttpContext.Current.Session["dicCustomAttributes"];
                if (!dicCustomAttributes.IsNullOrEmpty())
                {
                    foreach (var item in dicCustomAttributes)
                    {
                        customAttributes.Append(item.Value);
                    }
                }
            }
            string customAttribute = customAttributes.ToString();

            resultData = ProfileSharingManager.GetRequirementSharesData(View.UserID, View.OrganizationUserId,
                               tenantIds, View.ClinicalRotationSearchContract, View.InvitationSearchContract, View.GridCustomPaging, customAttribute);

            View.RotationAndInvitationData = resultData;

            if (resultData.IsNullOrEmpty())
            {
                View.VirtualPageCount = AppConsts.NONE;
                View.CurrentPageIndex = 1;
            }
            else
            {
                View.VirtualPageCount = resultData[0].TotalRecordCount;
                View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
            }
        }

        /// <summary>
        /// Method To get all Tenants
        /// </summary>
        public List<TenantDetailContract> GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
            serviceRequest.Parameter1 = SortByName;
            serviceRequest.Parameter2 = clientCode;
            var _serviceResponse = _clinicalRotationProxy.GetTenants(serviceRequest);
            return _serviceResponse.Result;
        }

        public void GetWeekDays(Int32 tenantID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = tenantID;
            var _serviceResponse = _clinicalRotationProxy.GetWeekDayList(serviceRequest);
            View.WeekDayList = _serviceResponse.Result;
        }

        public void GetRotationStatus(Int32 tenantID)
        {
            List<SharedUserRotationReviewStatusContract> rotationStatusList = new List<SharedUserRotationReviewStatusContract>()
            {
                new SharedUserRotationReviewStatusContract { Code = RotationStatus.Active.GetStringValue(), Name = "Active" },
                new SharedUserRotationReviewStatusContract { Code = RotationStatus.Completed.GetStringValue(), Name = "Completed" }
            };
            View.RotationStatusList = rotationStatusList;
        }

        public List<Int32> GetSharedUserTenantIds()
        {
            ServiceRequest<List<String>> serviceRequest = new ServiceRequest<List<String>>();
            serviceRequest.Parameter = View.SharedUserTypeCodes;
            var _serviceResponse = _clinicalRotationProxy.GetSharedUserTenantIDs(serviceRequest);
            return _serviceResponse.Result;
        }

        public Int32 GetRequirementSubscriptionIdByClinicalRotID(String clinicalRotationID, String tenantID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = Convert.ToInt32(clinicalRotationID);
            serviceRequest.SelectedTenantId = Convert.ToInt32(tenantID);
            var _serviceResponse = _clinicalRotationProxy.GetRequirementSubscriptionIdByClinicalRotID(serviceRequest);
            return _serviceResponse.Result;
        }

        public void GetRotationReviewStatus(Int32 tenantID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = tenantID;
            var _serviceResponse = _clinicalRotationProxy.GetRotationReviewStatus(serviceRequest);
            View.lstRotationReviewStatus = _serviceResponse.Result;
        }

        public void GetInvitationReviewStatus()
        {
            List<lkpSharedUserInvitationReviewStatu> lstSharedUserInvitationReviewStatus = LookupManager.GetSharedDBLookUpData<lkpSharedUserInvitationReviewStatu>().Where(cond => !cond.SUIRS_IsDeleted).ToList();
            String sharedUserInvDroppedReviewStatusCode = SharedUserInvitationReviewStatus.Dropped.GetStringValue(); //UAT-4460
            View.lstInvitationReviewStatus = lstSharedUserInvitationReviewStatus.Where(cond=>cond.SUIRS_Code != sharedUserInvDroppedReviewStatusCode).Select(col => new SharedUserInvitationReviewStatusContract
            {
                Code = col.SUIRS_Code,
                Description = col.SUIRS_Description,
                IsDeleted = col.SUIRS_IsDeleted,
                Name = col.SUIRS_Name,
                ReviewStatusID = col.SUIRS_ID
            }).ToList();
        }

        public void GetAttestationDocumentsToExport(Int32 rotationID, Int32 tenantID, Int32 agencyID)
        {
            ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>> serviceRequest = new ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>>();
            serviceRequest.Parameter1 = new Dictionary<String, Int32> { { AppConsts.ROTATION_ID, rotationID } };
            serviceRequest.Parameter2 = new List<Tuple<Int32, Int32, Int32>> { new Tuple<Int32, Int32, Int32>(rotationID, tenantID, agencyID) };
            View.LstInvitationDocumentContract = _clinicalRotationProxy.GetAttestationDocumentsToExport(serviceRequest).Result;
        }

        /// <summary>
        ///  UAT-2035 Get checked rotation details to export attestation
        /// </summary>
        public void GetCheckedAttestationDocumentsToExport(Int32 rotationId)
        {
            ServiceRequest<Dictionary<String, List<Int32>>> serviceRequest = new ServiceRequest<Dictionary<String, List<Int32>>>();
            serviceRequest.Parameter = new Dictionary<String, List<Int32>> { { AppConsts.ROTATION_ID, View.SelectedRotationIds.Where(con => con == rotationId).ToList() } };
            View.LstInvitationDocumentContract = ClinicalRotationManager.GetSelectedAttestationDocumentsToExport(serviceRequest, View.OrganizationUserId, View.lstRotationIdsDetailContract.Where(con => con.RotationID == rotationId).ToList());
        }
        public void GetApplicantInviteDocuments(List<InvitationIDsContract> selectedInvitationIDsList, Int32 tenantID)
        {
            List<InvitationDocumentContract> documentToExport = new List<InvitationDocumentContract>();

            var applicantInvitationDocList = ProfileSharingManager.GetApplicantInviteDocuments(tenantID
                                                                                               , selectedInvitationIDsList);
            //Commented for UAT-2475  getting the data from single sp
            //var adminInvitationDocList = ProfileSharingManager.GetClientInviteDocuments(tenantID
            //                                                                            , selectedInvitationIDsList.Where(cond => !cond.IsInvitationSourceApplicant)
            //                                                                            .ToList());

            if (applicantInvitationDocList.IsNotNull())
            {
                documentToExport.AddRange(applicantInvitationDocList);
            }

            //Commented for UAT-2475  getting the data from single sp
            //if (adminInvitationDocList.IsNotNull())
            //{
            //    documentToExport.AddRange(adminInvitationDocList);
            //}

            View.DocumentListToExport = documentToExport;
        }

        public void GetPassportReportData(List<InvitationIDsContract> invitationIDsContract, Int32 tenantID)
        {

            View.PassportReportData = AssignValuesToPassportReportDataModel(ProfileSharingManager.GetPassportReportData(tenantID, invitationIDsContract));
        }
        //UAT:2475
        public void GetPassportReportDataForRotaiton(List<InvitationIDsContract> invitationIDsContract, Int32 tenantID)
        {
            View.PassportReportData = ProfileSharingManager.GetPassportReportDataForRotation(tenantID, invitationIDsContract);
        }


        //UAT:2475
        public List<InvitationIDsDetailContract> GetProfileSharingInvitationIdsByRotationId(int rotationID, Int32 tenantIds)
        {
            return ProfileSharingManager.GetPassportReportDataForParticularRotation(rotationID, View.OrganizationUserId, tenantIds);
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

        public Boolean UpdateViewRemaining(List<Int32> lstSelectedInvitationIds)
        {
            //Code commented for UAT-2276 
            //List<Int32> lstSelectedInvitationIds = selectedInvitationIds.Select(cond => cond.Key).ToList();
            return ProfileSharingManager.UpdateViewRemaining(lstSelectedInvitationIds, View.CurrentLoggedInUserId);
        }

        public Boolean IsNotApprovedReviewStatusSelected(Int32 reviewStatusId)
        {
            Boolean IsNotApprovedReviewStatusSelected = false;
            List<lkpSharedUserInvitationReviewStatu> lstSharedUserInvitationReviewStatus = LookupManager.GetSharedDBLookUpData<lkpSharedUserInvitationReviewStatu>().ToList();
            if (!lstSharedUserInvitationReviewStatus.IsNullOrEmpty())
            {
                lkpSharedUserInvitationReviewStatu sharedUserInvReviewStatus_NotApproved = lstSharedUserInvitationReviewStatus.FirstOrDefault(x => x.SUIRS_ID == reviewStatusId && !x.SUIRS_IsDeleted);
                if (!sharedUserInvReviewStatus_NotApproved.IsNullOrEmpty()
                    && String.Compare(sharedUserInvReviewStatus_NotApproved.SUIRS_Code, SharedUserInvitationReviewStatus.NOT_APPROVED.GetStringValue(), true) == 0)
                {
                    IsNotApprovedReviewStatusSelected = true;
                }
            }
            return IsNotApprovedReviewStatusSelected;
        }

        public bool SaveUpdateReviewStatus()
        {
            lkpSharedUserInvitationReviewStatu lstSharedUserInvitationReviewStatus = LookupManager.GetSharedDBLookUpData<lkpSharedUserInvitationReviewStatu>().Where(cond => !cond.SUIRS_IsDeleted && cond.SUIRS_ID == View.SelectedReviewStatusID).FirstOrDefault();

            List<Int32> lstSelectedInvitationIds = View.SelectedInvitationIds.Select(cond => cond.Key).ToList();
            //return ProfileSharingManager.SaveUpdateSharedUserInvitationReviewStatus(lstSelectedInvitationIds, View.CurrentLoggedInUserId, View.CurrentLoggedInUserId, View.SelectedReviewStatusID);

            //Creating dictory containing rotation ids and corresponding invitation ids
            List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData = new List<Tuple<Int32, Int32, Int32, List<Int32>>>();
            Boolean isIndividualReview = true;

            if (!View.SelectedRotationIds.IsNullOrEmpty())
            {
                lstRotationData = ProfileSharingManager.GetRotationSharedInvitations(View.lstRotationIdsDetailContract, View.OrganizationUserId); //If user select multiple rotation(s) from requirement shares screen
                isIndividualReview = false;
            }

            int tenantId = 0;
            if (!View.lstSelectedTenants.IsNullOrEmpty())
                tenantId = View.lstSelectedTenants[0].TenantID;

            Dictionary<String, String> dicStatusCodeAndNotes = new Dictionary<String, String>();
            dicStatusCodeAndNotes.Add(AppConsts.DIC_KEY_REVIEW_STATUS, !lstSharedUserInvitationReviewStatus.IsNullOrEmpty() ? lstSharedUserInvitationReviewStatus.SUIRS_Code : string.Empty);
            dicStatusCodeAndNotes.Add(AppConsts.DIC_KEY_NOTES, string.Empty);
            dicStatusCodeAndNotes.Add(AppConsts.DIC_KEY_SCREEN_TYPE, AppConsts.REQUIREMENT_SHARES_SCREEN_NAME);//UAT-2463
            dicStatusCodeAndNotes.Add(AppConsts.CURRENT_USER_ID_QUERY_STRING, Convert.ToString(View.CurrentLoggedInUserId));


            ServiceRequest<Dictionary<String, String>, List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean> serviceRequest = new ServiceRequest<Dictionary<String, String>, List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean>();
            serviceRequest.Parameter1 = dicStatusCodeAndNotes;
            serviceRequest.Parameter2 = lstSelectedInvitationIds;
            serviceRequest.Parameter3 = lstRotationData;
            serviceRequest.Parameter4 = isIndividualReview;
            serviceRequest.SelectedTenantId = tenantId;
            var _serviceResponse = _clinicalRotationProxy.UpdateRotAndInvitationReviewStatus(serviceRequest);
            Boolean Result = _serviceResponse.Result;

            if (Result)
            {
                //UAt-2538
                CallParallelTaskForMail(lstRotationData, lstSharedUserInvitationReviewStatus.SUIRS_Code, lstSelectedInvitationIds); // lstSelectedInvitationIds added as per UAT-2942
            }
            return Result;


        }

        public void GetAgencyList()
        {

            List<INTSOF.ServiceDataContracts.Modules.Common.AgencyDetailContract> lstAgencyTemp = new List<INTSOF.ServiceDataContracts.Modules.Common.AgencyDetailContract>();
            List<Int32> selectedTenantIDs = null;
            if (!View.lstSelectedTenants.IsNullOrEmpty())
            {
                selectedTenantIDs = View.lstSelectedTenants.Select(x => x.TenantID).ToList();
            }
            //UAT-2183
            else
            {
                selectedTenantIDs = View.lstTenants.Select(x => x.TenantID).ToList();
            }
            lstAgencyTemp = ProfileSharingManager.GetInstitutionMappedAgency(selectedTenantIDs, View.UserID, true, View.OrganizationUserId);
            View.lstAgency = lstAgencyTemp;
        }

        #region UAT-3425
        /// <summary>
        /// Save Invitation Expiration Request
        /// </summary>
        public void SaveInvExpirationRequest()
        {
            List<Int32> selectedInvitationList = View.SelectedInvitationIds.Select(cond => cond.Key).ToList(); ;
            ServiceRequest<List<Int32>, Int32?> serviceRequest = new ServiceRequest<List<Int32>, Int32?>();
            serviceRequest.Parameter1 = selectedInvitationList;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            var _serviceResponse = _clinicalRotationProxy.UpdateInvitationExpirationRequirementShares(serviceRequest);
        }
        #endregion

        #region UAT-2538

        public void CallParallelTaskForMail(List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData, String reviewStatusCode, List<Int32> lstSelectedInvitationIds) // lstSelectedInvitationIds added as per UAT-2942
        {
            Int32 CurrentLoggedInUserID = View.CurrentLoggedInUserId;
            String CurrentUserName = View.CurrentUserFirstName + " " + View.CurrentUserLastName;

            Dictionary<String, Object> Data = new Dictionary<String, Object>();
            Data.Add("lstRotationData", lstRotationData);
            Data.Add("CurrentLoggedInUserID", CurrentLoggedInUserID);
            Data.Add("CurrentUserName", CurrentUserName);
            Data.Add("reviewStatusCode", reviewStatusCode);
            Data.Add("lstSelectedInvitationIds", lstSelectedInvitationIds);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            ParallelTaskContext.PerformParallelTask(SendMailForRotInvAppRej, Data, LoggerService, ExceptiomService);
        }
        public void SendMailForRotInvAppRej(Dictionary<String, Object> data)
        {
            List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData = (List<Tuple<Int32, Int32, Int32, List<Int32>>>)(data.GetValue("lstRotationData"));
            String reviewStatusCode = Convert.ToString(data.GetValue("reviewStatusCode"));
            Int32 CurrentLoggedInUserID = Convert.ToInt32(data.GetValue("CurrentLoggedInUserID"));
            String CurrentUserName = Convert.ToString(data.GetValue("CurrentUserName"));
            List<Int32> lstSelectedInvitationIds = (List<Int32>)(data.GetValue("lstSelectedInvitationIds"));
            ProfileSharingManager.SendMailForRotInvAppRej(lstRotationData, reviewStatusCode, CurrentLoggedInUserID, CurrentUserName);
            ProfileSharingManager.SendNotificationToApplicantOnRequirementApproved(lstRotationData, reviewStatusCode, CurrentLoggedInUserID, CurrentUserName);

            #region UAT-2942
            if (!lstSelectedInvitationIds.IsNullOrEmpty() && lstSelectedInvitationIds.Count > AppConsts.NONE)
            {
                ProfileSharingManager.SendMailForApprovedApplicantProfileInvitation(lstSelectedInvitationIds, reviewStatusCode, CurrentLoggedInUserID, CurrentUserName);
            }
			#endregion

			#region UAT-3606
			//Notification for Applicant who used profile share with agency dropdown when agency user has marked the share approved
			if (lstRotationData.IsNullOrEmpty() )
			{
				ProfileSharingManager.SendNotificationToApplicantOnNonRequirementApproved(lstSelectedInvitationIds, reviewStatusCode, CurrentLoggedInUserID, CurrentUserName);
			}
			#endregion
		}


		#endregion

		#region UAT-3220
		public Boolean HideRequirementSharesDetailLink(Guid userID)
        {
            ServiceRequest<Guid> serviceRequest = new ServiceRequest<Guid>();
            serviceRequest.Parameter = userID;
            var _serviceResponse = _clinicalRotationProxy.HideRequirementSharesDetailLink(serviceRequest);
            return _serviceResponse.Result;
        }
        #endregion

        #region UAT-3470
        public void GetInvitationArchiveStateList()
        {
            View.lstInvitationArchiveState = ProfileSharingManager.GetinvitationArchiveStateList();
        }

        public bool SaveUpdateInvitationArchiveState(Boolean IsPerformArchiveOperation)
        {
            List<Int32> lstSelectedInvitationIds = View.SelectedInvitationIds.Select(cond => cond.Key).ToList();

            //Creating dictory containing rotation ids and corresponding invitation ids
            List<Tuple<Int32, Int32, Int32, List<Int32>>> lstRotationData = new List<Tuple<Int32, Int32, Int32, List<Int32>>>();

            if (!View.SelectedRotationIds.IsNullOrEmpty())
            {
                lstRotationData = ProfileSharingManager.GetRotationSharedInvitations(View.lstRotationIdsDetailContract, View.OrganizationUserId); //If user select multiple rotation(s) from requirement shares screen
            }

            int tenantId = 0;
            if (!View.lstSelectedTenants.IsNullOrEmpty())
                tenantId = View.lstSelectedTenants[0].TenantID;

            ServiceRequest<List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean> serviceRequest = new ServiceRequest<List<Int32>, List<Tuple<Int32, Int32, Int32, List<Int32>>>, Boolean>();
            serviceRequest.Parameter1 = lstSelectedInvitationIds;
            serviceRequest.Parameter2 = lstRotationData;
            serviceRequest.Parameter3 = IsPerformArchiveOperation;
            serviceRequest.SelectedTenantId = tenantId;
            var _serviceResponse = _clinicalRotationProxy.SaveUpdateInvitationArchiveState(serviceRequest);
            return _serviceResponse.Result;
        }
        #endregion

        #region UAT-4399
        public byte[] GetRotationDetailSummaryReport(String InvitationId, String TenantId, String ReportName, String tempFilePath, String rotationID, String agencyID)
        {
            byte[] reportContent = ProfileSharingManager.GetRotationDetailSummaryReport(InvitationId, TenantId, ReportName, tempFilePath, rotationID, agencyID);
            return reportContent;
        }
        #endregion
    }
}
