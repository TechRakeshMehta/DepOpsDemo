#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Linq;


#endregion

#region UserDefined

using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.Utils;
using CoreWeb.IntsofSecurityModel;



#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{

    public partial class ContactDetaill : BaseWebPage, IContactDetailView
    {

        #region Properties

        private Int32 _selectedTenantId;

        /// <summary>
        /// Gets and sets the selected tenantId
        /// </summary>
        Int32 IContactDetailView.SelectedTenantId
        {
            get
            {
                if (!ViewState["SelectedTenantId"].IsNull())
                {
                    return (Int32)(ViewState["SelectedTenantId"]);
                }
                else
                {
                    return AppConsts.NONE;
                }
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        /// <summary>
        /// 
        /// Gets and sets the currentLoggedInUserId
        /// </summary>
        Int32 IContactDetailView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        /// <summary>
        /// Gets and sets the InstitutionContactId
        /// </summary>
        Int32 IContactDetailView.InstitutionContactId
        {
            get
            {
                if (!ViewState["ContactId"].IsNull())
                {
                    return (Int32)(ViewState["ContactId"]);
                }
                else
                {
                    return AppConsts.NONE;
                }
            }
            set
            {
                ViewState["ContactId"] = value;
            }
        }

        /// <summary>
        /// Gets and sets the HierarchyNodeID
        /// </summary>
        Int32 IContactDetailView.HierarchyNodeID
        {
            get
            {
                if (!ViewState["NodeId"].IsNull())
                {
                    return (Int32)(ViewState["NodeId"]);
                }
                else
                {
                    return AppConsts.NONE;
                }
            }
            set
            {
                ViewState["NodeId"] = value;
            }
        }

        /// <summary>
        /// Gets the CurrentViewContext
        /// </summary>
        IContactDetailView CurrentViewContext
        {
            get { return this; }
        }

        /// <summary>
        /// Gets and sets the TenandId
        /// </summary>
        Int32 IContactDetailView.TenantId
        {
            get
            {
                if (!ViewState["TenantId"].IsNull())
                {
                    return (Int32)(ViewState["TenantId"]);
                }
                else
                {
                    return AppConsts.NONE;
                }
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        /// <summary>
        /// Gets and sets the Presenter
        /// </summary>
        public ContactDetailPresenter Presenter
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
        /// Gets and sets the FirstName
        /// </summary>
        String IContactDetailView.FirstName
        {
            get
            {
                return txtFirstName.Text.Trim();
            }
            set
            {
                txtFirstName.Text = value;
            }
        }

        /// <summary>
        /// Gets and sets the Lastname
        /// </summary>
        String IContactDetailView.Lastname
        {
            get
            {
                return txtLastName.Text.Trim();
            }
            set
            {
                txtLastName.Text = value;
            }
        }

        /// <summary>
        /// Gets and sets the Title
        /// </summary>
        String IContactDetailView.Title
        {
            get
            {
                return txtTitle.Text.Trim();
            }
            set
            {
                txtTitle.Text = value;
            }
        }

        /// <summary>
        /// Gets and sets the PrimaryPhone
        /// </summary>
        String IContactDetailView.PrimaryPhone
        {
            get
            {
                return txtPhone.Text.Trim();
            }
            set
            {
                txtPhone.Text = value;
            }
        }

        /// <summary>
        /// Gets and sets the PrimaryEmailAddress
        /// </summary>
        String IContactDetailView.PrimaryEmailAddress
        {
            get
            {
                return txtPrimaryEmailAddress.Text.Trim();
            }
            set
            {
                txtPrimaryEmailAddress.Text = value;
            }
        }

        /// <summary>
        /// Gets and sets the Address1
        /// </summary>
        String IContactDetailView.Address1
        {
            get
            {
                return txtAddress1.Text.Trim();
            }
            set
            {
                txtAddress1.Text = value;
            }
        }

        /// <summary>
        /// Gets and sets the Address2
        /// </summary>
        String IContactDetailView.Address2
        {
            get
            {
                return txtAddress2.Text.Trim();
            }
            set
            {
                txtAddress2.Text = value;
            }
        }

        /// <summary>
        /// Gets and sets the ZipCodeId
        /// </summary>
        Int32? IContactDetailView.ZipCodeId
        {
            get
            {

                if (txtZipCode.Text.Trim().IsNullOrEmpty())
                {
                    return (Int32?)null;
                }
                else
                {
                    return Convert.ToInt32(txtZipCode.Text.Trim());
                }


            }
            set
            {
                txtZipCode.Text = value.ToString();
            }
        }


        #endregion

        #region Variables

        private ContactDetailPresenter _presenter = new ContactDetailPresenter();

        #endregion

        #region Events

        #region Page Events

        /// <summary>
        /// Load event of Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!Request.QueryString["SelectedTenantId"].IsNullOrEmpty())
                    {
                        CurrentViewContext.SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                    }

                    if (!Request.QueryString["ContactId"].IsNullOrEmpty())
                    {
                        CurrentViewContext.InstitutionContactId = Convert.ToInt32(Request.QueryString["ContactId"]);
                    }

                    if (!Request.QueryString["ParentID"].IsNullOrEmpty())
                    {
                        CurrentViewContext.HierarchyNodeID = Convert.ToInt32(Request.QueryString["ParentID"]);
                    }

                    Presenter.GetContactData();

                }


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


        #endregion

        #region Button Events


        /// <summary>
        /// Event to update Contact
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarNew_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                Boolean status = Presenter.SaveContact();
                if (status)
                {
                    lblSuccess.Visible = true;
                    lblSuccess.ShowMessage("Contact updated successfully.", MessageType.SuccessMessage);
                    if (!(SysXWebSiteUtils.SessionService).BusinessChannelType.IsNull() &&
             (SysXWebSiteUtils.SessionService).BusinessChannelType.BusinessChannelTypeID == AppConsts.AMS_BUSINESS_CHANNEL_TYPE)
                    {

                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    }
                }
                else
                {
                    lblSuccess.Visible = true;
                    lblSuccess.ShowMessage("Some error has occured while updating the contact.", MessageType.Error);
                }
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

        protected void fsucCmdBarNew_CancelClick(object sender, EventArgs e)
        {
            Presenter.GetContactData();
        }


        #endregion

        #endregion


    }
}