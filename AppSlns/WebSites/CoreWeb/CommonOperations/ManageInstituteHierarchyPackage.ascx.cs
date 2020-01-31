#region Namespaces

#region System Defined
using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Collections;
using System.Linq;

#endregion

#region UserDefined
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using Newtonsoft.Json;
#endregion
#endregion

namespace CoreWeb.CommonOperations.Views
{
    public partial class ManageInstituteHierarchyPackage : BaseUserControl, IManageInstituteHierarchyPackageView
    {
        #region Variables

        #region Private Variables
        private ManageInstituteHierarchyPackagePresenter _presenter = new ManageInstituteHierarchyPackagePresenter();
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties
        public IManageInstituteHierarchyPackageView CurrentViewContext
        {
            get
            {
                return this;
            }
        }
        public ManageInstituteHierarchyPackagePresenter Presenter
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
        public Int32 TenantID
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
        public String CompliancePackageTypeCode
        {
            get
            {
                return String.IsNullOrEmpty(hdnCompliancePackageTypeCode.Value) ? String.Empty : hdnCompliancePackageTypeCode.Value;
            }
            set
            {
                hdnCompliancePackageTypeCode.Value = value.ToString();
            }

        }


        public Boolean IsCompliancePackage
        {
            get
            {
                return (hdnIsCompliancePackage.Value.IsNullOrEmpty()) ? false : Convert.ToBoolean(hdnIsCompliancePackage.Value);
            }
            set
            {
                hdnIsCompliancePackage.Value = value.ToString();
            }
        }
        public String PackageNodeMappingID
        {
            get
            {
                return (hdnPackageNodeMappingID.Value.IsNullOrEmpty()) ? String.Empty : (hdnPackageNodeMappingID.Value);
            }
            set
            {
                hdnPackageNodeMappingID.Value = value.ToString();
            }
        }

        public String PackageName
        {
            get
            {
                return (hdnPackageName.Value.IsNullOrEmpty()) ? String.Empty : (hdnPackageName.Value);
            }
            set
            {
                hdnPackageName.Value = value.ToString();
            }
        }
        public String PackageId
        {
            get
            {
                return (hdnPackageId.Value.IsNullOrEmpty()) ? String.Empty : Convert.ToString(hdnPackageId.Value);
            }
            set
            {
                hdnPackageId.Value = value.ToString();
            }
        }

        public String InstitutionHierarchyNodeID
        {
            get
            {
                return (hdnInstitutionHierarchyNodeID.Value.IsNullOrEmpty()) ? String.Empty : Convert.ToString(hdnInstitutionHierarchyNodeID.Value);
            }
            set
            {
                hdnInstitutionHierarchyNodeID.Value = value.ToString();
            }
        }

        public String DispalyText
        {
            get
            {
                return (lblDisplayText.IsNullOrEmpty()) ? String.Empty : (lblDisplayText.Text);
            }
            set
            {
                lblDisplayText.Text = value;
            }
        }
        //public String DeptProgramMappingID
        //{
        //    get
        //    {
        //        return (hdnDeptProgramMappingID.Value.IsNullOrEmpty()) ? String.Empty : Convert.ToString(hdnDeptProgramMappingID.Value);
        //    }
        //    set
        //    {
        //        hdnDeptProgramMappingID.Value = value.ToString();
        //    }
        //}

        public String InstituteHierarchyPackageName
        {
            get
            {
                return (lblInstituteHierarchyPackage.IsNullOrEmpty()) ? String.Empty : (lblInstituteHierarchyPackage.Text);
            }
            set
            {
                lblInstituteHierarchyPackage.Text = value;
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
            lblInstituteHierarchyPackage.Text = PackageName;
        }
        #endregion 

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #endregion

    }
}