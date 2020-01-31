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
using System.Text;
namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class NewCustomAttributeLoaderSearch : BaseUserControl, ICustomAttributeLoaderSearchView
    {
        #region Variables

        #region Private Variables

        private CustomAttributeLoaderSearchPresenter _presenter = new CustomAttributeLoaderSearchPresenter();

        #endregion

        #region Public Variables

        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
            hdnTenantId.Value = TenantId.ToString();
            lblinstituteHierarchy.Text = CurrentViewContext.nodeLable;
            if (!hdnInstitutionNodeId.Value.IsNullOrEmpty() && TenantId != 0)
            {
                Presenter.GetCustomAttributes(CurrentViewContext.DPM_ID, CustomAttributeUseTypeContext.Hierarchy.GetStringValue(), CurrentViewContext.TenantId);
                SetCustomAttributeValues();
            }
        }


        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	
        #region Methods
        #region Private Methods
        private void SetCustomAttributeValues()
        {
            Int32 _attributesPerRow = 3;

            if (CurrentViewContext.lstTypeCustomAttributes == null)
                CurrentViewContext.lstTypeCustomAttributes = new List<TypeCustomAttributesSearch>();

            // List of current attributes to be added into the control
            List<TypeCustomAttributesSearch> lst = null;

            // Maintains list of attributes that are already added
            List<Int32> lstTemporary = new List<Int32>();

            Int32 _rowsAdded = 0;
            for (int i = 1; i <= Math.Ceiling(Convert.ToDecimal(CurrentViewContext.lstTypeCustomAttributes.Count()) / _attributesPerRow); i++)
            {
                if (_rowsAdded == _attributesPerRow)
                {
                    _rowsAdded = 0;
                }
                lst = new List<TypeCustomAttributesSearch>();
                foreach (var tca in CurrentViewContext.lstTypeCustomAttributes)
                {
                    if (!lstTemporary.Contains(tca.CAMId))
                        lst.Add(tca);
                }

                lst = lst.Take(_attributesPerRow).ToList();
                lstTemporary.AddRange(lst.Select(tca => tca.CAMId));
                LoadRowControl(lst);
                _rowsAdded += 1;
            }

            if (CurrentViewContext.lstTypeCustomAttributes.Count() > 0)
            {
                this.divForm.Visible = true;
            }
            else
            {
                Reset(null, false);
                this.divForm.Visible = false;
            }
        }

        private void LoadRowControl(List<TypeCustomAttributesSearch> lstAttributes)
        {
            Control customAttributeRow = Page.LoadControl("~\\ComplianceAdministration\\UserControl\\NewCustomAttributeRowControlSearch.ascx");
            (customAttributeRow as NewCustomAttributeRowControlSearch).lstTypeCustomAttributes = lstAttributes;
            (customAttributeRow as NewCustomAttributeRowControlSearch).previousValues = CurrentViewContext.previousValues;
            //(customAttributeRow as CustomAttributeRowControlSearch).SelectedRecordId = this.ValueRecordId;
            pnlRows.Controls.Add(customAttributeRow);
        }

        /// <summary>
        /// Add data of the attributes into list, both add and update mode
        /// </summary>
        private void AddDataToList(object ctrl, String attributeValue, String attributeTypeCode, String lblText)
        {
            // Get the 'ApplicantComplianceAttributeId' while updating 
            Control ucAttributes = (ctrl as NewCustomAttributeControlSearch) as Control;
            Control hdfAttribute = ucAttributes.FindControlRecursive("hdfCAVId");
            Control hdfIdentity = ucAttributes.FindControlRecursive("lblHdfIdentity");
            if (lstCustomAttributeValues == null)
                lstCustomAttributeValues = new List<TypeCustomAttributesSearch>();

            lstCustomAttributeValues.Add(new TypeCustomAttributesSearch
            {
                CADataTypeCode = attributeTypeCode,
                CALabel = lblText,
                CAName = lblText,
                CAMId = Convert.ToInt32((hdfIdentity as Label).Text),
                Cvalue = attributeValue.ToString()
            });
        }

        #endregion

        #region Public Methods

        public String GetCustomDataXML()
        {
            List<TypeCustomAttributesSearch> customDataList = GetCustomAttributeValues();
            if (customDataList.Count > 0)
            {
                if (customDataList.Where(x => !x.Cvalue.IsNullOrEmpty()).Select(x => x.Cvalue).Count() > 0)
                {
                    StringBuilder customData = new StringBuilder();
                    customData.Append("<CustomAttributes>");
                    foreach (var item in customDataList.Where(x => !x.Cvalue.IsNullOrEmpty()).Select(x => x))
                    {
                        customData.Append("<CustomAttribute>");
                        customData.Append("<CustomAttributeMappingID>" + item.CAMId + "</CustomAttributeMappingID>");
                        customData.Append("<AttributeValue>" + item.Cvalue + "</AttributeValue>");
                        customData.Append("<AttributeDataType>" + item.CADataTypeCode + "</AttributeDataType>");
                        customData.Append("<HierarchyNodeID>" + hdnDepartmntPrgrmMppng.Value + "</HierarchyNodeID>");
                        customData.Append("</CustomAttribute>");
                    }
                    customData.Append("</CustomAttributes>");

                    return customData.ToString();
                }
            }
            return null;
        }

        public List<TypeCustomAttributesSearch> GetCustomAttributeValues()
        {
            foreach (Control rowControl in pnlRows.Controls)
            {
                #region GET THE ROW LEVEL USER CONTROL

                if (rowControl.GetType().BaseType == typeof(NewCustomAttributeRowControlSearch))
                {
                    foreach (var attributeControlPanel in rowControl.Controls)
                    {
                        if (attributeControlPanel.GetType() == typeof(Panel))
                        {
                            foreach (var ctrl in (attributeControlPanel as Panel).Controls)
                            {
                                #region GET THE ATTRIBUTE LEVEL USER CONTROL

                                if (ctrl.GetType().BaseType == typeof(NewCustomAttributeControlSearch))
                                {
                                    HtmlContainerControl attrDiv = ((ctrl as NewCustomAttributeControlSearch).FindControl("divControlMode") as HtmlContainerControl);
                                    Label lblLabel = ((ctrl as NewCustomAttributeControlSearch).FindControl("lblLabel") as Label);

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
                new List<TypeCustomAttributesSearch>() : CurrentViewContext.lstCustomAttributeValues;
        }
        #endregion
        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties


        public CustomAttributeLoaderSearchPresenter Presenter
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

        public ICustomAttributeLoaderSearchView CurrentViewContext
        {
            get { return this; }
        }

        public List<TypeCustomAttributesSearch> lstTypeCustomAttributes
        {
            get;
            set;
        }

        public String TypeCode
        {
            get;
            set;
        }

        public String previousValues
        {
            get;
            set;
        }

        public String nodeLable
        {
            get
            {
                return hdnHierarchyLabel.Value;
            }
            set
            {
                hdnHierarchyLabel.Value = value;
            }
        }

        public Int32 DPM_ID
        {
            get
            {
                return hdnDepartmntPrgrmMppng.Value.IsNullOrEmpty() ? 0 : Convert.ToInt32(hdnDepartmntPrgrmMppng.Value);
            }
            set
            {
                hdnDepartmntPrgrmMppng.Value = value.ToString();
            }
        }

        public Int32 NodeId
        {
            get
            {
                return hdnInstitutionNodeId.Value.IsNullOrEmpty() ? 0 : Convert.ToInt32(hdnInstitutionNodeId.Value);
            }
            set
            {
                hdnInstitutionNodeId.Value = value.ToString();
            }

        }

        public Int32 MappingRecordId
        {
            get
            {
                return Convert.ToInt32(hdnInstitutionNodeId.Value);
            }
            set
            {
                hdnInstitutionNodeId.Value = value.ToString();
            }
        }

        public Int32 ValueRecordId
        {
            get;
            set;
        }

        public Int32 TenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["TenantId"]);
            }
            set
            {
                ViewState["TenantId"] = value.ToString();
                hdnTenantId.Value = Convert.ToString(value);
                if (value == 0)
                    hdnTenantId.Value = String.Empty;

            }
        }

        public List<TypeCustomAttributesSearch> lstCustomAttributeValues
        {
            get;
            set;
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
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
        public List<TypeCustomAttributesSearch> AttributeValues
        {
            get;
            set;
        }
        public String ValidationGroup
        {
            get;
            set;
        }

        public Boolean IsCustomAttributesHide
        {
            set
            {
                if (value == true)
                    divForm.Attributes.Add("style", "display:none");
            }
        }

        #endregion

        #endregion

        #region Reset
        public void Reset(Int32? tenantId = null, Boolean resetTenant = true)
        {
            if (pnlRows.Controls.Count > 0)
            {
                pnlRows.Controls.Clear();
            }
            if (resetTenant)
            {
                hdnInstitutionNodeId.Value = String.Empty;
                lblinstituteHierarchy.Text = String.Empty;
                hdnHierarchyLabel.Value = String.Empty;
                hdnDepartmntPrgrmMppng.Value = String.Empty;
                if (Presenter.IsDefaultTenant() && tenantId == null)
                {
                    hdnTenantId.Value = String.Empty;
                    TenantId = 0;
                }
            }
        }
        #endregion

    }

}

