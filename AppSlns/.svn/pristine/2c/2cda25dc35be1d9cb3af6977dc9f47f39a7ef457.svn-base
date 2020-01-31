using CoreWeb.ClinicalRotation.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class RotationStudentDetailsPopup : BaseUserControl, IRotationStudentDetailsPopupView
    {
        #region [Private Variables]

        private RotationStudentDetailsPopupPresenter _presenter = new RotationStudentDetailsPopupPresenter();
        private Int32 _tenantId;
        private ClinicalRotationDetailContract _viewContract = null;

        #endregion

        #region [Properties]

        public RotationStudentDetailsPopupPresenter Presenter
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


        public IRotationStudentDetailsPopupView CurrentViewContext
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

        public Int32 clinicalRotationId
        {
            get;
            set;
        }

        public Int32 AgencyID
        {
            get;
            set;
        }

        public Int32 CurrentLoggedInUserId
        {
            get;
            set;
        }


        public List<RotationMemberDetailContract> lstRotationMemberDetail
        {
            get;
            set;
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Presenter.GetClinicalRotationMembersDetails();
            List<RotationMemberDetailContract> lstStudentNames = lstRotationMemberDetail.Where(x=>!x.IsInstructor).ToList();
            BindNumberOFStudentsPopup(lstStudentNames);
        }

        private void BindNumberOFStudentsPopup(List<RotationMemberDetailContract> lstStudentNames)
        {
            if (!lstStudentNames.IsNullOrEmpty())
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append(@"<table border='0' cellpadding='0' cellspacing='0'>");
                strBuilder.Append("<th> First Name </th> <th> Last Name</th>");

                lstStudentNames.ForEach(gen =>
                {
                    strBuilder.AppendFormat("<tr><td>{0}</td> <td>{1}</td></tr>", "<li>" + gen.RotationMemberDetail.ApplicantFirstName + "</li>", "<li>" + gen.RotationMemberDetail.ApplicantLastName + "</li>");
                });
                strBuilder.AppendFormat("</table>");
                divNumberOfStudents.InnerHtml = strBuilder.ToString();
            }
            else
            {
                lblNoStudents.Text = "No students assigned in this Rotation.";
                divNumberOfStudents.Visible = false;
            }
        }
    }
}