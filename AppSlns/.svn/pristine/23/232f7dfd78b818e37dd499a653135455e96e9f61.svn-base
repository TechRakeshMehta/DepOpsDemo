<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Messaging.Views.MessageViewer"
    Title="" MasterPageFile="~/Shared/PopupMaster.master" Codebehind="MessageViewer.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="prxMP">
        <%--<infs:LinkedResource Path="~/Resources/Mod/Messaging/writemessage.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Messaging/writemessage.js" ResourceType="JavaScript" />--%>
        <infs:LinkedResource Path="~/Resources/Mod/Messaging/MessageViewer.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <infs:WclSplitter ID="WclSplitter1" runat="server" LiveResize="true" Orientation="Horizontal"
        Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
        <infs:WclPane ID="WclPane1" runat="server" MinHeight="32" Height="32">
            <infs:WclToolBar ID="WclToolBar1" runat="server" Width="100%" OnClientButtonClicked="btntoolbar_clicked">
                <Items>
                    <telerik:RadToolBarButton Text="Print" CommandName="Print" ImageUrl="~/Resources/Mod/Messaging/icons/print.png"
                        ImagePosition="Left" />
                    <telerik:RadToolBarButton Text="PrintSeparator" IsSeparator="true">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton Text="Reply" CommandName="Reply" ImageUrl="~/Resources/Mod/Messaging/icons/reply.png"
                        ImagePosition="Left" />
                    <telerik:RadToolBarButton Text="Reply All" CommandName="ReplyAll" ImageUrl="~/Resources/Mod/Messaging/icons/replyall.png"
                        ImagePosition="Left" />
                    <telerik:RadToolBarButton Text="Forward" CommandName="Forward" ImageUrl="~/Resources/Mod/Messaging/icons/forward.png"
                        ImagePosition="Left" />
                    <telerik:RadToolBarButton Text="Delete" CommandName="Delete" ImageUrl="~/Resources/Mod/Messaging/Images/delete_inbox.gif"
                        ImagePosition="Left" />
                    <telerik:RadToolBarButton Text="Separator" IsSeparator="true">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton Value="HighImportance" Text="High Importance" Enabled="false" ImageUrl="~/Resources/Mod/Messaging/Icons/important.png"
                        CheckOnClick="true">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarSplitButton Text="Attachment(s)" Value="Attachment" ImageUrl="~/Resources/Mod/Messaging/Icons/attachment.png"
                        EnableDefaultButton="false">
                        <Buttons>
                          <%--  <telerik:RadToolBarButton Text="File 1.pdf">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton Text="File 2.jpg">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton Text="" IsSeparator="true">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton Text="Download All...">
                            </telerik:RadToolBarButton>--%>
                        </Buttons>
                    </telerik:RadToolBarSplitButton>
                </Items>
            </infs:WclToolBar>
        </infs:WclPane>
        <infs:WclPane ID="WclPane3" runat="server" Scrolling="Both">
        </infs:WclPane>
    </infs:WclSplitter>
    <asp:HiddenField runat="server" ID="hdnMessageId" />
</asp:Content>
