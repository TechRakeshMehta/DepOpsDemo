using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Core;
using DAL;
using INTSOF.ServiceDataContracts.Modules.Common;
using System.Text;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class SharedUserCustomAttributeForm : BaseUserControl, ISharedUserCustomAttributeLoaderView
    {
        #region Variables

        #region Private Variables

        private SharedUserCustomAttributeFormPresenter _presenter = new SharedUserCustomAttributeFormPresenter();
        private Boolean _isFalsePostBack = false;
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties


        public SharedUserCustomAttributeFormPresenter Presenter
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

        public ISharedUserCustomAttributeLoaderView CurrentViewContext
        {
            get { return this; }
        }

        public List<CustomAttribteContract> lstTypeCustomAttributes
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

        public List<CustomAttribteContract> lstCustomAttributeValues
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
        public List<CustomAttribteContract> AttributeValues
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

        public Boolean ShowUserGroupCustomAttribute
        {
            get;
            set;
        }

        public String previousValues
        {
            get;
            set;
        }

        public Boolean IsSearchTypeControl
        {
            get;
            set;
        }

        public Boolean DoNotShowDefaultValues
        {
            get;
            set;
        }

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

            if (!CurrentViewContext.lstTypeCustomAttributes.IsNullOrEmpty() && !ShowUserGroupCustomAttribute)
            {
                //UAT 1438 : Removes the User Group Custom Attribute Type from the list i.e UserGroup Type custom attribute control will not be generated.
                CurrentViewContext.lstTypeCustomAttributes.RemoveAll(x => x.CustomAttributeDataTypeCode.ToLower().Trim() == CustomAttributeDatatype.User_Group.GetStringValue().ToLower().Trim());
            }

            SetCustomAttributeValues();

            // To manage the design of the control as per the Order confirmation screen
            if (this.ControlDisplayMode == DisplayMode.Controls || this.ControlDisplayMode == DisplayMode.ReadOnlyLabels)
            {
                //divForm.Attributes.Add("class", "sxform auto");
                //pnlRows.Attributes.Add("class", "sxpnl");

            }
            else
            {
                divSection.Attributes.Remove("class");
                litTitle.Text = String.Format("<h6><font style='color : #1c4d87;'>{0}</font></h6>", this.Title);
            }
        }



        #endregion

        #region Methods

        #region Private Methods

        public void SetCustomAttributeValues()
        {
            Int32 _attributesPerRow = 4;
            List<CustomAttribteContract> lstDistinctAttributes = new List<CustomAttribteContract>();

            if (CurrentViewContext.lstTypeCustomAttributes != null)
                lstDistinctAttributes = CurrentViewContext.lstTypeCustomAttributes.DistinctBy(x => x.CustomAttributeId).ToList();

            // List of current attributes to be added into the control
            List<CustomAttribteContract> lst = lstDistinctAttributes.Take(_attributesPerRow).ToList();

            // Maintains list of attributes that are already added
            List<Int32> lstTemporary = new List<Int32>();

            Int32 _rowsAdded = 0;
            for (int i = 1; i <= Math.Ceiling(Convert.ToDecimal(lstDistinctAttributes.Count()) / _attributesPerRow); i++)
            {
                if (_rowsAdded == _attributesPerRow)
                {
                    _rowsAdded = 0;
                }
                lst = new List<CustomAttribteContract>();
                //foreach (var tca in CurrentViewContext.lstTypeCustomAttributes)
                foreach (var tca in lstDistinctAttributes)
                {
                    if (!lstTemporary.Contains(tca.CustomAttributeId))
                        lst.Add(tca);
                }

                lst = lst.Take(_attributesPerRow).ToList();
                lstTemporary.AddRange(lst.Select(tca => tca.CustomAttributeId));
                LoadRowControl(lst);
                _rowsAdded += 1;
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
        private void LoadRowControl(List<CustomAttribteContract> lstAttributes)
        {
            SharedUserCustomAttributeDisplayRowControl customAttributeRow = (SharedUserCustomAttributeDisplayRowControl)Page.LoadControl("~\\ClinicalRotation\\UserControl\\SharedUserCustomAttributeDisplayRowControl.ascx");
            customAttributeRow.lstTypeCustomAttributes = lstAttributes;
            customAttributeRow.SelectedRecordId = this.ValueRecordId;
            customAttributeRow.IsReadOnly = this.IsReadOnly;
            customAttributeRow.ControlDisplayMode = this.ControlDisplayMode;
            customAttributeRow.DoNotShowDefaultValues = this.DoNotShowDefaultValues;

            if (!String.IsNullOrEmpty(this.ValidationGroup))
                customAttributeRow.ValidationGroup = this.ValidationGroup;

            if (this.AttributeValues.IsNotNull() && AttributeValues.Count() > 0)
                customAttributeRow.AttributeValues = this.AttributeValues;
            //UAT-1778
            customAttributeRow.previousValues = this.previousValues;
            customAttributeRow.IsSearchTypeControl = this.IsSearchTypeControl;
            pnlRows.Controls.Add(customAttributeRow);
        }

        /// <summary>
        /// Add data of the attributes into list, both add and update mode
        /// </summary>
        private void AddDataToList(object ctrl, String attributeValue, String attributeTypeCode, String lblText)
        {
            // Get the 'ApplicantComplianceAttributeId' while updating 
            Control ucAttributes = (ctrl as SharedUserCustomAttributeDisplayControl) as Control;
            Control hdfAttribute = ucAttributes.FindControlRecursive("hdfCAVId");
            Control hdfIdentity = ucAttributes.FindControlRecursive("hdfIdentity");
            Control hdfCAId = ucAttributes.FindControlRecursive("hdfCAId");
            Int32 caId = Convert.ToInt32(String.IsNullOrEmpty((hdfCAId as HiddenField).Value) ? AppConsts.NONE : Convert.ToInt32((hdfCAId as HiddenField).Value));

            //Get the previous list fetched from database
            List<CustomAttribteContract> lstAllAttributes = new List<CustomAttribteContract>();

            if (CurrentViewContext.lstTypeCustomAttributes != null)
                lstAllAttributes = CurrentViewContext.lstTypeCustomAttributes.ToList();

            //Get Custom Attibute
            if ((hdfIdentity as HiddenField).Value.IsNullOrEmpty())
            {
                (hdfIdentity as HiddenField).Value = AppConsts.ZERO;
            }
            CustomAttribteContract typeCustomAttribute = lstAllAttributes.Where(x => x.CustomAttributeId == caId && x.CustomAttrMappingId == Convert.ToInt32((hdfIdentity as HiddenField).Value)).FirstOrDefault();

            if (lstCustomAttributeValues == null)
                lstCustomAttributeValues = new List<CustomAttribteContract>();

            lstCustomAttributeValues.Add(new CustomAttribteContract
            {
                CustomAttributeId = caId,
                CustomAttrValueId = String.IsNullOrEmpty((hdfAttribute as HiddenField).Value) ? AppConsts.NONE : Convert.ToInt32((hdfAttribute as HiddenField).Value),
                CustomAttributeValue = attributeValue,
                CustomAttributeDataTypeCode = attributeTypeCode,
                CustomAttributeLabel = lblText,
                CustomAttributeName = lblText,
                CustomAttrMappingId = Convert.ToInt32((hdfIdentity as HiddenField).Value),
            });

            //Get Similar Custom Attibutes
            typeCustomAttribute = lstAllAttributes.Where(x => x.CustomAttributeId == caId && x.CustomAttrMappingId != Convert.ToInt32((hdfIdentity as HiddenField).Value)).FirstOrDefault();
            if (typeCustomAttribute.IsNotNull())
            {
                lstCustomAttributeValues.Add(new CustomAttribteContract
                {
                    CustomAttributeId = caId,
                    CustomAttrValueId = String.IsNullOrEmpty((hdfAttribute as HiddenField).Value) ? AppConsts.NONE : Convert.ToInt32((hdfAttribute as HiddenField).Value),
                    CustomAttributeValue = attributeValue,
                    CustomAttributeDataTypeCode = attributeTypeCode,
                    CustomAttributeLabel = lblText,
                    CustomAttributeName = lblText,
                    CustomAttrMappingId = Convert.ToInt32(typeCustomAttribute.CustomAttrMappingId),
                });
            }
        }

        #endregion

        #region Public Methods

        public String GetCustomDataXML()
        {
            List<CustomAttribteContract> customDataList = GetCustomAttributeValues();
            if (customDataList.Count > 0)
            {
                if (customDataList.Where(x => !x.CustomAttributeValue.IsNullOrEmpty() && (x.CustomAttributeValue != "0" )).Select(x => x.CustomAttributeValue).Count() > 0)
                {
                    StringBuilder customData = new StringBuilder();
                    customData.Append("<CustomAttributes>");
                    foreach (var item in customDataList.Where(x => !x.CustomAttributeValue.IsNullOrEmpty()).Select(x => x))
                    {
                        if (item.CustomAttributeValue != AppConsts.ZERO.ToString() && item.CustomAttributeValue.Trim() !="")
                        {
                            Session["IsCustomAttributeApplied"] = true;
                        }
                        customData.Append("<CustomAttribute>");
                        customData.Append("<CustomAttributeMappingID>" + item.CustomAttrMappingId + "</CustomAttributeMappingID>");
                        customData.Append("<AttributeValue>" + item.CustomAttributeValue + "</AttributeValue>");
                        customData.Append("<AttributeDataType>" + item.CustomAttributeDataTypeCode + "</AttributeDataType>");
                        customData.Append("<CustomAttributeID>" + item.CustomAttributeId + "</CustomAttributeID>");
                        customData.Append("<TenantID>" + CurrentViewContext.TenantId.ToString() + "</TenantID>");
                        customData.Append("</CustomAttribute>");
                    }
                    customData.Append("</CustomAttributes>");

                    return customData.ToString();
                }
            }
            return null;
        }

        public List<CustomAttribteContract> GetCustomAttributeValues()
        {
            foreach (Control rowControl in pnlRows.Controls)
            {
                #region GET THE ROW LEVEL USER CONTROL

                if (rowControl.GetType().BaseType == typeof(SharedUserCustomAttributeDisplayRowControl))
                {
                    foreach (var attributeControlPanel in rowControl.Controls)
                    {
                        if (attributeControlPanel.GetType() == typeof(Panel))
                        {
                            foreach (var ctrl in (attributeControlPanel as Panel).Controls)
                            {
                                #region GET THE ATTRIBUTE LEVEL USER CONTROL

                                if (ctrl.GetType().BaseType == typeof(SharedUserCustomAttributeDisplayControl))
                                {
                                    HtmlContainerControl attrDiv = ((ctrl as SharedUserCustomAttributeDisplayControl).FindControl("divControlMode") as HtmlContainerControl);
                                    Label lblLabel = ((ctrl as SharedUserCustomAttributeDisplayControl).FindControl("lblLabel") as Label);

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
                new List<CustomAttribteContract>() : CurrentViewContext.lstCustomAttributeValues;
        }

        //UAT-1778
        public void Reset(Int32? tenantId = null, Boolean resetTenant = true)
        {
            litTitle.Text = String.Empty;

            if (pnlRows.Controls.Count > 0)
            {
                pnlRows.Controls.Clear();
            }
            if (resetTenant)
            {

            }
        }

        /// <summary>
        /// UAT-1778: To Reset Custom Attributes
        /// </summary>
        public void ResetCustomAttributes()
        {
            foreach (var control in pnlRows.Controls)
            {
                //if (control.GetType().FullName == typeof(SharedUserCustomAttributeDisplayRowControl).FullName)
                if (control.GetType().FullName.ToLower().Contains("sharedusercustomattributedisplayrowcontrol"))
                {
                    ((SharedUserCustomAttributeDisplayRowControl)control).ResetCustomAttributes();
                }
            }
        }

        #endregion

        #endregion


    }
}