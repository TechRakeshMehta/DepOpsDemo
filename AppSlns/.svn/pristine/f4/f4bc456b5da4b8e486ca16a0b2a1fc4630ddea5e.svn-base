<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SetupRuleTemplateBkg.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.SetupRuleTemplateBkg" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div class="section">
    <h1 class="mhdr">
        Rule Templates
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
                        <%--<infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
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
            <infs:WclGrid runat="server" ID="grdRuleTemplates" DataSourceID="" AllowPaging="True"
                ShowAllExportButtons="false" PageSize="10" AutoGenerateColumns="False" AllowSorting="True"
                GridLines="Both" NonExportingColumns="EditCommandColumn,DeleteColumn,BRLT_ID" OnNeedDataSource="grdRuleTemplates_NeedDataSource"
                OnItemCommand="grdRuleTemplates_ItemCommand" OnItemDataBound="grdRuleTemplates_ItemDataBound"
                OnInit="grdRuleTemplates_Init">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="BRLT_ID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Rule Template"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"
                        ShowRefreshButton="true" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="BRLT_ID" FilterControlAltText="Filter BRLT_ID column"
                            HeaderText="ID" SortExpression="BRLT_ID" UniqueName="BRLT_ID" Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BRLT_Name" FilterControlAltText="Filter BRLT_Name column"
                            HeaderText="Rule Template Name" SortExpression="BRLT_Name" UniqueName="BRLT_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="lkpBkgRuleResultType.BRSL_Description" FilterControlAltText="Filter ResultType column"
                            HeaderText="Result Type" SortExpression="lkpBkgRuleResultType.BRSL_Description" UniqueName="ResultType">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BRLT_IsActive" FilterControlAltText="Filter BRLT_IsActive column"
                            HeaderText="Is Active" SortExpression="BRLT_IsActive" UniqueName="BRLT_IsActive">
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