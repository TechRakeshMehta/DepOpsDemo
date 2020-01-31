<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageUnArchivalRequests.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.ManageUnArchivalRequests" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<style type="text/css">
    #modhdr .phdr {
        text-transform: none !important;
    }

    .radio_list label {
        font-size: 11px !important;
    }
</style>
<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/KeyBoardSupport.js" ResourceType="JavaScript" />
    
<infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<script type="text/javascript">
    function CheckAll(id) {
        var masterTable = $find("<%= grdUnArchivalRequests.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectItem").checked = isChecked; // for checking the checkboxes
            }
        }
    }
    function UnCheckHeader(id) {
        var checkHeader = true;
        var masterTable = $find("<%= grdUnArchivalRequests.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    }

    function pageLoad() {

      

        SetDefaultButtonForSection("divSearchPanel", "cmdBar_btnSave", true);
    }
</script>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Manage Archived Order(s)
            </h2>
        </div>
    </div>
    <div class="col-md-12">
        <div class="row">
            <div class='form-group col-md-3'>
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
        </div>
    </div>
    <div class="row bgLightGreen" id="divSearchPanel">
        <asp:Panel runat="server" ID="pnlTenant">
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3'>
                        <asp:Label ID="lblTenant" runat="server" Text="Institution" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                        <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnDataBound="ddlTenant_DataBound" CausesValidation="false" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenant"
                                InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Institution is required." />
                        </div>
                    </div>
                    <div class='form-group col-md-3'></div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Archive Package Type</span>
                        <asp:RadioButtonList ID="rblPackageSelection" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdbPackageSelection_SelectedIndexChanged"
                            CssClass="w_cptn" AutoPostBack="true" Width="100%" Skin="Silk" AutoSkinMode="false">
                            <asp:ListItem Text="Tracking" Value="AAAA" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Screening" Value="AAAB"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class='form-group col-md-3'></div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-5'>
                        <span class="cptn">Archive State</span>
                        <div id="dvSubscriptionState" runat="server" visible="false">
                            <asp:RadioButtonList ID="rbSubscriptionState" runat="server" OnSelectedIndexChanged="rbSubscriptionState_SelectedIndexChanged" RepeatDirection="Horizontal"
                                CssClass="w_cptn" AutoPostBack="false" Width="100%" Skin="Silk" AutoSkinMode="false">
                                <asp:ListItem Text="All Archived" Value="ARSB" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Un-Archival Requested" Value="UARS"> </asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
            </div>

        </asp:Panel>
    </div>
    <div class="row text-center">
        <infsu:CommandBar ID="cmdBar" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
            AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonText="Reset" SubmitButtonIconClass="rbUndo"
            SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel"
            OnSubmitClick="cmdBar_SubmitClick" OnSaveClick="cmdBar_SaveClick" OnCancelClick="cmdBar_CancelClick" UseAutoSkinMode="false" ButtonSkin="Silk">
        </infsu:CommandBar>
    </div>
    <div class="row" runat="server" id="divUnArchivalRequests" style="margin-top: 20px">
        <infs:WclGrid runat="server" ID="grdUnArchivalRequests"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false" ClearFiltersButtonText="Clear filters"
            OnNeedDataSource="grdUnArchivalRequests_NeedDataSource"
            OnItemDataBound="grdUnArchivalRequests_ItemDataBound"
            EnableLinqExpressions="false" NonExportingColumns="SelectSubscription" ShowClearFiltersButton="true" UseAutoSkinMode="false" ButtonSkin="Silk">
            <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="UnArchiveRequestId">
                <CommandItemSettings ShowAddNewRecordButton="false"
                    ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"
                    ShowRefreshButton="true" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="SelectSubscription" AllowFiltering="false"
                        ShowFilterIcon="false">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectItem" runat="server" onclick="UnCheckHeader(this)" OnCheckedChanged="chkSelectItem_CheckedChanged" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="OrderNumber" HeaderText="Order ID"
                        SortExpression="OrderNumber" UniqueName="OrderID" FilterControlWidth="40px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PackageName" HeaderText="Package" SortExpression="PackageName"
                        UniqueName="PackageName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="HierarchyLabel" HeaderText="Institution Hierarchy" SortExpression="HierarchyLabel"
                        UniqueName="HierarchyLabel">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="FirstName" HeaderText="Applicant First Name"
                        SortExpression="FirstName" UniqueName="FirstName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="LastName" HeaderText="Applicant Last Name"
                        SortExpression="LastName" UniqueName="LastName">
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn DataField="ExpiryDate" HeaderText="Expiry Date" SortExpression="ExpiryDate" DataFormatString="{0:MM/dd/yyyy}" EnableTimeIndependentFiltering="true"
                        FilterControlWidth="125px" UniqueName="ExpiryDate">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridDateTimeColumn DataField="ArchiveDate" HeaderText="Archival Date" SortExpression="ArchiveDate" DataFormatString="{0:MM/dd/yyyy}" EnableTimeIndependentFiltering="true"
                        FilterControlWidth="125px" UniqueName="ArchiveDate">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridDateTimeColumn DataField="UnarchiveRequestDate" HeaderText="Un-Archival Request Date" SortExpression="UnArchiveRequestDate" DataFormatString="{0:MM/dd/yyyy}" EnableTimeIndependentFiltering="true"
                        FilterControlWidth="130px" UniqueName="UnarchiveRequestDate" Display="false">
                    </telerik:GridDateTimeColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>

    <div style="padding: 10px; text-align: center" runat="server" id="divApproveRejectButtons">
        <infs:WclButton ID="btnApprove" runat="server" AutoSkinMode="false" Skin="Silk"
            AutoPostBack="true" Text="Approve Request(s)" OnClick="btnApprove_Click" />
        <infs:WclButton ID="btnRejection" runat="server" AutoSkinMode="false" Skin="Silk"
            AutoPostBack="true" Text="Reject Request(s)" OnClick="btnRejection_Click" />
    </div>
    <div style="padding: 10px; text-align: center" runat="server" id="divUnarchiveButton">
        <infs:WclButton ID="btnUnarchive" runat="server"
            AutoPostBack="true" Text="Un-Archive" OnClick="btnUnarchive_Click" AutoSkinMode="false" Skin="Silk">
            <Icon PrimaryIconCssClass="rbArchive" />
        </infs:WclButton>
    </div>
</div>


