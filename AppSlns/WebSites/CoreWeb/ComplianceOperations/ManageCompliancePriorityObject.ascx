<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageCompliancePriorityObject.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.ManageCompliancePriorityObject" %>

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
            <h1 class="header-color">Manage Compliance Priority Object</h1>
        </div>
    </div>

    <div id="dvComplianceObject" class="row">
        <infs:WclGrid runat="server" ID="grdCompPriorityObject" AllowPaging="true" AutoGenerateColumns="false" CssClass="gridhover"
            AllowSorting="true" AllowFilteringByColumn="true" AutoSkinMode="true" CellSpacing="0" GridLines="Both" EnableDefaultFeatures="true"
            ShowAllExportButtons="false" ShowExtraButtons="true" ValidationGroup="grdCompliancePriorityObject"
            PageSize="10" NonExportingColumns="CPO_ID,EditCommandColumn, DeleteColumn" OnNeedDataSource="grdCompPriorityObject_NeedDataSource" OnItemCommand="grdCompPriorityObject_ItemCommand">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <GroupingSettings CaseSensitive="false" />
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="CPO_ID" AllowFilteringByColumn="true">
                <CommandItemSettings ShowExportToExcelButton="true"
                    ShowExportToPdfButton="true" ShowExportToCsvButton="true" AddNewRecordText="Add New Compliance Priority Object" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <%-- <telerik:GridTemplateColumn UniqueName="AssignItems" HeaderTooltip="Click this box to select all users on the active page"
                        AllowFiltering="true" ShowFilterIcon="false">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAllOrderQueue(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectOrders" runat="server" onclick="UnCheckHeader(this)" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>

                    <telerik:GridNumericColumn DataField="CPO_ID" FilterControlAltText="Filter CPO_ID column"
                        HeaderText="CPO_ID" SortExpression="CPO_ID" DataType="System.Int32" UniqueName="CPO_ID" Display="false"
                        DecimalDigits="0" HeaderTooltip="This column displays the compliance priority object id for each record in the grid">
                    </telerik:GridNumericColumn>

                    <telerik:GridBoundColumn DataField="CPO_Name" FilterControlAltText="Filter CPO_Name column"
                        HeaderText="Name" SortExpression="CPO_Name" UniqueName="CPO_Name"
                        HeaderTooltip="This column displays the name of compliance priority object for each record in the grid">
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn DataField="CPO_Description" FilterControlAltText="Filter CPO_Description column"
                        HeaderText="Description" SortExpression="CPO_Description" UniqueName="CPO_Description"
                        HeaderTooltip="This column displays the description of compliance priority object for each record in the grid">
                    </telerik:GridBoundColumn>

                    <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif"
                        UniqueName="EditCommandColumn">
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridEditCommandColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif"
                        CommandName="Delete" ConfirmText="Are you sure you want to delete this compliance priority object?"
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
                                        <asp:Label ID="lblTitleCompliancePriorityObject" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Compliance Priority Object" : "Update Compliance Priority Object" %>'
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
                            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlCompPriorityObject">
                                <div class="row bgLightGreen">
                                    <div class="col-md-12">
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Name</span><span class='reqd'>*</span>
                                                <infs:WclTextBox runat="server" ID="txtName" MaxLength="50" Text='<%# Eval("CPO_Name") %>' Width="100%" CssClass="form-control">
                                                </infs:WclTextBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                                                        Display="Dynamic" CssClass="errmsg" ValidationGroup="grdCompliancePriorityObject"
                                                        Text="Name is required." />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Description</span><%--<span class="reqd">*</span>--%>
                                                <infs:WclTextBox Width="100%" ID="txtDescription" runat="server" Skin="Silk" Text='<%# Eval("CPO_Description") %>'
                                                    TextMode="MultiLine" MaxLength="1024" CssClass="form-control" AutoSkinMode="false">
                                                </infs:WclTextBox>
                                            </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div class="col-md-12 text-right">
                                <infsu:CommandBar ID="fsucCmdBarNode" runat="server" GridMode="true" GridInsertText="Save" GridUpdateText="Save"
                                    ValidationGroup="grdCompliancePriorityObject" ExtraButtonIconClass="icnreset" UseAutoSkinMode="false" ButtonSkin="Silk" />
                            </div>
                        </div>
                    </FormTemplate>
                </EditFormSettings>

                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" Position="TopAndBottom" />
            </MasterTableView>
        </infs:WclGrid>
    </div>

</div>
