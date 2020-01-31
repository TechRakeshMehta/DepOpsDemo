using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
namespace CoreWeb.Search.Views
{
    public partial class SearchMain : System.Web.UI.UserControl, ISearchMainView
    {
        private SearchMainPresenter _presenter=new SearchMainPresenter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                ddlSearchType.DataSource = Presenter.LoadSearchType();
                ddlSearchType.DataTextField = "SearchTypeText";
                ddlSearchType.DataValueField = "SearchTypeValue";
                ddlSearchType.DataBind();
            }
            txtSearchText.Focus();
            this._presenter.OnViewLoaded();
        }

        
        public SearchMainPresenter Presenter
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


        protected void grdUserSearchResult_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            grdUserSearchResult.DataSource = Presenter.PerformSearch(); 
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
           
            grdUserSearchResult.Rebind();
        }

        /// <summary>
        /// Search Text</summary>
        /// <value>
        /// Gets or sets the value for Search Text.</value>
        public String SearchText
        {
            get
            {
                return txtSearchText.Text;
            }
            set
            {
                txtSearchText.Text = value;
            }
        }

        /// <summary>
        /// Search Type</summary>
        /// <value>
        /// Gets or sets the value for Search Type.</value>
        public String SearchType
        {
            get
            {
                return ddlSearchType.SelectedValue;
            }
            set
            {
                ddlSearchType.SelectedValue = value;
            }
        }
    }
}

