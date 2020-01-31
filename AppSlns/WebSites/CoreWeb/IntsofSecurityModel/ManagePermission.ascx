<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.ManagePermission" Codebehind="ManagePermission.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="Commandbar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Import Namespace="INTSOF.Utils" %>
<div class="msgbox" id="divSuccessMsg">
    <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
</div>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblManagePermission" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdPermission" DataSourceID="" AllowPaging="True"
                PageSize="10" AutoGenerateColumns="False" AllowSorting="True" GridLines="Both"
                OnDeleteCommand="grdPermission_DeleteCommand" OnInsertCommand="grdPermission_InsertCommand"
                OnNeedDataSource="grdPermission_NeedDataSource" OnUpdateCommand="grdPermission_UpdateCommand"
                OnItemCreated="grdPermission_ItemCreated" OnItemCommand="grdPermission_ItemCommand"
                NonExportingColumns="EditCommandColumn, DeleteColumn">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="297mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="PermissionId">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Permission" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="Name" HeaderText="Permission Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Description" HeaderText="Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PermissionType.Name" HeaderText="Permission Type">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                            <HeaderStyle Width="30px" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Permission?"
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
                                    <asp:Label ID="lblHeading" Text='<%#(Container is GridEditFormInsertItem) ? "Add Permission" : "Edit Permission"%>'
                                        runat="server"></asp:Label></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="required"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlManagePermission">
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" Text="Permission Name" CssClass="cptn"></asp:Label><span
                                                        class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox Width="100%" Text='<%# Bind("Name")%>' MaxLength="216" TabIndex="1"
                                                        ID="txtName" runat="server" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                                                            Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_PERMISSION_REQUIRED)%>'
                                                            ValidationGroup="grpValdManagePermission" />
                                                        <asp:RegularExpressionValidator runat="server" ID="revName" ControlToValidate="txtName"
                                                            Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\,\%\@\(\)]{3,216}$"
                                                            ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>'
                                                            ValidationGroup="grpValdManagePermission" />
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
                                                    <infs:WclTextBox Width="100%" Text='<%# Bind("Description")%>' MaxLength="255" TabIndex="2"
                                                        ID="txtDescription" runat="server" />
                                                    <div class="vldx">
                                                        <asp:RegularExpressionValidator runat="server" ID="revDescription" ControlToValidate="txtDescription"
                                                            Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>'
                                                            ValidationExpression="^[\w\d\s\-\.\,\%\@\(\)]{3,255}$" ValidationGroup="grpValdManagePermission"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblPermType" runat="server" AssociatedControlID="cmbPermType" Text="Permission Type" CssClass="cptn"></asp:Label><span
                                                        class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbPermType" TabIndex="3" MarkFirstMatch="true" Width="60%"
                                                        runat="server" DataTextField="Name" DataValueField="PermissionTypeID" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvPermType" ControlToValidate="cmbPermType"
                                                            ValidationGroup="grpValdManagePermission" InitialValue="--SELECT--" Display="Dynamic"
                                                            CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_PERMISSIONTYPE_REQUIRED)%>' />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:Commandbar ID="fsucCmdBarManagePermission" ValidationGroup="grpValdManagePermission" GridInsertText="Save" GridUpdateText="Save"
                                        runat="server" GridMode="true" TabIndexAt="4" DefaultPanel="pnlManagePermission" />
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
