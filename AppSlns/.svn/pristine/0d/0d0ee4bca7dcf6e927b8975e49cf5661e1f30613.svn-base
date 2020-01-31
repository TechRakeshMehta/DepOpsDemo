<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.IntsofSecurityModel.Views.MapUserRole" Codebehind="MapUserRole.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxMapProductFeature">
    <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/MapUserRole.js"
        ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/MapUserRole.js"
        ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblMapUserRole" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblSuffix" runat="server" Text=""></asp:Label>
    </h1>
    <div class="content">
        <telerik:RadCaptcha ID="radCpatchaDefaultPassword" runat="server" CaptchaImage-TextChars="LettersAndNumbers"
            CaptchaImage-TextLength="10" Visible="false" Display="Dynamic" />
        <div class="swrap">
            <asp:Panel runat="server" ID="pnlMapUserRole">
                <infs:WclGrid runat="server" ID="grdUserRole" AllowPaging="True" PageSize="10" AutoGenerateColumns="False"
                    AllowSorting="True" GridLines="Both" OnNeedDataSource="grdUserRole_NeedDataSource" ShowAllExportButtons="false"
                    OnItemDataBound="grdUserRole_ItemDataBound" NonExportingColumns="RoleAssignedColumn" OnItemCommand="grdUserRole_ItemCommand">
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        Pdf-PageWidth="297mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                    </ExportSettings>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="RoleId, RoleName">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true" ShowExportToExcelButton="true" 
                        ShowExportToPdfButton = "true"></CommandItemSettings>
                        <Columns>
                            <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="RoleAssignedColumn">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" OnCheckedChanged="chkAssigned_CheckedChanged" Checked='<%#Bind("RoleAssigned")%>'
                                        ID="chkAssigned" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn UniqueName="RoleName" HeaderText="Role Name" DataField="RoleName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Description" HeaderText="Description">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
                </infs:WclGrid>
                <infsu:CommandBar ID="fsucCmdBar" runat="server" DisplayButtons="Submit,Cancel" TabIndexAt="1"
                    SubmitButtonText="Save Mapping" CancelButtonText="Cancel" DefaultPanel="pnlMapUserRole" DefaultPanelButton="Submit"
                    SubmitButtonIconClass="rbSave" CancelButtonIconClass="rbCancel" OnCancelClick="btnCancel_Click" OnSubmitClick="btnSave_Click" AutoPostbackButtons="Submit,Cancel" />
            </asp:Panel>
        </div>
        <div class="gclr">
        </div>
    </div>
</div>
