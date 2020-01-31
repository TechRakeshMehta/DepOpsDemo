using System;
using Microsoft.Practices.ObjectBuilder;
using Entity;
using System.Collections.Generic;
using Telerik.Web.UI;
using INTSOF.Utils;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageBkgServiceCustomForm : BaseUserControl,IManageBkgServiceCustomFormView
    {

        #region Variables
        #region public Variables
        #endregion

        #region private Variables
        private ManageBkgServiceCustomFormPresenter _presenter = new ManageBkgServiceCustomFormPresenter();
        private String _viewType;
        #endregion
        #endregion

        #region properties
        public ManageBkgServiceCustomFormPresenter Presenter
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

        public Int32 ServiceId
        {
            get
            {
                if (!ViewState["ServiceId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["ServiceId"]);
                }
                return 0;
            }
            set
            {
                ViewState["ServiceId"] = value;
            }
        }

        public String ServiceName
        {
            get
            {
                if (!ViewState["ServiceName"].IsNull())
                {
                    return Convert.ToString(ViewState["ServiceName"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["ServiceName"] = value;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }
        public IManageBkgServiceCustomFormView CurrentViewContext
        {
            get { return this; }
        }

        public List<ManageServiceCustomFormContract> MappedCustomFormList
        {
            get;
            set;
        }
        public List<CustomForm> CustomFormList 
        {
            get;
            set;
        }

        public String ErrorMessage
        {
            get;
            set;
        }
        #endregion

        #region Events
        #region Page events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.Title = "Manage Custom Form mapping";
                base.SetPageTitle("Manage Custom Form mapping");
                base.OnInit(e);
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
            if (!this.IsPostBack)
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    CaptureQuerystringParameters(args);
 
                }
                ApplyActionLevelPermission(ActionCollection, "Manage BkgService Custom Form");
                Presenter.OnViewInitialized();
                lblServiceName.Text = CurrentViewContext.ServiceName.ToString().HtmlEncode();
            }
            Presenter.OnViewLoaded();
        }
        #endregion

        #region Button Events
        protected void CmdBarCancel_Click(Object sender, EventArgs e)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    
                                                                    { "Child", ChildControls.ManageMasterService}
                                                                 
                                                                   
                                                                 };
            string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }
        #endregion
        #endregion

        #region Methods
        private void CaptureQuerystringParameters(Dictionary<String, String> args)
        {
            args.ToDecryptedQueryString(Request.QueryString["args"]);


            if (args.ContainsKey("ServiceID"))
            {
                ServiceId = Convert.ToInt32(args["ServiceID"]);

            }
            if (args.ContainsKey("ServiceName"))
            {
                ServiceName = Convert.ToString(args["ServiceName"]);
            }
        }
        #endregion

        protected void grdCusfomFormmapping_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            _presenter.GetCustomFormsForService();
            grdCusfomFormmapping.DataSource = CurrentViewContext.MappedCustomFormList;
        }

        protected void grdCusfomFormmapping_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (GridEditFormItem)e.Item;
                    WclComboBox ddlCustomForm = (WclComboBox)editform.FindControl("ddlCustomForm");                  
                    Presenter.GetAllCustomForm();
                    if (!CurrentViewContext.CustomFormList.IsNull())
                    {
                        ddlCustomForm.DataSource = CurrentViewContext.CustomFormList;
                        //ddlCustomForm.Items.Insert(0, new RadComboBoxItem { Text = "--SELECT--", Value = "0" });
                        ddlCustomForm.DataBind();
                        //For Update
                        //if (!(e.Item is GridEditFormInsertItem))
                        //{
                            
                        //}
                    }
                    //if (CurrentViewContext.CustomFormList.IsNull() || CurrentViewContext.CustomFormList.Count == AppConsts.NONE)
                    //{
                    //    grdCusfomFormmapping.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                    //}
                   
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

        protected void grdCusfomFormmapping_InsertCommand(object sender, GridCommandEventArgs e)
        {
            WclComboBox chkedForms = (e.Item.FindControl("ddlCustomForm") as WclComboBox);
            List<Int32> selectedcustomForms = new List<Int32>();
            for (Int32 i = 0; i < chkedForms.CheckedItems.Count; i++)
            {
                if (chkedForms.CheckedItems[i].Checked)
                {
                    selectedcustomForms.Add(Convert.ToInt32(chkedForms.CheckedItems[i].Value));
                }
            }
            _presenter.SaveSvcFormMapping(selectedcustomForms);
            if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
            {
                e.Canceled = true;
                //base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.ServiceName));
                (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0}.", CurrentViewContext.ErrorMessage), MessageType.Error);
            }
            else
            {
                e.Canceled = false;
                base.ShowSuccessMessage("Custom Forms mapped successfully.");
            }
            //if (CurrentViewContext.CustomFormList.IsNull() || CurrentViewContext.CustomFormList.Count == AppConsts.NONE)
            //{
            //    grdCusfomFormmapping.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
            //}
        }

        protected void grdCusfomFormmapping_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            String svcFormMappingID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SvcFormMappingID"].ToString();
            String customFormID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CustomFormID"].ToString();
            _presenter.DeleteSvcFormMApping(Convert.ToInt32(svcFormMappingID));
            if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
            {
                base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
                
            }
            else
            {
               
                base.ShowSuccessMessage("Custom Form mapping deleted successfully.");
            }
        }



        private void ApplyPermisions()
        {
            List<Entity.FeatureRoleAction> permission = base.ActionPermission;
            if (permission.IsNotNull())
            {
                permission.ForEach(x =>
                {
                    switch (x.PermissionID)
                    {
                        case AppConsts.ONE:
                            {
                                break;
                            }
                        case AppConsts.THREE:
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "Add")
                                {
                                    grdCusfomFormmapping.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                //else if (x.FeatureAction.CustomActionId == "Edit")
                                //{
                                //    grdCusfomFormmapping.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                //}
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdCusfomFormmapping.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }



        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                Entity.ClientEntity.ClsFeatureAction objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Add";
                objClsFeatureAction.CustomActionLabel = "Add New";
                objClsFeatureAction.ScreenName = "Manage BkgService Custom Form";
                actionCollection.Add(objClsFeatureAction);
                //objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                //objClsFeatureAction.CustomActionId = "Edit";
                //objClsFeatureAction.CustomActionLabel = "Edit";
                //objClsFeatureAction.ScreenName = "ManageBkgServiceCustomForm";
                //actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Delete";
                objClsFeatureAction.CustomActionLabel = "Delete";
                objClsFeatureAction.ScreenName = "Manage BkgService Custom Form";
                actionCollection.Add(objClsFeatureAction);
                return actionCollection;
            }
        }


        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            ApplyPermisions();

        }

    }
}

