<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.RuleList"
    Title="RuleList" MasterPageFile="~/Shared/ChildPage.master" CodeBehind="RuleList.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagName="ExpressionObject" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/ExpressionObject.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .buttonHidden {
            display: none;
        }
    </style>
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
        function RefreshRuleGrid() {

            var btn = $jQuery('[id$=btnRuleDeletionDoPostBack]');
            btn.click();
        };
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
        function openRuleAssociationPopUp(sender) {

            var btnID = sender.id;
            var containerID = btnID.substr(0, btnID.indexOf("lnkRuleDelete"));
            var hdnfRuleMappingId = $jQuery("[id$=" + containerID + "hdnfRuleMappingId]").val();
            var hdnfRuleSetId = $jQuery("[id$=" + containerID + "hdnfRuleSetId]").val();
            var hdnfCategoryId = $jQuery("[id$=hdnfCategoryId]").val();
            var hdnfAttributeId = $jQuery("[id$=hdnfAttributeId]").val();
            var hdnfitemId = $jQuery("[id$=hdnfitemId]").val();
            var hdnfSelectedtenantId = $jQuery("[id$=hdnfSelectedtenantId]").val();
            var hdnfPackageId = $jQuery("[id$=hdnfPackageId]").val();


            var documentType = "ServiceFormDocument";
            var composeScreenWindowName = "Rule Association With Packages";

            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceAdministration/Pages/RuleAssociationViewer.aspx?RuleMappingId=" + hdnfRuleMappingId + "&RuleSetId=" + hdnfRuleSetId + "&CategoryId=" + hdnfCategoryId + "&SelectedTenantId=" + hdnfSelectedtenantId + "&PackageId=" + hdnfPackageId + "&ItemId=" + hdnfitemId + "&AttributeId=" + hdnfAttributeId);
            var win = $window.createPopup(url, {
                size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close |
                Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose
            });
            winopen = true;
            return false;
        }
        function OnClientClose(oWnd, args) {
        
            oWnd.get_contentFrame().src = ''; //This is added for fixing pop-up close issue in Safari browser.
            oWnd.remove_close(OnClientClose);
            if (winopen) {
                winopen = false;
            }
            if (args != null && args._argument != null) {
                RefrshTree();
                RefreshRuleGrid();
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
                                        <span class="cptn">Select a Template</span><span class="reqd">*</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclComboBox ID="cmbMasterTemplates" runat="server" AutoPostBack="true" ToolTip="Select from a master list"
                                            OnSelectedIndexChanged="cmbMasterTemplates_SelectedIndexChanged">
                                            <Items>
                                                <%--  <telerik:RadComboBoxItem Text="Create New" Value="0" />
                                                  <telerik:RadComboBoxItem Text="MMR Rule Template" Value="1" />
                                                <telerik:RadComboBoxItem Text="PPD Rule Template" Value="2" />
                                                <telerik:RadComboBoxItem Text="30 Days Gap Template" Value="3" />--%>
                                            </Items>
                                        </infs:WclComboBox>
                                        <div class='vldx'>
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="cmbMasterTemplates"
                                                ID="rfvTemplates" class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic"
                                                ErrorMessage="Please select template." InitialValue="--SELECT--" />
                                        </div>
                                    </div>
                                    <div class='sxlm'>
                                        <%--Visible only if Create New selected from above combo
                                        <infs:WclButton runat="server" ID="btnCreate" Text="Create New" Visible="true">
                                            <Icon PrimaryIconCssClass="rbAdd" />
                                        </infs:WclButton>--%>
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
                                        <%--<infs:WclButton runat="server" ID="chkActive" ToggleType="CheckBox" ButtonType="ToggleButton"
                                            AutoPostBack="false">
                                            <ToggleStates>
                                                <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                <telerik:RadButtonToggleState Text="No" Value="False" />
                                            </ToggleStates>
                                        </infs:WclButton>--%>
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
                            <div class='sxro sx3co' id="dvUsergroup" runat="server">
                                <div class='sxlb'>
                                    <span class="cptn">Applies To</span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclComboBox ID="cmbUserGroup" runat="server" EmptyMessage="--SELECT--" CheckBoxes="true" Height="100px">
                                    </infs:WclComboBox>
                                    <div id="dvUserGroupVldx" class='vldx'>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvUserGroup" ControlToValidate="cmbUserGroup"
                                            class="errmsg" ErrorMessage="Applies To is required." Display="Dynamic" ValidationGroup='grpFormSubmit' />
                                    </div>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co' id="dvSharedInstance" runat="server">
                                <div class='sxlb'>
                                    <span class="cptn">Also Add To</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclComboBox ID="cmbShareBetween" runat="server" EnableCheckAllItemsCheckBox="true" EmptyMessage="--SELECT--" CheckBoxes="true" Height="100px">
                                    </infs:WclComboBox>
                                </div>
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
                        <%-- OnItemDataBound ="grdRules_ItemDataBound" --%>
                        <infs:WclGrid runat="server" ID="grdRules" AllowPaging="True" OnItemCommand="grdRules_ItemCommand" OnItemCreated="grdRules_ItemCreated"
                            PageSize="10" AutoGenerateColumns="False" AllowSorting="True" GridLines="Both"
                            EnableDefaultFeatures="true" OnNeedDataSource="grdRules_NeedDataSource">
                            <ClientSettings EnableRowHoverStyle="true">
                                <Selecting AllowRowSelect="true"></Selecting>
                            </ClientSettings>
                            <MasterTableView CommandItemDisplay="Top" DataKeyNames="RLM_ID,RLM_RuleSetID,IsRuleAssociationExists">
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
                                    <telerik:GridTemplateColumn UniqueName="DeleteRuleAssociation" AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkRuleDelete" runat="server" Text="Delete Rule Association" OnClientClick="openRuleAssociationPopUp(this)" ForeColor="Blue" />
                                            <asp:HiddenField ID="hdnfRuleMappingId" runat="server" Value='<%#Eval("RLM_ID")%>' />
                                            <asp:HiddenField ID="hdnfRuleSetId" runat="server" Value='<%#Eval("RLM_RuleSetID")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
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

                    <asp:HiddenField ID="hdnfCategoryId" runat="server" Value="" />
                    <asp:HiddenField ID="hdnfSelectedtenantId" runat="server" Value="" />
                    <asp:HiddenField ID="hdnfPackageId" runat="server" Value="" />
                    <asp:HiddenField ID="hdnfitemId" runat="server" Value="" />
                    <asp:HiddenField ID="hdnfAttributeId" runat="server" Value="" />
                    <div class="gclr">
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="btnRuleDeletionDoPostBack" OnClick="btnRuleDeletionDoPostBack_Click" runat="server" CssClass="buttonHidden" />
</asp:Content>
