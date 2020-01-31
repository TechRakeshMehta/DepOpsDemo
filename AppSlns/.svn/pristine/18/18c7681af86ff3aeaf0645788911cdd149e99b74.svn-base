<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.SetupRuleTemplate" CodeBehind="SetupRuleTemplate.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>--%>
<div class="section">
    <h1 class="mhdr">Rule Templates
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenant">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <asp:Label ID="lblTenant" runat="server" Text="Institution" CssClass="cptn"></asp:Label>
                    </div>
                    <div class='sxlm'>
                       <%-- <infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" DefaultMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged">
                        </infs:WclDropDownList>--%>
                        <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID"  EmptyMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                             Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"> 
                        </infs:WclComboBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdRuleTemplates" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0" ShowClearFiltersButton="true"
                GridLines="Both" EnableDefaultFeatures="true" ShowAllExportButtons="False" ShowExtraButtons="true"
                NonExportingColumns="EditCommandColumn,DeleteColumn" EnableLinqExpressions="false"
                OnNeedDataSource="grdRuleTemplates_NeedDataSource"
                OnItemCommand="grdRuleTemplates_ItemCommand" OnItemDataBound="grdRuleTemplates_ItemDataBound"
                OnInit="grdRuleTemplates_Init">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>

                <MasterTableView CommandItemDisplay="Top" DataKeyNames="RLT_ID" AllowFilteringByColumn="true"> 
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Rule Template"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="RLT_ID"
                            HeaderText="ID" SortExpression="RLT_ID" UniqueName="RLT_ID" Visible="false" AllowFiltering="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RLT_Name" AllowFiltering="true"
                            HeaderText="Rule Template Name" SortExpression="RLT_Name" UniqueName="RLT_Name" >
                        </telerik:GridBoundColumn>
                        <%--Code commented to bind columns directly through Entities Navigations--%>
                        <%--<telerik:GridTemplateColumn FilterControlAltText="Filter lkpRuleType column" HeaderText="Rule Type"
                            SortExpression="lkpRuleType" UniqueName="lkpRuleType">
                            <ItemTemplate>
                                <asp:Label ID="lblRuleType" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn FilterControlAltText="Filter lkpRuleActionType column"
                            HeaderText="Action Type" SortExpression="lkpRuleActionType" UniqueName="lkpRuleActionType">
                            <ItemTemplate>
                                <asp:Label ID="lblActionType" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn FilterControlAltText="Filter lkpRuleResultType column" HeaderText="Result Type"
                            SortExpression="lkpRuleResultType" UniqueName="lkpRuleResultType">
                            <ItemTemplate>
                                <asp:Label ID="lblResultType" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridBoundColumn DataField="lkpRuleType.RLT_Description" AllowFiltering="true"
                            HeaderText="Rule Type" SortExpression="lkpRuleType.RLT_Description" UniqueName="lkpRuleType.RLT_Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="lkpRuleActionType.ACT_Description" AllowFiltering="true"
                            HeaderText="Action Type" SortExpression="lkpRuleActionType.ACT_Description" UniqueName="lkpRuleActionType.ACT_Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="lkpRuleResultType.RSL_Description" AllowFiltering="true"
                            HeaderText="Result Type" SortExpression="lkpRuleResultType.RSL_Description" UniqueName="lkpRuleResultType.RSL_Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RLT_IsActive" AllowFiltering="true" DataType="System.Boolean"
                            HeaderText="Is Active" SortExpression="RLT_IsActive" UniqueName="RLT_IsActive">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn UniqueName="EditCommandColumn" AllowFiltering="false">
                            <HeaderStyle CssClass="tplcohdr" Width="1px" />
                            <ItemTemplate>
                                <telerik:RadButton ID="btnEdit" ButtonType="LinkButton" CommandName="Edit" runat="server"
                                    Text="Edit" BackColor="Transparent" Font-Underline="true" BorderStyle="None">
                                    <Icon PrimaryIconCssClass="rgEdit" PrimaryIconLeft="10px" />
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Rule Template?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" Width="30px" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <!-- enter edit form here or change edit form type to UserControl or Popup -->
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
