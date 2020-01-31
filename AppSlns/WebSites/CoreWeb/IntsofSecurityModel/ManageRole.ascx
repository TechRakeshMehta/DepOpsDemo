<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.IntsofSecurityModel.Views.ManageRole" CodeBehind="ManageRole.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Import Namespace="INTSOF.Utils" %>
<div class="msgbox" id="divSuccessMsg">
    <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
</div>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblManageRole" runat="server" Text=""></asp:Label><asp:Label ID="lblSuffix"
            runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdRoleDetail" AllowPaging="True" PageSize="10"
                AutoGenerateColumns="False" AllowSorting="True" GridLines="Both" OnDeleteCommand="grdRoleDetail_DeleteCommand"
                OnInsertCommand="grdRoleDetail_InsertCommand" OnNeedDataSource="grdRoleDetail_NeedDataSource"
                OnUpdateCommand="grdRoleDetail_UpdateCommand" OnItemDataBound="grdRoleDetail_ItemDataBound"
                OnItemCreated="grdRoleDetail_ItemCreated" OnItemCommand="grdRoleDetail_ItemCommand" ShowAllExportButtons="false"
                NonExportingColumns="ManageFeature, ManageUsers, EditCommandColumn, DeleteColumn"
                ValidationGroup="grpValdManageRole">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="RoleDetailId">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Role" ShowExportToCsvButton="true"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowRefreshButton="true" />
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Role Name" DataField="RoleName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Description" HeaderText="Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TenantProduct.Name" HeaderText="Product">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TenantProduct.Tenant.TenantName" HeaderText="Institution Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CreatedByUserName" HeaderText="Created By" UniqueName="CreatedByUserName"
                            SortExpression="CreatedByUserName">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageFeature">
                            <ItemTemplate>
                                <a runat="server" id="ancFeture">Manage Features</a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="SetPolicy">
                            <ItemTemplate>
                                <a runat="server" id="ancPolicy">Manage Policy</a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageUsers">
                            <ItemTemplate>
                                <a runat="server" id="ancUsers">Manage Users</a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageFeaturesWiz"
                            Visible="false">
                            <ItemTemplate>
                                <infs:WclButton ID="SysXButton1" ButtonType="LinkButton" CssClass="lnkbtn" BorderStyle="None"
                                    Text="Manage Features" Font-Underline="true" CommandName="ManageFeatures" runat="server"
                                    CommandArgument='<%# Eval("RoleDetailId")  %>'>
                                </infs:WclButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                            <HeaderStyle Width="30px" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Role?"
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
                                    <asp:Label ID="lblHeading" Text='<%#(Container is GridEditFormInsertItem) ? "Add Role" : "Edit Role"%>'
                                        runat="server"></asp:Label></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlManageRole">
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" Text="Role Name" CssClass="cptn"></asp:Label><span
                                                        class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox Width="100%" Text='<%# Eval("Name") %>' TabIndex="1" ID="txtName"
                                                        MaxLength="200" runat="server">
                                                    </infs:WclTextBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                                                            Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_ROLE_NAME_REQUIRED)%>'
                                                            ValidationGroup="grpValdManageRole" />
                                                        <asp:RegularExpressionValidator runat="server" ID="revName" ControlToValidate="txtName"
                                                            Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\%\@\(\)]{3,200}$"
                                                            ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>'
                                                            ValidationGroup="grpValdManageRole" />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblDescription" runat="server" AssociatedControlID="txtDescription"
                                                        Text="Description" CssClass="cptn"></asp:Label>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox Width="100%" Text='<%# Bind("Description")%>' TabIndex="2" ID="txtDescription"
                                                        MaxLength="50" runat="server">
                                                    </infs:WclTextBox>
                                                    <div class="vldx">
                                                        <asp:RegularExpressionValidator runat="server" ID="revDescription" ControlToValidate="txtDescription"
                                                            Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>'
                                                            ValidationExpression="^[\w\d\s\-\.\%\@\(\)]{3,50}$" ValidationGroup="grpValdManageRole"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblProdcut" runat="server" AssociatedControlID="cmbProduct" Text="Product" CssClass="cptn"></asp:Label><span
                                                        class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbProduct" runat="server" MarkFirstMatch="true" TabIndex="3"
                                                        Width="60%" DataTextField="Name" DataValueField="TenantProductId" Style="z-index: 7002;" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvProdcut" ControlToValidate="cmbProduct"
                                                            Display="Dynamic" InitialValue="--SELECT--" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_PRODUCT_REQUIRED)%>'
                                                            ValidationGroup="grpValdManageRole" />
                                                        <asp:Label ID="lblProductName" Visible="false" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblIsUserGroupLevel" runat="server" AssociatedControlID="chkIsUserGroupLevel"
                                                        Text="Is User Group Level" CssClass="cptn">
                                                    </asp:Label>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:CheckBox ID="chkIsUserGroupLevel" runat="server" Checked='<%# (Container is GridEditFormInsertItem) ? false : Eval("IsUserGroupLevel")%>' />
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblShowAdminEntryPortal" runat="server" AssociatedControlID="chkShowAdminEntryPortal"
                                                        Text="Show Admin Entry Dashboard" CssClass="cptn">
                                                    </asp:Label>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:CheckBox ID="chkShowAdminEntryPortal" runat="server" Checked='<%# (Container is GridEditFormInsertItem) ? false : Eval("ShowAdminEntryDashboard")%>' />
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdbarManageRole" runat="server" TabIndexAt="4" GridMode="true" GridInsertText="Save" GridUpdateText="Save"
                                        DefaultPanel="pnlManageRole" ValidationGroup="grpValdManageRole" />
                                </div>
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
    </div>
</div>
