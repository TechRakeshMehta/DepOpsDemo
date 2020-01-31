using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.BkgOperations.Views;
using INTSOF.Utils;

namespace CoreWeb.BkgOperations.Views
{
    public partial class OrderApprovalQueue : BaseUserControl, IOrderApprovalQueueView
    {

        protected override void OnInit(EventArgs e)
        {
            try
            {
               
                base.OnInit(e);
                base.Title = "Order Queue";
             
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
            if (!IsPostBack)
            {
                ApplyActionLevelPermission(ActionCollection, "Order Approval Queue");
            }
        }

        #region Apply Permission
        /// <summary>
        /// Add the action permissions 
        /// </summary>
        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                Entity.ClientEntity.ClsFeatureAction objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "ViewDetail";
                objClsFeatureAction.CustomActionLabel = "View Detail";
                objClsFeatureAction.ScreenName = "Order Approval Queue";
                actionCollection.Add(objClsFeatureAction);

                //actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                //{
                //    CustomActionId = "SubmitOrder",
                //    CustomActionLabel = "Submit Order",
                //    ScreenName = "Order Payment Detail"
                //});
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Next",
                    CustomActionLabel = "Next",
                    ScreenName = "Order Payment Detail"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "ApprovePayment",
                    CustomActionLabel = "Approve Payment",
                    ScreenName = "Order Payment Detail"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "RejectPayment",
                    CustomActionLabel = "Reject Payment",
                    ScreenName = "Order Payment Detail"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "RejectCancellation",
                    CustomActionLabel = "Reject Cancellation",
                    ScreenName = "Order Payment Detail"
                });

                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "ApprovedCancellation",
                    CustomActionLabel = "Approved Cancellation",
                    ScreenName = "Order Payment Detail"
                });
                return actionCollection;
            }
        }

        /// <summary>
        /// Set action level permissions
        /// </summary>
        /// <param name="ctrlCollection">ctrlCollection</param>
        /// <param name="screenName">screenName</param>
        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
        }
        #endregion

    }
}