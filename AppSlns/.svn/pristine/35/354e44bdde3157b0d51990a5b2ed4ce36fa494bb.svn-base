using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;
using Entity;
using INTSOF.UI.Contract.SysXSecurityModel;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public partial class FeatureActionList : BaseWebPage, IFeatureActionListView
    {
        #region Variables

        private FeatureActionListPresenter _presenter = new FeatureActionListPresenter();

        #endregion


        #region Properties

        public Int32 ProductFeatureID
        {
            get
            {
                if (ViewState["ProductFeatureID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["ProductFeatureID"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["ProductFeatureID"] = value;
            }
        }

        public Int32 SysXBlockFeatureID
        {
            get
            {
                if (ViewState["SysXBlockFeatureID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["SysXBlockFeatureID"]);
                }
                return AppConsts.NONE;

            }
            set
            {
                ViewState["SysXBlockFeatureID"] = value;
            }
        }

        public List<String> lstNodeIds
        {
            get;
            set;
        }

        public Int32 RolePermissionProductFeatueID
        {
            get
            {
                if (ViewState["RolePermissionProductFeatureID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["RolePermissionProductFeatureID"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["RolePermissionProductFeatureID"] = value;
            }
        }

        String DefaultAttributeAccess
        {
            get
            {
                if (ViewState["DefaultAttributeAccess"].IsNotNull())
                {
                    return Convert.ToString(ViewState["DefaultAttributeAccess"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["DefaultAttributeAccess"] = value;
            }
        }




        public FeatureActionListPresenter Presenter
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



        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Request.QueryString["ProductFeatureID"].IsNull())
                {
                    ProductFeatureID = Convert.ToInt32(Request.QueryString["ProductFeatureID"]);
                }

                if (!Request.QueryString["RolePermissionProductFeatueID"].IsNull())
                {
                    RolePermissionProductFeatueID = Convert.ToInt32(Request.QueryString["RolePermissionProductFeatueID"]);
                }

                if (!Request.QueryString["SysXBlockFeatureID"].IsNull())
                {
                    SysXBlockFeatureID = Convert.ToInt32(Request.QueryString["SysXBlockFeatureID"]);
                }

                if (!Request.QueryString["defaultAttributeAccess"].IsNull())
                {
                    DefaultAttributeAccess = Convert.ToString(Request.QueryString["defaultAttributeAccess"]);
                }
                BindTreeList();
            }
        }


        #endregion

        #region Button Events

        protected void fsucFeatureActionList_SaveClick(object sender, EventArgs e)
        {
            String permissionName = String.Empty;
            List<RoleFeatureActionContract> roleFeatureActions;
            if (Session["RoleFeatureActions"].IsNotNull())
            {
                roleFeatureActions = Session["RoleFeatureActions"] as List<RoleFeatureActionContract>;
            }
            else
            {
                Session["RoleFeatureActions"] = new List<RoleFeatureActionContract>();
                roleFeatureActions = Session["RoleFeatureActions"] as List<RoleFeatureActionContract>;
            }
            List<Permission> lstPermissions = Presenter.GetListOFPermissions();

            if (treeListFeatureAction.Items.Count > AppConsts.NONE)
            {
                foreach (TreeListDataItem treeListItem in treeListFeatureAction.Items)
                {
                    RadioButton rbFeatureFullAccess = (RadioButton)(treeListItem.FindControl("rbFeatureFullAccess"));
                    RadioButton rbFeatureReadOnly = (RadioButton)(treeListItem.FindControl("rbFeatureReadOnly"));
                    RadioButton rbFeatureNoAccess = (RadioButton)(treeListItem.FindControl("rbFeatureNoAccess"));
                    if (rbFeatureFullAccess.Checked == true)
                    {
                        Int32 nodeId = Convert.ToInt32(treeListItem.GetDataKeyValue("NodeId"));
                        permissionName = rbFeatureFullAccess.ID.Substring(AppConsts.NINE);
                        SetPermission(permissionName, roleFeatureActions, lstPermissions, nodeId);
                    }
                    else if (rbFeatureReadOnly.Checked == true)
                    {
                        Int32 nodeId = Convert.ToInt32(treeListItem.GetDataKeyValue("NodeId"));
                        permissionName = rbFeatureReadOnly.ID.Substring(AppConsts.NINE);
                        SetPermission(permissionName, roleFeatureActions, lstPermissions, nodeId);
                    }
                    else if (rbFeatureNoAccess.Checked == true)
                    {
                        Int32 nodeId = Convert.ToInt32(treeListItem.GetDataKeyValue("NodeId"));
                        permissionName = rbFeatureNoAccess.ID.Substring(AppConsts.NINE);
                        SetPermission(permissionName, roleFeatureActions, lstPermissions, nodeId);
                    }
                }
            }
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ClosePopUp", "ClosePopup();", true);
        }

        private void SetPermission(String permissionName, List<RoleFeatureActionContract> roleFeatureActions, List<Permission> lstPermissions, Int32 nodeId)
        {
            RoleFeatureActionContract contract = new RoleFeatureActionContract();

            contract.SysXBlockFeatureID = SysXBlockFeatureID;
            contract.FeatureActionID = Convert.ToInt32(nodeId);


            RoleFeatureActionContract existingContract = roleFeatureActions.FirstOrDefault(cond => cond.FeatureActionID == contract.FeatureActionID && cond.SysXBlockFeatureID == contract.SysXBlockFeatureID);

            if (existingContract.IsNotNull())
            {
                existingContract.PermissionID = lstPermissions.FirstOrDefault(x => x.Name.Equals(permissionName)).PermissionId;
            }
            else
            {
                contract.PermissionID = lstPermissions.FirstOrDefault(x => x.Name.Equals(permissionName)).PermissionId;
                roleFeatureActions.Add(contract);
            }
        }


        #endregion

        #region Tree Events

        protected void treeListFeatureAction_ItemDataBound(object sender, TreeListItemDataBoundEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(TreeListItemType.AlternatingItem) || e.Item.ItemType.Equals(TreeListItemType.Item) || e.Item.ItemType.Equals(TreeListItemType.SelectedItem))
                {
                    TreeListDataItem item = (TreeListDataItem)e.Item;
                    if (!Request.QueryString["SysXBlockFeatureID"].IsNull())
                    {
                        SysXBlockFeatureID = Convert.ToInt32(Request.QueryString["SysXBlockFeatureID"]);
                    }
                    List<RoleFeatureActionContract> roleFeatureActions;
                    if (Session["RoleFeatureActions"].IsNotNull())
                    {
                        roleFeatureActions = Session["RoleFeatureActions"] as List<RoleFeatureActionContract>;
                    }
                    else
                    {
                        Session["RoleFeatureActions"] = new List<RoleFeatureActionContract>();
                        roleFeatureActions = Session["RoleFeatureActions"] as List<RoleFeatureActionContract>;
                    }
                    String code = ((FeatureActionContract)(((TreeListEditableItem)(item)).DataItem)).Code;


                    if (code == "AAAD")
                    {
                        Int32 featureActionId = Convert.ToInt32(item.GetDataKeyValue("NodeId"));
                        RoleFeatureActionContract roleFeatureActionContract = roleFeatureActions.FirstOrDefault(cond => cond.FeatureActionID == featureActionId && cond.SysXBlockFeatureID == SysXBlockFeatureID);

                        if (roleFeatureActionContract.IsNotNull())
                        {
                            Int32 permissionID = roleFeatureActionContract.PermissionID;
                            switch (permissionID)
                            {
                                case AppConsts.ONE:
                                    {
                                        ((RadioButton)e.Item.FindControl("rbFeatureFullAccess")).Checked = true;
                                        break;
                                    }
                                case AppConsts.THREE:
                                    {
                                        ((RadioButton)e.Item.FindControl("rbFeatureReadOnly")).Checked = true;
                                        break;
                                    }
                                case AppConsts.FOUR:
                                    {
                                        ((RadioButton)e.Item.FindControl("rbFeatureNoAccess")).Checked = true;
                                        break;
                                    }
                                default:
                                    {
                                        ((RadioButton)e.Item.FindControl("rbFeatureNoAccess")).Checked = true;
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            // ((RadioButton)e.Item.FindControl("rbFeatureNoAccess")).Checked = true;
                            if (DefaultAttributeAccess == AttributeAccessType.FullAcess.GetStringValue())
                            {
                                ((RadioButton)e.Item.FindControl("rbFeatureFullAccess")).Checked = true;
                            }
                            else if (DefaultAttributeAccess == AttributeAccessType.ReadOnly.GetStringValue())
                            {
                                ((RadioButton)e.Item.FindControl("rbFeatureReadOnly")).Checked = true;
                            }
                            else
                            {
                                ((RadioButton)e.Item.FindControl("rbFeatureNoAccess")).Checked = true;
                            }

                        }

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

        protected void treeListFeatureAction_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        { 
        }

        #endregion
        public void BindTreeList()
        {
            List<FeatureActionContract> lstData = Presenter.GetFeatureActionT(ProductFeatureID);
            FeatureActionContract featureActionContract = lstData.FirstOrDefault(x => x.ParentId == "");
            featureActionContract.ParentId = null;
            var copyFeatureAction = featureActionContract;
            lstData.Remove(featureActionContract);
            lstData.Add(copyFeatureAction);
            treeListFeatureAction.DataSource = lstData;
            treeListFeatureAction.DataBind();
            treeListFeatureAction.ExpandAllItems();
            if (lstData.FirstOrDefault(x => x.Code.Equals("AAAD")).IsNullOrEmpty())
            {
                fsucFeatureActionList.DisplayButtons = CommandBarButtons.Cancel;
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideCollapseAll", "HideCollapseAll();", true);
        }

        ///// <summary>
        ///// Renders the given writer.
        ///// </summary>
        ///// <param name="writer">.</param>
        //protected override void Render(HtmlTextWriter writer)
        //{
        //    try
        //    {
        //        base.Render(writer);
        //    }
        //    catch (SysXException ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }

        //}
        #endregion

    }
}