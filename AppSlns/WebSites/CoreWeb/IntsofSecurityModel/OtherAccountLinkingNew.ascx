<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OtherAccountLinking.ascx.cs"
    Inherits="CoreWeb.IntsofSecurityModel.Views.OtherAccountLinking" %>


<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<style type="text/css"></style>
<script src="../../Resources/Mod/Shared/ApplyNewIcons.js"></script>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Link Account</h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12" id="msgBox" runat="server">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </div>
    </div>
    <div class="row">
        <asp:Panel runat="server" ID="pnlRegForm">
            <div class='col-md-12'>
                <div class='row'>
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Enter Email Address</span><span class="reqd">*</span>
                        <infs:WclTextBox runat="server" ID="txtEmailAddress" Width="100%" CssClass="form-control" MaxLength="100">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvEmailAddress" ControlToValidate="txtEmailAddress"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Email Address is required." ValidationGroup="grpLinkAccount" />
                            <asp:RegularExpressionValidator ID="VldtxtEmail" runat="server" Display="Dynamic" ValidationGroup="grpLinkAccount"
                                ErrorMessage="Email Address is not valid." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                ControlToValidate="txtEmailAddress" CssClass="errmsg">
                            </asp:RegularExpressionValidator>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

    <infsu:CommandBar ID="fsucCmdBar" runat="server" DisplayButtons="Submit,Cancel" AutoPostbackButtons="Submit,Cancel"
        UseAutoSkinMode="false" ButtonSkin="Silk"
        ButtonPosition="Center" SubmitButtonText="Search" SubmitButtonIconClass="rbSearch" OnSubmitClick="fsucCmdBar_SubmitClick"
        CancelButtonText="Cancel" CancelButtonIconClass="rbCancel" OnCancelClick="fsucCmdBar_CancelClick" ValidationGroup="grpLinkAccount">
    </infsu:CommandBar>
    <div id="dvlstExistingUsers" runat="server" visible="false">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Matched User(s)</h2>
            </div>
        </div>
        <div class='row'>
            <asp:Panel runat="server" ID="Panel1">
                <div class='col-md-12'>
                    <div class='row'>
                        <div class='form-group col-md-3'>
                            <asp:Label ID="lblMatchedUserName" runat="server" CssClass='cptn' Text=""></asp:Label><span class="reqd">*</span>
                            <infs:WclTextBox TabIndex="8" runat="server" ID="txtPassword" TextMode="Password" Width="100%" CssClass="form-control" MaxLength="15"
                                autocomplete="off">
                            </infs:WclTextBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvPassword" ControlToValidate="txtPassword"
                                    ValidationGroup="grpCredentialsSubmit" Display="Dynamic" ErrorMessage="Password is required." CssClass="errmsg" />
                            </div>
                        </div>
                    </div>
                    <div class='col-md-12'>
                    </div>
                </div>
            </asp:Panel>
        </div>


        <infsu:CommandBar ID="cmbUserCredentials" runat="server" DisplayButtons="Submit,Cancel" AutoPostbackButtons="Submit,Cancel"
            ButtonPosition="Center" SubmitButtonText="Validate" SubmitButtonIconClass="rbvalidate" OnSubmitClick="cmbUserCredentials_SubmitClick"
             CancelButtonText="Cancel" CancelButtonIconClass="rbCancel" OnCancelClick="fsucCmdBar_CancelClick" ValidationGroup="grpCredentialsSubmit"
            UseAutoSkinMode="false" ButtonSkin="Silk">
        </infsu:CommandBar>
    </div>
    <div id="dvUserDataSelection" runat="server" visible="false">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Select the User for which you wish to retain username and email address.</h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:Panel runat="server" ID="pnlUserDataSelection">
                    <div class='row'>
                        <div class='col-md-12'>
                            <div class='form-group col-md-1' style="width:1.33%" >
                                <span>&nbsp;</span>
                                <asp:RadioButton runat="server" ID="rbSelectedUser1" GroupName="rblGroup"></asp:RadioButton>
                            </div>
                            <div class='form-group col-md-3'>
                                <span class='cptn'>User Name</span><span class="reqd">*</span>
                                <asp:Label ID="lblUserName1" runat="server" CssClass="form-control"><</asp:Label>
                            </div>
                            <div class='form-group col-md-3'>
                                <span class='cptn'>Email Address</span><span class="reqd">*</span>
                                <asp:Label ID="lblEmailAddress1" runat="server" CssClass="form-control"><</asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class='col-md-12'>
                            <div class='form-group col-md-1' style="width:1.33%">
                                <span>&nbsp;</span>
                                <asp:RadioButton runat="server" ID="rbSelectedUser2" GroupName="rblGroup"></asp:RadioButton>
                            </div>
                            <div class='form-group col-md-3'>
                                <span class='cptn'>User Name</span><span class="reqd">*</span>
                                <asp:Label ID="lblUserName2" runat="server" CssClass="form-control"><</asp:Label>
                            </div>
                            <div class='form-group col-md-3'>
                                <span class='cptn'>Email Address</span><span class="reqd">*</span>
                                <asp:Label ID="lblEmailAddress2" runat="server" CssClass="form-control"><</asp:Label>
                            </div>
                        </div>
                    </div>

                </asp:Panel>
            </div>
        </div>
        <infsu:CommandBar ID="cmbLinkAccount" runat="server" DisplayButtons="Submit,Cancel" AutoPostbackButtons="Submit,Cancel"
            ButtonPosition="Center" SubmitButtonText="Link Account" SubmitButtonIconClass="rbSave" OnSubmitClick="cmbLinkAccount_SubmitClick"
            CancelButtonText="Cancel" CancelButtonIconClass="rbCancel" OnCancelClick="fsucCmdBar_CancelClick" UseAutoSkinMode="false" ButtonSkin="Silk">
        </infsu:CommandBar>
    </div>
</div>
