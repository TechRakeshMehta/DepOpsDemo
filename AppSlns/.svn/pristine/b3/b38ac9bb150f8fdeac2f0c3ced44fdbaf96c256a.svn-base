<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.Search.Views.ApplicantPortfolioSearchMaster" Codebehind="ApplicantPortfolioSearchMaster.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/CustomAttributeLoaderSearch.ascx"
    TagName="CustomAttributeLoaderSearch" TagPrefix="uc" %>
<%@ Register Src="~/SearchUI/ApplicantPortFolioSearch_Copy.ascx" TagName="ApplicantPortFolioSearch_Copy"
    TagPrefix="uc" %>
<telerik:RadTabStrip ID="RadTabStrip" SelectedIndex="0" runat="server" MultiPageID="RadMultiPage"
    Skin="Outlook" CssClass="NoBg" OnTabClick="RadTabStrip_TabClick">
    <Tabs>
        <telerik:RadTab runat="server" Text="Search" PageViewID="SearchPage">
        </telerik:RadTab>
        <telerik:RadTab runat="server" Text="Offline Search Results" PageViewID="SearchResultPage"
            Selected="True">
        </telerik:RadTab>
    </Tabs>
</telerik:RadTabStrip>
<telerik:RadMultiPage ID="RadMultiPage" runat="server" SelectedIndex="0">
    <telerik:RadPageView ID="SearchPage" runat="server">
        <uc:ApplicantPortFolioSearch_Copy runat="server" ID="ucAPFS_Online"  MasterPageTabIndex="0" />
    </telerik:RadPageView>
    <telerik:RadPageView ID="SearchResultPage" runat="server">
        <div class="section">
            <h1 class="mhdr">
                <asp:Label ID="lblApplicantSearchMaster" runat="server" Text="Offline Search Results"></asp:Label></h1>
            <div class="content">
                <div class="sxform auto">
                    <asp:Panel ID="pnlSearch" CssClass="sxpnl" runat="server">
                        <asp:Repeater ID="rptrSearchResults" runat="server" OnItemCommand="rptrSearchResults_ItemCommand"
                            OnItemDataBound="rptrSearchResults_ItemDataBound">
                            <HeaderTemplate>
                                <div class='sxro sx3co'>
                                    <h1>
                                        <div class='sxlb' style="text-align: center; width: 400px">
                                            <asp:Label ID="lblSearchName" runat="server" CssClass="cptn">Search Name</asp:Label>
                                        </div>
                                    </h1>
                                    <h1>
                                        <div class='sxlb' style="text-align: center">
                                            <asp:Label ID="lblSearchDate" runat="server" CssClass="cptn">Search Date</asp:Label>
                                        </div>
                                    </h1>
                                    <h1>
                                        <div class='sxlb' style="text-align: center">
                                            <asp:Label ID="lblStatus" runat="server" CssClass="cptn">Status</asp:Label>
                                        </div>
                                    </h1>
                                    <h1>
                                        <div class='sxlb' style="text-align: center">
                                            <asp:Label ID="lblRowCount" runat="server" CssClass="cptn">Record Count</asp:Label>
                                        </div>
                                    </h1>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class='sxro sx3co'>
                                    <div class='sxlb' style="text-align: center; width: 400px">
                                        <asp:Label ID="lblSearchName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SRI_SearchInstanceName")%>' CssClass="cptn"></asp:Label>
                                    </div>
                                    <div class='sxlb' style="text-align: center">
                                        <asp:Label ID="lblSearchDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SRI_CreatedOn")%>' CssClass="cptn"></asp:Label>
                                    </div>
                                    <div class='sxlb' style="text-align: center">
                                        <asp:Label ID="lblStatus" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Status")%>' CssClass="cptn"></asp:Label>
                                    </div>
                                    <div class='sxlb' style="text-align: center">
                                        <asp:Label ID="lblRowCount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SRI_TotalRecordCount")%>' CssClass="cptn"></asp:Label>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Button runat="server" ID="btnShowResult" CommandName="ShowResults" Text="View Results"
                                            BorderStyle="Solid" BorderWidth="1" Visible="true" Width="200px" Height="20px" />
                                        <asp:HiddenField ID="hdnSearchInstanceId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SRI_ID")%>' />
                                        <asp:HiddenField ID="hdnSearchParam" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SRI_SearchParams")%>' />
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </asp:Panel>
                </div>
            </div>
        </div>
        <div class="section">
            <h1 class="mhdr">
                <asp:Label ID="lblSearchResults" runat="server" Text="Search Results"></asp:Label>
            </h1>
            <div class="content">
              
                    <asp:Panel ID="Panel1" CssClass="sxpnl" runat="server">
                        <h5>
                            <asp:Label ID="lblSearchName" runat="server" Text=""></asp:Label>
                        </h5>
                        <uc:ApplicantPortFolioSearch_Copy runat="server" ID="ucAPFS_Offline" IsOfflineMode="true" MasterPageTabIndex="1" />
                    </asp:Panel>
               
            </div>
        </div>
    </telerik:RadPageView>
</telerik:RadMultiPage>
