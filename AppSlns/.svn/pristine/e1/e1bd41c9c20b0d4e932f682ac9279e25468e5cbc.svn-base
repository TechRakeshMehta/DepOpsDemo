<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageRoleConfiguration.ascx.cs" Inherits="CoreWeb.IntsofSecurityModel.Views.ManageRoleConfiguration" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Import Namespace="INTSOF.Utils" %>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color" tabindex="0">
                <asp:Label runat="server" ID="lblRoleConfig" CssClass="page-heading"></asp:Label>
            </h2>
        </div>
    </div>

    <infs:WclGrid runat="server" ID="grdRoleConfiguration" AutoGenerateColumns="False" AllowSorting="True" CssClass="gridhover"
        AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0" EnableLinqExpressions="false" ClientSettings-AllowKeyboardNavigation="true"
        ShowClearFiltersButton="false" GridLines="Both" ShowAllExportButtons="False" EnableAriaSupport="true" AllowPaging="true" AllowCustomPaging="false"
        OnNeedDataSource="grdRoleConfiguration_NeedDataSource" OnItemCommand="grdRoleConfiguration_ItemCommand" OnItemDataBound="grdRoleConfiguration_ItemDataBound"
        NonExportingColumns="IsAllowPreferredTenantCheckbox,IsAllowDataEntryCheckbox,IsAllowComplianceVerficationCheckbox,IsAllowRotationVerficationCheckbox,IsAllowLocationEnrollerCheckbox">
        <ClientSettings EnableRowHoverStyle="true">
            <Selecting AllowRowSelect="true"></Selecting>
        </ClientSettings>
        <GroupingSettings CaseSensitive="false" />
        <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
            Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
            ExportOnlyData="true" IgnorePaging="true">
        </ExportSettings>
        <MasterTableView CommandItemDisplay="Top" DataKeyNames="RPTS_ID,RPTS_RoleID" AllowFilteringByColumn="false">
            <CommandItemSettings ShowExportToExcelButton="true"
                ShowExportToPdfButton="true" ShowExportToCsvButton="true" />
            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
            </RowIndicatorColumn>
            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
            </ExpandCollapseColumn>
            <Columns>
                <telerik:GridBoundColumn DataField="RoleName" FilterControlAltText="Filter RoleName column"
                    HeaderText="Role Name" SortExpression="RoleName" UniqueName="RoleName"
                    HeaderTooltip="This column displays the role name for each record in the grid">
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="RoleDescription" FilterControlAltText="Filter RoleDescription column"
                    HeaderText="Description" SortExpression="RoleDescription" UniqueName="RoleDescription"
                    HeaderTooltip="This column displays the role description for each record in the grid">
                </telerik:GridBoundColumn>

                <telerik:GridTemplateColumn UniqueName="IsAllowPreferredTenantCheckbox" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Allow Preferred Tenant"
                    DataField="RPTS_IsAllowPreferredTenant" SortExpression="RPTS_IsAllowPreferredTenant" HeaderTooltip="This column displays the default preferred selection setting for each record in the grid">
                    <ItemTemplate>
                        <infs:WclButton ID="chkIsAllowPreferredTenant" runat="server" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="True" CommandName="CheckChange">
                            <ToggleStates>
                                <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                <telerik:RadButtonToggleState Text="No" Value="False" />
                            </ToggleStates>
                        </infs:WclButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn UniqueName="_IsAllowPreferredTenantExp" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Allow Preferred Tenant" Display="false"
                    DataField="_IsAllowPreferredTenantExp" SortExpression="_IsAllowPreferredTenantExp">
                </telerik:GridBoundColumn>

                <telerik:GridTemplateColumn UniqueName="IsAllowDataEntryCheckbox" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Allow Data Entry"
                    DataField="RPTS_IsAllowDataEntry" SortExpression="RPTS_IsAllowDataEntry" HeaderTooltip="This column displays the default preferred selection setting for each record in the grid">
                    <ItemTemplate>
                        <infs:WclButton ID="chkIsAllowDataEntry" runat="server" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="True" CommandName="DataEntryCheck">
                            <ToggleStates>
                                <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                <telerik:RadButtonToggleState Text="No" Value="False" />
                            </ToggleStates>
                        </infs:WclButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn UniqueName="_IsAllowDataEntryExp" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Allow Data Entry" Display="false"
                    DataField="_IsAllowDataEntryExp" SortExpression="_IsAllowDataEntryExp">
                </telerik:GridBoundColumn>

                <telerik:GridTemplateColumn UniqueName="IsAllowComplianceVerficationCheckbox" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Allow Compliance Verfication"
                    DataField="RPTS_IsAllowComplianceVerfication" SortExpression="RPTS_IsAllowComplianceVerfication" HeaderTooltip="This column displays the default preferred selection setting for each record in the grid">
                    <ItemTemplate>
                        <infs:WclButton ID="chkIsAllowComplianceVerfication" runat="server" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="True" CommandName="ComplianceVerficationCheck">
                            <ToggleStates>
                                <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                <telerik:RadButtonToggleState Text="No" Value="False" />
                            </ToggleStates>
                        </infs:WclButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn UniqueName="_IsAllowComplianceVerficationExp" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Allow Compliance Verfication" Display="false"
                    DataField="_IsAllowComplianceVerficationExp" SortExpression="_IsAllowComplianceVerficationExp">
                </telerik:GridBoundColumn>

                <telerik:GridTemplateColumn UniqueName="IsAllowRotationVerficationCheckbox" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Allow Rotation Verfication"
                    DataField="RPTS_IsAllowRotationVerfication" SortExpression="RPTS_IsAllowRotationVerfication" HeaderTooltip="This column displays the default preferred selection setting for each record in the grid">
                    <ItemTemplate>
                        <infs:WclButton ID="chkIsAllowRotationVerfication" runat="server" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="True" CommandName="RotationVerficationCheck">
                            <ToggleStates>
                                <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                <telerik:RadButtonToggleState Text="No" Value="False" />
                            </ToggleStates>
                        </infs:WclButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn UniqueName="_IsAllowRotationVerficationExp" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Allow Rotation Verfication" Display="false"
                    DataField="_IsAllowRotationVerficationExp" SortExpression="_IsAllowRotationVerficationExp">
                </telerik:GridBoundColumn>

                <telerik:GridTemplateColumn UniqueName="IsAllowLocationEnrollerCheckbox" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Allow Location Enroller"
                    DataField="RPTS_IsAllowLocationEnroller" SortExpression="RPTS_IsAllowLocationEnroller" HeaderTooltip="This column displays the default preferred selection setting for each record in the grid">
                    <ItemTemplate>
                        <infs:WclButton ID="chkIsAllowLocationEnroller" runat="server" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="True" CommandName="LocationEnrollerCheck">
                            <ToggleStates>
                                <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                <telerik:RadButtonToggleState Text="No" Value="False" />
                            </ToggleStates>
                        </infs:WclButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn UniqueName="_IsAllowLocationEnrollerExp" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Allow Location Enroller" Display="false"
                    DataField="_IsAllowLocationEnrollerExp" SortExpression="_IsAllowLocationEnrollerExp">
                </telerik:GridBoundColumn>

            </Columns>
            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" Position="TopAndBottom" />
        </MasterTableView>
        <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
        <FilterMenu EnableImageSprites="False">
        </FilterMenu>
    </infs:WclGrid>

</div>
