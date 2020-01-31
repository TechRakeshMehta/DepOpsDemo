using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class TrackingPackageDetailsPopUp : BaseUserControl, ITrackingPackageDetailsPopUpView
    {
        #region [Private Variables]

        private TrackingPackageDetailsPopUpPresenter _presenter = new TrackingPackageDetailsPopUpPresenter();
        private Int32 _tenantId;
        private ClinicalRotationDetailContract _viewContract = null;

        #endregion

        #region [Properties]

        public TrackingPackageDetailsPopUpPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public ITrackingPackageDetailsPopUpView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }

        public Int32 trackingPackageRequiredDOCURLId
        {
            get;
            set;
        }

        public List<CompliancePackage> lstPackagesNames
        {
            get;
            set;
        }

        public List<TrackingPackageRequiredContract> lstPackagesIDs
        {
            get;
            set;
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            Presenter.GetPackagesIDs();
            Presenter.GetPackagesNames();
            String lstPackagesID = lstPackagesIDs.Where(x => x.trackingPackageRequiredDOCURLId == trackingPackageRequiredDOCURLId).Select(x => x.PackageIds).FirstOrDefault();
            List<CompliancePackage> lstPackagesName = new List<CompliancePackage>();
            //String lstPackageId = lstPackagesID.Select(x => x.PackageIds).FirstOrDefault();
            foreach (String packageID in lstPackagesID.Split(','))
            {
                lstPackagesName.Add(lstPackagesNames.FirstOrDefault(x => x.CompliancePackageID == Convert.ToInt32(packageID)));
            }
            
            BindNameOfPackagesPopup(lstPackagesName);
        }

        private void BindNameOfPackagesPopup(List<CompliancePackage> lstPackagesName)
        {
            if (!lstPackagesName.IsNullOrEmpty())
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append(@"<table border='0' cellpadding='0' cellspacing='0'>");
                strBuilder.Append("<tr><th> Package Name </th><th> Package Label </th></tr>");

                lstPackagesName.ForEach(gen =>
                {
                    strBuilder.AppendFormat("<tr><td width='50%'>{0}</td><td width='50%'>{1}</td></tr>", "<li>" + gen.PackageName + "</li>", "<li>" + gen.PackageLabel + "</li>");
                });
                strBuilder.AppendFormat("</table>");

                divNameOfPackages.InnerHtml = strBuilder.ToString();
            }
            else
            {
                lblNameOfPackages.Text = "Package not found.";
                divNameOfPackages.Visible = false;
            }
        }
    }
}