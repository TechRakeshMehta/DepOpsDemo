using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.Utils;
using Entity.ClientEntity;
using System.Collections.Generic;
using INTERSOFT.WEB.UI.WebControls;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class CustomAttributeControlSearchMultipleNodes : BaseUserControl, ICustomAttributeControlSearchMultipleNodesView
    {
        private CustomAttributeControlSearchMultipleNodesPresenter _presenter = new CustomAttributeControlSearchMultipleNodesPresenter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            CreateDynamicControls();
            Presenter.OnViewLoaded();
        }


        public CustomAttributeControlSearchMultipleNodesPresenter Presenter
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

        public TypeCustomAttributesSearch TypeCustomtAttribute
        {
            get;
            set;
        }

        public ICustomAttributeControlSearchMultipleNodesView CurrentViewContext
        {
            get { return this; }
        }

        /// <summary>
        /// Receive the attribute values and set to the controls, in Edit profile, in order flow
        /// </summary>
        public List<TypeCustomAttributesSearch> AttributeValues
        {
            get;
            set;
        }

        public String previousValues
        {
            get;
            set;
        }

        public Boolean IsReadOnly { get; set; }
        #region   Methods

        #region Public Methods

        public void ResetCustomAttribute()
        {

            //Reset all types of server controls
            String dataTypeCode = CurrentViewContext.TypeCustomtAttribute.CADataTypeCode.ToLower().Trim();
            Int32 _customAttrId = CurrentViewContext.TypeCustomtAttribute.CAMId;
            if (dataTypeCode == CustomAttributeDatatype.Date.GetStringValue().ToLower().Trim())
            {
                var dp = divControlMode.FindServerControlRecursively("dp_" + _customAttrId) as WclDatePicker;
                if (dp.IsNotNull())
                    dp.SelectedDate = null;
            }
            else if (dataTypeCode == CustomAttributeDatatype.Text.GetStringValue().ToLower().Trim())
            {
                var txtTextType = divControlMode.FindServerControlRecursively("txtTextType_" + _customAttrId) as WclTextBox;
                if (txtTextType.IsNotNull())
                    txtTextType.Text = String.Empty;
            }
            else if (dataTypeCode == CustomAttributeDatatype.Numeric.GetStringValue().ToLower().Trim())
            {
                var txtNumericType = divControlMode.FindServerControlRecursively("txtNumericType_" + _customAttrId) as WclNumericTextBox;
                if (txtNumericType.IsNotNull())
                    txtNumericType.Text = String.Empty;
            }
            else if (dataTypeCode == CustomAttributeDatatype.Boolean.GetStringValue().ToLower().Trim())
            {
                var rbtnListType = divControlMode.FindServerControlRecursively("rbtnListType_" + _customAttrId) as RadioButtonList;
                if (rbtnListType.IsNotNull())
                    rbtnListType.SelectedValue = "0";
            }
        }

        #endregion
        #region Private Methods

        /// <summary>
        /// Generate the dynamic attribute control
        /// </summary>
        private void CreateDynamicControls()
        {
            divControlMode.Controls.Clear();
            String value = String.Empty;
            String dataTypeCode = CurrentViewContext.TypeCustomtAttribute.CADataTypeCode.ToLower().Trim();
            Int64 _mappingId = CurrentViewContext.TypeCustomtAttribute.CAMId;
            if (!CurrentViewContext.previousValues.IsNullOrEmpty())
            {
                value = GetValue(_mappingId);
            }

            String _caType = lblLabel.Text = String.IsNullOrEmpty(CurrentViewContext.TypeCustomtAttribute.CALabel)
             ? CurrentViewContext.TypeCustomtAttribute.CAName : CurrentViewContext.TypeCustomtAttribute.CALabel;


            lblHdfIdentity.Text = Convert.ToString(CurrentViewContext.TypeCustomtAttribute.CAMId);
            hdfIdentity.Value = Convert.ToString(CurrentViewContext.TypeCustomtAttribute.CAMId);

            if (AttributeValues.IsNotNull() && AttributeValues.Count > 0)
            {
                TypeCustomAttributesSearch typeCustomAttributes = AttributeValues.Where(av => av.CAMId == _mappingId).FirstOrDefault();

            }

            if (dataTypeCode == CustomAttributeDatatype.Date.GetStringValue().ToLower().Trim())
            {
                WclDatePicker dPicker = new WclDatePicker();
                dPicker.DateInput.DateFormat = "MM-dd-yyyy";
                dPicker.ID = "dp_" + _mappingId;
                dPicker.DateInput.EmptyMessage = "Select a date";
                dPicker.MinDate = Convert.ToDateTime("01-01-1900");
                dPicker.MaxDate = DateTime.Now;
                dPicker.Enabled = !this.IsReadOnly;
                dPicker.ClientEvents.OnPopupClosing = "OnCalenderClosing";
                dPicker.EnableAriaSupport = true;
                dPicker.DateInput.SelectionOnFocus = Telerik.Web.UI.SelectionOnFocus.CaretToBeginning;
                dPicker.Calendar.EnableKeyboardNavigation = true;
                if (!value.IsNullOrEmpty())
                {
                    dPicker.SelectedDate = Convert.ToDateTime(value);
                }
                dPicker.CssClass = "form-control";
                dPicker.Width = new Unit("100%");
                divControlMode.Controls.Add(dPicker);

            }
            else if (dataTypeCode == CustomAttributeDatatype.Text.GetStringValue().ToLower().Trim())
            {
                WclTextBox txtTextType = new WclTextBox();
                txtTextType.ID = "txtTextType_" + _mappingId;
                txtTextType.Enabled = !this.IsReadOnly;
                if (CurrentViewContext.TypeCustomtAttribute.MaxLength.IsNull() || CurrentViewContext.TypeCustomtAttribute.MaxLength <= 0)
                    txtTextType.MaxLength = 50;
                else
                    txtTextType.MaxLength = Convert.ToInt32(CurrentViewContext.TypeCustomtAttribute.MaxLength);

                if (!value.IsNullOrEmpty())
                {
                    txtTextType.Text = value;
                }
                txtTextType.CssClass = "form-control";
                txtTextType.Width = new Unit("100%");
                divControlMode.Controls.Add(txtTextType);

            }
            else if (dataTypeCode == CustomAttributeDatatype.Numeric.GetStringValue().ToLower().Trim())
            {
                WclNumericTextBox txtNumeric = new WclNumericTextBox();
                txtNumeric.ID = "txtNumericType_" + _mappingId;
                txtNumeric.Enabled = !this.IsReadOnly;
                if (!value.IsNullOrEmpty())
                {
                    txtNumeric.Text = value;
                }
                txtNumeric.CssClass = "form-control";
                txtNumeric.Width = new Unit("100%");
                divControlMode.Controls.Add(txtNumeric);
            }

            else if (dataTypeCode == CustomAttributeDatatype.Boolean.GetStringValue().ToLower().Trim())
            {
                RadioButtonList rbtnList = new RadioButtonList(); rbtnList.RepeatDirection = RepeatDirection.Horizontal;
                rbtnList.Items.Add(new ListItem { Text = "Yes", Value = "1" });
                rbtnList.Items.Add(new ListItem { Text = "No", Value = "2" });
                rbtnList.Items.Add(new ListItem { Text = "NA", Value = "0" });
                rbtnList.ID = "rbtnListType_" + _mappingId;
                rbtnList.Enabled = !this.IsReadOnly;
                if (!value.IsNullOrEmpty())
                {
                    rbtnList.SelectedValue = value;

                }
                rbtnList.CssClass = "form-control";
                rbtnList.Width = new Unit("100%");
                divControlMode.Controls.Add(rbtnList);
            }

        }

        private String GetValue(Int64 mappingId)
        {
            XmlDataSource xml = new XmlDataSource();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(CurrentViewContext.previousValues);
            XmlNodeList nodes = doc.SelectNodes("CustomAttributes/CustomAttribute");
            foreach (XmlNode node in nodes)
            {

                if (node.ChildNodes[0].ChildNodes[0].Value.ToString() == mappingId.ToString())
                {
                    return node.ChildNodes[1].ChildNodes[0].Value.ToString();
                }
            }
            return String.Empty;
        }
        #endregion


        #endregion
        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	

    }
}

