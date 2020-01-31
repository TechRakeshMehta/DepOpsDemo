<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.ManageUserGroup" CodeBehind="ManageUserGroup.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Import Namespace="INTSOF.Utils" %>


<div class="section">
   
    <h1 class="mhdr">
        <asp:Label Text="Manage User Groups" runat="server" ID="lblUManHdr" /></h1>
    <div class="content">
        <div class="msgbox" id="divSuccessMsg">
            <asp:Label Text="" ID="lblSuccess" runat="server" />
        </div>
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdUsrGrps" AllowPaging="True" AutoGenerateColumns="False"  EnableAriaSupport="true"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="True" ShowAllExportButtons="false" ShowExtraButtons="False" GridLines="Both"
                OnInsertCommand="grdUsrGrps_InsertCommand" OnDeleteCommand="grdUsrGrps_ItemDeleted"
                OnUpdateCommand="grdUsrGrps_ItemUpdated" OnNeedDataSource="grdUsrGrps_NeedDataSource"
                OnItemCommand="grdUsrGrps_ItemCommand" OnItemDataBound="grdUsrGrps_ItemDataBound" OnItemCreated="grdUsrGrps_ItemCreated">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True" 
                    Pdf-PageWidth="297mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                    <ClientEvents OnKeyPress="keyPress" />
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="UserGroupId,TenantID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add new User Group" ShowExportToCsvButton="true"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="UserGroupID" FilterControlAltText="Filter ID column"
                            HeaderText="ID" SortExpression="UserGroupID" UniqueName="UserGroupID" Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserGroupName" FilterControlAltText="Filter User_Group column"
                            HeaderText="User Group" SortExpression="UserGroupName" UniqueName="UserGroupName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserGroupDesc" FilterControlAltText="Filter Description column"
                            HeaderText="Description" SortExpression="UserGroupDesc" UniqueName="UserGroupDesc"
                            AllowFiltering="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageRoles">
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <a runat="server" id="ancRoles" href="#">Manage Roles</a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageQueueOnGrpLvl">
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <asp:HyperLink ID="hypMngQueGrp" runat="server" NavigateUrl="#" Text="Manage Queue"></asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                            <HeaderStyle Width="30px" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Group?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle Width="30px" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblGrpFrmHdr" Text='<%# (Container is GridEditFormInsertItem) ? "Add New User Group" : "Update User Group" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblGrpMsg" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlGroup">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">User Group Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtName" Text='<%# Bind("UserGroupName") %>'>
                                                    </infs:WclTextBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvName" CssClass="errmsg" ControlToValidate="txtName"
                                                            Display="Dynamic" ErrorMessage="User Group Name is required." ValidationGroup="grpManageUserGroup" />
                                                        <asp:RegularExpressionValidator runat="server" CssClass="errmsg" ID="revName" ControlToValidate="txtName"
                                                            Display="Dynamic" ValidationExpression="^[\w\d\s\-\.\,\%\(\)\/]{3,50}$" ErrorMessage="Invalid character(s)."
                                                            ValidationGroup="grpManageUserGroup"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <!-- added tenent-->
                                            <div id="divTenant" runat="server" class='sxro sx3co' clientidmode="Static">
                                                <div class='sxlb'>
                                                    <span class="cptn">Select Client</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbTenant" runat="server" DataValueField="TenantID" DataTextField="TenantName"
                                                        MarkFirstMatch="true">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvTenant" ControlToValidate="cmbTenant"
                                                            InitialValue="--SELECT--" class="errmsg" ValidationGroup="grpManageUsers" Display="Dynamic"
                                                            ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.USERGROUP_TENANT_REQUIRED)%>' />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <!--end -->
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Description</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <infs:WclTextBox runat="server" ID="txtDescription" TextMode="MultiLine" Text='<%# Bind("UserGroupDesc") %>'
                                                        CssClass="txt2ro">
                                                    </infs:WclTextBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvDescription" CssClass="errmsg"
                                                            ControlToValidate="txtDescription" Display="Dynamic" ErrorMessage="Description is required."
                                                            ValidationGroup="grpManageUserGroup" />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucGroup" runat="server" GridMode="true" DefaultPanel="pnlGroup" GridInsertText="Save" GridUpdateText="Save"
                                        ValidationGroup="grpManageUserGroup" />
                                </div>
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                    <NestedViewTemplate>
                        <div class="swrap">
                            <infs:WclGrid runat="server" ID="grdName1" AllowPaging="True" AutoGenerateColumns="False"
                                AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
                                EnableDefaultFeatures="True" ShowAllExportButtons="false" ShowExtraButtons="False"
                                OnNeedDataSource="grdName1_NeedDataSource" OnDeleteCommand="grdName1_DeleteCommand"
                                OnInsertCommand="grdName1_InsertCommand"
                                OnItemDataBound="grdName1_ItemDataBound" OnItemCreated="grdName1_ItemCreated">
                                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                                </ExportSettings>
                                <ClientSettings EnableRowHoverStyle="true">
                                    <Selecting AllowRowSelect="true"></Selecting>
                                </ClientSettings>
                                <MasterTableView CommandItemDisplay="Top" DataKeyNames="UsersInUserGroupID">
                                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add User" />
                                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="aspnet_Users.UserName" FilterControlAltText="Filter User column"
                                            HeaderText="User" SortExpression="User" UniqueName="User" AllowFiltering="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageRoles">
                                            <HeaderStyle Width="100px" />
                                            <ItemTemplate>
                                                <a runat="server" id="ancRoles" href="#">Manage Roles</a>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageQueueOnUsrLvl">
                                            <HeaderStyle Width="100px" />
                                            <ItemTemplate>
                                                <a runat="server" id="ancManageUsr" href="#">Manage Queue</a>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to remove this User?"
                                            Text="Delete" UniqueName="DeleteColumn">
                                            <HeaderStyle Width="30px" />
                                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                                        </telerik:GridButtonColumn>
                                    </Columns>
                                    <EditFormSettings EditFormType="Template">
                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                        </EditColumn>
                                        <FormTemplate>
                                            <div class="section">
                                                <h1 class="mhdr">
                                                    <asp:Label ID="lblUsrHdr" Text='<%# (Container is GridEditFormInsertItem) ? "Add User" : "Update User" %>'
                                                        runat="server" /></h1>
                                                <div class="content">
                                                    <div class="sxform auto">
                                                        <div class="msgbox">
                                                            <asp:Label ID="lblUsrMsg" runat="server" CssClass="info"></asp:Label>
                                                        </div>
                                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlUsers">

                                                            <div class='sxro sx3co'>
                                                                <div class='sxlb'>
                                                                    <span class="cptn">Select User</span>
                                                                </div>
                                                                <div class='sxlm'>
                                                                    <infs:WclComboBox ID="cmbUsrList" runat="server" DataValueField="UserId" DataTextField="UserName"
                                                                        MarkFirstMatch="true">
                                                                    </infs:WclComboBox>
                                                                    <div class="vldx">
                                                                        <asp:RequiredFieldValidator runat="server" ID="rfvUsrList" ControlToValidate="cmbUsrList"
                                                                            InitialValue="--SELECT--" class="errmsg" ValidationGroup="grpManageUsers" Display="Dynamic"
                                                                            ErrorMessage='<%#SysXUtils.GetMessage(
    ResourceConst.USERGROUP_USER_REQUIRED)%>' />
                                                                    </div>
                                                                </div>
                                                                <div class='sxroend'>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                    <infsu:CommandBar ID="fsucUsers" runat="server" GridMode="true" DefaultPanel="pnlUsers" GridInsertText="Save" GridUpdateText="Save"
                                                        ValidationGroup="grpManageUsers" />
                                                </div>
                                            </div>
                                        </FormTemplate>
                                    </EditFormSettings>
                                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                                </MasterTableView>
                                <FilterMenu EnableImageSprites="False">
                                </FilterMenu>
                            </infs:WclGrid>
                        </div>
                        <div class="gclr">
                        </div>
                    </NestedViewTemplate>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
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
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
    </div>
</div>
