using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ManageCompliancePriorityObjectMappingPresenter : Presenter<IManageCompliancePriorityObjectMappingView>
    {
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenants = SecurityManager.GetTenants(SortByName, false, clientCode);
        }

        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantId);
        }

        public void GetCompPriorityObject()
        {
            View.lstCompPriorityObjects = SecurityManager.GetCompliancePriorityObjects();

        }

        public void GetSelectedTenantCategory()
        {
            if (!View.selectedTenantID.IsNullOrEmpty() && View.selectedTenantID > AppConsts.NONE)
            {
                View.lstCategoryItems = ComplianceDataManager.GetCategoryItems(View.selectedTenantID);
                View.lstCategory = View.lstCategoryItems.DistinctBy(col => col.CategoryID).ToList();
            }
            else
            {
                View.lstCategoryItems = new List<CompliancePriorityObjectContract>();
            }
        }

        public void GetItems(Boolean isEditMode, Int32 compObjmappingID)
        {
            if (!View.selectedCategoryID.IsNullOrEmpty() && View.selectedCategoryID > AppConsts.NONE)
            {
                if (!View.lstCategoryItems.IsNullOrEmpty())
                {
                    List<Int32?> lstItemIdsAlreadyMapped = new List<Int32?>();
                    View.lstItem = View.lstCategoryItems.Where(cond => cond.CategoryID == View.selectedCategoryID).ToList();

                    if (compObjmappingID == AppConsts.NONE)
                    {
                        lstItemIdsAlreadyMapped = View.lstCompObjMappings.Where(cond => cond.CategoryID == View.selectedCategoryID).Select(sel => sel.ItemID).ToList();
                    }
                    else
                    {
                        var result = View.lstCompObjMappings.Where(cond => cond.CategoryID == View.selectedCategoryID && cond.CCIPOM_ID == compObjmappingID).FirstOrDefault();
                        if (result.ItemID.IsNullOrEmpty())
                        {
                            lstItemIdsAlreadyMapped = View.lstCompObjMappings.Where(cond => cond.CategoryID == View.selectedCategoryID).Select(sel => sel.ItemID).ToList();
                        }
                        else
                        {
                            lstItemIdsAlreadyMapped = View.lstCompObjMappings.Where(cond => cond.CategoryID == View.selectedCategoryID).Select(sel => sel.ItemID).ToList();
                            if (lstItemIdsAlreadyMapped.Contains(result.ItemID))
                                lstItemIdsAlreadyMapped.Remove(result.ItemID);
                        }

                    }
                    View.lstItem.RemoveAll(x => lstItemIdsAlreadyMapped.Contains(x.ItemID));

                    //if (!isEditMode)
                    //{
                    //    List<Int32?> lstItemIdsAlreadyMapped = View.lstCompObjMappings.Where(cond => cond.CategoryID == View.selectedCategoryID).Select(sel => sel.ItemID).ToList();
                    //    View.lstItem.RemoveAll(x => lstItemIdsAlreadyMapped.Contains(x.ItemID));
                    //}
                }
            }
            else
            {
                View.lstItem = new List<CompliancePriorityObjectContract>();
            }
        }

        public void GetCompObjMappings()
        {
            if (!View.selectedTenantID.IsNullOrEmpty() && View.selectedTenantID > AppConsts.NONE)
            {
                View.lstCompObjMappings = ComplianceDataManager.GetCompObjMappings(View.selectedTenantID);
            }
            else
                View.lstCompObjMappings = new List<CompliancePriorityObjectContract>();
        }

        public Boolean SaveCompObjMapping(CompliancePriorityObjectContract compObjMapping)
        {
            return ComplianceDataManager.SaveCompObjMapping(View.selectedTenantID, compObjMapping, View.CurrentLoggedInUserID);
        }

        public Boolean DeleteCompObjMapping(Int32 compObjMappingID)
        {
            return ComplianceDataManager.DeleteCompObjMapping(View.selectedTenantID, View.CurrentLoggedInUserID, compObjMappingID);
        }
    }
}
