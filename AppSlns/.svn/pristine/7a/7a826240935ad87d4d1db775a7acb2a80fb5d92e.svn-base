using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Linq;
using Entity.ClientEntity;
using System.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using DAL;
using INTSOF.Utils;
using INTSOF.UI.Contract.SystemSetUp;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class CustomAttributeProfileLoader : BaseUserControl, ICustomAttributeLoaderView
    {
        #region Variables

        #region Private Variables

        private CustomAttributeLoaderPresenter _presenter = new CustomAttributeLoaderPresenter();
        private Boolean _isFalsePostBack = false;
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack || IsFalsePostBack)
            {
                Presenter.OnViewInitialized();

            }
            Presenter.OnViewLoaded();
            litTitle.Text = this.Title;
            if (CurrentViewContext.DataSourceModeType == DataSourceMode.Ids && CurrentViewContext.IsOrder == false)
            {
                Presenter.GetCustomAttributes(CurrentViewContext.MappingRecordId, CurrentViewContext.ValueRecordId, CurrentViewContext.TypeCode, CurrentViewContext.TenantId);
            }
            else if (CurrentViewContext.DataSourceModeType == DataSourceMode.Ids && CurrentViewContext.IsOrder == true)
            {
                Presenter.GetCustomAttributesByNodes(CurrentViewContext.TypeCode, CurrentViewContext.ValueRecordId, CurrentViewContext.TenantId);
            }

            #region UAT 1438: Enhancement to allow students to select a User Group.

            if (!ShowUserGroupCustomAttribute)
            {
                if (!CurrentViewContext.lstTypeCustomAttributes.IsNullOrEmpty())
                {
                    //Removes the User Group Custom Attribute Type from the list.
                    //i.e UserGroup Type custom attribute control will not generated.
                    CurrentViewContext.lstTypeCustomAttributes.RemoveAll(x => x.CADataTypeCode.ToLower().Trim() == CustomAttributeDatatype.User_Group.GetStringValue().ToLower().Trim());
                }

                //This will create a new section with Single User Group Custom Attribute.
                if (ShowUserGroupCustAttributeMerged)
                {
                    CurrentViewContext.lstTypeCustomAttributes = new List<TypeCustomAttributes>();
                    UpdateCustomAttributeTypeForUserGroup();
                }
            }

            if (ShowReadOnlyUserGroupCustomAttribute)
            {
                UpdateCustomAttributeTypeForUserGroup();
            }

            String usrGrpCode = CustomAttributeDatatype.User_Group.GetStringValue().ToLower();
            if (!CurrentViewContext.lstTypeCustomAttributes.IsNullOrEmpty())
            {
                if ((CurrentViewContext.lstTypeCustomAttributes.Any(x => x.CADataTypeCode.ToLower() == usrGrpCode) && CurrentViewContext.DataSourceModeType != DataSourceMode.ExternalList)
                    && (lstUserGroups.IsNullOrEmpty() && lstUserGroupsForUser.IsNullOrEmpty()))
                {
                    Presenter.GetAllUserGroup();
                    Presenter.GetUserGroupsForUser();
                }

                //Reordering the position of UserGroupCustomAttribute.
                TypeCustomAttributes typeCustomAttribute = CurrentViewContext.lstTypeCustomAttributes.Where(x => x.CADataTypeCode.ToLower().Trim() == CustomAttributeDatatype.User_Group.GetStringValue().ToLower().Trim()).FirstOrDefault();
                if (typeCustomAttribute.IsNotNull())
                {
                    CurrentViewContext.lstTypeCustomAttributes.RemoveAll(x => x.CADataTypeCode.ToLower().Trim() == CustomAttributeDatatype.User_Group.GetStringValue().ToLower().Trim());
                    CurrentViewContext.lstTypeCustomAttributes.Add(typeCustomAttribute);
                }
            }
            #endregion

            SetCustomAttributeValues();

            // To manage the design of the control as per the Order confirmation screen
            //if (this.ControlDisplayMode == DisplayMode.Controls || this.ControlDisplayMode == DisplayMode.ReadOnlyLabels)
            //{
            //    divForm.Attributes.Add("class", "sxform auto");
            //    pnlRows.Attributes.Add("class", "sxpnl");

            //}
            //else
            //{
            //    divSection.Attributes.Remove("class");
            //    litTitle.Text = String.Format("<h6><font style='color : #1c4d87;'>{0}</font></h6>", this.Title);
            //}
        }

        private void UpdateCustomAttributeTypeForUserGroup()
        {
            if (lstTypeCustomAttributes != null) //Do not replace it with null or empty.
            {
                TypeCustomAttributes typeCustomAttributes = new TypeCustomAttributes();
                typeCustomAttributes.CAId = AppConsts.NONE;
                typeCustomAttributes.CAMId = AppConsts.NONE;
                typeCustomAttributes.CALabel = "User Group";
                typeCustomAttributes.CADataTypeCode = CustomAttributeDatatype.User_Group.GetStringValue();
                lstTypeCustomAttributes.Add(typeCustomAttributes);
            }
        }



        #endregion

        #region Methods

        #region Private Methods

        private void SetCustomAttributeValues()
        {
            #region Code Commented in UAT-3067
            ////UAT-2883
            //if (!NetIDAttributeValue.IsNullOrEmpty())
            //{
            //    Int32 customAttributeIDofNetID = Presenter.GetCustomAttributeIDFromAppConfiguration(CurrentViewContext.TenantId, AppConsts.NET_ID);
            //    if (customAttributeIDofNetID != AppConsts.NONE && !NetIDAttributeValue.IsNullOrEmpty())
            //    {
            //        CurrentViewContext.lstTypeCustomAttributes.ForEach(x =>
            //        {
            //            if (x.CAId == customAttributeIDofNetID)
            //            {
            //                x.CAValue = NetIDAttributeValue;
            //            }
            //        });
            //    }
            //}
            //else if (!StudentIdAttributeValue.IsNullOrEmpty())
            //{
            //    Int32 customAttributeIDofStudentId = Presenter.GetCustomAttributeIDFromAppConfiguration(CurrentViewContext.TenantId, AppConsts.STUDENT_ID);
            //    if (customAttributeIDofStudentId != AppConsts.NONE)
            //    {
            //        CurrentViewContext.lstTypeCustomAttributes.ForEach(x =>
            //        {
            //            if (x.CAId == customAttributeIDofStudentId)
            //            {
            //                x.CAValue = StudentIdAttributeValue;
            //            }
            //        });
            //    }
            //}
            ////UAT-3067
            //if (!PeopleSoftAttributeValue.IsNullOrEmpty())
            //{
            //    Int32 customAttributeIDofPeopleSoftID = Presenter.GetCustomAttributeIDFromAppConfiguration(CurrentViewContext.TenantId, AppConsts.PEOPLE_SOFT_ID);
            //    if (customAttributeIDofPeopleSoftID != AppConsts.NONE && !PeopleSoftAttributeValue.IsNullOrEmpty())
            //    {
            //        CurrentViewContext.lstTypeCustomAttributes.ForEach(x =>
            //        {
            //            if (x.CAId == customAttributeIDofPeopleSoftID)
            //            {
            //                x.CAValue = PeopleSoftAttributeValue;
            //            }
            //        });
            //    }
            //} 
            #endregion

            //UAT-3067
            if (!dictShibbolethCustomAttributes.IsNullOrEmpty())
            {
                dictShibbolethCustomAttributes.ForEach(x =>
                {
                    CurrentViewContext.lstTypeCustomAttributes.ForEach(y =>
                    {
                        if ((Convert.ToInt32(x.Key)) == y.CAId)
                        {
                            y.CAValue = x.Value;
                        }
                    });
                });
            }

            //UAT-3133
            if (!ShibbolethHandlerType.IsNullOrEmpty() && ShibbolethHandlerType.ToLower() == AppConsts.SHIBBOLETH_UCONN)
            {
                Int32 peopleSoftID = AppConsts.NONE;
                peopleSoftID = Presenter.GetCustomAttributeIDFromAppConfiguration(this.TenantId, AppConsts.PEOPLE_SOFT_ID);
                if (peopleSoftID > AppConsts.NONE)
                {
                    CurrentViewContext.lstTypeCustomAttributes.ForEach(x =>
                    {
                        if (x.CAId == peopleSoftID)
                        {
                          x.IsReadOnly = "true";
                        }
                    });
                }
            }

            Int32 _attributesPerRow = 3;
            List<TypeCustomAttributes> lstDistinctAttributes = new List<TypeCustomAttributes>();

            //if (CurrentViewContext.lstTypeCustomAttributes == null)
            //    CurrentViewContext.lstTypeCustomAttributes = new List<TypeCustomAttributes>();
            if (CurrentViewContext.lstTypeCustomAttributes != null)
                lstDistinctAttributes = CurrentViewContext.lstTypeCustomAttributes.DistinctBy(x => x.CAId).ToList();

            // List of current attributes to be added into the control
            //List<TypeCustomAttributes> lst = CurrentViewContext.lstTypeCustomAttributes.Take(_attributesPerRow).ToList();
            List<TypeCustomAttributes> lst = lstDistinctAttributes.Take(_attributesPerRow).ToList();

            // Maintains list of attributes that are already added
            List<Int32> lstTemporary = new List<Int32>();

            Int32 _rowsAdded = 0;
            //for (int i = 1; i <= Math.Ceiling(Convert.ToDecimal(CurrentViewContext.lstTypeCustomAttributes.Count()) / _attributesPerRow); i++)
            for (int i = 1; i <= Math.Ceiling(Convert.ToDecimal(lstDistinctAttributes.Count()) / _attributesPerRow); i++)
            {
                if (_rowsAdded == _attributesPerRow)
                {
                    _rowsAdded = 0;
                }
                lst = new List<TypeCustomAttributes>();
                //foreach (var tca in CurrentViewContext.lstTypeCustomAttributes)
                //UAT-2440: Profile level custom attributes by tenant.
                if (CurrentViewContext.TypeCode == CustomAttributeUseTypeContext.Profile.GetStringValue())
                {
                    foreach (TypeCustomAttributes tca in lstDistinctAttributes)
                    {
                        if (!lstTemporary.Contains(tca.CAId))
                            lst.Add(tca);
                    }
                    lst = lst.Take(_attributesPerRow).ToList();
                    lstTemporary.AddRange(lst.Select(tca => tca.CAId));
                    LoadRowControl(lst);
                    _rowsAdded += 1;
                }
                else
                {
                    foreach (var tca in lstDistinctAttributes)
                    {
                        if (!lstTemporary.Contains(tca.CAMId.Value))
                            lst.Add(tca);
                    }
                    lst = lst.Take(_attributesPerRow).ToList();
                    lstTemporary.AddRange(lst.Select(tca => tca.CAMId.Value));
                    LoadRowControl(lst);
                    _rowsAdded += 1;
                }


            }

            //if (CurrentViewContext.lstTypeCustomAttributes.Count() > 0)
            if (lstDistinctAttributes.Count() > 0)
                this.Visible = true;
            else
                this.Visible = false;
        }

        /// <summary>
        ///  Add the row control for the selected type.
        /// </summary>
        /// <param name="lstAttributes">List of attributes for which Row control is to be added</param>
        private void LoadRowControl(List<TypeCustomAttributes> lstAttributes)
        {
            Control customAttributeRow = Page.LoadControl("~\\ComplianceAdministration\\UserControl\\CustomAttributeRowProfileControl.ascx");

            (customAttributeRow as CustomAttributeRowProfileControl).lstTypeCustomAttributes = lstAttributes;
            (customAttributeRow as CustomAttributeRowProfileControl).SelectedRecordId = this.ValueRecordId;
            (customAttributeRow as CustomAttributeRowProfileControl).IsReadOnly = this.IsReadOnly;
            (customAttributeRow as CustomAttributeRowProfileControl).ControlDisplayMode = this.ControlDisplayMode;

            #region UAT 1438: Enhancement to allow students to select a User Group.

            (customAttributeRow as CustomAttributeRowProfileControl).TenantID = this.TenantId;
            (customAttributeRow as CustomAttributeRowProfileControl).OrganizationUserId = this.CurrentLoggedInUserId;
            (customAttributeRow as CustomAttributeRowProfileControl).ShowReadOnlyUserGroupCustomAttribute = this.ShowReadOnlyUserGroupCustomAttribute;
            (customAttributeRow as CustomAttributeRowProfileControl).lstUserGroups = this.lstUserGroups;
            (customAttributeRow as CustomAttributeRowProfileControl).lstUserGroupsForUser = this.lstUserGroupsForUser;
            #endregion

            if (!String.IsNullOrEmpty(this.ValidationGroup))
                (customAttributeRow as CustomAttributeRowProfileControl).ValidationGroup = this.ValidationGroup;

            if (this.AttributeValues.IsNotNull() && AttributeValues.Count() > 0)
                (customAttributeRow as CustomAttributeRowProfileControl).AttributeValues = this.AttributeValues;

            pnlRows.Controls.Add(customAttributeRow);
        }

        /// <summary>
        /// Add data of the attributes into list, both add and update mode
        /// </summary>
        private void AddDataToList(object ctrl, String attributeValue, String attributeTypeCode, String lblText)
        {
            // Get the 'ApplicantComplianceAttributeId' while updating 
            Control ucAttributes = (ctrl as CustomAttributeProfileControl) as Control;
            Control hdfAttribute = ucAttributes.FindControlRecursive("hdfCAVId");
            Control hdfIdentity = ucAttributes.FindControlRecursive("hdfIdentity");
            Control hdfCAId = ucAttributes.FindControlRecursive("hdfCAId");
            Int32 caId = Convert.ToInt32(String.IsNullOrEmpty((hdfCAId as HiddenField).Value) ? AppConsts.NONE : Convert.ToInt32((hdfCAId as HiddenField).Value));

            //Get the previous list fetched from database
            List<TypeCustomAttributes> lstAllAttributes = new List<TypeCustomAttributes>();

            if (CurrentViewContext.lstTypeCustomAttributes != null)
                lstAllAttributes = CurrentViewContext.lstTypeCustomAttributes.ToList();

            //Get Custom Attibute
            TypeCustomAttributes typeCustomAttribute = lstAllAttributes.Where(x => x.CAId == caId).FirstOrDefault();

            if (lstCustomAttributeValues == null)
                lstCustomAttributeValues = new List<TypeCustomAttributes>();

            lstCustomAttributeValues.Add(new TypeCustomAttributes
            {
                CAId = caId,
                CAVId = String.IsNullOrEmpty((hdfAttribute as HiddenField).Value) ? AppConsts.NONE : Convert.ToInt32((hdfAttribute as HiddenField).Value),
                CAValue = attributeValue,
                CADataTypeCode = attributeTypeCode,
                CALabel = lblText,
                CAName = lblText,
                CAMId = String.IsNullOrEmpty((hdfIdentity as HiddenField).Value) ? AppConsts.NONE : Convert.ToInt32((hdfIdentity as HiddenField).Value)
            });
        }

        #endregion

        #region Public Methods

        public List<TypeCustomAttributes> GetCustomAttributeValues()
        {
            foreach (Control rowControl in pnlRows.Controls)
            {
                #region GET THE ROW LEVEL USER CONTROL

                if (rowControl.GetType().BaseType == typeof(CustomAttributeRowProfileControl))
                {
                    foreach (var attributeControlPanel in rowControl.Controls)
                    {
                        if (attributeControlPanel.GetType() == typeof(Panel))
                        {
                            foreach (var ctrl in (attributeControlPanel as Panel).Controls)
                            {
                                #region GET THE ATTRIBUTE LEVEL USER CONTROL

                                if (ctrl.GetType().BaseType == typeof(CustomAttributeProfileControl))
                                {
                                    HtmlContainerControl attrDiv = ((ctrl as CustomAttributeProfileControl).FindControl("divControlMode") as HtmlContainerControl);
                                    Label lblLabel = ((ctrl as CustomAttributeProfileControl).FindControl("lblLabel") as Label);

                                    String _labelText = lblLabel.IsNotNull() ? lblLabel.Text : "No Label";

                                    if (attrDiv.IsNotNull())
                                    {
                                        foreach (var ctrlType in attrDiv.Controls)
                                        {
                                            #region GET THE ACTUAL VALUES BASED ON THE CONTROL TYPE

                                            Type baseControlType = ctrlType.GetType();
                                            String attributeValue = String.Empty;

                                            if (baseControlType == typeof(WclTextBox))
                                            {
                                                attributeValue = (ctrlType as WclTextBox).Text;
                                                AddDataToList(ctrl, attributeValue, CustomAttributeDatatype.Text.GetStringValue(), _labelText);
                                            }
                                            else if (baseControlType == typeof(WclNumericTextBox))
                                            {
                                                attributeValue = (ctrlType as WclNumericTextBox).Text;
                                                AddDataToList(ctrl, attributeValue, CustomAttributeDatatype.Numeric.GetStringValue(), _labelText);
                                            }

                                            else if (baseControlType == typeof(WclDatePicker))
                                            {
                                                if ((ctrlType as WclDatePicker).SelectedDate == null)
                                                    attributeValue = String.Empty;
                                                else
                                                    attributeValue = Convert.ToDateTime((ctrlType as WclDatePicker).SelectedDate).ToShortDateString();

                                                AddDataToList(ctrl, attributeValue, CustomAttributeDatatype.Date.GetStringValue(), _labelText);
                                            }
                                            else if (baseControlType == typeof(RadioButtonList))
                                            {
                                                attributeValue = Convert.ToString((ctrlType as RadioButtonList).SelectedValue);
                                                AddDataToList(ctrl, attributeValue, CustomAttributeDatatype.Boolean.GetStringValue(), _labelText);
                                            }

                                            #endregion
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
                #endregion
            }
            return CurrentViewContext.lstCustomAttributeValues == null ?
                new List<TypeCustomAttributes>() : CurrentViewContext.lstCustomAttributeValues;
        }

        /// <summary>
        /// UAT 1438: Enhancement to allow students to select a User Group. 
        /// Returns the list of checked User Group IDs.
        /// </summary>
        /// <returns></returns>
        public List<Int32> GetUserGroupCustomAttributeValues()
        {
            List<Int32> lstUserGroupIDs = new List<Int32>();
            foreach (Control rowControl in pnlRows.Controls)
            {
                #region GET THE ROW LEVEL USER CONTROL

                if (rowControl.GetType().BaseType == typeof(CustomAttributeRowControl))
                {
                    foreach (var attributeControlPanel in rowControl.Controls)
                    {
                        if (attributeControlPanel.GetType() == typeof(Panel))
                        {
                            foreach (var ctrl in (attributeControlPanel as Panel).Controls)
                            {
                                #region GET THE ATTRIBUTE LEVEL USER CONTROL

                                if (ctrl.GetType().BaseType == typeof(CustomAttributeControl))
                                {
                                    HtmlContainerControl attrDiv = ((ctrl as CustomAttributeControl).FindControl("divControlMode") as HtmlContainerControl);
                                    Label lblLabel = ((ctrl as CustomAttributeControl).FindControl("lblLabel") as Label);

                                    String _labelText = lblLabel.IsNotNull() ? lblLabel.Text : "No Label";

                                    if (attrDiv.IsNotNull())
                                    {
                                        foreach (var ctrlType in attrDiv.Controls)
                                        {
                                            #region GET THE ACTUAL VALUES BASED ON THE CONTROL TYPE

                                            Type baseControlType = ctrlType.GetType();
                                            String attributeValue = String.Empty;

                                            //UAT 1438: Enhancement to allow students to select a User Group. 
                                            if (baseControlType == typeof(WclComboBox))
                                            {
                                                WclComboBox cmbUserGroup = ctrlType as WclComboBox;

                                                if (cmbUserGroup.CheckedItems.Count > AppConsts.NONE)
                                                {
                                                    foreach (var checkedItem in cmbUserGroup.CheckedItems)
                                                    {
                                                        //Add each checked item in property.
                                                        lstUserGroupIDs.Add(Convert.ToInt32(checkedItem.Value));
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
                #endregion
            }
            return lstUserGroupIDs;
        }

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties


        public CustomAttributeLoaderPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        /// <summary>
        /// Property to enable disable the controls
        /// </summary>
        public Boolean IsReadOnly { get; set; }

        /// <summary>
        /// Property to display the labels or actual controls
        /// </summary>
        public DisplayMode ControlDisplayMode { get; set; }

        public ICustomAttributeLoaderView CurrentViewContext
        {
            get { return this; }
        }

        public List<TypeCustomAttributes> lstTypeCustomAttributes
        {
            get;
            set;
        }

        public String TypeCode
        {
            get;
            set;
        }

        public Int32 MappingRecordId
        {
            get;
            set;
        }

        public Int32 ValueRecordId
        {
            get;
            set;
        }

        public Int32 TenantId
        {
            get;
            set;
        }

        public List<TypeCustomAttributes> lstCustomAttributeValues
        {
            get;
            set;
        }

        public Int32 CurrentLoggedInUserId
        {
            get;
            set;
        }

        public DataSourceMode DataSourceModeType
        {
            get;
            set;
        }

        public String Title
        {
            get;
            set;
        }

        /// <summary>
        /// Receive the attribute values and set to the controls, in Edit profile, in order flow. 
        /// To make sure that all properties are set again for controls, like Maxlength, Required field, when navigating back with data from external list 
        /// </summary>
        public List<TypeCustomAttributes> AttributeValues
        {
            get;
            set;
        }
        public String ValidationGroup
        {
            get;
            set;
        }

        /// <summary>
        /// To Store Department Program Package ID. Will be used to fetch DPM_ID to get custom attributes.
        /// </summary>
        public Int32? DPP_ID
        {
            get;
            set;
        }

        /// <summary>
        /// To Store Bkg Package Hierarchy Mapping ID. Will be used to fetch DPM_ID to get custom attributes.
        /// </summary>
        public Int32? BPHM_ID
        {
            get;
            set;
        }

        /// <summary>
        /// Will be used to check the custom attributes are fetched from order flow or any other screen.
        /// </summary>
        public Boolean IsOrder
        {
            get;
            set;
        }

        public Boolean IsFalsePostBack
        {
            get
            {
                return _isFalsePostBack;
            }
            set
            {
                _isFalsePostBack = value;
            }
        }

        public List<ClientSettingCustomAttributeContract> LstProfileCustomAttributeOverride
        {
            get;
            set;
        }

        #region  UAT 1438: Enhancement to allow students to select a User Group.

        /// <summary>
        /// True : This will generate the Usergroup combobox with each of the Hierarchy Node:
        /// False: This will hide the Usergroup combobox.
        /// </summary>
        public Boolean ShowUserGroupCustomAttribute
        {
            get;
            set;
        }


        /// <summary>
        /// Show Single UserGroup Dropdown. Instead of showing in Usergroup dropdown with each heirarchy node.
        /// </summary>
        public Boolean ShowUserGroupCustAttributeMerged
        {
            get;
            set;
        }

        /// <summary>
        /// Show the user group's in comma seperated string format.
        /// </summary>
        public Boolean ShowReadOnlyUserGroupCustomAttribute
        {
            get;
            set;
        }

        public IQueryable<UserGroup> lstUserGroups
        {
            get;
            set;
        }

        public IList<UserGroup> lstUserGroupsForUser
        {
            get;
            set;
        }
        #endregion

        #region Code Commented in UAT-3067
        ////UAT-2792
        //public String NetIDAttributeValue
        //{
        //    get;
        //    set;
        //}
        ////UAT-2883
        //public String StudentIdAttributeValue
        //{
        //    get;
        //    set;
        //}
        ////UAT-3067
        //public String PeopleSoftAttributeValue
        //{
        //    get;
        //    set;
        //}

        public String ShibbolethHandlerType
        {
            get;
            set;
        }
        #endregion

        public Dictionary<String, String> dictShibbolethCustomAttributes
        {
            get;
            set;
        }

        #region UAT-3430
        public Boolean IsNeedToHideCommandBar { get; set; }
        #endregion

        #region UAT-3455
        public Boolean IsMultipleValsSelected { get; set; }
        #endregion

        #endregion
        #endregion
    }
}

