<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageCompliancePriorityObjectMapping.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.ManageCompliancePriorityObjectMapping" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>


<infs:WclResourceManagerProxy runat="server" ID="SysXResourceManagerProxy1">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />  
<infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>




<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h1 class="header-color">Manage Compliance Priority Object Mapping</h1>
        </div>
    </div>

    <div class="row bgLightGreen">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class='col-md-12'>
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span><span class='reqd'>*</span>
                            <infs:WclComboBox ID="ddlTenant" runat="server" DataTextField="TenantName" AutoPostBack="true"
                                DataValueField="TenantID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" EnableAriaSupport="true"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged" OnDataBound="ddlTenant_DataBound">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenant" InitialValue="--SELECT--"
                                   Display="Dynamic" ValidationGroup="grpCompObjMapping" CssClass="errmsg" 
                                    Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-12">&nbsp;</div>
            <div class="col-md-12">
                <div class="row text-center">
                    <infsu:CommandBar ID="cmdBarCompObjMapping" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                        AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo"
                        SubmitButtonText="Reset" SaveButtonText="Search" SaveButtonIconClass="rbSearch" ValidationGroup="grpCompObjMapping"
                        CancelButtonText="Cancel" OnSubmitClick="cmdBarCompObjMapping_ResetClick" OnSaveClick="cmdBarCompObjMapping_SearchClick"
                        OnCancelClick="cmdBarCompObjMapping_CancelClick" UseAutoSkinMode="false" ButtonSkin="Silk">
                    </infsu:CommandBar>
                </div>
                <div class="col-md-12">&nbsp;</div>
            </div>
        </asp:Panel>
    </div>
    <div id="dvCompObjMapping" class="row">
        <infs:WclGrid runat="server" ID="grdCompObjMapping" AllowPaging="true" AutoGenerateColumns="false" CssClass="gridhover"
            AllowSorting="true" AllowFilteringByColumn="true" AutoSkinMode="true" CellSpacing="0" GridLines="Both" EnableDefaultFeatures="true"
            ShowAllExportButtons="false" ShowExtraButtons="true" ValidationGroup="grpCompObjMapping"
            PageSize="10" NonExportingColumns="EditCommandColumn, DeleteColumn" OnNeedDataSource="grdCompObjMapping_NeedDataSource" OnItemCommand="grdCompObjMapping_ItemCommand"
            OnItemDataBound="grdCompObjMapping_ItemDataBound">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <GroupingSettings CaseSensitive="false" />
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="CCIPOM_ID" AllowFilteringByColumn="true">
                <CommandItemSettings ShowExportToExcelButton="true"
                    ShowExportToPdfButton="true" ShowExportToCsvButton="true" AddNewRecordText="Add New Compliance Priority Object Mapping" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>

                    <telerik:GridNumericColumn DataField="CCIPOM_ID" FilterControlAltText="Filter CCIPOM_ID column"
                        HeaderText="CCIPOM_ID" SortExpression="CCIPOM_ID" DataType="System.Int32" UniqueName="CCIPOM_ID" Display="false"
                        DecimalDigits="0" HeaderTooltip="This column displays the compliance priority object Mapping id for each record in the grid">
                    </telerik:GridNumericColumn>

                    <telerik:GridBoundColumn DataField="CPO_Name" FilterControlAltText="Filter CPO_Name column"
                        HeaderText="Compliance Object" SortExpression="CPO_Name" UniqueName="CPO_Name"
                        HeaderTooltip="This column displays the name of compliance object for each record in the grid">
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn DataField="CategoryName" FilterControlAltText="Filter CategoryName column"
                        HeaderText="Category" SortExpression="CategoryName" UniqueName="CategoryName"
                        HeaderTooltip="This column displays the name of category for each record in the grid">
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn DataField="ItemName" FilterControlAltText="Filter ItemName column"
                        HeaderText="Item" SortExpression="ItemName" UniqueName="ItemName"
                        HeaderTooltip="This column displays the name of item for each record in the grid">
                    </telerik:GridBoundColumn>

                    <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif"
                        UniqueName="EditCommandColumn">
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridEditCommandColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif"
                        CommandName="Delete" ConfirmText="Are you sure you want to delete this compliance priority object Mapping?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                </Columns>

                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                    <FormTemplate>
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-12">
                                    <h2 class="header-color">
                                        <asp:Label ID="lblTitleCompObjMapping" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Compliance Object Mapping" : "Update Compliance Object Mapping" %>'
                                            runat="server" /></h2>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="msgbox">
                                        <asp:Label runat="server" ID="lblName" CssClass="info"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlCompObjMapping">
                                <div class="row bgLightGreen">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <%--<div class='form-group col-md-3'>
                                                <span class="cptn">Institution</span><span class='reqd'>*</span>
                                                <infs:WclComboBox ID="ddlTenant" runat="server" DataTextField="TenantName" AutoPostBack="true"
                                                    DataValueField="TenantID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                                    Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenant"
                                                        InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpCompObjMapping" CssClass="errmsg"
                                                        Text="Institution is required." />
                                                </div>
                                            </div>--%>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Compliance Object</span><span class='reqd'>*</span>
                                                <infs:WclComboBox ID="ddlObject" runat="server" DataTextField="CPO_Name" AutoPostBack="false"
                                                    DataValueField="CPO_ID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                                    Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvObjectr" ControlToValidate="ddlObject"
                                                        InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpCompObjMapping" CssClass="errmsg"
                                                        Text="Compliance object is required." />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Category</span><span class='reqd'>*</span>
                                                <infs:WclComboBox ID="ddlCategory" runat="server" DataTextField="CategoryName" AutoPostBack="true"
                                                    DataValueField="CategoryID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                                    Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvCategory" ControlToValidate="ddlCategory"
                                                        InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpCompObjMapping" CssClass="errmsg"
                                                        Text="Category is required." />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Item</span>
                                                <infs:WclComboBox ID="ddlItem" runat="server" DataTextField="ItemName" AutoPostBack="false"
                                                    DataValueField="ItemID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Enabled="false"
                                                    Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" EmptyMessage="--SELECT--">
                                                </infs:WclComboBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div class="col-md-12 text-right">
                                <infsu:CommandBar ID="fsucCmdBarNode" runat="server" GridMode="true" GridInsertText="Save" GridUpdateText="Save"
                                    ValidationGroup="grpCompObjMapping" ExtraButtonIconClass="icnreset" UseAutoSkinMode="false" ButtonSkin="Silk" />
                            </div>
                        </div>
                    </FormTemplate>
                </EditFormSettings>

                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" Position="TopAndBottom" />
            </MasterTableView>
        </infs:WclGrid>
    </div>
</div>
