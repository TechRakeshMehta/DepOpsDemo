<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.PackageSubscription" CodeBehind="PackageSubscription.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/OrderHistory.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="section">
    <h1 class="mhdr">Purchased Subscription(s)</h1>
    <div class="content">
        <div id="dvPurchasedSubscription" runat="server" class="sxform auto">
            <infs:WclGrid runat="server" ID="grdPurchasedSubscription" AllowPaging="false" AutoGenerateColumns="False"
                PagerStyle-AlwaysVisible="false" AutoSkinMode="True" CellSpacing="0" EnableDefaultFeatures="True"
                ShowAllExportButtons="False" ShowExtraButtons="true" GridLines="None" OnNeedDataSource="grdPurchasedSubscription_NeedDataSource"
                OnItemDataBound="grdPurchasedSubscription_ItemDataBound" OnItemCommand="grdPurchasedSubscription_ItemCommand"
                ShowClearFiltersButton="false">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    HideStructureColumns="true" Pdf-PageWidth="297mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm">
                    <Excel AutoFitImages="true" />
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <ClientEvents OnRowDblClick="grd_rwDbClick" />
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView AllowFilteringByColumn="false" AllowPaging="false" AllowSorting="false"
                    PageSize="50" CommandItemDisplay="Top" DataKeyNames="CompliancePackageID,OrderID,PackageSubscriptionID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="false"
                        ShowExportToPdfButton="false" ShowExportToWordButton="false" ShowRefreshButton="false" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="InstituteHierarchy" FilterControlAltText="Filter InstituteHierarchy column"
                            HeaderText="Institution Hierarchy" SortExpression="InstituteHierarchy" UniqueName="InstituteHierarchy" HeaderTooltip="A description of your program within your school">
                        </telerik:GridBoundColumn>
                        <%--  <telerik:GridBoundColumn DataField="PackageName" FilterControlAltText="Filter PackageName column"
                            HeaderText="Package" SortExpression="PackageName" UniqueName="PackageName">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridTemplateColumn FilterControlAltText="Filter PackageName column" HeaderText="Package Name"
                            UniqueName="PackageName" HeaderTooltip="The collection of requirements for the programs to which you are subscribing">
                            <ItemTemplate>
                                <asp:Literal ID="litPackage" runat="server" Text='<%# String.IsNullOrEmpty(Convert.ToString(Eval("PackageLabel"))) ? INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("PackageName"))) : INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("PackageLabel"))) %>'></asp:Literal>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="SubscriptionStatus" FilterControlAltText="Filter SubscriptionStatus column"
                            HeaderText="Time Remaining" SortExpression="SubscriptionStatus" UniqueName="SubscriptionStatus" HeaderTooltip="The number of days remaining in your subscription">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridButtonColumn ButtonType="LinkButton"  HeaderText="Subscription Renewal" CommandName="RenewSubscription"
                            Text="Renew Subscription" UniqueName="RenewSubscriptionColumn"  Visible="False">
                            <HeaderStyle CssClass="tplcohdr" />
                        </telerik:GridButtonColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Subscription Renewal" AllowFiltering="false"
                            Visible="true" UniqueName="RenewSubscriptionColumn" HeaderTooltip="Click a link in this column to renew your subscription before or after it expires">
                            <ItemTemplate>
                                <%--<a runat="server" visible="false" id="ancRenewSubscription" title="Click to renew your subscription before or after it expires">Renew Subscription</a>--%>
                                <asp:LinkButton ID="lnkbtnRenewSubscription" runat="server" Visible="false" ToolTip="Click to renew your subscription before or after it expires"
                                    Text="Renew Subscription" CommandName="RenewSubscription"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="ComplianceStatus" FilterControlAltText="Filter ComplianceStatus column"
                            HeaderText="Compliance Status" SortExpression="ComplianceStatus" UniqueName="ComplianceStatus" HeaderTooltip="Your overall compliance status">
                        </telerik:GridBoundColumn>
                        <%-- <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="EnterData" HeaderText=""
                            Text="Enter Data" UniqueName="EnterDataColumn">
                            <HeaderStyle CssClass="tplcohdr" />

                        </telerik:GridButtonColumn>--%>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="EnterDataColumn" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnEnterData" ButtonType="LinkButton" CommandName="EnterData"
                                    runat="server" Text="Enter Data" BackColor="Transparent" Font-Underline="true" BorderStyle="None" ForeColor="Black">
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="PendingProgramDuration" FilterControlAltText="Filter PendingProgramDuration column"
                            HeaderText="PendingProgramDuration" SortExpression="PendingProgramDuration" UniqueName="PendingProgramDuration"
                            Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IsOrderApproved" FilterControlAltText="Filter IsOrderApproved column"
                            HeaderText="IsOrderApproved" SortExpression="IsOrderApproved" UniqueName="IsOrderApproved"
                            Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SubscriptionMonth" FilterControlAltText="Filter SubscriptionMonth column"
                            HeaderText="SubscriptionMonth" SortExpression="SubscriptionMonth" UniqueName="SubscriptionMonth"
                            Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SubscriptionYear" FilterControlAltText="Filter SubscriptionYear column"
                            HeaderText="SubscriptionYear" SortExpression="SubscriptionYear" UniqueName="SubscriptionYear"
                            Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ProgramDuration" FilterControlAltText="Filter ProgramDuration column"
                            HeaderText="ProgramDuration" SortExpression="ProgramDuration" UniqueName="ProgramDuration"
                            Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ExpiryDate" FilterControlAltText="Filter ExpiryDate column"
                            HeaderText="ExpiryDate" SortExpression="ExpiryDate" UniqueName="ExpiryDate" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="" UniqueName="UnArchiveRequestColumn">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnUnArchiveRequest" runat="server" Visible="false" Text="Send Un-archive Request"
                                    CommandName="UnArchiveRequest"></asp:LinkButton>
                                <asp:Label ID="lblUnArchiverequestSent" Text="Un-archive request sent" Visible="false" runat="server"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="tplcohdr" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="ChangeSubscription"
                            HeaderText="" Text="Change Program" UniqueName="ChangeSubscriptionColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                        </telerik:GridButtonColumn>
                        <%--<telerik:GridButtonColumn ButtonType="LinkButton" CommandName="ChangeInstitution"  HeaderText="Change Institution" Text="Change Institution"
                            UniqueName="ChangeInstitutionColumn" >
                            <HeaderStyle CssClass="tplcohdr" />
                        </telerik:GridButtonColumn> --%>
                    </Columns>
                    <PagerStyle AlwaysVisible="false" Visible="false"></PagerStyle>
                </MasterTableView>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
    </div>
</div>

