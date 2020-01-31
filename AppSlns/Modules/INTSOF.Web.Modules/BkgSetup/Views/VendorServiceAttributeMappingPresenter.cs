using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using System.Data;

namespace CoreWeb.BkgSetup.Views
{
    public class VendorServiceAttributeMappingPresenter : Presenter<IVendorServiceAttributeMappingView>
    {
        public override void OnViewInitialized()
        {
            View.FormatType = BackgroundSetupManager.GetFormatTypes();
        }

        public void GetVendorServiceAttributeMapping()
        {
            View.VendorServiceAttributeMappingList = BackgroundSetupManager.GetVendorServiceAttributeMappingList(View.VendorServiceMappingID);
        }

        public Boolean DeleteVendorServiceAttributeMapping(Int32 currentUserId)
        {
            Boolean isDeleted = BackgroundSetupManager.DeleteVendorServiceAttributeMapping(View.VendorServiceMappingID, View.VendorServiceFieldID, currentUserId);
            return true;
        }

        public void BindDropDown()
        {
            Int32? vndSvcFieldId = null;
            if (View.VendorServiceFieldID != 0)
                vndSvcFieldId = View.VendorServiceFieldID;
            ServiceAttributesContract svcAttributeContract = BackgroundSetupManager.GetBkgSvcExtSvcAttributes(View.VendorServiceMappingID, vndSvcFieldId);
            if (svcAttributeContract.IsNotNull())
            {
                if (svcAttributeContract.ExternalServiceAttributeList != null && svcAttributeContract.ExternalServiceAttributeList.Count > 0)
                {
                    View.ExternalServiceAttributeList = svcAttributeContract.ExternalServiceAttributeList;
                }
                if (svcAttributeContract.InternalServiceAttributeList != null && svcAttributeContract.InternalServiceAttributeList.Count > 0)
                {
                    View.InternalServiceAttributeList = svcAttributeContract.InternalServiceAttributeList;
                }
            }
        }

        public Boolean AddVendorServiceAttributeMapping()
        {
            List<ExternalSvcAtributeMapping> extSvcAttMappingList = new List<ExternalSvcAtributeMapping>();

            if (View.ViewContract.BkgSvcAttMappingIDs != null && View.ViewContract.BkgSvcAttMappingIDs.Count > 0)
            {
                foreach (var item in View.ViewContract.BkgSvcAttMappingIDs)
                {
                    ExternalSvcAtributeMapping extSvcAttMapping = new ExternalSvcAtributeMapping();
                    extSvcAttMapping.ESAM_BkgSvcAttributeGroupMappingID = item; //View.ViewContract.ESAM_BkgSvcAttributeGroupMappingID;
                    extSvcAttMapping.ESAM_ExternalBkgSvcAttributeID = View.ViewContract.ESAM_ExternalBkgSvcAttributeID;
                    extSvcAttMapping.ESAM_DefaultValue = null;
                    extSvcAttMapping.ESAM_CountyInd = false;
                    extSvcAttMapping.ESAM_FieldDelimiter = View.ViewContract.ESAM_FieldDelimiter;
                    extSvcAttMapping.ESAM_Code = Guid.NewGuid();
                    extSvcAttMapping.ESAM_CopiedFromCode = null;
                    extSvcAttMapping.ESAM_IsDeleted = false;
                    extSvcAttMapping.ESAM_CreatedBy = View.ViewContract.ESAM_CreatedBy;
                    extSvcAttMapping.ESAM_CreatedOn = View.ViewContract.ESAM_CreatedOn;
                    extSvcAttMapping.ESAM_ModifiedBy = null;
                    extSvcAttMapping.ESAM_ModifiedOn = null;
                    extSvcAttMapping.ESAM_ServiceMappingId = View.ViewContract.ESAM_ServiceMappingId;

                    extSvcAttMappingList.Add(extSvcAttMapping);
                }

                if (View.ViewContract.ExtSvcAttList != null && View.ViewContract.ExtSvcAttList.Count > 0)
                {
                    foreach (var item in View.ViewContract.ExtSvcAttList)
                    {
                        extSvcAttMappingList.Where(x => x.ESAM_BkgSvcAttributeGroupMappingID.HasValue
                                                        ?(x.ESAM_BkgSvcAttributeGroupMappingID.Value 
                                                        == item.BkgSvcAttributeGroupMappingID.Value):false)
                                                        .ForEach(condition =>
                                                        {
                                                            condition.ESAM_FieldSequence = item.FieldSequence;
                                                            condition.ESAM_FormatType = item.FormatTypeID;
                                                        });
                    }
                }
            }
            else
            {
                ExternalSvcAtributeMapping extSvcAttMappingData = new ExternalSvcAtributeMapping
                {
                    ESAM_BkgSvcAttributeGroupMappingID = View.ViewContract.ESAM_BkgSvcAttributeGroupMappingID,
                    ESAM_ExternalBkgSvcAttributeID = View.ViewContract.ESAM_ExternalBkgSvcAttributeID,
                    ESAM_DefaultValue = null,
                    ESAM_CountyInd = false,
                    ESAM_FieldDelimiter = View.ViewContract.ESAM_FieldDelimiter,
                    ESAM_Code = Guid.NewGuid(),
                    ESAM_CopiedFromCode = null,
                    ESAM_IsDeleted = false,
                    ESAM_CreatedBy = View.ViewContract.ESAM_CreatedBy,
                    ESAM_CreatedOn = View.ViewContract.ESAM_CreatedOn,
                    ESAM_ModifiedBy = null,
                    ESAM_ModifiedOn = null,
                    ESAM_ServiceMappingId = View.ViewContract.ESAM_ServiceMappingId
                };
                extSvcAttMappingList.Add(extSvcAttMappingData);
            }

            Boolean isAdded = BackgroundSetupManager.SaveVendorServiceAttributeMapping(extSvcAttMappingList);
            return true;
        }

        public Boolean UpdateVendorServiceAttributeMapping(Int32 currentUserId)
        {
            Boolean isDeleted = DeleteVendorServiceAttributeMapping(currentUserId);
            Boolean isAdded = AddVendorServiceAttributeMapping();
            return true;
        }
    }
}
