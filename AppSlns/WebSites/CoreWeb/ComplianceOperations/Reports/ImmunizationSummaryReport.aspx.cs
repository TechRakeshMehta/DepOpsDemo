using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Reflection;
using Business.RepoManagers;
using CoreWeb.Reports.Views;
using System.Web.UI;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ImmunizationSummaryReport : Page, IImmunizationSummaryReportView
    {
        private ImmunizationSummaryReportPresenter _presenter = new ImmunizationSummaryReportPresenter();

        //UAT-2780
        public Boolean IsApplicantLoggedIn
        {
            get
            {
                if (ViewState["IsApplicant"] != null)
                {
                    return Convert.ToBoolean(ViewState["IsApplicant"]);
                }
                return false;
            }

            set
            {
                ViewState["IsApplicant"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                ShowReport();
            }
            Presenter.OnViewLoaded();
            ssrsReport.ReportRefresh += new System.ComponentModel.CancelEventHandler(ssrsReport_ReportRefresh);
            ssrsReport.PreRender += new EventHandler(ssrsReport_PreRender);

            Title = "Compliance Tracking Summary Report";
        }

        void ssrsReport_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ShowReport();
        }

        private string _reportPath = @"RDLC\ImmunizationSummary.rdlc";

        protected void ShowReport()
        {
            SysXMembershipUser _user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            int tenantID;
            int subscriptionID;
            if (!Int32.TryParse(Request["tid"], out tenantID))
                return;
            if (!Int32.TryParse(Request["psid"], out subscriptionID))
                return;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("SubscriptionId", subscriptionID.ToString());
            parameters.Add("LoggedInUserId", _user.OrganizationUserId.ToString());
            parameters.Add("TenantID", _user.TenantId.ToString());
            ssrsReport.Visible = true;
            ssrsReport.Reset();
            ssrsReport.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            Microsoft.Reporting.WebForms.LocalReport localReport = ssrsReport.LocalReport;
            ssrsReport.LocalReport.ReportPath = Server.MapPath(_reportPath);
            DataSet verificationDs = ComplianceDataManager.Get(tenantID, "ImmunizationSummary", parameters);
            verificationDs.DataSetName = "DataSet1";
            Microsoft.Reporting.WebForms.ReportDataSource rds = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", verificationDs.Tables[0]);
            ssrsReport.LocalReport.DataSources.Add(rds);
            ssrsReport.LocalReport.Refresh();

            //UAT-2780: As an applicant, I should only be able to export my passport report in PDF
            IsApplicantLoggedIn = _user.IsApplicant;
        }

        //Method to disable the Export Format option.
        protected void ssrsReport_PreRender(object sender, EventArgs e)
        {
            List<String> lstFormatToDisable = new List<string>();
            lstFormatToDisable.Add("excel");
            lstFormatToDisable.Add("word");
            //DisableExportFormat(ssrsReport, lstFormatToDisable);

            if (IsApplicantLoggedIn)
            {
                DisableUnwantedExportFormat("PDF");
            }
        }

        /// <summary>
        /// Hidden the special SSRS rendering format in ReportViewer control
        /// </summary>
        /// <param name="ReportViewerID">The ID of the relevant ReportViewer control</param>
        /// <param name="strFormatName">Format Name</param>
        private void DisableExportFormat(CoreWeb.Reports.Views.ReportViewer ssrsReport, List<String> lstFormats)
        {
            //FieldInfo info;
            //foreach (RenderingExtension extension in ssrsReport.LocalReport.ListRenderingExtensions())
            //{
            //    if (lstFormats.Contains(extension.Name.ToLower()))
            //    {
            //        info = extension.GetType().GetField("m_isVisible", BindingFlags.Instance | BindingFlags.NonPublic);
            //        info.SetValue(extension, false);
            //    }
            //}
        }

        /// <summary>
        /// Hide Unwanted Export options from report
        /// UAT-2780: As an applicant, I should only be able to export my passport report in PDF
        /// </summary>
        /// <param name="strFormatName"></param>
        public void DisableUnwantedExportFormat(String requiredFormat)
        {
            FieldInfo info;
            foreach (RenderingExtension extension in ssrsReport.LocalReport.ListRenderingExtensions())
            {
                if (extension.Name != requiredFormat)
                {
                    info = extension.GetType().GetField("m_isVisible", BindingFlags.Instance | BindingFlags.NonPublic);
                    info.SetValue(extension, false);
                }
            }
        } 

        public ImmunizationSummaryReportPresenter Presenter
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
    }
}

