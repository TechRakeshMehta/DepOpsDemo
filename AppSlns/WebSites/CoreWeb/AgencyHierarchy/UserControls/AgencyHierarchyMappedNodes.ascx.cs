using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using Entity.ClientEntity;

namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class AgencyHierarchyMappedNodes : BaseUserControl, IAgencyHierarchyMappedNodesView
    {
        #region Handlers
        public delegate bool ShowMessageHandler(object sender, StatusMessages messageType, String message);
        public event ShowMessageHandler eventShowMessage;

        public delegate void ShowCtrHandler();
        public event ShowCtrHandler eventShowCtr;

        #endregion

        #region Private Variables
        private AgencyHierarchyMappedNodesPresenter _presenter = new AgencyHierarchyMappedNodesPresenter();
        private Int32 _tenantId = 0;
        #endregion

        #region Properties

        #region Public Properties

        public AgencyHierarchyMappedNodesPresenter Presenter
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

        public IAgencyHierarchyMappedNodesView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        #endregion

        #region Private Properties

        Int32 IAgencyHierarchyMappedNodesView.TenantId
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

        Boolean IAgencyHierarchyMappedNodesView.IsAdminLoggedIn
        {
            get;
            set;
        }

        Int32 IAgencyHierarchyMappedNodesView.CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        List<AgencyHierarchyDataContract> IAgencyHierarchyMappedNodesView.lstMappedNodes { get; set; }


        Int32 IAgencyHierarchyMappedNodesView.NodeId
        {
            get;
            set;
        }
        public Int32 ParentNodeId
        {
            get
            {
                if (ViewState["ParentNodeId"].IsNotNull())
                    return Convert.ToInt32(ViewState["ParentNodeId"]);
                return 0;
            }
            set
            {
                ViewState["ParentNodeId"] = Convert.ToString(value);
            }
        }

        public List<Int32> MappedNodeIds
        {
            get
            {
                if (ViewState["MappedNodeIds"].IsNotNull())
                    return (List<Int32>)(ViewState["MappedNodeIds"]);
                return new List<Int32>();
            }
            set
            {
                ViewState["MappedNodeIds"] = value;
            }
        }
        #endregion
        #endregion

        #region Page Events


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                }
                Presenter.OnViewLoaded();
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

        #region Grid Events

        //protected void grdMappedNodes_Init(object sender, System.EventArgs e)
        //{
        //    GridFilterMenu menu = grdMappedNodes.FilterMenu;

        //    int i = 0;
        //    while (i < menu.Items.Count)
        //    {
        //        if (menu.Items[i].Text == GridKnownFunction.Between.ToString() || menu.Items[i].Text == GridKnownFunction.NotBetween.ToString() ||
        //            menu.Items[i].Text == GridKnownFunction.NotIsEmpty.ToString() || menu.Items[i].Text == GridKnownFunction.NotIsNull.ToString())
        //        {
        //            menu.Items.RemoveAt(i);
        //        }
        //        else
        //        {
        //            i++;
        //        }
        //    }
        //}
        protected void grdMappedNodes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetMappedNodes();
                MappedNodeIds = new List<Int32>();
                MappedNodeIds = CurrentViewContext.lstMappedNodes.Select(x => x.AgencyNodeID).ToList();
                grdMappedNodes.DataSource = CurrentViewContext.lstMappedNodes;
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

        protected void grdMappedNodes_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.NodeId = Convert.ToInt32((e.Item as GridDataItem).GetDataKeyValue("AgencyHierarchyID"));


                if (Presenter.DeleteNodeMapping())
                {
                    eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Node Mapping deleted successfully.");
                    grdMappedNodes.Rebind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefreshHierarchyTree();", true);
                    eventShowCtr();
                }
                else
                {
                    eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, "Some error has occurred.Please try again.");
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
        #endregion

        #region Methods
        public void RebindControls(object sender)
        {
            try
            {
                grdMappedNodes.Rebind();
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

        protected void grdMappedNodes_RowDrop(object sender, GridDragDropEventArgs e)
        {
            try
            {
                if (CurrentViewContext.lstMappedNodes.IsNull())
                    Presenter.GetMappedNodes();

                if (String.IsNullOrEmpty(e.HtmlElement))
                {
                    if (e.DestDataItem != null && e.DestDataItem.OwnerGridID == grdMappedNodes.ClientID)
                    {
                        AgencyHierarchyDataContract selectedNode = new AgencyHierarchyDataContract();
                        var nodeList = CurrentViewContext.lstMappedNodes;
                        Int32 agencyHierarchyID = Convert.ToInt32(e.DestDataItem.GetDataKeyValue("AgencyHierarchyID"));
                        selectedNode = nodeList.Where(cond => cond.AgencyHierarchyID == agencyHierarchyID).FirstOrDefault();
                        Int32? destinationIndex = selectedNode.DisplayOrder;
                        List<AgencyHierarchyDataContract> nodesToMove = new List<AgencyHierarchyDataContract>();
                        List<AgencyHierarchyDataContract> shiftedNodes = null; 

                        foreach (GridDataItem draggedItem in e.DraggedItems)
                        {
                            Int32 draggedNodeId = Convert.ToInt32(draggedItem.GetDataKeyValue("AgencyHierarchyID"));
                            AgencyHierarchyDataContract tmpNodesList = nodeList.Where(cond => cond.AgencyHierarchyID == draggedNodeId).FirstOrDefault();
                            if (tmpNodesList != null)
                                nodesToMove.Add(tmpNodesList);
                        }
                        Int32? sourceIndex = nodesToMove.OrderByDescending(i => i.DisplayOrder).FirstOrDefault().DisplayOrder;
                        if (sourceIndex == destinationIndex)
                        {
                            shiftedNodes = nodeList.ToList();
                            if (shiftedNodes.IsNotNull())
                                nodesToMove = shiftedNodes;

                           Presenter.UpdateNodeDisplayOrder(nodesToMove, destinationIndex);
                           Presenter.GetMappedNodes();
                           nodeList = CurrentViewContext.lstMappedNodes;
                           destinationIndex = nodeList.Where(cond => cond.AgencyHierarchyID == agencyHierarchyID).FirstOrDefault().DisplayOrder;
                           nodesToMove = new List<AgencyHierarchyDataContract>();
                           foreach (GridDataItem draggedItem in e.DraggedItems)
                           {
                               Int32 draggedNodeId = Convert.ToInt32(draggedItem.GetDataKeyValue("AgencyHierarchyID"));
                               AgencyHierarchyDataContract tmpNodesList = nodeList.Where(cond => cond.AgencyHierarchyID == draggedNodeId).FirstOrDefault();                              
                               if (tmpNodesList != null)
                                   nodesToMove.Add(tmpNodesList);
                           }
                           sourceIndex = nodesToMove.OrderByDescending(i => i.DisplayOrder).FirstOrDefault().DisplayOrder;                         
                        }

                        AgencyHierarchyDataContract lastNodeToMove = nodesToMove.OrderByDescending(i => i.DisplayOrder).FirstOrDefault();                       
                        if (sourceIndex > destinationIndex)
                        {
                            shiftedNodes = nodeList.Where(obj => obj.DisplayOrder >= destinationIndex && obj.DisplayOrder < sourceIndex).ToList();
                            if (shiftedNodes.IsNotNull())
                                nodesToMove.AddRange(shiftedNodes);

                            // Update Sequence                            
                                Presenter.UpdateNodeDisplayOrder(nodesToMove, destinationIndex);
                        }
                        else if (sourceIndex < destinationIndex)
                        {
                            shiftedNodes = nodeList.Where(obj => obj.DisplayOrder <= destinationIndex && obj.DisplayOrder > sourceIndex).ToList();
                            if (shiftedNodes.IsNotNull())
                                shiftedNodes.AddRange(nodesToMove);
                            nodesToMove = shiftedNodes;
                            destinationIndex = sourceIndex;
                            // Update Sequence
                                Presenter.UpdateNodeDisplayOrder(nodesToMove, destinationIndex);
                        }
                        //Rebind grid and refresh tree
                        grdMappedNodes.Rebind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefreshHierarchyTree();", true);
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
    }
}