using Business.RepoManagers;
using Entity.SharedDataEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class ManageUniversalAttributePresenter : Presenter<IManageUniversalAttributeView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            GetAttributeDataType();
        }

        public Boolean DeleteUniversalAttributeByID()
        {
            if (View.UniversalItemID > AppConsts.NONE && View.UniversalAttributeID > AppConsts.NONE)
            {
                return UniversalMappingDataManager.DeleteUniversalAttributeByID(View.UniversalItemID, View.UniversalAttributeID, View.CurrentUserId);
            }
            return false;
        }

        public void GetUniversalAttributesDetails()
        {
            List<UniversalItemAttributeMapping> UIAM = UniversalMappingDataManager.GetUniversalAttributesByItemID(0, View.UniversalItemID);

            View.lstUniversalAttribute = new List<UniversalAttribute>();

            UIAM.ForEach(x =>
            {
                if (!x.UniversalAttribute.UA_IsDeleted)
                {
                    View.lstUniversalAttribute.Add(x.UniversalAttribute);
                }
            });
        }

        private void GetAttributeDataType()
        {
            View.lstAttributeDatatype = UniversalMappingDataManager.GetAttributeDataType();
        }

        public Boolean SaveUpdateAttributeDetails()
        {
            UniversalAttribute UA = null;
            UniversalItemAttributeMapping UIAM = null;
            List<UniversalAttributeOption> UAO = null;

            if (View.UniversalAttributeID > AppConsts.NONE)
            {
                UA = UniversalMappingDataManager.GetUniversalAttributeDataByID(View.UniversalItemID, View.UniversalAttributeID);
                UIAM = UA.UniversalItemAttributeMappings.Where(cond => !cond.UIAM_IsDeleted).FirstOrDefault();

                UA.UA_Name = View.AttributeName;
                UA.UA_ModifiedBy = View.CurrentUserId;
                UA.UA_ModifiedOn = DateTime.Now;

                UIAM.UIAM_UniversalItemID = View.UniversalItemID;
                UIAM.UIAM_ModifiedBy = View.CurrentUserId;
                UIAM.UIAM_ModifiedOn = DateTime.Now;

                if (UA.lkpUniversalAttributeDataType.LUADT_Code == UniversalAttributeDataTypeEnum.OPTIONS.GetStringValue())
                {
                    if (View.OptionDataTypeValue != GetOptionText(UIAM.UniversalAttributeOptions.ToList()))
                    {
                        UAO = UIAM.UniversalAttributeOptions.ToList();

                        UAO.ForEach(x =>
                        {
                            x.UAO_IsDeleted = true;
                            x.UAO_ModifiedBy = View.CurrentUserId;
                            x.UAO_ModifiedON = DateTime.Now;
                        });

                        UAO = GetUniversalAttributeOption(View.OptionDataTypeValue).ToList();
                        UAO.ForEach(x =>
                        {
                            UIAM.UniversalAttributeOptions.Add(x);
                        });
                    }
                }
            }
            else
            {
                UA = new UniversalAttribute();
                UIAM = new UniversalItemAttributeMapping();

                UA.UA_Name = View.AttributeName;
                UA.UA_CreatedOn = DateTime.Now;
                UA.UA_CreatedBy = View.CurrentUserId;
                UA.UA_IsDeleted = false;
                UA.UA_AttributeDataTypeID = View.lstAttributeDatatype.Where(cond => cond.LUADT_Code == View.AttributeDataTypeID).FirstOrDefault().LUADT_ID;

                if (View.AttributeDataTypeID == UniversalAttributeDataTypeEnum.OPTIONS.GetStringValue())
                {
                    UAO = GetUniversalAttributeOption(View.OptionDataTypeValue).ToList();
                    UAO.ForEach(x =>
                    {
                        UIAM.UniversalAttributeOptions.Add(x);
                    });
                }

                UIAM.UIAM_UniversalItemID = View.UniversalItemID;
                UIAM.UIAM_CreatedBy = View.CurrentUserId;
                UIAM.UIAM_CreatedOn = DateTime.Now;
                UIAM.UIAM_IsDeleted = false;

                UA.UniversalItemAttributeMappings.Add(UIAM);
            }
            return UniversalMappingDataManager.SaveUpdateAttributeDetails(UA);
        }

        public Boolean IsValidAttributeName()
        {
            UniversalAttribute UA = UniversalMappingDataManager.GetUniversalAttributeDataByID(View.UniversalItemID, View.UniversalAttributeID);
            if (!UA.IsNullOrEmpty() && UA.UA_Name == View.AttributeName)
            {
                return false;
            }
            return UniversalMappingDataManager.IsValidAttributeName(View.AttributeName);
        }

        public void GetUniversalAttributeData()
        {
            if (View.UniversalItemID > AppConsts.NONE && View.UniversalAttributeID > AppConsts.NONE)
            {
                UniversalAttribute UA = UniversalMappingDataManager.GetUniversalAttributeDataByID(View.UniversalItemID, View.UniversalAttributeID);

                View.AttributeName = UA.UA_Name;
                View.AttributeDataTypeID = UA.lkpUniversalAttributeDataType.LUADT_Code;
                View.OptionDataTypeValue = GetOptionText(UA.UniversalItemAttributeMappings.FirstOrDefault().UniversalAttributeOptions.ToList());
            }
        }

        public Boolean IsValidOptionFormat(String options)
        {
            if (!String.IsNullOrEmpty(options))
            {
                string[] arrayOfOptions = options.Split(',');
                if (arrayOfOptions.Length > 0)
                {
                    for (int counter = 0; counter < arrayOfOptions.Length; counter++)
                    {
                        string[] option = arrayOfOptions[counter].Split('=');
                        if (!option.Length.Equals(2) || String.IsNullOrEmpty(option[1]))
                            return false;

                    }
                }
                else
                    return false;
            }
            return true;
        }

        private String GetOptionText(List<UniversalAttributeOption> uniAttributeOption)
        {
            StringBuilder formatOptions = new StringBuilder();
            if (uniAttributeOption != null && uniAttributeOption.Count > 0)
            {
                foreach (UniversalAttributeOption attributeOption in uniAttributeOption.Where(x => !x.UAO_IsDeleted))
                    formatOptions.AppendFormat("{0}={1},", attributeOption.UAO_OptionText, attributeOption.UAO_OptionValue);

                Int32 index = formatOptions.ToString().LastIndexOf(',');
                if (index >= 0)
                    formatOptions.Remove(index, 1);
            }
            return formatOptions.ToString();
        }

        private System.Data.Entity.Core.Objects.DataClasses.EntityCollection<UniversalAttributeOption> GetUniversalAttributeOption(String options)
        {
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<UniversalAttributeOption> lstUniversalAttributeOption = null;
            if (String.IsNullOrEmpty(options))
                return lstUniversalAttributeOption;

            string[] arrayOfOptions = options.Split(',');
            if (arrayOfOptions.Length > 0)
            {
                for (int counter = 0; counter < arrayOfOptions.Length; counter++)
                {
                    string[] option = arrayOfOptions[counter].Split('=');
                    if (option.Length.Equals(2))
                    {
                        if (lstUniversalAttributeOption == null)
                            lstUniversalAttributeOption = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<UniversalAttributeOption>();

                        lstUniversalAttributeOption.Add(new UniversalAttributeOption()
                        {
                            UAO_OptionText = option[0],
                            UAO_OptionValue = option[1],
                            UAO_CreatedBy = View.CurrentUserId,
                            UAO_CreatedOn = DateTime.Now,
                            UAO_IsDeleted = false

                        });
                    }
                }
            }
            return lstUniversalAttributeOption;
        }
    }
}
