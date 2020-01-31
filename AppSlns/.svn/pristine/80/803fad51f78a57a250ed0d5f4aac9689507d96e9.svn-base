using CoreWeb.ClinicalRotation.Views;
using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.ClinicalRotation.Pages
{
    public partial class RotationCustomAttributes : BaseWebPage, IRotationCustomAttribute
    {
        #region Private Properties

        private RotationCustomAttributePresenter _presenter = new RotationCustomAttributePresenter();

        #endregion

        #region PUBLIC PROPRTIES

        public RotationCustomAttributePresenter Presenter
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

        public IRotationCustomAttribute CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 TenantID
        {
            get;
            set;
        }

        Int32 LoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        List<CustomAttribteContract> IRotationCustomAttribute.CustomAttributeList { get; set; }

        public List<Int32> lstTenant
        {
            get
            {
                if (!ViewState["lstTenant"].IsNullOrEmpty())
                {
                    return (List<Int32>)ViewState["lstTenant"];
                }
                return new List<int>();
            }
            set
            {
                ViewState["lstTenant"] = value;
            }
        }


        List<TenantDetailContract> IRotationCustomAttribute.lstTenantDetail
        {
            get
            {
                if (!ViewState["lstTenantDetail"].IsNull())
                {
                    return ViewState["lstTenantDetail"] as List<TenantDetailContract>;
                }

                return new List<TenantDetailContract>();
            }
            set
            {
                ViewState["lstTenantDetail"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    CaptureQuerystringParameters();
                    Presenter.GetTenants();
                }

                foreach (int tenantID in lstTenant)
                {
                    CurrentViewContext.TenantID = tenantID;

                    var tenant = CurrentViewContext.lstTenantDetail.Where(cond => cond.TenantID == tenantID).FirstOrDefault();
                    string instName = string.Empty;
                    if (!tenant.IsNullOrEmpty())
                    {
                        instName = tenant.TenantName;
                    }

                    SharedUserCustomAttributeForm caCustomAttributes = (SharedUserCustomAttributeForm)Page.LoadControl("~\\ClinicalRotation\\UserControl\\SharedUserCustomAttributeForm.ascx");
                    caCustomAttributes.ID = string.Concat("caCustomAttributes_", tenantID.ToString());
                    caCustomAttributes.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                    caCustomAttributes.Title = string.Concat("Other Details", (" - " + instName));

                    Presenter.GetCustomAttributeList();

                    if (!CurrentViewContext.CustomAttributeList.IsNullOrEmpty())
                    {
                        GenerateCustomAttributes(caCustomAttributes, tenantID);
                        pnlRotationCustomAttribues.Controls.Add(caCustomAttributes);
                    }
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

        private void CaptureQuerystringParameters()
        {
            lstTenant = new List<int>();

            //Getting Data From QueryString
            if (!Request["TenantIDs"].IsNullOrEmpty())
            {
                string tenantIds = Convert.ToString(Request["TenantIDs"]);
                foreach (string tenantID in tenantIds.Split(','))
                {
                    Int32 t = 0;
                    Int32.TryParse(tenantID, out t);

                    if (t > 0)
                        lstTenant.Add(t);
                }
            }
        }

        void GenerateCustomAttributes(SharedUserCustomAttributeForm caCustomAttributes, Int32 tenantID)
        {
            //Generate the control using database, but set the values from the session
            caCustomAttributes.TenantId = tenantID;
            caCustomAttributes.TypeCode = CustomAttributeUseTypeContext.Hierarchy.GetStringValue();
            //caCustomAttributes.MappingRecordId = this.NodeId;
            //caCustomAttributes.ValueRecordId = this.DeptProgId;
            caCustomAttributes.DataSourceModeType = DataSourceMode.Ids;
            //caCustomAttributes.Title = "Other Details";
            caCustomAttributes.ControlDisplayMode = DisplayMode.Controls;
            caCustomAttributes.CurrentLoggedInUserId = LoggedInUserID;
            caCustomAttributes.ValidationGroup = "grpFormSubmit";
            caCustomAttributes.IsReadOnly = false;
            caCustomAttributes.lstTypeCustomAttributes = CurrentViewContext.CustomAttributeList;
            caCustomAttributes.IsSearchTypeControl = true;

            Dictionary<Int32, string> dicCustomAttributes = new Dictionary<int, string>();

            if (Session["dicCustomAttributes"] != null && !Session["dicCustomAttributes"].IsNullOrEmpty())
                dicCustomAttributes = (Dictionary<Int32, string>)Session["dicCustomAttributes"];


            foreach (var customKey in dicCustomAttributes.Keys.ToList())
            {
                Int32 currentTenantID = Convert.ToInt32(customKey);
                if (!lstTenant.Where(s => s == currentTenantID).Any())
                {
                    dicCustomAttributes.Remove(customKey);
                }
            }

            if (!dicCustomAttributes.IsNullOrEmpty())
            {
                if (dicCustomAttributes.ContainsKey(tenantID))
                {
                    caCustomAttributes.previousValues = dicCustomAttributes[tenantID];
                }
            }
        }

        #region Button Events

        /// <summary>
        /// Reset Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                Session["dicCustomAttributes"] = null;

                foreach (var tenantID in lstTenant)
                {
                    string controlID = string.Concat("caCustomAttributes_", tenantID.ToString());
                    SharedUserCustomAttributeForm caCustomAttributes = (SharedUserCustomAttributeForm)pnlRotationCustomAttribues.FindControl(controlID);

                    if (caCustomAttributes != null && !caCustomAttributes.IsNullOrEmpty())
                    {
                        caCustomAttributes.ResetCustomAttributes();                        
                    }
                }

                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "parent.setIsCustomAttributeApplied('false');", true);
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

        /// <summary>
        /// Apply Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_SaveClick(object sender, EventArgs e)
        {
            try
            {
                Dictionary<Int32, string> dicCustomAttributes = new Dictionary<int, string>();

                //if (Session["dicCustomAttributes"] != null && !Session["dicCustomAttributes"].IsNullOrEmpty())
                //    dicCustomAttributes = (Dictionary<Int32, string>)Session["dicCustomAttributes"];
                Boolean IsCustomAttributeApplied = false;
                Session["IsCustomAttributeApplied"] = null;
                foreach (var tenantID in lstTenant)
                {
                    string controlID = string.Concat("caCustomAttributes_", tenantID.ToString());
                    SharedUserCustomAttributeForm caCustomAttributes = (SharedUserCustomAttributeForm)pnlRotationCustomAttribues.FindControl(controlID);

                    if (caCustomAttributes != null && !caCustomAttributes.IsNullOrEmpty())
                    {
                        if (dicCustomAttributes.ContainsKey(tenantID))
                            dicCustomAttributes[tenantID] = caCustomAttributes.GetCustomDataXML();
                        else
                            dicCustomAttributes[tenantID] = caCustomAttributes.GetCustomDataXML();

                        IsCustomAttributeApplied = Convert.ToBoolean(Session["IsCustomAttributeApplied"]);                       
                    }
                }

                Session["dicCustomAttributes"] = dicCustomAttributes;
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "parent.setIsCustomAttributeApplied('" + IsCustomAttributeApplied + "');", true);
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "parent.closeCustomAttributePopUp('" + IsCustomAttributeApplied + "');", true);
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
    }
}