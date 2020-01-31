<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.ManageAttributeGroup" Codebehind="ManageAttributeGroup.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>
<div class="section">
    <h1 class="mhdr">
        Manage Attribute Group</h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="paneTenant">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <asp:Label ID="lblSelectClient" runat="server" Text="Institution" CssClass="cptn"></asp:Label>
                    </div>
                    <div class='sxlm'>
                        <%--<infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" DefaultMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChange">
                        </infs:WclDropDownList>--%>
                        <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" DefaultMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChange"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">  
                        </infs:WclComboBox>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div id="dvComplianceAttributeGroup" runat="server" class="swrap">
            <infs:WclGrid runat="server" ID="grdComplianceAttributeGroup" AllowPaging="True"
                PageSize="10" AutoGenerateColumns="False" AllowSorting="True" GridLines="Both"
                ShowAllExportButtons="False" NonExportingColumns="EditCommandColumn, DeleteColumn"
                ValidationGroup="grpValdComplianceAttributeGroup" OnNeedDataSource="grdComplianceAttributeGroup_NeedDataSource"
                 OnItemCommand="grdComplianceAttributeGroup_ItemCommand" OnInsertCommand="grdComplianceAttributeGroup_InsertCommand"
                OnUpdateCommand="grdComplianceAttributeGroup_UpdateCommand" OnDeleteCommand="grdComplianceAttributeGroup_DeleteCommand"
                >
                <ExportSettings Pdf-PageWidth="300mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="CAG_ID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Attribute Group"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true">
                    </CommandItemSettings>
                    <Columns>
                        <telerik:GridBoundColumn DataField="CAG_Name" FilterControlAltText="Filter CAG_Name column"
                            HeaderText="Name" SortExpression="CAG_Name" UniqueName="CAG_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAG_Label" FilterControlAltText="Filter CAG_Label column"
                            HeaderText="Label" SortExpression="CAG_Label" UniqueName="CAG_Label">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                            <HeaderStyle Width="30px" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle Width="30px" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section" runat="server" id="divEditBlock" visible="true">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblTitlePriceAdjustment" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Attribute Group" : "Update Attribute Group" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPriceAdjustment">
                                            <infs:WclTextBox runat="server" Text='<%# Eval("CAG_ID") %>' ID="txtComplianceAttributeGroupId"
                                                Visible="false">
                                            </infs:WclTextBox>
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox ID="txtComplianceAttributeGroupName" Width="100%" runat="server"
                                                        Text='<%# Eval("CAG_Name") %>' MaxLength="50">
                                                    </infs:WclTextBox>
                                                    <div id="dvLabel" class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvLabel" ControlToValidate="txtComplianceAttributeGroupName"
                                                            class="errmsg" ErrorMessage="Name is required." ValidationGroup='grpValdComplianceAttributeGroup'
                                                            Enabled="true" />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Label</span>
                                                </div>
                                                <div class='sxlm '>
                                                    <infs:WclTextBox Width="100%" ID="txtComplianceAttributeGroupLabel" runat="server"
                                                        Text='<%# Eval("CAG_Label") %>' MaxLength="100">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarPriceAdjustment" runat="server" GridMode="true" DefaultPanel="pnlPriceAdjustment"
                                        ValidationGroup="grpValdComplianceAttributeGroup" GridInsertText="Save" GridUpdateText="Save" ExtraButtonIconClass="icnreset" />
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
