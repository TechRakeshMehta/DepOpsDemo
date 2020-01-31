using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.UI.Contract.BkgSetup;
using Business.RepoManagers;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ServiceFormMappingTemplate : BaseUserControl
    {
        public List<ServiceForm> lstServiceForm
        {
            set
            {
                cmbServiceForm.DataSource = value;
                cmbServiceForm.DataBind();
            }
        }

        public List<BackgroundServiceMapping> lstBackgroundServiceMapping
        {
            get
            {
                if (ViewState["lstServices"] != null)
                    return (List<BackgroundServiceMapping>)(ViewState["lstServices"]);
                return null;
            }
            set
            {
                cmbService.DataSource = value;
                cmbService.DataBind();
                ViewState["lstServices"] = value;
            }
        }

        public Int32 SelectedTenantID
        {
            get;
            set;
        }


        public Int32 SelectedServiceID
        {
            get
            {
                if (!cmbService.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbService.SelectedValue);
                return 0;
            }
            set
            {
                cmbService.SelectedValue = value.ToString();
            }

        }

        public Int32 SelectedServiceFormID
        {
            get
            {
                if (!cmbServiceForm.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt16(cmbServiceForm.SelectedValue);
                return 0;
            }
            set
            {
                cmbServiceForm.SelectedValue = value.ToString();
            }
        }

        public Int32? ExistingServiceID
        {
            get;
            set;
        }

        public Int32 DepartmentProgramMappingID
        {
            get
            {
                if (!hdnChildDepartmntPrgrmMppng.Value.IsNullOrEmpty())
                {
                    return Convert.ToInt32(hdnChildDepartmntPrgrmMppng.Value);
                }
                return 0;
            }
            set
            {
                hdnChildTenantId.Value = SelectedTenantID.ToString();
                hdnChildDepartmntPrgrmMppng.Value = value.ToString();
            }
        }

        public String InstitutionHierarchyLabel
        {
            set
            {
                lblChildinstituteHierarchy.Text = value.HtmlEncode();
                hdnChildHierarchyLabel.Value = value;
            }
        }

        public String HierarchyVldLabel
        {
            set
            {
                lblHierarchyVld.Text = value;
            }
        }

        public Boolean SelectedDispatchedType
        {
            get
            {
                if (!rblDispatchMode.SelectedValue.IsNullOrEmpty())
                    return Convert.ToBoolean(Convert.ToInt32(rblDispatchMode.SelectedValue));
                return false;
            }
            set
            {
                rblDispatchMode.SelectedValue = Convert.ToInt32(value).ToString();
            }
        }

        public Boolean IsManualByDefault
        {
            get
            {
                if (ViewState["IsManualByDefault"] != null)
                    return Convert.ToBoolean(ViewState["IsManualByDefault"]);
                return false;
            }
            set
            {
                ViewState["IsManualByDefault"] = value;
            }
        }


        protected void cmbServiceForm_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(cmbServiceForm.SelectedValue))
            {
                List<Int32> serviceIds = BackgroundSetupManager.GetServiceIdsByServiceForm(SecurityManager.DefaultTenantID, Convert.ToInt32(cmbServiceForm.SelectedValue));
                Entity.ServiceAttachedForm serviceAttachedForm = BackgroundSetupManager.GetServiceAttachedFormByID(SecurityManager.DefaultTenantID, Convert.ToInt32(cmbServiceForm.SelectedValue));
                if (!serviceAttachedForm.IsNullOrEmpty())
                {
                    divDispatchedMode.Visible = true;
                    SelectedDispatchedType = serviceAttachedForm.SF_SendAutomatically;
                    if (!serviceAttachedForm.SF_SendAutomatically)
                    {
                        rblDispatchMode.Enabled = false;
                        IsManualByDefault = true;
                    }
                    else
                    {
                        rblDispatchMode.Enabled = true;
                        IsManualByDefault = false;
                    }
                }
                //if (ExistingServiceID.HasValue)
                //    serviceIds.Remove(ExistingServiceID.Value);
                if (SelectedTenantID == 0)
                    lstBackgroundServiceMapping = lstBackgroundServiceMapping.Where(x => !serviceIds.Contains(x.BSE_ID)).ToList();
                //else if (SelectedTenantID > 0 && ExistingServiceID.HasValue)
                //    lstBackgroundServiceMapping = lstBackgroundServiceMapping.Where(x => !serviceIds.Contains(x.BSE_ID)).ToList();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(hdnChildHierarchyLabel.Value))
                lblChildinstituteHierarchy.Text = Convert.ToString(hdnChildHierarchyLabel.Value).HtmlEncode();
            else
                lblChildinstituteHierarchy.Text = String.Empty;
            //rblDispatchMode.Items.Clear();
            //Array lstValues = Enum.GetValues(typeof(DispatchType));
            //foreach (var value in lstValues)
            //{
            //    ListItem item = new ListItem(Enum.GetName(typeof(DispatchType), value), Convert.ToInt32(value).ToString());
            //    rblDispatchMode.Items.Add(item);
            //}
        }

        protected void cmbServiceForm_DataBound(object sender, EventArgs e)
        {
            cmbServiceForm.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbService_DataBound(object sender, EventArgs e)
        {
            cmbService.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }
    }
}