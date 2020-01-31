<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.ManageSubTenant" Codebehind="ManageSubTenant.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <div class="msgbox">
        <asp:Label ID="lblErrorMessage" runat="server" CssClass="error"></asp:Label>
    </div>
    <h1 class="mhdr">
        <asp:Label ID="lblManageSubTenant" runat="server"></asp:Label></h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdManageSubTenant" AllowPaging="True" PageSize="10"
                AutoGenerateColumns="False" AllowSorting="True" GridLines="Both" EnableDefaultFeatures="true" ShowAllExportButtons="false"
                OnItemCommand="grdManageSubTenant_ItemCommand" OnNeedDataSource="grdManageSubTenant_NeedDataSource"
                OnDeleteCommand="grdManageSubTenant_DeleteCommand" NonExportingColumns="DeleteColumn">
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <ExportSettings>
                    <Pdf PageHeight="210mm" PageWidth="350mm" PageTitle="Manage Third Party" DefaultFontFamily="Arial Unicode MS"
                        PageBottomMargin="20mm" PageTopMargin="20mm" PageLeftMargin="20mm" PageRightMargin="20mm" />
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" TableLayout="Auto" AllowFilteringByColumn="True"
                    DataKeyNames="TenantID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add Third Party" ShowExportToWordButton= "false"
                    ShowExportToCsvButton="true" ShowExportToExcelButton="true" ShowExportToPdfButton="true"/>
                    <Columns>
                        <telerik:GridNumericColumn DataField="TenantID" HeaderText="Reviewer ID" UniqueName="TenantID"
                            FilterControlWidth="60px">
                        </telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="TenantName" HeaderText="Reviewer Name" UniqueName="SupplierName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TenantAddress" HeaderText="Reviewer Address"
                            UniqueName="Addresses">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TenantCity" HeaderText="Reviewer City" UniqueName="City">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TenantState" HeaderText="Reviewer State" UniqueName="State">
                        </telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="TenantZipCode" HeaderText="Reviewer ZipCode"
                            UniqueName="ZipCode" >
                        </telerik:GridNumericColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Tenant?"
                            Text="Inactivate" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
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

<div class="section" id="dvAddSubTenant" runat="server" visible="false">
    <h1 class="mhdr">
        <asp:Label ID="lblHeading" runat="server" Text="Add Third Party"></asp:Label></h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdAddSubTenant" DataSourceID="" AllowPaging="True"
                PageSize="10" AutoGenerateColumns="False" AllowSorting="True" GridLines="Both"
                EnableDefaultFeatures="true" OnNeedDataSource="grdAddSubTenant_NeedDataSource"
                OnItemDataBound="grdAddSubTenant_ItemDataBound" ShowAllExportButtons="false">
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <ExportSettings>
                    <Pdf PageHeight="210mm" PageWidth="350mm" PageTitle="Manage Third Party" DefaultFontFamily="Arial Unicode MS"
                        PageBottomMargin="20mm" PageTopMargin="20mm" PageLeftMargin="20mm" PageRightMargin="20mm" />
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" TableLayout="Auto" AllowFilteringByColumn="True"
                    DataKeyNames="TenantID" ClientDataKeyNames="TenantID,TenantName">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true" ShowExportToExcelButton="true" ShowExportToPdfButton ="true"/>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Active" AllowFiltering="false" HeaderStyle-Width="200"
                            Resizable="false">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkStatus" runat="server" AutoPostBack="True" OnCheckedChanged="chkStatus_CheckedChanged" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridNumericColumn DataField="TenantID" HeaderText="Reviewer ID" UniqueName="TenantID"
                            FilterControlWidth="60px">
                        </telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="TenantName" HeaderText="Reviewer Name" UniqueName="TenantName">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
    </div>
    <infsu:CommandBar ID="fsucCmdBarManageSubTenant" runat="server" DisplayButtons="Save,Cancel"
        AutoPostbackButtons="Save,Cancel" CancelButtonText="Cancel" SaveButtonText="Save"
        OnCancelClick="btnCancel_Click" OnSaveClick="btnSave_Click" ButtonPosition="Center" />
</div>
