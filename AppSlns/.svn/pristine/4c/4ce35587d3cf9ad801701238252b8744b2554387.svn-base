<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.RuleTemplateForm" CodeBehind="RuleTemplateForm.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div class="section">
    <h1 class="mhdr">Rule Template Components</h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel CssClass="sxpnl" runat="server" ID="pnlRuleForm">
                <div class='sxro sx4co'>
                    <div class='sxlb'>
                        <span class="cptn">Rule Template Name</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtTemplateName" MaxLength="100">
                        </infs:WclTextBox>
                        <div class='vldx'>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTemplateName" ID="vldtxtTemplateName"
                                class="errmsg" ValidationGroup="vgrpComponent" Display="Dynamic" ErrorMessage="Rule Template Name is required" />
                        </div>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Rule Type</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclDropDownList ID="ddlRuleType" runat="server" DataTextField="RLT_Description"
                            DataValueField="RLT_ID">
                        </infs:WclDropDownList>
                        <div class='vldx'>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlRuleType" ID="vldddlRuleType"
                                class="errmsg" ValidationGroup="vgrpComponent" Display="Dynamic" ErrorMessage="Rule Type is required." InitialValue="--SELECT--" />
                        </div>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Result Type</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclDropDownList ID="ddlResultType" runat="server" DataTextField="RSL_Description"
                            DataValueField="RSL_ID">
                        </infs:WclDropDownList>
                        <div class='vldx'>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlResultType" ID="vldddlResultType"
                                class="errmsg" ValidationGroup="vgrpComponent" Display="Dynamic" ErrorMessage="Result Type is required." InitialValue="0" />
                        </div>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Action Type</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclDropDownList ID="ddlActionType" runat="server" DataTextField="ACT_Description"
                            DataValueField="ACT_ID">
                        </infs:WclDropDownList>
                        <div class='vldx'>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlActionType" ID="vldddlActionType"
                                class="errmsg" ValidationGroup="vgrpComponent" Display="Dynamic" ErrorMessage="Action Type is required." InitialValue="0" />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx4co'>
                    <div class='sxlb'>
                        <span class="cptn">Object Count</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="ntxtCountObject" MinValue="1" MaxLength="2"
                            runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number">
                            <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                        </infs:WclNumericTextBox>
                        <div class='vldx'>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ntxtCountObject" ID="vldntxtCountObject"
                                class="errmsg" ValidationGroup="vgrpComponent" Display="Dynamic" ErrorMessage="Object Count is required." />
                        </div>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Note</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtNotes" MaxLength="2048">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
    <infsu:CommandBar ID="cmdBarInitBuilder" runat="server" ButtonPosition="Right">
        <ExtraCommandButtons>
            <infs:WclButton runat="server" ID="btnInitExprBuilder" Text="Initialize Rule Builder"
                SingleClick="true" SingleClickText="Please Wait" ValidationGroup="vgrpComponent"
                OnClick="btnInitExprBuilder_click">
                <%-- <Icon PrimaryIconCssClass="" /> --%>
            </infs:WclButton>
        </ExtraCommandButtons>
    </infsu:CommandBar>
</div>
<br />
<div class="section" id="divExprBuilderBlock" visible="false" runat="server">
    <h1 class="mhdr">Rule Template Expressions</h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdRuleExpressions" AutoGenerateColumns="False"
                AutoSkinMode="True" CellSpacing="0" EnableDefaultFeatures="true" ShowAllExportButtons="false"
                ShowExtraButtons="true" OnNeedDataSource="grdRuleExpressions_NeedDataSource" ShowClearFiltersButton="false"
                OnItemDataBound="grdRuleExpressions_ItemDataBound" OnItemCommand="grdRuleExpressions_ItemCommand">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <ExtraCommandButtons>
                    <infs:WclButton runat="server" ID="btnAddExpr" Text="Add Expression" CommandName="CMD_INSERT_EXPRESSION">
                        <Icon PrimaryIconCssClass="rbAdd" />
                    </infs:WclButton>
                </ExtraCommandButtons>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="" AllowPaging="True">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="EX_Name" AllowFiltering="false" HeaderText="Name"
                            SortExpression="EX_Name" UniqueName="EX_Name">
                            <HeaderStyle Width="50px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="EX_Expression" AllowFiltering="false" HeaderText="Expression"
                            SortExpression="EX_Expression" UniqueName="EX_Expression">
                            <ItemTemplate>
                                <asp:Repeater runat="server" ID="repeatExprElements" OnItemDataBound="repeatExprElements_ItemDataBound">
                                    <ItemTemplate>
                                        <infs:WclDropDownList runat="server" ID="ddlEprOperators" DataTextField="EO_UILabel"
                                            DataValueField="EO_SQL" Width="100px" DropDownHeight="100px" DropDownWidth="110px">
                                        </infs:WclDropDownList>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <infs:WclNumericTextBox ID="txtElementCount" runat="server" MinValue="1" MaxValue="99" Type="Number" Width="30px">
                                    <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                </infs:WclNumericTextBox>
                                <infs:WclButton ID="WclButtonAdd" runat="server" ButtonType="LinkButton" BorderStyle="None"
                                    Width="14px" ToolTip="Add New Element" CommandName="CMD_PUSH_ELEMENT" BackColor="Transparent">
                                    <Icon PrimaryIconCssClass="rbAdd" />
                                </infs:WclButton>
                                <infs:WclButton ID="WclButtonMinus" runat="server" ButtonType="LinkButton" BorderStyle="None"
                                    Width="14px" ToolTip="Remove Last Element" CommandName="CMD_POP_ELEMENT" BackColor="Transparent">
                                    <Icon PrimaryIconCssClass="icnminus" />
                                </infs:WclButton>
                            </ItemTemplate>
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this record?"
                            Text="Delete Expression" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" Width="30px" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                    </EditFormSettings>
                    <NoRecordsTemplate>
                        Please click <span style='font-weight: bold'>Add Expression</span> button from the
                        right to create a new rule expression.
                    </NoRecordsTemplate>
                </MasterTableView>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
        <div class="auto" runat="server" id="divValidationResult" style="margin-top: 10px"
            visible="false">
            <span>Validation Result</span>
            <infs:WclTextBox runat="server" ID="txtValidationResult" Height="50px" TextMode="MultiLine"
                ReadOnly="true">
            </infs:WclTextBox>
        </div>
    </div>
    <infsu:CommandBar ID="cmdBarValidations" runat="server" ButtonPosition="Right">
        <ExtraCommandButtons>
            <infs:WclButton runat="server" ID="btnExprValidator" Text="Validate Expressions"
                SingleClick="true" SingleClickText="Please Wait" OnClick="btnExprValidator_click">
                <Icon PrimaryIconCssClass="rbOk" />
            </infs:WclButton>
        </ExtraCommandButtons>
    </infsu:CommandBar>
</div>
<infsu:CommandBar ID="fsucCmdBar" runat="server" ButtonPosition="Center">
    <ExtraCommandButtons>
        <infs:WclButton runat="server" ID="btnSaveTemplate" Text="Save Rule Template" ValidationGroup="vgrpComponent" OnClick="cmdBarSave_Click" Enabled="false">
            <Icon PrimaryIconCssClass="rbSave" />
        </infs:WclButton>
        <infs:WclButton runat="server" ID="btnCancelSave" Text="Cancel" OnClick="cmdBarCancelSave_Click">
            <Icon PrimaryIconCssClass="rbCancel" />
        </infs:WclButton>
        <infs:WclButton runat="server" ID="btnCopy" Text="Copy" Enabled="false" OnClientClicked="openPopUp">
        </infs:WclButton>
    </ExtraCommandButtons>
</infsu:CommandBar>
<script type="text/javascript">

    function openPopUp()
    {
        var ruleTemplateID = $jQuery("[id$=hdnCopiedTemplateID]")[0].value;
        var selectedTenantId = $jQuery("[id$=hdnSelectedTenantID]")[0].value;
        var TemplateCopyScreenWindowName = "RuleTemplateCopyScreen";
        ResetTimer();
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (80 / 100);

        var url = $page.url.create("~/ComplianceAdministration/Pages/RuleTemplateCopy.aspx?TenantID=" + selectedTenantId + "&RuleTemplateID=" + ruleTemplateID);
        var win = $window.createPopup(url, { size: "800,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: TemplateCopyScreenWindowName, onclose: OnClientClose });
        winopen = true;
    }
    function ResetTimer()
    {
        var hdntimeout = $jQuery('[id$=hdntimeout]');
        if (hdntimeout != null) {
            var timeout = hdntimeout.val();
            parent.StartCountDown(timeout);
        }
    }
    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        //if (winopen) {
        //   }
        winopen = false;
    } 
</script>
<asp:HiddenField ID="hdnClosePopUp" runat="server" />
<asp:HiddenField ID="hdnCopiedTemplateID" runat="server" Value="" />
<asp:HiddenField ID="hdnSelectedTenantID" runat="server" Value="" />
<div style="height: 50px"></div>
