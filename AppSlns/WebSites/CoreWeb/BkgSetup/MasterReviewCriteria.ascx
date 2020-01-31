<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MasterReviewCriteria.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.MasterReviewCriteria" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>


<div class="section">
    <h1 class="mhdr">Master Review Criteria
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
                        <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" EmptyMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                            Filter="None" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvTenant" ControlToValidate="ddlTenant"
                                ValidationGroup="grpFormSubmit" Display="Dynamic" Enabled="true" CssClass="errmsg" Text="Institution is required." />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdMstrRevCriteria"
                AutoSkinMode="true" CellSpacing="0" GridLines="Both" AutoGenerateColumns="False" AllowFilteringByColumn="false"
                AllowSorting="false" OnNeedDataSource="grdMstrRevCriteria_NeedDataSource" ShowClearFiltersButton="false" AllowPaging="false"
                OnInsertCommand="grdMstrRevCriteria_InsertCommand" OnUpdateCommand="grdMstrRevCriteria_UpdateCommand"
                OnDeleteCommand="grdMstrRevCriteria_DeleteCommand" EnableDefaultFeatures="true">
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="BRC_ID" AllowFilteringByColumn="true">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Review Criteria" ShowRefreshButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridBoundColumn AllowFiltering="true" AllowSorting="true" DataField="BRC_Name" HeaderText="Review Name"
                            SortExpression="BRC_Name" UniqueName="BRC_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn AllowFiltering="true" AllowSorting="true" DataField="BRC_Description" HeaderText="Description"
                            SortExpression="BRC_Description" UniqueName="BRC_Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Review Criteria?"
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
                                    <asp:Label ID="lblEHCustomForm" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Review Criteria" : "Update Review Criteria" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlReviewCriteria">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Review Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtReviewName" Text='<%# Eval("BRC_Name") %>'
                                                        MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvCReviewName" ControlToValidate="txtReviewName"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Review name is required." />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtDescription" Text='<%# Eval("BRC_Description") %>'
                                                        MaxLength="1024">
                                                    </infs:WclTextBox>
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
        <div class="gclr">
        </div>
    </div>
</div>
