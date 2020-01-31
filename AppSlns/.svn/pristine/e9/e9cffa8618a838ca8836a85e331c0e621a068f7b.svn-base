<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpcomingExpirationsSearchControl.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.UpcomingExpirationsSearchControl" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="AgencyHierarchyMultiple" Src="~/AgencyHierarchy/UserControls/AgencyHierarchyMultipleSelection.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxComplianceSearch">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/KeyBoardSupport.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/AgencyHierarchy/AgencyHierarchyMultipleSelection.js" ResourceType="JavaScript" />
    
<infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <asp:Label ID="lblUpcomingExpirationsTitle" runat="server" Text=""></asp:Label>
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen" id="divSearchPanel">
        <asp:Panel ID="Panel1" runat="server">
            <div class="col-md-12">
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span><span class="reqd">*</span>
                            <%--<infs:WclDropDownList ID="ddlTenantName" runat="server" DataTextField="TenantName"
                                CausesValidation="false" AutoPostBack="true" DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                OnDataBound="ddlTenantName_DataBound" Enabled="false">
                            </infs:WclDropDownList>--%>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName"
                                CausesValidation="true" AutoPostBack="true" DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                OnDataBound="ddlTenantName_DataBound" ValidationGroup="grpFormSubmit" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <%-- <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    InitialValue="--Select--" ValidationGroup="SearchData" Display="Dynamic" CssClass="errmsg" Text="Institution is required." />
                            </div>--%>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Institution is required." />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Select a Agency whose data you want to view.">
                        <span class="cptn">Institution Hierarchy</span>
                        <a style="color: blue;" href="#" id="lnkInstitutionHierarchyPB" runat="server" class="">Select Institution Hierarchy</a><br />
                        <asp:Label ID="lblInstitutionHierarchyPB" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <div class='sxroend'>
            </div>

            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Select one or more Category Statuses to restrict search results to those statuses">
                        <span class="cptn">Category(s)</span>
                        <infs:WclComboBox ID="ddlCategories" runat="server" CheckBoxes="true" DataTextField="CategoryName"
                            DataValueField="ComplianceCategoryID" EnableCheckAllItemsCheckBox="true" EmptyMessage="--Select--" OnSelectedIndexChanged="ddlCategories_SelectedIndexChanged" Width="100%"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            CssClass="form-control" Skin="Silk" AutoPostBack="true" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Select one or more Item Statuses to restrict search results to those statuses">
                        <span class="cptn">Item(s)</span>
                        <infs:WclComboBox ID="ddlItems" runat="server" CheckBoxes="true" DataTextField="Name"
                            DataValueField="ComplianceItemID" EnableCheckAllItemsCheckBox="true" EmptyMessage="--Select--" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Select an Overall Compliance Status to restrict search results to that status">
                        <span class="cptn">User Group(s)</span>
                        <infs:WclComboBox ID="ddlUserGroups" runat="server" EnableCheckAllItemsCheckBox="true" CheckBoxes="true" DataTextField="UG_Name"
                            DataValueField="UG_ID" EmptyMessage="--Select--" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoPostBack="false" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered last name">
                        <span class="cptn">From</span>
                        <infs:WclDatePicker ID="dpkrDateFrom" runat="server" DateInput-EmptyMessage="Select a date" Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>
                    <div id="divSSN" runat="server">
                        <div class='form-group col-md-3' title="Restrict search results to the entered SSN or ID Number">
                            <span class="cptn">To</span>
                            <infs:WclDatePicker ID="dpkrDateTo" runat="server" DateInput-EmptyMessage="Select a date" Width="100%" CssClass="form-control">
                            </infs:WclDatePicker>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="form-group col-md-12">
                <div class="row" id="trailingText">
                    <infsu:CommandBar ID="fsucCmdBar1" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel,Clear"
                        AutoPostbackButtons="Submit,Save,Cancel,Clear" SubmitButtonIconClass="rbUndo" SubmitButtonText="Reset"
                        SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel"
                        OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click" OnCancelClick="CmdBarCancel_Click"
                        ClearButtonText="Send Message" OnClearClick="btnSendMessage_Click"
                        ClearButtonIconClass="rbEnvelope" ValidationGroup="grpFormSubmit" UseAutoSkinMode="false" ButtonSkin="Silk">
                    </infsu:CommandBar>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <infs:WclGrid runat="server" ID="grdUpcomingExpiration" AllowPaging="True" AutoGenerateColumns="False" AllowFilteringByColumn="false"
            AllowSorting="True" AutoSkinMode="True" CellSpacing="0" GridLines="Both" ShowAllExportButtons="False"
            OnNeedDataSource="grdUpcomingExpiration_NeedDataSource" OnItemCommand="grdUpcomingExpiration_ItemCommand"
            ShowClearFiltersButton="false" AllowCustomPaging="true" OnSortCommand="grdUpcomingExpiration_SortCommand"
            OnInit="grdUpcomingExpiration_Init" OnItemDataBound="grdUpcomingExpiration_ItemDataBound"
            NonExportingColumns="SelectUsers"
            EnableLinqExpressions="false">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="false" IgnorePaging="true">
            </ExportSettings>
            <GroupingSettings CaseSensitive="false" />
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="StudentID"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="SelectUsers" HeaderTooltip="Click this box to select all users on the active page"
                        AllowFiltering="false" ShowFilterIcon="false" ItemStyle-Width="10px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectUser" runat="server" OnCheckedChanged="chkSelectUser_CheckedChanged"
                                onclick="UnCheckHeader(this)" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="FirstName" FilterControlAltText="Filter FirstName column"
                        HeaderText="First Name" SortExpression="FirstName" UniqueName="FirstName"
                        ItemStyle-Width="100px" HeaderTooltip="This column displays the First Name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="LastName" FilterControlAltText="Filter LastName column"
                        HeaderText="Last Name" SortExpression="LastName" UniqueName="LastName"
                        ItemStyle-Width="100px" HeaderTooltip="This column displays the Last Name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CustomAttributes" HeaderText="Custom Attributes"
                        SortExpression="CustomAttributes" UniqueName="CustomAttributes"
                        ItemStyle-Width="75px" HeaderTooltip="Custom Attributes">
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn DataField="UserGroups" FilterControlAltText="Filter UserGroups column"
                        HeaderText="User Groups" SortExpression="UserGroups" UniqueName="UserGroups"
                        ItemStyle-Width="75px" FilterControlWidth="100px" HeaderTooltip="This column displays the User Groups for each record in the grid">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="Category" FilterControlAltText="Filter Category column"
                        HeaderText="Category" SortExpression="Category" UniqueName="Category" ItemStyle-Width="130px"
                        HeaderTooltip="This column displays the Category for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Item" FilterControlAltText="Filter Item column"
                        AllowFiltering="false" HeaderText="Item" AllowSorting="false" ItemStyle-Width="130px"
                        UniqueName="Item" HeaderTooltip="This column displays theItem for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ExpirationDate" DataFormatString="{0:d}" AllowFiltering="false" HeaderText="Expiration Date" AllowSorting="false" ItemStyle-Width="300px"
                        UniqueName="ExpirationDate">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="NonComplianceDate" FilterControlAltText="Filter NonComplianceDate column"
                        HeaderText="Non-Compliance Date" SortExpression="NonComplianceDate" UniqueName="NonComplianceDate"
                        ItemStyle-Width="130px" DataFormatString="{0:d}" HeaderTooltip="This column displays NonCompliance Date in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ItemStatus" FilterControlAltText="Filter Category/ItemStatus column"
                        HeaderText="Category/Item Current Status" SortExpression="ItemStatus" UniqueName="ItemStatus"
                        ItemStyle-Width="100px" HeaderTooltip="This column displays Category/Item Status for each record in the grid">
                    </telerik:GridBoundColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
        </infs:WclGrid>
    </div>
    <asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
    <asp:HiddenField ID="hdnTenantIdNew" runat="server" Value="" />
    <asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
    <asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
    <asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />
    <asp:HiddenField ID="hdnDepartmentProgmapNew" runat="server" Value="" />
    <asp:HiddenField ID="hdnInstNodeIdNew" runat="server" Value="" />
    <asp:HiddenField ID="hdnCurrentRotStartDate" runat="server" Value="" />
    <asp:HiddenField ID="hdnInstNodeLabel" runat="server" Value="" />
    <asp:HiddenField ID="hdnValidateFileUploadControl" runat="server" Value="" />
    <asp:HiddenField ID="hdMessageSent" runat="server" Value="new" />    
</div>

<script>
    function CheckAll(id) {
        var masterTable = $find("<%= grdUpcomingExpiration.ClientID %>").get_masterTableView();
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
        var masterTable = $find("<%= grdUpcomingExpiration.ClientID %>").get_masterTableView();
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

    function OpenPopup(sender, eventArgs) {
        var composeScreenWindowName = "composeScreen";
        var fromScreenName = "ComplianceSearch";
        var communicationTypeId = 'CT01';
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/Messaging/Pages/WriteMessage.aspx?cType=" + communicationTypeId + "&SName=" + fromScreenName);
        var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: composeScreenWindowName, onclose: OnMessagePopupClose });
        return false;
    }

    function OnMessagePopupClose(oWnd, args) {
        oWnd.remove_close(OnMessagePopupClose);
        var arg = args.get_argument();
        if (arg) {
            if (arg.MessageSentStatus == "sent") {
                $jQuery("[id$=hdMessageSent]").val("sent");
                var masterTable = $find("<%= grdUpcomingExpiration.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        }
    }

    function pageLoad() {

       

        SetDefaultButtonForSection("divSearchPanel", "fsucCmdBar1_btnSave", true);
    }

    function BindInstitutionLabel() {
        setTimeout(function () {
            var InstNodeLabel = $jQuery("[id$=hdnInstNodeLabel]").val();
            $jQuery($jQuery("[id$=lblInstitutionHierarchyPB]")[0]).text(InstNodeLabel);
        }, 1100);
    }

    $jQuery(document).ready(function () {
        $jQuery(document).on('click', '[id$=lnkInstitutionHierarchyPB]', function (e) {
            if (!$jQuery(this).hasClass('disabled')) {
                OpenInstitutionHierarchyPopupInsideGrid(false);
            }
        });
    });

    var winopen = false;

    function OpenInstitutionHierarchyPopupInsideGrid(IsMappingScreen) {
        //debugger;
        //UAT-1843: Bug Fix: Institution Hierarchy popup not shown on top if this popup maximized
        if (IsMappingScreen) {
            var win = $page.get_window();
            if (win) {
                win.restore();
            }
        }

        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "CommonScreen";
        var tenantId = $find($jQuery("[id$=ddlTenantName]").attr("id")).get_value();
        if (tenantId != "0" && tenantId != "") {
            var DelemittedDeptPrgMapIds = $jQuery("[id$=hdnDepartmentProgmapNew]").val();
            var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&DelemittedDeptPrgMapIds=" + DelemittedDeptPrgMapIds + "&ScreenName=" + screenName);
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var win = $window.createPopup(url, {
                size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move,
                name: composeScreenWindowName, onclose: OnHierarhyClientClose
            });
            winopen = true;
        }
        else {
            $alert("Please select Institution.");
        }
        return false;
    }

    function OnHierarhyClientClose(oWnd, args) {

        oWnd.remove_close(OnHierarhyClientClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {

                $jQuery("[id$=hdnDepartmentProgmapNew]").val(arg.DepPrgMappingId);
                $jQuery("[id$=hdnInstitutionHierarchyPBLbl]").val(arg.HierarchyLabel);
                $jQuery("[id$=hdnInstNodeIdNew]").val(arg.InstitutionNodeId);
                $jQuery("[id$=lblInstitutionHierarchyPB]")[0].innerHTML = arg.HierarchyLabel;
                if ($jQuery("[id$=hdnInstNodeLabel]").length > 0) {
                    $jQuery("[id$=hdnInstNodeLabel]").val(arg.HierarchyLabel);
                }
                $jQuery("[id$=spnInsPre]").attr('class', 'reqd controlHidden');
            }
            winopen = false;
        }
    }
</script>
