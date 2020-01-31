<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderClientStatus.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.OrderClientStatus" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <h1 class="mhdr">Order Client Status
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server"> </asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenant">
                <div class='sxro sx2co'>
                    <div class='sxlb'>
                        <asp:Label ID="lblTenant" runat="server" Text="Institution" CssClass="cptn"></asp:Label>
                    </div>
                    <div class='sxlm'>
                        <%--<infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" DefaultMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged">
                        </infs:WclDropDownList>--%>
                        <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" EmptyMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">   
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvTenant" ControlToValidate="ddlTenant"
                                Display="Dynamic" Enabled="false" CssClass="errmsg" Text="Institution is required." />
                        </div>
                    </div>
                    <%--  <div class='sxlb'>
                        <asp:Label ID="lblName" runat="server" Text="Name" CssClass="cptn"></asp:Label>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtName" runat="server"></infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                                Display="Dynamic" Enabled="false" CssClass="errmsg" Text="Name is required." />
                        </div>
                    </div>--%>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>

<div class="section">
    <div class="swrap" runat="server" id="dvClientStatus">
        <div class="content">
            <infs:WclGrid runat="server" ID="grdOrderClientStatus"
                AutoSkinMode="true" CellSpacing="0" GridLines="Both" AutoGenerateColumns="False"
                AllowFilteringByColumn="false" AllowSorting="false" OnNeedDataSource="grdOrderClientStatus_NeedDataSource" ShowClearFiltersButton="false" AllowPaging="false" 
                OnRowDrop="grdOrderClientStatus_RowDrop" EnableDefaultFeatures="false" OnInsertCommand="grdOrderClientStatus_InsertCommand" OnUpdateCommand="grdOrderClientStatus_UpdateCommand" OnDeleteCommand="grdOrderClientStatus_DeleteCommand">
                <ClientSettings EnableRowHoverStyle="true" AllowRowsDragDrop="true" AllowAutoScrollOnDragDrop="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="BOCS_ID" AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Record" ShowRefreshButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridBoundColumn DataField="Tenant.TenantName" HeaderText="Institution Name"
                            SortExpression="Tenant.TenantName" UniqueName="BOCS_InstitutionID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BOCS_OrderClientStatusTypeName" HeaderText="Name"
                            SortExpression="BOCS_OrderClientStatusTypeName" UniqueName="BOCS_OrderClientStatusTypeName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BOCS_DisplayOrder" HeaderText="Display Sequence"
                            SortExpression="BOCS_DisplayOrder" UniqueName="BOCS_DisplayOrder">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Status?"
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
                                    <asp:Label ID="lblEHCustomForm" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Record" : "Update Record" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlCustomForm">
                                            <div class='sxro sx3co'>
                                                <%-- <div class='sxlb'>
                                                    <asp:Label ID="lblTenant" runat="server" Text="Institution" CssClass="cptn"></asp:Label>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclDropDownList ID="ddlTenants" runat="server" AutoPostBack="true" DataTextField="TenantName"
                                                        DataValueField="TenantID" DefaultMessage="--Select--">
                                                    </infs:WclDropDownList>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvTenant" ControlToValidate="ddlTenant"
                                                            Display="Dynamic" Enabled="false" CssClass="errmsg" Text="Institution is required." />
                                                    </div>
                                                </div>--%>
                                                <div class='sxlb'>
                                                    <span class="cptn">Display Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtClientStatusName" Text='<%# Eval("BOCS_OrderClientStatusTypeName") %>'
                                                        MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvClientStatusName" ControlToValidate="txtClientStatusName"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Display Name is required." />
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
            <div class="gclr">
            </div>
        </div>
    </div>
</div>
