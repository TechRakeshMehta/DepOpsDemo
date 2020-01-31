<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientUserSearch.ascx.cs" Inherits="CoreWeb.Search.Views.ClientUserSearch" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="AgencyHierarchyMultipleOld" Src="~/AgencyHierarchy/UserControls/OldAgencyHierarchyMultipleSelection.ascx" %>
<%@ Register TagPrefix="uc" TagName="AgencyHierarchyMultiple" Src="~/AgencyHierarchy/UserControls/AgencyHierarchyMultipleSelection.ascx" %>

<script src="../Resources/Mod/Shared/KeyBoardSupport.js"></script>
<style type="text/css">
    .userType tr td:first-child label {
        margin-right: 2px !important;
    }

    .userType tr td:last-child label {
        margin-right: 2px !important;
    }

    .breakword {
        word-break: break-all;
    }

    .LinkDisabled {
        color: gray !important;
    }

    .LinkEnabled {
        color: blue !important;
    }
    .textClass {
    color:black !important;
    }
</style>

<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblClientUserSearch" runat="server" Text="Manage Client Search"></asp:Label></h1>
    <div class="content">
        <div class="sxform auto" id="divSearchPanel">
            <asp:Panel ID="pnlSearch" CssClass="sxpnl" runat="server">
                <div class='sxro sx3co'>
                    <div class='sxlb' title="Select the Institution whose data you want to view">
                        <span class="cptn">User Type</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <asp:RadioButtonList runat="server" AutoPostBack="true" RepeatDirection="Horizontal" ID="rblUserType" CssClass="userType" OnSelectedIndexChanged="rblUserType_SelectedIndexChanged">
                            <asp:ListItem Value="ALL" Text="All" Selected="True" />
                            <asp:ListItem Value="TNTUT" Text="Client Admin" />
                            <asp:ListItem Value="AUUT" Text="Agency User" />
                        </asp:RadioButtonList>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="rblUserType" ValidationGroup="grpFormSubmit"
                                Display="Dynamic" CssClass="errmsg" Text="User Type is required." />
                        </div>
                    </div>

                    <div id="divTenant" runat="server">
                        <div class='sxlb' title="Select the Institution(s) whose data you want to view">
                            <span class="cptn">Institution</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName"
                                CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                OnClientDropDownClosed="OnTenantClosed" CausesValidation="false" AutoPostBack="false" DataValueField="TenantID" Enabled="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName" ValidationGroup="grpFormSubmit"
                                    Display="Dynamic" CssClass="errmsg" Text="Institution is required." />
                            </div>
                        </div>
                    </div>


                    <div title="Click the link and select a node to restrict search results to the selected node" id="dvInstHierarchy" runat="server">
                        <div class='sxlb' title="">
                            <span class="cptn">Institution Hierarchy</span>
                        </div>
                        <div class='sxlm'>
                            <a runat="server" href="#" id="instituteHierarchy" onclick="openInstitutionPopUp();">Select Institution Hierarchy</a>&nbsp;&nbsp
                        <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
                        </div>
                    </div>


                    <%-- <div id="divAgency" runat="server">
                        <div class='sxlb' title="Select the Agency(s) whose data you want to view">
                            <span class="cptn">Agency</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="cmbAgency" runat="server" AutoPostBack="false" DataTextField="AG_Name" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                EmptyMessage="--Select--" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" DataValueField="AG_ID" CausesValidation="true">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvAgency" ControlToValidate="cmbAgency" ValidationGroup="grpFormSubmit"
                                    Display="Dynamic" CssClass="errmsg" Text="Agency is required." />
                            </div>
                        </div>
                    </div>--%>
                </div>
                <div class='sxroend'>
                </div>
                <div class='sxro sx3co'>

                    <div id="dvAgencyHierarchy" runat="server">
                        <div class='sxlb' title="Select the Agency Hierarchy Node(s) whose data you want to view">
                            <span class="cptn">Agency Hierarchy Nodes</span><span runat="server" id="spnAgencyHierarchyNodes" class="reqd">*</span>
                        </div>


                        <div class='sxlm'>
                            <infs:WclComboBox ID="cmbAgencyHierarchy" Width="100%" runat="server" AutoPostBack="false" DataTextField="AgencyName"
                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnClientDropDownClosed="OnAgencyClosed"
                                CausesValidation="false" Enabled="true"
                                DataValueField="AgencyID" EnableCheckAllItemsCheckBox="true" CheckBoxes="true" ValidationGroup="grpFormSubmit">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvAgency" ControlToValidate="cmbAgencyHierarchy"
                                    Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                    Text="Agency Hierarchy is required." />
                            </div>
                        </div>
                    </div>


                    <div class='sxlb' id="dvAgencyHierarchyOld" title="Select a Agency(s) whose data you want to view.">
                        <uc:AgencyHierarchyMultipleOld ID="ucAgencyHierarchyMultipleToSearchRotation" runat="server" />
                    </div>
                    <div class='sxlm'>
                    </div>

                    <div class='sxlb' title="Restrict search results to the entered User Name">
                        <span class="cptn">Username</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtUserName" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>

                    <div class='sxlb' title="Restrict search results to the entered first name">
                        <span class="cptn">First Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtFirstName" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb' title="Restrict search results to the entered last name">
                        <span class="cptn">Last Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtLastName" runat="server">
                        </infs:WclTextBox>
                    </div>

                    <div class='sxlb' title="Restrict search results to the entered email address">
                        <span class="cptn">Email Address</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtEmail" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
            AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbRefresh" SubmitButtonText="Reset"
            SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel" ValidationGroup="grpFormSubmit"
            OnSubmitClick="fsucCmdBarButton_ResetClick" OnSaveClick="fsucCmdBarButton_SearchClick" OnCancelClick="fsucCmdBarButton_CancelClick">
        </infsu:CommandBar>
        <br />
        <br />
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdClientSearchData" AllowCustomPaging="True"
                AutoGenerateColumns="False" AllowSorting="True" AllowFilteringByColumn="false"
                AutoSkinMode="True" CellSpacing="0" GridLines="Both" ShowAllExportButtons="False"
                ShowClearFiltersButton="false" OnNeedDataSource="grdClientSearchData_NeedDataSource" OnItemDataBound="grdClientSearchData_ItemDataBound"
                OnItemCommand="grdClientSearchData_ItemCommand" OnSortCommand="grdClientSearchData_SortCommand"
                 EnableLinqExpressions="false" NonExportingColumns="ViewDetail,AssignedRoles">
                <ClientSettings EnableRowHoverStyle="true">
                    <ClientEvents OnRowDblClick="grd_rwDbClick" />
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrganizationUserId,UserID" AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridNumericColumn DataField="OrganizationUserId" HeaderText="User ID" SortExpression="OrganizationUserId" ItemStyle-Width="5%"
                            UniqueName="OrganizationUserId" HeaderTooltip="This column displays the User ID for each record in the grid">
                        </telerik:GridNumericColumn>

                        <telerik:GridNumericColumn DataField="AgencyName" HeaderText="Agency Name" SortExpression="AgencyName" ItemStyle-Width="15%"
                            UniqueName="AgencyName" HeaderTooltip="This column displays the Agency Name for each record in the grid">
                        </telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="TenantName" HeaderText="Tenant Name" SortExpression="TenantName" ItemStyle-CssClass="breakword" ItemStyle-Width="15%"
                            UniqueName="TenantName" HeaderTooltip="This column displays the Tenant Name for each record in the grid">
                        </telerik:GridNumericColumn>

                        <telerik:GridNumericColumn DataField="AgencyUserId" HeaderText="User Type" SortExpression="AgencyUserId"
                            UniqueName="AgencyUserId_tmp" Display="false" HeaderTooltip="This column displays the User Type for each record in the grid">
                        </telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="UserType" HeaderText="User Type" SortExpression="UserType" ItemStyle-Width="5%"
                            UniqueName="UserType" HeaderTooltip="This column displays the User Type for each record in the grid">
                        </telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="ClientFirstName" FilterControlAltText="Filter ClientFirstName column" ItemStyle-Width="15%"
                            HeaderText="First Name" SortExpression="ClientFirstName" UniqueName="ClientFirstName"
                            HeaderTooltip="This column displays the first name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ClientLastName" FilterControlAltText="Filter ApplicantLastName column" ItemStyle-Width="15%"
                            HeaderText="Last Name" SortExpression="ClientLastName" UniqueName="ApplicantLastName" AllowSorting="true"
                            HeaderTooltip="This column displays the last name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EmailAddress" FilterControlAltText="Filter EmailAddress column" ItemStyle-Width="15%"
                            HeaderText="Email Address" SortExpression="EmailAddress" UniqueName="EmailAddress" AllowSorting="true"
                            HeaderTooltip="This column displays the email address for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Phone" HeaderText="Phone Number" SortExpression="Phone" ItemStyle-Width="10%"
                            HeaderTooltip="This column displays the phone for each record in the grid"
                            UniqueName="Phone">
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="AssignedRoles" HeaderText="Assigned Roles" ItemStyle-CssClass="breakword"
                            HeaderTooltip="This column displays the assigned roles for each record in the grid" Display="false"
                            UniqueName="_AssignedRoles">
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="AssignedRoles" HeaderText="Assigned Roles" SortExpression="AssignedRoles" ItemStyle-CssClass="breakword"
                            HeaderTooltip="This column displays the assigned roles for each record in the grid" AllowSorting="false" ItemStyle-Width="15%"
                            UniqueName="AssignedRoles">
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="LastLoginDateTime" HeaderText="Last Login Date" SortExpression="LastLoginDateTime" ItemStyle-Width="20%"
                            HeaderTooltip="This column displays last login date for each record in the grid" AllowSorting="false"
                            UniqueName="LastLoginDateTime">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnDetails" ButtonType="LinkButton" CommandName="ViewDetail"
                                    ToolTip="Click here to edit the profile information of the user" runat="server"
                                    Text="Detail" BackColor="Transparent" Font-Underline="true" BorderStyle="None">
                                </telerik:RadButton>
                                <asp:HiddenField ID="hdnTenantID" runat="server" Value='<%#Eval("ClientTenantID") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="AgencyView" ItemStyle-Width="25%">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnAgencyView" ButtonType="LinkButton" CommandName="AgencyView"
                                    runat="server" Text="Agency's_Login" ToolTip="Click to login as agency"
                                    BackColor="Transparent" Font-Underline="true" BorderStyle="None">
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
        <div class="gclr">
        </div>
    </div>
</div>
<asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
<asp:HiddenField runat="server" ID="hdnPreviousAgencyValues" />
<asp:HiddenField ID="hdnIsPagePostBack" runat="server" Value="" />
<asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
<asp:HiddenField ID="hdnInstitutionNodeID" runat="server" Value="" />
<asp:HiddenField ID="hdnSelectedTenantID" runat="server" Value="" />
<asp:HiddenField ID="hdnInstHierarchyLabel" runat="server" Value="" />
<asp:Button ID="btnCheckAgencyRootNode" runat="server" OnClick="btnCheckAgencyRootNode_Click" />
<asp:Button ID="btnCheckInstitutionChanged" runat="server" OnClick="btnCheckInstitutionChanged_Click" />
<asp:HiddenField ID="hdnIsInstHierDisabled" runat="server" Value="" />

<script type="text/javascript">
    //click on link button while double click on any row of grid.
    function grd_rwDbClick(s, e) {
        var _id = "btnDetails";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

    function OnTenantClosed(sender, eventArgs) {
        // debugger;
        $jQuery("[id$=btnCheckInstitutionChanged]").click();
        //if (areThereAnyChangesAtTheSelection(sender)) {
        //    __doPostBack('ddlTenantName', '');
        //}
        //else {
        //    return false;
        //}
    }

    function OnAgencyClosed(sender, eventArgs) {
        //debugger;
        $jQuery("[id$=btnCheckAgencyRootNode]").click();
        //if (areThereAnyChangesAtTheSelection(sender)) {
        //    __doPostBack('cmbAgencyHierarchy', '');
        //}
        //else {
        //    return false;
        //}
    }

    var oldSelectedIdList = [];

    function radComboBoxSelectedIdList(sender) {
        var selectedIdList = [];
        var combo = sender;
        var items = combo.get_items();
        var checkedIndices = items._parent._checkedIndices;
        var checkedIndicesCount = checkedIndices.length;
        for (var itemIndex = 0; itemIndex < checkedIndicesCount; itemIndex++) {
            var item = items.getItem(checkedIndices[itemIndex]);
            selectedIdList.push(item._properties._data.value);
        }
        return selectedIdList;
    }


    function areThereAnyChangesAtTheSelection(sender) {
        var hdnPreviousAgencyValues = $jQuery("[id$=hdnPreviousAgencyValues]");
        if (hdnPreviousAgencyValues.val() != "" && hdnPreviousAgencyValues.val() != null && hdnPreviousAgencyValues.val() != undefined) {
            oldSelectedIdList = hdnPreviousAgencyValues.val().split(',');
        }
        var selectedIdList = radComboBoxSelectedIdList(sender);
        hdnPreviousAgencyValues.val(selectedIdList.join(","));
        var isTheCountOfEachSelectionEqual = (selectedIdList.length == oldSelectedIdList.length);
        if (isTheCountOfEachSelectionEqual == false)
            return true;

        var oldIdListMINUSNewIdList = $jQuery(oldSelectedIdList).not(selectedIdList).get();
        var newIdListMINUSOldIdList = $jQuery(selectedIdList).not(oldSelectedIdList).get();

        if (oldIdListMINUSNewIdList.length != 0 || newIdListMINUSOldIdList.length != 0)
            return true;

        return false;
    }

    function pageLoad() {

        //debugger;
        var hdnHierarchyLabel = $jQuery("[id$=hdnHierarchyLabel]");
        var hdnIsPagePostBack = $jQuery("[id$=hdnIsPagePostBack]");
        if (hdnHierarchyLabel.val() != "" && hdnIsPagePostBack.val() == "Focus Set") {
            setTimeout(function () { $jQuery("[id$=instituteHierarchy]").focus(); }, 100);
            hdnIsPagePostBack.val("");
        }

        SetDefaultButtonForSection("divSearchPanel", "fsucCmdBarButton_btnSave", true);

        AddStyleToAgencyHierarchy();
        AddStyleToInstHierarchy();
    }

    function OpenAgencyUserView(navUrl) {
        var win = window.open(navUrl, '_blank');
        if (win) {
            //Browser has allowed it to be opened
            win.focus();
        }
    }

    //UAT-4257
    function openInstitutionPopUp() {
        //debugger;
        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "CommonScreen";
        //var tenantId = 2;
        var isInstHierDisabled = $jQuery("[id$=hdnIsInstHierDisabled]").val();
        if (isInstHierDisabled != undefined && (isInstHierDisabled == "true" || isInstHierDisabled == "True"))
            return;

        var tenantId = $jQuery("[id$=hdnSelectedTenantID]").val();
        if (tenantId != "0" && tenantId != "" && tenantId > 0) {
            $jQuery("[id$=hdnIsPagePostBack]").val("Focus Set");
            $jQuery("[id$=instituteHierarchy]").focusout();
            var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();

            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName=" + screenName + "&DelemittedDeptPrgMapIds=" + DepartmentProgramId);
            var win = $window.createPopup(url, { size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientPopUpClose });
            winopen = true;
        }
        //else {
        //    // debugger;
        //    $alert("Please select Institution.");
        //}
        return false;
    }

    function OnClientPopUpClose(oWnd, args) {
        //debugger;
        oWnd.remove_close(OnClientPopUpClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
                $jQuery("[id$=hdnDepartmntPrgrmMppng]").val(arg.DepPrgMappingId);
                $jQuery("[id$=hdnInstHierarchyLabel]").val(arg.HierarchyLabel);
                $jQuery("[id$=hdnInstitutionNodeID]").val(arg.InstitutionNodeId);
                __doPostBack("<%= btnDoPostBack.ClientID %>", "");
            }
            winopen = false;

            setTimeout(function () { $jQuery("[id$=instituteHierarchy]").focus(); }, 100);
        }
    }

    function AddStyleToAgencyHierarchy() {
       // debugger;
        var AddDisableStyle = $jQuery("[id$=hdnAddDisabledStyle]").val();
        var dvAgencyHierarchyOld = $jQuery("[id$=dvAgencyHierarchyOld]");
        var lblAgencyHierarchy = $jQuery("[id$=lblAgencyHierarchy]");
        if (AddDisableStyle != undefined && (AddDisableStyle == "true" || AddDisableStyle == "True")) {
            $jQuery("[id$=AgencyHierarchy]").addClass("LinkDisabled");
            $jQuery("[id$=AgencyHierarchy]").removeClass("LinkEnabled");
            dvAgencyHierarchyOld[0].title = "";
        } else {
            $jQuery("[id$=AgencyHierarchy]").addClass("LinkEnabled");
            $jQuery("[id$=AgencyHierarchy]").removeClass("LinkDisabled");
            dvAgencyHierarchyOld[0].title = "Select a Agency(s) whose data you want to view.";
        }

        lblAgencyHierarchy.addClass("textClass");
        lblAgencyHierarchy.removeClass("LinkEnabled");
        lblAgencyHierarchy.removeClass("LinkDisabled");
    }

    function AddStyleToInstHierarchy() {
        //debugger;
        var AddDisableStyle = $jQuery("[id$=hdnIsInstHierDisabled]").val();
        var dvInstHierarchy = $jQuery("[id$=dvInstHierarchy]");
        var lblinstituteHierarchy = $jQuery("[id$=lblinstituteHierarchy]");
        if (AddDisableStyle != undefined && (AddDisableStyle == "true" || AddDisableStyle == "True")) {
            $jQuery("[id$=instituteHierarchy]").addClass("LinkDisabled");
            $jQuery("[id$=instituteHierarchy]").removeClass("LinkEnabled");
            dvInstHierarchy[0].title = "";
        } else {
            $jQuery("[id$=instituteHierarchy]").addClass("LinkEnabled");
            $jQuery("[id$=instituteHierarchy]").removeClass("LinkDisabled");
            dvInstHierarchy[0].title = "Click the link and select a node to restrict search results to the selected node";
        }
        lblinstituteHierarchy.addClass("textClass");
        lblinstituteHierarchy.removeClass("LinkEnabled");
        lblinstituteHierarchy.removeClass("LinkDisabled");
    }


</script>
