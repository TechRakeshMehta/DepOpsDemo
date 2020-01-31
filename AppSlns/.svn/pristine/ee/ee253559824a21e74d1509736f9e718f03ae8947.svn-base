<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TenantUserMapping.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.TenantUserMapping" %>


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
            <h1 class="header-color">Manage Tenant User Mapping</h1>
        </div>
    </div>

    <div id="dvTenantUserMapping" class="row">
        <infs:WclGrid runat="server" ID="grdTenantUserMapping" AllowPaging="true" AutoGenerateColumns="false" CssClass="gridhover"
            AllowSorting="true" AllowFilteringByColumn="true" AutoSkinMode="true" CellSpacing="0" GridLines="Both" EnableDefaultFeatures="true"
            ShowAllExportButtons="false" ShowExtraButtons="true" ValidationGroup="grpTenantUser"
            PageSize="10" NonExportingColumns="DeleteColumn" OnNeedDataSource="grdTenantUserMapping_NeedDataSource" OnItemCommand="grdTenantUserMapping_ItemCommand"
            OnItemDataBound="grdTenantUserMapping_ItemDataBound">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <GroupingSettings CaseSensitive="false" />
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="TUM_ID" AllowFilteringByColumn="true">
                <CommandItemSettings ShowExportToExcelButton="true"
                    ShowExportToPdfButton="true" ShowExportToCsvButton="true" AddNewRecordText="Add New Tenant User Mapping" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>

                    <telerik:GridNumericColumn DataField="TUM_ID" FilterControlAltText="Filter CPO_ID column"
                        HeaderText="CPO_ID" SortExpression="CPO_ID" DataType="System.Int32" UniqueName="CPO_ID" Display="false"
                        DecimalDigits="0" HeaderTooltip="This column displays the compliance priority object id for each record in the grid">
                    </telerik:GridNumericColumn>

                    <telerik:GridBoundColumn DataField="TenantName" FilterControlAltText="Filter TenantName column"
                        HeaderText="Institution" SortExpression="TenantName" UniqueName="TenantName"
                        HeaderTooltip="This column displays the name of institution for each record in the grid">
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn DataField="UserName" FilterControlAltText="Filter UserName column"
                        HeaderText="Name" SortExpression="UserName" UniqueName="UserName"
                        HeaderTooltip="This column displays the name of user for each record in the grid">
                    </telerik:GridBoundColumn>

                  <%--  <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif"
                        UniqueName="EditCommandColumn">
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridEditCommandColumn>--%>
                    <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif"
                        CommandName="Delete" ConfirmText="Are you sure you want to delete this tenant user mapping?"
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
                                        <asp:Label ID="lblTitleTenantUserMapping" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Tenant User Mapping" : "Update Tenant User Mapping" %>'
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
                            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenantUserMappping">
                                <div class="row bgLightGreen">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Institution</span><span class='reqd'>*</span>
                                                <infs:WclComboBox ID="ddlTenant" runat="server" DataTextField="TenantName" AutoPostBack="true"
                                                    DataValueField="TenantID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                                    Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenant"
                                                        InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpTenantUser" CssClass="errmsg"
                                                        Text="Institution is required." />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Admin</span><span class='reqd'>*</span>
                                                <infs:WclComboBox ID="ddlUser" runat="server" DataTextField="FirstName" AutoPostBack="false" CheckBoxes="true"
                                                    DataValueField="OrganizationUserID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" EmptyMessage="--SELECT--"
                                                    Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                    <Localization CheckAllString="All" />
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvUser" ControlToValidate="ddlUser"
                                                         Display="Dynamic" ValidationGroup="grpTenantUser" CssClass="errmsg"
                                                        Text="Admin user is required." />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div class="col-md-12 text-right">
                                <infsu:CommandBar ID="fsucCmdBarNode" runat="server" GridMode="true" GridInsertText="Save" GridUpdateText="Save"
                                    ValidationGroup="grpTenantUser" ExtraButtonIconClass="icnreset" UseAutoSkinMode="false" ButtonSkin="Silk" />
                            </div>
                        </div>
                    </FormTemplate>
                </EditFormSettings>

                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" Position="TopAndBottom" />
            </MasterTableView>
        </infs:WclGrid>
    </div>

</div>
