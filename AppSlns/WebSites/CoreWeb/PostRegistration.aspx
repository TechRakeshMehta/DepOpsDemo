<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="PostRegistration" StylesheetTheme="NoTheme" MasterPageFile="~/Shared/PublicPageMaster.master" CodeBehind="PostRegistration.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageHeadContent" runat="Server">
    <title>Complio :: Registration Complete!</title>
    <style type="text/css">
        .bold {
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div id="dvNewApplicantAccount" runat="server" visible="false">
        <h1 class="page_header">Account has been created!
        </h1>
        <div id="dvNewApplicant" runat="server">
            <p>
                Thank you for registering! An email has been sent with an activation link to your
            email,
            <asp:Label ID="lblEmail" runat="server" CssClass="bold"></asp:Label>. Please click
            the activation link to activate your account.
            </p>

            <h2 style="color: red; padding-top: 40px; font-style: italic">Please check your spam/junk folder if the activation email does not show up in your inbox.
            </h2>
        </div>
        <div id="dvSharedUser" runat="server" visible="false">
            <p>
                Thank you for registering! Your Account is activated.
                <asp:Label ID="lblLogin" runat="server" CssClass="bold"></asp:Label>
                <a href="Login.aspx">Click here to login</a>.
            </p>
        </div>

    </div>
    <div id="dvLinkedApplicantAccount" runat="server" visible="false">
        <h1 class="page_header">Account has been Linked!
        </h1>
        <p>
            Thank you for registering! An email has been sent to your email,
            <asp:Label ID="lblLinkEmail" runat="server" CssClass="bold"></asp:Label>. <a href="Login.aspx">Click here to login</a>.
        </p>
    </div>
    <div id="dvSharedUserLinkedAccount" runat="server" visible="false">
        <h1 class="page_header">Account has been Linked!
        </h1>
        <p>
            Thank you for registering! Your Account is activated.
            <a href="Login.aspx">Click here to login</a>.
        </p>
    </div>
</asp:Content>
