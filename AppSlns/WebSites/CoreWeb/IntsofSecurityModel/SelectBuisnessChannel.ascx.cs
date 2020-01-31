using System;
using Microsoft.Practices.ObjectBuilder;
using System.Web.Security;
using CoreWeb.Shell;
using Business.RepoManagers;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using INTSOF.Utils;
using Entity;
using System.Linq;
using System.Collections.Generic;


namespace CoreWeb.IntsofSecurityModel.Views
{
    public partial class SelectBuisnessChannel : BaseUserControl, ISelectBuisnessChannelView
    {
        private ISysXSessionService _sessionService = SysXWebSiteUtils.SessionService;
        private SelectBuisnessChannelPresenter _presenter = new SelectBuisnessChannelPresenter();


        public SelectBuisnessChannelPresenter Presenter
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

        /// <summary>
        /// UserName</summary>
        /// <value>
        /// Gets or sets the value for user name.</value>
        Int32 ISelectBuisnessChannelView.OrganizationUserId
        {
            get;
            set;
        }

        /// <summary>
        /// DefaultLineOfBusiness</summary>
        /// <value>
        /// Gets or sets the value for Default Line of Business.</value>
        String ISelectBuisnessChannelView.DefaultLineOfBusiness
        {
            get;
            set;
        }

        /// <summary>
        /// SelectedBlockID</summary>
        /// <value>
        /// Gets the value for selected block's id.</value>
        public Int32 SelectedBlockId
        {
            get;
            set;
        }

        /// <summary>
        /// SelectedBlockName</summary>
        /// <value>
        /// Gets the value for selected block name.</value>
        public String SelectedBlockName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <value>
        /// The current view context.
        /// </value>
        ISelectBuisnessChannelView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Page OnInit event.
        /// </summary>
        /// <param name="e">Event</param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Apply Theme";
                base.OnInit(e);
                // For binding the line of business dropdown.
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                List<vw_UserAssignedBlocks> userAssignedBlocks = this.Presenter.GetLineOfBusinessesByUser(Convert.ToString(_sessionService.UserId)).ToList();

                cmbSxBlocks.DataSource = userAssignedBlocks.Where(col=>col.TenantID==user.TenantId).Select(block =>
                    new
                    {
                        SysXBlockId = block.SysXBlockId,
                        Name = block.NAME + " (" + Presenter.GetBusinessChannelTypes()
                        .FirstOrDefault(cond => cond.BusinessChannelTypeID == block.BusinessChannelTypeID.Value).Name + ")"
                    });
                cmbSxBlocks.DataBind();
                cmbSxBlocks.SelectedValue = this.Presenter.GetDefaultLineOfBusinessOfLoggedInUser(_sessionService.OrganizationUserId);
            }
            catch (SysXException ex)
            {
                base.LogError(base.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(base.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.RegisterControlForPostBack(btnProceed);
                base.RegisterControlForPostBack(btnCancel);
                if (!this.IsPostBack)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    //cmbSxBlocks.DataSource = this.Presenter.GetLineOfBusinessesByUser(Convert.ToString(user.UserId));
                    //cmbSxBlocks.DataBind();
                    //cmbSxBlocks.SelectedIndex = this.Presenter.GetDefaultLineOfBusinesses(user.OrganizationUserId);

                    Presenter.OnViewInitialized();
                }
                Presenter.OnViewLoaded();
            }
            catch (SysXException ex)
            {
                base.LogError(base.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(base.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                base.ShowErrorMessage(ex.Message);
            }
        }


        protected void btnProceed_Click(object sender, EventArgs e)
        {
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            CurrentViewContext.OrganizationUserId = user.OrganizationUserId;
            Int32 selecteditemID = Convert.ToInt32(cmbSxBlocks.SelectedValue);

            vw_UserAssignedBlocks selectedBlock = Presenter.GetLineOfBusinessesByUser(Convert.ToString(_sessionService.UserId)).FirstOrDefault(cond => cond.SysXBlockId == selecteditemID);
            SysXWebSiteUtils.SessionService.BusinessChannelType =
                        new BusinessChannelTypeMappingData
                        {
                            BusinessChannelTypeID = selectedBlock.BusinessChannelTypeID.Value,
                            BusinessChannelTypeName = Presenter.GetBusinessChannelTypes()
                            .FirstOrDefault(cond => cond.BusinessChannelTypeID == selectedBlock.BusinessChannelTypeID.Value).Name
                        };
            Presenter.ChangeBuisnessChannelAndRedirect(selecteditemID);
            CurrentViewContext.SelectedBlockId = Convert.ToInt32(cmbSxBlocks.SelectedValue);
            CurrentViewContext.SelectedBlockName = cmbSxBlocks.Text;
            SysXWebSiteUtils.SessionService.SetSysXBlockId(CurrentViewContext.SelectedBlockId);
            SysXWebSiteUtils.SessionService.SetSysXBlockName(CurrentViewContext.SelectedBlockName);
            SysXWebSiteUtils.SessionService.SetSysXMembershipUser(user);
            FormsAuthentication.RedirectFromLoginPage(user.UserName, false);

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            SysXWebSiteUtils.SessionService.ClearSession(true);
            Response.Redirect(FormsAuthentication.LoginUrl);
        }
    }
}

