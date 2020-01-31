<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageUniversalCategory.aspx.cs"
    Inherits="CoreWeb.ComplianceAdministration.Views.ManageUniversalCategory" Title="ManageUniversalCatgory"
    MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxetupUniversalMapping">
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <script type="text/javascript"> 
        function RefrshTree() {
            //debugger;
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }
    </script>
    <div class="container-fluid">
        <div class="col-md-12">
            <div class="row">&nbsp</div>
        </div>
        <div class="col-md-12">
            <div id="dvAddNewCategory" runat="server" style="text-align: right">
                <infs:WclButton runat="server" ID="btnAddUniCat" Text="Add Universal Category" OnClick="btnAddCategory_Click"
                    ButtonType="StandardButton" Icon-PrimaryIconCssClass="rbAddNew" Skin="Silk" AutoSkinMode="false">
                </infs:WclButton>
            </div>
        </div>
        <div id="dvAddCategory" runat="server" style="display: none">
            <div class="row">
                <div class="col-md-12">
                    <h2 class="header-color">
                        <asp:Label ID="lblHeader" runat="server"></asp:Label></h2>
                </div>
            </div>
            <div class="row bgLightGreen">
                <div class="col-md-12">
                    <div class="msgbox">
                        <asp:Label ID="lblSuccess" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                    </div>
                    <asp:Panel runat="server" ID="pnlCategory">
                        <div class="col-md-12">
                            <div class="row">
                                <div class='form-group col-md-3'>
                                    <span class="cptn">Category Name</span><span class="reqd">*</span>
                                    <infs:WclTextBox runat="server" ID="txtCategoryName" MaxLength="100" Enabled="true" Width="100%" CssClass="form-control">
                                    </infs:WclTextBox>
                                    <div class="vldx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvCategoryName" ControlToValidate="txtCategoryName" ValidationGroup="grp1"
                                            Display="Dynamic" CssClass="errmsg" Text="Category Name is required." />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="col-md-12">
                        <div class="row">
                            <div style="float: right" id="dvSaveCancelBtn" runat="server">
                                <infsu:CommandBar ID="fsucCmdBarSaveCategory" runat="server" SaveButtonIconClass="rbSave" CancelButtonIconClass="rbCancel"
                                    DisplayButtons="Save,Cancel" AutoPostbackButtons="Save,Cancel" ValidationGroup="grp1" UseAutoSkinMode="false" ButtonSkin="Silk"
                                    ButtonPosition="Right" OnSaveClick="fsucCmdBarSaveCategory_SaveClick" OnCancelClick="fsucCmdBarSaveCategory_CancelClick">
                                </infsu:CommandBar>
                            </div>
                            <div style="float: right" id="dvEditBtn" runat="server">
                                <infs:WclButton ID="btnEdit" Text="Edit" runat="server" OnClick="btnEdit_Click" Skin="Silk"
                                    AutoSkinMode="false" Icon-PrimaryIconCssClass="rbEdit">
                                </infs:WclButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-12"><div class="row">&nbsp</div></div>
        <div id="dvUniCatDetails" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <h2 class="header-color">Universal Category(s)</h2>
                </div>
            </div>
            <div class="row">
                <infs:WclGrid runat="server" ID="grdUniversalCategory" AllowPaging="True" AutoGenerateColumns="False"
                    AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0" OnItemCommand="grdUniversalCategory_ItemCommand"
                    EnableDefaultFeatures="true" ShowAllExportButtons="false" ShowExtraButtons="false" OnNeedDataSource="grdUniversalCategory_NeedDataSource">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        HideStructureColumns="true">
                    </ExportSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="UC_ID">
                        <CommandItemSettings ShowAddNewRecordButton="false" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="UC_Name" FilterControlAltText="Filter UC_Name column"
                                HeaderText="Category Name" SortExpression="UC_Name" UniqueName="UC_Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this universal category because deleting this universal category will remove all the mapping from tracking and rotation package?"
                                Text="Delete" UniqueName="DeleteColumn">
                                <HeaderStyle CssClass="tplcohdr" />
                                <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                            </telerik:GridButtonColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
                    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </infs:WclGrid>
            </div>
        </div>
        <div id="dvUniItemDetails" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <h2 class="header-color">Universal Items(s)</h2>
                </div>
            </div>
            <div class="row">
                <infs:WclGrid runat="server" ID="grdUniItems" AllowPaging="True" AutoGenerateColumns="False"
                    AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0" OnItemCommand="grdUniItems_ItemCommand"
                    EnableDefaultFeatures="true" ShowAllExportButtons="false" ShowExtraButtons="false" OnNeedDataSource="grdUniItems_NeedDataSource">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        HideStructureColumns="true">
                    </ExportSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="UI_ID">
                        <CommandItemSettings ShowAddNewRecordButton="false" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="UI_Name" FilterControlAltText="Filter UI_Name column"
                                HeaderText="Item Name" SortExpression="UI_Name" UniqueName="UI_Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this universal item because deleting this universal item will remove all the mapping from tracking and rotation package?"
                                Text="Delete" UniqueName="DeleteColumn">
                                <HeaderStyle CssClass="tplcohdr" />
                                <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                            </telerik:GridButtonColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
                    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </infs:WclGrid>
            </div>
        </div>
    </div>
</asp:Content>
