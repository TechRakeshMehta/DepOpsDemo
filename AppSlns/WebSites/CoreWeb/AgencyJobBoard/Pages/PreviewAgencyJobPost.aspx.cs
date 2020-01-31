using CoreWeb.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.AgencyJobBoard;
using INTERSOFT.WEB.UI.WebControls;
using Telerik.Web.UI;
using System.Web.Configuration;
using System.IO;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using CoreWeb.ClinicalRotation.Views;

namespace CoreWeb.AgencyJobBoard.Pages
{
    public partial class PreviewAgencyJobPost : BaseWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                try
                {
                    Control ucAgencyJobPost = Page.LoadControl(AppConsts.View_AGENCY_JOB_POST_FOR_APPLICANT_CONTROL);
                    AgencyJobContract AgencyJob = Session["PreviewAgencyJobData"] as AgencyJobContract;
                    (ucAgencyJobPost as CoreWeb.AgencyJobBoard.Views.IViewAgencyJobPostView).AgencyJobDetail = AgencyJob;
                    (ucAgencyJobPost as CoreWeb.AgencyJobBoard.Views.IViewAgencyJobPostView).IsPreviewMode = true;
                    pnlPreviewJobPost.Controls.Add(ucAgencyJobPost);
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                    base.ShowErrorMessage(ex.Message);
                }
                catch (System.Exception ex)
                {
                    base.LogError(ex);
                    base.ShowErrorMessage(ex.Message);
                }
            }
        }
    }
}