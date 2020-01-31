<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.Messaging.Views.SubscriptionSettingByAdmin" Codebehind="SubscriptionSettingByAdmin.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<style type="text/css">
    
      label {
  font-size: 11px !important;
}
</style>
<div class="msgbox" id="divSuccessMsg">
    <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
</div>
<div class="section">
    <h1 class="mhdr">
        Email Settings</h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdUsers" AllowPaging="True" AllowCustomPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" OnNeedDataSource="grdUsers_NeedDataSource"
                AutoSkinMode="True" CellSpacing="0" EnableDefaultFeatures="true" ShowAllExportButtons="false" EnableLinqExpressions="false"
                GridLines="Both" ShowExtraButtons="False" ShowHeader="true" OnInit="grdUsers_Init" OnItemCommand="grdUsers_ItemCommand" OnItemDataBound="grdUsers_ItemDataBound">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" GroupLoadMode="Client" ClientDataKeyNames="OrganizationUserID,FirstName"
                    DataKeyNames="OrganizationUserID,FirstName" >
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false"
                        ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="OrganizationUserID" FilterControlAltText="Filter ID column" DataType="System.Int32"
                            HeaderText="ID" SortExpression="OrganizationUserID" UniqueName="OrganizationUserID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="FirstName" FilterControlAltText="Filter FirstName column"
                            HeaderText="First Name" SortExpression="FirstName" UniqueName="FirstName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LastName" FilterControlAltText="Filter LastName column"
                            HeaderText="Last Name" SortExpression="LastName" UniqueName="LastName">
                        </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn DataField="Email" FilterControlAltText="Filter Email column"
                            HeaderText="Email" SortExpression="Email" UniqueName="Email" Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TenantName" FilterControlAltText="Filter Institution column"
                            HeaderText="Institution" SortExpression="TenantName" UniqueName="TenantName"
                            Groupable="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="Notification Subscription">
                            <ItemTemplate>
                                <asp:CheckBoxList ID="cblNotification" runat="server" DataTextField="Name" DataValueField="CommunicationEventId">
                                </asp:CheckBoxList>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="Reminder Subscription">
                            <ItemTemplate>
                                <asp:CheckBoxList ID="cblReminder" runat="server" DataTextField="Name" DataValueField="CommunicationEventId">
                                </asp:CheckBoxList>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                        </FormTemplate>
                    </EditFormSettings>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
    </div>
    <infsu:CommandBar ID="fsucCmdBar1" runat="server" DefaultPanel="pnlName1" DisplayButtons="Save"
        ButtonPosition="Center" AutoPostbackButtons="Save" OnSaveClick="btnSubmit_Click" />
</div>
