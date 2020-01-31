<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.ShotSeriesRuleList"
    Title="RuleList" MasterPageFile="~/Shared/ChildPage.master" CodeBehind="ShotSeriesRuleList.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagName="ExpressionObject" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/ShotSeriesExpressionObject.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/mapping_pages.css" ResourceType="StyleSheet" />
    </infs:WclResourceManagerProxy>
    <script type="text/javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
        });

        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoaded);

        function pageLoaded(sender, args) {
            var updatedPanels = args.get_panelsUpdated();
            // check if Main Panel was updated 
            for (idx = 0; idx < updatedPanels.length; idx++) {
                if (updatedPanels[idx].id == "<%=UpdatePanel1.ClientID %>") {
                    parent.ResetTimer();
                    break;
                }
            }
        }
    </script>
    <div class="page_cmd">
        <infs:WclButton runat="server" ID="btnAdd" Text="+ Add a Rule" OnClick="btnAdd_Click"
            Height="30px" ButtonType="LinkButton">
        </infs:WclButton>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="msgbox" id="divSuccessMsg">
                <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
            </div>
            <div class="section" id="divAddForm" runat="server" visible="false">
                <h1 class="mhdr">Add Rule</h1>
                <div class="content">
                    <div class="sxform ">
                        <div class="msgbox">
                            <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                        </div>
                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPackage">
                            <div class="sxgrp auto" id="divSelect" runat="server" visible="true">

                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Apply Rule On</span><span class="reqd">*</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclComboBox ID="ddlObjectType" runat="server" AutoPostBack="true" ToolTip="Select from given list"
                                            OnSelectedIndexChanged="ddlObjectType_SelectedIndexChanged">
                                            <Items>
                                            </Items>
                                        </infs:WclComboBox>
                                        <div class='vldx'>
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlObjectType"
                                                ID="RequiredFieldValidator1" class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic"
                                                ErrorMessage="Please select object type." InitialValue="--SELECT--" />
                                        </div>
                                    </div>
                                    <div id="dvSeriesItem" runat="server" visible="false">
                                        <div class='sxlb'>
                                            <span class="cptn">Series Items</span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclComboBox ID="cmbSeriesItems" runat="server" EmptyMessage="--SELECT--" CheckBoxes="true" Height="100px">
                                            </infs:WclComboBox>
                                            <div class="vldx">
                                                <asp:RequiredFieldValidator ID="rvSeriesItem" runat="server" ControlToValidate="cmbSeriesItems"
                                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please select Series Item(s)">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Select a Template</span><span class="reqd">*</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclComboBox ID="cmbMasterTemplates" runat="server" AutoPostBack="true" ToolTip="Select from a master list"
                                            OnSelectedIndexChanged="cmbMasterTemplates_SelectedIndexChanged">
                                            <Items>
                                            </Items>
                                        </infs:WclComboBox>
                                        <div class='vldx'>
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="cmbMasterTemplates"
                                                ID="rfvTemplates" class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic"
                                                ErrorMessage="Please select template." InitialValue="--SELECT--" />
                                        </div>
                                    </div>
                                    <div class='sxlm'>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Rule Name</span><span class="reqd">*</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtRuleName" MaxLength="100">
                                        </infs:WclTextBox>
                                        <div class='vldx'>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtRuleName"
                                                class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Rule Name is required." />
                                        </div>
                                    </div>
                                    <div class='sxlb'>
                                        <span class="cptn">Rule Action</span>
                                    </div>
                                    <div class='sxlm'>
                                        <div class='ronly'>
                                            <asp:Label ID="lblAction" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                    <div class='sxlb'>
                                        <asp:Label Text="Select Rule" runat="server" ID="lblActionMapping" CssClass="cptn" />
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtActionMapping">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Is Active</span>
                                    </div>
                                    <div class='sxlm'>
                                        <uc1:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="true" IsAutoPostBack="false" />
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Specify Error Message</span>
                                    </div>
                                    <div class='sxlm m3spn'>
                                        <infs:WclTextBox runat="server" ID="txtErrorMessage">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Specify Sucess Message</span>
                                    </div>
                                    <div class='sxlm m3spn'>
                                        <infs:WclTextBox runat="server" ID="txtSucessMessage">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="sxgrp" runat="server" id="divCreate" visible="true">
                                <div id="divCateogry">
                                    <%--Conditional Category Rule Demonstration --%>
                                    <div class="sxro">
                                        <h1 class="shdr">Rule Expressions</h1>
                                        <pre>
                                            <asp:Literal ID="litExpression" runat="server"></asp:Literal>
                                    </pre>
                                    </div>
                                    <h1 class="shdr">Map Expression Objects</h1>
                                    <asp:Panel ID="pnlExpressionObjects" runat="server">
                                    </asp:Panel>
                                </div>
                            </div>
                            <div style="padding: 5px; text-align: right">
                                <infs:WclButton runat="server" ID="btnValidate" Text="Validate Rule" OnClick="btnValidate_Click">
                                </infs:WclButton>
                            </div>
                            <div class='sxro sx3co monly auto'>
                                <div class='sxlb'>
                                    <span class="cptn">Validation Result</span>
                                </div>
                                <infs:WclTextBox runat="server" ID="txtValidationResult" Height="50px" ReadOnly="true"
                                    TextMode="MultiLine">
                                </infs:WclTextBox>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div style="padding: 5px; text-align: right">
                                <infs:WclDropDownList ID="ddlTestUsersList" runat="server" Enabled="false" DefaultMessage="Select Test Users">
                                    <Items>
                                        <telerik:DropDownListItem Text="Test User1" Value="1" />
                                        <telerik:DropDownListItem Text="Test User2" Value="2" />
                                    </Items>
                                </infs:WclDropDownList>
                                <infs:WclButton runat="server" ID="btn" Text="Test Rule" Enabled="false">
                                </infs:WclButton>
                            </div>
                            <div class='sxro sx3co monly auto'>
                                <div class='sxlb'>
                                    <span class="cptn">Test Result</span>
                                </div>
                                <infs:WclTextBox runat="server" ID="WclTextBox1" Height="50px" ReadOnly="true" TextMode="MultiLine">
                                </infs:WclTextBox>
                                <div class='sxroend'>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <infsu:CommandBar ID="cmdBarSaveRule" runat="server" ValidationGroup="grpFormSubmit"
                        DefaultPanel="pnlPackage" DisplayButtons="Save,Cancel" AutoPostbackButtons="Save,Cancel"
                        OnSaveClick="cmdBarSaveRule_SaveClick" OnCancelClick="cmdBarSaveRule_CancelClick" />
                </div>
            </div>
            <asp:HiddenField ID="hdnObjectCount" runat="server" Value="0" />
            <div class="section">
                <h1 class="mhdr">Rules</h1>
                <div class="content">
                    <div class="swrap">
                        <infs:WclGrid runat="server" ID="grdRules" AllowPaging="True" OnItemCommand="grdRules_ItemCommand"
                            PageSize="10" AutoGenerateColumns="False" AllowSorting="True" GridLines="Both"
                            EnableDefaultFeatures="true" OnNeedDataSource="grdRules_NeedDataSource">
                            <ClientSettings EnableRowHoverStyle="true">
                                <Selecting AllowRowSelect="true"></Selecting>
                            </ClientSettings>
                            <MasterTableView CommandItemDisplay="Top" DataKeyNames="RLM_ID">
                                <CommandItemSettings ShowAddNewRecordButton="false" />
                                <Columns>
                                    <telerik:GridBoundColumn HeaderText="Rule Name" DataField="RLM_Name">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Rule Type" DataField="RuleTemplate.lkpRuleType.RLT_Description">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Return Type" DataField="RuleTemplate.lkpRuleResultType.RSL_Description">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Action Type" DataField="RuleTemplate.lkpRuleActionType.ACT_Description">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn DataField="RLM_IsActive" FilterControlAltText="Filter IsActive column"
                                        HeaderText="Is Active" SortExpression="RLM_IsActive" UniqueName="RLM_IsActive">
                                        <ItemTemplate>
                                            <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("RLM_IsActive"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="RLM_IsCurrent" FilterControlAltText="Filter IsCurrent column"
                                        HeaderText="Is Current" SortExpression="RLM_IsCurrent" UniqueName="RLM_IsCurrent">
                                        <ItemTemplate>
                                            <asp:Label ID="IsCurrent" runat="server" Text='<%# Convert.ToBoolean(Eval("RLM_IsCurrent"))==true ?Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <%--        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                                <HeaderStyle CssClass="tplcohdr" />
                                <ItemStyle CssClass="MyImageButton" />
                            </telerik:GridEditCommandColumn>--%>
                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"
                                        Text="Delete" UniqueName="DeleteColumn">
                                        <HeaderStyle CssClass="tplcohdr" />
                                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                                    </telerik:GridButtonColumn>
                                </Columns>
                                <EditFormSettings EditFormType="Template">
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                    <FormTemplate>
                                        <!-- enter edit form here or change edit form type to UserControl or Popup -->
                                    </FormTemplate>
                                </EditFormSettings>
                                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                            </MasterTableView>
                        </infs:WclGrid>
                    </div>
                    <div class="gclr">
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
