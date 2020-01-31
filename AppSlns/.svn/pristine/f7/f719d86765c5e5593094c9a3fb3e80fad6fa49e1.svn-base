using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgOperations.Views
{
    public partial class DocsEntityMappingTemplate : BaseUserControl
    {

        public List<LookupContract> lstElements { get; set; }

        public List<LookupContract> BindCountryDropdown
        {
            set
            {
                cmbMapCountry.DataSource = value;
                cmbMapCountry.DataBind();
            }
        }
        public List<LookupContract> BindStateDropdown
        {
            set
            {
                cmbMapState.DataSource = value;
                cmbMapState.DataBind();
            }
        }
        public List<LookupContract> BindServiceDropdown
        {
            set
            {
                cmbMapService.DataSource = value;
                cmbMapService.DataBind();
            }
        }

        public List<LookupContract> BindRegulatoryEntityTypeDropdown
        {
            set
            {
                cmbMapRegulatoryEntity.DataSource = value;
                cmbMapRegulatoryEntity.DataBind();
            }
        }
        public List<LookupContract> BindDocumentsDropdown
        {
            set
            {
                cmbMapDRDocuments.DataSource = value;
                cmbMapDRDocuments.DataBind();
            }
        }
        public List<LookupContract> BindTenantsDropdown
        {
            set
            {
                cmbMapTenant.DataSource = value;
                cmbMapTenant.DataBind();
            }
        }

        public Int32 SelectedTenantID
        {
            get
            {
                if (!cmbMapTenant.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbMapTenant.SelectedValue);
                return 0;
            }
            set
            {
                cmbMapTenant.SelectedValue = value.ToString();
            }

        }

        public Int32 SelectedCountryID
        {
            get
            {
                if (!cmbMapCountry.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbMapCountry.SelectedValue);
                return 0;
            }
            set
            {
                cmbMapCountry.SelectedValue = value.ToString();
            }

        }
        public Int32 SelectedStateID
        {
            get
            {
                if (!cmbMapState.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbMapState.SelectedValue);
                return 0;
            }
            set
            {
                cmbMapState.SelectedValue = value.ToString();
            }

        }
        public Int32 SelectedServiceID
        {
            get
            {
                if (!cmbMapService.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbMapService.SelectedValue);
                return 0;
            }
            set
            {
                cmbMapService.SelectedValue = value.ToString();
            }

        }

        public Int16 SelectedRegulatoryEntityTypeID
        {
            get
            {
                if (!cmbMapRegulatoryEntity.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt16(cmbMapRegulatoryEntity.SelectedValue);
                return 0;
            }
            set
            {
                cmbMapRegulatoryEntity.SelectedValue = value.ToString();
            }
        }
        public Int32 SelectedDRDocumentID
        {
            get
            {
                if (!cmbMapDRDocuments.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbMapDRDocuments.SelectedValue);
                return 0;
            }
            set
            {
                cmbMapDRDocuments.SelectedValue = value.ToString();
            }
        }

        public Int32 DeptProgramMappingID
        {
            get
            {
                if (!hdnDepartmntPrgrmMppng.Value.IsNullOrEmpty())
                {
                    return Convert.ToInt32(hdnDepartmntPrgrmMppng.Value);
                }
                return 0;
            }
            set
            {
                hdnTenantId.Value = SelectedTenantID.ToString();
                hdnDepartmntPrgrmMppng.Value = value.ToString();
            }
        }

        public String InstitutionHierarchyLabel
        {
            set
            {
                lblinstituteHierarchy.Text = value;
            }
        }

        protected void cmbMapTenant_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            hdnTenantId.Value = SelectedTenantID.ToString();
            hdnDepartmntPrgrmMppng.Value = String.Empty;
            lblinstituteHierarchy.Text = String.Empty;
        }

        protected void cmbMapCountry_DataBound(object sender, EventArgs e)
        {
            cmbMapCountry.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbMapDRDocuments_DataBound(object sender, EventArgs e)
        {
            cmbMapDRDocuments.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbMapState_DataBound(object sender, EventArgs e)
        {
            cmbMapState.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbMapService_DataBound(object sender, EventArgs e)
        {
            cmbMapService.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbMapTenant_DataBound(object sender, EventArgs e)
        {
            cmbMapTenant.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbMapRegulatoryEntity_DataBound(object sender, EventArgs e)
        {
            cmbMapRegulatoryEntity.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

    }
}