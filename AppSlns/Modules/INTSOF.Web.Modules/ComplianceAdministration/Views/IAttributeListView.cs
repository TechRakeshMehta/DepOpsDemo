using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IAttributeListView
    {
        Int32 tenantId
        {
            get;
            set;
        }

        Int32 DefaultTenantId { get; set; }

        Int32 CurrentUserId
        {
            get;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        #region UAT-2985

        List<Entity.SharedDataEntity.UniversalField> LstUniversalField { get; set; }

        Int32 SelectedUniversalFieldID
        {
            get;
            set;
        }

        Int32 ComplianceCategoryItemID { get; set; }

        Int32 ComplianceItemAttributeID { get; set; }

        //List<ComplianceAttributeOptionMappingContract> lstMappedFieldOptionData
        //{
        //    get;
        //    set;
        //}

        Dictionary<Int32, String> lstUniversalFieldOptions
        {
            get;
            set;
        }

        List<Int32> selectedInputFields { get; set; }

        List<UniversalFieldInputTypeMapping> lstSelectedInputFieldData
        {
            get;
            set;
        }

        Int32 MappedUniversalFieldMappingID
        {
            get;
            set;
        }

        Int32 AttributeID { get; set; }
        Int32 CategoryId { get; set; }
        Int32 ItemID { get; set; }

        #endregion

        //#region UAT-2305:Tracking to Rotation category/item/attribute mapping
        //List<Entity.SharedDataEntity.UniversalAttribute> LstUniversalAttribute { get; set; }

        //Int32 SelectedUniversalItemAttrID
        //{
        //    get;
        //    set;
        //}

        //Int32 SelectedUniversalItemID
        //{
        //    get;
        //    set;
        //}
        //Int32 MappedUniversalItemID
        //{
        //    get;
        //    set;
        //}

        //Int32 ItemAttributeMappingID { get; set; }

        //List<Int32> selectedInputAttributes { get; set; }
        //List<UniversalAttributeInputTypeMapping> lstSelectedInputAttributesData
        //{
        //    get;
        //    set;
        //}
        //#endregion

        //#region UAT-2402:

        List<ComplianceAttributeOptionMappingContract> lstSelectedAttributeOptionData
        {
            get;
            set;
        }

        //Dictionary<Int32, String> lstUniversalAttributeOptions
        //{
        //    get;
        //    set;
        //}

        ////Int32 AttrID { get; set; }
        //#endregion
    }
}




