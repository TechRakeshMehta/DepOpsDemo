using Business.RepoManagers;
using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace INTSOF.SharedObjects
{
    public class ComplianceDataStore
    {
        private Dictionary<Int32, List<ComplianceCategoryItem>> _lstComplianceCategoriesItems;
        private Dictionary<Int32, List<ComplianceItemAttribute>> _lstComplianceItemsattributes;
        private Dictionary<String, List<lkpConstantType>> _lstConstantType;
        private Dictionary<String, List<lkpObjectType>> _lstObjectType;
        private Dictionary<String, List<ItemSeriesAttribute>> _lstSeriesAttribute;
        private Dictionary<String, List<lkpItemComplianceStatu>> _lstItemComplianceStatus;


        public Dictionary<Int32, List<ComplianceCategoryItem>> lstComplianceCategoriesItems
        {
            get
            {
                if (_lstComplianceCategoriesItems == null)
                    _lstComplianceCategoriesItems = new Dictionary<int, List<ComplianceCategoryItem>>();
                return _lstComplianceCategoriesItems;
            }

            set
            {
                _lstComplianceCategoriesItems = value;
            }
        }

        public Dictionary<Int32, List<ComplianceItemAttribute>> lstComplianceItemsattributes
        {
            get
            {
                if (_lstComplianceItemsattributes == null)
                    _lstComplianceItemsattributes = new Dictionary<int, List<ComplianceItemAttribute>>();
                return _lstComplianceItemsattributes;
            }
            set
            {
                _lstComplianceItemsattributes = value;
            }
        }

        List<CompliancePackageCategory> lstCompliaceCategories
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

        public Dictionary<String, List<ItemSeriesAttribute>> lstSeriesAttribute
        {
            get
            {
                if (_lstSeriesAttribute == null)
                    _lstSeriesAttribute = new Dictionary<String, List<ItemSeriesAttribute>>();
                return _lstSeriesAttribute;
            }
            set
            {
                _lstSeriesAttribute = value;
            }
        }

        public Dictionary<String, List<lkpItemComplianceStatu>> lstItemComplianceStatus
        {
            get
            {
                if (_lstItemComplianceStatus == null)
                    _lstItemComplianceStatus = new Dictionary<String, List<lkpItemComplianceStatu>>();
                return _lstItemComplianceStatus;
            }
            set
            {
                _lstItemComplianceStatus = value;
            }
        }

        public List<CompliancePackageCategory> getComplianceCategoriesByPackageId(Int32 packageId, Int32 tenantId)
        {
            if (lstCompliaceCategories == null)
            {
                lstCompliaceCategories = ComplianceSetupManager.GetcomplianceCategoriesByPackage(packageId, tenantId, false);
            }
            return lstCompliaceCategories;
        }

        public List<ComplianceCategoryItem> getComplianceItemsByCategoryId(Int32 categoryId, Int32 tenantId)
        {
            List<ComplianceCategoryItem> lstItems = new List<ComplianceCategoryItem>();
            if (lstComplianceCategoriesItems != null && lstComplianceCategoriesItems.TryGetValue(categoryId, out lstItems))
            {

            }
            else
            {
                lstItems = ComplianceSetupManager.GetComplianceCategoryItems(categoryId, tenantId, false);
                lstComplianceCategoriesItems.Add(categoryId, lstItems);
            }
            return lstItems;
        }

        public List<ComplianceItemAttribute> getComplianceAttributesByItemsId(Int32 itemId, Int32 tenantId)
        {
            List<ComplianceItemAttribute> lstAttributes = new List<ComplianceItemAttribute>();
            if (lstComplianceItemsattributes != null && lstComplianceItemsattributes.TryGetValue(itemId, out lstAttributes))
            {

            }
            else
            {
                lstAttributes = ComplianceSetupManager.GetComplianceItemAttribute(itemId, tenantId, false);
                lstComplianceItemsattributes.Add(itemId, lstAttributes);
            }
            return lstAttributes;
        }

        public List<lkpConstantType> getConstantGroup(Int32 tenantId, Boolean isSeriesDataRqd)
        {
            if (lstConstantGroup == null)
            {
                lstConstantGroup = RuleManager.getConstantType(tenantId, isSeriesDataRqd).ToList();
            }
            return lstConstantGroup;
        }

        public List<lkpConstantType> getConstantType(String group, Int32 tenantId,Boolean isSeriesDataRqd)
        {
            List<lkpConstantType> lstConstantList = new List<lkpConstantType>();
            if (lstConstantType != null && lstConstantType.TryGetValue(group, out lstConstantList))
            {

            }
            else
            {
                lstConstantList = RuleManager.getConstantType(tenantId,isSeriesDataRqd).Where(obj => obj.Group == group).ToList();
                lstConstantType.Add(group, lstConstantList);
            }
            return lstConstantList;
        }

        public List<lkpObjectType> getObjectType(String ruleObjectMappingTypeCode, Int32 tenantId)
        {
            List<lkpObjectType> lstObjectTypeList = new List<lkpObjectType>();
            if (lstObjectType != null && lstObjectType.TryGetValue(ruleObjectMappingTypeCode, out lstObjectTypeList))
            {

            }
            else
            {
                lstObjectTypeList = RuleManager.GetObjectTypes(ruleObjectMappingTypeCode, tenantId);
                lstObjectType.Add(ruleObjectMappingTypeCode, lstObjectTypeList);
            }
            return lstObjectTypeList;
        }


        public List<ItemSeriesAttribute> getItemSeriesAttribute(Int32 SelectedSeriesId, Int32 tenantId)
        {
            List<ItemSeriesAttribute> lstSeriesAttrList = new List<ItemSeriesAttribute>();
            if (lstSeriesAttribute != null && lstSeriesAttribute.TryGetValue(SelectedSeriesId.ToString(), out lstSeriesAttrList))
            {

            }
            else
            {
                lstSeriesAttrList = RuleManager.GetItemSeriesAttributeBySeriesId(SelectedSeriesId, tenantId);
                lstSeriesAttribute.Add(SelectedSeriesId.ToString(), lstSeriesAttrList);
            }
            return lstSeriesAttrList;
        }

        public List<lkpItemComplianceStatu> getItemComplianceStatusList(Int32 tenantId)
        {
            List<lkpItemComplianceStatu> lstItemComplianceStatuList = new List<lkpItemComplianceStatu>();
            if (lstItemComplianceStatus != null && lstItemComplianceStatus.TryGetValue(tenantId.ToString(), out lstItemComplianceStatuList))
            {

            }
            else
            {
                lstItemComplianceStatuList = ComplianceDataManager.GetItemComplianceStatus(tenantId);
                lstItemComplianceStatus.Add(tenantId.ToString(), lstItemComplianceStatuList);
            }
            return lstItemComplianceStatuList;
        }
    }
}
