<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RotationPackageCategoryDetailPopUp.aspx.cs" Inherits="CoreWeb.ClinicalRotation.Views.RotationPackageCategoryDetailPopUp"
    MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxRotationDetails">
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />

        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>

    <div class="container-fluid" style="padding-top: 20px;">
        <div class="row">
            <infs:WclGrid runat="server" ID="grdRequirementCategoryDetail" AllowCustomPaging="false"
                AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
                EnableLinqExpressions="false"  OnItemDataBound="grdRequirementCategoryDetail_ItemDataBound"
                AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"   
                ShowClearFiltersButton="false" OnNeedDataSource="grdRequirementCategoryDetail_NeedDataSource">
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings> 
                <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm" 
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true"  >
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                    <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="RequirementCategoryName" HeaderText="Category" HeaderStyle-Width ="150"
                            SortExpression="RequirementCategoryName" UniqueName="RequirementCategoryName" HeaderTooltip="This column displays the Requirement Category Name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ExplanatoryNotes" HeaderText="Category Description"
                            SortExpression="ExplanatoryNotes" UniqueName="ExplanatoryNotes" HeaderTooltip="This column displays the Requirement Category Description for each record in the grid">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                        Position="TopAndBottom" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
    </div>
</asp:Content>
