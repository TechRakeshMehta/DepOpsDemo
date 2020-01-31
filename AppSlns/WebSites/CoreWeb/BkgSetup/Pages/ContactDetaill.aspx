<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactDetaill.aspx.cs" Inherits="CoreWeb.BkgSetup.Views.ContactDetaill"
    MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
            parent.Page.hideProgress();
        });

        function RefrshTree() {

            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }

    </script>
    <div class="msgbox" id="divSuccessMsg">
        <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
    </div>
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblNewHeading" Text='Update Contact'
                runat="server"></asp:Label></h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="lblContactSuccess" runat="server"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlMNew">

                    <div class='sxro sx3co'>
                        <div id="divUserType" runat="server">
                            <div class='sxlb'>
                                <asp:Label ID="lblFirstName" Text="First Name" runat="server"
                                    CssClass="cptn" /><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox ID="txtFirstName" Width="100%" runat="server"
                                    MaxLength="50">
                                </infs:WclTextBox>
                                <div id="dvLabel" class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtFirstName"
                                        Display="Dynamic" class="errmsg" ErrorMessage="First Name is required." ValidationGroup='grpValdContacts' />
                                    <asp:RegularExpressionValidator runat="server" ID="revFirstName" ControlToValidate="txtFirstName"
                                        Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.]{1,50}$" ValidationGroup='grpValdContacts'
                                        ErrorMessage="Invalid character(s)." />
                                </div>
                            </div>
                        </div>
                        <div class='sxlb'>
                            <asp:Label ID="lblLastName" Text="Last Name" runat="server"
                                CssClass="cptn" /><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtLastName" Width="100%" runat="server"
                                MaxLength="50">
                            </infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                                    Display="Dynamic" class="errmsg" ErrorMessage="Last Name is required." ValidationGroup='grpValdContacts' />
                                <asp:RegularExpressionValidator runat="server" ID="rgvLastName" ControlToValidate="txtLastName"
                                    Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.]{1,50}$" ValidationGroup='grpValdContacts'
                                    ErrorMessage="Invalid character(s)." />
                            </div>
                        </div>

                        <div class='sxlb'>
                            <asp:Label ID="lblTitle" Text="Title" runat="server"
                                CssClass="cptn" />
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtTitle" Width="100%" runat="server"
                                MaxLength="50">
                            </infs:WclTextBox>
                            <div id="Div5" class='vldx'>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>


                    <div class='sxro sx3co'>
                        <div runat="server">
                            <div class='sxlb'>
                                <asp:Label ID="lblEmailAddress" Text="Email Address" runat="server"
                                    CssClass="cptn" /><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox ID="txtPrimaryEmailAddress" Width="100%" runat="server"
                                    MaxLength="50">
                                </infs:WclTextBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvPrimaryEmailAddress" ControlToValidate="txtPrimaryEmailAddress"
                                        Display="Dynamic" class="errmsg" ErrorMessage="Email Address is required." ValidationGroup='grpValdContacts' />
                                    <asp:RegularExpressionValidator runat="server" ID="rgvPrimaryEmailAddress" ControlToValidate="txtPrimaryEmailAddress"
                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="Email Address is not valid." ValidationGroup='grpValdContacts'
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                </div>
                            </div>
                        </div>
                        <div class='sxlb'>
                            <asp:Label ID="lblPhone" Text="Phone" runat="server"
                                CssClass="cptn" />
                        </div>
                        <div class='sxlm'>
                            <infs:WclMaskedTextBox Width="99%" ID="txtPhone" runat="server" TabIndex="2"
                                Mask="(###)-###-####" />
                            <div class='vldx'>
                            </div>
                        </div>

                        <div class='sxlb'>
                            <asp:Label ID="lblAddress1" Text="Address1" runat="server"
                                CssClass="cptn" />
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtAddress1" Width="100%" runat="server"
                                MaxLength="50">
                            </infs:WclTextBox>
                            <div class='vldx'>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>

                    <div class='sxro sx3co'>
                        <div runat="server">
                            <div class='sxlb'>
                                <asp:Label ID="lblAddress2" Text="Address2" runat="server"
                                    CssClass="cptn" />
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox ID="txtAddress2" Width="100%" runat="server"
                                    MaxLength="50">
                                </infs:WclTextBox>
                                <div class='vldx'>
                                </div>
                            </div>
                        </div>
                        <div class='sxlb'>
                            <asp:Label ID="lblZipCode" Text="Zip Code" runat="server"
                                CssClass="cptn" />
                        </div>
                        <div class='sxlm'>
                            <infs:WclMaskedTextBox ID="txtZipCode" Width="95%" runat="server" MaxLength="5" Mask="#####" />
                            <div class='vldx'>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <infsu:CommandBar ID="fsucCmdBarNew" runat="server" DefaultPanel="pnlMNew" ValidationGroup="grpValdContacts"
                DisplayButtons="Submit,Cancel" AutoPostbackButtons="Submit,Cancel"
                SubmitButtonText="Save"
                ButtonPosition="Center" OnSubmitClick="fsucCmdBarNew_SubmitClick" OnCancelClick="fsucCmdBarNew_CancelClick">
            </infsu:CommandBar>
        </div>
    </div>
</asp:Content>
