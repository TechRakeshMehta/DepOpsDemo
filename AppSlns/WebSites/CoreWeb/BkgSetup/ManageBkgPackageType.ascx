<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageBkgPackageType.ascx.cs" Inherits="CoreWeb.BkgSetup.ManageBkgPackageType" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>


<infs:WclResourceManagerProxy runat="server" ID="rprxBkgPackageType">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<style type="text/css">
    .controlHidden
    {
        display: none;
    }

    .disabled
    {
        pointer-events: none;
        cursor: default;
        text-decoration: none;
        color: gray !important;
    }

    .highlightGrid
    {
        color: red !important;
    }

    .highlight
    {
        border: 2px solid red !important;
        border-radius: 5px !important;
    }

    .top3
    {
        top: 3px !important;
    }

    .btn
    {
        width: 100%;
        text-align: left;
    }

    .RadMenu .rmGroup .rmText
    {
        padding: 0px;
        margin: 0px;
    }

    .rmVertical.rmGroup.rmLevel1
    {
        border: none;
    }

    .customPie
    {
        width: 30% !Important;
        float: left;
        margin-right: 10px;
    }

    .customPie16
    {
        width: 16% !Important;
        float: left;
        margin-right: 10px;
    }

    .customPielabel
    {
        margin-right: 11px !important;
        width: 15%;
        float: left;
    }

    .pieparent
    {
        width: 100%;
        float: left;
    }

    .pieareaheight
    {
        height: auto;
        min-height: 41px;
    }

    .rcpRoundedRight
    {
        height: 15% !important;
    }
  
</style>
<div id="dvTop" class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Manage Package Type
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class='col-md-12'>
                <div class="row">
                    <div id="divTenant" runat="server" class='form-group col-md-3' title="Select the Institution whose data you want to view">
                        <span class="cptn">Institution</span>
                        <infs:WclComboBox ID="ddlTenant" runat="server" DataTextField="TenantName"
                            AutoPostBack="true" DataValueField="TenantID" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                            OnDataBound="ddlTenant_DataBound" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenant"
                                ValidationGroup="grpFormSearch"
                                InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Institution is required." />
                        </div>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the entered Package Type Name.">
                        <span class="cptn">Package Type Name</span>
                        <infs:WclTextBox ID="txtPackageTypeName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered Package Type Code.">
                        <span class="cptn">Package Type Code</span>
                        <infs:WclTextBox ID="txtPackageTypeCode" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <div class="col-md-12">&nbsp;</div>
        <div class="col-md-12">
            <div class="row ">
                <div style="width: 50%; float: left;">
                    <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Right" DisplayButtons="Submit,Save,Cancel"
                        AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo"
                        SubmitButtonText="Reset" SaveButtonText="Search" SaveButtonIconClass="rbSearch"
                        CancelButtonText="Cancel" OnSubmitClick="fsucCmdBarButton_SubmitClick" OnSaveClick="fsucCmdBarButton_SaveClick"
                        OnCancelClick="fsucCmdBarButton_CancelClick" UseAutoSkinMode="false" ButtonSkin="Silk">
                    </infsu:CommandBar>
                </div>
            </div>
            <div class="col-md-12">&nbsp;</div>
        </div>
    </div>

    <div class="row">
        <div id="Div1" runat="Server">
            <infs:WclGrid runat="server" ID="grdBkgPackageType" AllowCustomPaging="false"
                AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
                AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
                OnNeedDataSource="grdBkgPackageType_NeedDataSource" OnItemCommand="grdBkgPackageType_ItemCommand"
                OnSortCommand="grdBkgPackageType_SortCommand" OnItemDataBound="grdBkgPackageType_ItemDataBound"
                OnItemCreated="grdBkgPackageType_ItemCreated"
                NonExportingColumns="EditCommandColumn,DeleteColumn,PackageTypeColorCode" EnableLinqExpressions="false"
                ShowClearFiltersButton="false">
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="BPT_Id"
                    AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Package Type"
                        ShowExportToCsvButton="true"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                    <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="BPT_Name" HeaderText="Name" HeaderStyle-Width="30%" SortExpression="BPT_Name"
                            HeaderTooltip="This column displays the Package Type for each record in the grid"
                            UniqueName="Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BPT_Code" HeaderText="Code" HeaderStyle-Width="24%" SortExpression="BPT_Code"
                            HeaderTooltip="This column displays the Package Type Code for each record in the grid"
                            UniqueName="PackageTypeCode">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="BPT_Color" HeaderText="Color" HeaderStyle-Width="24%" SortExpression="BPT_Color"
                            HeaderTooltip="This column displays the Package Type Code for each record in the grid"
                            UniqueName="PackageTypeColorCode">
                            <ItemTemplate>
                                <asp:TextBox ID="txtcolor" runat="server"></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif"
                            CommandName="Delete" ConfirmText="Are you sure you want to delete this Package Type?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif"
                            UniqueName="EditCommandColumn">
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridEditCommandColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-12">
                                        <h2 class="header-color">
                                            <asp:Label ID="lblPkgType" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Package Type" : "Update Package Type" %>'
                                                runat="server" />
                                        </h2>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="msgbox">
                                            <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <asp:Panel ID="pnlEditForm" runat="server">
                                    <div class="row bgLightGreen">
                                        <div class="col-md-12">

                                            <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                                                <span class="cptn">Institution</span><span class='reqd'>*</span>

                                                <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName" AutoPostBack="true"
                                                    DataValueField="TenantID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                                    OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                                    Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvddlTenantName" ControlToValidate="ddlTenantName"
                                                        InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                                        Text="Institution is required." />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Package Type Name</span><span class='reqd'>*</span>
                                                <infs:WclTextBox CssClass="form-control" Text='<%# Eval("BPT_Name") %>' margin-top="6px" MaxLength="100" Width="100%" ID="txtPkgTypeName" runat="server"></infs:WclTextBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvPackageTypeName" ControlToValidate="txtPkgTypeName"
                                                        Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                                        Text="Package Name is required." />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Package Type Code</span><span class='reqd'>*</span>
                                                <infs:WclTextBox CssClass="form-control" Text='<%# Eval("BPT_Code") %>' margin-top="6px" MaxLength="3" Width="100%" ID="txtPkgTypeCode" runat="server">
                                                </infs:WclTextBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvPackageTypeCode" ControlToValidate="txtPkgTypeCode"
                                                        Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                                        Text="Package Type Code is required." />
                                                    <asp:RegularExpressionValidator runat="server" ID="rgvPackageTypeCode" ControlToValidate="txtPkgTypeCode"
                                                        class="errmsg" Display="Dynamic" ErrorMessage="Only alphanumeric is allowed."
                                                        ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="grpFormSubmit" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class='form-group col-md-2'>
                                                <span class="cptn" style="padding-bottom: 20px;">Package Type Color</span>
                                                <div style="min-width:230px !important;">
                                                    <telerik:RadColorPicker SkinID="Office" RenderMode="Lightweight" runat="server" ID="crpPieChartColor" AutoPostBack="true"
                                                        OnColorChanged="crpPieChartColor_ColorChanged" Width="25%"  Height="20%" PaletteModes="All" CssClass="ColorPickerPreview" Preset="Default"
                                                        KeepInScreenBounds="true">
                                                    </telerik:RadColorPicker>
                                                </div>


                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <infsu:CommandBar ID="fsucCmdBarRotation" runat="server" GridMode="true" DefaultPanel="pnlCategory"
                                    GridInsertText="Save" GridUpdateText="Save"
                                    ValidationGroup="grpFormSubmit" UseAutoSkinMode="false" ButtonSkin="Silk" />
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                        Position="TopAndBottom" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
    </div>
</div>
