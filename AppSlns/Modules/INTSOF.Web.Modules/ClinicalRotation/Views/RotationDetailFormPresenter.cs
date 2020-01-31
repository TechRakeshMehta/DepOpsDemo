using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ClinicalRotation;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreWeb.ClinicalRotation.Views
{
    public class RotationDetailFormPresenter : Presenter<IRotationDetailFormView>
    {
        private ClinicalRotationProxy _clientRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }

        /// <summary>
        /// Get Clinical Rotation data to bind Rotation Detail section
        /// </summary>
        public void BindRotationDetail()
        {
            ServiceRequest<Int32, Int32?> serviceRequest = new ServiceRequest<Int32, Int32?>();
            serviceRequest.SelectedTenantId = View.SelectedTenantId;
            serviceRequest.Parameter1 = View.ClinicalRotationID;
            serviceRequest.Parameter2 = (Int32?)null;
            var _serviceResponse = _clientRotationProxy.GetClinicalRotationById(serviceRequest);
            View.ClinicalRotationDetails = _serviceResponse.Result;

            if (!View.ClinicalRotationDetails.IsNullOrEmpty())
                View.RotationAgencyIds = View.ClinicalRotationDetails.AgencyIDs;
            else
                View.RotationAgencyIds = string.Empty;
        }

        /// <summary>
        /// Gets Clinical Rotation Members
        /// </summary>
        public void GetClinicalRotationMembers()
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.SelectedTenantId = View.SelectedTenantId;
            serviceRequest.Parameter1 = View.ClinicalRotationID;
            serviceRequest.Parameter2 = View.AgencyId;
            var _serviceResponse = _clientRotationProxy.GetClinicalRotationMembers(serviceRequest);
            View.RotationMemberDetailList = _serviceResponse.Result;
            //UAT-2544
            View.RotationMemberDetailList.ForEach(cnd =>
            {
                cnd.IsRotationStart = View.IsRotationStart;
            });

        }

        /// <summary>
        /// To perform search
        /// </summary>
        public void PerformSearch()
        {
            View.ApplicantSearchData = null;

            if (!View.IsSearchClicked)
            {
                View.ApplicantSearchData = new List<ApplicantDataListContract>();
                return;
            }

            if (View.SelectedTenantId == 0)
            {
                View.ApplicantSearchData = new List<ApplicantDataListContract>();
            }
            else
            {
                ClinicalRotationSearchContract searchDataContract = new ClinicalRotationSearchContract();
                searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
                searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
                searchDataContract.EmailAddress = String.IsNullOrEmpty(View.EmailAddress) ? null : View.EmailAddress;
                searchDataContract.ApplicantSSN = ApplicationDataManager.GetSSNForFilters(View.SSN);
                searchDataContract.DateOfBirth = View.DateOfBirth;
                if (View.FilterUserGroupId > SysXDBConsts.NONE)
                {
                    searchDataContract.FilterUserGroupID = View.FilterUserGroupId;
                }
                if (View.TenantId != SecurityManager.DefaultTenantID)
                {
                    searchDataContract.LoggedInUserId = View.CurrentLoggedInUserId;
                }
                searchDataContract.LoggedInUserTenantId = View.TenantId;
                searchDataContract.SelectedDPMIds = View.SelectedDPMIds;

                try
                {
                    //View.ApplicantSearchData = ClinicalRotationManager.GetApplicantClinicalRotationSearch(View.SelectedTenantId, View.ClinicalRotationID, searchDataContract, View.GridCustomPaging);
                    ServiceRequest<Int32, ClinicalRotationSearchContract, CustomPagingArgsContract> serviceRequest =
                            new ServiceRequest<Int32, ClinicalRotationSearchContract, CustomPagingArgsContract>();
                    serviceRequest.SelectedTenantId = View.SelectedTenantId;
                    serviceRequest.Parameter1 = View.ClinicalRotationID;
                    serviceRequest.Parameter2 = searchDataContract;
                    serviceRequest.Parameter3 = View.GridCustomPaging;
                    var _serviceResponse = _clientRotationProxy.GetApplicantClinicalRotationSearch(serviceRequest);
                    View.ApplicantSearchData = _serviceResponse.Result;


                    if (View.ApplicantSearchData.IsNotNull() && View.ApplicantSearchData.Count > 0)
                    {
                        if (View.ApplicantSearchData[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.ApplicantSearchData[0].TotalCount;
                        }
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                    }
                    else
                    {
                        View.VirtualRecordCount = 0;
                        View.CurrentPageIndex = 1;
                    }

                    //foreach (var record in View.RotationMemberDetailList)
                    //{
                    //    var alreadyAssignedRecord = View.ApplicantSearchData.FirstOrDefault(x => x.OrganizationUserId == record.applicantData.OrganizationUserId);
                    //    if (alreadyAssignedRecord != null)
                    //    {
                    //        View.ApplicantSearchData.Remove(alreadyAssignedRecord);
                    //    }
                    //}
                }
                catch (Exception e)
                {
                    View.ApplicantSearchData = null;
                    throw e;
                }
            }
        }


        /// <summary>
        /// Get all GetAllUserIds for UAT-2887: Add Select all records to assign to rotation screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GetAllUserIds()
        {
            if (!View.IsSearchClicked)
            {
                View.ApplicantSearchData = new List<ApplicantDataListContract>();
                return;
            }

            if (View.SelectedTenantId == 0)
            {
                View.ApplicantSearchData = new List<ApplicantDataListContract>();
            }
            else
            {
                ClinicalRotationSearchContract searchDataContract = new ClinicalRotationSearchContract();
                searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
                searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
                searchDataContract.EmailAddress = String.IsNullOrEmpty(View.EmailAddress) ? null : View.EmailAddress;
                searchDataContract.ApplicantSSN = ApplicationDataManager.GetSSNForFilters(View.SSN);
                searchDataContract.DateOfBirth = View.DateOfBirth;
                if (View.FilterUserGroupId > SysXDBConsts.NONE)
                {
                    searchDataContract.FilterUserGroupID = View.FilterUserGroupId;
                }
                if (View.TenantId != SecurityManager.DefaultTenantID)
                {
                    searchDataContract.LoggedInUserId = View.CurrentLoggedInUserId;
                }
                searchDataContract.LoggedInUserTenantId = View.TenantId;
                searchDataContract.SelectedDPMIds = View.SelectedDPMIds;

                try
                {
                    ServiceRequest<Int32, ClinicalRotationSearchContract, CustomPagingArgsContract> serviceRequest = new ServiceRequest<Int32, ClinicalRotationSearchContract, CustomPagingArgsContract>();
                    serviceRequest.SelectedTenantId = View.SelectedTenantId;
                    serviceRequest.Parameter1 = View.ClinicalRotationID;
                    serviceRequest.Parameter2 = searchDataContract;
                    View.GridCustomPaging.CurrentPageIndex = 1;
                    View.GridCustomPaging.PageSize = View.VirtualRecordCount;
                    serviceRequest.Parameter3 = View.GridCustomPaging;

                    var _serviceResponse = _clientRotationProxy.GetApplicantClinicalRotationSearch(serviceRequest);

                    if (!_serviceResponse.IsNullOrEmpty()
                        && !_serviceResponse.Result.IsNullOrEmpty()
                        && _serviceResponse.Result.Count > 0)
                    {
                        View.lstSelectedUserIDs = string.Join(",", _serviceResponse.Result.Select(cond => cond.OrganizationUserId).Distinct().ToList());
                    }
                }
                catch (Exception e)
                {
                    View.ApplicantSearchData = null;
                    throw e;
                }
            }
        }

        /// <summary>
        /// To get user groups
        /// </summary>
        public void GetAllUserGroups()
        {
            if (View.SelectedTenantId == 0)
                View.lstUserGroup = new List<UserGroupContract>();
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedTenantId;
                var _serviceResponse = _clientRotationProxy.GetAllUserGroup(serviceRequest);
                View.lstUserGroup = _serviceResponse.Result.OrderBy(ex => ex.UG_Name).ToList();
            }
        }

        /// <summary>
        /// To get Requirement Packages
        /// </summary>
        public void GetRequirementPackages()
        {
            if (View.SelectedTenantId == 0)
            {
                View.lstTenantRequirementPackage = new List<RequirementPackageContract>();
                View.lstSharedRequirementPackage = new List<RequirementPackageContract>();
                View.lstCombinedRequirementPackage = new List<RequirementPackageContract>();
            }
            else
            {
                ServiceRequest<String, Boolean> serviceRequestForTenantPackages = new ServiceRequest<String, Boolean>();
                serviceRequestForTenantPackages.SelectedTenantId = View.SelectedTenantId;
                serviceRequestForTenantPackages.Parameter1 = View.RotationAgencyIds;
                serviceRequestForTenantPackages.Parameter2 = false;
                ServiceResponse<List<RequirementPackageContract>> _serviceResponseForTenantPackages = _requirementPackageProxy
                                                                                                            .GetRequirementPackages(serviceRequestForTenantPackages);
                List<RequirementPackageContract> lstTenantRequirementPackage = _serviceResponseForTenantPackages.Result;

                if (!lstTenantRequirementPackage.IsNullOrEmpty())
                {
                    lstTenantRequirementPackage = lstTenantRequirementPackage.Where(cond => !cond.IsCopied ||
                                                        (View.RotationRequirementPackage != null
                                                        && cond.RequirementPackageID == View.RotationRequirementPackage.RequirementPackageID)).ToList();
                }
                View.lstTenantRequirementPackage = lstTenantRequirementPackage;

                ServiceRequest<String, Boolean> serviceRequestForSharedPackages = new ServiceRequest<String, Boolean>();
                serviceRequestForSharedPackages.SelectedTenantId = View.SelectedTenantId;
                serviceRequestForSharedPackages.Parameter1 = View.RotationAgencyIds;
                serviceRequestForSharedPackages.Parameter2 = true;
                ServiceResponse<List<RequirementPackageContract>> _serviceResponseForSharedPackages = _requirementPackageProxy
                                                                                                            .GetRequirementPackages(serviceRequestForSharedPackages);
                List<RequirementPackageContract> lstSharedRequirementPackage = _serviceResponseForSharedPackages.Result;

                if (!lstSharedRequirementPackage.IsNullOrEmpty())
                {
                    lstSharedRequirementPackage = lstSharedRequirementPackage.Where(cond => View.RotationRequirementPackage != null
                                                        && cond.RequirementPackageCode.ToString().ToLower() != View.RotationRequirementPackage.RequirementPackageCode.ToString().ToLower()
                                                         && (!lstTenantRequirementPackage.Any(m => m.RootParentCode == cond.RootParentCode)
                                                         )).ToList();

                }
                View.lstSharedRequirementPackage = lstSharedRequirementPackage;

                if (View.ClinicalRotationDetails.IsNull())
                {
                    BindRotationDetail();
                }
                DateTime? RotationStartDate = View.ClinicalRotationDetails.StartDate;
                DateTime? RotationEndDate = View.ClinicalRotationDetails.EndDate;

                var CombinedPackageList = lstTenantRequirementPackage.Concat(lstSharedRequirementPackage).ToList();


                //CombinedPackageList = CombinedPackageList.Where(cond => cond.EffectiveStartDate < RotationStartDate &&
                //                                                                      (cond.EffectiveEndDate.IsNull() || cond.EffectiveEndDate > RotationEndDate)).ToList();


                CombinedPackageList = CombinedPackageList.Where(cond => (cond.EffectiveEndDate > RotationStartDate || cond.EffectiveEndDate.IsNull())
                                                                && (cond.EffectiveStartDate.IsNull() || cond.EffectiveStartDate < RotationEndDate)).ToList();

                //UAT-2514 
                View.lstCombinedRequirementPackage = null;

                #region UAT-4657
                List<RequirementPackageContract> finalPkgList = new List<RequirementPackageContract>();

                CombinedPackageList.DistinctBy(cond => cond.RootParentCode).Select(col => col.RootParentCode).ForEach(rootParentCode =>
                {
                    List<RequirementPackageContract> lstPkgListForGroup = CombinedPackageList.Where(cond => cond.RootParentCode == rootParentCode).ToList();
                    if (!lstPkgListForGroup.IsNullOrEmpty())
                    {
                        if (lstPkgListForGroup.Count == AppConsts.ONE)
                        {
                            finalPkgList.AddRange(lstPkgListForGroup);
                        }
                        else
                        {
                            DateTime? pkgHighestEffDate = lstPkgListForGroup.Where(con => con.EffectiveStartDate < RotationStartDate).Any() ?
                                                        lstPkgListForGroup.Where(con => con.EffectiveStartDate < RotationStartDate).Max(x => x.EffectiveStartDate).Value : (DateTime?)null;
                            // DateTime pkgHighestEffDate = lstPkgListForGroup.OrderByDescending(ord => ord.EffectiveStartDate).FirstOrDefault().EffectiveStartDate.Value;
                            finalPkgList.AddRange(lstPkgListForGroup.Where(con => con.EffectiveStartDate.Value == pkgHighestEffDate).ToList());
                        }
                    }
                });

                View.lstCombinedRequirementPackage = finalPkgList;


                #endregion
                //View.lstCombinedRequirementPackage = lstTenantRequirementPackage.Concat(lstSharedRequirementPackage).ToList();

                #region Commented in UAT-4657

                //View.lstSharedRequirementPackage = new List<RequirementPackageContract>();
                //View.lstTenantRequirementPackage = new List<RequirementPackageContract>();


                //lstSharedRequirementPackage.DistinctBy(cond => cond.RootParentID).Select(col => col.RootParentID).ForEach(parentId =>
                //{
                //    List<RequirementPackageContract> lstPkgListForGroup = lstSharedRequirementPackage.Where(cond => cond.RootParentID == parentId).ToList();
                //    if (!lstPkgListForGroup.IsNullOrEmpty() && lstPkgListForGroup.Any())
                //    {
                //        if (lstPkgListForGroup.Count == AppConsts.ONE)
                //        {
                //            if (!lstTenantRequirementPackage.IsNullOrEmpty() && lstTenantRequirementPackage.Count > AppConsts.NONE)
                //            {
                //                if (lstTenantRequirementPackage.Where(con => con.RequirementPackageCode == lstPkgListForGroup[0].RequirementPackageCode).Any())
                //                {
                //                    finalPkgList.Add(lstTenantRequirementPackage.Where(con => con.RequirementPackageCode == lstPkgListForGroup[0].RequirementPackageCode).FirstOrDefault());
                //                    View.lstTenantRequirementPackage.Add(lstTenantRequirementPackage.Where(con => con.RequirementPackageCode == lstPkgListForGroup[0].RequirementPackageCode).FirstOrDefault());
                //                }
                //                else
                //                {
                //                    finalPkgList.AddRange(lstPkgListForGroup);
                //                    View.lstSharedRequirementPackage.AddRange(lstPkgListForGroup);
                //                }
                //            }
                //            else
                //            {
                //                finalPkgList.AddRange(lstPkgListForGroup);
                //                View.lstSharedRequirementPackage.AddRange(lstPkgListForGroup);
                //            }
                //        }
                //        else
                //        {
                //            DateTime pkgHighestEffDate = lstPkgListForGroup.OrderByDescending(ord => ord.EffectiveStartDate).FirstOrDefault().EffectiveStartDate.Value;
                //            List<Guid> guids = lstPkgListForGroup.Where(con => con.EffectiveStartDate.Value == pkgHighestEffDate).Select(sel => sel.RequirementPackageCode).ToList();

                //            foreach (Guid item in guids)
                //            {
                //                if (lstTenantRequirementPackage.Where(con => con.RequirementPackageCode == lstPkgListForGroup[0].RequirementPackageCode).Any())
                //                {
                //                    finalPkgList.Add(lstTenantRequirementPackage.Where(con => con.RequirementPackageCode == item).FirstOrDefault());
                //                    View.lstTenantRequirementPackage.Add(lstTenantRequirementPackage.Where(con => con.RequirementPackageCode == item).FirstOrDefault());
                //                }
                //                else
                //                {
                //                    finalPkgList.Add(lstPkgListForGroup.Where(con => con.RequirementPackageCode == item).FirstOrDefault());
                //                    View.lstSharedRequirementPackage.Add(lstPkgListForGroup.Where(con => con.RequirementPackageCode == item).FirstOrDefault());
                //                }
                //            }
                //        }
                //    }
                //});


                ////  View.lstSharedRequirementPackage = lstSharedRequirementPackage;

                //View.lstCombinedRequirementPackage = finalPkgList;
                #endregion


            }
        }

        /// <summary>
        /// Get Clinical Rotation requirement package
        /// </summary>
        public void GetRotationRequirementPackage()
        {
            if (View.SelectedTenantId == 0)
                View.RotationRequirementPackage = new ClinicalRotationRequirementPackageContract();
            else
            {
                ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
                serviceRequest.SelectedTenantId = View.SelectedTenantId;
                serviceRequest.Parameter1 = View.ClinicalRotationID;
                serviceRequest.Parameter2 = RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue();
                var _serviceResponse = _clientRotationProxy.GetRotationRequirementPackage(serviceRequest);
                View.RotationRequirementPackage = _serviceResponse.Result;
            }
        }

        /// <summary>
        /// Add applicants to Clinical Rotation
        /// </summary>
        public void AddApplicantsToRotation()
        {
            ServiceRequest<Int32, Dictionary<Int32, Boolean>> serviceRequest = new ServiceRequest<Int32, Dictionary<Int32, Boolean>>();
            serviceRequest.SelectedTenantId = View.SelectedTenantId;
            serviceRequest.Parameter1 = View.ClinicalRotationID;
            serviceRequest.Parameter2 = View.AssignOrganizationUserIds;
            var _serviceResponse = _clientRotationProxy.AddApplicantsToRotation(serviceRequest);

            if (_serviceResponse.Result)
            {
                View.SuccessMessage = "Applicant(s) assigned to rotation successfully.";
            }
            else
            {
                View.ErrorMessage = "Some error occurred. Please try again.";
            }
        }

        /// <summary>
        /// Remove applicants from rotation
        /// </summary>
        public void RemoveApplicantsFromRotation()
        {
            ServiceRequest<Dictionary<Int32, Boolean>, List<Int32>> serviceRequest = new ServiceRequest<Dictionary<Int32, Boolean>, List<Int32>>();
            Dictionary<Int32, Boolean> RemovedClinicalRotationMemberIds = new Dictionary<Int32, Boolean>();
            View.RemovedClinicalRotationMemberIds.ForEach(d => RemovedClinicalRotationMemberIds.Add(d.Key, d.Value.Item1));
            //UAT-2544
            List<Int32> approvedMemberList = FilterApprovedMembersFromRemoveList();
            serviceRequest.SelectedTenantId = View.SelectedTenantId;
            serviceRequest.Parameter1 = RemovedClinicalRotationMemberIds;
            //UAT-2544
            serviceRequest.Parameter2 = approvedMemberList;
            var _serviceResponse = _clientRotationProxy.RemoveApplicantsFromRotation(serviceRequest);

            if (_serviceResponse.Result)
            {
                View.SuccessMessage = "Rotation member(s) removed successfully.";
            }
            else
            {
                View.ErrorMessage = "Some error occurred. Please try again.";
            }
        }

        public Int32 AssignPackageToRotation(Boolean isSharedPackage, Boolean isNewPackage)
        {
            Int32 packageIdToBeAssigned = AppConsts.NONE;
            if (isSharedPackage)
            {
                //Copy package data into client database
                ServiceRequest<Int32, Int32> serviceRequestPkg = new ServiceRequest<Int32, Int32>();
                serviceRequestPkg.SelectedTenantId = View.SelectedTenantId;
                serviceRequestPkg.Parameter1 = View.RequirementPackageID;
                serviceRequestPkg.Parameter2 = View.CurrentLoggedInUserId;
                //UAT-2514 Different Workflow for Copy of New and Old Packages
                if (isNewPackage)
                {
                    packageIdToBeAssigned = _requirementPackageProxy.CopySharedRequirementPackageToClientNew(serviceRequestPkg).Result;
                }
                else
                {
                    packageIdToBeAssigned = _requirementPackageProxy.CopySharedRqrmntPkgToClient(serviceRequestPkg).Result;
                }
            }

            ServiceRequest<Int32, Int32, String> serviceRequest = new ServiceRequest<Int32, Int32, String>();
            serviceRequest.SelectedTenantId = View.SelectedTenantId;
            serviceRequest.Parameter1 = View.ClinicalRotationID;
            serviceRequest.Parameter2 = packageIdToBeAssigned > AppConsts.NONE ? packageIdToBeAssigned : View.RequirementPackageID;
            serviceRequest.Parameter3 = RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue();
            var _serviceResponse = _clientRotationProxy.AddPackageToRotation(serviceRequest);
            if (packageIdToBeAssigned == AppConsts.NONE)
            {
                packageIdToBeAssigned = View.RequirementPackageID;
            }

            if (_serviceResponse.Result)
            {
                View.SuccessMessage = "Package assigned to rotation successfully.";
            }
            else
            {
                View.ErrorMessage = "Some error occurred. Please try again.";
            }

            return packageIdToBeAssigned;
        }

        /// <summary>
        /// Is Clinical Rotation Members exist for clinical rotation
        /// </summary>
        /// <returns></returns>
        public Boolean IsClinicalRotationMembersExistForRotation()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.SelectedTenantId = View.SelectedTenantId;
            serviceRequest.Parameter = View.ClinicalRotationID;
            var _serviceResponse = _clientRotationProxy.IsClinicalRotationMembersExistForRotation(serviceRequest);

            return _serviceResponse.Result;
        }

        /// <summary>
        /// Getting Masked SSN
        /// </summary>
        /// <param name="unMaskedSSN"></param>
        /// <returns></returns>
        public String GetMaskedSSN(String unMaskedSSN)
        {
            ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
            serviceRequest.Parameter = unMaskedSSN;
            var _serviceResponse = _clientRotationProxy.GetMaskedSSN(serviceRequest);
            return _serviceResponse.Result;
        }

        /// <summary>
        /// Getting Formatted SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetFormattedSSN(String unformattedSSN)
        {
            ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
            serviceRequest.Parameter = unformattedSSN;
            var _serviceResponse = _clientRotationProxy.GetFormattedSSN(serviceRequest);
            return _serviceResponse.Result;
        }

        /// <summary>
        /// To get Requirement Packages
        /// </summary>
        public void GetInstructorRequirementPackages()
        {
            if (View.SelectedTenantId == 0)
            {
                View.lstInstructorRequirementPackage = new List<RequirementPackageContract>();
                View.lstSharedInstructorRequirementPackages = new List<RequirementPackageContract>();
                View.lstCombinedInstructorRequirementPackages = new List<RequirementPackageContract>();
            }
            else
            {
                ServiceRequest<String, Boolean> serviceRequestForTenantPackages = new ServiceRequest<String, Boolean>();
                serviceRequestForTenantPackages.SelectedTenantId = View.SelectedTenantId;
                serviceRequestForTenantPackages.Parameter1 = View.RotationAgencyIds;
                serviceRequestForTenantPackages.Parameter2 = false;
                ServiceResponse<List<RequirementPackageContract>> _serviceResponseForTenantPackages = _requirementPackageProxy
                                                                                                   .GetInstructorRequirementPackages(serviceRequestForTenantPackages);
                List<RequirementPackageContract> lstTenantInstructorRequirementPackage = _serviceResponseForTenantPackages.Result;

                if (!lstTenantInstructorRequirementPackage.IsNullOrEmpty())
                {
                    lstTenantInstructorRequirementPackage = lstTenantInstructorRequirementPackage.Where(cond => !cond.IsCopied ||
                                                        (View.MappedInstructorRequirementPackage != null
                                                        && cond.RequirementPackageID == View.MappedInstructorRequirementPackage.RequirementPackageID)).ToList();
                }
                View.lstInstructorRequirementPackage = lstTenantInstructorRequirementPackage;

                ServiceRequest<String, Boolean> serviceRequestForSharedPackages = new ServiceRequest<String, Boolean>();
                serviceRequestForSharedPackages.SelectedTenantId = View.SelectedTenantId;
                serviceRequestForSharedPackages.Parameter1 = View.RotationAgencyIds;
                serviceRequestForSharedPackages.Parameter2 = true;
                ServiceResponse<List<RequirementPackageContract>> _serviceResponseForSharedPackages = _requirementPackageProxy
                                                                                                            .GetInstructorRequirementPackages(serviceRequestForSharedPackages);
                List<RequirementPackageContract> lstSharedInstructorRequirementPackage = _serviceResponseForSharedPackages.Result;

                if (!lstSharedInstructorRequirementPackage.IsNullOrEmpty())
                {
                    lstSharedInstructorRequirementPackage = lstSharedInstructorRequirementPackage.Where(cond => View.MappedInstructorRequirementPackage != null
                                                        && cond.RequirementPackageCode.ToString().ToLower() != View.MappedInstructorRequirementPackage.RequirementPackageCode.ToString().ToLower()
                                                        && (!lstTenantInstructorRequirementPackage.Any(m => m.RootParentCode == cond.RootParentCode))
                                                        ).ToList();
                }

                if (View.ClinicalRotationDetails.IsNull())
                {
                    BindRotationDetail();
                }
                DateTime? RotationStartDate = View.ClinicalRotationDetails.StartDate;
                DateTime? RotationEndDate = View.ClinicalRotationDetails.EndDate;

                //UAT-3785
                View.lstSharedRequirementPackage = lstSharedInstructorRequirementPackage.Where(cond => (cond.EffectiveEndDate > RotationStartDate || cond.EffectiveEndDate.IsNull())
                                                                && (cond.EffectiveStartDate.IsNull() || cond.EffectiveStartDate < RotationEndDate)).ToList(); ;

                var CombinedPackageList = lstTenantInstructorRequirementPackage.Concat(lstSharedInstructorRequirementPackage).ToList();


                #region UAT-4657

                //UAT-2514
                //View.lstCombinedInstructorRequirementPackages = CombinedPackageList.Where(cond => (cond.EffectiveEndDate > RotationStartDate || cond.EffectiveEndDate.IsNull())
                //                                                && (cond.EffectiveStartDate.IsNull() || cond.EffectiveStartDate < RotationEndDate)).ToList();
                //View.lstCombinedInstructorRequirementPackages = lstTenantInstructorRequirementPackage.Concat(lstSharedInstructorRequirementPackage).ToList();.


                View.lstCombinedInstructorRequirementPackages = null;

                CombinedPackageList = CombinedPackageList.Where(cond => (cond.EffectiveEndDate > RotationStartDate || cond.EffectiveEndDate.IsNull())
                                                               && (cond.EffectiveStartDate.IsNull() || cond.EffectiveStartDate < RotationEndDate)).ToList();
                List<RequirementPackageContract> finalPkgList = new List<RequirementPackageContract>();

                CombinedPackageList.DistinctBy(cond => cond.RootParentCode).Select(col => col.RootParentCode).ForEach(rootParentCode =>
                {
                    List<RequirementPackageContract> lstPkgListForGroup = CombinedPackageList.Where(cond => cond.RootParentCode == rootParentCode).ToList();
                    if (!lstPkgListForGroup.IsNullOrEmpty() && lstPkgListForGroup.Any())
                    {
                        if (lstPkgListForGroup.Count == AppConsts.ONE)
                        {
                            finalPkgList.AddRange(lstPkgListForGroup);
                        }
                        else
                        {
                            DateTime? pkgHighestEffDate = lstPkgListForGroup.Where(con => con.EffectiveStartDate < RotationStartDate).Any() ?
                                                         lstPkgListForGroup.Where(con => con.EffectiveStartDate < RotationStartDate).Max(x => x.EffectiveStartDate).Value : (DateTime?)null;
                            //DateTime pkgHighestEffDate = lstPkgListForGroup.OrderByDescending(ord => ord.EffectiveStartDate).FirstOrDefault().EffectiveStartDate.Value;
                            finalPkgList.AddRange(lstPkgListForGroup.Where(con => con.EffectiveStartDate.Value == pkgHighestEffDate).ToList());
                        }
                    }
                });

                View.lstCombinedInstructorRequirementPackages = finalPkgList;
                View.lstSharedRequirementPackage = finalPkgList;

                #endregion
            }
        }

        /// <summary>
        /// Get Clinical Rotation requirement package
        /// </summary>
        public void GetMappedInstructorRequirementPackage()
        {
            if (View.SelectedTenantId == 0)
                View.MappedInstructorRequirementPackage = new ClinicalRotationRequirementPackageContract();
            else
            {
                ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
                serviceRequest.SelectedTenantId = View.SelectedTenantId;
                serviceRequest.Parameter1 = View.ClinicalRotationID;
                serviceRequest.Parameter2 = RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue();
                var _serviceResponse = _clientRotationProxy.GetRotationRequirementPackage(serviceRequest);
                View.MappedInstructorRequirementPackage = _serviceResponse.Result;
            }
        }

        /// <summary>
        /// Assign requirement package to rotation
        /// </summary>
        public Int32 AssignInstructorPackageToRotation(Boolean isSharedPackage, Boolean isNewPackage)
        {
            Int32 packageIdToBeAssigned = AppConsts.NONE;
            if (isSharedPackage)
            {
                //Copy package data into client database
                ServiceRequest<Int32, Int32> serviceRequestPkg = new ServiceRequest<Int32, Int32>();
                serviceRequestPkg.SelectedTenantId = View.SelectedTenantId;
                serviceRequestPkg.Parameter1 = View.InstructorRequirementPackageID;
                serviceRequestPkg.Parameter2 = View.CurrentLoggedInUserId;

                //UAT-2514 Copy Package
                if (isNewPackage)
                {
                    packageIdToBeAssigned = _requirementPackageProxy.CopySharedRequirementPackageToClientNew(serviceRequestPkg).Result;
                }
                else
                {
                    packageIdToBeAssigned = _requirementPackageProxy.CopySharedRqrmntPkgToClient(serviceRequestPkg).Result;
                }

            }

            ServiceRequest<Int32, Int32, String> serviceRequest = new ServiceRequest<Int32, Int32, String>();
            serviceRequest.SelectedTenantId = View.SelectedTenantId;
            serviceRequest.Parameter1 = View.ClinicalRotationID;
            serviceRequest.Parameter2 = packageIdToBeAssigned > AppConsts.NONE ? packageIdToBeAssigned : View.InstructorRequirementPackageID;
            serviceRequest.Parameter3 = RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue();
            var _serviceResponse = _clientRotationProxy.AddPackageToRotation(serviceRequest);

            if (packageIdToBeAssigned == AppConsts.NONE)
            {
                packageIdToBeAssigned = View.InstructorRequirementPackageID;
            }

            if (_serviceResponse.Result)
            {
                View.SuccessMessage = "Package assigned to rotation successfully.";
            }
            else
            {
                View.ErrorMessage = "Some error occurred. Please try again.";
            }

            return packageIdToBeAssigned;
        }

        /// <summary>
        /// Add applicants to Clinical Rotation
        /// </summary>
        public void CreateRotationSubscrptnForClientContacts(List<Int32> contactIds, Int32 assignedPkgId)
        {
            if (View.OldInstRequirementPackageID > AppConsts.NONE && View.OldInstRequirementPackageID != View.InstructorRequirementPackageID)
            {
                ServiceRequest<Int32, Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32, Int32>();
                serviceRequest.SelectedTenantId = View.SelectedTenantId;
                serviceRequest.Parameter1 = View.ClinicalRotationID;
                serviceRequest.Parameter2 = View.OldInstRequirementPackageID;
                serviceRequest.Parameter3 = assignedPkgId > AppConsts.NONE ? assignedPkgId : View.InstructorRequirementPackageID;
                var _serviceResponse = _clientRotationProxy.UpdateRotationSubscriptionForClientContact(serviceRequest);

                if (_serviceResponse.Result)
                {
                    View.SuccessMessage = "Package assigned to rotation successfully.";
                }
                else
                {
                    View.ErrorMessage = "Some error occurred. Please try again.";
                }
            }
            else
            {
                ServiceRequest<List<Int32>, Int32> serviceRequest = new ServiceRequest<List<Int32>, Int32>();
                serviceRequest.SelectedTenantId = View.SelectedTenantId;
                serviceRequest.Parameter1 = contactIds;
                serviceRequest.Parameter2 = View.ClinicalRotationID;
                var _serviceResponse = _clientRotationProxy.CreateRotationSubscriptionForClientContact(serviceRequest);

                if (_serviceResponse.Result)
                {
                    View.SuccessMessage = "Package assigned to rotation successfully.";
                }
                else
                {
                    View.ErrorMessage = "Some error occurred. Please try again.";
                }
            }
        }

        /// <summary>
        /// Is Clinical Rotation Members exist for clinical rotation
        /// </summary>
        /// <returns></returns>
        public Boolean IfAnyContactIsMappedToRotation()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.SelectedTenantId = View.SelectedTenantId;
            serviceRequest.Parameter = View.ClinicalRotationID;
            var _serviceResponse = _clientRotationProxy.IfAnyContactIsMappedToRotation(serviceRequest);

            return _serviceResponse.Result;
        }

        /// <summary>
        /// Is Clinical Rotation Members exist for clinical rotation
        /// </summary>
        /// <returns></returns>
        public Boolean IfAnyContactHasEnteredDataForRotation(Int32 mappedPkgId)
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.SelectedTenantId = View.SelectedTenantId;
            serviceRequest.Parameter1 = mappedPkgId;
            serviceRequest.Parameter2 = View.ClinicalRotationID;
            var _serviceResponse = _clientRotationProxy.IfAnyContactHasEnteredDataForRotation(serviceRequest);

            return _serviceResponse.Result;
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// UAT-1629 : As a client admin, I should not be able to edit rotation packages
        /// </summary>
        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantId);
        }

        /// <summary>
        /// UAT-1784: Get Granular Permissions for current user
        /// </summary>
        public void GetGranularPermissions()
        {
            var _serviceResponse = _clientRotationProxy.GetGranularPermissions();
            View.dicGranularPermissions = _serviceResponse.Result;
            //ONDB:15934
            GetGranularPermissionForClientAdmins();
        }

        public void GetRequirementPackageEligibility(String selectedApplicants)
        {
            ServiceRequest<Int32, Int32, String> serviceRequest = new ServiceRequest<Int32, Int32, String>();
            serviceRequest.Parameter1 = View.SelectedTenantId;
            serviceRequest.Parameter2 = View.ClinicalRotationID;
            serviceRequest.Parameter3 = selectedApplicants;
            var _serviceResponse = _clientRotationProxy.GetRequirementPackageStatusByRotationID(serviceRequest);
            View.IsRotationPackageEligibleForSharing = _serviceResponse.Result;
        }

        public void GetComplianceStatusOfImmunizationAndRotationPackages(String selectedApplicants)
        {
            Dictionary<String, String> _applicantData = new Dictionary<String, String>();
            _applicantData.Add("DelimittedOrgUserIDs", selectedApplicants);
            _applicantData.Add("DelimittedTrackingPkgIDs", String.Empty);
            _applicantData.Add("SearchType", "ROTA");


            ServiceRequest<Int32, Dictionary<String, String>, Int32> serviceRequest = new ServiceRequest<Int32, Dictionary<String, String>, Int32>();
            serviceRequest.Parameter1 = View.SelectedTenantId;
            serviceRequest.Parameter2 = _applicantData;
            serviceRequest.Parameter3 = View.ClinicalRotationID;
            var _serviceResponse = _clientRotationProxy.GetComplianceStatusOfImmunizationAndRotationPackages(serviceRequest);

            View.LstStatusMessages = _serviceResponse.Result;
            //ProfileSharingManager.GetComplianceStatusOfImmunizationAndRotationPackages(View.TenantID, View.DelimittedOrgUserIDs, View.DelimittedTrackingPkgIDs, View.RotationID, "IMNZ");
        }

        public Dictionary<Int32, String> IsRequirementPkgCompliantReqd(String agencyIDs)
        {
            ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
            serviceRequest.Parameter = agencyIDs;

            var _serviceResponse = _clientRotationProxy.IsRequirementPkgCompliantReqd(serviceRequest);
            return _serviceResponse.Result;
        }

        #region UAT-2196, Add "Send Message" button on rotation details screen.

        public void GetSelectedOrganizatioUserIDs()
        {
            //List<Int32> tmpRotationIDs = View.RemovedClinicalRotationMemberIds.Keys.Select(x => Convert.ToInt32(x)).ToList(); // lstRotMemberIDs.Split(',').Select(x => Convert.ToInt32(x)).ToList(); View.RemovedClinicalRotationMemberIds.Keys
            //List<Int32> lstOrgUserIds = View.RemovedClinicalRotationMemberIds.Values.Select(x => Convert.ToInt32(x.Item2)).ToList();
            List<Int32> lstOrgUserIds = View.CustomMessageOrgUserIds.Keys.Select(x => Convert.ToInt32(x)).ToList();
            View.lstSelectedOrgUserIDs = GetOrganizationUserDetailsByOrgUserIDs(lstOrgUserIds);// GetOrgUserIDsListByRotationMemberIDs(tmpRotationIDs);
        }

        public void SendScheduleRotationNotification(Dictionary<String, Object> conversionData)
        {
            String orgUserIds = GetOrganizationUserIDByRotMemberIDs(Convert.ToString(conversionData["orgUserIds"]));
            Int32 tenantId = Convert.ToInt32(conversionData["tenantId"]);
            Int32 CurentLoggedInUserId = Convert.ToInt32(conversionData["CurentLoggedInUserId"]);

            String tenantName = SecurityManager.GetTenant(tenantId).TenantName;
            String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

            List<ClinicalRotationMemberDetail> lstRotationDeatils = ClinicalRotationManager.GetRotationDetailsByOrgUserIds(orgUserIds, tenantId, View.ClinicalRotationID);

            foreach (ClinicalRotationMemberDetail clinicalRotationMemberDetail in lstRotationDeatils)
            {
                //Create Dictionary
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, clinicalRotationMemberDetail.ApplicantName);
                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId.ToString());
                dictMailData.Add(EmailFieldConstants.ROTATION_NAME, clinicalRotationMemberDetail.RotationName);
                dictMailData.Add(EmailFieldConstants.ROTATION_START_DATE, Convert.ToDateTime(clinicalRotationMemberDetail.StartDate).ToString("MM/dd/yyyy"));
                dictMailData.Add(EmailFieldConstants.ROTATION_DETAILS, ClinicalRotationManager.GenerateRotationDetailsHTML(clinicalRotationMemberDetail));

                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                mockData.UserName = clinicalRotationMemberDetail.ApplicantName;
                mockData.EmailID = clinicalRotationMemberDetail.PrimaryEmailaddress;
                mockData.ReceiverOrganizationUserID = clinicalRotationMemberDetail.OrganizationUserId;

                //send assign to rotation sms --UAT-3688
                CommunicationManager.SaveDataForSMSNotification(CommunicationSubEvents.NOTIFICATION_CLINICAL_ROTATION_ASSIGNED_SMS, mockData,
                                                                new Dictionary<String, object>(), tenantId, AppConsts.NONE);

                //Send mail
                CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.ROTATION_ABOUT_TO_START, dictMailData, mockData, tenantId, -1, null, null, true, false, null, clinicalRotationMemberDetail.RotationHirarchyIds, View.ClinicalRotationID);

                //Send Message
                CommunicationManager.SaveMessageContent(CommunicationSubEvents.ROTATION_ABOUT_TO_START, dictMailData, clinicalRotationMemberDetail.OrganizationUserId, tenantId);

                ClinicalRotationManager.UpdateClinicalRotationMenberForNagMail(clinicalRotationMemberDetail.RotationMemberId, CurentLoggedInUserId, tenantId);
            }
        }


        public void SendScheduleRequirementsNotification(Dictionary<String, Object> conversionData)
        {
            String orgUserIds = GetOrganizationUserIDByRotMemberIDs(Convert.ToString(conversionData["orgUserIds"]));
            Int32 tenantId = Convert.ToInt32(conversionData["tenantId"]);
            Int32 CurentLoggedInUserId = Convert.ToInt32(conversionData["CurentLoggedInUserId"]);
            String tenantName = SecurityManager.GetTenant(tenantId).TenantName;
            String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

            List<ClinicalRotationMemberDetail> lstRotationDeatils = ClinicalRotationManager.GetRotationDetailsByOrgUserIds(orgUserIds, tenantId, View.ClinicalRotationID);

            foreach (ClinicalRotationMemberDetail clinicalRotationMemberDetail in lstRotationDeatils)
            {
                //Create Dictionary
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, clinicalRotationMemberDetail.ApplicantName);
                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId.ToString());
                dictMailData.Add(EmailFieldConstants.ROTATION_NAME, clinicalRotationMemberDetail.RotationName);
                dictMailData.Add(EmailFieldConstants.ROTATION_START_DATE, Convert.ToDateTime(clinicalRotationMemberDetail.StartDate).ToString("MM/dd/yyyy"));

                //UAT-2191
                dictMailData.Add(EmailFieldConstants.AGENCY_NAME, clinicalRotationMemberDetail.AgencyName);
                //UAT-2290
                if (!clinicalRotationMemberDetail.DeadlineDate.IsNullOrEmpty())
                {
                    dictMailData.Add(EmailFieldConstants.DEADLINE_DATE, Convert.ToDateTime(clinicalRotationMemberDetail.DeadlineDate).ToString("MM/dd/yyyy"));
                }
                else
                {
                    dictMailData.Add(EmailFieldConstants.DEADLINE_DATE, String.Empty);
                }

                dictMailData.Add(EmailFieldConstants.ROTATION_DETAILS, ClinicalRotationManager.GenerateRotationDetailsHTML(clinicalRotationMemberDetail));

                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                mockData.UserName = clinicalRotationMemberDetail.ApplicantName;
                mockData.EmailID = clinicalRotationMemberDetail.PrimaryEmailaddress;
                mockData.ReceiverOrganizationUserID = clinicalRotationMemberDetail.OrganizationUserId;

                //Send mail
                CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFY_OF_ROTATION_SCHEDULE_AND_REQUIREMENTS, dictMailData, mockData, tenantId, -1, null, null, true, false, null, clinicalRotationMemberDetail.RotationHirarchyIds, View.ClinicalRotationID);

                //Send Message
                CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFY_OF_ROTATION_SCHEDULE_AND_REQUIREMENTS, dictMailData, clinicalRotationMemberDetail.OrganizationUserId, tenantId);
            }
        }

        private String GetOrganizationUserIDByRotMemberIDs(String lstRotMemberIDs)
        {
            List<Int32> tmpRotationIDs = lstRotMemberIDs.Split(',').Select(x => Convert.ToInt32(x)).ToList();

            Dictionary<Int32, String> tmpUserInfo = GetOrgUserIDsListByRotationMemberIDs(tmpRotationIDs);

            // Dictionary<Int32, String> tmpUserInfo = GetOrganizationUserDetailsByOrgUserIDs(tmpRotationIDs);
            if (!tmpUserInfo.IsNullOrEmpty())
            {
                List<Int32> lstOrganizationUserID = tmpUserInfo.Keys.ToList();
                return String.Join(",", lstOrganizationUserID);
            }
            return String.Empty;
        }

        private Dictionary<Int32, String> GetOrgUserIDsListByRotationMemberIDs(List<Int32> tmpRotationIDs)
        {
            return ProfileSharingManager.GetOrganizationUserIDByRotMemberIDs(View.SelectedTenantId, tmpRotationIDs);
        }

        #region UAT-3463
        //GetOrganizationUserDetailsByOrgUserIDs
        private Dictionary<Int32, String> GetOrganizationUserDetailsByOrgUserIDs(List<Int32> lstOrgUserIds)
        {
            return ProfileSharingManager.GetOrganizationUserDetailsByOrgUserIDs(View.SelectedTenantId, lstOrgUserIds);
        }
        #endregion
        #endregion


        #region UAT:2477

        public List<ClinicalRotationDetailContract> GetRotationPackageAndAgencyData(int rotationID, int tenantID)
        {
            //List<ClinicalRotationDetailContract> objClinicalRotationDetailContract = new List<ClinicalRotationDetailContract>();

            //objClinicalRotationDetailContract =
            return ClinicalRotationManager.GetRotationPackageAndAgencyData(rotationID, tenantID);
            // View.ClinicalRotationDetails = objClinicalRotationDetailContract;
        }
        #endregion

        #region UAT-2544:
        private List<Int32> FilterApprovedMembersFromRemoveList()
        {
            //    if (View.IsRotationStart) //UAT-4460
            //    {
            List<Int32> clinicalRotationMembersList = new List<Int32>();
            clinicalRotationMembersList = View.ClinicalRotationMemberIdsToDrop.Where(x => x.Value == true).Select(slct => slct.Key).ToList();
            clinicalRotationMembersList.ForEach(rem =>
                {
                    View.RemovedClinicalRotationMemberIds.Remove(rem);
                });

            return clinicalRotationMembersList;
            //
            //View.RemovedClinicalRotationMemberIds = View.RemovedClinicalRotationMemberIds.Where(y => approvedMembersList.Contains(y.Key)).Select(slct => );
            //}
            //return new List<Int32>();
        }

        public Boolean IsApprovedStudentProfileSharing()
        {
            if (View.IsRotationStart)
            {
                return View.ApprovedClinicalRotationMemberIdsToRemove.Where(cond => cond.Key > AppConsts.NONE).Any(x => x.Value == true);
            }
            return false;
        }
        #endregion


        #region UAT-2763:SSN Granular permission on rotation detail screen
        public void GetGranularPermissionForClientAdmins()
        {
            View.SSNPermissionCode = EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue();
            if (!View.dicGranularPermissions.IsNullOrEmpty())
            {
                if (View.dicGranularPermissions.ContainsKey(EnumSystemEntity.SSN.GetStringValue()))
                {
                    View.SSNPermissionCode = View.dicGranularPermissions[EnumSystemEntity.SSN.GetStringValue()];
                }
            }
        }
        #endregion

        #region [UAT-2735]

        public void FilterApplicantHavingOnlyNonActiveOrExpireOrders(String selectedApplicants)
        {
            ServiceRequest<Int32, String> serviceRequest = new ServiceRequest<Int32, String>();
            serviceRequest.Parameter1 = View.SelectedTenantId;
            serviceRequest.Parameter2 = selectedApplicants;
            var _serviceResponse = _clientRotationProxy.FilterApplicantHavingOnlyNonActiveOrExpireOrders(serviceRequest);

            if (_serviceResponse.Result.Count > 0)
            {
                StringBuilder sbApplicantName = new StringBuilder("The following students do not have any requirements to share: <br/><br/>");

                foreach (string applicantName in _serviceResponse.Result)
                {
                    sbApplicantName.Append(string.Concat(applicantName, ", "));
                }

                sbApplicantName.Remove(sbApplicantName.Length - 2, 2);

                sbApplicantName.Append("<br/><br/>Please instruct the students to place an order within the Complio system or assign a rotation package to this rotation.");

                RotationAndTrackingPkgStatusContract obj = new RotationAndTrackingPkgStatusContract();
                obj.ErrorMessage = sbApplicantName.ToString();

                View.LstStatusMessages = new List<RotationAndTrackingPkgStatusContract>();
                View.LstStatusMessages.Add(obj);
            }
        }
        #endregion


        #region UAT-3049

        public List<ApplicantDocumentContract> GetRotationMemberDocuments(List<RotationMemberSearchDetailContract> lstApplicantRotationToExport)
        {
            List<ApplicantDocumentContract> lstRotationMemberDocuments = new List<ApplicantDocumentContract>();
            if (lstApplicantRotationToExport.Count > AppConsts.NONE)
            {
                //Direct call manager method without using proxy service, because getting error [entity too large].
                lstRotationMemberDocuments = ClinicalRotationManager.GetApplicantDocumentToExport(lstApplicantRotationToExport, View.SelectedTenantId);
            }
            else
            {
                lstRotationMemberDocuments = new List<ApplicantDocumentContract>();
            }
            return lstRotationMemberDocuments;
        }
        #endregion


        #region UAT-3977
        public Dictionary<Int32, String> InstructorPreceptorRequiredPkgCompliantReqd(String agencyIDs)
        {
            ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
            serviceRequest.Parameter = agencyIDs;

            var _serviceResponse = _clientRotationProxy.InstructorPreceptorRequiredPkgCompliantReqd(serviceRequest);
            return _serviceResponse.Result;
        }

        public void GetComplianceStatusOfInstructorRotationPackages(String selectedApplicants)
        {
            Dictionary<String, String> _applicantData = new Dictionary<String, String>();
            _applicantData.Add("DelimittedOrgUserIDs", selectedApplicants);
            _applicantData.Add("DelimittedTrackingPkgIDs", String.Empty);
            _applicantData.Add("SearchType", "INSTR");


            ServiceRequest<Int32, Dictionary<String, String>, Int32> serviceRequest = new ServiceRequest<Int32, Dictionary<String, String>, Int32>();
            serviceRequest.Parameter1 = View.SelectedTenantId;
            serviceRequest.Parameter2 = _applicantData;
            serviceRequest.Parameter3 = View.ClinicalRotationID;
            var _serviceResponse = _clientRotationProxy.GetComplianceStatusOfImmunizationAndRotationPackages(serviceRequest);

            View.LstStatusMessages = _serviceResponse.Result;
            //ProfileSharingManager.GetComplianceStatusOfImmunizationAndRotationPackages(View.TenantID, View.DelimittedOrgUserIDs, View.DelimittedTrackingPkgIDs, View.RotationID, "IMNZ");
        }

        #endregion

        #region UAT-4147

        /// <summary>
        /// To check is applicant(s) already exists as Instructor(s) in Clinical Rotation
        /// </summary>
        public List<ClinicalRotationMembersContract> IsOrgUserAlreadyExistsAsInstructorOrApplicantInClinicalRotation(string rotationIDs, int tenantID, string selectedOrgUserIDs, string selectedClientContactIDs)
        {
            return ClinicalRotationManager.IsOrgUserAlreadyExistsAsInstructorOrApplicantInClinicalRotation(rotationIDs, tenantID, selectedOrgUserIDs, selectedClientContactIDs);
        }
        #endregion

        #region UAT 4398
        public void GetAgencyUserListByAgencIds(List<Int32> agencyIDs)
        {
            View.LstAgencyUserByAgency = ProfileSharingManager.GetAgencyUserListByAgencIds(agencyIDs);
        }

        public void SendRotationDetailsNotificationToAgencyUsers(Dictionary<String, Object> conversionData)
        {
            ClinicalRotationDetailContract objClinicalRotationDetailContract = (ClinicalRotationDetailContract)conversionData["rotationDetails"];
            List<RotationMemberDetailContract> lstRotationMembers = new List<RotationMemberDetailContract>();
            lstRotationMembers = View.lstSelectedRotationMembers;
            Int32 tenantId = Convert.ToInt32(conversionData["tenantId"]);
            Int32 CurentLoggedInUserId = Convert.ToInt32(conversionData["CurentLoggedInUserId"]);
            String tenantName = SecurityManager.GetTenant(tenantId).TenantName;
            String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);
            List<Int32> agencyIDs = objClinicalRotationDetailContract.AgencyIDs.Split(',').Select(int.Parse).ToList();
            GetAgencyUserListByAgencIds(agencyIDs);
            List<Entity.SharedDataEntity.AgencyUser> LstAgencyUserByAgency = View.LstAgencyUserByAgency;

            foreach (Entity.SharedDataEntity.AgencyUser objAgencyUser in View.LstAgencyUserByAgency)
            {
                // Create Dictionary
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();

                // Rotation Details
                dictMailData.Add(EmailFieldConstants.ROTATION_DETAILS, ClinicalRotationManager.GenerateHTMLForRotationDetails(objClinicalRotationDetailContract));

                // Rotation members' details
                dictMailData.Add(EmailFieldConstants.ROTATION_MEMBERS_DETAILS, ClinicalRotationManager.GenerateHTMLForRotationMembersDetails(lstRotationMembers));

                if (objAgencyUser.AGU_UserID != Guid.Empty)
                {
                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                    mockData.UserName = objAgencyUser.AGU_Name;
                    mockData.EmailID = objAgencyUser.AGU_Email;
                    mockData.ReceiverOrganizationUserID = ProfileSharingManager.GetAgencyUserOrganizationUserId(objAgencyUser.AGU_UserID);

                    //Send mail
                    CommunicationManager.SendRotationAndMembersDetailsNotificationMail(CommunicationSubEvents.NOTIFICATION_TO_AGENCY_USER_FOR_ROTATIONDETAILS_AND_ROTATIONMEMBERSDETAILS, dictMailData, mockData, tenantId, -1, null, null, true, false, null, null, View.ClinicalRotationID);
                }
            }
        }
        #endregion
    }
}
