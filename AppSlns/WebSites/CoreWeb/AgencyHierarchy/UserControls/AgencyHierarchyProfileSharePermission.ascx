<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgencyHierarchyProfileSharePermission.ascx.cs" Inherits="CoreWeb.AgencyHierarchy.UserControls.AgencyHierarchyProfileSharePermission" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rmpHierarchyControls">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~//Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Scripts/bootstrap.min.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="container-fluid" tabindex="-1" id="dvAgencyHierarchyProfileSharePermission" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Agency Hierarchy Profile Share Permission</h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <infs:WclGrid ID="grdAgencyHierarProfileSharePerm" runat="server" AllowCustomPaging="false" AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
                AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false" NonExportingColumns="EditCommandColumn,DeleteColumn"
                OnDeleteCommand="grdAgencyHierarProfileSharePerm_DeleteCommand" OnNeedDataSource="grdAgencyHierarProfileSharePerm_NeedDataSource"
                OnItemCommand="grdAgencyHierarProfileSharePerm_ItemCommand" OnItemDataBound="grdAgencyHierarProfileSharePerm_ItemDataBound"
                EnableLinqExpressions="false" ShowClearFiltersButton="false" EnableAriaSupport="true">
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="TenantID,AgencyHierarchyAgencyProfileSharePermissionsID"
                    AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add Profile Share Permission" ShowExportToCsvButton="true"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                    <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="TenantName" HeaderText="School Name" SortExpression="TenantName"
                            UniqueName="TenantName" HeaderTooltip="This column displays the School Name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IsStudentShare" HeaderText="Is Student Share" SortExpression="IsStudentShare"
                            UniqueName="IsStudentShare" HeaderTooltip="This column displays the Institution Node for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IsAdminShare" HeaderText="Is Admin Share" SortExpression="IsAdminShare"
                            UniqueName="IsAdminShare" HeaderTooltip="This column displays the Institution Node for each record in the grid">
                        </telerik:GridBoundColumn>
                         <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/Resources/Mod/Dashboard/images/CancelGrid.gif" ItemStyle-Width="1%"
                            CommandName="Delete" ConfirmText="Are you sure you want to delete this profile share permission?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn" ItemStyle-Width="2%">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                       
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-12">
                                        <h2 class="header-color">
                                            <asp:Label ID="lblEHServiceGroup" Text='<%# (Container is GridEditFormInsertItem) ? "Add Agency Hierarchy Profile Share Permission" : "Update Agency Hierarchy Profile Share Permission" %>'
                                                runat="server" />
                                        </h2>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="msgbox">
                                            <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <asp:Panel ID="pnlEditForm" runat="server">
                                    <div class="row bgLightGreen">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3' title="Select an Institution.">
                                                    <span class="cptn">Institution</span><span class="reqd">*</span>
                                                    <infs:WclComboBox ID="ddlTenant" runat="server" DataTextField="TenantName" AutoPostBack="false"
                                                        DataValueField="TenantID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                                        Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenant"
                                                            InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFormSubmitPSA" CssClass="errmsg"
                                                            Text="Institution is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3'>
                                                    <div class="col-md-12">
                                                        <span class="cptn">Allow Profile Share</span>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:CheckBox ID="chkStudentProfileSharingPermission" Text="Student" runat="server" Width="100%" CssClass="form-control" Checked='<%# (Container is GridEditFormInsertItem) ? false: Convert.ToBoolean(Eval("IsStudentShare"))%>' />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:CheckBox ID="chkAdminProfileSharingPermission" Text="Admin" runat="server" Checked='<%#(Container is GridEditFormInsertItem) ? false: Convert.ToBoolean(Eval("IsAdminShare"))%>'
                                                                Width="100%" CssClass="form-control" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>

                                <infsu:CommandBar ID="fsucCmdBarAssociation" runat="server" GridMode="true"
                                    GridInsertText="Save" GridUpdateText="Save" SaveButtonIconClass="rbSave"
                                    ValidationGroup="grpFormSubmitPSA" UseAutoSkinMode="false" ButtonSkin="Silk" />
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
    </div>
</div>
<asp:Label runat="server" TabIndex="-1" ID="lblFocus"></asp:Label>
