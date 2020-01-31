<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceVendors.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ServiceVendors" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <h1 class="mhdr">Service Vendors
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server"> </asp:Label>
            </div>

            <infs:WclGrid runat="server" ID="grdServiceVendors"
                AutoSkinMode="true" CellSpacing="0" GridLines="Both" AutoGenerateColumns="False"
                AllowFilteringByColumn="false" AllowSorting="false" ShowClearFiltersButton="false" AllowPaging="false"
                OnItemDataBound="grdServiceVendors_ItemDataBound" EnableDefaultFeatures="false" OnDeleteCommand="grdServiceVendors_DeleteCommand" OnInsertCommand="grdServiceVendors_InsertCommand" OnNeedDataSource="grdServiceVendors_NeedDataSource" OnUpdateCommand="grdServiceVendors_UpdateCommand" OnItemCommand="grdServiceVendors_ItemCommand">
                <ClientSettings EnableRowHoverStyle="true" AllowRowsDragDrop="false">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="EVE_ID" AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Vendor" ShowRefreshButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridBoundColumn DataField="EVE_Name" HeaderText="Name"
                            SortExpression="EVE_Name" UniqueName="EVE_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EVE_Description" HeaderText="Description"
                            SortExpression="EVE_Description" UniqueName="EVE_Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EVE_ContactName" HeaderText="Contact Name"
                            SortExpression="EVE_ContactName" UniqueName="EVE_ContactName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EVE_ContactPhone" HeaderText="Contact Phone"
                            SortExpression="EVE_ContactPhone" UniqueName="EVE_ContactPhone">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EVE_ContactEmail" HeaderText="Contact Email"
                            SortExpression="EVE_ContactEmail" UniqueName="EVE_ContactEmail">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="VendorServices">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnVendorServices" ButtonType="LinkButton" CommandName="VendorServices"
                                    runat="server" Text="Vendor Services" BackColor="Transparent" Font-Underline="true" BorderStyle="None" ForeColor="Black">
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageAccount">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnMngAccount" ButtonType="LinkButton" CommandName="ManageAccount"
                                    runat="server" Text="Manage Account" BackColor="Transparent" Font-Underline="true" BorderStyle="None" ForeColor="Black">
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Vendor?"
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
                                    <asp:Label ID="lblEHCustomForm" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Vendor" : "Update Vendor" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlServiceVendors">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtName" Text='<%# Eval("EVE_Name") %>'
                                                        MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Name is required." />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtDescription" Text='<%# Eval("EVE_Description") %>'
                                                        MaxLength="256">
                                                    </infs:WclTextBox>

                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Contact Name</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtContactName" Text='<%# Eval("EVE_ContactName") %>'
                                                        MaxLength="256">
                                                    </infs:WclTextBox>

                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Contact Phone</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclMaskedTextBox ID="txtContactPhone" runat="server" Mask="(###)-###-####" Text='<%# Eval("EVE_ContactPhone") %>'>
                                                    </infs:WclMaskedTextBox>
                                                    <div class="vldx">
                                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revContactPhone" runat="server"
                                                            CssClass="errmsg" ErrorMessage="Format is (###)-###-####" ControlToValidate="txtContactPhone"
                                                            ValidationExpression="\(\d{3}\)-\d{3}-\d{4}" />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Contact Email</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtContactEmail" Text='<%# Eval("EVE_ContactEmail") %>'
                                                        MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RegularExpressionValidator ID="revContactEmail" runat="server" Display="Dynamic"
                                                            ErrorMessage="Email Address is not valid." ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                                                            ControlToValidate="txtContactEmail" CssClass="errmsg">
                                                        </asp:RegularExpressionValidator>
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
