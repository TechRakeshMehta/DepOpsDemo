<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageAgencyNode.ascx.cs" Inherits="CoreWeb.AgencyHierarchy.Views.ManageAgencyNode" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="SysXResourceManagerProxy1">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h1 class="header-color">Manage Agency Node</h1>
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
        <div class='col-md-12'>
            <div class="row">
                <div class='form-group col-md-3'>
                    <span class="cptn">Agency Node Name</span>
                    <infs:WclTextBox CssClass="form-control" Width="100%" ID="txtAgencyNodeName" runat="server"></infs:WclTextBox>
                </div>
                <div class='form-group col-md-3'>
                    <span class="cptn">Description</span>
                    <infs:WclTextBox CssClass="form-control" Width="100%" ID="txtDescription" runat="server"></infs:WclTextBox>
                </div>
            </div>
        </div>

        <div class="col-md-12">
            <div class="row ">
                <div style="width: 50%; float: left;">
                    <%--UAT-3138--%>
                    <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Right" DisplayButtons="Submit,Save,Cancel"
                        AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo"
                        SubmitButtonText="Reset" SaveButtonText="Search" SaveButtonIconClass="rbSearch"
                        CancelButtonText="Cancel" OnSubmitClick="fsucCmdBarButton_SubmitClick" OnSaveClick="fsucCmdBarButton_SaveClick"
                        OnCancelClick="fsucCmdBarButton_CancelClick" UseAutoSkinMode="false" ButtonSkin="Silk">
                    </infsu:CommandBar>
                </div>
            </div>
        </div>
    </div>

    <div id="dvNode" runat="server" class="row">
        <infs:WclGrid runat="server" ID="grAgencyNode" AllowCustomPaging="true" AutoGenerateColumns="false"
            AllowSorting="false" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
            GridLines="Both" EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true" ValidationGroup="grpNode"
            PageSize="10" NonExportingColumns="EditCommandColumn,DeleteColumn" OnNeedDataSource="grAgencyNode_NeedDataSource" ShowClearFiltersButton="false"
            OnInsertCommand="grAgencyNode_InsertCommand" OnUpdateCommand="grAgencyNode_UpdateCommand" OnDeleteCommand="grAgencyNode_DeleteCommand" OnSortCommand="grAgencyNode_SortCommand">
            <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                HideStructureColumns="true" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm">
                <Excel AutoFitImages="true" />
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
            <GroupingSettings CaseSensitive="false" />
            <MasterTableView CommandItemDisplay="Top" AllowFilteringByColumn="false" AllowSorting="false" DataKeyNames="NodeId">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Node"
                    ShowExportToCsvButton="true" ShowExportToExcelButton="true" ShowExportToPdfButton="true"
                    ShowRefreshButton="true" />
                <RowIndicatorColumn Visible="false" FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="NodeName" HeaderText="Name" AllowFiltering="false" FilterControlAltText="Filter Name Column"
                        SortExpression="NodeName" UniqueName="NodeName">
                    </telerik:GridBoundColumn>
                    <%-- <telerik:GridBoundColumn DataField="NodeLabel" HeaderText="Label" FilterControlAltText="Filter Label Column"
                        SortExpression="NodeLabel" UniqueName="NodeLabel">
                    </telerik:GridBoundColumn>--%>
                    <telerik:GridBoundColumn DataField="NodeDescription" HeaderText="Description" FilterControlAltText="Filter Description Column" AllowFiltering="false"
                        SortExpression="NodeDescription" UniqueName="NodeDescription">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="MappedRootHierachies" HeaderText="Mapped Root Nodes" FilterControlAltText="Filter Mapped Root Hierachies Column" AllowFiltering="false"
                        SortExpression="MappedRootHierachies" UniqueName="MappedRootHierachies">
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Node?"
                        Text="Delete" UniqueName="DeleteColumn" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif">
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
                                    <asp:Label ID="lblTitleNode" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Node" : "Update Node" %>'
                                        runat="server" /></h2>
                            </div>
                            <div class="msgbox">
                                <asp:Label runat="server" ID="lblName" CssClass="info"></asp:Label>
                            </div>
                            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlNode">
                                <div class='form-group col-md-3'>
                                    <infs:WclTextBox runat="server" Text='<%# Eval("NodeId") %>' ID="txtNodeId" Visible="false">
                                    </infs:WclTextBox>
                                </div>
                                <div class="col-md-12">
                                    <div class="row bgLightGreen">
                                        <div class='form-group col-md-3'>
                                            <span class="cptn">Name</span><span class="reqd">*</span>
                                            <infs:WclTextBox ID="txtName" Width="100%" runat="server" Text='<%# Eval("NodeName") %>'
                                                MaxLength="50" CssClass="form-control">
                                            </infs:WclTextBox>
                                            <div id="Div1" class='vldx'>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                                                    Display="Dynamic" class="errmsg" ErrorMessage="Name is required." ValidationGroup='grpNode'
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
                                            <infs:WclTextBox Width="100%" ID="txtDescription" runat="server" Text='<%# Eval("NodeDescription") %>'
                                                MaxLength="255" CssClass="form-control">
                                            </infs:WclTextBox>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="col-md-12 text-right">
                            <infsu:CommandBar ID="fsucCmdBarNode" runat="server" GridMode="true" DefaultPanel="pnlNode" GridInsertText="Save" GridUpdateText="Save"
                                ValidationGroup="grpNode" ExtraButtonIconClass="icnreset" UseAutoSkinMode="false" ButtonSkin="Silk" />
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                    Position="TopAndBottom" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
        </infs:WclGrid>
    </div>
</div>
