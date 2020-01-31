<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateAccountInvitation.ascx.cs" Inherits="CoreWeb.SystemSetUp.Views.CreateAccountInvitation" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/ClinicalRotation/ManageRotation.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/ManageBulkArchive.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class='col-md-12'>
    <div class="row">
        <div class="msgbox" id="divSuccessMsg">
            <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
        </div>
    </div>
</div>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <asp:Label ID="lblCreateAccountInvitation" runat="server" Text=""></asp:Label>
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Institution</span><span class="reqd">*</span>
                        <infs:WclComboBox ID="ddlTenantName" runat="server" CssClass="form-control" CausesValidation="true" AutoPostBack="false"
                            DataTextField="TenantName" OnDataBound="ddlTenantName_DataBound" DataValueField="TenantID" Width="100%"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" AutoSkinMode="false" Skin="Silk">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvClient" ControlToValidate="ddlTenantName"
                                ValidationGroup="grpSendInvite" InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Institution is required." />
                        </div>
                    </div>
                    <div class='form-group col-md-2'>
                        <span class="cptn" style="color: transparent !important; display: block;"></span>
                        <infs:WclButton ID="btnDownloadTemplate" ButtonType="StandardButton" runat="server"
                            AutoPostBack="true" OnClick="btnDownloadTemplate_Click"
                            Text="Download Template" ButtonPosition="Center" ValidationGroup=""
                            CssClass="redBtn">
                        </infs:WclButton>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Upload Document</span>
                        <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" MaxFileInputsCount="1"
                            MultipleFileSelection="Disabled" OnClientFileSelected="onClientFileSelected"
                            OnClientFileUploaded="onFileUploadedZeroSize" OnClientFileUploadRemoved="onFileRemoved"
                            UploadedFilesRendering="BelowFileInput"
                            AllowedFileExtensions="xls,xlsx" ToolTip="Click here to select files to upload from your computer"
                            Width="100%" CssClass="form-control">
                            <Localization Select="Browse" />
                        </infs:WclAsyncUpload>
                    </div>
                    <div class='form-group col-md-2'>
                        <span class="cptn" style="color: transparent !important; display: block;"></span>
                        <infs:WclButton ID="btnUploadTemplate" ButtonType="StandardButton" runat="server"
                            AutoPostBack="true" OnClick="btnUploadTemplate_Click"
                            Text="Upload Template" ButtonPosition="Center" ValidationGroup="grpSendInvite"
                            CssClass="redBtn">
                        </infs:WclButton>
                    </div>
                </div>
            </div>
            <asp:UpdatePanel ID="udpnlPersonalAlias" runat="server">
                <ContentTemplate>
                    <asp:Repeater runat="server" ID="rptrRecpientName" OnItemCommand="rptrRecpientName_ItemCommand" OnItemDataBound="rptrRecpientName_ItemDataBound">
                        <ItemTemplate>
                            <div class='col-md-12'>
                                <div class="row">
                                    <div class='form-group col-md-3'>
                                        <span class='cptn'>First Name</span>
                                        <asp:Label runat="server" Width="100%" ID="lblfirstName"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("FirstName"))) %> </asp:Label>
                                        <infs:WclTextBox runat="server" ID="txtFirstName1" CssClass="form-control" Width="100%" Text='<%# Eval("FirstName") %>' Enabled="false" Visible="false"></infs:WclTextBox>
                                        <infs:WclTextBox runat="server" CssClass="form-control helloAni" Width="100%" ID="txtFirstName" Text='<%# Eval("FirstName") %>' Visible="false" MaxLength="50"></infs:WclTextBox>
                                        <div class="vldx">
                                            <asp:RequiredFieldValidator runat="server" ID="rfvFirstName" ControlToValidate="txtFirstName"
                                                Display="Dynamic" CssClass="errmsg" ErrorMessage="First Name is required." ValidationGroup="grpPersonAliasEdit" />
                                        </div>
                                    </div>
                                    <div class='form-group col-md-3'>
                                        <span class='cptn'>Last Name</span>
                                        <asp:Label runat="server" Width="100%" ID="lblLastName"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("LastName"))) %></asp:Label>
                                        <infs:WclTextBox runat="server" ID="txtLastName1" CssClass="form-control" Width="100%" Text='<%# Eval("LastName") %>' Enabled="false" Visible="false"></infs:WclTextBox>
                                        <infs:WclTextBox runat="server" ID="txtLastName" CssClass="form-control" Width="100%" Text='<%# Eval("LastName") %>' Visible="false" MaxLength="50"></infs:WclTextBox>
                                        <div class="vldx">
                                            <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Last Name is required." ValidationGroup="grpPersonAliasEdit" />
                                        </div>
                                    </div>
                                    <div class='form-group col-md-3'>
                                        <span class='cptn'>Email</span>
                                        <asp:Label runat="server" Width="100%" ID="lblEmail"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Email"))) %></asp:Label>
                                        <infs:WclTextBox runat="server" ID="txtEmail1" CssClass="form-control" Width="100%" Text='<%# Eval("Email") %>' Enabled="false" Visible="false"></infs:WclTextBox>
                                        <infs:WclTextBox runat="server" ID="txtEmail" CssClass="form-control" Width="100%" Text='<%# Eval("Email") %>' Visible="false" MaxLength="50"></infs:WclTextBox>
                                        <div class="vldx">
                                            <asp:RequiredFieldValidator runat="server" ID="rfvEmail" ControlToValidate="txtEmail"
                                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Email is required." ValidationGroup="grpPersonAliasEdit" />
                                        </div>
                                    </div>
                                    <div class='form-group col-md-3'>
                                        <span style="color: transparent !important;" class="cptn"></span>
                                        <div class='form-group col-md-1' style="padding-left: 0px;">
                                            <asp:LinkButton CommandName="edit" runat="server" CssClass="form-control blueText"
                                                Text="Edit" ID="btnEdit" CausesValidation="true" ValidationGroup="grpPersonAliasEdit"></asp:LinkButton>
                                        </div>
                                        <div class='form-group col-md-2'>
                                            <asp:LinkButton ID="btnDelete" CssClass="form-control blueText" OnClientClick="return confirm('Are you sure you want to delete this record ?')"
                                                runat="server" CommandName="delete" Text="Delete" ValidationGroup="grpPersonAliasEdit"
                                                CausesValidation="false" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class='col-md-12' id="divErrorMessage" runat="server" visible="false">
                                <asp:Label ID="lblErrorMsg" runat="server" Text="" CssClass="errmsg"></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <div class='col-md-12'>
                        <div class="row">
                            <div class='form-group col-md-3'>
                                <span class="cptn">First Name</span><span class="reqd">*</span>
                                <infs:WclTextBox runat="server" CssClass="form-control" Width="100%" ID="txtNewFirstName" MaxLength="50"></infs:WclTextBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvFullName" ControlToValidate="txtNewFirstName"
                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="First name is required." ValidationGroup="grpFormSubmit" />
                                </div>
                            </div>
                            <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                                <span class="cptn">Last Name</span><span class="reqd">*</span>
                                <infs:WclTextBox runat="server" CssClass="form-control" Width="100%" ID="txtNewLastName" MaxLength="50"></infs:WclTextBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtNewLastName"
                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="Last name is required." ValidationGroup="grpFormSubmit" />
                                </div>
                            </div>
                            <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                                <span class="cptn">Email</span><span class="reqd">*</span>
                                <infs:WclTextBox runat="server" CssClass="form-control" ID="txtNewEmail" Width="100%" MaxLength="50"></infs:WclTextBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvEmailAddress" ControlToValidate="txtNewEmail"
                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="Email Address is required." ValidationGroup="grpFormSubmit" />
                                    <asp:RegularExpressionValidator runat="server" ID="revEmailAddress" ControlToValidate="txtNewEmail"
                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="Email Address is not valid." ValidationGroup="grpFormSubmit"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                </div>
                            </div>
                            <div class='form-group col-md-3'>
                                <span style="color: transparent !important;" class="cptn"></span>
                                <asp:LinkButton ID="btnAddNewRecord" Text="Add" CssClass="form-control blueText"
                                    runat="server" OnClick="AddMore_Click" CausesValidation="true" ValidationGroup="grpFormSubmit" />
                            </div>
                        </div>
                    </div>
                    <div class='col-md-12'>
                        <div id="divErrorMessage" runat="server" visible="false">
                            <asp:Label ID="lblErrorMsg" runat="server" Text="" CssClass="errmsg"></asp:Label>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnDownloadTemplate" />
                </Triggers>
            </asp:UpdatePanel>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-6'>
                        <span class='cptn'>Subject</span><span class="reqd">*</span>
                        <infs:WclTextBox runat="server" CssClass="form-control" Width="100%" ID="txtSubject"></infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvSubject" ControlToValidate="txtSubject"
                                ValidationGroup="grpSendInvite" Display="Dynamic" CssClass="errmsg" Text="Subject is required." />
                        </div>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-10'>
                        <asp:Label ID="lblTemplateEditor" Text="Template Content" runat="server" AssociatedControlID="editorContent" CssClass="cptn" /><span
                            class="reqd">*</span>
                        <infs:WclEditor ID="editorContent" CssClass="form-control" Height="200px" runat="server" Width="100%" EditModes="Design"
                            OnClientCommandExecuting="OnClientCommandExecuting" OnClientLoad="OnClientLoad" ToolsFile="../WebSite/Data/Tools.xml">
                            <Content>
                                       <body style="background-color:White;"></body>
                            </Content>
                        </infs:WclEditor>
                        <div class='vldx'>
                            <asp:RequiredFieldValidator runat="server" ID="rfvContent" ControlToValidate="editorContent"
                                class="errmsg" Display="Dynamic" ErrorMessage="Template content is required."
                                ValidationGroup="grpSendInvite" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="col-md-12">
        <div class="row text-center">
            <infsu:CommandBar ID="CmdBarSubmit" runat="server" ButtonPosition="Center" DisplayButtons="Save,Submit,Cancel"
                AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo" DefaultPanelButton="Save" SubmitButtonText="Reset"
                SaveButtonText="Send Invite" OnSaveClick="CmdBarSubmit_SaveClick" OnCancelClick="CmdBarSubmit_CancelClick" OnSubmitClick="CmdBarSubmit_ResetClick"
                CancelButtonText="Cancel" ValidationGroup="grpSendInvite" UseAutoSkinMode="false" ButtonSkin="Silk">
            </infsu:CommandBar>
        </div>
    </div>
</div>
<script type="text/javascript">

    function OnClientCommandExecuting(editor, args) {

        var name = args.get_name();
        var val = args.get_value();

        if (name == "ddPlaceHolders") {
            editor.pasteHtml(val);
            //Cancel the further execution of the command as such a command does not exist in the editor command list
            args.set_cancel(true);
        }
    }

    // Function added to handle the issue related to distortion of the Template management in Add/Edit mode
    function OnClientLoad(editor, args) {
        $jQuery('ul.reToolbar').width('auto');
        //This function added to set focus on text box's in TemplatesMaintenanceFormEventSpecific.ascx to resolved the issue of User is not able to edit the Template Content 
        //Subject/Template Name until or unless make any changes in either Days and frequency fields
        setTimeout(function () {
            $jQuery("input[id$='txtNoOfDays']").focus();
            $jQuery("input[id$='txtTemplateName']").focus();
            $jQuery("input[id$='txtTemplateName']").val($jQuery("input[id$='txtTemplateName']").val());
            // $jQuery("input[id$='txtTemplateName']").blur();
            //$jQuery("input[id$='txtNoOfDays']").blur();
        });
    }
</script>
