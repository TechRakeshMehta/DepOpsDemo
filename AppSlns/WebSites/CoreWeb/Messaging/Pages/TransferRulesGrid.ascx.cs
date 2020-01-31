using System;
using Microsoft.Practices.ObjectBuilder;
using CoreWeb.Shell;
using System.Collections.Generic;
using Entity;
using Telerik.Web.UI;
using INTSOF.Utils;


namespace CoreWeb.Messaging.Views
{
    public partial class TransferRulesGrid : BaseUserControl, ITransferRulesGridView
    {
        private TransferRulesGridPresenter _presenter=new TransferRulesGridPresenter();

        private Int32 _ruleId;
        private List<MessageRule> _messageRules;

        public List<MessageRule> MessageRules
        {
            get
            {
                return _messageRules;
            }
            set
            {
                _messageRules = value;
            }
        }

        public int CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public ITransferRulesGridView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 RuleId
        {
            get
            {
                return _ruleId;
            }
            set
            {
                _ruleId = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
        }

        
        public TransferRulesGridPresenter Presenter
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

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	


        /// <summary>
        /// Need Datasource event for the Rules grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdRules_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Presenter.GetMessageRules();
            grdRules.DataSource = CurrentViewContext.MessageRules;
        }

        /// <summary>
        /// Item command event for the Rules grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdRules_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    CurrentViewContext.RuleId = Convert.ToInt32((e.Item as GridDataItem).GetDataKeyValue("MessageRuleID"));
                    Presenter.DeleteMessageRules();
                    base.ShowSuccessMessage("Message rule has been deleted successfully.");
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
    }
}

