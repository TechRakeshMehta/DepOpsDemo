<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BkgOrderServiceLinePriceInfo.ascx.cs"
    Inherits="CoreWeb.BkgOperations.Views.BkgOrderServiceLinePriceInfo" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<style>
    .rgMasterTable td {
        word-wrap: break-word;
        word-break: break-all;
    }
</style>
<infs:WclResourceManagerProxy runat="server" ID="rprxManageInvitationExpiration">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Background Package</h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <div class="msgbox">
            <asp:Label ID="lblSuccess" runat="server" Visible="false"></asp:Label>
        </div>
        <asp:Panel runat="server" ID="Panel1">
            <asp:Repeater runat="server" ID="rptrBackgroundPackage">                
                <ItemTemplate>                                                                                                                          
                    <div class='col-md-6 '>
                        <div class="row">
                            <div class='col-md-6'>
                                <span class='cptn'>Package Name</span>
                                <asp:Label runat="server" style="height:auto !important;line-height:14px !important" CssClass="form-control" ID="lblPackageName"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("BkgPackageName"))) %></asp:Label>
                            </div>
                            <div class='col-md-6'>
                                <span class='cptn'>Package Price</span>
                                <asp:Label runat="server" CssClass="form-control" ID="lblPackagePrice"><%# String.Format("{0:c}",Eval("BkgPackagePrice")) %></asp:Label>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </asp:Panel>
    </div>

    <div class="dummyBtn" style="display: none;">
        <infs:WclButton runat="server" ID="WclButton2" Text=""
            ButtonType="LinkButton" Height="30px">
        </infs:WclButton>
    </div>
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Service Price</h2>
        </div>
    </div>
    <div class="row">
        <div id="Div1" runat="server">
            <infs:WclGrid runat="server" ID="grdOrderServiceLinePriceInfo" AutoGenerateColumns="false"
                AllowSorting="True" AutoSkinMode="True" CellSpacing="0" ShowFooter="true" CssClass="removeExtraSpace"
                GridLines="Both" ShowAllExportButtons="False" ShowExtraButtons="True" OnNeedDataSource="grdOrderServiceLinePriceInfo_NeedDataSource"
                EnableDefaultFeatures="false">
                <MasterTableView CommandItemDisplay="Top" AllowFilteringByColumn="false" PagerStyle-Font-Bold="true">
                    <CommandItemSettings ShowAddNewRecordButton="false"
                        ShowExportToExcelButton="False" ShowExportToPdfButton="False" ShowExportToCsvButton="False"
                        ShowRefreshButton="False" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="BackgroundServiceName"
                            HeaderText="Service Information" SortExpression="BackgroundServiceName" UniqueName="BackgroundServiceName">
                            <ItemStyle Wrap="true" Width="400px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Amount" Aggregate="Sum" FooterText="Total Amount: $"
                            HeaderText="Amount" SortExpression="Amount" UniqueName="Amount" DataFormatString="{0:c}"
                            HeaderStyle-Width="100px">
                            <FooterStyle Font-Bold="true" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AdjAmount" Aggregate="Sum" FooterText="Total Adj Amount: $"
                            HeaderText="Adj Amount" SortExpression="AdjAmount" UniqueName="Amount" DataFormatString="{0:c}"
                            HeaderStyle-Width="100px">
                            <FooterStyle Font-Bold="true" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NetAmount" Aggregate="Sum" FooterText="Total Net Amount: $"
                            HeaderText="Net Amount" SortExpression="NetAmount" UniqueName="NetAmount" DataFormatString="{0:c}"
                            HeaderStyle-Width="100px">
                            <FooterStyle Font-Bold="true" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Description"
                            HeaderText="Adjustment Information" SortExpression="Description" UniqueName="Description">
                        </telerik:GridBoundColumn>
                        <%--  <telerik:GridNumericColumn DataField="Amount" HeaderText="Ext. Price" AllowSorting="true" Aggregate="Sum" FooterText="Total Sales: $"
                            SortExpression="ExtPrc" UniqueName="ExtPrc" HeaderTooltip="This column displays the Extension Price in dollars for each record in the grid">
                        </telerik:GridNumericColumn>--%>
                    </Columns>
                </MasterTableView>
            </infs:WclGrid>
        </div>
        <div class="dummyBtn" style="display: none;">
            <infs:WclButton runat="server" ID="WclButton1" Text=""
                ButtonType="LinkButton" Height="30px">
            </infs:WclButton>
        </div>
    </div>
</div>
<div class="container-fluid">
    <div id="divOtherCharges" runat="server" visible="false">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Other Services</h2>
            </div>
        </div>
        <div class="col-md-12">
            <div class="msgbox">
                <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
            </div>
        </div>
        <div class="row bgLightGreen">
            <asp:Panel runat="server" ID="Panel2">
                <div class="col-md-12">
                    <div class="row ">
                        <div class='form-group col-md-3'>
                            <span class='cptn'>Compliance Package Name</span>
                            <asp:Label runat="server" CssClass="form-control" ID="lblCompliancePackageName"></asp:Label>
                        </div>
                        <div class='form-group col-md-3'>
                            <span class='cptn'>Compliance Package Amount</span>
                            <asp:Label runat="server" CssClass="form-control" ID="lblCompliancePkgAmount"></asp:Label>
                        </div>
                        <div id="dvRushOrder" class='form-group col-md-3' runat="server" visible="false">
                            <span class='cptn'>Rush Order Price</span>
                            <asp:Label runat="server" CssClass="form-control" ID="lblRushOrderPrice"></asp:Label>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <div class="col-md-12">&nbsp;</div>
        </div>
    </div>
</div>
