<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstitutionConfigurationGlobalFeeDetails.aspx.cs" Inherits="CoreWeb.SystemSetUp.Views.InstitutionConfigurationGlobalFeeDetails" MaintainScrollPositionOnPostback="true"
    MasterPageFile="~/Shared/ChildPage.master" Title="InstitutionConfigurationGlobalFeeDetails" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/mapping_pages.css" ResourceType="StyleSheet" />
    </infs:WclResourceManagerProxy>

    <div class="page_cmd">
        <infs:WclButton runat="server" ID="btnBackToQueue" Text="Back To Package Detail" OnClick="btnBackToQueue_Click"
            Height="30px" ButtonType="LinkButton">
        </infs:WclButton>
    </div>
    <div class="section" runat="server" id="divMainSection">
        <h1 class="mhdr">
            <asp:Label ID="lblFeeItem" runat="server"></asp:Label>
            <asp:HiddenField ID="hdnFeeItemName" runat="server" />
        </h1>
        <div class="content">
            <div class="swrap">
                <infs:WclGrid runat="server" ID="grdFeeRecord" AllowPaging="True" AutoGenerateColumns="False"
                    AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                    EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true"
                    GridLines="Both" NonExportingColumns="" OnItemCreated="grdFeeRecord_ItemCreated"
                    OnNeedDataSource="grdFeeRecord_NeedDataSource"
                    EnableLinqExpressions="false">
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        HideStructureColumns="false" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                        Pdf-PageRightMargin="20mm">
                        <Excel AutoFitImages="true" />
                    </ExportSettings>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="SIFR_ID">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="true"
                            ShowExportToExcelButton="true" ShowExportToCsvButton="true"
                            ShowRefreshButton="true" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="PSIF_Name" FilterControlAltText="Filter FeeItem column"
                                HeaderText="Fee Item" SortExpression="PSIF_Name" UniqueName="PSIF_Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FieldValue" FilterControlAltText="Filter FieldValue column"
                                HeaderText="Applicable On" SortExpression="FieldValue" UniqueName="FieldValue">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SIFR_Amount" FilterControlAltText="Filter FieldAmount column"
                                HeaderText="Amount($)" DataType="System.Decimal" SortExpression="SIFR_Amount" UniqueName="SIFR_Amount">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
                    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </infs:WclGrid>
            </div>
        </div>
        <div class="gclr">
        </div>
    </div>
</asp:Content>
