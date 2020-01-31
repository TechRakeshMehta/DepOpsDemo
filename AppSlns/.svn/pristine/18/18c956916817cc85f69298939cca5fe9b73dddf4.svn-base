<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PackageBundleBkg.aspx.cs" Inherits="CoreWeb.BkgSetup.Views.PackageBundleBkg" MasterPageFile="~/Shared/ChildPage.master" %>


<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
        });

        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }
    </script>

    <div id="divContent" runat="server">
        <div class="section">
            <h1 class="mhdr">
                <asp:Label ID="lblTitle" runat="server" Text="Package Detail"></asp:Label>
            </h1>
            <div class="content">
                <div class="sxform auto">
                    <div class="msgbox">
                        <asp:Label ID="lblMessage" runat="server">
                        </asp:Label>
                    </div>
                    <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenant">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Is Exclusive</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rbtnExclusive" runat="server" RepeatDirection="Horizontal"
                                    CssClass="radio_list" AutoPostBack="false">
                                    <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="False" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvExclusive" ControlToValidate="rbtnExclusive"
                                        Display="Dynamic" CssClass="errmsg" Text="Is Exclusive is required." ValidationGroup="grpBundleSubmit" />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>

                    </asp:Panel>
                </div>
                <div class='sxroend'>
                </div>          
                    <infsu:CommandBar ID="CmdBar" runat="server" ButtonPosition="Right" DisplayButtons="Save"
                        AutoPostbackButtons="Save"
                        SaveButtonText="Save" SaveButtonIconClass="rbSave" OnSaveClick="CmdBarSave_Click"
                        DefaultPanelButton="Save" DefaultPanel="pnlTenant">
                    </infsu:CommandBar>

            </div>
        </div>
        <div class="section">
            <h1 class="mhdr">
                <asp:Label ID="lblBundleTitle" runat="server" Text="Bundle Package"></asp:Label>
            </h1>
            <div class="content">
                <div class="swrap">
                    <infs:WclGrid runat="server" ID="grdPackageBundleBkg" AllowPaging="True" AutoGenerateColumns="False"
                        AllowSorting="True" AllowCustomPaging="false" AllowFilteringByColumn="false" AutoSkinMode="True"
                        CellSpacing="0" GridLines="None" ShowAllExportButtons="False" EnableDefaultFeatures="True"
                        OnNeedDataSource="grdPackageBundleBkg_NeedDataSource">
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true"></Selecting>
                        </ClientSettings>
                        <GroupingSettings CaseSensitive="false" />
                        <MasterTableView CommandItemDisplay="Top">
                            <CommandItemSettings ShowAddNewRecordButton="false"
                                ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false"
                                ShowRefreshButton="false" />
                            <RowIndicatorColumn Visible="false" FilterControlAltText="Filter RowIndicator column">
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn Visible="false" FilterControlAltText="Filter ExpandColumn column">
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="PackageName" FilterControlAltText="Filter PackageName column"
                                    HeaderText="Package Name" SortExpression="PackageName" UniqueName="PackageName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PackageType" FilterControlAltText="Filter PackageType column"
                                    HeaderText="Package Type" SortExpression="PackageType" UniqueName="PackageType">
                                </telerik:GridBoundColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                        </MasterTableView>
                        <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                    </infs:WclGrid>
                </div>
                <div class="gclr">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
