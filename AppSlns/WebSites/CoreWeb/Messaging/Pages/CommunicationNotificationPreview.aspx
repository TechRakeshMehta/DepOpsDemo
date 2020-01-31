<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.Messaging.Views.CommunicationNotificationPreview" Title="Communication Notification"
    MasterPageFile="~/Shared/PopupMaster.master" Codebehind="CommunicationNotificationPreview.aspx.cs" %>

<asp:Content ID="content" ContentPlaceHolderID="PoupContent" runat="Server">
    <div id="msgBodyContent">
        <asp:Literal ID="litSubject" runat="server"></asp:Literal>
        <div style="padding-bottom: 15px">
        </div>
        <asp:Literal ID="litMessageContent" runat="server"></asp:Literal>
    </div>
</asp:Content>
