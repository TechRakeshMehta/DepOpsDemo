<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.ManageLineOfBusiness" CodeBehind="ManageLineOfBusiness.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Import Namespace="INTSOF.Utils" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxName1">
    <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Styles/mod.css" ResourceType="StyleSheet" />
</infs:WclResourceManagerProxy>
<div class="msgbox" id="divSuccessMsg">
    <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
</div>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblManageLineOfBusiness" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdBlock" AllowPaging="True" PageSize="10" AutoGenerateColumns="False"
                AllowSorting="True" GridLines="Both" OnDeleteCommand="grdLineOfBusinesses_DeleteCommand"
                OnInsertCommand="grdLineOfBusinesses_InsertCommand" OnNeedDataSource="grdLineOfBusinesses_NeedDataSource"
                OnUpdateCommand="grdgrdLineOfBusinesses_UpdateCommand" OnItemDataBound="grdLineOfBusinesses_ItemDataBound"
                OnItemCommand="grdgrdLineOfBusinesses_ItemCommand" AllowMultiRowEdit="false" ShowAllExportButtons="false" OnItemCreated="grdLineOfBusinesses_ItemCreated"
                NonExportingColumns="ManageFeatures, EditCommandColumn, DeleteColumn">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="SysXBlockId,Code">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New User Type" ShowExportToCsvButton="true"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowRefreshButton="true" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="Name" HeaderText="User Type Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Code" HeaderText="Code">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Description" HeaderText="Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="lkpBusinessChannelType.Name" HeaderText="Business Channel Type">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageFeatures">
                            <ItemTemplate>
                                <a runat="server" id="ancFeature">Manage Features</a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                            <HeaderStyle Width="30px" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this User Type?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblHeading" Text='<%#(Container is GridEditFormInsertItem) ? "Add User Type" : "Edit User Type"%>'
                                        runat="server"></asp:Label></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlManageBlock">
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" Text="User Type Name" CssClass="cptn"></asp:Label><span
                                                        class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox Width="100%" MaxLength="60" TabIndex="1" Text='<%# Bind("Name")%>'
                                                        ID="txtName" runat="server" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                                                            Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.USER_TYPE_NAME_REQUIRED)%>'
                                                            ValidationGroup="grpValdManageBlock" />
                                                        <asp:RegularExpressionValidator runat="server" ID="revNameInvalidCharacter" ControlToValidate="txtName"
                                                            Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\,\%\@\(\)\/]{3,60}$"
                                                            ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>'
                                                            ValidationGroup="grpValdManageBlock"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblCode" runat="server" AssociatedControlID="txtCode" Text="User Type Code" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox Width="100%" MaxLength="6" ReadOnly='<%#!(Container is GridEditFormInsertItem) %>'
                                                        TabIndex="2" Text='<%# Bind("Code")%>' ID="txtCode" runat="server" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvCode" ControlToValidate="txtCode"
                                                            Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.USER_TYPE_CODE_REQUIRED)%>'
                                                            ValidationGroup="grpValdManageBlock" />
                                                        <asp:RegularExpressionValidator runat="server" ID="revCode" ControlToValidate="txtCode"
                                                            Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\,\%\@\(\)\/]{6,10}$"
                                                            ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_OF_SIX_CHARACTERS)%>'
                                                            ValidationGroup="grpValdManageBlock"></asp:RegularExpressionValidator>
                                                        <asp:CustomValidator ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.USER_TYPE_CODE_IS_ALREADY_IN_USE)%>'
                                                            ControlToValidate="txtCode" ID="cvDuplicateCode" OnServerValidate="cvDuplicateCode_ServerValidate"
                                                            runat="server" ValidationGroup="grpValdManageBlock" Display="Dynamic" CssClass="errmsg" />
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
                                                    <infs:WclTextBox Width="100%" Text='<%# Bind("Description")%>' TabIndex="3" MaxLength="255"
                                                        ID="txtDescription" runat="server" />
                                                    <div class="vldx">
                                                        <asp:RegularExpressionValidator runat="server" ID="revDescription" ControlToValidate="txtDescription"
                                                            Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(
    ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>'
                                                            ValidationExpression="^[\w\d\s\-\.\,\%\@\(\)\/]{3,255}$"
                                                            ValidationGroup="grpValdManageBlock"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblBusinessChannelType" runat="server" AssociatedControlID="cmbBusinessChannelType" CssClass="cptn"
                                                        Text="Business Channel Type"></asp:Label><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbBusinessChannelType" runat="server" DataTextField="Name" DataValueField="BusinessChannelTypeID">
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
                                    <infsu:CommandBar ID="fsucCmdBarManageBlock" ValidationGroup="grpValdManageBlock" GridInsertText="Save" GridUpdateText="Save"
                                        runat="server" GridMode="true" TabIndexAt="4" DefaultPanel="pnlManageBlock" />
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
