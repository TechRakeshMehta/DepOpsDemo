<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminEntryCountryIdentificationDetails.aspx.cs" Inherits="CoreWeb.AdminEntryPortal.Views.AdminEntryCountryIdentificationDetails" MasterPageFile="~/Shared/PopupMaster.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">
    <asp:Panel ID="pnlCountryidentificationDetails" CssClass="sxpnl" runat="server">
        <infs:WclGrid runat="server" ID="grdCountryIdentification" AllowPaging="false" AutoGenerateColumns="False"
            AllowSorting="false" AllowFilteringByColumn="true" AutoSkinMode="True" CellSpacing="0" PagerStyle-AlwaysVisible="false"
            GridLines="Both" EnableDefaultFeatures="false" ShowAllExportButtons="False" ShowExtraButtons="false"
            NonExportingColumns="EditCommandColumn,DeleteColumn" OnNeedDataSource="grdCountryIdentification_NeedDataSource">
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="CIN_ID" PagerStyle-AlwaysVisible="false">
                <CommandItemSettings ShowAddNewRecordButton="false"
                    ShowExportToCsvButton="False" ShowExportToExcelButton="False" ShowExportToPdfButton="False"
                    ShowRefreshButton="False" />
                <Columns>
                    <telerik:GridBoundColumn DataField="CIN_Country_Name" AllowFiltering="true" AllowSorting="false"
                        HeaderText="Country" SortExpression="CIN_Country_Name" UniqueName="Country">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CIN_Identification_Number" AllowFiltering="true" AllowSorting="false"
                        HeaderText="Identification" SortExpression="CIN_Identification_Number" UniqueName="Identification">
                    </telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
        </infs:WclGrid>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CommandContent" runat="server">
</asp:Content>
