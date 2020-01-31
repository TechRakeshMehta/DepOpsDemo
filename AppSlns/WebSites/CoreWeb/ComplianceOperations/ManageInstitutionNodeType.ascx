<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ManageInstitutionNodeType" Codebehind="ManageInstitutionNodeType.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>
<div class="section">
    <h1 class="mhdr">
        Manage Institution Node Type</h1>
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
                       <%-- <infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" DefaultMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChange" >
                        </infs:WclDropDownList>--%>
                        <infs:WclComboBox  ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID"  EmptyMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChange"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"> 
                        </infs:WclComboBox>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div id="dvInstitutionNodeType" runat="server" class="swrap">
            <infs:WclGrid runat="server" ID="grdInstitutionNodeType" AllowPaging="True" PageSize="10"
                AutoGenerateColumns="False" AllowSorting="True" GridLines="Both" ShowAllExportButtons="False"
                NonExportingColumns="EditCommandColumn, DeleteColumn" ValidationGroup="grpValdInstitutionNodeType"
                OnNeedDataSource="grdInstitutionNodeType_NeedDataSource" OnItemCommand="grdInstitutionNodeType_ItemCommand"
                OnInsertCommand="grdInstitutionNodeType_InsertCommand" OnUpdateCommand="grdInstitutionNodeType_UpdateCommand"
                OnDeleteCommand="grdInstitutionNodeType_DeleteCommand" OnItemDataBound="grdInstitutionNodeType_ItemDataBound">
                <ExportSettings Pdf-PageWidth="300mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="INT_ID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Node Type"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true">
                    </CommandItemSettings>
                    <Columns>
                        <telerik:GridBoundColumn DataField="INT_Name" FilterControlAltText="Filter INT_Name column"
                            HeaderText="Name" SortExpression="INT_Name" UniqueName="INT_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="INT_Description" FilterControlAltText="Filter INT_Description column"
                            HeaderText="Description" SortExpression="INT_Description" UniqueName="INT_Description">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Wrap="false" UniqueName="ManageQueue">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlManageQueue" Text="Manage Queue" NavigateUrl="#" Visible="false" runat="server"></asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                            <HeaderStyle Width="30px" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Node Type?"
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
                                    <asp:Label ID="lblTitlePriceAdjustment" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Node Type" : "Update Node Type" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPriceAdjustment">
                                            <infs:WclTextBox runat="server" Text='<%# Eval("INT_ID") %>' ID="txtInstitutionNodeTypeId"
                                                Visible="false">
                                            </infs:WclTextBox>
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox ID="txtInstitutionNodeTypeName" Width="100%" runat="server" Text='<%# Eval("INT_Name") %>'
                                                        MaxLength="50">
                                                    </infs:WclTextBox>
                                                    <div id="dvLabel" class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvLabel" ControlToValidate="txtInstitutionNodeTypeName"
                                                            class="errmsg" ErrorMessage="Name is required." ValidationGroup='grpValdInstitutionNodeType'
                                                            Enabled="true" />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm '>
                                                    <infs:WclTextBox Width="100%" ID="txtInstitutionNodeTypeDescription" runat="server"
                                                        Text='<%# Eval("INT_Description") %>' MaxLength="255">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarPriceAdjustment" runat="server" GridMode="true" DefaultPanel="pnlPriceAdjustment" GridInsertText="Save" GridUpdateText="Save"
                                        ValidationGroup="grpValdInstitutionNodeType" ExtraButtonIconClass="icnreset" />
                                    <%--<infsu:CommandBar ID="fsucCmdBarPriceAdjustment" runat="server" DefaultPanel="PriceAdjustment"
                                        ValidationGroup='grpValdInstitutionNodeType'>
                                        <ExtraCommandButtons>
                                            <infs:WclButton runat="server" ID="btnSaveForm" ValidationGroup="grpValdInstitutionNodeType"
                                                Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>' CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'>
                                                <Icon PrimaryIconCssClass="rbSave" />
                                            </infs:WclButton>
                                            <infs:WclButton runat="server" ID="btnCancelForm" Text="Cancel" CommandName="Cancel">
                                                <Icon PrimaryIconCssClass="rbCancel" />
                                            </infs:WclButton>
                                        </ExtraCommandButtons>
                                    </infsu:CommandBar>--%>
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
