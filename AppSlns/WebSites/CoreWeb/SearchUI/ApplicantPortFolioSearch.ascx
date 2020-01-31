<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.Search.Views.ApplicantPortFolioSearch" CodeBehind="ApplicantPortFolioSearch.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%--<%@ Register Src="~/ComplianceAdministration/UserControl/CustomAttributeLoaderSearch.ascx"
    TagName="CustomAttributeLoaderSearch" TagPrefix="uc" %>--%>
<%@ Register Src="~/ComplianceAdministration/UserControl/CustomAttributeLoaderSearchMultipleNodes.ascx"
    TagName="CustomAttributeLoaderSearch" TagPrefix="uc" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxPortFolioSearch">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/KeyBoardSupport.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/main-accessibility.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Main-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Grid-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/SearchUI/ApplicantPortFolioSearch.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style type="text/css">
    .chkAllResults {
        width: 10%;
        float: left;
        margin-top: 10px;
        padding-left: 7px;
    }

    .sxcbar {
        padding-left: 0px !important;
        width: 89%;
    }
</style>
<script type="text/javascript">
    function pageLoad() { 
        SetDefaultButtonForSection("divSearchPanel", "fsucCmdBarButton_btnSave", true);

        //UAT-1955
        $jQuery("[id$=ddlTenantName]").attr('tabindex', '0');


        $jQuery(".rgHeader").each(function () {
            if (this.innerText == 'Institution' || this.innerText == 'Institution Hierarchy' || this.innerText == 'Custom Attributes') {
                this.tabIndex = 0;
            }
        });
    }
</script>
<asp:HiddenField ID="hdnCurrentClicked" Value="" runat="server" />
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color" tabindex="0">
                <asp:Label ID="lblApplicantPortFolioSearch" runat="server" Text="Manage Applicant Portfolio Search"></asp:Label>
            </h2>
        </div>
    </div>

    <%--<div class="content">
        <div class="sxform auto">
            <asp:Panel ID="pnlSearch" CssClass="sxpnl" runat="server">
                <div class='sxro sx3co'>--%>

    <div class="row bgLightGreen" id="divSearchPanel">
        <asp:Panel runat="server" ID="pnlShowFilters">
            <div class="col-md-12">
                <div class="row">
                    <div id="div1" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <%--<span class='cptn'>Institution</span><span class="reqd">*</span>--%>
                            <label for="<%= ddlTenantName.ClientID %>_Input" class="cptn">Institution</label><span class="reqd">*</span>
                            <%-- <infs:WclDropDownList ID="ddlTenantName" runat="server" AutoPostBack="true" OnItemSelected="ddlTenantName_ItemSelected" Enabled="false"
                                DataTextField="TenantName" DataValueField="TenantID" OnDataBound="ddlTenantName_DataBound">
                            </infs:WclDropDownList>--%>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName"
                                CausesValidation="false" AutoPostBack="true" DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                OnDataBound="ddlTenantName_DataBound" Enabled="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlTenantName"
                                    InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                    <div class='form-group col-md-3'>&nbsp;</div>
                    <div class='form-group col-md-3' title="Select a User Group to restrict search results to that group">
                        <%--<span id="spnUserGroup" class="cptn">User Group</span>--%>
                        <label id="lblUserGrp" for="<%= ddlUserGroup.ClientID %>_Input" class="cptn">User Group</label>
                        <infs:WclComboBox EnableAriaSupport="true" ID="ddlUserGroup" runat="server" DataTextField="UG_Name" DataValueField="UG_ID"
                            OnDataBound="ddlUserGroup_DataBound" Width="100%" CssClass="form-control" Skin="Silk"
                            AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                   <div class='form-group col-md-3'>
                        <span class="cptn">Show active orders only</span>
                       <asp:RadioButtonList ID="rbShowActiveOrdersOnly" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                           <asp:ListItem Text="Yes " Value="True" Selected="True" />
                           <asp:ListItem Text="No" Value="False" />
                       </asp:RadioButtonList>
                    </div>
                </div>
            </div>
             <div class="col-md-12">
                <div class="row">
            <uc:CustomAttributeLoaderSearch ID="ucCustomAttributeLoaderSearch" runat="server" />
                   </div>
                </div>  
            <div class="col-md-12">
                <div class="row">
                    <div id="div2" runat="server">
                        <div class='form-group col-md-3' title="Restrict search results to the entered User ID">
                            <label id="lblUserID" class="cptn">User ID</label>
                            <infs:WclNumericTextBox aria-labelledby="lblUserID" ShowSpinButtons="false" EnableAriaSupport="true" Type="Number" ID="txtUserID" MaxValue="2147483647"
                                runat="server" InvalidStyleDuration="100" MinValue="1" Width="100%" CssClass="form-control">
                                <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                            </infs:WclNumericTextBox>
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered first name">
                        <label id="lblFName" class="cptn">Applicant First Name</label>
                        <infs:WclTextBox ID="txtFirstName" aria-labelledby="lblFName" EnableAriaSupport="true" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered last name">
                        <label id="lblLName" class="cptn">Applicant Last Name</label>
                        <infs:WclTextBox ID="txtLastName" EnableAriaSupport="true" aria-labelledby="lblLName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered email address">
                        <label id="lblEmailAddress" class="cptn">Email Address</label>
                        <infs:WclTextBox ID="txtEmail" EnableAriaSupport="true" aria-labelledby="lblEmailAddress"  runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                        <div class="vldx">
                        <asp:RegularExpressionValidator Display = "Dynamic" CssClass="errmsg" ControlToValidate = "txtEmail" ID="RegularExpressionValidator2" ValidationExpression = "^[\s\S]{3,}$" runat="server" ErrorMessage="Minimum 3 characters required."></asp:RegularExpressionValidator>
                       </div>
                    </div>
                </div>
            </div>

            <div class="col-md-12">
                <div class="row">
                    <div id="divSSN" runat="server">
                        <div class='form-group col-md-3' title="Restrict search results to the entered SSN or ID Number">
                            <label id="lblSSN" class="cptn">SSN/ID Number</label>
                            <infs:WclMaskedTextBox EnableAriaSupport="true" aria-labelledby="lblSSN" ID="txtSSN" runat="server" MaxLength="10" Width="100%" CssClass="form-control"
                                Mask="aaa-aa-aaaa">
                            </infs:WclMaskedTextBox>
                        </div>
                    </div>
                    <div id="divDOB" runat="server">
                        <div class='col-md-3' title="Restrict search results to the entered Date of Birth">
                            <label for="<%= dpkrDOB.ClientID %>_dateInput" class="cptn">Date of Birth</label>
                            <infs:WclDatePicker ID="dpkrDOB" runat="server" DateInput-EmptyMessage="Select a date"
                                Width="100%" CssClass="form-control" DateInput-EnableAriaSupport="true" DateInput-SelectionOnFocus="CaretToBeginning"
                                ClientEvents-OnPopupClosing="OnCalenderClosing" EnableAriaSupport="true"
                                DateInput-DateFormat="MM/dd/yyyy">
                                <Calendar EnableKeyboardNavigation="true" EnableAriaSupport="true"></Calendar>
                            </infs:WclDatePicker>
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Select &#34All&#34 to view all subscriptions per the other parameters or &#34Archived&#34 to view only archived subscriptions or &#34Active&#34 to view only non archived subscriptions">
                        <label id="lblSubscriptionArchiveState" class="cptn">Subscription Archive State</label>
                        <asp:RadioButtonList ID="rbSubscriptionState" runat="server" OnSelectedIndexChanged="rbSubscriptionState_SelectedIndexChanged"
                            RepeatDirection="Horizontal"
                            DataTextField="As_Name" DataValueField="AS_Code" CssClass="radio_list form-control"
                            AutoPostBack="true" Width="100%">
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

    <div class="row">&nbsp;</div>
    <div class="row">
        <div class="col-md-12">
            <div class="form-group col-md-2">
                <div class="row">
                    <asp:CheckBox ID="chkSelectAllResults" Text="Select All Results" runat="server" OnCheckedChanged="chkSelectAllResults_CheckedChanged"
                        Width="100%" AutoPostBack="true" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group col-md-9">
                <div class="row text-center" id="trailingText">
                    <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel,Clear,Extra"
                        AutoPostbackButtons="Submit,Save,Cancel,Clear,Extra" SubmitButtonText="Reset"
                        SubmitButtonIconClass="rbUndo"
                        SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel"
                        OnSubmitClick="CmdBarReset_Click"
                        OnSaveClick="CmdBarSearch_Click" OnCancelClick="CmdBarCancel_Click" ClearButtonText="Send Message"
                        ExtraButtonText="Passport Report" OnExtraClick="btnReportViewer_Click" OnClearClick="btnSendMessage_Click"
                        UseAutoSkinMode="false" ButtonSkin="Silk" ClearButtonIconClass="rbEnvelope" ExtraButtonIconClass="rbPassport">
                        <%-- DefaultPanel="pnlShowFilters" DefaultPanelButton="Save"--%>
                        <ExtraCommandButtons>
                            <infs:WclButton ID="btnArchieve" runat="server" Text="Archive" OnClick="btnArchieve_Click"
                                Enabled="true" AutoSkinMode="false" Skin="Silk">
                                <Icon PrimaryIconCssClass="rbArchive"></Icon>
                            </infs:WclButton>
                        </ExtraCommandButtons>
                    </infsu:CommandBar>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdApplicantSearchData" AllowCustomPaging="true"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
            ShowClearFiltersButton="false" OnNeedDataSource="grdApplicantSearchData_NeedDataSource"
            OnItemCommand="grdApplicantSearchData_ItemCommand" OnSortCommand="grdApplicantSearchData_SortCommand"
            OnItemDataBound="grdApplicantSearchData_ItemDataBound" EnableLinqExpressions="false"
            NonExportingColumns="ViewDetail,SelectUsers,CustomAttributes,SSN">
            <ClientSettings EnableRowHoverStyle="true" ClientEvents-OnGridCreated="onGridCreated">
                <ClientEvents OnRowDblClick="grd_rwDbClick" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrganizationUserId,ApplicantUserID,TenantID"
                ClientDataKeyNames="OrganizationUserId,ApplicantUserID,TenantID" AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="SelectUsers" HeaderTooltip="Click this box to select all users on the active page"
                        AllowFiltering="false" ShowFilterIcon="false">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectUser" runat="server" ToolTip="Select User" UserID='<%#Eval("OrganizationUserId")%>'
                                onclick="UnCheckHeader(this)" OnCheckedChanged="chkSelectUser_CheckedChanged" />
                            <%-- Checked='<%#Convert.ToBoolean(Eval("IsUserGroupMatching")) %>'
                                   <asp:Label ID="lblIsUserGroup" runat="server" Text='<%#Eval("IsUserGroupMatching") %>'
                                    Visible="false"></asp:Label>--%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="OrganizationUserId" HeaderText="User ID" SortExpression="OrganizationUserId"
                        UniqueName="UserID" HeaderTooltip="This column displays the User ID for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantFirstName" HeaderText="Applicant First Name"
                        SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName" HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantLastName" HeaderText="Applicant Last Name"
                        SortExpression="ApplicantLastName" UniqueName="ApplicantLastName" HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="InstituteName" HeaderText="Institution" AllowSorting="false"
                        SortExpression="InstituteName" UniqueName="InstituteName" HeaderTooltip="This column displays the Institution for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EmailAddress" HeaderText="Email Address" SortExpression="EmailAddress"
                        UniqueName="EmailAddress" ItemStyle-Width="180px" HeaderTooltip="This column displays the applicant's email address for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn DataField="DateOfBirth" HeaderText="Date of Birth" SortExpression="DateOfBirth"
                        UniqueName="DateOfBirth" DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="75px"
                        ItemStyle-Width="88px" HeaderTooltip="This column displays the applicant's date of birth for each record in the grid">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN"
                        UniqueName="SSN" ItemStyle-Width="130px" HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN"
                        Display="false"
                        UniqueName="_SSN" ItemStyle-Width="130px" HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="InstitutionHierarchy" HeaderText="Institution Hierarchy"
                        ItemStyle-Width="250px" AllowSorting="false" SortExpression="InstitutionHierarchy"
                        UniqueName="InstitutionHierarchy" HeaderTooltip="This column displays the institution hierarchy for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CustomAttributes" HeaderText="Custom Attributes"
                        ItemStyle-Width="250px" AllowSorting="false" SortExpression="CustomAttributes"
                        UniqueName="CustomAttributes" HeaderTooltip="This column displays the Custom Attributelist for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CustomAttributes" AllowFiltering="false" HeaderText="Custom Attributes"
                        AllowSorting="false" ItemStyle-Width="300px"
                        UniqueName="CustomAttributesTemp" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail" HeaderStyle-Width="10%">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnDetails" ButtonType="LinkButton" UserID='<%#Eval("OrganizationUserId")%>' CommandName="ViewDetail"
                                runat="server" Text="View Portfolio">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <%-- <telerik:GridButtonColumn ButtonType="LinkButton"  CommandName="ViewDetail" Text="View Portfolio"
                        ItemStyle-Width="100px" UniqueName="ViewDetail">
                    </telerik:GridButtonColumn>--%>
                    <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="ManageDocuments" Text="Manage Documents"
                        ItemStyle-Width="80px" UniqueName="ManageDocuments">
                    </telerik:GridButtonColumn>
                    <%--   <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="ApplicantView" Text="Applicant's Login"
                        ItemStyle-Width="100px" UniqueName="ApplicantView" HeaderTooltip="Click here for Applicant's Login">
                    </telerik:GridButtonColumn>--%>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ApplicantView" HeaderStyle-Width="10%">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnApplicantView" ButtonType="LinkButton" CommandName="ApplicantView"
                                runat="server" Text="Applicant's Login" ToolTip="Click to login as applicant"
                                Visible='<%# Convert.ToBoolean(Eval("IsAdmin")) == false %>'>
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>
</div>

<asp:HiddenField ID="hdnSubscriptionIds" runat="server" />
<asp:HiddenField ID="hdnSelectedTenantID" runat="server" />
<asp:HiddenField ID="hdnCurrentUserID" runat="server" />
<asp:HiddenField ID="hdnGetSubscriptionsFromPopup" runat="server" />
<asp:HiddenField ID="hdnArchivedFromPopup" runat="server" Value="false" />
<asp:HiddenField ID="hdMessageSent" runat="server" Value="new" />
<script type="text/javascript">
    //click on link button while double click on any row of grid.
    function grd_rwDbClick(s, e) {
        var _id = "btnDetails";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

    function CheckAll(id) {
        var masterTable = $find("<%= grdApplicantSearchData.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectUser").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectUser").checked = isChecked; // for checking the checkboxes
            }
        }
    }

    function UnCheckHeader(id) {
        var checkHeader = true;
        var masterTable = $find("<%= grdApplicantSearchData.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectUser").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectUser").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    }
    //This event fired when PassPort Report popup closed.
    function OnPassPortReportClose(oWnd, args) {        
        oWnd.remove_close(OnClose);
        var currentLinkFocus = $jQuery("[id$=hdnCurrentClicked]").val();
        window.parent.parent.$jQuery("[id$=dvSkipContent]").attr("tabindex", "0");
        if (currentLinkFocus != undefined && currentLinkFocus != null && currentLinkFocus != "") {
            setTimeout(function () { $jQuery("[id$=" + currentLinkFocus + "]").focus(); }, 500);
            $jQuery("[id$=hdnCurrentClicked]").val("");
        }
        $jQuery("[id$=<%= fsucCmdBarButton.ExtraButton.ClientID %>]").focus();
        return false;
    }

    function OpenPopup(sender, eventArgs) {
        var composeScreenWindowName = "composeScreen";
        var fromScreenName = "PortfolioSearch";
        var communicationTypeId = 'CT01';
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/Messaging/Pages/WriteMessage.aspx?cType=" + communicationTypeId + "&SName=" + fromScreenName);
        var win = $window.createPopup(url, { size: "900,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: composeScreenWindowName, onclose: OnMessagePopupClose });
        return false;
    }

    function OpenReportPopup() {
        //debugger;
        window.parent.parent.$jQuery("[id$=dvSkipContent]").attr("tabindex", "-1");
        var composeScreenWindowName = "Report Viewer";
        var fromScreenName = "PortfolioSearch";
        var url = $jQuery('[id$="hdnSubscriptionIds"]').val();
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var win = $window.createPopup(url, { size: "800,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: composeScreenWindowName, onclose: OnPassPortReportClose });
        return false;
    }

    //Function to open Multiple subscription Popup to archive specific subscription of applicants who have more than one subscription.
    function OpenMutlipleSubscriptionsPopup() {
        //debugger;
        var popupWindowName = "Manage Multiple Subscriptions";
        var fromScreenName = "PortfolioSearch";
        var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
        var currentUserID = $jQuery("[id$=hdnCurrentUserID]").val();
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/SearchUI/Pages/ManageMultipleSubscriptionsPopup.aspx?TenantID=" + tenantID + "&CurrentUserID=" + currentUserID + "&SName=" + fromScreenName);
        var win = $window.createPopup(url, { size: "900,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnClose }
           );
        return false;
    }

    //This event fired when multiple subscription popup closed.
    function OnClose(oWnd, args) {
        //debugger;
        oWnd.remove_close(OnClose);
        var arg = args.get_argument();
        if (arg) {
            if (arg.Action == "Submit") {
                $jQuery("[id$=hdnArchivedFromPopup]").val("true");
                var masterTable = $find("<%= grdApplicantSearchData.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        }
    }

    //This event fired when Send Message popup closed.
    function OnMessagePopupClose(oWnd, args) {
        //debugger;
        oWnd.remove_close(OnMessagePopupClose);
        var arg = args.get_argument();
        if (arg) {
            if (arg.MessageSentStatus == "sent") {
                $jQuery("[id$=hdMessageSent]").val("sent");
                var masterTable = $find("<%= grdApplicantSearchData.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        }
    }

    function OpenApplicantView(navUrl) {
        var win = window.open(navUrl, '_blank');
        if (win) {
            //Browser has allowed it to be opened
            win.focus();
        }
    }

    /*function UnCheckSubscriptionsAfterArchival() {
        var masterTable = 
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            masterTable.get_dataItems()[i].findElement("chkSelectUser").checked = false; // for checking the checkboxes
        }
    }*/
</script>
