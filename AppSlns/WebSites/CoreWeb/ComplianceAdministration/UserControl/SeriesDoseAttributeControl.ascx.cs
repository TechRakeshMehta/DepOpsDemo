using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.ComplianceAdministration.Views;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class SeriesDoseAttributeControl : BaseUserControl, ISeriesDoseAttributeControlView
    {
        /// <summary>
        /// Attributes to be added in Combobox, for Items' list of attributes
        /// </summary>
        public List<SeriesAttributeContract> _lstAttributes
        {
            get;
            set;
        }

        /// <summary>
        /// Identify whether the control is at Series Level or Item level
        /// </summary>
        public Boolean IsSeriesLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the Key Attibute - TODO May not be required 
        /// </summary>
        public String KeyAttributeName
        {
            get;
            set;
        }

        /// <summary>
        /// ComplianceAttributeID to be set as Selected for the Items' Compliance Attribute Dropdown
        /// </summary>
        public Int32 SelectedAttributeId
        {
            get;
            set;
        }

        /// <summary>
        /// ItemSeriesAttributeID of the SeriesAttribute which is mapped to the ComplianceAttribute being iterated
        /// </summary>
        public Int32 ItemSeriesAttributeId
        {
            get;
            set;
        }

        private Int32 CmpItemId
        {
            get
            {
                return Convert.ToInt32(_lstAttributes.First().CmpItemId);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsSeriesLevel)
            {
                GenerateSeriesAttributeLabel();
            }
            else
            {
                GenerateAttributeCombo();
            }
        }

        /// <summary>
        /// Generate Text Literal for the Series Attribute.
        /// </summary>
        private void GenerateSeriesAttributeLabel()
        {
            Literal litKeyAttributeName = new Literal();
            litKeyAttributeName.Text = KeyAttributeName;
            pnlAttributeContainer.Controls.Add(litKeyAttributeName);


            //TextBox _ahiddenFieldISAId = new TextBox();
            //_ahiddenFieldISAId.Text = Convert.ToString(this.ItemSeriesAttributeId);
            //pnlAttributeContainer.Controls.Add(_ahiddenFieldISAId);
        }

        /// <summary>
        /// Generate Combobox for Item Attributes
        /// </summary>
        private void GenerateAttributeCombo()
        {
            //if (!_lstAttributes.Any(att => att.CmpAttributeId == 0))
            {
                _lstAttributes.Insert(AppConsts.NONE, new SeriesAttributeContract
                {
                    CmpAttributeName = AppConsts.COMBOBOX_ITEM_SELECT,
                    CmpAttributeId = AppConsts.NONE
                });
            }

            WclComboBox _combo = new WclComboBox();
            _combo.ID = "combo_" + this.ItemSeriesAttributeId;

            foreach (var attr in _lstAttributes)
            {
                _combo.Items.Add(new RadComboBoxItem
                {
                    Value = Convert.ToString(attr.CmpAttributeId),
                    Text = attr.CmpAttributeName
                });
            }

            _combo.SelectedValue = Convert.ToString(this.SelectedAttributeId);
            pnlAttributeContainer.Controls.Add(_combo);

            // Required to Save the Attribute and Item Mapping corresponding to the ItemSeriesAttributeID of the Series ComplianceAttribute
            // below which the Attribute dropdown is rendered
            //HiddenField _hiddenFieldISAId = new HiddenField();
            //_hiddenFieldISAId.ID = "hdn_" + this.ItemSeriesAttributeId;
            //_hiddenFieldISAId.Value = Convert.ToString(this.ItemSeriesAttributeId);
            //pnlAttributeContainer.Controls.Add(_hiddenFieldISAId);
        }
    }
}