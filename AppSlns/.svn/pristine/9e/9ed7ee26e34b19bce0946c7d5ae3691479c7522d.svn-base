<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlacementSpecialty.ascx.cs" Inherits="CoreWeb.PlacementMatching.Views.PlacementSpecialty" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="SysXResourceManagerProxy1">
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
            <h1 class="header-color">Manage Specialty</h1>
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
    <div id="dvSpecialty" runat="server" class="row">
        <infs:WclGrid runat="server" ID="grSpecialty" AllowPaging="true" AutoGenerateColumns="false"
            AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0" EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true" ValidationGroup="grpSpecialty" NonExportingColumns="EditCommandColumn,DeleteColumn" OnNeedDataSource="grSpecialty_NeedDataSource"
            OnInsertCommand="grSpecialty_InsertCommand" OnUpdateCommand="grSpecialty_UpdateCommand" OnDeleteCommand="grSpecialty_DeleteCommand" EnableCachedExport="False" EnableCustomExporting="False" ExportCachePath="" ShowClearFiltersButton="False">
            <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                HideStructureColumns="true" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm">
                <Excel AutoFitImages="true" />
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
            <GroupingSettings CaseSensitive="false" />
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="SpecialtyId">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Specialty"
                    ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false"
                    ShowRefreshButton="false" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="Name" HeaderText="Specialty Name" FilterControlAltText="Filter Name Column"
                        SortExpression="Name" UniqueName="Name">
                    </telerik:GridBoundColumn>
                    <%-- <telerik:GridBoundColumn DataField="NodeLabel" HeaderText="Label" FilterControlAltText="Filter Label Column"
                        SortExpression="NodeLabel" UniqueName="NodeLabel">
                    </telerik:GridBoundColumn>--%>
                    <telerik:GridBoundColumn DataField="Description" HeaderText="Description" FilterControlAltText="Filter Description Column"
                        SortExpression="Description" UniqueName="Description">
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Specialty?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <HeaderStyle Width="30px" />
                        <%--  <ItemStyle CssClass="MyImageButton" Width="3%" HorizontalAlign="Center" />--%>
                    </telerik:GridButtonColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" Width="3%" />
                    </telerik:GridEditCommandColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                    <FormTemplate>
                        <div class="bgLightGreen">
                            <div class="col-md-12 bgLightGreen">
                                <h2 class="header-color paddTopBottom10 marginTopBottom0 heighAuto">
                                    <asp:Label ID="lblTitleNode" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Specialty" : "Update Specialty" %>'
                                        runat="server" /></h2>
                            </div>
                            <div class="msgbox">
                                <asp:Label runat="server" ID="lblName" CssClass="info"></asp:Label>
                            </div>
                            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlSpecialty">
                                <div class='form-group col-md-3'>
                                    <infs:WclTextBox runat="server" Text='<%# Eval("SpecialtyId") %>' ID="txtSpecialtyId" Visible="false">
                                    </infs:WclTextBox>
                                </div>
                                <div class="col-md-12">
                                    <div class="row bgLightGreen">
                                        <div class='form-group col-md-3'>
                                            <span class="cptn">Name</span><span class="reqd">*</span>
                                            <infs:WclTextBox ID="txtName" Width="100%" runat="server" Text='<%# Eval("Name") %>'
                                                MaxLength="50" CssClass="form-control">
                                            </infs:WclTextBox>
                                            <div id="Div1" class='vldx'>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                                                    Display="Dynamic" class="errmsg" ErrorMessage="Name is required." ValidationGroup='grpSpecialty'
                                                    Enabled="true" />
                                            </div>
                                        </div>
                                        <%--  <div class='form-group col-md-3'>
                                            <span class="cptn">Label</span>
                                            <infs:WclTextBox ID="txtLabel" Width="100%" runat="server" Text='<%# Eval("NodeLabel") %>'
                                                MaxLength="50" CssClass="form-control">
                                            </infs:WclTextBox>
                                        </div>--%>
                                        <div class='form-group col-md-3'>
                                            <span class="cptn">Description</span>
                                            <infs:WclTextBox Width="100%" ID="txtDescription" runat="server" Text='<%# Eval("Description") %>'
                                                MaxLength="255" CssClass="form-control">
                                            </infs:WclTextBox>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="col-md-12 text-right">
                            <infsu:CommandBar ID="fsucCmdBarNode" runat="server" GridMode="true" DefaultPanel="pnlSpecialty" GridInsertText="Save" GridUpdateText="Save"
                                ValidationGroup="grpSpecialty" ExtraButtonIconClass="icnreset" UseAutoSkinMode="false" ButtonSkin="Silk" />
                        </div>
                    </FormTemplate>
                </EditFormSettings>

            </MasterTableView>
        </infs:WclGrid>
    </div>
</div>
<script>
    function pageLoad() {
        $jQuery("[id$=txtName]").val($jQuery("[id$=txtName]").val()).focus();
    }
</script>
