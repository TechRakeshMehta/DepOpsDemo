<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Messaging.Views.AddressLookup"
    Title="Address Book" MasterPageFile="~/Shared/PopupMaster.master" Codebehind="AddressLookup.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">
    <infs:WclResourceManagerProxy ID="userListManager" runat="server">
        <infs:LinkedResource Path="~/Resources/Mod/Messaging/MessagingUser.js" ResourceType="JavaScript" />
       
    </infs:WclResourceManagerProxy>
    <div>
        <div class="section" runat="server" id="divUsers">
            <div class="content" style="overflow-x: scroll">
                <div class="swrap">
                    <infs:WclGrid runat="server" ID="grdUsers" AutoGenerateColumns="False" GroupingSettings-CaseSensitive="false" EnableAriaSupport="true"
                        AllowPaging="true" AllowSorting="True" AllowFilteringByColumn="True" OnNeedDataSource="grdUsers_NeedDataSource"
                        AutoSkinMode="True" CellSpacing="0" EnableDefaultFeatures="true" ShowAllExportButtons="false"
                        ShowExtraButtons="true  " GroupingEnabled="true" AllowMultiRowSelection="true" 
                        GridLines="Both" ShowGroupPanel="false" ShowHeader="true" OnPreRender="grdUsers_PreRender"
                        AllowCustomPaging="true" OnInit="grdUsers_Init" OnItemCommand="grdUsers_ItemCommand"
                        OnSortCommand="grdUsers_SortCommand" >
                        <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                            Pdf-PageWidth="297mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                        </ExportSettings>
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true"></Selecting>
                            <ClientEvents OnRowSelected="grdUsers_RowSelected" OnRowDeselected="grdUsers_RowDeselected" />
                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top" GroupLoadMode="Client" ClientDataKeyNames="OrganizationUserID,FirstName,LastName"
                            DataKeyNames="OrganizationUserID,FirstName">
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
                                <telerik:GridBoundColumn DataField="aspnet_Users.aspnet_Membership.Email" FilterControlAltText="Filter aspnet_Users.aspnet_Membership.Email column"
                                    HeaderText="Email" SortExpression="aspnet_Users.aspnet_Membership.Email" UniqueName="aspnet_Users.aspnet_Membership.Email"
                                    Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Organization.Tenant.TenantName" FilterControlAltText="Filter Organization.Tenant.TenantName column"
                                    HeaderText="School" SortExpression="Organization.Tenant.TenantName" UniqueName="Organization.Tenant.TenantName"
                                    Groupable="true">
                                </telerik:GridBoundColumn>
                                <%--<telerik:GridBoundColumn DataField="Organization.OrganizationName" FilterControlAltText="Filter Organization.OrganizationName column"
                                    HeaderText="Department Name" SortExpression="Organization.OrganizationName" UniqueName="Organization.OrganizationName" Groupable="true">
                                </telerik:GridBoundColumn>--%>
                                <%--<telerik:GridBoundColumn DataField="Program" FilterControlAltText="Filter Program column"
                                    HeaderText="Program" SortExpression="Program" UniqueName="Program" Groupable="true">
                                </telerik:GridBoundColumn>--%>
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
                        <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                    </infs:WclGrid>
                </div>
                <div class="gclr">
                </div>
            </div>
        </div>
        <div class="section" runat="server" id="divMsgGroups">
            <div class="content" style="overflow-x: scroll">
                <div class="swrap">
                    <infs:WclGrid runat="server" ID="grdMessagingGroups" AutoGenerateColumns="False" EnableAriaSupport="true"
                        GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowSorting="True"
                        AllowFilteringByColumn="True" OnNeedDataSource="grdMessagingGroups_NeedDataSource"
                        AutoSkinMode="True" CellSpacing="0" EnableDefaultFeatures="true" ShowAllExportButtons="True"
                        ShowExtraButtons="true" GroupingEnabled="true" AllowMultiRowSelection="true" ShowClearFiltersButton="true"
                        ShowGroupPanel="false" ShowHeader="true" AllowCustomPaging="true" OnInit="grdMessagingGroups_Init"
                        OnItemCommand="grdMessagingGroups_ItemCommand" OnSortCommand="grdMessagingGroups_SortCommand"  >
                        <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                        </ExportSettings>
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true"></Selecting>
                            <ClientEvents OnRowSelected="grdMessagingGroups_RowSelected" OnRowDeselected="grdMessagingGroups_RowDeselected" />
                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top" GroupLoadMode="Client" ClientDataKeyNames="ID,Name,Type"
                            DataKeyNames="ID,Name,Type">
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="true" />
                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column"
                                    HeaderText="ID" SortExpression="ID" UniqueName="ID">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Type" FilterControlAltText="Filter Type column"
                                    HeaderText="Type" SortExpression="Type" UniqueName="Type">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter Name column"
                                    HeaderText="Name" SortExpression="Name" UniqueName="Name">
                                </telerik:GridBoundColumn>
                            </Columns>
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
        </div>
    </div>
    <div style="padding: 8px; background-color: #efefef;">
        <table width="100%">
            <tr>
                <td>
                    <infs:WclButton runat="server" ID="btnToUsers" Text="To --&gt;" Width="95%" AutoPostBack="false"
                        ClientKey="toButton">
                        <%-- <Icon PrimaryIconCssClass="" /> --%>
                    </infs:WclButton>
                </td>
                <td>
                    <infs:WclAutoCompleteBox runat="server" AllowCustomEntry="false" Width="410px" ID="acbToList" ClientIDMode="Static"
                        InputType="Token">
                        <TokensSettings AllowTokenEditing="true" />
                    </infs:WclAutoCompleteBox>
                </td>
            </tr>
            <tr>
                <td>
                    <infs:WclButton runat="server" ID="btnCcUsers" Text="Cc --&gt;" Width="95%" AutoPostBack="false"
                        ClientKey="ccButton">
                        <%-- <Icon PrimaryIconCssClass="" /> --%>
                    </infs:WclButton>
                </td>
                <td>
                    <infs:WclAutoCompleteBox runat="server" AllowCustomEntry="false" Width="410px" ID="acbCcList" ClientIDMode="Static"
                        InputType="Token">
                        <TokensSettings AllowTokenEditing="true" />
                    </infs:WclAutoCompleteBox>
                    <asp:HiddenField ID="hdnParentScreen" runat="server" />
                </td>
            </tr>
            <tr id="dvBccList" runat="server">
                <td>
                    <infs:WclButton runat="server" ID="btnBccUsers" Text="Bcc --&gt;" Width="95%" AutoPostBack="false"
                        ClientKey="bccButton">
                        <%-- <Icon PrimaryIconCssClass="" /> --%>
                    </infs:WclButton>
                </td>
                <td>
                    <infs:WclAutoCompleteBox runat="server" AllowCustomEntry="false" Width="410px" ID="acbBccList" ClientIDMode="Static"
                        InputType="Token">
                        <TokensSettings AllowTokenEditing="true" />
                    </infs:WclAutoCompleteBox>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CommandContent" runat="server">
    <infsu:CommandBar ID="fsucCmdBar1" runat="server" DisplayButtons="Save,Cancel" OnCancelClientClick="closeWindow" 
        OnSaveClientClick="closeBindUsers" DefaultPanel="pnlName1" SaveButtonText="OK" />
    <script type="text/javascript">

        $jQuery(document).ready(function () {
            $jQuery("[id$=grdUsers]").focus();
            disableKeyPress("acbToList");
            disableKeyPress("acbCcList");
        });

        function disableKeyPress(control) {
            $jQuery("[id$=" + control + "]").on('keydown', function (e) {
                var key = e.charCode || e.keyCode;
                if (key != 9) {
                    e.preventDefault();
                }
            });
        }
    </script>
</asp:Content>
