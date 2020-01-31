<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Messaging.Views.MessageDetails"
    Title="MessageDetails" MasterPageFile="~/Shared/PopupMaster.master" Codebehind="MessageDetails.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="content" ContentPlaceHolderID="PoupContent" runat="Server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxName1">
        <infs:LinkedResource Path="~/Resources/Mod/Messaging/messaging.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Generic/popup.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Messaging/MessageDetails.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <infs:WclToolBar Skin="Default" runat="server" ID="RadBtn" EnableViewState="false"
        OnClientButtonClicked="onButtonClicked" Width="100%">
        <Items>
            <telerik:RadToolBarButton Text="Print" CommandName="Print" ImageUrl="~/Resources/Mod/Messaging/icons/print.png"
                ImagePosition="Left" />
            <telerik:RadToolBarButton Text="Reply" CommandName="reply" ImageUrl="~/Resources/Mod/Messaging/Images/reply.gif"
                ImagePosition="Left" />
            <telerik:RadToolBarButton Text="Forward" CommandName="Forward" ImageUrl="~/Resources/Mod/Messaging/Images/ForwardIcon.gif"
                ImagePosition="Left" />
            <telerik:RadToolBarButton Text="Delete" CommandName="Delete" ImageUrl="~/Resources/Mod/Messaging/Images/delete_inbox.gif"
                ImagePosition="Left" />
            <telerik:RadToolBarSplitButton Text="Attachement(s)" Value="Attachment" ImageUrl="~/Resources/Mod/Messaging/icons/attachment.png"
                EnableDefaultButton="false">
                <Buttons>
                    <%--<telerik:RadToolBarButton Text="File.pdf" ImageUrl="../../App_Themes/Default/images/pdf.png"
                        ImagePosition="Left" Checked="true" NavigateUrl="../../MessageAttachments/ac9f8b0c-7679-45c6-8d94-9b070f424273.docx" />
                        <telerik:RadToolBarButton Text="File.doc" ImageUrl="../../App_Themes/Default/images/word.png"
                        ImagePosition="Left" />--%>
                </Buttons>
            </telerik:RadToolBarSplitButton>
        </Items>
    </infs:WclToolBar>
    <div class="section">
        <div class="content">
            <div class='sxro sx1co'>
                <asp:Label ID="lblMessageDetails" runat="server"></asp:Label>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnMessageId" />
</asp:Content>
