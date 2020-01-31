<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Permissions.ascx.cs" Inherits="CoreWeb.ComplianceAdministration.UserControl.Views.Permissions" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<script type="text/javascript">
    function ValidatePermission(sender, args) {
        var permissionOptions = $jQuery("[id$=rblPermissions] input:checked").is(":checked");
        if (permissionOptions) {
            args.IsValid = true;
        }
        else {
            args.IsValid = false;
        }
    }
</script>

<div class="swrap">
    <infs:WclGrid runat="server" ID="grdUsrPermission" AllowPaging="True" PageSize="10"
        AutoGenerateColumns="False" AllowSorting="True" GridLines="Both" OnNeedDataSource="grdUsrPermission_NeedDataSource"
        OnItemCreated="grdUsrPermission_ItemCreated" OnInsertCommand="grdUsrPermission_InsertCommand"
        OnUpdateCommand="grdUsrPermission_UpdateCommand" OnDeleteCommand="grdUsrPermission_DeleteCommand"
        ShowAllExportButtons="False" NonExportingColumns="EditCommandColumn, DeleteColumn">
        <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
            Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
        </ExportSettings>
        <ClientSettings EnableRowHoverStyle="true">
            <Selecting AllowRowSelect="true"></Selecting>
        </ClientSettings>
        <MasterTableView CommandItemDisplay="Top" DataKeyNames="HierarchyPermissionID">
            <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Map User Permissions"
                ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"></CommandItemSettings>
            <Columns>
                <telerik:GridBoundColumn DataField="UserFirstName" FilterControlAltText="Filter UserFirstName column"
                    HeaderText="First Name" SortExpression="UserFirstName" UniqueName="UserFirstName"
                    HeaderStyle-Width="130">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="UserLastName" FilterControlAltText="Filter UserLastName column"
                    HeaderText="Last Name" SortExpression="UserLastName" UniqueName="UserLastName"
                    HeaderStyle-Width="130">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="UserName" FilterControlAltText="Filter UserName column"
                    HeaderText="User Name" HeaderStyle-Width="50px" SortExpression="UserName" UniqueName="UserName">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PermissionName" FilterControlAltText="Filter PermissionName column"
                    HeaderText="Permission" SortExpression="PermissionName" UniqueName="PermissionName"
                    HeaderStyle-Width="130">
                </telerik:GridBoundColumn>
                <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                    <HeaderStyle Width="30px" />
                </telerik:GridEditCommandColumn>
                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete?"
                    Text="Delete" UniqueName="DeleteColumn">
                    <HeaderStyle Width="30px" />
                </telerik:GridButtonColumn>
            </Columns>
            <EditFormSettings EditFormType="Template">
                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                </EditColumn>
                <FormTemplate>
                    <div class="section">
                        <h1 class="mhdr">
                            <asp:Label ID="lblHeading" Text='<%#(Container is GridEditFormInsertItem) ? "Map User Permissions" : "Edit User Permission"%>'
                                runat="server"></asp:Label></h1>
                        <div class="content">
                            <div class="sxform auto">
                                <div class="msgbox">
                                    <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                                </div>
                                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlMUser">
                                    <div class='sxro sx2co'>
                                        <div class='sxlb'>
                                            <asp:Label ID="lblUserName" runat="server" AssociatedControlID="ddlHierPerUser" Text="Select User" CssClass="cptn">                                                        
                                            </asp:Label><span class='reqd <%# (Container is GridEditFormInsertItem) ? "" : "nodisp" %>'>*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclComboBox ID="ddlHierPerUser" runat="server" AutoPostBack="true" DataTextField="FirstName"
                                                MaxHeight="200px" DataValueField="OrganizationUserID" OnSelectedIndexChanged="ddlHierPerUser_SelectedIndexChanged">
                                            </infs:WclComboBox>
                                            <div class='vldx'>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvHierPerUser" ControlToValidate="ddlHierPerUser"
                                                    class="errmsg" ValidationGroup="grpValdUsrPermission" Display="Dynamic" ErrorMessage="User is required."
                                                    InitialValue="--SELECT--" />
                                            </div>
                                            <infs:WclTextBox Enabled="false" runat="server" Visible="false" ID="txtHierUser">
                                            </infs:WclTextBox>
                                        </div>
                                        <div class='sxlb'>
                                            <asp:Label ID="Label2" runat="server" AssociatedControlID="ddlHierPerUser" Text="Permissions" CssClass="cptn"></asp:Label><span
                                                class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <asp:RadioButtonList ID="rblPermissions" runat="server" RepeatDirection="Horizontal"
                                                DataTextField="PER_Description" DataValueField="PER_ID">
                                            </asp:RadioButtonList>
                                            <div class='vldx'>
                                                <asp:CustomValidator ID="cvPermission" CssClass="errmsg" Display="Dynamic" runat="server"
                                                    EnableClientScript="true" ErrorMessage="Permission is required." ValidationGroup="grpValdUsrPermission"
                                                    ClientValidationFunction="ValidatePermission">
                                                </asp:CustomValidator>
                                            </div>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx2co'>
                                        <div class='sxlb'>
                                            <span class="cptn">Apply Permission on Compliance Also</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclButton runat="server" ID="chkApplyOnBoth" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="true" Checked="true" OnCheckedChanged="chkApplyOnBoth_CheckedChanged">
                                                <ToggleStates>
                                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                                </ToggleStates>
                                            </infs:WclButton>
                                        </div>
                                        <div runat="server" id="divOrderPermission" visible="false">
                                            <div class='sxlb'>
                                                <asp:Label ID="lblOrderPermission" runat="server" Text="Order Queue Permissions" CssClass="cptn"></asp:Label><span
                                                    class="reqd">*</span>
                                            </div>
                                            <div class='sxlm'>
                                                <asp:RadioButtonList ID="rblOrderPermission" runat="server" RepeatDirection="Horizontal"
                                                    DataTextField="PER_Description" DataValueField="PER_ID">
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx2co' runat="server" id="divOtherPermissions" visible="false">
                                        <div class='sxlb'>
                                            <asp:Label ID="lblVerificationPermission" runat="server" Text="Verification Permissions" CssClass="cptn"></asp:Label><span
                                                class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <asp:RadioButtonList ID="rblVerificationPermission" runat="server" RepeatDirection="Horizontal"
                                                DataTextField="PER_Description" DataValueField="PER_ID">
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class='sxlb'>
                                            <asp:Label ID="lblProfilePermission" runat="server" Text="Profile Permissions" CssClass="cptn"></asp:Label><span
                                                class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <asp:RadioButtonList ID="rblProfilePermission" runat="server" RepeatDirection="Horizontal"
                                                DataTextField="PER_Description" DataValueField="PER_ID">
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class="sxro sx2co" runat="server" id="dvPackagePermission" visible="false">
                                        <div class="sxlb">
                                            <asp:Label ID="Label1" runat="server" Text="Package Permissions" CssClass="cptn"></asp:Label>
                                            <span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <asp:RadioButtonList ID="rblPackagePermission" runat="server" RepeatDirection="Horizontal"
                                                DataTextField="PER_Description" DataValueField="PER_ID">
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <infsu:CommandBar ID="fsucCmdBarMUser" runat="server" GridMode="true" DefaultPanel="pnlMUser" GridInsertText="Save" GridUpdateText="Save"
                                ExtraButtonIconClass="icnreset" ValidationGroup="grpValdUsrPermission" />
                            <%-- <infsu:CommandBar ID="fsucCmdBarMUser" runat="server" DefaultPanel="pnlMUser" TabIndexAt="8">
                                            <ExtraCommandButtons>
                                                <infs:WclButton runat="server" ID="btnSaveForm" ValidationGroup="grpValdUsrPermission"
                                                    Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>' CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'>
                                                    <Icon PrimaryIconCssClass="rbSave" />
                                                </infs:WclButton>
                                                <infs:WclButton runat="server" ID="btnCancelForm" Text="Cancel" CommandName="Cancel">
                                                    <Icon PrimaryIconCssClass="rbCancel" />
                                                </infs:WclButton>
                                            </ExtraCommandButtons>
                                        </infsu:CommandBar>--%>
                        </div>
                    </div>
                </FormTemplate>
            </EditFormSettings>
            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
        </MasterTableView>
    </infs:WclGrid>
</div>
<div class="gclr">
</div>

