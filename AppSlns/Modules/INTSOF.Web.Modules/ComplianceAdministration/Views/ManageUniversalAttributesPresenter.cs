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
    public class ManageUniversalAttributesPresenter : Presenter<IManageUniversalAttributesView>
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


        private void GetAttributeDataType()
        {
            View.lstAttributeDatatype = UniversalMappingDataManager.GetAttributeDataType();
        }
        public void GetUniversalAttributesDetails()
        {
            View.lstUniversalField = UniversalMappingDataManager.GetUniversalAttributeField(AppConsts.NONE);
        }
        public Boolean DeleteUniversalFieldByID(Int32 universalFieldId)
        {
            return UniversalMappingDataManager.DeleteUniversalFieldByID(AppConsts.NONE, universalFieldId, View.CurrentUserId);
        }

        public Boolean IsValidOptionFormat(String options)
        {
            if (!String.IsNullOrEmpty(options))
            {
                //UAT-3486
                //string[] arrayOfOptions = options.Split(',');
                string[] arrayOfOptions = options.Split('|');

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

        public Boolean SaveUpdateUniversalField()
        {
            UniversalField unversalField = null;
            List<UniversalFieldOption> UAO = null;
            if (View.UniversalFieldID > AppConsts.NONE)
            {
                unversalField = UniversalMappingDataManager.GetUniversalFieldById(AppConsts.NONE, View.UniversalFieldID);

                unversalField.UF_ID = View.UniversalFieldID;
                unversalField.UF_AttributeDataTypeID = View.AttributeDataTypeID;
                unversalField.UF_Name = View.FieldName;
                unversalField.UF_ModifiedBy = View.CurrentUserId;
                unversalField.UF_ModifiedOn = DateTime.Now;

                if (unversalField.lkpUniversalAttributeDataType.LUADT_Code == UniversalAttributeDataTypeEnum.OPTIONS.GetStringValue())
                {
                    if (View.OptionDataTypeValue != GetOptionText(unversalField.UniversalFieldOptions.ToList()))
                    {
                        UAO = unversalField.UniversalFieldOptions.ToList();

                        //UAO.ForEach(x =>
                        //{
                        //    x.UFO_IsDeleted = true;
                        //    x.UFO_ModifiedBy = View.CurrentUserId;
                        //    x.UFO_ModifiedON = DateTime.Now;
                        //});

                        UAO = GetUniversalAttributeOption(View.OptionDataTypeValue, unversalField.UniversalFieldOptions.ToList()).ToList();
                        UAO.ForEach(x =>
                        {
                            unversalField.UniversalFieldOptions.Add(x);
                        });
                    }
                }
            }

            {
                unversalField = new UniversalField();
                unversalField.UF_ID = View.UniversalFieldID;
                unversalField.UF_AttributeDataTypeID = View.AttributeDataTypeID;
                unversalField.UF_Name = View.FieldName;
                unversalField.UF_CreatedBy = View.CurrentUserId;
                unversalField.UF_CreatedOn = DateTime.Now;
                unversalField.UF_IsDeleted = false;

                lkpUniversalAttributeDataType attributeDataType = View.lstAttributeDatatype.Where(x => x.LUADT_ID == View.AttributeDataTypeID).FirstOrDefault();
                if (attributeDataType.LUADT_Code == UniversalAttributeDataTypeEnum.OPTIONS.GetStringValue())
                {
                    UAO = GetUniversalAttributeOption(View.OptionDataTypeValue).ToList();
                    UAO.ForEach(x =>
                    {
                        unversalField.UniversalFieldOptions.Add(x);
                    });
                }
            }
            return UniversalMappingDataManager.SaveUpdateUniversalField(AppConsts.NONE, unversalField, View.CurrentUserId);
        }

        public void GetUniversalAttributeOptionTypeValue(Int32 universalFieldID)
        {
            UniversalField universalField = UniversalMappingDataManager.GetUniversalFieldById(AppConsts.NONE, View.UniversalFieldID);
            View.OptionDataTypeValue = GetOptionText(universalField.UniversalFieldOptions.ToList());
        }

        private String GetOptionText(List<UniversalFieldOption> uniAttributeOption)
        {
            StringBuilder formatOptions = new StringBuilder();
            if (uniAttributeOption != null && uniAttributeOption.Count > 0)
            {
                foreach (UniversalFieldOption attributeOption in uniAttributeOption.Where(x => !x.UFO_IsDeleted))
                    formatOptions.AppendFormat("{0}={1}|", attributeOption.UFO_OptionText, attributeOption.UFO_OptionValue);

                Int32 index = formatOptions.ToString().LastIndexOf('|');
                if (index >= 0)
                    formatOptions.Remove(index, 1);
            }
            return formatOptions.ToString();
        }

        private System.Data.Entity.Core.Objects.DataClasses.EntityCollection<UniversalFieldOption> GetUniversalAttributeOption(String options, List<UniversalFieldOption> universalFieldOptions)
        {
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<UniversalFieldOption> lstUniversalAttributeOption = null;
            if (String.IsNullOrEmpty(options))
                return lstUniversalAttributeOption;

            string[] arrayOfOptions = options.Split(',');
            if (arrayOfOptions.Length > 0)
            {
                List<String> optionTextList = new List<String>();
                for (int counter = 0; counter < arrayOfOptions.Length; counter++)
                {
                    string[] option = arrayOfOptions[counter].Split('=');
                    if (option.Length.Equals(2))
                    {
                        if (lstUniversalAttributeOption == null)
                            lstUniversalAttributeOption = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<UniversalFieldOption>();

                        optionTextList.Add(option[0].ToLower());

                        if (universalFieldOptions.Any(x => x.UFO_OptionText.ToLower() == option[0].ToLower() && !x.UFO_IsDeleted))
                        {
                            if (universalFieldOptions.Any(x => x.UFO_OptionText.ToLower() == option[0].ToLower() && !x.UFO_IsDeleted && x.UFO_OptionValue != option[1]))
                                universalFieldOptions.Where(x => x.UFO_OptionText.ToLower() == option[0].ToLower() && !x.UFO_IsDeleted).FirstOrDefault().UFO_OptionValue = option[1];
                        }
                        else
                        {
                            lstUniversalAttributeOption.Add(new UniversalFieldOption()
                            {
                                UFO_OptionText = option[0],
                                UFO_OptionValue = option[1],
                                UFO_CreatedBy = View.CurrentUserId,
                                UFO_CreatedOn = DateTime.Now,
                                UFO_IsDeleted = false

                            });
                        }

                    }
                }


                if (optionTextList.IsNotNull() && optionTextList.Count > AppConsts.NONE)
                {
                    universalFieldOptions.Where(x => !optionTextList.Contains(x.UFO_OptionText.ToLower()) && !x.UFO_IsDeleted).ForEach(x =>
                    {
                        x.UFO_IsDeleted = true;
                        x.UFO_ModifiedBy = View.CurrentUserId;
                        x.UFO_ModifiedON = DateTime.Now;
                    });
                }

            }
            return lstUniversalAttributeOption;
        }

        private System.Data.Entity.Core.Objects.DataClasses.EntityCollection<UniversalFieldOption> GetUniversalAttributeOption(String options)
        {
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<UniversalFieldOption> lstUniversalAttributeOption = null;
            if (String.IsNullOrEmpty(options))
                return lstUniversalAttributeOption;
            //UAT-3486
            //string[] arrayOfOptions = options.Split(',');
            string[] arrayOfOptions = options.Split('|');

            if (arrayOfOptions.Length > 0)
            {
                for (int counter = 0; counter < arrayOfOptions.Length; counter++)
                {
                    string[] option = arrayOfOptions[counter].Split('=');
                    if (option.Length.Equals(2))
                    {
                        if (lstUniversalAttributeOption == null)
                            lstUniversalAttributeOption = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<UniversalFieldOption>();

                        lstUniversalAttributeOption.Add(new UniversalFieldOption()
                        {
                            UFO_OptionText = option[0],
                            UFO_OptionValue = option[1],
                            UFO_CreatedBy = View.CurrentUserId,
                            UFO_CreatedOn = DateTime.Now,
                            UFO_IsDeleted = false

                        });
                    }
                }
            }
            return lstUniversalAttributeOption;
        }

        public Boolean IsUniversalFieldNameExists()
        {
            return UniversalMappingDataManager.IsUniversalFieldNameExists(AppConsts.NONE, View.FieldName.Trim());
        }
    }
}
