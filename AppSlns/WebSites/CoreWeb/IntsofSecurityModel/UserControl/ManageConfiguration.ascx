<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.ManageConfiguration" Codebehind="ManageConfiguration.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="Commandbar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Import Namespace="INTSOF.Utils" %>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblManageWebConfiguration" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdAppConfigDetail" DataSourceID="" AllowPaging="True"
                PageSize="10" AutoGenerateColumns="False" AllowSorting="True" GridLines="Both"
                OnUpdateCommand="grdAppConfigDetail_UpdateCommand" OnNeedDataSource="grdAppConfigDetail_NeedDataSource"
                NonExportingColumns="EditCommandColumn" OnItemCommand="grdAppConfigDetail_ItemCommand">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="297mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="Key">
                    <CommandItemSettings ShowAddNewRecordButton="false" AddNewRecordText="Add New Web Configuration"
                        ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="Key" HeaderText="App Config Key Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Value" HeaderText="Value">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                            EditText="Edit">
                            <HeaderStyle Width="30px" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="EditAppConfig">
                        </EditColumn>
                        <FormTemplate>
                            <div class="sxform auto">
                                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlManageAppConfig">
                                    <div class='sxro sx1co'>
                                        <div class='sxlb'>
                                            <asp:Label ID="lblAppName" runat="server" AssociatedControlID="txtAppKeyName" Text="App Config Key Name" CssClass="cptn"></asp:Label>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox Width="100%" Text='<%#Bind("Key")%>' ID="txtAppKeyName" MaxLength="213"
                                                runat="server" Enabled="False" />
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx1co'>
                                        <div class='sxlb'>
                                            <asp:Label ID="lblAppValue" runat="server" AssociatedControlID="txtAppValue" Text="Value" CssClass="cptn"></asp:Label><span
                                                class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox Width="100%" Text='<%# Bind("Value")%>' ID="txtAppValue" MaxLength="50"
                                                runat="server" />
                                            <div class="valdx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvAppValue" ControlToValidate="txtAPPValue"
                                                    ValidationGroup="manageUserGroup" Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_VALUE_REQUIRED)%>' />
                                            </div>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <infsu:CommandBar ID="fsucCmdBarAppConfigDetail" ValidationGroup="manageUserGroup" GridInsertText="Save" GridUpdateText="Save"
                                runat="server" GridMode="true" DefaultPanel="pnlManageAppConfig" />
                        </FormTemplate>
                    </EditFormSettings>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat="{4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
           </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
    </div>
</div>
<%--Below section handles the operations for Manage database configurations--%>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblManageDBConfiguration" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdDBConfigDetail" DataSourceID="" AllowPaging="True"
                PageSize="10" AutoGenerateColumns="False" AllowSorting="True" GridLines="Both"
                OnUpdateCommand="grdDBConfigDetail_UpdateCommand" OnNeedDataSource="grdDBConfigDetail_NeedDataSource"
                OnInsertCommand="grdDBConfigDetail_InsertCommand" NonExportingColumns="EditCommandColumn" OnItemCommand="grdDBConfigDetail_ItemCommand">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="297mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="SysXKey">
                    <CommandItemSettings ShowAddNewRecordButton="false" AddNewRecordText="Add New Database Configuration"
                        ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="SysXKey" HeaderText="Database Config Key Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Value" HeaderText="Value">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                            EditText="Edit">
                            <HeaderStyle Width="30px" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="EditDBConfig">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblDBHeading" Text='Database Config Key Name' runat="server"></asp:Label></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlManageDBConfig">
                                            <div class='sxro sx1co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblDBName" runat="server" AssociatedControlID="txtDBKeyName" Text="Database Config Key Name" CssClass="cptn"></asp:Label>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox Width="100%" Enabled="false" Text='<%# Bind("SysXKey")%>' ID="txtDBKeyName" MaxLength="213"
                                                        runat="server" />
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx1co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblDBValue" runat="server" AssociatedControlID="txtDBValue" Text="Value" CssClass="cptn"></asp:Label><span
                                                        class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox Width="100%" Text='<%# Bind("Value")%>' ID="txtDBValue" MaxLength="50"
                                                        runat="server" />
                                                    <div class="valdx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvDBValue" ControlToValidate="txtDBValue"
                                                            ValidationGroup="manageUserGroup" Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_VALUE_REQUIRED)%>' />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarManageDBConfig" ValidationGroup="manageUserGroup" GridInsertText="Save" GridUpdateText="Save"
                                        runat="server" GridMode="true" DefaultPanel="pnlManageDBConfig" />
                                </div>
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                </MasterTableView>
           </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
    </div>
</div>
