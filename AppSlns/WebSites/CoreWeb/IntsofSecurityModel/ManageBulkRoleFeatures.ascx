<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageBulkRoleFeatures.ascx.cs" Inherits="CoreWeb.IntsofSecurityModel.Views.ManageBulkRoleFeatures" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxAdminView">
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/verification.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/ManageBulkFeatures.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<style type="text/css">
    html, body, #frmmod, #UpdatePanel1, #box_content
    {
        height: 100% !important;
        overflow: hidden;
    }
</style>

<infs:WclSplitter ID="sptrAdminView" runat="server" LiveResize="true" Orientation="Horizontal"
    Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
    <infs:WclPane ID="pnToolbar" runat="server" MinHeight="50" Height="50" Width="100%"
        Scrolling="None" Collapsed="false">
        <div id="modwrapo">
            <div id="modwrapi">
                <div id="breadcmb">
                    <infsu:breadcrumb ID="breadcrum" runat="server" />
                </div>
                <div id="modhdr">
                    <h1>
                        <asp:Label Text="Security" runat="server" ID="lblModHdr" />&nbsp;
                        <asp:Label Text="Manage Bulk features"
                            runat="server" ID="lblPageHdr" CssClass="phdr" /></h1>
                </div>
            </div>
        </div>
    </infs:WclPane>

    <infs:WclPane ID="pnLower" runat="server" Scrolling="None" Width="70%">
        <infs:WclSplitter ID="sptrMainView" runat="server" LiveResize="true" Orientation="vertical"
            Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
            <infs:WclPane ID="pnLeft" MinWidth="200" runat="server" Width="40%" Height="100%"
                PersistScrollPosition="true" Collapsed="false">
                <asp:Panel runat="server" ID="pnlUP" ScrollBars="Auto" Height="90%">
                    <div class="section">
                        <h1 class="mhdr">Select Feature(s)</h1>
                        <div class="content">
                            <div class="sxform auto">
                                <div class="content">
                                    <asp:Panel ID="pnlMapProductFeature" CssClass="sxpnl" runat="server">
                                        <div class='sxro sxco'>
                                            <div class='sxlb'>
                                                <asp:Label ID="lblBusinessChannelType" runat="server" Width="100%" AssociatedControlID="cmbBusinessChannelType" CssClass="cptn"
                                                    Text="Business Channel Type"></asp:Label><span class="reqd">*</span>
                                            </div>
                                            <div class='sxlm' style="border-width: 0px">
                                                <infs:WclComboBox ID="cmbBusinessChannelType" runat="server" DataTextField="Name"
                                                    DataValueField="BusinessChannelTypeID" AutoPostBack="true"
                                                    OnSelectedIndexChanged="cmbBusinessChannelType_SelectedIndexChanged" OnDataBound="cmbBusinessChannelType_DataBound">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvBusinessChannelType" ControlToValidate="cmbBusinessChannelType"
                                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="Business Channel Type is required."
                                                        ValidationGroup="grpValdManageBlock" />
                                                </div>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="sxro" style="background-color: grey; margin-bottom: 0px;">
                                <h3 style="margin-bottom: 0px; padding-left: 30px; color: black !important;"><b>Business Channel Type :</b>
                                    <asp:Label ID="lblBusinessChannelName" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <asp:HiddenField ID="hdnIfHeaderChecked" runat="server" />
                            <div class="swrap" style="background-color: grey;">
                                <div style="float: left; width: 5%">&nbsp;</div>
                                <div style="float: right; width: 92%;">
                                    <infs:WclTreeList runat="server" ID="treeListFeature" AllowPaging="false" OnItemDataBound="treeListFeature_ItemDataBound"
                                        DataKeyNames="ProductFeatureID" ParentDataKeyNames="ParentProductFeatureID"
                                        OnNeedDataSource="treeListFeature_NeedDataSource" OnInit="treeListFeature_Init" OnPreRender="treeListFeature_PreRender"
                                        AutoGenerateColumns="false" AllowMultiItemSelection="true">
                                        <Columns>
                                            <telerik:TreeListTemplateColumn HeaderStyle-Width="30px">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAllFeature" runat="server" onclick="CheckAll(this)" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkFeature" runat="server" parent='<%#DataBinder.Eval(Container.DataItem,"ParentProductFeatureID")%>'
                                                        fieldIndex='<%# DataBinder.Eval(Container.DataItem, "ProductFeatureID")%>'  />
                                                </ItemTemplate>
                                            </telerik:TreeListTemplateColumn>
                                            <telerik:TreeListBoundColumn DataField="Name" UniqueName="Name" HeaderText="Feature Name" />
                                            <telerik:TreeListBoundColumn DataField="ProductFeatureID" UniqueName="ProductFeatureID" Visible="false" />
                                        </Columns>
                                        <ClientSettings AllowPostBackOnItemClick="false">
                                        </ClientSettings>
                                    </infs:WclTreeList>

                                </div>
                            </div>
                            <div class="gclr">
                            </div>

                        </div>

                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlDown" Height="10%">
                    <div style="text-align: center; vertical-align: central; padding-top: 20px;">
                        <infs:WclButton ID="btnAddFeatures" ToolTip="Click to load User types" runat="server" Icon-PrimaryIconCssClass="rbNext"
                            Text="Next" ButtonPosition="Center" OnClick="btnAddFeatures_Click">
                        </infs:WclButton>
                    </div>
                </asp:Panel>
            </infs:WclPane>
            <infs:WclSplitBar runat="server" ID="spltBarLeft"></infs:WclSplitBar>
            <infs:WclPane ID="pnMiddle" runat="server" MinWidth="200" Width="30%" Height="100%"
                PersistScrollPosition="true">
                <asp:Panel runat="server" ID="pnlUTUp" ScrollBars="Auto" Height="90%">
                    <div class="section">
                        <h1 class="mhdr">Select User Type(s)</h1>
                        <div class="swrap">
                            <infs:WclGrid runat="server" ID="grdBlock" AllowPaging="false" AutoGenerateColumns="False"
                                AllowSorting="false" GridLines="Both" OnNeedDataSource="grdBlock_NeedDataSource"
                                AllowMultiRowEdit="false" EnableDefaultFeatures="false" NonExportingColumns="">
                                <ClientSettings EnableRowHoverStyle="true">
                                    <Selecting AllowRowSelect="true"></Selecting>
                                </ClientSettings>
                                <MasterTableView CommandItemDisplay="Top" DataKeyNames="SysXBlockId">
                                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                                    <Columns>
                                        <telerik:GridTemplateColumn>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAllUserType" runat="server" onclick="CheckAllUserType(this)" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkUserType" runat="server" onclick="UnCheckAllUserTypeHeader(this)" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="Name" HeaderText="User Type Name">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="SysXBlockId" HeaderText="Business Channel Type" Visible="false">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                                </MasterTableView>
                            </infs:WclGrid>
                        </div>
                        <div class="gclr">
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlUTDown" Height="10%">
                    <div style="text-align: center; vertical-align: central; padding-top: 20px;">
                        <infs:WclButton ID="btnAddUserTypes" ToolTip="Click to load Roles" runat="server" Icon-PrimaryIconCssClass="rbNext"
                            Text="Next" ButtonPosition="Center" OnClick="btnAddUserTypes_Click">
                        </infs:WclButton>
                    </div>
                </asp:Panel>
            </infs:WclPane>
            <infs:WclSplitBar runat="server" ID="spltRightBar"></infs:WclSplitBar>
            <infs:WclPane ID="pnRight" runat="server" MinWidth="200" Width="30%" Height="100%"
                PersistScrollPosition="true">
                <asp:Panel runat="server" ID="pnlRDUp" ScrollBars="Auto" Height="90%">
                    <div class="section">
                        <h1 class="mhdr">Select Role(s)</h1>
                        <div class="swrap">
                            <infs:WclGrid runat="server" ID="grdRoleDetail" AllowPaging="false" AutoGenerateColumns="False"
                                AllowSorting="True" GridLines="Both" OnNeedDataSource="grdRoleDetail_NeedDataSource"
                                EnableDefaultFeatures="false">
                                <ClientSettings EnableRowHoverStyle="true">
                                    <Selecting AllowRowSelect="true"></Selecting>
                                </ClientSettings>
                                <MasterTableView CommandItemDisplay="Top" DataKeyNames="RoleDetailId">
                                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                                    <Columns>
                                        <telerik:GridTemplateColumn>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAllRoles" runat="server" onclick="CheckAllRoles(this)" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRoles" runat="server" onclick="UnCheckRoleHeader(this)" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn HeaderText="Role Name" DataField="Name">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="TenantName" HeaderText="Institution Name">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                                </MasterTableView>
                            </infs:WclGrid>
                        </div>
                        <div class="gclr">
                        </div>

                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlRDDown" Height="10%">
                    <div style="text-align: center; vertical-align: central; padding-top: 20px;">
                        <infs:WclButton ID="btnAddFeature" ToolTip="Click to save bulk features" runat="server" Icon-PrimaryIconCssClass="rbSave"
                            Text="Add Features" ButtonPosition="Center" OnClick="btnSave_Click">
                        </infs:WclButton>
                    </div>
                </asp:Panel>
            </infs:WclPane>
        </infs:WclSplitter>
    </infs:WclPane>
</infs:WclSplitter>