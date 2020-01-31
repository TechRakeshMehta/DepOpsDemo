<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstitutionConfigurationDetails.aspx.cs"
    Inherits="CoreWeb.SystemSetUp.Views.InstitutionConfigurationDetails" MaintainScrollPositionOnPostback="true"
    MasterPageFile="~/Shared/ChildPage.master" Title="InstitutionConfigurationDetails" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" language="javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
        });

        function KeyPress(sender, args) {
            if (args.get_keyCharacter() == sender.get_numberFormat().DecimalSeparator || args.get_keyCharacter() == '-') {
                args.set_cancel(true);
            }
        }
    </script>
    <infs:WclResourceManagerProxy runat="server" ID="rprxSetUp">
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/SystemSetUp/InstitutionConfiguration.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
     
    <div class="container-fluid">
        <div id="divHierarchyNodePackage" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <asp:Label ID="lblMessage" runat="server" CssClass="header-color"> </asp:Label>

                </div>
            </div>
            <div class="row">
                <div class="col-md-12" style="">
                    <div class="row">
                        <div class="form-group col-md-8">
                        </div>
                        <div class="form-group col-md-4">

                            <telerik:RadButton ID="btnExportConfigurationReport" ButtonType="StandardButton" Skin="Silk"
                                runat="server" Text="Export Institution Configuration Report" OnClick="btnExportConfigurationReport_Click">
                            </telerik:RadButton>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row bgLightGreen" id="divOverallComplianceStatus" runat="server">
                <div class="col-md-12 ">
                    <h2>
                        <asp:Label ID="lblOverallComplianceStatusTitle" runat="server" Text="Overall Compliance Status Setting" CssClass="header-color"></asp:Label>
                    </h2>
                </div>
                <div id="divOvrComStatus" runat="server" visible="true">
                    <asp:Panel ID="pnlOvrComStatus" runat="server">
                        <div class='col-md-12'>
                            <div class="row">
                                <div class="form-group col-md-7">
                                    <span class="cptn">Approved to Pending review category has no impact on Overall Compliance</span>
                                    <asp:Label ID="lblOverallComplianceStatusValue" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
            <hr style="border-bottom: solid 1px #c0c0c0;" />
            <div class="row bgLightGreen" id="divNodePackage" runat="server">
                <div class="col-md-12 ">
                    <h2>
                        <asp:Label ID="lblNodeTitle" runat="server" Text="" CssClass="header-color"></asp:Label>
                    </h2>
                </div>
                <div id="divShowNode" runat="server" visible="true">
                    <asp:Panel ID="pnlNode" runat="server">
                        <div class='col-md-12'>
                            <div class="row">
                                <div class="form-group col-md-3">
                                    <span class="cptn">Splash Screen URL</span>
                                    <asp:Label ID="lblSplashURL" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
            <hr style="border-bottom: solid 1px #c0c0c0;" />
            <div class="row bgLightGreen">
                <div class="col-md-12">
                    <infs:WclButton runat="server" ID="btnDummy" UseAutoSkinMode="false" ButtonSkin="Silk">
                    </infs:WclButton>
                    <h2>
                        <asp:Label ID="lblPackages" runat="server" Text="Packages" CssClass="header-color"></asp:Label>
                    </h2>
                </div>
            </div>
            <div class="row">
                <infs:WclGrid runat="server" ID="grdPackages" AllowPaging="true" AutoGenerateColumns="False"
                    AllowSorting="True" AutoSkinMode="True" CellSpacing="0"
                    GridLines="Both" EnableDefaultFeatures="true" ShowAllExportButtons="False" ShowExtraButtons="True"
                    EnableLinqExpressions="false"
                    PageSize="10" OnNeedDataSource="grdPackages_NeedDataSource" OnItemCommand="grdPackages_ItemCommand" OnItemDataBound="grdPackages_ItemDataBound"
                    ShowClearFiltersButton="true" ClearFiltersButtonText="Clear Filters">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="PackageID,IsCompliancePackage,IsParentPackage,PackageHierarchyID,PackageType,PackageName,Fee"
                        AllowFilteringByColumn="true">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false"
                            ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowRefreshButton="true" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="PackageName" HeaderText="Package Name" SortExpression="PackageName"
                                UniqueName="PackageName"
                                FilterControlAltText="Filter PackageName column" AllowFiltering="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PackageType" FilterControlAltText="Filter PackageType column"
                                AllowFiltering="true"
                                HeaderText="Package Type" SortExpression="PackageType" UniqueName="PackageType">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Fee" DataFormatString="{0:c}" FilterControlAltText="Filter Price column"
                                AllowFiltering="true"
                                HeaderText="Price" SortExpression="Fee" UniqueName="Fee">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SubscriptionOption" FilterControlAltText="Filter SubscriptionOption column"
                                AllowFiltering="true"
                                HeaderText="Subscription Option" SortExpression="SubscriptionOption" UniqueName="SubscriptionOption">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PaymentMethods"
                                HeaderText="Payment Options" SortExpression="PaymentMethods" HeaderStyle-Width="300px"
                                UniqueName="PaymentMethods">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <telerik:RadButton ID="btnEdit" ButtonType="LinkButton" CommandName="ViewDetail"
                                        runat="server" Text="View Details">
                                    </telerik:RadButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%-- <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="ViewDetail" Text="View Details"
                                ItemStyle-Width="100px" UniqueName="ViewDetail">
                            </telerik:GridButtonColumn>--%>
                        </Columns>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
                    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </infs:WclGrid>
            </div>

            <hr style="border-bottom: solid 1px #c0c0c0;" />
            <div class="row bgLightGreen">
                <div class="col-md-12">
                    <h2>
                        <asp:Label ID="lblAdministrators" runat="server" Text="Administrators" CssClass="header-color"></asp:Label>
                    </h2>
                </div>
            </div>
            <div class="row">
                <infs:WclGrid runat="server" ID="grdAdministrators" CssClass="removeExtraSpace" AllowPaging="false"
                    AutoGenerateColumns="False" AllowSorting="True" GridLines="Both" OnNeedDataSource="grdAdministrators_NeedDataSource"
                    OnItemCommand="grdAdministrators_ItemCommand"
                    ShowAllExportButtons="False" EnableDefaultFeatures="false">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrganizationUserId">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false"
                            ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="UserFirstName"
                                HeaderText="First Name" SortExpression="UserFirstName" UniqueName="UserFirstName"
                                HeaderStyle-Width="130">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UserLastName"
                                HeaderText="Last Name" SortExpression="UserLastName" UniqueName="UserLastName"
                                HeaderStyle-Width="130">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UserName"
                                HeaderText="User Name" HeaderStyle-Width="50px" SortExpression="UserName" UniqueName="UserName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ComliancePermissionName"
                                HeaderText="Compliance Permission" SortExpression="ComliancePermissionName" UniqueName="ComliancePermissionName"
                                HeaderStyle-Width="130">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="BkgPermissionName"
                                HeaderText="Background Permission" SortExpression="BkgPermissionName" UniqueName="BkgPermissionName"
                                HeaderStyle-Width="130">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="VerificationPermissionName"
                                HeaderText="Verification Permission" SortExpression="VerificationPermissionName"
                                UniqueName="VerificationPermissionName"
                                HeaderStyle-Width="130">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ProfilePermissionName"
                                HeaderText="Profile Permission" SortExpression="ProfilePermissionName" UniqueName="ProfilePermissionName"
                                HeaderStyle-Width="130">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="OrderQueuePermissionName"
                                HeaderText="Order Queue Permission" SortExpression="OrderQueuePermissionName"
                                UniqueName="OrderQueuePermissionName"
                                HeaderStyle-Width="130">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="PackagePermissionName"
                                HeaderText="Package Permission" SortExpression="PackagePermissionName"
                                UniqueName="PackagePermissionName"
                                HeaderStyle-Width="130">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="EmailAddress"
                                HeaderText="Email Address" SortExpression="EmailAddress"
                                UniqueName="EmailAddress"
                                HeaderStyle-Width="130">
                            </telerik:GridBoundColumn>
                            <%--<telerik:GridBoundColumn DataField="IsActive" FilterControlAltText="Filter Active column"
                                HeaderText="Active" HeaderStyle-Width="50px" SortExpression="IsActive" UniqueName="IsActive">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridTemplateColumn DataField="IsActive" FilterControlAltText="Filter IsActive column"
                                HeaderText="Is Active" SortExpression="IsActive" UniqueName="IsActive">
                                <ItemTemplate>
                                    <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("IsActive")) == true ? Convert.ToString("Yes") : Convert.ToString("No") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail">
                                <ItemTemplate>
                                    <telerik:RadButton ID="btnDetails" ButtonType="LinkButton" CommandName="ViewDetail"
                                        ToolTip="Click here to view the permission information of the user" runat="server"
                                        Text="Detail">
                                    </telerik:RadButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--  <telerik:GridButtonColumn ButtonType="LinkButton"  HeaderTooltip="Click here to view the permission information of the user" CommandName="ViewDetail" Text="Detail"
                                ItemStyle-Width="100px" UniqueName="ViewDetail">
                            </telerik:GridButtonColumn>--%>
                        </Columns>
                    </MasterTableView>
                </infs:WclGrid>
            </div>
            <div class="row">&nbsp;</div>
        </div>
    </div>
    <iframe id="ifrExportDocument" runat="server" height="0" width="0"></iframe>
</asp:Content>


