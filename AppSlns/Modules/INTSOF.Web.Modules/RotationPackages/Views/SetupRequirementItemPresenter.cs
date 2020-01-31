#region NameSpace
#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion
#region Project Specific
using INTSOF.SharedObjects;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.UI.Contract.ComplianceManagement;
#endregion
#endregion

namespace CoreWeb.RotationPackages.Views
{
    public class SetupRequirementItemPresenter : Presenter<ISetupRequirementItemView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            GetRequirementItemDetail();
            //GetUniversalItemsByUniReqCatID();
            //if (View.RequirementItemID > AppConsts.NONE)
            //{
            //    GetUniversalItemByReqCatItmID(View.RequirementItemID);
            //}
        }

        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }

        public void GetRequirementItemDetail()
        {
            if (View.RequirementItemID > AppConsts.NONE)
            {
                ServiceRequest<Int32, Int32,Int32?> serviceRequest = new ServiceRequest<Int32, Int32,Int32?>();
                serviceRequest.Parameter1 = View.RequirementCategoryID;
                serviceRequest.Parameter2 = View.RequirementItemID;
                serviceRequest.Parameter3 = View.RequirementCategoryID;
                ServiceResponse<RequirementItemContract> serviceResponse = _requirementPackageProxy.GetRequirementItemDetail(serviceRequest);
                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    View.ItemName = serviceResponse.Result.RequirementItemName;
                    View.ItemLabel = serviceResponse.Result.RequirementItemLabel;
                    View.IsAllowDataMovement = serviceResponse.Result.AllowDataMovement;
                    View.ExplanatoryNotes = serviceResponse.Result.RequirementItemNotes;
                    View.RequirementItemDisplayOrder = serviceResponse.Result.RequirementItemDisplayOrder; //UAT-3078
                    View.ReqItemSampleDocumentFormURL = serviceResponse.Result.RequirementItemSampleDocumentFormURL; //UAT-3309
                    //UAT-3077
                    View.IsPaymentType = serviceResponse.Result.IsPaymentType;
                    View.Amount = serviceResponse.Result.Amount;
                    //UAT 3792
                    View.IsAllowItemDataEntry = serviceResponse.Result.AllowItemDataEntry;
                    View.DocumentUrlTempList = new List<DocumentUrlContract>();
                    if (serviceResponse.Result.listRequirementItemURLContract != null)
                    {
                        foreach (var item in serviceResponse.Result.listRequirementItemURLContract)
                        {
                            View.DocumentUrlTempList.Add(new DocumentUrlContract
                            {
                                ID = item.RItemURLID,
                                SampleDocFormURL = item.RItemURLSampleDocURL,
                                SampleDocFormUrlDisplayLabel = item.RItemURLLabel
                            });

                        }
                    }
                    View.IsCustomSettings = serviceResponse.Result.IsCustomSetting;
                    View.lstEditableBy = serviceResponse.Result.SelectedEditableBy;
                }
            }
        }

        public void SaveUpdateRequirementItemDetail()
        {
            if (View.SelectedExistigItemID > 0)
            {
                ServiceRequest<Int32, Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32, Int32>();
                serviceRequest.Parameter1 = View.SelectedExistigItemID;
                serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
                serviceRequest.Parameter3 = View.RequirementCategoryID;
                ServiceResponse<Boolean> serviceResponse = _requirementPackageProxy.CloneRequirementItem(serviceRequest);
                if (!serviceResponse.Result)
                {
                    View.ErrorMessage = "Some error has occurred.Please try again.";
                }
                View.SelectedExistigItemID = 0;
            }
            else
            {
                RequirementItemContract reqItemContarct = new RequirementItemContract();
                reqItemContarct.RequirementItemID = View.RequirementItemID;
                reqItemContarct.RequirementCategoryID = View.RequirementCategoryID;
                reqItemContarct.RequirementItemName = View.ItemName;
                reqItemContarct.RequirementItemNotes = View.ExplanatoryNotes; //UAT-2676
                #region UAT-2213
                //UAT-2213:New Rotation Package Process: Master Setup
                if (!View.IsNewPackage)
                {
                    reqItemContarct.RequirementPackageID = View.SelectedPackageID;
                }
                reqItemContarct.IsNewPackage = View.IsNewPackage;
                #endregion
                reqItemContarct.RequirementItemLabel = View.ItemLabel;
                //UAT-3077
                reqItemContarct.IsPaymentType = View.IsPaymentType;
                reqItemContarct.Amount = View.Amount;

                reqItemContarct.RequirementItemDisplayOrder = View.RequirementItemDisplayOrder; //UAT-3078
                if (  View.DocumentUrlTempList!=null &&  View.DocumentUrlTempList.Count > AppConsts.NONE)
                {
                    reqItemContarct.listRequirementItemURLContract = new List<RequirementItemURLContract>();

                    View.DocumentUrlTempList.ForEach(item =>
                    {
                        reqItemContarct.listRequirementItemURLContract.Add(new RequirementItemURLContract
                        {
                            RItemURLID = item.ID,
                            RItemURLLabel = item.SampleDocFormUrlDisplayLabel,
                            RItemURLSampleDocURL = item.SampleDocFormURL

                        });
                    });
                }

                // reqItemContarct.RequirementItemSampleDocumentFormURL = View.ReqItemSampleDocumentFormURL; //UAT-3309

                //if ((!View.UniversalItemData.IsNullOrEmpty() && View.UniCatItmMappingID != View.UniversalItemData.UniversalItemID) ||
                //    (View.UniCatItmMappingID > AppConsts.NONE || (!View.UniversalItemData.IsNullOrEmpty() && View.UniversalItemData.UniReqItmMappingID > AppConsts.NONE)))
                //{
                //    reqItemContarct.UniversalItemContract = new UniversalItemContract();
                //    reqItemContarct.UniversalItemContract.UniCatItmMappingID = View.UniCatItmMappingID;
                //    reqItemContarct.UniversalItemContract.UniReqCatMappingID = View.lstUniversalItems.Where(cond => cond.UniCatItmMappingID == View.UniCatItmMappingID).IsNullOrEmpty() ?
                //        0 : View.lstUniversalItems.Where(cond => cond.UniCatItmMappingID == View.UniCatItmMappingID).FirstOrDefault().UniReqCatMappingID;
                //    if (!View.UniversalItemData.IsNullOrEmpty())
                //    {
                //        reqItemContarct.UniversalItemContract.UniReqItmMappingID = View.UniversalItemData.UniReqItmMappingID;
                //        reqItemContarct.UniversalItemContract.ReqCatIteMappingmID = View.UniversalItemData.ReqCatIteMappingmID;
                //    }
                //}
                //UAT-2603
                reqItemContarct.AllowDataMovement = View.IsAllowDataMovement;
                //UAT 3792
                reqItemContarct.AllowItemDataEntry = View.IsAllowItemDataEntry;
                //UAT-4165
                reqItemContarct.SelectedEditableBy = View.lstEditableBy;
                reqItemContarct.IsCustomSetting = View.IsCustomSettings;
                ServiceRequest<RequirementItemContract> serviceRequest = new ServiceRequest<RequirementItemContract>();
                serviceRequest.Parameter = reqItemContarct;
                ServiceResponse<Boolean> serviceResponse = _requirementPackageProxy.SaveUpdateRequirementItemData(serviceRequest);
                if (!serviceResponse.Result)
                {
                    View.ErrorMessage = "Some error has occurred.Please try again.";
                }
                //else
                //{
                //    if (!View.UniversalItemData.IsNullOrEmpty())
                //    {
                //        View.UniversalItemData.UniversalItemID = View.UniCatItmMappingID;
                //    }
                //}
            }
        }

        public void GetRequirementCategoryItems()
        {
            View.lstCategoryItems = new List<RequirementItemContract>();
            if (View.RequirementCategoryID > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.RequirementCategoryID;
                ServiceResponse<List<RequirementItemContract>> serviceResponse = _requirementPackageProxy.GetRequirementItemsByCategoryID(serviceRequest);
                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    View.lstCategoryItems = serviceResponse.Result.OrderBy(x => x.RequirementItemDisplayOrder).ToList(); //UAT-3078
                }
            }
        }

        public Boolean DeleteReqCategoryItemMapping(Int32 reqCategoryItemID)
        {
            ServiceRequest<Int32, Int32, Boolean> serviceRequest = new ServiceRequest<Int32, Int32, Boolean>();
            serviceRequest.Parameter1 = reqCategoryItemID;
            serviceRequest.Parameter2 = View.SelectedPackageID;
            //UAT-2514:
            serviceRequest.Parameter3 = View.IsNewPackage;

            ServiceResponse<String> serviceResponse = _requirementPackageProxy.DeleteReqCategoryItemMapping(serviceRequest);
            View.ErrorMessage = serviceResponse.Result;
            return View.ErrorMessage.IsNullOrEmpty() ? true : false;
        }

        public Boolean DeleteReqItemFieldMapping(Int32 reqItemFieldID)
        {
            ServiceRequest<Int32, String, Boolean, Int32> serviceRequest = new ServiceRequest<Int32, String, Boolean, Int32>();
            serviceRequest.Parameter1 = reqItemFieldID;
            serviceRequest.Parameter2 = View.ItemHId;
            serviceRequest.Parameter3 = View.IsNewPackage;
            serviceRequest.Parameter4 = View.RequirementCategoryID;
            ServiceResponse<String> serviceResponse = _requirementPackageProxy.DeleteReqItemFieldMapping(serviceRequest);
            View.ErrorMessage = serviceResponse.Result;
            return View.ErrorMessage.IsNullOrEmpty() ? true : false;
        }

        public void GetRequirementItemFields()
        {
            View.lstItemFields = new List<RequirementFieldContract>();
            if (View.RequirementItemID > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.RequirementItemID;
                ServiceResponse<List<RequirementFieldContract>> serviceResponse = _requirementPackageProxy.GetRequirementFieldsByItemID(serviceRequest);
                if (!serviceResponse.Result.IsNullOrEmpty())
                {
                    View.lstItemFields = serviceResponse.Result.OrderBy(x => x.RequirementFieldDisplayOrder).ToList(); //UAT-3078
                }
            }
        }

        #region UAT-2305
        //public void GetUniversalItemsByUniReqCatID()
        //{
        //    View.lstUniversalItems = new List<UniversalItemContract>();
        //    ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
        //    serviceRequest.Parameter = View.RequirementCategoryID;
        //    ServiceResponse<List<UniversalItemContract>> serviceResponse = _requirementPackageProxy.GetUniversalItemsByUniReqCatID(serviceRequest);
        //    if (!serviceResponse.Result.IsNullOrEmpty())
        //    {
        //        View.lstUniversalItems = serviceResponse.Result.ToList();
        //    }
        //}
        //public void GetUniversalItemByReqCatItmID(Int32 ReqItmID)
        //{
        //    ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
        //    serviceRequest.Parameter1 = View.RequirementCategoryID;
        //    serviceRequest.Parameter2 = ReqItmID;
        //    ServiceResponse<UniversalItemContract> serviceResponse = _requirementPackageProxy.GetUniversalItemsByReqCatItmID(serviceRequest);
        //    if (!serviceResponse.Result.IsNullOrEmpty())
        //    {
        //        View.UniversalItemData = serviceResponse.Result;
        //        View.UniCatItmMappingID = View.UniversalItemData.UniCatItmMappingID;
        //    }
        //}
        //TO DO For deleteUAT-2305
        //public void DeleteUniversalReqItmMapping(Int32 requirementItemID)
        //{
        //    if (requirementItemID.IsNullOrEmpty())
        //    {
        //        GetUniversalItemByReqCatItmID(requirementItemID);
        //        if (View.UniversalItemData.IsNullOrEmpty() && View.UniversalItemData.UniReqItmMappingID.IsNullOrEmpty())
        //        {
        //            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
        //            serviceRequest.Parameter = View.UniversalItemData.UniReqItmMappingID;
        //            ServiceResponse<Boolean> serviceResponse = _requirementPackageProxy.DeleteUniversalReqItmMapping(serviceRequest);
        //        }
        //        View.UniversalItemData = new UniversalItemContract();
        //        View.UniCatItmMappingID = AppConsts.NONE;
        //    }
        //}
        #endregion
    }
}
