#region Namespaces

#region SystemDefined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

#region UserDefined
using INTSOF.Utils;
#endregion

#endregion

namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class AgencyHierarchySelection : BaseUserControl, IAgencyHierarchySelectionView
    {
        #region Variables

        #region Private Variables
        private AgencyHierarchySelectionPresenter _presenter = new AgencyHierarchySelectionPresenter();
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties
        public AgencyHierarchySelectionPresenter Presenter
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
        public IAgencyHierarchySelectionView CurrentViewContext
        {
            get
            {
                return this;
            }
        }
        public String Label
        {
            get
            {
                return String.IsNullOrEmpty(hdnHierarchyLabel.Value) ? String.Empty : hdnHierarchyLabel.Value;
            }

            set
            {
                hdnHierarchyLabel.Value = value;
            }
        }
        public Int32 AgencyId
        {
            get
            {
                return String.IsNullOrEmpty(hdnAgencyNodeId.Value) ? 0 : Convert.ToInt32(hdnAgencyNodeId.Value);
            }

            set
            {
                hdnAgencyNodeId.Value = value.ToString();
            }
        }
        public Int32 NodeId
        {
            get
            {
                return String.IsNullOrEmpty(hdnNodeId.Value) ? 0 : Convert.ToInt32(hdnNodeId.Value);
            }

            set
            {
                hdnNodeId.Value = value.ToString();
            }

        }
        public Int32 TenantId
        {
            get
            {
                return String.IsNullOrEmpty(hdnTenantId.Value) ? 0 : Convert.ToInt32(hdnTenantId.Value);
            }

            set
            {
                hdnTenantId.Value = value.ToString();
            }

        }
        public String AgencyName
        {

            get
            {
                return String.IsNullOrEmpty(hdnAgencyName.Value) ? String.Empty : Convert.ToString(hdnAgencyName.Value);
            }

            set
            {
                hdnAgencyName.Value = value.ToString();
            }

        }
        public Int32 SelectedRootNodeId
        {
            get
            {
                return String.IsNullOrEmpty(hdnselectedRootNodeId.Value) ? 0 : Convert.ToInt32(hdnselectedRootNodeId.Value);
            }

            set
            {
                hdnselectedRootNodeId.Value = value.ToString();
            }

        }
        public String SelectedInstitutionNodeIds
        {
            get
            {
                return String.IsNullOrEmpty(hdnInstitutionNodeIds.Value) ? String.Empty : Convert.ToString(hdnInstitutionNodeIds.Value);
            }

            set
            {
                hdnInstitutionNodeIds.Value = value.ToString();
            }
        }
        public Boolean IsInstitutionHierarchyRequired
        {
            get
            {
                return String.IsNullOrWhiteSpace(hdnIsInstitutionHierarchyRequired.Value) ? false : Convert.ToBoolean(hdnIsInstitutionHierarchyRequired.Value);
            }
            set
            {
                hdnIsInstitutionHierarchyRequired.Value = value.ToString();
            }
        }
        public Boolean IsReadOnlyMode
        {
            get
            {
                bool _isReadOnlyMode = false;

                if (!hdnReadOnlyMode.Value.IsNullOrEmpty())
                {
                    _isReadOnlyMode = Convert.ToBoolean(Convert.ToInt32(hdnReadOnlyMode.Value));
                }

                return _isReadOnlyMode;
            }
            set
            {
                hdnReadOnlyMode.Value = Convert.ToInt32(value).ToString();
            }
        }

        //UAT-3245
        public List<Int32> lstTenantId
        {
            get
            {
                return String.IsNullOrEmpty(hdnlstTenantIds.Value) ? new List<Int32>() : hdnlstTenantIds.Value.Split(',').Select(Int32.Parse).ToList();
            }
            set
            {
                hdnlstTenantIds.Value = String.Join(",", value);
            }

        }
        #endregion

        #endregion

        #region Events

        #region Page Events
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (AgencyName.IsNullOrEmpty()
                && NodeId > 0
                && AgencyId > 0
                && SelectedRootNodeId > 0)
            {
                lblAgencyHierarchy.Text = Presenter.GetAgencyHierarchyLabel();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblAgencyHierarchy.Text = CurrentViewContext.Label;
        }
        #endregion

        #endregion

        #region Methods

        #region Public
        /// <summary>
        /// Reset
        /// </summary>
        public void Reset()
        {
            hdnAgencyNodeId.Value = String.Empty;
            hdnHierarchyLabel.Value = String.Empty;
            hdnNodeId.Value = String.Empty;
            hdnAgencyName.Value = String.Empty;
            hdnInstitutionNodeIds.Value = String.Empty;
            hdnAgencyHierarchyNodeIds.Value = String.Empty;
            lblAgencyHierarchy.Text = String.Empty;
            hdnInstitutionNodeIds.Value = String.Empty;
            //hdnlstTenantIds.Value = String.Empty;
            hdnselectedRootNodeId.Value = String.Empty;
        }
        #endregion

        #endregion
    }
}