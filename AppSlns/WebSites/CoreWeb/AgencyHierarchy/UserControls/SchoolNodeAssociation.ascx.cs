using CoreWeb.AgencyHierarchy.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.Common;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;

namespace CoreWeb.AgencyHierarchy.UserControls
{
    public partial class SchoolNodeAssociation : BaseUserControl, ISchoolNodeAssociationView
    {
        #region Handlers

        public delegate bool ShowMessageHandler(object sender, StatusMessages messageType, String message);
        public event ShowMessageHandler eventShowMessage;

        #endregion

        #region [Variables / Properties]

        #region [Private Variables]

        private SchoolNodeAssociationPresenter _presenter = new SchoolNodeAssociationPresenter();
        private Int32 _tenantId;

        #endregion

        public ISchoolNodeAssociationView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public SchoolNodeAssociationPresenter Presenter
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

        Int32 ISchoolNodeAssociationView.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
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

        List<SchoolNodeAssociationDataContract> ISchoolNodeAssociationView.lstSchoolNodeAssociation
        {
            get;
            set;
        }

        public Int32 AgencyHierarchyID
        {
            get
            {
                return Convert.ToInt32(ViewState["AgencyHierarchyId"]);
            }
            set
            {
                ViewState["AgencyHierarchyId"] = value;
            }
        }

        Int32 ISchoolNodeAssociationView.SelectedTenantID
        {
            get;
            set;
        }

        List<TenantDetailContract> ISchoolNodeAssociationView.lstTenant
        {
            get;
            set;
            //get
            //{
            //    if (ViewState["lstTenant"].IsNullOrEmpty())
            //    {
            //        Presenter.GetTenants();
            //    }
            //    return (List<TenantDetailContract>)ViewState["lstTenant"];
            //}
            //set
            //{
            //    ViewState["lstTenant"] = value;
            //}
        }

        SchoolNodeAssociationContract ISchoolNodeAssociationView.SchoolNodeAssociationContract
        {
            get;
            set;
        }

        #endregion

        #region [Page Events]

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        #endregion

        #region [Grid Events]

        protected void grdSchoolNodeAss_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetSchoolNodeAssociationByAgencyHierarchyID();
                grdSchoolNodeAss.DataSource = CurrentViewContext.lstSchoolNodeAssociation;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdSchoolNodeAss_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    hdnDepartmentProgmapNew.Value = String.Empty;
                    hdnInstNodeIdNew.Value = String.Empty;
                    if (e.Item is GridEditFormItem && !(e.Item is GridEditFormInsertItem))
                    { Presenter.GetTenants(true); }
                    else
                    { Presenter.GetTenants(false); }

                    WclComboBox ddlTenant = editform.FindControl("ddlTenant") as WclComboBox;
                    ddlTenant.DataSource = CurrentViewContext.lstTenant;
                    ddlTenant.DataBind();
                    ddlTenant.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));

                    ddlTenant.Focus();

                    if (e.Item is GridEditFormItem && !(e.Item is GridEditFormInsertItem))
                    {
                        //Edit
                        Label lblInstitutionHierarchyPB = editform.FindControl("lblInstitutionHierarchyPB") as Label;

                        int tenantID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"]); ;
                        //int dpm_ID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DPM_ID"]); ;
                        //string dpmText = Convert.ToString((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DPM_Label"]); ;

                        //hdnInstNodeIdNew.Value = dpm_ID.ToString();
                        hdnDepartmentProgmapNew.Value = CurrentViewContext.lstSchoolNodeAssociation.Where(x => x.TenantID == tenantID).FirstOrDefault().CommaSeparatedDpmIds;
                        lblInstitutionHierarchyPB.Text = CurrentViewContext.lstSchoolNodeAssociation.Where(x => x.TenantID == tenantID).FirstOrDefault().CommaSeparatedDpmlabel.HtmlEncode();
                        hdnDeptLabel.Value = CurrentViewContext.lstSchoolNodeAssociation.Where(x => x.TenantID == tenantID).FirstOrDefault().CommaSeparatedDpmlabel;

                        ddlTenant.SelectedValue = tenantID.ToString();
                        ddlTenant.Enabled = false;
                    }
                }
                else if (e.Item is GridDataItem)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    Boolean IsAdminShare = Convert.ToBoolean(item["IsAdminShare"].Text);
                    if (IsAdminShare)
                    {
                        item["IsAdminShare"].Text = "Yes";
                    }
                    else
                    {
                        item["IsAdminShare"].Text = "No";
                    }
                    Boolean IsStudentShare = Convert.ToBoolean(item["IsStudentShare"].Text);
                    if (IsStudentShare)
                    {
                        item["IsStudentShare"].Text = "Yes";
                    }
                    else
                    {
                        item["IsStudentShare"].Text = "No";
                    }
                }

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdSchoolNodeAss_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {

                    if (hdnDepartmentProgmapNew.Value == AppConsts.ZERO || hdnDepartmentProgmapNew.Value == String.Empty)
                    {
                        e.Canceled = true;
                        (e.Item.FindControl("lblInstitutionHierarchyPB") as Label).Text = String.Empty;
                        (e.Item.FindControl("lblName1") as Label).ShowMessage("Please select Institution Hierarchy.", MessageType.Error);
                        return;
                    }

                    CurrentViewContext.SchoolNodeAssociationContract = new SchoolNodeAssociationContract();

                    if (e.CommandName == RadGrid.UpdateCommandName)
                    {
                        CurrentViewContext.SchoolNodeAssociationContract.AgencyHierarchyID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyHierarchyID"]);
                    }

                    WclComboBox ddlTenant = e.Item.FindControl("ddlTenant") as WclComboBox;
                    CurrentViewContext.SelectedTenantID = Convert.ToInt32(ddlTenant.SelectedValue);

                    CheckBox chkStudentProfileSharingPermission = e.Item.FindControl("chkStudentProfileSharingPermission") as CheckBox;
                    CurrentViewContext.SchoolNodeAssociationContract.IsStudentShare = chkStudentProfileSharingPermission.Checked;
                    CheckBox chkAdminProfileSharingPermission = e.Item.FindControl("chkAdminProfileSharingPermission") as CheckBox;
                    CurrentViewContext.SchoolNodeAssociationContract.IsAdminShare = chkAdminProfileSharingPermission.Checked;

                    CurrentViewContext.SchoolNodeAssociationContract.AgencyHierarchyID = CurrentViewContext.AgencyHierarchyID;
                    //hdnDepartmentProgmapNew.Value;
                    List<Int32> dpmIds = hdnDepartmentProgmapNew.Value.IsNotNull() ? hdnDepartmentProgmapNew.Value.Split(',').Select(t => Int32.Parse(t)).ToList() : new List<Int32>();
                    CurrentViewContext.SchoolNodeAssociationContract.DPM_IDs = dpmIds;
                    CurrentViewContext.SchoolNodeAssociationContract.CurrentLoggedInUserID = CurrentViewContext.CurrentUserId;

                    //if (!Presenter.IsSchoolNodeAssociationExists())
                    //{
                    Presenter.SaveUpdateSchoolNodeAssociation();
                    if (e.CommandName == RadGrid.UpdateCommandName)
                    {
                        eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "School node association updated successfully!");
                        lblFocus.Focus();
                    }
                    else
                    {
                        eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "School node association added successfully!");
                        lblFocus.Focus();
                    }
                    //}
                    //else
                    //{
                    //    e.Canceled = true;
                    //    (e.Item.FindControl("lblInstitutionHierarchyPB") as Label).Text = hdnDeptLabel.Value;
                    //    (e.Item.FindControl("lblName1") as Label).ShowMessage("School node association is already exists for selected hierarchy!", MessageType.Error);
                    //    return;
                    //}
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdSchoolNodeAss_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.SelectedTenantID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"]);

                CurrentViewContext.SchoolNodeAssociationContract = new SchoolNodeAssociationContract();
                CurrentViewContext.SchoolNodeAssociationContract.CurrentLoggedInUserID = CurrentViewContext.CurrentUserId;
                CurrentViewContext.SchoolNodeAssociationContract.AgencyHierarchyID = CurrentViewContext.AgencyHierarchyID;

                String dpmIds = ((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CommaSeparatedDpmIds"]).ToString();
                List<Int32> lstDpmIds = dpmIds.Split(',').Select(t => Int32.Parse(t)).ToList();
                CurrentViewContext.SchoolNodeAssociationContract.DPM_IDs = lstDpmIds;
                Presenter.RemoveSchoolNodeAssociation();
                eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "School node association deleted successfully!");
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        #endregion

        #region [Private Methods]


        #endregion
    }
}