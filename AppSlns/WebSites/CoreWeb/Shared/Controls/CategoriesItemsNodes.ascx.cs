using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.Shell.Views
{
    public partial class CategoriesItemsNodes : BaseUserControl, ICategoriesItemsNodes
    {
        #region Private Variables
        CategoriesItemsNodesPresenter _presenter = new CategoriesItemsNodesPresenter();

        #endregion

        #region [Properties]

        public List<NodesContract> ListofNodes
        {
            set;
            get;
        }

        public Int32 SelectedTenantId
        {
            get;
            set;
        }

        public int ComplianceCategoryId { get; set; }

        public int? ComplianceItemId { get; set; }

        public CategoriesItemsNodesPresenter Presenter
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

        public ICategoriesItemsNodes CurrentViewContext
        {
            get { return this; }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindListofNodes()
        {
            Presenter.GetListofNodes();
            if (CurrentViewContext.ListofNodes.Count != 0)
            {
                foreach (var item in CurrentViewContext.ListofNodes)
                {
                    if (item.DPM_ID != 0)
                    {
                        item.DPM_Label = item.DPM_Label + " (" + item.PackageName + ")";
                    }
                }
                rptNodes.DataSource = CurrentViewContext.ListofNodes;
                rptNodes.DataMember = "DPM_Label";
                rptNodes.DataBind();
            }            
        }
    }
}