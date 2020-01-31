using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class RequirementVerificationDetail : BaseUserControl, IRequirementVerificationDetail
    {
        #region Variables

        private RequirementVerificationDetailPresenter _presenter = new RequirementVerificationDetailPresenter();

        #endregion;

        #region Properties

        public RequirementVerificationDetailPresenter Presenter
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

        /// <summary>
        /// Selected ApplicantId
        /// </summary>
        Int32 IRequirementVerificationDetail.SelectedApplicantId
        {
            get;
            set;
        }

        /// <summary>
        /// Selected TenantId
        /// </summary>
        Int32 IRequirementVerificationDetail.SelectedTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Selected RotationId
        /// </summary>
        Int32 IRequirementVerificationDetail.ClinicalRotationId
        {
            get;
            set;
        }

        /// <summary>
        /// RequiremnetPackageSubscriptionID i.e. RPS_ID
        /// </summary>
        Int32 IRequirementVerificationDetail.RPSId
        {
            get;
            set;
        }

        /// <summary>
        /// Data of the Applicant.
        /// </summary>
        OrganizationUserContract IRequirementVerificationDetail.ApplicantData
        {
            get;
            set;
        }


        /// <summary>
        /// List for 'lkpRequirementItemStatus' entity
        /// </summary>
        List<RequirementItemStatusContract> IRequirementVerificationDetail.lstReqItemStatusTypes
        {
            get;
            set;
        }

        public IRequirementVerificationDetail CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Complete data of the Verification details screen, including the data entered by applicant
        /// </summary>
        List<RequirementVerificationDetailContract> IRequirementVerificationDetail.lstVerificationDetailData
        {
            get;
            set;
        }

        String IRequirementVerificationDetail.CategoryControlIdPrefix
        {
            get
            {
                return "ucCategoryControl_";
            }
        }

        /// <summary>
        /// Contains the complete data of the verification details, which needs to be saved/updated
        /// </summary>
        RequirementVerificationData IRequirementVerificationDetail.DataToSave
        {
            get;
            set;
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
        }

        private String ControlUseType { get; set; }
        #endregion

        #region Events

        /// <summary>
        /// OnInit event to set page titles
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.SetPageTitle("Requirement Verification detail");
                base.Title = "Requirement Verification detail";
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
        protected void Page_Load(object sender, EventArgs e)
        {
            CaptureQueryString();
            if (!IsPostBack)
            {
                (ucRotationDetails as CoreWeb.CommonControls.Views.IRotationDetails).TenantId = CurrentViewContext.SelectedTenantId;
                (ucRotationDetails as CoreWeb.CommonControls.Views.IRotationDetails).ClinicalRotationId = CurrentViewContext.ClinicalRotationId;
                BindApplicantData();
                hdfNotApproved.Value = RequirementItemStatus.NOT_APPROVED.GetStringValue();
                hdfIncomplete.Value = RequirementItemStatus.INCOMPLETE.GetStringValue();
            }
            BindVerificationDetailData();
            HideControls();
        }

        /// <summary>
        /// Save and Move to next Record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdBar_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                SaveData();
                var _selectedData = SysXWebSiteUtils.SessionService.GetCustomData("QueueData") as List<RFQSelectedDataContract>;
                var _nextRecord = _selectedData.SkipWhile(rfq => rfq.RPSId != CurrentViewContext.RPSId).Skip(1).FirstOrDefault();

                if (_nextRecord.IsNotNull())
                {
                    var queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.SelectedTenantId) },
                                                                    { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL_CONTROL},
                                                                    { ProfileSharingQryString.ReqPkgSubscriptionId,Convert.ToString( _nextRecord.RPSId )},
                                                                    { ProfileSharingQryString.RotationId,Convert.ToString( _nextRecord.RotationId) }, 
                                                                    { ProfileSharingQryString.ApplicantId ,  Convert.ToString(_nextRecord.OrganizationUserId ) }
                                                                 };
                    var url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }
                else
                {
                    RedirectToQueue();
                }
            }
            catch (Exception ex)
            {
                base.ShowInfoMessage("Data could not be saved.");
                base.LogError(ex);
            }
        }

        /// <summary>
        /// Save and Return to Queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdBar_SaveClick(object sender, EventArgs e)
        {
            try
            {
                SaveData();
                RedirectToQueue();
            }
            catch (Exception ex)
            {
                base.ShowInfoMessage("Data could not be saved.");
                base.LogError(ex);
            }
        }

        /// <summary>
        /// Cancel and return to Queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdBar_CancelClick(object sender, EventArgs e)
        {
            if (ControlUseType == AppConsts.ROTATION_MEMBER_SEARCH_USE_TYPE_CODE.ToLower().Trim())
            {
                ReturnToRotationMemberSearchQueue();
            }
            else
            {
                RedirectToQueue();
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// Get Query string values
        /// </summary>
        private void CaptureQueryString()
        {
            var args = new Dictionary<String, String>();

            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);

                if (args.ContainsKey(ProfileSharingQryString.SelectedTenantId))
                {
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(args[ProfileSharingQryString.SelectedTenantId]);
                }

                if (args.ContainsKey(ProfileSharingQryString.RotationId))
                {
                    CurrentViewContext.ClinicalRotationId = Convert.ToInt32(args[ProfileSharingQryString.RotationId]);
                }
                if (args.ContainsKey(ProfileSharingQryString.ApplicantId))
                {
                    CurrentViewContext.SelectedApplicantId = Convert.ToInt32(args[ProfileSharingQryString.ApplicantId]);
                }
                if (args.ContainsKey(ProfileSharingQryString.ReqPkgSubscriptionId))
                {
                    CurrentViewContext.RPSId = Convert.ToInt32(args[ProfileSharingQryString.ReqPkgSubscriptionId]);
                }
                if (args.ContainsKey(ProfileSharingQryString.ControlUseType))
                {
                    ControlUseType = args[ProfileSharingQryString.ControlUseType].ToLower().Trim();
                }
            }
        }

        /// <summary>
        /// Set the applicant specific data
        /// </summary>
        private void BindApplicantData()
        {
            Presenter.GetApplicantData();
            if (CurrentViewContext.ApplicantData.MiddleName.IsNullOrEmpty())
            {
                txtApplicantName.Text = CurrentViewContext.ApplicantData.FirstName + " " + CurrentViewContext.ApplicantData.LastName;
            }
            else
            {
                txtApplicantName.Text = CurrentViewContext.ApplicantData.FirstName + " " + CurrentViewContext.ApplicantData.MiddleName + " " + CurrentViewContext.ApplicantData.LastName;
            }

            txtApplicantUserName.Text = CurrentViewContext.ApplicantData.UserName;

            if (CurrentViewContext.ApplicantData.Address1.IsNotNull())
            {

                txtAddress.Text = CurrentViewContext.ApplicantData.Address1
                                          + (String.IsNullOrEmpty(CurrentViewContext.ApplicantData.Address2) ? String.Empty : ", " + CurrentViewContext.ApplicantData.Address2)
                                          + (String.IsNullOrEmpty(CurrentViewContext.ApplicantData.County) ? String.Empty : ", " + CurrentViewContext.ApplicantData.County);

                txtAddress.Text += ", " + CurrentViewContext.ApplicantData.City;
                txtAddress.Text += ", " + CurrentViewContext.ApplicantData.State;
                txtAddress.Text += ", " + CurrentViewContext.ApplicantData.Country;
                txtAddress.Text += " " + CurrentViewContext.ApplicantData.ZipCode;
            }
            else
            {
                txtAddress.Text = "NA";
            }
            txtDOB.Text = CurrentViewContext.ApplicantData.DateOfBirth.IsNullOrEmpty()
                        ? "NA"
                        : CurrentViewContext.ApplicantData.DateOfBirth.Value.ToString("MM/dd/yyyy");

            txtEmail.Text = CurrentViewContext.ApplicantData.Email;
            txtPhoneNo.Text = CurrentViewContext.ApplicantData.Phone;
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindVerificationDetailData()
        {
            Presenter.GetVerificationDetailData();
            Presenter.GetReqItemStatusTypes();
            lblComplianceStatus.Text = CurrentViewContext.lstVerificationDetailData.First().PkgStatusName;

            lblComplianceStatus.Font.Bold = true;
            lblComplianceStatus.Font.Size = new FontUnit(11);

            var _pkgData = CurrentViewContext.lstVerificationDetailData.First();

            if (_pkgData.PkgStatusCode == RequirementPackageStatus.REQUIREMENT_NOT_COMPLIANT.GetStringValue())
            {
                imgOverallComplianceStatus.ImageUrl = AppConsts.PACKAGE_NON_COMPLIANCE_IMAGE_URL;
                lblComplianceStatus.ForeColor = System.Drawing.Color.Red;
                imgOverallComplianceStatus.ToolTip = lblComplianceStatus.Text = _pkgData.PkgStatusName;
            }
            else
            {
                imgOverallComplianceStatus.ImageUrl = AppConsts.PACKAGE_COMPLIANCE_IMAGE_URL;
                lblComplianceStatus.ForeColor = System.Drawing.Color.Green;
                imgOverallComplianceStatus.ToolTip = lblComplianceStatus.Text = _pkgData.PkgStatusName;
            }

            GenerateCategoryControls();
        }

        /// <summary>
        /// Generate category level controls
        /// </summary>
        private void GenerateCategoryControls()
        {
            var _distinctCategories = CurrentViewContext.lstVerificationDetailData.OrderBy(vdd => vdd.CatId).Select(vdd => vdd.CatId).Distinct().ToList();

            foreach (var catId in _distinctCategories)
            {
                //if (ControlUseType == AppConsts.ROTATION_MEMBER_SEARCH_USE_TYPE_CODE.ToLower().Trim())
                //{
                //    System.Web.UI.Control _categoryControl = Page.LoadControl("~/ClinicalRotation/UserControl/RequirementVerificationReadOnlyCategoryControl.ascx");
                //    (_categoryControl as IRequirementVerificationReadOnlyCategoryControlView).lstCategoryLevelData = CurrentViewContext.lstVerificationDetailData.Where(vdd => vdd.CatId == catId).ToList();
                //    (_categoryControl as IRequirementVerificationReadOnlyCategoryControlView).lstReqItemStatusTypes = CurrentViewContext.lstReqItemStatusTypes;
                //    (_categoryControl as IRequirementVerificationReadOnlyCategoryControlView).CategoryId = catId;
                //    (_categoryControl as IRequirementVerificationReadOnlyCategoryControlView).SelectedTenantId = CurrentViewContext.SelectedTenantId;
                //    (_categoryControl as RequirementVerificationReadOnlyCategoryControl).ID = CurrentViewContext.CategoryControlIdPrefix + catId;
                //    pnlCategoryContainer.Controls.Add(_categoryControl);
                //}
                //else
                //{
                System.Web.UI.Control _categoryControl = Page.LoadControl("~/ClinicalRotation/UserControl/RequirementVerificationCategoryControl.ascx");
                (_categoryControl as IRequirementVerificationCategoryControl).lstCategoryLevelData = CurrentViewContext.lstVerificationDetailData.Where(vdd => vdd.CatId == catId).ToList();
                (_categoryControl as IRequirementVerificationCategoryControl).lstReqItemStatusTypes = CurrentViewContext.lstReqItemStatusTypes;
                (_categoryControl as IRequirementVerificationCategoryControl).CategoryId = catId;
                (_categoryControl as IRequirementVerificationCategoryControl).SelectedTenantId = CurrentViewContext.SelectedTenantId;
                (_categoryControl as RequirementVerificationCategoryControl).ID = CurrentViewContext.CategoryControlIdPrefix + catId;
                pnlCategoryContainer.Controls.Add(_categoryControl);
                //}
            }
        }

        /// <summary>
        /// Redirect the user back to Queue.
        /// </summary>
        private void RedirectToQueue()
        {
            var queryString = new Dictionary<String, String>();

            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.SelectedTenantId) },
                                                                    { "Child",  @"~\ClinicalRotation\RequirementVerificationQueue.ascx"}
                                                                 };
            string url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }

        /// <summary>
        /// Save the data of the Verification details screen.
        /// </summary>
        private void SaveData()
        {
            CurrentViewContext.DataToSave = new RequirementVerificationData();
            CurrentViewContext.DataToSave.lstData = new List<RequirementVerificationCategoryData>();
            CurrentViewContext.DataToSave.RPSId = CurrentViewContext.RPSId;

            var _controlCount = pnlCategoryContainer.Controls.Count;
            var _distinctCategories = CurrentViewContext.lstVerificationDetailData.Select(vdd => vdd.CatId).Distinct().ToList();

            foreach (var catId in _distinctCategories)
            {
                var _ctrl = pnlCategoryContainer.FindServerControlRecursively(CurrentViewContext.CategoryControlIdPrefix + catId);

                if (_ctrl.IsNotNull() && _ctrl is RequirementVerificationCategoryControl)
                {
                    var _categoryControl = _ctrl as RequirementVerificationCategoryControl;
                    CurrentViewContext.DataToSave.lstData.Add(_categoryControl.GetCategoryItemData());
                }
            }
            Presenter.SaveData();
        }

        private void HideControls()
        {
            //if (ControlUseType == AppConsts.ROTATION_MEMBER_SEARCH_USE_TYPE_CODE.ToLower().Trim())
            //{
            //    cmdBar.SubmitButton.Style["display"] = "none";
            //    cmdBar.SaveButton.Style["display"] = "none";
            //}
        }

        private void ReturnToRotationMemberSearchQueue()
        {
            var queryString = new Dictionary<String, String>();

            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.SelectedTenantId) },
                                                                    { "Child",  @"~\ClinicalRotation\RotationMemberSearch.ascx"}
                                                                 };
            string url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }
        #endregion

        #endregion
    }
}