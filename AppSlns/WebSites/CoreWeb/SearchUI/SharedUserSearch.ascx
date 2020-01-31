<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharedUserSearch.ascx.cs" Inherits="CoreWeb.SearchUI.Views.SharedUserSearch" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblSharedUserSearch" runat="server" Text="Manage Shared User Search"></asp:Label></h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel ID="pnlSearch" CssClass="sxpnl" runat="server">
                <div class='sxroend'>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb' title="Restrict search results to the entered first name">
                        <span class="cptn">User ID</span>
                    </div>
                    <div class='sxlm'>
                         <infs:WclNumericTextBox ID="txtUserId" runat="server" NumberFormat-DecimalDigits="0"
                            NumberFormat-AllowRounding="false" MaxLength="9">
                        </infs:WclNumericTextBox>
                    </div>
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
                    
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb' title="Restrict search results to the entered User Name">
                        <span class="cptn">Username</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtUserName" runat="server">
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

            <infs:WclGrid runat="server" ID="grdSharedUserSearch" AllowCustomPaging="True"
                AutoGenerateColumns="False" AllowSorting="True" AllowFilteringByColumn="false"
                AutoSkinMode="True" CellSpacing="0" GridLines="Both" ShowAllExportButtons="False"
                ShowClearFiltersButton="false" OnNeedDataSource="grdSharedUserSearch_NeedDataSource" OnItemDataBound="grdSharedUserSearch_ItemDataBound"
                OnItemCommand="grdSharedUserSearch_ItemCommand" OnSortCommand="grdSharedUserSearch_SortCommand"
                EnableLinqExpressions="false" NonExportingColumns="ViewDetail">
                <ClientSettings EnableRowHoverStyle="true">
                    <ClientEvents OnRowDblClick="grd_rwDbClick" />
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="SharedUserID" AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridNumericColumn DataField="SharedUserID" HeaderText="Shared User ID" SortExpression="SharedUserID"
                            UniqueName="SharedUserID" HeaderTooltip="This column displays the User ID for each record in the grid">
                        </telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="FirstName" FilterControlAltText="Filter First Name column"
                            HeaderText="First Name" SortExpression="FirstName" UniqueName="FirstName"
                            HeaderTooltip="This column displays the Shared User's first name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LastName" FilterControlAltText="Filter Last Name column"
                            HeaderText="Last Name" SortExpression="LastName" UniqueName="LastName"
                            HeaderTooltip="This column displays the Shared User's last name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserName" HeaderText="User Name" SortExpression="UserName"
                            HeaderTooltip="This column displays the Shared User UserName for each record in the grid"
                            UniqueName="UserName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EmailAddress" FilterControlAltText="Filter Email Address column"
                            HeaderText="Email Address" SortExpression="EmailAddress" UniqueName="EmailAddress"
                            HeaderTooltip="This column displays the Shared User's Email Address for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnDetails" ButtonType="LinkButton" CommandName="ViewDetail"
                                    ToolTip="Click here to display profile sharing information" runat="server"
                                    Text="Detail" BackColor="Transparent" Font-Underline="true" BorderStyle="None">
                                </telerik:RadButton>
                                <asp:HiddenField ID="hdnTenantID" runat="server"/>
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
<script type="text/javascript">
    //click on link button while double click on any row of grid.
    function grd_rwDbClick(s, e) {
        var _id = "btnDetails";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }
</script>

