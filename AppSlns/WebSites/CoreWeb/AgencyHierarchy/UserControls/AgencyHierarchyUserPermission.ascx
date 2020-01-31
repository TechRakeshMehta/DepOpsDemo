<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgencyHierarchyUserPermission.ascx.cs" Inherits="CoreWeb.AgencyHierarchy.Views.AgencyHierarchyUserPermission" %>


<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxManageAgencyHierarchyPackages">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>


<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Permissions</h2>
        </div>
    </div>
    <div class="row">
        <%--<div class="col-md-12">--%>
        <infs:WclGrid runat="server" ID="grdAgencyHirarchyUserPermission" AllowCustomPaging="false" AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false" NonExportingColumns="EditCommandColumn,DeleteColumn"
            OnDeleteCommand="grdAgencyHirarchyUserPermission_DeleteCommand"
            OnNeedDataSource="grdAgencyHirarchyUserPermission_NeedDataSource" OnItemCommand="grdAgencyHirarchyUserPermission_ItemCommand" OnItemDataBound="grdAgencyHirarchyUserPermission_ItemDataBound"
            OnSortCommand="grdAgencyHirarchyUserPermission_SortCommand" EnableLinqExpressions="false" ShowClearFiltersButton="false" EnableAriaSupport="true">
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="AGU_ID, AgencyHierarchyID"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Agency User" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="AGU_Name" HeaderText="Agency User Name" ItemStyle-Width="30%"
                        SortExpression="AGU_Name" UniqueName="AGU_Name" HeaderTooltip="This column displays the Agency User Name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/Resources/Mod/Dashboard/images/CancelGrid.gif" ItemStyle-Width="1%"
                        CommandName="Delete" ConfirmText="Are you sure want to delete this agency user?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn" ItemStyle-Width="1%">
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
                                        <asp:Label ID="lblEHServiceGroup" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Agency User" : "Update Agency User Permissions" %>'
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
                                            <div class='form-group col-md-3' title="Select a package.">
                                                <span class="cptn">Select User</span><span class="reqd">*</span>

                                                <infs:WclComboBox ID="cmbAgencyUser" runat="server" DataTextField="AGU_Name"
                                                    DataValueField="AGU_ID" Filter="Contains"
                                                    AutoPostBack="false" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvPackage" ControlToValidate="cmbAgencyUser"
                                                        InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" ValidationGroup="grpUserPermissionFormSubmit"
                                                        Text="Agency User is required." />
                                                </div>
                                            </div>
                                            <div id="divpermissionTemplate" class='form-group col-md-3' runat="server" title="Select a Template.">
                                                <span class="cptn">Agency Permission Template</span><span class="reqd">*</span>

                                                <infs:WclComboBox ID="cmbAgencyPermissionTemplate" AutoPostBack="true" OnSelectedIndexChanged="cmbAgencyPermissionTemplate_SelectedIndexChanged" CssClass="form-control" Skin="Silk" Width="100%" AutoSkinMode="false" runat="server"></infs:WclComboBox>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvAgencyPermissionTemplate" ControlToValidate="cmbAgencyPermissionTemplate"
                                                    Display="Dynamic" ValidationGroup="grpValdManageAgencyUsers" CssClass="errmsg" Text="Agency Permission Template is required." Enabled="true" />

                                            </div>
                                        </div>
                                        <div class="row">&nbsp;</div>
                                        <div id="dvAgencyUserPermission" runat="server">
                                            <div class="row bgLightGreen">

                                                <div class="form-group col-md-3">
                                                    <span class="cptn">Information to Reflect on Invite</span><span class="reqd">*</span>
                                                    <infs:WclComboBox ID="cmbProfileSharedInformation" CssClass="form-control" runat="server" AutoPostBack="false" DataTextField="AIMD_Name" CheckBoxes="true" EnableCheckAllItemsCheckBox="false" Skin="Silk" AutoSkinMode="false"
                                                        Width="100%" DataValueField="AIMD_ID" EmptyMessage="--Select--" Filter="None" OnClientKeyPressing="openCmbBoxOnTab">
                                                    </infs:WclComboBox>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvProfileSharedInformation" ControlToValidate="cmbProfileSharedInformation"
                                                        Display="Dynamic" ValidationGroup="grpUserPermissionFormSubmit" CssClass="errmsg" Text="Information to Reflect on Invite is required." Enabled="true" />

                                                </div>
                                                <div class="form-group col-md-3">
                                                    <asp:Label ID="lblImmunizationCompliancePermission" runat="server" Text="Immunization/Compliance Information" CssClass="cptn"></asp:Label><span class="reqd">*</span>

                                                    <infs:WclComboBox ID="cmbCompliancePermissions" CssClass="form-control" Skin="Silk" Width="100%" AutoSkinMode="false" runat="server" EmptyMessage="--Select--"></infs:WclComboBox>

                                                    <asp:RequiredFieldValidator runat="server" ID="rfvImmunizationCompliancePermission" ControlToValidate="cmbCompliancePermissions"
                                                        Display="Dynamic" ValidationGroup="grpUserPermissionFormSubmit" CssClass="errmsg" Text="Immunization/Compliance Information is required." Enabled="true" />
                                                </div>
                                                <div class="form-group col-md-3">
                                                    <asp:Label ID="lblBackgroundPermission" runat="server" Text="Background Screening Information" CssClass="cptn"></asp:Label>

                                                    <infs:WclComboBox ID="cmbBackgroundPermission" runat="server" Width="100%" CssClass="form-control marginTop2" AutoPostBack="false" DataTextField="SharedInfoType" CheckBoxes="true" EnableCheckAllItemsCheckBox="false" Skin="Silk" AutoSkinMode="false"
                                                        DataValueField="SharedInfoTypeID" EmptyMessage="--Select--" Filter="None" OnClientKeyPressing="openCmbBoxOnTab">
                                                    </infs:WclComboBox>
                                                </div>
                                            </div>
                                            <div class="row">&nbsp;</div>
                                            <div class="row bgLightGreen">
                                                <div class="form-group col-md-3">
                                                    <asp:Label ID="lblRotationPermission" runat="server" Text=" Agency Orientation Information" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                                                    <infs:WclComboBox ID="cmbRotationPermissions" runat="server" Skin="Silk" AutoSkinMode="false" Width="100%" CssClass="form-control" AutoPostBack="false" EmptyMessage="--Select--">
                                                    </infs:WclComboBox>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvRotationPermission" ControlToValidate="cmbRotationPermissions"
                                                        Display="Dynamic" ValidationGroup="grpUserPermissionFormSubmit" CssClass="errmsg" Text=" Agency Orientation Information is required." Enabled="true" />
                                                </div>
                                                <div class="form-group col-md-3">
                                                    <asp:Label ID="lblMngRotationPckgPermsn" runat="server" Text="Manage Agency Requirements" CssClass="cptn"></asp:Label><span class="reqd">*</span>

                                                    <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="rblMngRotationPckgPermsn">
                                                        <asp:ListItem Value="True" Text="Yes" />
                                                        <asp:ListItem Value="False" Text="No" Selected="True" />
                                                    </asp:RadioButtonList>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvMngRotationPckgPermsn" ControlToValidate="rblMngRotationPckgPermsn"
                                                        Display="Dynamic" ValidationGroup="grpUserPermissionFormSubmit" CssClass="errmsg" Text="Manage Rotation Package is required." Enabled="true" />

                                                </div>
                                                <div class="form-group col-md-3">
                                                    <asp:Label ID="lblMngAgencyUSerPermsn" runat="server" Text="Manage Agency User(s)" CssClass="cptn"></asp:Label><span class="reqd">*</span>

                                                    <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="rblMngAgencyUserPermsn">
                                                        <asp:ListItem Value="True" Text="Yes" />
                                                        <asp:ListItem Value="False" Text="No" Selected="True" />
                                                    </asp:RadioButtonList>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvMngAgencyUSerPermsn" ControlToValidate="rblMngAgencyUserPermsn"
                                                        Display="Dynamic" ValidationGroup="grpUserPermissionFormSubmit" CssClass="errmsg" Text="Manage Agency User(s) is required." Enabled="true" />
                                                </div>
                                                <div class="form-group col-md-3">
                                                    <span class="cptn">Manage Attestation Text</span><span class="reqd">*</span>
                                                    <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="rblAttestationTxtPermsn">
                                                        <asp:ListItem Value="True" Text="Yes" />
                                                        <asp:ListItem Value="False" Text="No" Selected="True" />
                                                    </asp:RadioButtonList>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvAttestationTxtPermsn" ControlToValidate="rblAttestationTxtPermsn"
                                                        Display="Dynamic" ValidationGroup="grpUserPermissionFormSubmit" CssClass="errmsg" Text="Manage Attestation Text is required." Enabled="true" />
                                                </div>
                                            </div>
                                            <div class="row">&nbsp;</div>
                                            <div class="row">
                                                <%--  <div class="form-group col-md-3">
                                                    <span class="cptn">Email Configuration</span><span class="reqd">*</span>
                                                    <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="rblEmailNeedtoSend">
                                                        <asp:ListItem Value="True" Text="Yes" />
                                                        <asp:ListItem Value="False" Text="No" Selected="True" />
                                                    </asp:RadioButtonList>
                                                    <asp:RequiredFieldValidator runat="server" ID="rvfEmailNeedToSent" ControlToValidate="rblEmailNeedtoSend"
                                                        Display="Dynamic" ValidationGroup="grpUserPermissionFormSubmit" CssClass="errmsg" Text="Email Need to Send is required." Enabled="true" />
                                                </div>--%>
                                                <div class="form-group col-md-3">
                                                    <asp:Label ID="lblRequirementPackage" runat="server" Text="View Requirement Package" CssClass="cptn"></asp:Label><span class="reqd">*</span>

                                                    <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="rblRequirementPackage">
                                                        <asp:ListItem Value="True" Text="Yes" />
                                                        <asp:ListItem Value="False" Text="No" Selected="True" />
                                                    </asp:RadioButtonList>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvRequirementPackage" ControlToValidate="rblRequirementPackage"
                                                        Display="Dynamic" ValidationGroup="grpUserPermissionFormSubmit" CssClass="errmsg" Text="View Requirement Package is required." Enabled="true" />
                                                </div>
                                                <div class="form-group col-md-3">
                                                    <span class="cptn">Allow Job Posting</span><span class="reqd">*</span>
                                                    <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="rblAllowJobPosting">
                                                        <asp:ListItem Value="True" Text="Yes" />
                                                        <asp:ListItem Value="False" Text="No" Selected="True" />
                                                    </asp:RadioButtonList>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvAllowJobPosting" ControlToValidate="rblAllowJobPosting"
                                                        Display="Dynamic" ValidationGroup="grpUserPermissionFormSubmit" CssClass="errmsg" Text="Allow job posting is required." Enabled="true" />
                                                </div>
                                                <div class="form-group col-md-3">
                                                    <span class="cptn">SSN Permission</span><span class="reqd">*</span>
                                                    <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="rblSSNpermission">
                                                        <asp:ListItem Value="True" Text="Full SSN" />
                                                        <asp:ListItem Value="False" Text=" Masked" Selected="True" />
                                                    </asp:RadioButtonList>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvSSNpermission" ControlToValidate="rblSSNpermission"
                                                        Display="Dynamic" ValidationGroup="grpUserPermissionFormSubmit" CssClass="errmsg" Text="SSN Permission is required." Enabled="true" />
                                                </div>
                                                <%--   </div>
                                            <div class="row">--%>
                                                <div class="form-group col-md-3">
                                                    <span class="cptn">Do not show Non-Agency Shares</span><span class="reqd">*</span>
                                                    <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="rblDoNotShowNonAgencyShares">
                                                        <asp:ListItem Value="True" Text="Yes" />
                                                        <asp:ListItem Value="False" Text="No" Selected="True" />
                                                    </asp:RadioButtonList>
                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="rblDoNotShowNonAgencyShares"
                                                        Display="Dynamic" ValidationGroup="grpUserPermissionFormSubmit" CssClass="errmsg" Text="Do not show non-agency shares is required." Enabled="true" />
                                                </div>
                                                <div class="form-group col-md-3">
                                                    <span class="cptn">Agency Portal Detail Link Permission</span><span class="reqd">*</span>
                                                    <asp:RadioButtonList runat="server" ToolTip="This setting is used to hide/unhide detail link from requirement share & rotation student screen" RepeatDirection="Horizontal" ID="rblAgencyPortalDetailLink">
                                                        <asp:ListItem Value="True" Text="Hide" />
                                                        <asp:ListItem Value="False" Text=" Unhide" Selected="True" />
                                                    </asp:RadioButtonList>
                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="rblAgencyPortalDetailLink"
                                                        Display="Dynamic" ValidationGroup="grpValdManageAgencyUsers" CssClass="errmsg" Text="Agency Portal Detail Link is required." Enabled="true" />
                                                </div>
                                            </div>
                                            <div class="row">&nbsp;</div>
                                            <div class="row">
                                                <div class="form-group col-md-12">
                                                    <span class="cptn">Notification setting for agency user</span>
                                                    <asp:CheckBoxList Style="height: 55px !important;" ID="chkAgencUserNotifications" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                    </asp:CheckBoxList>
                                                </div>
                                            </div>
                                            <div class="row">&nbsp;</div>
                                            <div class="row">
                                               
                                                    <div class="form-group col-md-5">
                                                        <span class="cptn" title="Check the report that you want to hide for this agency user.">Report Permissions</span> <span style="font-size: small">(Check report to hide)</span>
                                                        <infs:WclComboBox ID="cmbReportPermissions" runat="server" Width="100%" CssClass="form-control marginTop2" AutoPostBack="false" DataTextField="AUR_Name" CheckBoxes="true" EnableCheckAllItemsCheckBox="false" Skin="Silk" AutoSkinMode="false"
                                                            DataValueField="AUR_ID" EmptyMessage="--Select--" Filter="None" OnClientKeyPressing="openCmbBoxOnTab">
                                                        </infs:WclComboBox>
                                                    </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <infsu:CommandBar ID="fsucCmdBarRotation" runat="server" GridMode="true"
                                GridInsertText="Save" GridUpdateText="Save" SaveButtonIconClass="rbSave"
                                ValidationGroup="grpUserPermissionFormSubmit" UseAutoSkinMode="false" ButtonSkin="Silk" />
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
        <%--</div>--%>
    </div>
</div>
