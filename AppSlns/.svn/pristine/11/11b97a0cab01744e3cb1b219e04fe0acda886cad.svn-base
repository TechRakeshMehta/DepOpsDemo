#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  RuleTemplateForm.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using System.Collections;

#endregion

#region Application Specific

using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;
using CoreWeb.CommonControls.Views;
using INTSOF.Utils.Consts;
using INTERSOFT.WEB.UI.WebControls;
//using Microsoft.Practices.CompositeWeb.Web.UI;
using Entity.ClientEntity;
using Business.RepoManagers;
using System.Threading;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class RuleTemplateForm : BaseUserControl, IRuleTemplateFormView
    {

        #region Private Variables

        private RuleTemplateFormPresenter _presenter = new RuleTemplateFormPresenter();
        private String _viewType;
        private Int32 _tenantid;
        private List<Int32> lstExpressionIds = new List<Int32>();

        #endregion

        #region Properties

        public RuleTemplateFormPresenter Presenter
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
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IRuleTemplateFormView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<lkpRuleType> RuleTypes
        {
            set
            {
                ddlRuleType.DataSource = value;
                ddlRuleType.DataBind();

                //Telerik.Web.UI.DropDownListItem itemSelect = new DropDownListItem();
                //itemSelect.Text = "--SELECT--";
                //ddlRuleType.Items.Insert(0, itemSelect);

                //ddlRuleType.Items.Insert(0, new RadComboBoxItem("Create New", string.Empty));
                //ddlRuleType.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
            }
        }

        public List<lkpRuleResultType> RuleResultTypes
        {
            set
            {
                ddlResultType.DataSource = value;
                ddlResultType.DataBind();
            }
        }

        public List<lkpRuleActionType> RuleActionTypes
        {
            set
            {
                ddlActionType.DataSource = value;
                ddlActionType.DataBind();
            }
        }

        public List<lkpExpressionOperator> ExpressionOperators { get; set; }

        public bool IsUIRule
        {
            get
            {
                return ddlRuleType.SelectedText.Contains("UI") ? true : false;
            }
        }

        public Int32 ObjectCount
        {
            get
            {
                return ntxtCountObject.Value != null ? (int)ntxtCountObject.Value : 0;
            }

        }

        public Entity.ComplianceRuleTemplate CurrentRuleTemplate { get; set; }

        public List<Entity.ComplianceRuleExpressionTemplate> ComplianceExpressionTemplates
        {
            get
            {
                return CurrentRuleTemplate.ComplianceRuleExpressionTemplates;
            }
        }

        /// <summary>
        /// ID of Current Rule Template (Blank if form in insert mode)
        /// </summary>
        public int RuleTemplateID { get; set; }

        /// <summary>
        /// Get or Set the Tenant ID
        /// </summary>
        public Int32 TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantid;
            }
            set { _tenantid = value; }
        }

        /// <summary>
        /// Returns the current logged-in user ID.
        /// </summary>
        public Int32 currentloggedinuserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        public List<Int32> ExpressionIds
        {
            get { return (List<Int32>)(ViewState["ExpressionIds"]); }
            set { ViewState["ExpressionIds"] = value; }
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        public Int32 SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(Session["SelectedTenantId"]);
            }
            set
            {
                Session["SelectedTenantId"] = value;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Presenter.OnViewLoaded();

            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();

                //
                //Get Rule Template ID?
                //

                //Loading Rule Template to work with
                Presenter.InitializeRuleTemplate();

                //Disabling default features on grid
                grdRuleExpressions.AllowFilteringByColumn = false;
                grdRuleExpressions.AllowPaging = false;
                grdRuleExpressions.MasterTableView.AllowPaging = false;
                grdRuleExpressions.AllowSorting = false;

                //Saving Current Rule Template for future postbacks
                if (CurrentRuleTemplate != null)
                {
                    ViewState["CurrentRuleTemplate"] = CurrentRuleTemplate;
                }

                if (CurrentViewContext.RuleTemplateID > 0)
                {
                    //Initializing controls with values
                    txtTemplateName.Text = CurrentRuleTemplate.RLT_Name;
                    ddlResultType.SelectedValue = CurrentRuleTemplate.RLT_ResultType.ToString();
                    ddlRuleType.SelectedValue = CurrentRuleTemplate.RLT_Type.ToString();
                    ddlActionType.SelectedValue = CurrentRuleTemplate.RLT_ActionType.ToString();
                    ntxtCountObject.Text = CurrentRuleTemplate.RLT_ObjectCount.ToString();
                    txtNotes.Text = CurrentRuleTemplate.RLT_Notes;
                    btnInitExprBuilder.Text = "Re-Initialize Rule Builder";
                    divExprBuilderBlock.Visible = true;
                }
            }
            else
            {
                //Retrieving Rule Template user is working on from viewstate
                CurrentRuleTemplate = ViewState["CurrentRuleTemplate"] as Entity.ComplianceRuleTemplate;

                int expr_count = CurrentRuleTemplate.ComplianceRuleExpressionTemplates.Count;
                for (int expr_index = 0; expr_index < expr_count; expr_index++)
                {
                    Repeater rExpression = grdRuleExpressions.MasterTableView.Items[expr_index].FindControl("repeatExprElements") as Repeater;
                    if (rExpression != null)
                    {
                        int elm_count = rExpression.Items.Count;
                        for (int elm_index = 0; elm_index < elm_count; elm_index++)
                        {
                            WclDropDownList ddlOperators = rExpression.Items[elm_index].FindControl("ddlEprOperators") as WclDropDownList;
                            if (ddlOperators != null)
                            {
                                CurrentRuleTemplate.ComplianceRuleExpressionTemplates[expr_index].RuleExpressionElements[elm_index].ElementValue =
                                    String.IsNullOrEmpty(ddlOperators.SelectedValue) ? ddlOperators.SelectedText : ddlOperators.SelectedValue;
                                CurrentRuleTemplate.ComplianceRuleExpressionTemplates[expr_index].RuleExpressionElements[elm_index].ElementOperator = ddlOperators.SelectedValue;
                            }
                        }
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Master Rule Templates";
                base.SetPageTitle("Master Rule Templates");

                int ruleTemplateID;

                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                if (!IsPostBack)
                {
                    Dictionary<String, String> args = new Dictionary<String, String>();

                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);
                        if (args.ContainsKey("RuleTemplateID"))
                        {
                            string templateID = Convert.ToString(args["RuleTemplateID"]);
                            if (!string.IsNullOrWhiteSpace(templateID))
                            {
                                Int32.TryParse(templateID, out ruleTemplateID);
                                CurrentViewContext.RuleTemplateID = ruleTemplateID;
                                //UAT-2047 Added Template CopyFunctionality
                                hdnCopiedTemplateID.Value = ruleTemplateID.ToString();
                                btnCopy.Enabled = true;
                            }
                        }

                        //Sets the SelectedTenantId value recieved from Rule Template screen.
                        //(Tenant ID for which Rule Template Form is to be open)
                        if (args.ContainsKey("SelectedTenantId"))
                        {
                            String selectedTenant = Convert.ToString(args["SelectedTenantId"]);
                            if (!String.IsNullOrWhiteSpace(selectedTenant))
                            {
                                Int32 selectedTenantId = 0;
                                Int32.TryParse(selectedTenant, out selectedTenantId);
                                CurrentViewContext.SelectedTenantId = selectedTenantId;
                                //UAT-2047 Added Template CopyFunctionality
                                hdnSelectedTenantID.Value = SelectedTenantId.ToString();
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

        protected void grdRuleExpressions_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                grdRuleExpressions.DataSource = CurrentViewContext.ComplianceExpressionTemplates;

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

        protected void grdRuleExpressions_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    GridDataItem item = e.Item as GridDataItem;
                    DatabindExprRepeater(item);
                    if (((Entity.ComplianceRuleExpressionTemplate)(e.Item.DataItem)).EX_Name == "(Group)")
                    {
                        ((GridDataItem)(e.Item))["DeleteColumn"].Controls[0].Visible = false;
                    }

                    Entity.ComplianceRuleExpressionTemplate data = CurrentRuleTemplate.ComplianceRuleExpressionTemplates[item.ItemIndex];
                    WclButton btnMinus = e.Item.FindControl("WclButtonMinus") as WclButton;

                    if (btnMinus != null && data.RuleExpressionElements.Count <= 1)
                    {
                        btnMinus.Enabled = false;
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

        protected void grdRuleExpressions_ItemCommand(object sender, GridCommandEventArgs e)
        {
            String eX_Name = String.Empty;

            try
            {
                if (e.CommandName == "CMD_INSERT_EXPRESSION")
                {
                    //Adding 'Gouping Expression' Expression
                    if (grdRuleExpressions.MasterTableView.Items.Count < 1)
                    {
                        Entity.ComplianceRuleExpressionTemplate _gexpr = new Entity.ComplianceRuleExpressionTemplate();
                        eX_Name = "(Group)";
                        _gexpr.EX_Name = eX_Name;
                        //_gexpr.ExpressionOrder = 
                        _gexpr.RuleExpressionElements.Add(new Entity.ComplianceRuleExpressionElement() { ElementValue = eX_Name, ElementOperator = eX_Name });
                        CurrentViewContext.ComplianceExpressionTemplates.Add(_gexpr);
                    }

                    string _eprName = "E" + (CurrentViewContext.ComplianceExpressionTemplates.Count).ToString();

                    Entity.ComplianceRuleExpressionTemplate _expr = new Entity.ComplianceRuleExpressionTemplate();
                    _expr.EX_Name = _eprName;
                    _expr.ExpressionOrder = (CurrentViewContext.ComplianceExpressionTemplates.Count);
                    _expr.RuleExpressionElements.Add(new Entity.ComplianceRuleExpressionElement() { ElementValue = _eprName, ElementOperator = _eprName });
                    CurrentViewContext.ComplianceExpressionTemplates.Insert(CurrentViewContext.ComplianceExpressionTemplates.Count - 1, _expr);
                    DataBindExprGrid();

                    //CurrentViewContext.ComplianceExpressionTemplates.Where(x => x.EX_Name == "(Group)")
                    //    .Select(x => x.ExpressionOrder = CurrentViewContext.ComplianceExpressionTemplates.Count + 1);

                    foreach (var x in CurrentViewContext.ComplianceExpressionTemplates.Where(x => x.EX_Name == "(Group)"))
                    {
                        x.ExpressionOrder = CurrentViewContext.ComplianceExpressionTemplates.Count;
                    }
                }
                if (e.CommandName == "CMD_PUSH_ELEMENT")
                {
                    GridDataItem item = e.Item as GridDataItem;

                    Entity.ComplianceRuleExpressionTemplate data = CurrentRuleTemplate.ComplianceRuleExpressionTemplates[item.ItemIndex];
                    if (data != null)
                    {
                        //UAT-1690: Bulk object additions on create rule template screen
                        WclNumericTextBox txtElementCount = e.Item.FindControl("txtElementCount") as WclNumericTextBox;
                        if (txtElementCount.IsNotNull() && !txtElementCount.Text.IsNullOrEmpty())
                        {
                            Int32 elementCount = Convert.ToInt32(txtElementCount.Text.Trim());

                            for (int index = 0; index < elementCount; index++)
                            {
                                data.RuleExpressionElements.Add(new Entity.ComplianceRuleExpressionElement());
                            }
                        }
                        else
                            data.RuleExpressionElements.Add(new Entity.ComplianceRuleExpressionElement());

                        DataBindExprGrid();
                    }
                }
                if (e.CommandName == "CMD_POP_ELEMENT")
                {
                    GridDataItem item = e.Item as GridDataItem;

                    Entity.ComplianceRuleExpressionTemplate data = CurrentRuleTemplate.ComplianceRuleExpressionTemplates[item.ItemIndex];
                    if (data != null)
                    {
                        WclButton btnMinus = e.Item.FindControl("WclButtonMinus") as WclButton;

                        if (btnMinus != null && data.RuleExpressionElements.Count <= 1)
                        {
                            btnMinus.Enabled = false;
                        }

                        if (data.RuleExpressionElements.Count > 1)
                        {
                            //UAT-1690: Bulk object additions on create rule template screen
                            WclNumericTextBox txtElementCount = e.Item.FindControl("txtElementCount") as WclNumericTextBox;
                            Int32 ruleExpElementCount = data.RuleExpressionElements.Count;

                            if (txtElementCount.IsNotNull() && !txtElementCount.Text.IsNullOrEmpty())
                            {
                                Int32 elementCount = Convert.ToInt32(txtElementCount.Text.Trim());

                                for (int index = 1; index <= elementCount; index++)
                                {
                                    if (ruleExpElementCount > index)
                                        data.RuleExpressionElements.RemoveAt(ruleExpElementCount - index);
                                }
                            }
                            else
                                data.RuleExpressionElements.RemoveAt(data.RuleExpressionElements.Count - 1);

                            DataBindExprGrid();
                        }
                    }
                }
                if (e.CommandName == "Delete")
                {
                    GridDataItem item = e.Item as GridDataItem;
                    int indexCounter = 0;
                    //Initialize and get list of ExpressionIds which are deleted from grid
                    ExpressionIds = ViewState["ExpressionIds"] == null ? new List<Int32>() : (List<Int32>)(ViewState["ExpressionIds"]);

                    var complianceRuleExpressionTemplates = CurrentRuleTemplate.ComplianceRuleExpressionTemplates.ElementAt(item.ItemIndex);

                    if (complianceRuleExpressionTemplates.IsNotNull() && complianceRuleExpressionTemplates.EX_ID > 0)
                    {
                        ExpressionIds.Add(complianceRuleExpressionTemplates.EX_ID);
                    }

                    CurrentRuleTemplate.ComplianceRuleExpressionTemplates.RemoveAt(item.ItemIndex);
                    for (indexCounter = 0; indexCounter < CurrentRuleTemplate.ComplianceRuleExpressionTemplates.Count - 1; indexCounter++)
                    {
                        CurrentRuleTemplate.ComplianceRuleExpressionTemplates[indexCounter].EX_Name = "E" + (indexCounter + 1).ToString();
                        CurrentRuleTemplate.ComplianceRuleExpressionTemplates[indexCounter].ExpressionOrder = (indexCounter + 1);
                    }

                    CurrentRuleTemplate.ComplianceRuleExpressionTemplates[indexCounter].EX_Name = "(Group)";
                    CurrentRuleTemplate.ComplianceRuleExpressionTemplates[indexCounter].ExpressionOrder = indexCounter + 1;
                    DataBindExprGrid();

                    ViewState["ExpressionIds"] = ExpressionIds;
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

        protected void cmdBarCancelSave_Click(object sender, EventArgs e)
        {
            try
            {
                RoutePageBack(false);
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
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

        protected void cmdBarSave_Click(object sender, EventArgs e)
        {
            try
            {
                //
                //Parse required dropdown values
                //
                int resultType;
                Int32.TryParse(ddlResultType.SelectedValue, out resultType);

                int actionType;
                Int32.TryParse(ddlActionType.SelectedValue, out actionType);

                int ruleType;
                Int32.TryParse(ddlRuleType.SelectedValue, out ruleType);

                int objectCount;
                Int32.TryParse(ntxtCountObject.Text, out objectCount);

                CurrentRuleTemplate.RLT_Name = txtTemplateName.Text;
                CurrentRuleTemplate.RLT_Notes = txtNotes.Text;
                CurrentRuleTemplate.RLT_Description = "Description";
                CurrentRuleTemplate.RLT_ResultType = resultType;
                CurrentRuleTemplate.RLT_IsActive = true;
                CurrentRuleTemplate.RLT_IsDeleted = false;
                CurrentRuleTemplate.RLT_CreatedByID = base.CurrentUserId;
                CurrentRuleTemplate.RLT_CreatedOn = DateTime.Now;
                CurrentRuleTemplate.RLT_ActionType = actionType;
                CurrentRuleTemplate.RLT_Type = ruleType;

                CurrentRuleTemplate.RLT_ObjectCount = objectCount;

                CurrentRuleTemplate.RLT_ModifiedByID = base.CurrentUserId;
                CurrentRuleTemplate.RLT_ModifiedOn = DateTime.Now;

                var xmlOutputString = Presenter.ValidateRuleTemplate(resultType);
                if (xmlOutputString.Status == 0)
                {
                    CurrentRuleTemplate.RuleGroupExpression = xmlOutputString.UIExpressionLabel;

                    if (Presenter.SaveRuleTemplate())
                    {
                        txtValidationResult.Text = "Success: " + xmlOutputString.UIExpressionLabel;
                        //txtValidationResult.ForeColor = System.Drawing.Color.Green;
                        if (CurrentRuleTemplate.RLT_ID != null && CurrentRuleTemplate.RLT_ID != AppConsts.NONE)
                        {
                            RoutePageBack(true, false);
                        }
                        else
                        {
                            RoutePageBack(true, true);
                        }
                    }
                }
                else
                {
                    txtValidationResult.Text = "Error: " + xmlOutputString.ErrorMessage;
                    //txtValidationResult.ForeColor = System.Drawing.Color.Red;
                    //btnSaveTemplate.Enabled = false;
                }
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
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

        protected void btnInitExprBuilder_click(object sender, EventArgs e)
        {

            DataBindExprGrid();

            divExprBuilderBlock.Visible = true;
            btnInitExprBuilder.Text = "Re-Initialize Rule Builder";

        }

        protected void btnExprValidator_click(object sender, EventArgs e)
        {
            //Code to validate expressions
            var xmlOutputString = Presenter.ValidateRuleTemplate(Convert.ToInt32(ddlResultType.SelectedValue));

            if (xmlOutputString.Status == 0)
            {
                txtValidationResult.Text = "Success: " + xmlOutputString.UIExpressionLabel;
                //txtValidationResult.ForeColor = System.Drawing.Color.Blue;                

                //Restricts the user to save any change if RuleTemplate is associated with any Rule.
                if (CurrentRuleTemplate.IsRuleTemplateAssociatedWithRule)
                {
                    btnSaveTemplate.Enabled = false;
                }
                //Validation success then enable save button
                else
                {
                    btnSaveTemplate.Enabled = true;
                }

                btnExprValidator.Text = "Re-validate Expressions";
            }
            else
            {
                txtValidationResult.Text = "Error: " + xmlOutputString.ErrorMessage;
                //txtValidationResult.ForeColor = System.Drawing.Color.Red;
                //Validation fail then disable save button
                btnSaveTemplate.Enabled = false;
                //btnExprValidator.Text = "Re-validate Expressions";
            }

            divValidationResult.Visible = true;
        }

        protected void repeatExprElements_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Entity.ComplianceRuleExpressionElement element = e.Item.DataItem as Entity.ComplianceRuleExpressionElement;

                WclDropDownList ddlElements = e.Item.FindControl("ddlEprOperators") as WclDropDownList;
                if (ddlElements != null)
                {
                    ddlElements.DataSource = ExpressionOperators;
                    ddlElements.DataBind();
                    //ddlElements.SelectedText = element.ElementValue;
                    ddlElements.SelectedValue = element.ElementValue;
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

        #region Methods

        private void DataBindExprGrid()
        {
            if (!grdRuleExpressions.DataSourceIsAssigned)
            {
                grdRuleExpressions.DataSource = CurrentViewContext.ComplianceExpressionTemplates;
                grdRuleExpressions.DataBind();
            }
            else
            {
                grdRuleExpressions.Rebind();
            }
        }

        private void DatabindExprRepeater(GridDataItem item)
        {

            if (item != null)
            {
                Entity.ComplianceRuleExpressionTemplate data = item.DataItem as Entity.ComplianceRuleExpressionTemplate;
                Repeater exprBuilder = item.FindControl("repeatExprElements") as Repeater;
                if (exprBuilder != null)
                {
                    Presenter.CreateElementList(item.ItemIndex);
                    exprBuilder.DataSource = data.RuleExpressionElements;
                    exprBuilder.DataBind();
                }
            }

        }

        private void RoutePageBack(bool showSuccessMessage, bool isSaveRequest = false)
        {

            Dictionary<String, String> queryString = new Dictionary<String, String>();
            if (showSuccessMessage)
            {
                queryString.Add("tplname", CurrentRuleTemplate.RLT_Name);
                queryString.Add("isSaveRequest", Convert.ToString(isSaveRequest));
            }
            queryString.Add("Child", @"SetupRuleTemplate.ascx");
            queryString.Add("SelectedTenantId", Convert.ToString(SelectedTenantId));
            string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Session["SelectedTenantId"] = null;
            Response.Redirect(url, true);
        }

        #endregion

    }
}

