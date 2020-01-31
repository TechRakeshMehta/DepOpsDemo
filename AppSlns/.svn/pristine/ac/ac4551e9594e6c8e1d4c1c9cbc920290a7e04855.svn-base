using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class InstitutionNodes : BaseUserControl, IInstitutionNodesView
    {
        private InstitutionNodesPresenter _presenter = new InstitutionNodesPresenter();

        public InstitutionNodesPresenter Presenter
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

        Int32? IInstitutionNodesView.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        public Int32 TenantId
        {
            get;
            set;
        }

        public IInstitutionNodesView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Dictionary<Int32, String> IInstitutionNodesView.dicNodes
        {
            set
            {
                cmbNodes.Items.Add(new RadComboBoxItem
                       {
                           Text = AppConsts.COMBOBOX_ITEM_SELECT,
                           Value = AppConsts.ZERO
                       });

                foreach (var node in value)
                {
                    cmbNodes.Items.Add(new RadComboBoxItem
                    {
                        Text = node.Value,
                        Value = Convert.ToString(node.Key)
                    });
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack )
            {
                Presenter.GetInstitutionNodes();
            }
        }
    }
}