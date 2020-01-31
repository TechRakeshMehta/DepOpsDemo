<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendorAccountSettings.ascx.cs" Inherits="CoreWeb.BkgSetup.UserControl.Views.VendorAccountSettings" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <h1 class="mhdr">Service Vendors Account Setting
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server"> </asp:Label>
            </div>

            <infs:WclGrid runat="server" ID="grdVendorAccountSettings"
                AutoSkinMode="true" CellSpacing="0" GridLines="Both" AutoGenerateColumns="False" AllowPaging="false" PagerStyle-Visible="false"
                AllowFilteringByColumn="false" AllowSorting="false" ShowClearFiltersButton="false"
                EnableDefaultFeatures="false" OnDeleteCommand="grdVendorAccountSettings_DeleteCommand" OnInsertCommand="grdVendorAccountSettings_InsertCommand" OnNeedDataSource="grdVendorAccountSettings_NeedDataSource" OnUpdateCommand="grdVendorAccountSettings_UpdateCommand">
                <ClientSettings EnableRowHoverStyle="true" AllowRowsDragDrop="false">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="EVA_ID" AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Account Setting" ShowRefreshButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridBoundColumn DataField="EVA_AccountNumber" HeaderText="Account Number"
                            SortExpression="EVA_AccountNumber" UniqueName="EVA_AccountNumber">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EVA_AccountName" HeaderText="Account Name"
                            SortExpression="EVA_AccountName" UniqueName="EVA_AccountName">
                        </telerik:GridBoundColumn>

                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Vendor Account Setting?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section" visible="true" id="divEditFormBlock" runat="server">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblEHCustomForm" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Account Setting" : "Update Account Setting" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlServiceVendors">
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Account Number</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtAccNumber" Text='<%# Eval("EVA_AccountNumber") %>'
                                                        MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvAccNumber" ControlToValidate="txtAccNumber"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Account number is required." />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Account Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtAccName" Text='<%# Eval("EVA_AccountName") %>'
                                                        MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtAccName"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Account name is required." />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlCategory"
                                        ValidationGroup="grpFormSubmit" GridInsertText="Save" GridUpdateText="Save" ExtraButtonIconClass="icnreset" />
                                </div>
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                </MasterTableView>
            </infs:WclGrid>
        </div>
    </div>
</div>
<div style="width: 100%; text-align: center" id="dvShowBackLink" runat="server">
    <infs:WclButton runat="server" ID="btnGoBack" Text="Go Back To Service Vendors" OnClick="CmdBarCancel_Click">
    </infs:WclButton>
</div>
