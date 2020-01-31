using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.Mobility;
using INTSOF.Utils.Consts;
using System.Xml.Linq;


namespace CoreWeb.Mobility.Views
{
    public class ComplianceItemMappingQueuePresenter : Presenter<IComplianceItemMappingQueueView>
    {


        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }
        //to get the Tenant List             
        public override void OnViewInitialized()
        {

        }

        /// <summary>
        /// to get the Tenant List
        /// </summary>
        public void GetTenantList()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            List<Entity.Tenant> lstTemp = SecurityManager.GetTenants(SortByName, false, clientCode);
            //Done changes for UAT_2258
            //lstTemp.Insert(0, new Entity.Tenant { TenantName = "--SELECT--", TenantID = 0 });
            View.lstTenant = lstTemp;
        }
        //get the Status from package mapping master
        public void GetApplicantMappingStatus()
        {
            //List<Entity.lkpPkgMappingStatu> mappingStatusLst = MobilityManager.GetMappingStatus();
           // String reviewedInstance = PkgMappingStatus.Reviewed_instance.GetStringValue();
            List<Entity.lkpPkgMappingStatu> mappingStatusLst = LookupManager.GetLookUpData<Entity.lkpPkgMappingStatu>().Where(cond=>cond.PMS_IsDeleted==false).ToList();
            View.lstMappingStatus = mappingStatusLst;
               // .Where(cond => !cond.PMS_Code.Equals(reviewedInstance)).ToList();


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
        /// to get tenant id
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
                    return View.SelectedSourceTenantIds.IsNullOrEmpty() ? 0 : 1;
                return View.TenantId;
            }
        }
        /// <summary>
        /// To Perform Search 
        /// </summary>
        public void PerformSearch(Boolean isInstituteChange)
        {
            try
            {
                if (ClientId != 0 && ClientId.IsNotNull() && isInstituteChange==false)
                {
                    MobilitySearchDataContract mobilityItemDataContract = new INTSOF.UI.Contract.Mobility.MobilitySearchDataContract();
                    mobilityItemDataContract.SourcePackage = String.IsNullOrEmpty(View.SourcePackage) ? null : View.SourcePackage;
                    mobilityItemDataContract.TargetPackage = String.IsNullOrEmpty(View.TargetPackage) ? null : View.TargetPackage;
                    //if (View.SelectedSourceTenantId > SysXDBConsts.NONE)
                    //{
                    //    mobilityItemDataContract.SourceTenantId = View.SelectedSourceTenantId;
                    //}
                    //if (View.SelectedTargetTenantId > SysXDBConsts.NONE)
                    //{
                    //    mobilityItemDataContract.TargetTenantId = View.SelectedTargetTenantId;
                    //}
                    if (View.SelectedMappingStatusID > SysXDBConsts.NONE)
                    {
                        mobilityItemDataContract.StatusID = View.SelectedMappingStatusID;
                    }
                    if (!View.SelectedSourceTenantIds.IsNullOrEmpty())
                    {
                        mobilityItemDataContract.SourceTenantIds = View.SelectedSourceTenantIds;
                    }


                    String searchParameter = BuildSearchParameterXML(mobilityItemDataContract);
                    //Search
                    View.ApplicantSearchData = MobilityManager.GetApplicantMappingList(View.TenantId, View.GridCustomPaging, searchParameter);

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

                }
                else
                {
                    View.ApplicantSearchData = new List<Entity.ApplicantTransitionMappingList>();
                }
               
            }
            catch (Exception e)
            {
                View.ApplicantSearchData = null;
                throw e;
            }

        }

        
        /// <summary>
        /// to check mapping master id exist corresponding to the pkgMappingMasterId in Package Subscription table
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="pkgMappingMasterId">pkgMappingMasterId</param>
        /// <returns>true/false</returns>
        public Boolean IsSubscriptionExist(Int32 tenantId, Int32 pkgMappingMasterId)
        {
            return MobilityManager.IsSubscriptionExist(tenantId, pkgMappingMasterId);
        }

        /// <summary>
        /// to check corresponding to pkgMappingMasterId status is reviewedinstance
        /// </summary>
        /// <param name="pkgMappingMasterId">pkgMappingMasterId</param>
        /// <returns>True/False</returns>
        public Boolean HasNoReviewedInstance(Int32 pkgMappingMasterId)
        {
            return MobilityManager.HasNoReviewedInstance(pkgMappingMasterId);
        }
        /// <summary>
        /// to delete the record corresponding to pkgMappingMasterId(Set Isdeleted=true )
        /// </summary>
        /// <param name="pkgMappingMasterId"></param>
        /// <returns></returns>
        public Boolean DeleteMapping(Int32 pkgMappingMasterId)
        {

            if (MobilityManager.UpdateChanges(pkgMappingMasterId, View.CurrentLoggedInUserId))
            {
                View.SuccessMessage = "Mapping deleted successfully.";
                return true;
            }
            else
            {
                View.ErrorMessage = "Some error occurred.Please try again.";
                return false;
            }
        }

        /// <summary>
        /// to generate XML file of search Parameter.
        /// </summary>
        /// <param name="mobilityItemDataContract"></param>
        /// <returns></returns>
        private String BuildSearchParameterXML(MobilitySearchDataContract mobilityItemDataContract)
        {
            XElement root = new XElement("root");
            XElement row = null;

            if (mobilityItemDataContract.SourcePackage.IsNotNull())
            {
                row = new XElement("SourcePackage");
                row.Value = mobilityItemDataContract.SourcePackage;
                root.Add(row);
            }
            if (mobilityItemDataContract.TargetPackage.IsNotNull())
            {
                row = new XElement("TargetPackage");
                row.Value = mobilityItemDataContract.TargetPackage;
                root.Add(row);
            }
            if (mobilityItemDataContract.SourceTenantIds.IsNotNull())
            {
                row = new XElement("SourceTenantID");
                //row.Value = mobilityItemDataContract.SourceTenantId.Value.ToString();
                row.Value = mobilityItemDataContract.SourceTenantIds.ToString();
                root.Add(row);

            }

            if (mobilityItemDataContract.TargetTenantId.IsNotNull())
            {
                row = new XElement("TargetTenantID");
                row.Value = mobilityItemDataContract.TargetTenantId.Value.ToString();
                root.Add(row);
            }
            if (mobilityItemDataContract.StatusID.IsNotNull())
            {
                row = new XElement("ApplicantMappingStatusID");
                row.Value = mobilityItemDataContract.StatusID.ToString();
                root.Add(row);

            }


            return root.ToString();
        }

        public Int16? GetPkgMappingStatusIDByCode(String code)
        {
            var lkpPakgMappingStatu = LookupManager.GetLookUpData<Entity.lkpPkgMappingStatu>().Where(cond => cond.PMS_IsDeleted == false && cond.PMS_Code == code).FirstOrDefault();
            if (lkpPakgMappingStatu.IsNotNull())
            {
                return lkpPakgMappingStatu.PMS_ID;
            }
            return null;
            //return MobilityManager.GetPkgMappingStatusIDByCode(code);
        }
    }
}




