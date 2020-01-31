<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.IntsofSecurityModel.Views.ManageUsers" CodeBehind="ManageUsers.ascx.cs"
    ValidateRequestMode="Enabled" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>
<%@ Import Namespace="INTSOF.Utils" %>


<%--<infs:WclResourceManagerProxy runat="server" ID="rprxManageUsers">
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/CommonOperations/AccountLinking.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>--%>

<style type="text/css">
    .userName100 {
        float: left;
        width: 100%;
    }

    .userName85 {
        float: left;
        width: 85%;
    }

    .userNamePrefix15 {
        float: left;
        width: 15%;
    }

    .myControl .RadInput {
        width: 85% !important;
    }

    .myControl input[type="checkbox"] {
        margin-top: 4px;
        position: absolute;
        right: 10px;
        top: 2px;
    }

    .myControl {
        position: relative;
    }

    .buttonHidden {
        display: none;
    }
</style>
<div class="msgbox" id="divSuccessMsg">
    <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
</div>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblManageUser" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblManageTenantSuffix" runat="server" Text=""></asp:Label>
    </h1>
    <div class="content">
        <telerik:RadCaptcha ID="radCpatchaPassword" runat="server" CaptchaImage-TextChars="LettersAndNumbers"
            CaptchaImage-TextLength="10" Visible="false" Display="Dynamic" />
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grmUsrList" AllowPaging="True" PageSize="10" AutoGenerateColumns="False"
                AllowSorting="True" GridLines="Both" OnInsertCommand="grmUsrList_InsertCommand" ShowAllExportButtons="False"
                OnNeedDataSource="grmUsrList_NeedDataSource" OnUpdateCommand="grmUsrList_UpdateCommand"
                OnDeleteCommand="grmUsrList_DeleteCommand" OnItemCreated="grmUsrList_ItemCreated"
                OnItemDataBound="grmUsrList_ItemDataBound" OnItemCommand="grmUsrList_ItemCommand"
                AllowCustomPaging="true" OnSortCommand="grmUsrList_SortCommand"
                NonExportingColumns="ManageRoles, EditCommandColumn, DeleteColumn" ValidationGroup="grpValdManageUser">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrganizationUserID,UserID,UserName">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New User" ShowExportToExcelButton="true"
                        ShowExportToPdfButton="true" ShowExportToCsvButton="true"></CommandItemSettings>
                    <Columns>
                        <telerik:GridBoundColumn DataField="FirstName" FilterControlAltText="Filter FirstName column"
                            HeaderText="First Name" SortExpression="FirstName" UniqueName="FirstName" HeaderStyle-Width="130">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LastName" FilterControlAltText="Filter LastName column"
                            HeaderText="Last Name" SortExpression="LastName" UniqueName="LastName" HeaderStyle-Width="130">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserName" FilterControlAltText="Filter User Name column"
                            HeaderText="User Id/Email" SortExpression="UserName" UniqueName="UserName"
                            HeaderStyle-Width="130">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MobileAlias" FilterControlAltText="Filter Mobile column"
                            HeaderText="Phone" SortExpression="MobileAlias" UniqueName="MobileAlias" FilterControlWidth="105px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LastActivityDate" FilterControlAltText="Filter Activity column"
                            HeaderText="Last Activity" SortExpression="LastActivityDate" UniqueName="Activity">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="OrganizationName" FilterControlAltText="Filter Organization column"
                            HeaderText="Organization" SortExpression="OrganizationName"
                            UniqueName="OrganizationName" HeaderStyle-Width="160">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CreatedByUserName" HeaderText="Created By" FilterControlAltText="Filter Created By column"
                            SortExpression="CreatedByUserName" UniqueName="CreatedByUserName" HeaderStyle-Width="130">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IsLockedOut" FilterControlAltText="Filter IsLockedOut column"
                            HeaderText="Locked" SortExpression="IsLockedOut" UniqueName="IsLockedOut" HeaderStyle-Width="130">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IsActive" FilterControlAltText="Filter Active column"
                            HeaderText="Active" HeaderStyle-Width="50px" SortExpression="IsActive" UniqueName="IsActive">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn UniqueName="ManageRoles" AllowFiltering="false">
                            <ItemTemplate>
                                <a runat="server" id="ancRole">Manage Role</a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageRolesWiz" ItemStyle-Wrap="false"
                            Visible="false">
                            <ItemTemplate>
                                <infs:WclButton ID="btnManageRoles" ButtonType="LinkButton" Text="Manage Roles"
                                    BorderStyle="None" Font-Underline="true" CommandName="ManageRoles" runat="server"
                                    CommandArgument='<%# Eval("UserId")  %>'>
                                </infs:WclButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Wrap="false" UniqueName="ManageQueue">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlManageQueue" Text="Manage Queue" NavigateUrl="#" Visible="false" runat="server"></asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn UniqueName="MapInstitution" AllowFiltering="false">
                            <ItemTemplate>
                                <a runat="server" id="ancInstitution">Map Institutions</a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                            <HeaderStyle Width="30px" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this User?"
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
                                    <asp:Label ID="lblHeading" Text='<%#(Container is GridEditFormInsertItem) ? "Add User" : "Edit User"%>'
                                        runat="server"></asp:Label></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlMUser">
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="txtFirstName" Text="First Name" CssClass="cptn"></asp:Label><span
                                                        class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox ID="txtFirstName" MaxLength="50" TabIndex="1" Text='<%# Bind("FirstName")%>'
                                                        runat="server" Width="98%">
                                                    </infs:WclTextBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvFirstName" ControlToValidate="txtFirstName"
                                                            ValidationGroup="grpValdManageUser" Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_FIRST_NAME_REQUIRED)%>' />
                                                        <asp:RegularExpressionValidator runat="server" ID="revFirstName" ControlToValidate="txtFirstName"
                                                            Display="Dynamic" ValidationGroup="grpValdManageUser" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.]{1,50}$"
                                                            ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTER)%>' />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblLastName" runat="server" AssociatedControlID="txtLastName" Text="Last Name" CssClass="cptn"></asp:Label><span
                                                        class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox ID="txtLastName" MaxLength="50" TabIndex="2" Text='<%# Bind("LastName")%>'
                                                        runat="server" Width="98%">
                                                    </infs:WclTextBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                                                            ValidationGroup="grpValdManageUser" Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_LAST_NAME_REQUIRED)%>' />
                                                        <asp:RegularExpressionValidator runat="server" ID="revLastName" ValidationGroup="grpValdManageUser"
                                                            ControlToValidate="txtLastName" Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.]{1,50}$"
                                                            ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTER)%>' />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <asp:Label runat="server" ID="lblOrgTitle" AssociatedControlID="cmbOrganization" CssClass="cptn"
                                                        Text="Organization"></asp:Label><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbOrganization" TabIndex="3" Width="98%" MarkFirstMatch="true"
                                                        runat="server" DataTextField="OrganizationName" DataValueField="OrganizationId"
                                                        Style="z-index: 7002;" AutoPostBack="true" OnSelectedIndexChanged="cmbOrganization_OnSelectedIndexChanged">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvOrganization" ControlToValidate="cmbOrganization"
                                                            ValidationGroup="grpValdManageUser" InitialValue="--SELECT--" Display="Dynamic"
                                                            CssClass="errmsg" />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblMobile" runat="server" AssociatedControlID="maskTxtMobile" Text="Phone" CssClass="cptn"></asp:Label><span
                                                        class="reqd">*</span>
                                                </div>
                                                <div class='sxlm myControl'>
                                                    <div id="dvMasking" runat="server">
                                                        <infs:WclMaskedTextBox ID="maskTxtMobile" Width="90%" TabIndex="4" runat="server"
                                                            Mask="(###)-###-####"
                                                            Text='<%# (Container is GridEditFormInsertItem) ? null : Eval("MobileAlias")%>'>
                                                        </infs:WclMaskedTextBox>
                                                    </div>
                                                    <div id="dvUnmasking" runat="server">
                                                        <infs:WclTextBox ID="unmaskTxtMobile" Width="90%" TabIndex="4" runat="server" MaxLength="15"
                                                            Text='<%# (Container is GridEditFormInsertItem) ? null : Eval("MobileAlias")%>'>
                                                        </infs:WclTextBox>
                                                    </div>
                                                    <infs:WclCheckBox ID="chkIsMaskingRequired" runat="server"
                                                        AutoPostBack="true" OnCheckedChanged="chkIsMaskingRequired_CheckedChanged"
                                                        Width="10%" Checked='<%# (Container is GridEditFormInsertItem) ? false : Eval("IsInternationalPhoneNumber")%>' />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator Display="Dynamic" ID="rfvTxtMobile" runat="server" CssClass="errmsg"
                                                            ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.PHONE_REQUIRED)%>' ControlToValidate="maskTxtMobile"
                                                            ValidationGroup="grpValdManageUser"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtMobile" runat="server"
                                                            CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.PHONE_INVALID)%>'
                                                            ControlToValidate="maskTxtMobile" ValidationExpression="\(\d{3}\)-\d{3}-\d{4}"
                                                            ValidationGroup="grpValdManageUser" />
                                                        <asp:RequiredFieldValidator Display="Dynamic" ID="rfvTxtMobilePrmyNonMasking" runat="server" CssClass="errmsg" ValidationGroup="grpValdManageUser"
                                                            ErrorMessage="Phone is required." ControlToValidate="unmaskTxtMobile"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtMobilePrmyNonMasking" runat="server" ValidationGroup="grpValdManageUser"
                                                            CssClass="errmsg" ErrorMessage="Invalid phone number. This field only contains +, - and numbers." ControlToValidate="unmaskTxtMobile"
                                                            ValidationExpression="(\d?)+(([-+]+?\d+)?)+([-+]?)+" />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx2co' id="divUserMain" runat="server" visible='False'>
                                                <div id="divUserNameSection" visible="false" runat="server">
                                                    <div class='sxlb'>
                                                        <asp:Label ID="lblUserName" runat="server" AssociatedControlID="txtUserName" Text="Username" CssClass="cptn"></asp:Label><span
                                                            class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <div class="userNamePrefix15" visible='<%# !(Container is GridEditFormInsertItem)%>'>
                                                            <infs:WclComboBox TabIndex="5" runat="server" ID="cmbUsernamePrefix" Visible='<%# (Container is GridEditFormInsertItem)%>'
                                                                DataTextField="UserNamePrefix" DataValueField="OrganizationUserNamePrefixID" MarkFirstMatch="true" Style="z-index: 7002;">
                                                                <%-- <Items>
                                                                    <telerik:RadComboBoxItem runat="server" Text="None" Value="0" Selected="true" />
                                                                    <telerik:RadComboBoxItem runat="server" Text="lc" Value="lc" />
                                                                    <telerik:RadComboBoxItem runat="server" Text="llc" Value="llc" />
                                                                    <telerik:RadComboBoxItem runat="server" Text="mvc" Value="vbs" />
                                                                    <telerik:RadComboBoxItem runat="server" Text="wc" Value="wc" />
                                                                </Items>--%>
                                                            </infs:WclComboBox>
                                                        </div>
                                                        <div class='<%# (Container is GridEditFormInsertItem) ? "userName85" : "userName100" %>'>
                                                            <infs:WclTextBox TabIndex="6" ID="txtUserName" Text='<%# (Container is GridEditFormInsertItem) ? null : Eval("UserName")%>'
                                                                runat="server" Width="98%"
                                                                MaxLength="240">
                                                            </infs:WclTextBox>
                                                        </div>
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvUserName" ControlToValidate="txtUserName"
                                                                ValidationGroup="grpValdManageUser" Display="Dynamic" CssClass="errmsg" ErrorMessage='Username is required.' />
                                                            <asp:RegularExpressionValidator runat="server" ID="revUserName" ControlToValidate="txtUserName"
                                                                Display="Dynamic" ValidationGroup="grpValdManageUser" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\@\+]{1,50}$"
                                                                ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTER)%>' />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="divEmailSection" visible="false" runat="server">
                                                    <div class='sxlb'>
                                                        <asp:Label ID="lblEmailAddress" runat="server" AssociatedControlID="txtEmailAddress"
                                                            Text="Email Address" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <%--TODO--%>
                                                        <infs:WclTextBox TabIndex="7" ID="txtEmailAddress" runat="server" Width="98%" Text='<%# (Container is GridEditFormInsertItem) ? null : Eval("Email")%>'
                                                            MaxLength="240">
                                                        </infs:WclTextBox>
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvEmailAddress" ControlToValidate="txtEmailAddress"
                                                                ValidationGroup="grpValdManageUser" Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_EMAIL_ADDRESS_REQUIRED)%>' />
                                                            <asp:RegularExpressionValidator runat="server" ID="revEmailAddress" ValidationGroup="grpValdManageUser"
                                                                ControlToValidate="txtEmailAddress" Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_EMAIL_ADDRESS_INVALID)%>'
                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx2co' id="divPasswordSection" visible="false" runat="server">
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword"
                                                        Text="Password" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox TabIndex="8" runat="server" ID="txtPassword" TextMode="Password" MaxLength="15"
                                                        autocomplete="off">
                                                    </infs:WclTextBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvPassword" ControlToValidate="txtPassword"
                                                            ValidationGroup="grpValdManageUser" Display="Dynamic" ErrorMessage="Password is required." CssClass="errmsg" />
                                                        <asp:RegularExpressionValidator runat="server" ID="revPassword" ControlToValidate="txtPassword"
                                                            ValidationGroup="grpValdManageUser" CssClass="errmsg"
                                                            Display="Dynamic" ValidationExpression="(?=^.{8,15}$)^(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^_+~!?\\\/\'\:\,\(\)\{\}\[\]\-])[a-zA-Z0-9@#$%^_+~!?\\\/\'\:\,\(\)\{\}\[\]\-]{8,}$" />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblConfirmPassword" runat="server" AssociatedControlID="txtConfirmPassword"
                                                        Text="Confirm Password" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox TabIndex="9" runat="server" ID="txtConfirmPassword" TextMode="Password" MaxLength="15"
                                                        autocomplete="off">
                                                    </infs:WclTextBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvConfirmPassword" ControlToValidate="txtConfirmPassword"
                                                            ValidationGroup="grpValdManageUser" Display="Dynamic" ErrorMessage="Confirm Password is required." CssClass="errmsg" />
                                                        <asp:CompareValidator ID="cmpvalCmpPassword" runat="server" CssClass="errmsg" ControlToCompare="txtPassword"
                                                            ControlToValidate="txtConfirmPassword" ValidationGroup="grpValdManageUser" Display="Dynamic" ErrorMessage="Password did not match."></asp:CompareValidator>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx2co'>
                                                <div id="divChangePasswordUponFirstLogin" visible="false" runat="server">
                                                    <div class='sxlb' title="Select a radio button to restrict the admin to either change the password upon first login or not">
                                                        <asp:Label ID="lblChangePasswordUponFirstLogin" runat="server" AssociatedControlID="rblChangePasswordUponFirstLogin"
                                                            Text="Change Password upon First Login" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <asp:RadioButtonList TabIndex="10" ID="rblChangePasswordUponFirstLogin" runat="server" RepeatDirection="Horizontal"
                                                            CssClass="radio_list" RepeatLayout="Flow">
                                                            <asp:ListItem Text="Yes" Value="true" />
                                                            <asp:ListItem Text="No" Value="false" />
                                                        </asp:RadioButtonList>
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator ValidationGroup="grpValdManageUser" ID="rfvChangePasswordUponFirstLogin" runat="server"
                                                                ControlToValidate="rblChangePasswordUponFirstLogin" Display="Dynamic" ErrorMessage="Change Password upon First Login is required." CssClass="errmsg">
                                                            </asp:RequiredFieldValidator>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class='sxlb'>
                                                    <asp:Label ID="lblActive" runat="server" AssociatedControlID="chkActive" Text="Active" CssClass="cptn"></asp:Label>
                                                </div>
                                                <div class='sxlm'>
                                                    <%--    <asp:CheckBox ID="chkActive" runat="server" TabIndex="6" Checked='<%# (Container is GridEditFormInsertItem) ? true : Eval("IsActive")%>' />--%>
                                                    <uc1:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="true" IsAutoPostBack="false" Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("IsActive") %>' />
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <%-- Removal of IsApplicant functionality-- %>
                                             <%-- <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblIsApplicant" runat="server" AssociatedControlID="chkApplicant" Text="IsApplicant"></asp:Label>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:CheckBox ID="chkApplicant" runat="server" TabIndex="7" Checked='<%# (Container is GridEditFormInsertItem) ? false : Eval("IsApplicant")%>' /></div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>--%>


                                            <div class='sxro sx2co'>
                                                <div class='sxlb <%# (Container is GridEditFormInsertItem) ? "nobg" : "" %>'>
                                                    <asp:Label ID="lblUnlockUser" runat="server" AssociatedControlID="chkUnlockUser" CssClass="cptn"
                                                        Text="Lock User"></asp:Label>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:CheckBox ID="chkUnlockUser" runat="server" TabIndex="11" Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("IsLockedOut")%>' />
                                                </div>
                                                <div id="dvClientAdmin" runat="server">
                                                    <div class='sxlb'>
                                                        <asp:Label ID="lblClientAdmin" runat="server" CssClass="cptn" Text="Replica of client admin"></asp:Label>
                                                    </div>
                                                    <div class="sxlm">
                                                        <infs:WclComboBox ID="cmbClientAdmin" runat="server" DataTextField="FirstName" DataValueField="OrganizationUserID"
                                                            AutoPostBack="false" Width="98%" EmptyMessage="--Select--" OnDataBound="cmbClientAdmin_DataBound">
                                                        </infs:WclComboBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarMUser" runat="server" GridMode="true" DefaultPanel="pnlMUser"
                                        TabIndexAt="12" ValidationGroup="grpValdManageUser" DisplayButtons="Save,Cancel,Extra" GridInsertText="Save" GridUpdateText="Save"
                                        ExtraButtonText="Reset Password" ExtraButtonIconClass="icnreset" OnExtraClick="btnReset_Click"
                                        EditModeButtons="Extra" AutoPostbackButtons="Extra" />
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
    </div>
</div>
<asp:Button ID="btnAccountLinkingDoPostBack" OnClick="AccountLinkingPostback_Click" runat="server" CssClass="buttonHidden" />

<script type="text/javascript">
    function UserAccountLinking() {
        var popupWindowName = "Link an account";
        winopen = true;
        var url = $page.url.create("~/CommonOperations/Pages/AccountLinkingPage.aspx");
        var popupHeight = $jQuery(window).height() * (100 / 100);
        var win = $window.createPopup(url, {
            size: "750," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Move | 
                Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnCloseUserAccountLinkingPopUp
        }, function () {
            this.set_title(popupWindowName);
        });
        return false;
    }

    function OnCloseUserAccountLinkingPopUp(oWnd, args) {
        oWnd.remove_close(OnCloseUserAccountLinkingPopUp);
        $jQuery('[id$=btnAccountLinkingDoPostBack]').click();
    }

</script>
