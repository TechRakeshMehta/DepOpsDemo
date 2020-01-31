<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridViewControl.ascx.cs" Inherits="CoreWeb.PlacementMatching.Views.GridViewControl" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxPlacementGrid">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style>
    .togglebutton {
        width: 50%;
        height: 100%;
        font-size: 16px;
        color: white;
        float: left;
        text-align: center;
    }

    #box_content i, #box_content em {
        font-style: normal;
    }
</style>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-7"></div>
            <div class="col-md-3" style="margin-left: 95px;">
                <asp:LinkButton ID="btnGrid" CssClass="togglebutton" runat="server" Enabled="false" BackColor="Gray" Font-Underline="false">
                   <i class="fa fa-th"></i> Grid
                </asp:LinkButton>
                <asp:LinkButton ID="btnCalender" CssClass="togglebutton" runat="server" Enabled="true" BackColor="#8C1921" OnClick="btnCalender_Click" Font-Underline="false">
                  <i class="fa fa-calendar"></i> Calender
                </asp:LinkButton>
            </div>
        </div>
    </div>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdPlacementMatchingMapping" AllowPaging="true" AutoGenerateColumns="false" CssClass="gridhover "
            AllowSorting="true" AllowFilteringByColumn="false" AutoSkinMode="true" CellSpacing="0" GridLines="Both"
            ShowAllExportButtons="false" ShowExtraButtons="false" AllowCustomPaging="false" ShowClearFiltersButton="false"
            PageSize="10" OnNeedDataSource="grdPlacementMatchingMapping_NeedDataSource" OnItemCommand="grdPlacementMatchingMapping_ItemCommand" OnSortCommand="grdPlacementMatchingMapping_SortCommand"
            OnItemDataBound="grdPlacementMatchingMapping_ItemDataBound">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <GroupingSettings CaseSensitive="false" />
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="RequestId,OpportunityId" AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="false" ShowRefreshButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <%-- <telerik:GridNumericColumn DataField="RequestID" FilterControlAltText="Filter Agency column"
                    HeaderText="PL ID" SortExpression="RequestID" UniqueName="RequestID"
                    HeaderTooltip="This column displays the RequestID">
                </telerik:GridNumericColumn>--%>
                    <telerik:GridNumericColumn DataField="RequestId" FilterControlAltText="Filter PlacementID column"
                        HeaderText="PL ID" SortExpression="RequestId" UniqueName="RequestId"
                        HeaderTooltip="This column displays the Placement ID ">
                    </telerik:GridNumericColumn>
                    <telerik:GridNumericColumn DataField="AgencyName" FilterControlAltText="Filter Agency column"
                        HeaderText="Agency" SortExpression="AgencyName" UniqueName="AgencyName"
                        HeaderTooltip="This column displays the Agency Name">
                    </telerik:GridNumericColumn>
                    <telerik:GridBoundColumn DataField="Location" FilterControlAltText="Filter Location column"
                        HeaderText="Location" SortExpression="FirstName" UniqueName="Location"
                        HeaderTooltip="This column displays the Location for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Department" FilterControlAltText="Filter LastName column"
                        HeaderText="Dept." SortExpression="Department" UniqueName="Department"
                        HeaderTooltip="This column displays the Department for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Specialty" FilterControlAltText="Filter Speciality column"
                        HeaderText="Specialty" SortExpression="Specialty" UniqueName="Specialty"
                        HeaderTooltip="This column displays the Specialty for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="StudentTypes" FilterControlAltText="Filter StudentType column"
                        HeaderText="Student Type" SortExpression="StudentTypes" UniqueName="StudentTypes"
                        HeaderTooltip="This column displays the Student Type for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Max" FilterControlAltText="Filter Max column"
                        HeaderText="Max #" SortExpression="Max" UniqueName="Max"
                        HeaderTooltip="This column displays the Max for each record in the grid">
                    </telerik:GridBoundColumn>
                    <%--  <telerik:GridBoundColumn DataField="Max" FilterControlAltText="Filter Max column"
                        HeaderText="Max #" SortExpression="Max" UniqueName="Max"
                        HeaderTooltip="This column displays the Max for each record in the grid">
                    </telerik:GridBoundColumn>--%>
                    <telerik:GridBoundColumn DataField="Days" FilterControlAltText="Filter Days column"
                        HeaderText="Days" SortExpression="Days" UniqueName="Days"
                        HeaderTooltip="This column displays the Days for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Shift" FilterControlAltText="Filter Shift column"
                        HeaderText="Shift" SortExpression="Shift" UniqueName="Shift"
                        HeaderTooltip="This column displays the Shift for each record in the grid">
                    </telerik:GridBoundColumn>
                   
                    <telerik:GridButtonColumn ButtonType="PushButton" CommandName="Details"
                        FilterControlAltText="Filter DeleteColumn column" HeaderText="DETAILS" Text="DETAILS"
                        UniqueName="CreateDraft" Resizable="false">
                        <HeaderStyle CssClass="rgHeader ButtonColumnHeader"></HeaderStyle>
                        <ItemStyle CssClass="ButtonColumn" />
                    </telerik:GridButtonColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" Position="TopAndBottom" />
            </MasterTableView>
        </infs:WclGrid>


    </div>

</div>
