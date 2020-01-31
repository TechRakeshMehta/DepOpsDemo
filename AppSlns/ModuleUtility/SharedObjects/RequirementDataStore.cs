using Business.RepoManagers;
using Entity.SharedDataEntity;
using INTSOF.UI.Contract.RotationPackages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSiteUtils.SharedObjects
{
    public class RequirementDataStore
    {
        #region Private variable

        private Dictionary<Int32, List<RequirementExpressionData>> _lstRequirementCategoryItems;
        private Dictionary<Int32, List<RequirementExpressionData>> _lstRequirementItemFields;
        private Dictionary<String, List<lkpConstantType>> _lstConstantType;
        private Dictionary<String, List<lkpObjectType>> _lstObjectType;
        private Dictionary<String, List<lkpRequirementItemStatus>> _lstRequirementItemComplianceStatus;

        #endregion

        #region Properties

        List<RequirementPackageCategory> lstrequirementCategories
        {
            get;
            set;
        }

        List<lkpConstantType> lstConstantGroup
        {
            get;
            set;
        }

        public Dictionary<String, List<lkpConstantType>> lstConstantType
        {
            get
            {
                if (_lstConstantType == null)
                    _lstConstantType = new Dictionary<String, List<lkpConstantType>>();
                return _lstConstantType;
            }
            set
            {
                _lstConstantType = value;
            }
        }

        public Dictionary<String, List<lkpObjectType>> lstObjectType
        {
            get
            {
                if (_lstObjectType == null)
                    _lstObjectType = new Dictionary<String, List<lkpObjectType>>();
                return _lstObjectType;
            }
            set
            {
                _lstObjectType = value;
            }
        }

        public Dictionary<String, List<lkpRequirementItemStatus>> lstRequirementItemComplianceStatus
        {
            get
            {
                if (_lstRequirementItemComplianceStatus == null)
                    _lstRequirementItemComplianceStatus = new Dictionary<String, List<lkpRequirementItemStatus>>();
                return _lstRequirementItemComplianceStatus;
            }
            set
            {
                _lstRequirementItemComplianceStatus = value;
            }
        }

        public Dictionary<Int32, List<RequirementExpressionData>> lstRequirementCategoriesItems
        {
            get
            {
                if (_lstRequirementCategoryItems == null)
                    _lstRequirementCategoryItems = new Dictionary<Int32, List<RequirementExpressionData>>();
                return _lstRequirementCategoryItems;
            }

            set
            {
                _lstRequirementCategoryItems = value;
            }
        }

        public Dictionary<Int32, List<RequirementExpressionData>> lstRequirementItemsattributes
        {
            get
            {
                if (_lstRequirementItemFields == null)
                    _lstRequirementItemFields = new Dictionary<Int32, List<RequirementExpressionData>>();
                return _lstRequirementItemFields;
            }
            set
            {
                _lstRequirementItemFields = value;
            }
        }

        public List<RequirementExpressionData> lstRequirementCategories { get; set; } 

        #endregion

        #region Methods

        public List<RequirementExpressionData> GetRequirementCategoriesByCategoryId(Int32 categoryID, String ObjectTypeCode)
        {
            if (lstRequirementCategories == null)
            {
                lstRequirementCategories = RequirementRuleManager.GetRequirementCategoryByCategoryID(categoryID, ObjectTypeCode);
            }
            return lstRequirementCategories;
        }

        public List<RequirementExpressionData> GetRequirementItemsByCategoryId(Int32 categoryId)
        {
            List<RequirementExpressionData> lstItems = new List<RequirementExpressionData>();
            if (lstRequirementCategoriesItems != null && lstRequirementCategoriesItems.TryGetValue(categoryId, out lstItems))
            {

            }
           else
            {
                lstItems = RequirementRuleManager.GetRequirementCategoryItems(categoryId);
                lstRequirementCategoriesItems.Add(categoryId, lstItems);
            }
            return lstItems;
        }

        public List<RequirementExpressionData> GetRequirementSubmissionItemsByCategoryId(Int32 categoryId)
        {
            List<RequirementExpressionData> lstItems = new List<RequirementExpressionData>();
            if (lstRequirementCategoriesItems != null && lstRequirementCategoriesItems.TryGetValue(categoryId, out lstItems))
            {

            }
            else
            {
                lstItems = RequirementRuleManager.GetRequirementSubmissionItemsByCategoryID(categoryId);
                lstRequirementCategoriesItems.Add(categoryId, lstItems);
            }
            return lstItems;
        }


        public List<RequirementExpressionData> GetRequirementAttributesByItemsId(Int32 itemId)
        {
            List<RequirementExpressionData> lstAttributes = new List<RequirementExpressionData>();
            if (lstRequirementItemsattributes != null && lstRequirementItemsattributes.TryGetValue(itemId, out lstAttributes))
            {

            }
            else
            {
                lstAttributes = RequirementRuleManager.GetComplianceItemAttribute(itemId);
                lstRequirementItemsattributes.Add(itemId, lstAttributes);
            }
            return lstAttributes;
        }

        public List<lkpConstantType> getConstantGroup()
        {
            if (lstConstantGroup == null)
            {
                lstConstantGroup = RequirementRuleManager.GetConstantType().ToList();
            }
            return lstConstantGroup;
        }

        public List<lkpConstantType> getConstantType(String group)
        {
            List<lkpConstantType> lstConstantList = new List<lkpConstantType>();
            if (lstConstantType != null && lstConstantType.TryGetValue(group, out lstConstantList))
            {

            }
            else
            {
                lstConstantList = RequirementRuleManager.GetConstantType().Where(obj => obj.Group == group).ToList();
                lstConstantType.Add(group, lstConstantList);
            }
            return lstConstantList;
        }

        public List<lkpObjectType> GetObjectType(String ruleObjectMappingTypeCode)
        {
            List<lkpObjectType> lstObjectTypeList = new List<lkpObjectType>();
            if (lstObjectType != null && lstObjectType.TryGetValue(ruleObjectMappingTypeCode, out lstObjectTypeList))
            {
                return lstObjectTypeList;
            }
            else
            {
                lstObjectTypeList = RequirementRuleManager.GetObjectTypes(ruleObjectMappingTypeCode);
                lstObjectType.Add(ruleObjectMappingTypeCode, lstObjectTypeList);
            }
            return lstObjectTypeList;
        }

        public List<lkpRequirementItemStatus> GetItemComplianceStatusList()
        {
            List<lkpRequirementItemStatus> lstRequirementItemStatuList = new List<lkpRequirementItemStatus>();
            if (lstRequirementItemStatuList != null && lstRequirementItemStatuList.Count > 0)
            {
                return lstRequirementItemStatuList;
            }
            else
            {
                lstRequirementItemStatuList = RequirementRuleManager.GetItemComplianceStatus();
            }
            return lstRequirementItemStatuList;
        }

        #endregion


































    }
}
