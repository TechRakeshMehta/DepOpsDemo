<%@ Page Title="InstitutionConfigurationDetails" Language="C#" MasterPageFile="~/Shared/ChildPage.master" AutoEventWireup="true"
    CodeBehind="InstitutionConfigurationBundleDetails.aspx.cs" Inherits="CoreWeb.SystemSetUp.Views.InstitutionConfigurationBundleDetails"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>--%>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/mapping_pages.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
 
    <div class="container-fluid">
        <div class="co-md-12">
            <div class="row">&nbsp;</div>
        </div>
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-10"></div>
                <div class="col-md-2 text-right">
                    <infs:WclButton runat="server" ID="btnBackToQueue" Text="Back To Queue"
                        OnClick="btnBackToQueue_Click" Height="30px" ButtonType="LinkButton" Skin="Silk"
                        AutoSkinMode="false">
                    </infs:WclButton>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div id="divRefund" runat="server">
                    <h2 class="header-color heighAuto">
                        <asp:Label ID="lblNodeTitle" runat="server"></asp:Label>
                    </h2>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
            <div class="row bgLightGreen">
                <div class="col-md-12">
                   <%-- <infs:WclButton runat="server" ID="btnDummy" UseAutoSkinMode="false" ButtonSkin="Silk">
                    </infs:WclButton>--%>
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
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="PackageID,IsCompliancePackage,IsParentPackage,PackageHierarchyID,PackageType"
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
                        </Columns>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
                    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </infs:WclGrid>
            </div>
            </div>
        </div>
    </div>
</asp:Content>
