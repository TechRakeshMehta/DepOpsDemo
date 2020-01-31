<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Search.Views.SearchMain" Codebehind="SearchMain.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <h1 class="mhdr">
        Search Entities</h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlName1">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Enter Search Text</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtSearchText">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Select Entity</span>
                    </div>
                    <div class='sxlm' style="margin-top: 0px">
                        <infs:WclComboBox ID="ddlSearchType" runat="server" AllowCustomText="true" MarkFirstMatch="true">
                            <Items>
                            </Items>
                        </infs:WclComboBox>
                    </div>
                    <div class='sxlm'>
                        <infsu:CommandBar ID="fsucCmdBar1" runat="server" DisplayButtons="Submit" OnSubmitClick="btnSearch_Click"
                            AutoPostbackButtons="Submit" SubmitButtonText="Search" SubmitButtonIconClass="rbSearch"
                            DefaultPanel="pnlName1" ButtonPosition="Center" />
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>
<div class="section">
    <h1 class="mhdr">
        Results</h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdUserSearchResult" AllowPaging="True" AllowSorting="True"
                AutoSkinMode="True" CellSpacing="0" EnableDefaultFeatures="True" ShowAllExportButtons="True" ShowClearFiltersButton="false"
                ShowExtraButtons="True" GridLines="None" OnNeedDataSource="grdUserSearchResult_NeedDataSource">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="true">
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="">
                    <CommandItemSettings ShowAddNewRecordButton="false" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <!-- enter edit form here or change edit form type to UserControl or Popup -->
                        </FormTemplate>
                    </EditFormSettings>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                    <WebServiceSettings>
                        <ODataSettings InitialContainerName="">
                        </ODataSettings>
                    </WebServiceSettings>
                </FilterMenu>
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                    <WebServiceSettings>
                        <ODataSettings InitialContainerName="">
                        </ODataSettings>
                    </WebServiceSettings>
                </HeaderContextMenu>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
    </div>
</div>
