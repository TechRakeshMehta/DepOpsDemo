#region Namespaces

#region System Defined
using System;
using Microsoft.Practices.ObjectBuilder;
#endregion

#region Project Specific
#endregion
#endregion

namespace CoreWeb.ComplianceOperations.Views
{
	public partial class ComplianceInstitutionHierarchy : System.Web.UI.UserControl, IComplianceInstitutionHierarchyView
    {
        #region Variables

        #region Private Variables
        private ComplianceInstitutionHierarchyPresenter _presenter=new ComplianceInstitutionHierarchyPresenter();
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        public Int32 TenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["TenantId"]);
            }
            set
            {
                ViewState["TenantId"] = value.ToString();
            }
        }

        public String DepProgramMappingId
        {
            get
            {
                return hdnDepartmntPrgrmMppng.Value;
            }
            set
            {
                hdnDepartmntPrgrmMppng.Value = value;
            }
        }

        public String InstitutionNodeId
        {
            get
            {
                return hdnInstitutionNodeId.Value;
            }
            set
            {
                hdnInstitutionNodeId.Value = value;
            }
        }

        public String HierarchyLabel
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

        
        public ComplianceInstitutionHierarchyPresenter Presenter
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

        #endregion

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				Presenter.OnViewInitialized();
			}

            hdnTenantId.Value = TenantId.ToString();
            lblinstituteHierarchy.Text = hdnHierarchyLabel.Value;
            
			Presenter.OnViewLoaded();
        }

        #endregion

        #region Method
        #region private Methods
        #endregion

        #region public Methods

        /// <summary>
        /// Method to refresh the page.
        /// </summary>
        public void Refresh()
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "Refresh();", true);
        }
        #endregion
        #endregion

    }
}

