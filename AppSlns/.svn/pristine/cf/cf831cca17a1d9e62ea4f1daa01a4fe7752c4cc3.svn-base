<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceOperations.Views.MobilitiyNodePackages" Codebehind="MobilitiyNodePackages.ascx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<div class="section">
        <h1 class="mhdr">
            Future Levels
        </h1>
        <div class="content">
            <div class="sxform auto">
                <asp:Panel ID="Panel3" CssClass="sxpnl" runat="server">
                    <infs:WclGrid runat="server" ID="grdMobilityPackages" AllowPaging="false" AutoGenerateColumns="False"
                        PagerStyle-AlwaysVisible="false" AutoSkinMode="True" CellSpacing="0" EnableDefaultFeatures="false"
                        ShowAllExportButtons="False" ShowExtraButtons="true" NonExportingColumns="EditCommandColumn"
                        GridLines="None" OnItemDataBound="grdMobilityPackages_ItemDataBound" OnItemCommand="grdMobilityPackages_ItemCommand">
                        <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                            HideStructureColumns="true" Pdf-PageWidth="297mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                            Pdf-PageRightMargin="20mm">
                            <Excel AutoFitImages="true" />
                        </ExportSettings>
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true"></Selecting>
                        </ClientSettings>
                        <MasterTableView AllowFilteringByColumn="false" AllowPaging="false" AllowSorting="false"
                            PageSize="50" CommandItemDisplay="Top" DataKeyNames="CompliancePackageId">
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="false"
                                ShowExportToPdfButton="false" ShowExportToWordButton="false" ShowRefreshButton="false" />
                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="NodeName" HeaderText="Node" SortExpression="NodeName"
                                    UniqueName="NodeName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PackageName" HeaderText="Package" SortExpression="PackageName"
                                    UniqueName="PackageName">
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridTemplateColumn HeaderText="Price">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrice" Text='<%# String.Format("${0}",Eval("PackagePrice", "{0:0.00}"))  %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn> 
                                    <ItemTemplate>
                                        <infs:WclButton ID="btnDetails" ButtonType="LinkButton" runat="server" Text="View Details"
                                           BackColor="Transparent" Font-Underline="true" BorderStyle="None"  CommandName="ViewDetails">
                                        </infs:WclButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle AlwaysVisible="false" Visible="false"></PagerStyle>
                        </MasterTableView>
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                    </infs:WclGrid>
                </asp:Panel>
            </div>
        </div>
    </div>
