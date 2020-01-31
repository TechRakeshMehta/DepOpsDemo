<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ContractManagement.Views.ManageContractTypes" CodeBehind="ManageContractTypes.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxManageContractTypes">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy> 
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Manage Contract Types</h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel runat="server" ID="pnlTenant">
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <asp:Label ID="lblSelectClient" runat="server" Text="Institution" CssClass="cptn"></asp:Label>
                        <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            AutoSkinMode="false" Width="100%" CssClass="form-control" Skin="Silk"
                            DataValueField="TenantID" EmptyMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChange"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

    <div id="dvContractTypes" runat="server" class="row">
        <infs:WclGrid runat="server" ID="grdContractTypes" AllowPaging="True" PageSize="10"
            AutoGenerateColumns="False" AllowSorting="True" GridLines="Both" ShowAllExportButtons="False"
            NonExportingColumns="EditCommandColumn, DeleteColumn" ValidationGroup="grpValdContractTypes"
            OnNeedDataSource="grdContractTypes_NeedDataSource" OnItemCommand="grdContractTypes_ItemCommand"
            OnInsertCommand="grdContractTypes_InsertCommand" OnUpdateCommand="grdContractTypes_UpdateCommand"
            OnDeleteCommand="grdContractTypes_DeleteCommand">
            <ExportSettings Pdf-PageWidth="300mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="CT_ID">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Contract Type"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true">
                </CommandItemSettings>
                <Columns>
                    <telerik:GridBoundColumn DataField="CT_Name" FilterControlAltText="Filter CT_Name column"
                        HeaderText="Name" SortExpression="CT_Name" UniqueName="CT_Name">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CT_Label" FilterControlAltText="Filter CT_Label column"
                        HeaderText="Label" SortExpression="CT_Label" UniqueName="CT_Label">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CT_Description" FilterControlAltText="Filter CT_Description column"
                        HeaderText="Description" SortExpression="CT_Description" UniqueName="CT_Description">
                    </telerik:GridBoundColumn> 
                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Contract Type?"
                        Text="Delete" UniqueName="DeleteColumn" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif">
                        <HeaderStyle Width="30px" />
                    </telerik:GridButtonColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit"
                        EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif" UniqueName="EditCommandColumn">
                        <HeaderStyle Width="30px" />
                    </telerik:GridEditCommandColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div class="col-md-12" runat="server" id="divEditBlock" visible="true">
                            <div class="row">
                                <div class="col-md-12">
                                    <h2 class="header-color">
                                        <asp:Label ID="lblTitlePriceAdjustment" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Contract Type" : "Update Contract Type" %>'
                                            runat="server" /></h2>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="msgbox">
                                        <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="row bgLightGreen">
                                <asp:Panel runat="server" ID="pnlContractType">
                                    <div class='col-md-12'>
                                        <div class="row">
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Name</span><span class="reqd">*</span>
                                                <infs:WclTextBox ID="txtContractTypeName" Width="100%" runat="server" Text='<%# Eval("CT_Name") %>'
                                                    MaxLength="50" CssClass="form-control">
                                                </infs:WclTextBox>
                                                <div id="dvLabel" class='vldx'>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvContractTypeName" ControlToValidate="txtContractTypeName"
                                                        class="errmsg" ErrorMessage="Name is required." ValidationGroup='grpValdContractType'
                                                        Enabled="true" />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Label</span>
                                                <infs:WclTextBox ID="txtContractTypeLabel" Width="100%" runat="server" Text='<%# Eval("CT_Label") %>'
                                                    MaxLength="50" CssClass="form-control">
                                                </infs:WclTextBox>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Description</span>
                                                <infs:WclTextBox Width="100%" ID="txtContractTypeDescription" runat="server"
                                                    Text='<%# Eval("CT_Description") %>' MaxLength="255" CssClass="form-control">
                                                </infs:WclTextBox>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="row text-right">
                                <infsu:CommandBar ID="fsucCmdBarPriceAdjustment" runat="server" GridMode="true" DefaultPanel="pnlContractType"
                                    GridInsertText="Save" GridUpdateText="Save" UseAutoSkinMode="false" ButtonSkin="Silk"
                                    ValidationGroup="grpValdContractType" ExtraButtonIconClass="icnreset" />
                            </div>
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
        </infs:WclGrid>
    </div>


</div>
