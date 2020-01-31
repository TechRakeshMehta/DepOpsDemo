<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientLoginSearch.ascx.cs" Inherits="CoreWeb.SearchUI.ClientLoginSearch" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
    <script src="../Resources/Mod/Shared/KeyBoardSupport.js"></script>
<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

</infs:WclResourceManagerProxy>

<style type="text/css">
    .controlHidden
    {
        display: none;
    }

    .disabled
    {
        pointer-events: none;
        cursor: default;
        text-decoration: none;
        color: gray !important;
    }

    .highlightGrid
    {
        color: red !important;
    }

    .highlight
    {
        border: 2px solid red !important;
        border-radius: 5px !important;
    }

    .top3
    {
        top: 3px !important;
    }

    .btn
    {
        width: 100%;
        text-align: left;
    }

    .RadMenu .rmGroup .rmText
    {
        padding: 0px;
        margin: 0px;
    }

    .rmVertical.rmGroup.rmLevel1
    {
        border: none;
    }
</style>


<div id="dvTop" class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color" tabindex="0">Manage Client Login
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <div class="sxform auto" id="divSearchPanel">
            <asp:Panel runat="server" ID="pnlSearch">
                <div class="col-md-12">
                    <div class="row">
                        <div id="divTenant" runat="server">
                            <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                                <span class="cptn">Institution</span><span class='reqd'>*</span>

                                <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName" DataValueField="TenantID"
                                    CausesValidation="false" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Filter="Contains" Width="100%"
                                    CssClass="form-control" Skin="Silk" AutoSkinMode="false" AutoPostBack="false" OnClientKeyPressing="openCmbBoxOnTab" Enabled="false">
                                    <Localization CheckAllString="All" />
                                </infs:WclComboBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                        Display="Dynamic" CssClass="errmsg" Text="Institution is required." ValidationGroup="grpFormSubmit" />
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="col-md-12">
                    <div class="row">
                        <div class='form-group col-md-3' title="Restrict search results to the entered first name">
                            <span class="cptn">First Name</span>
                            <infs:WclTextBox ID="txtFirstName" runat="server" Width="100%" CssClass="form-control">
                            </infs:WclTextBox>
                        </div>
                        <div class='form-group col-md-3' title="Restrict search results to the entered last name">
                            <span class="cptn">Last Name</span>
                            <infs:WclTextBox ID="txtLastName" runat="server" Width="100%" CssClass="form-control">
                            </infs:WclTextBox>
                        </div>
                        <div class='form-group col-md-3' title="Restrict search results to the entered email address">
                            <span class="cptn">User Name</span>
                            <infs:WclTextBox ID="txtUserName" runat="server" Width="100%" CssClass="form-control">
                            </infs:WclTextBox>
                        </div>
                        <div class='form-group col-md-3' title="Restrict search results to the entered email address">
                            <span class="cptn">Email Address</span>
                            <infs:WclTextBox ID="txtEmail" runat="server" Width="100%" CssClass="form-control">
                            </infs:WclTextBox>

                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class="col-md-12">&nbsp;</div>
        <div class="col-md-12">
            <div class="row ">
                <div style="width: 100%;">                    
                    <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                        AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo" ValidationGroup="grpFormSubmit"
                        SubmitButtonText="Reset" SaveButtonText="Search" SaveButtonIconClass="rbSearch"
                        CancelButtonText="Cancel" OnSubmitClick="fsucCmdBarButton_ResetClick" OnSaveClick="fsucCmdBarButton_SearchClick"
                        OnCancelClick="fsucCmdBarButton_CancelClick" UseAutoSkinMode="false" ButtonSkin="Silk">
                    </infsu:CommandBar>


                </div>
            </div>
            <div class="col-md-12">&nbsp;</div>
        </div>
    </div>

    <div class="row">
        <infs:WclGrid runat="server" ID="grdClientSearchData" AllowCustomPaging="True"
            AutoGenerateColumns="False" AllowSorting="True" AllowFilteringByColumn="false"
            AutoSkinMode="True" CellSpacing="0" GridLines="Both" ShowAllExportButtons="False"
            ShowClearFiltersButton="false" OnNeedDataSource="grdClientSearchData_NeedDataSource" OnItemDataBound="grdClientSearchData_ItemDataBound"
            OnItemCommand="grdClientSearchData_ItemCommand" OnSortCommand="grdClientSearchData_SortCommand"
            EnableLinqExpressions="false">
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="grd_rwDbClick" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrganizationUserId,UserID,ClientTenantID,IsActive" AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                ShowExportToExcelButton="true" ShowExportToPdfButton="true"/>
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridNumericColumn DataField="OrganizationUserId" HeaderText="User ID" SortExpression="OrganizationUserId" ItemStyle-Width="5%"
                        UniqueName="OrganizationUserId" HeaderTooltip="This column displays the User ID for each record in the grid">
                    </telerik:GridNumericColumn>

                    <telerik:GridNumericColumn DataField="TenantName" HeaderText="Tenant Name" SortExpression="TenantName" ItemStyle-CssClass="breakword" ItemStyle-Width="15%"
                        UniqueName="TenantName" HeaderTooltip="This column displays the Tenant Name for each record in the grid">
                    </telerik:GridNumericColumn>

                    <telerik:GridBoundColumn DataField="ClientFirstName" FilterControlAltText="Filter ClientFirstName column" ItemStyle-Width="15%"
                        HeaderText="First Name" SortExpression="ClientFirstName" UniqueName="ClientFirstName"
                        HeaderTooltip="This column displays the first name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ClientLastName" FilterControlAltText="Filter ApplicantLastName column" ItemStyle-Width="15%"
                        HeaderText="Last Name" SortExpression="ClientLastName" UniqueName="ApplicantLastName" AllowSorting="true"
                        HeaderTooltip="This column displays the last name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ClientUserName" FilterControlAltText="Filter ApplicantUserName column" ItemStyle-Width="15%"
                        HeaderText="User Name" SortExpression="ClientUserName" UniqueName="ClientUserName" AllowSorting="true"
                        HeaderTooltip="This column displays the user name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EmailAddress" FilterControlAltText="Filter EmailAddress column" ItemStyle-Width="15%"
                        HeaderText="Email Address" SortExpression="EmailAddress" UniqueName="EmailAddress" AllowSorting="true"
                        HeaderTooltip="This column displays the email address for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Phone" HeaderText="Phone Number" SortExpression="Phone" ItemStyle-Width="15%"
                        HeaderTooltip="This column displays the phone for each record in the grid"
                        UniqueName="Phone">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ClientView" ItemStyle-Width="25%">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnClientView" ButtonType="LinkButton" CommandName="ClientView"
                                runat="server" Text="Client's_Login" ToolTip="Click to login as client admin"
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
</div>

<script type="text/javascript">
    //click on link button while double click on any row of grid.    
    function grd_rwDbClick(s, e) {
        var _id = "btnDetails";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }
    function pageLoad() {
        SetDefaultButtonForSection("divSearchPanel", "fsucCmdBarButton_btnSave", true);
    }

    function OpenClientAdminUserView(navUrl) {
        var win = window.open(navUrl, '_blank');
        if (win) {
            //Browser has allowed it to be opened
            win.focus();
        }
    }
</script>
