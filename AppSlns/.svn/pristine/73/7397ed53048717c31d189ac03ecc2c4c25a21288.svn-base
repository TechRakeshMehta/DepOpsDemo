<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.ManageDepartment" Codebehind="ManageDepartment.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Import Namespace="INTSOF.Utils" %>
<div class="msgbox" id="divSuccessMsg">
    <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
</div>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblManageDepartment" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdDepartment" DataSourceID="" AllowPaging="True"
                PageSize="10" AutoGenerateColumns="False" AllowSorting="True" GridLines="Both"
                OnDeleteCommand="grdDepartment_DeleteCommand" OnInsertCommand="grdDepartment_InsertCommand"
                OnNeedDataSource="grdDepartment_NeedDataSource" OnItemDataBound="grdDepartment_ItemDataBound"
                OnUpdateCommand="grdDepartment_UpdateCommand" OnItemCommand="grdDepartment_ItemCommand" 
                OnItemCreated="grdDepartment_ItemCreated" ShowAllExportButtons="false"
                NonExportingColumns="ManageUsersOnMgDept,ManageGrades,SetupProgramSubscription,EditCommandColumn, DeleteColumn">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrganizationID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Department" ShowExportToCsvButton="true"
                    ShowExportToExcelButton ="true" ShowExportToPdfButton="true" ShowRefreshButton= "true"/>
                    <Columns>
                        <telerik:GridBoundColumn DataField="OrganizationName" HeaderText="Department Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="OrganizationDesc" HeaderText="Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Client" UniqueName="Client" DataField="Tenant.TenantName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Created By" UniqueName="CreatedBy" DataField="CreatedByUserName">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" UniqueName="ManageUsersOnMgDept">
                            <ItemTemplate>
                                <a runat="server" id="ancManageUsers">Manage Users</a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManagePrograms">
                            <ItemTemplate>
                                <a runat="server" id="ancManagePrograms">Manage Programs</a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageGrades">
                            <ItemTemplate>
                                <a runat="server" id="ancManageGrades">Manage Grade</a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="SetupProgramSubscription">
                            <ItemTemplate>
                                <a runat="server" id="ancSetupProgramSubscription">Manage Program</a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                            <HeaderStyle Width="30px" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Department?"
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
                                    <asp:Label ID="lblHeading" Text='<%#(Container is GridEditFormInsertItem) ? "Add Department" : "Edit Department"%>'
                                        runat="server"></asp:Label></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlManageDepartment">
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" Text="Department Name" CssClass="cptn"></asp:Label><span
                                                        class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox Width="100%" Text='<%# Bind("OrganizationName")%>' MaxLength="20"
                                                        TabIndex="1" ID="txtName" runat="server" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                                                            Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_DEPARTMENT_REQUIRED)%>'
                                                            ValidationGroup="grpValdManageDepartment" />
                                                        <asp:RegularExpressionValidator runat="server" ID="revName" ControlToValidate="txtName"
                                                            Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\,\%\@\&\(\)\/\-]{3,20}$"
                                                            ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>'
                                                            ValidationGroup="grpValdManageDepartment"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div id="divClient" runat="server" class='sxlb'>
                                                     <span class="cptn">Client</span><span class="reqd">*</span>
                                                </div>
                                                 <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbTenant" runat="server" MarkFirstMatch="true"
                                                        Width="60%" DataTextField="TenantName" DataValueField="TenantID" Style="z-index: 7002;" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvTenant" ControlToValidate="cmbTenant"
                                                            Display="Dynamic" InitialValue="-- SELECT --" CssClass="errmsg" ErrorMessage="Tenant is required."
                                                            ValidationGroup="grpValdManageDepartment" />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblDescription" runat="server" AssociatedControlID="txtDescription" CssClass="cptn"
                                                        Text="Description"></asp:Label>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox Width="100%" Text='<%# Bind("OrganizationDesc")%>' MaxLength="255"
                                                        TabIndex="2" ID="txtDescription" runat="server" />
                                                    <div class="vldx">
                                                        <asp:RegularExpressionValidator runat="server" ID="revDescription" ControlToValidate="txtDescription"
                                                            Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(
    ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>' ValidationExpression="^[\w\d\s\-\.\,\%\@\&\(\)\/\-]{3,255}$"
                                                            ValidationGroup="grpValdManageDepartment"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarManageDepartment" ValidationGroup="grpValdManageDepartment" GridInsertText="Save" GridUpdateText="Save"
                                        runat="server" GridMode="true" TabIndexAt="3" DefaultPanel="pnlManageDepartment" />
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
