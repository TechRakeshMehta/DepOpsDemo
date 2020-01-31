using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using Business.RepoManagers;

namespace CoreWeb.ClinicalRotation.Views
{
    public class ManageRotationAssignmentPopupPresenter : Presenter<IManageRotationAssignmentPopupView>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        private ClientContactProxy _clientContactProxy
        {
            get
            {
                return new ClientContactProxy();
            }
        }

        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }
        public void GetClientContacts()
        {
            if (View.TenantId == 0)
                View.ClientContactList = new List<ClientContactContract>();
            else
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.TenantId;
                var _serviceResponse = _clientContactProxy.GetClientContacts(serviceRequest);
                View.ClientContactList = _serviceResponse.Result;
            }
        }
        public void GetRequirementPackages()
        {
            if (View.TenantId == 0)
            {
                View.lstTenantRequirementPackage = new List<RequirementPackageContract>();
                View.lstSharedRequirementPackage = new List<RequirementPackageContract>();
                View.lstCombinedRequirementPackage = new List<RequirementPackageContract>();
            }
            else
            {
                ServiceRequest<String, Boolean> serviceRequestForTenantPackages = new ServiceRequest<String, Boolean>();
                serviceRequestForTenantPackages.SelectedTenantId = View.TenantId;
                serviceRequestForTenantPackages.Parameter1 = View.RotationAgencyIDs.ToString();
                serviceRequestForTenantPackages.Parameter2 = false;
                ServiceResponse<List<RequirementPackageContract>> _serviceResponseForTenantPackages = _requirementPackageProxy
                                                                                                            .GetRequirementPackages(serviceRequestForTenantPackages);
                List<RequirementPackageContract> lstTenantRequirementPackage = _serviceResponseForTenantPackages.Result;

                if (!lstTenantRequirementPackage.IsNullOrEmpty())
                {
                    lstTenantRequirementPackage = lstTenantRequirementPackage.Where(cond => !cond.IsCopied).ToList();
                }
                View.lstTenantRequirementPackage = lstTenantRequirementPackage;

                ServiceRequest<String, Boolean> serviceRequestForSharedPackages = new ServiceRequest<String, Boolean>();
                serviceRequestForSharedPackages.SelectedTenantId = View.TenantId;
                serviceRequestForSharedPackages.Parameter1 = View.RotationAgencyIDs.ToString();
                serviceRequestForSharedPackages.Parameter2 = true;
                ServiceResponse<List<RequirementPackageContract>> _serviceResponseForSharedPackages = _requirementPackageProxy
                                                                                                            .GetRequirementPackages(serviceRequestForSharedPackages);
                List<RequirementPackageContract> lstSharedRequirementPackage = _serviceResponseForSharedPackages.Result;

                View.lstSharedRequirementPackage = lstSharedRequirementPackage;

               

                var CombinedPackageList = lstTenantRequirementPackage.Concat(lstSharedRequirementPackage).ToList();

                #region UAT-2514 - Filter Packages if onny 1 rotation id is selected in the grid
                if (!View.RotationIDs.IsNullOrEmpty())
                {
                    List<Int32> rotationIDList = new List<Int32>();
                    rotationIDList = View.RotationIDs.Split(',').Select(int.Parse).ToList();
                    if (rotationIDList.Count == AppConsts.ONE)
                    {
                        Int32 rotationID = rotationIDList.FirstOrDefault();
                        //Getting Rotation Details
                        ServiceRequest<Int32, Int32> serviceRequestRotationDetail = new ServiceRequest<Int32, Int32>();
                        serviceRequestRotationDetail.Parameter1 = View.TenantId;
                        serviceRequestRotationDetail.Parameter2 = rotationID;
                        var rotationDetail = _requirementPackageProxy.GetRotationDetail(serviceRequestRotationDetail);
                        //View.lstCombinedRequirementPackage = CombinedPackageList.Where(cond => (cond.EffectiveEndDate > rotationDetail.Result.StartDate || cond.EffectiveEndDate.IsNull())
                        //                                       && (cond.EffectiveStartDate.IsNull() || cond.EffectiveStartDate < rotationDetail.Result.EndDate)).OrderBy(ord => ord.RequirementPackageName).ToList();

                        #region UAT-4657

                        CombinedPackageList = CombinedPackageList.Where(cond => (cond.EffectiveEndDate > rotationDetail.Result.StartDate || cond.EffectiveEndDate.IsNull())
                                                               && (cond.EffectiveStartDate.IsNull() || cond.EffectiveStartDate < rotationDetail.Result.EndDate)).OrderBy(ord => ord.RequirementPackageName).ToList();


                       
                        //CombinedPackageList = CombinedPackageList.Where(cond => (cond.EffectiveEndDate > rotationDetail.Result.StartDate || cond.EffectiveEndDate.IsNull())
                        //                                                        && (cond.EffectiveStartDate.IsNull() || cond.EffectiveStartDate < rotationDetail.Result.EndDate))
                        //                                                        .OrderBy(ord => ord.RequirementPackageName).ToList();

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
                                    DateTime? pkgHighestEffDate = lstPkgListForGroup.Where(con => con.EffectiveStartDate < rotationDetail.Result.StartDate).Any() ?
                                                         lstPkgListForGroup.Where(con => con.EffectiveStartDate < rotationDetail.Result.StartDate).Max(x => x.EffectiveStartDate).Value : (DateTime?)null;
                                    //DateTime pkgHighestEffDate = lstPkgListForGroup.OrderByDescending(ord => ord.EffectiveStartDate).FirstOrDefault().EffectiveStartDate.Value;
                                    finalPkgList.AddRange(lstPkgListForGroup.Where(con => con.EffectiveStartDate.Value == pkgHighestEffDate).ToList());
                                }
                            }
                        });

                        View.lstCombinedRequirementPackage = finalPkgList;

                        #endregion

                    }
                    else
                    {
                        View.lstCombinedRequirementPackage = CombinedPackageList.OrderBy(ord => ord.RequirementPackageName).ToList();
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// To get Requirement Packages
        /// </summary>
        public void GetInstructorRequirementPackages()
        {
            if (View.TenantId == 0)
            {
                View.lstInstructorRequirementPackage = new List<RequirementPackageContract>();
                View.lstSharedInstructorRequirementPackages = new List<RequirementPackageContract>();
                View.lstCombinedInstructorRequirementPackages = new List<RequirementPackageContract>();
            }
            else
            {
                ServiceRequest<String, Boolean> serviceRequestForTenantPackages = new ServiceRequest<String, Boolean>();
                serviceRequestForTenantPackages.SelectedTenantId = View.TenantId;
                serviceRequestForTenantPackages.Parameter1 = View.RotationAgencyIDs.ToString();
                serviceRequestForTenantPackages.Parameter2 = false;
                ServiceResponse<List<RequirementPackageContract>> _serviceResponseForTenantPackages = _requirementPackageProxy
                                                                                                   .GetInstructorRequirementPackages(serviceRequestForTenantPackages);
                List<RequirementPackageContract> lstTenantInstructorRequirementPackage = _serviceResponseForTenantPackages.Result;

                if (!lstTenantInstructorRequirementPackage.IsNullOrEmpty())
                {
                    lstTenantInstructorRequirementPackage = lstTenantInstructorRequirementPackage.Where(cond => !cond.IsCopied).ToList();
                }
                View.lstInstructorRequirementPackage = lstTenantInstructorRequirementPackage;

                ServiceRequest<String, Boolean> serviceRequestForSharedPackages = new ServiceRequest<String, Boolean>();
                serviceRequestForSharedPackages.SelectedTenantId = View.TenantId;
                serviceRequestForSharedPackages.Parameter1 = View.RotationAgencyIDs.ToString();
                serviceRequestForSharedPackages.Parameter2 = true;
                ServiceResponse<List<RequirementPackageContract>> _serviceResponseForSharedPackages = _requirementPackageProxy
                                                                                                            .GetInstructorRequirementPackages(serviceRequestForSharedPackages);
                List<RequirementPackageContract> lstSharedInstructorRequirementPackage = _serviceResponseForSharedPackages.Result;

                //if (!lstSharedInstructorRequirementPackage.IsNullOrEmpty())
                //{
                //    lstSharedInstructorRequirementPackage = lstSharedInstructorRequirementPackage.Where(cond => View.MappedInstructorRequirementPackage != null
                //                                        && cond.RequirementPackageCode.ToString().ToLower() != View.MappedInstructorRequirementPackage.RequirementPackageCode.ToString().ToLower()).ToList();
                //}

                View.lstSharedRequirementPackage = lstSharedInstructorRequirementPackage;

                //View.lstCombinedInstructorRequirementPackages = lstTenantInstructorRequirementPackage.Concat(lstSharedInstructorRequirementPackage)
                //                                                .OrderBy(ord => ord.RequirementPackageName).ToList();
                var CombinedPackageList = lstTenantInstructorRequirementPackage.Concat(lstSharedInstructorRequirementPackage).ToList();

                #region UAT-2514 - Filter Packages if onny 1 rotation id is selected in the grid
                if (!View.RotationIDs.IsNullOrEmpty())
                {
                    List<Int32> rotationIDList = new List<Int32>();
                    rotationIDList = View.RotationIDs.Split(',').Select(int.Parse).ToList();
                    if (rotationIDList.Count == AppConsts.ONE)
                    {
                        Int32 rotationID = rotationIDList.FirstOrDefault();
                        //Getting Rotation Details
                        ServiceRequest<Int32, Int32> serviceRequestRotationDetail = new ServiceRequest<Int32, Int32>();
                        serviceRequestRotationDetail.Parameter1 = View.TenantId;
                        serviceRequestRotationDetail.Parameter2 = rotationID;
                        var rotationDetail = _requirementPackageProxy.GetRotationDetail(serviceRequestRotationDetail);
                        //View.lstCombinedInstructorRequirementPackages = CombinedPackageList.Where(cond => (cond.EffectiveEndDate > rotationDetail.Result.StartDate || cond.EffectiveEndDate.IsNull())
                        //                                       && (cond.EffectiveStartDate.IsNull() || cond.EffectiveStartDate < rotationDetail.Result.EndDate)).OrderBy(ord => ord.RequirementPackageName).ToList();

                        #region UAT-4657

                        CombinedPackageList = CombinedPackageList.Where(cond => (cond.EffectiveEndDate > rotationDetail.Result.StartDate || cond.EffectiveEndDate.IsNull())
                                                               && (cond.EffectiveStartDate.IsNull() || cond.EffectiveStartDate < rotationDetail.Result.EndDate)).OrderBy(ord => ord.RequirementPackageName).ToList();



                        //CombinedPackageList = CombinedPackageList.Where(cond => (cond.EffectiveEndDate > rotationDetail.Result.StartDate || cond.EffectiveEndDate.IsNull())
                        //                                                        && (cond.EffectiveStartDate.IsNull() || cond.EffectiveStartDate < rotationDetail.Result.EndDate))
                        //                                                        .OrderBy(ord => ord.RequirementPackageName).ToList();

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
                                    DateTime? pkgHighestEffDate = lstPkgListForGroup.Where(con => con.EffectiveStartDate < rotationDetail.Result.StartDate).Any() ?
                                                         lstPkgListForGroup.Where(con => con.EffectiveStartDate < rotationDetail.Result.StartDate).Max(x => x.EffectiveStartDate).Value : (DateTime?)null;
                                    //DateTime pkgHighestEffDate = lstPkgListForGroup.OrderByDescending(ord => ord.EffectiveStartDate).FirstOrDefault().EffectiveStartDate.Value;
                                    finalPkgList.AddRange(lstPkgListForGroup.Where(con => con.EffectiveStartDate.Value == pkgHighestEffDate).ToList());
                                }
                            }
                        });

                        View.lstCombinedInstructorRequirementPackages = finalPkgList;

                        #endregion

                    }
                    else
                    {
                        View.lstCombinedInstructorRequirementPackages = CombinedPackageList.OrderBy(ord => ord.RequirementPackageName).ToList();
                    }
                }
                #endregion

            }
        }


        public Boolean SaveUpdateClinicalRotationAssignments(Boolean isSharedPackage, Boolean isNewPackage)
        {
            Boolean isDataSaved = false;

            if (!View.RotationIDs.IsNullOrEmpty())
            {
                List<Int32> rotationIDList = new List<Int32>();
                rotationIDList = View.RotationIDs.Split(',').Select(int.Parse).ToList();

                #region Check Rot. Eff. Start Date & EndDate               
                UpdateRotationIdsByRequirementPkgEffectiveDateFilter(isNewPackage);
                if (!View.UnMappedRotationIDList.IsNullOrEmpty())
                {
                    foreach (var item in View.UnMappedRotationIDList)
                    {
                        rotationIDList.Remove(item);
                    }
                }
                #endregion
                var lstInstNotAvailbilityRotationIds = View.lstInstructorAvailabilityContracts.Where(cond => cond.InsAvailibility && !cond.IsSchoolSendingInstructor).ToList();
                foreach (var item in lstInstNotAvailbilityRotationIds)
                {
                    rotationIDList.Remove(item.RotationID);
                }
                if (rotationIDList.IsNullOrEmpty())
                    return false;
                Int32 packageIdToBeAssigned = AppConsts.NONE;
                Dictionary<String, String> actionTypeDic = new Dictionary<String, String>();
                if (String.Compare(View.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_PRECEPTOR_PACKAGES.GetStringValue(), true) == 0)
                {
                    packageIdToBeAssigned = View.SelectedInstructionPackageID;
                    if (isSharedPackage)
                    {
                        //Copy package data into client database
                        ServiceRequest<Int32, Int32> serviceRequestPkg = new ServiceRequest<Int32, Int32>();
                        serviceRequestPkg.SelectedTenantId = View.TenantId;
                        serviceRequestPkg.Parameter1 = View.SelectedInstructionPackageID;
                        serviceRequestPkg.Parameter2 = View.CurrentLoggedInUserID;

                        //UAT-2213 Copy Package
                        if (isNewPackage)
                        {
                            packageIdToBeAssigned = _requirementPackageProxy.CopySharedRequirementPackageToClientNew(serviceRequestPkg).Result;
                        }
                        else
                        {
                            packageIdToBeAssigned = _requirementPackageProxy.CopySharedRqrmntPkgToClient(serviceRequestPkg).Result;
                        }

                    }
                    actionTypeDic.Add(View.RotationAssignmentTypeCode, Convert.ToString(packageIdToBeAssigned));
                }
                else if (String.Compare(View.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_STUDENT_PACKAGES.GetStringValue(), true) == 0)
                {
                    packageIdToBeAssigned = View.SelectedRequirementPackageID;
                    if (isSharedPackage)
                    {
                        //Copy package data into client database
                        ServiceRequest<Int32, Int32> serviceRequestPkg = new ServiceRequest<Int32, Int32>();
                        serviceRequestPkg.SelectedTenantId = View.TenantId;
                        serviceRequestPkg.Parameter1 = View.SelectedRequirementPackageID;
                        serviceRequestPkg.Parameter2 = View.CurrentLoggedInUserID;

                        //UAT-2213 Copy Package
                        if (isNewPackage)
                        {
                            packageIdToBeAssigned = _requirementPackageProxy.CopySharedRequirementPackageToClientNew(serviceRequestPkg).Result;
                        }
                        else
                        {
                            packageIdToBeAssigned = _requirementPackageProxy.CopySharedRqrmntPkgToClient(serviceRequestPkg).Result;
                        }
                    }
                    actionTypeDic.Add(View.RotationAssignmentTypeCode, Convert.ToString(packageIdToBeAssigned));
                }
                else if (String.Compare(View.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_PRECEPTOR.GetStringValue(), true) == 0)
                {
                    String senderEmailID = System.Configuration.ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID];
                    actionTypeDic.Add(View.RotationAssignmentTypeCode, senderEmailID);
                }
                else
                {
                    actionTypeDic.Add(View.RotationAssignmentTypeCode, AppConsts.ZERO);
                }
                ServiceRequest<List<Int32>, ClinicalRotationDetailContract, Dictionary<String, String>> serviceRequestForRotAssignment = new ServiceRequest<List<Int32>, ClinicalRotationDetailContract, Dictionary<String, String>>();

                serviceRequestForRotAssignment.SelectedTenantId = View.TenantId;
                serviceRequestForRotAssignment.Parameter1 = rotationIDList;
                serviceRequestForRotAssignment.Parameter2 = View.RotationDataContarct;
                serviceRequestForRotAssignment.Parameter3 = actionTypeDic;
                ServiceResponse<Boolean> serviceResponse = _clinicalRotationProxy.SaveUpdateClinicalRotationAssignments(serviceRequestForRotAssignment);
                if (!serviceResponse.IsNullOrEmpty())
                {
                    isDataSaved = serviceResponse.Result;
                }
            }
            return isDataSaved;
        }


        #region Check Effective Start Date and End Date For New Requirement Packages
        void UpdateRotationIdsByRequirementPkgEffectiveDateFilter(Boolean isNewPackage)
        {
            //Check Package Type
            Int32 packageIdToBeAssigned = AppConsts.NONE;
            if (((String.Compare(View.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_PRECEPTOR_PACKAGES.GetStringValue(), true) == 0) ||
                        (String.Compare(View.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_STUDENT_PACKAGES.GetStringValue(), true) == 0)) &&
               isNewPackage)
            {
                if (String.Compare(View.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_STUDENT_PACKAGES.GetStringValue(), true) == 0)
                {
                    packageIdToBeAssigned = View.SelectedRequirementPackageID;
                }
                else if (String.Compare(View.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_PRECEPTOR_PACKAGES.GetStringValue(), true) == 0)
                {
                    packageIdToBeAssigned = View.SelectedInstructionPackageID;
                }

                #region Requirement Pkg eff. Date Check and Update ( View.RotationIDs )

                if (!View.RotationIDs.IsNullOrEmpty())
                {
                    ServiceRequest<Int32, String, Int32> serviceRequestChkRotEffectiveDateRange = new ServiceRequest<Int32, String, Int32>();
                    serviceRequestChkRotEffectiveDateRange.Parameter1 = packageIdToBeAssigned;
                    serviceRequestChkRotEffectiveDateRange.Parameter2 = View.RotationIDs;
                    serviceRequestChkRotEffectiveDateRange.Parameter3 = View.TenantId;
                    var result = _requirementPackageProxy.CheckRotEffectiveDate(serviceRequestChkRotEffectiveDateRange);
                    View.UnMappedRotationIDList = result.Result1;
                    View.UnMappedRotationNameList = result.Result2;
                }

                #endregion

            }
        }
        #endregion

        public Boolean IsDataEnteredForAnyRotation(String packageType)
        {
            ServiceRequest<String, String> serviceRequestToCheckDataEnteredForRot = new ServiceRequest<String, String>();
            serviceRequestToCheckDataEnteredForRot.SelectedTenantId = View.TenantId;
            serviceRequestToCheckDataEnteredForRot.Parameter1 = View.RotationIDs;
            serviceRequestToCheckDataEnteredForRot.Parameter2 = packageType;

            ServiceResponse<Boolean> serviceResponse = _clinicalRotationProxy.IsDataEnteredForAnyRotation(serviceRequestToCheckDataEnteredForRot);
            if (!serviceResponse.IsNullOrEmpty())
            {
                return serviceResponse.Result;
            }
            return false;
        }

        #region UAT-4147

        /// <summary>
        /// To check is applicant(s) already exists as Instructor(s) in Clinical Rotation
        /// </summary>
        public List<ClinicalRotationMembersContract> IsOrgUserAlreadyExistsAsInstructorOrApplicantInClinicalRotation(string rotationIDs, int tenantID, string selectedOrgUserIDs, string selectedClientContactIDs)
        {
            return ClinicalRotationManager.IsOrgUserAlreadyExistsAsInstructorOrApplicantInClinicalRotation(rotationIDs, tenantID, selectedOrgUserIDs, selectedClientContactIDs);
        }
        #endregion

        public void CheckInstAvailabilityByRotationIds(String rotationIds)
        {
            ServiceRequest<String> checkInstAvailabilityByRotationIds = new ServiceRequest<String>();
            checkInstAvailabilityByRotationIds.SelectedTenantId = View.TenantId;
            checkInstAvailabilityByRotationIds.Parameter = View.RotationIDs;
            ServiceResponse<List<InstructorAvailabilityContract>> serviceResponse = _clinicalRotationProxy.CheckInstAvailabilityByRotationIds(checkInstAvailabilityByRotationIds);
            if (!serviceResponse.IsNullOrEmpty())
            {
                View.lstInstructorAvailabilityContracts = serviceResponse.Result;
                View.ComplioIDs = String.Empty;
                var lstInstNotAvailbilityRotationIds = View.lstInstructorAvailabilityContracts.Where(cond => cond.InsAvailibility && !cond.IsSchoolSendingInstructor).ToList();
                foreach (var item in lstInstNotAvailbilityRotationIds)
                {
                    View.ComplioIDs = View.ComplioIDs + ", " + item.ComplioID;
                }
                if (!View.ComplioIDs.IsNullOrEmpty())
                    View.ComplioIDs = View.ComplioIDs.Remove(0, 1);
            }

        }
    }
}
