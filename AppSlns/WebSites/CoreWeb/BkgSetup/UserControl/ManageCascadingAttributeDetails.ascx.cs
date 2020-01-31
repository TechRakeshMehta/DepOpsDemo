using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageCascadingAttributeDetails : BaseUserControl, ICascadingAttributeDetailView
    {
        #region private properties
        CascadingAttributeDetailPresenter _presenter = new CascadingAttributeDetailPresenter();
        #endregion

        #region Public Properties
        public CascadingAttributeDetailPresenter Presenter
        {
            get
            {
                _presenter.View = this;
                return _presenter;
            }
            set
            {
                _presenter = value;
                _presenter.View = this;
            }
        }

        //public string FilterExpression
        //{
        //    get
        //    {
        //        if(ViewState["FilterExpression"] == null)
        //        {
        //            ViewState["FilterExpression"] = "";
        //        }
        //        return ViewState["FilterExpression"].ToString();
        //    }
        //    set
        //    {
        //        ViewState["FilterExpression"] = value;
        //    }
        //}

        public string AttributeName
        {
            get
            {
                if (ViewState["AttributeName"] == null)
                {
                    ViewState["AttributeName"] = "";
                }
                return ViewState["AttributeName"].ToString();
            }
            set
            {
                ViewState["AttributeName"] = value;
            }
        }

        #endregion

        #region View Implementation
        public int SelectedTenantId
        {
            get
            {
                if (ViewState["SelectedTenantId"] == null)
                {
                    ViewState["SelectedTenantId"] = 0;
                }
                return Convert.ToInt32(ViewState["SelectedTenantId"]);
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        public int SelectedAttributeId
        {
            get
            {
                if (ViewState["SelectedAttributeId"] == null)
                {
                    ViewState["SelectedAttributeId"] = 0;
                }
                return Convert.ToInt32(ViewState["SelectedAttributeId"]);
            }
            set
            {
                ViewState["SelectedAttributeId"] = value;
            }
        }

        public List<CascadingAttributeOptionsContract> AttributeOptions { get; set; }

        public CascadingAttributeOptionsContract CurrentOption { get; set; }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return CurrentUserId;
            }
        }
        #endregion

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                Title = "Manage Cascading Attribute Details";
                base.OnInit(e);
            }
            catch (SysXException ex)
            {
                LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CaptureQueryString();
            }
            lblAttrName.Text = AttributeName.HtmlEncode();
        }
        #endregion Page Events

        private void CaptureQueryString()
        {

            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);
                if (args.ContainsKey("AttributeId"))
                {
                    SelectedAttributeId = Convert.ToInt32(args["AttributeId"].ToString());
                }
                if (args.ContainsKey("SelectedTenantId"))
                {
                    SelectedTenantId = Convert.ToInt32(args["SelectedTenantId"].ToString());
                }
                if (args.ContainsKey("AttributeName"))
                {
                    AttributeName = args["AttributeName"].ToString();
                }
                //if (args.ContainsKey("FilterExpression"))
                //{
                //    FilterExpression = args["FilterExpression"].ToString();
                //}
            }
        }

        #region Grid Events
        protected void grdAttributeOptions_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            InsertUpdate(e, true);
        }

        protected void grdAttributeOptions_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            InsertUpdate(e, false);
        }

        private void InsertUpdate(Telerik.Web.UI.GridCommandEventArgs e, bool IsUpdate)
        {
            try
            {
                if (Page.IsValid)
                {
                    CurrentOption = new CascadingAttributeOptionsContract();
                    GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                    if (IsUpdate)
                    {
                        CurrentOption.Id = Convert.ToInt32(gridEditableItem.GetDataKeyValue("Id"));
                    }
                    CurrentOption.AttributeId = SelectedAttributeId;
                    CurrentOption.Value = (gridEditableItem.FindControl("txtValue") as WclTextBox).Text.Trim();
                    CurrentOption.SourceValue = (gridEditableItem.FindControl("txtSourceValue") as WclTextBox).Text.Trim();
                    //CurrentOption.DisplaySequence = Convert.ToInt32((gridEditableItem.FindControl("txtDisplaySequence") as WclTextBox).Text);

                    if (Presenter.AddUpdateAttributeOption())
                        base.ShowSuccessMessage("Attribute option saved successfully.");
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

        protected void grdAttributeOptions_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                CurrentOption = new CascadingAttributeOptionsContract();
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentOption.Id = Convert.ToInt32(gridEditableItem.GetDataKeyValue("Id"));

                if (Presenter.DeleteOption())
                {
                    base.ShowSuccessMessage("Attribute option deleted successfully.");
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

        protected void grdAttributeOptions_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Presenter.GetAttributeOptions();
            grdAttributeOptions.DataSource = AttributeOptions;
            //grdAttributeOptions.DataBind();
        }
        
        #endregion Grid Events

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {                                                                    
                                                                    { "SelectedTenantId", SelectedTenantId.ToString()},
                                                                    //{ "FilterExpression", FilterExpression},                                                                    
                                                                    { "Child", AppConsts.BACKGROUND_SERVICE_ATTRIBUTES}
                                                                 };
            string url = String.Format("~/BkgSetup/Default.aspx?ucid={0}&args={1}", "", queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }
    }
}