<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PopupMaster.master" AutoEventWireup="true" CodeBehind="EmailViewer.aspx.cs" Inherits="CoreWeb.Messaging.Views.EmailViewer" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">

        <style type="text/css">
            html
            {
                overflow: auto !important;
            }

            body
            {
                font-size: 16px;
                padding: 0;
                margin: 0;
                font-family: Segoe UI,Arial,Sans-Serif;
            }

            /*Header styles*/
            .header
            {
                background-color: #efefef;
                padding: 5px;
                border-bottom: 1px solid #adadad;
                color: #5D5D5D;
            }

                .header .subject
                {
                    font-size: 18px;
                    font-weight: bold;
                    color: #5D5D5D;
                }

                .header .senders
                {
                    font-size: 14px;
                    font-weight: bold;
                }

                .header .receivers, .header .copies, .header .date
                {
                    font-size: 12px;
                }

                .header span
                {
                    font-weight: bold;
                }

            /*Message body Styles*/
            .message
            {
                padding: 10px;
                font-size: 12px;
            }


            .previous .header
            {
                background-color: transparent;
                border-top: 1px solid #adadad;
                margin-top: 20px;
                border-bottom: none;
                padding: 10px 10px 0 10px;
            }

            .previous .subject, .previous .senders, .previous .receivers, .previous .copies, .previous .date
            {
                font-size: 1em;
                font-weight: normal;
                color: Black;
            }

            .previous .message
            {
                color: Blue;
            }

            .hrClass
            {
                border-width: 1px;
            }

            blockquote
            {
                margin-top: 0px !important;
                margin-bottom: 0px !important;
            }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">
    <div class="msgbox" id="divSuccessMsg" style="width:auto;">
        <asp:Label Text="" ID="lblSuccess" runat="server" />
    </div>
    <infs:WclSplitter ID="WclSplitter1" runat="server" LiveResize="true" Orientation="Horizontal"
        Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
        <infs:WclPane ID="WclPane1" runat="server" MinHeight="32" Height="32">
            <infs:WclToolBar ID="WclToolBar1" runat="server" Width="100%" OnButtonClick="WclToolBar1_ButtonClick">
                <Items>
                     <telerik:RadToolBarButton Text="Resend" ImageUrl="~/Resources/Mod/Messaging/Icons/sendmessage.png"
                        CommandName="resend"></telerik:RadToolBarButton>
                    <telerik:RadToolBarSplitButton Text="Attachment(s)" Value="Attachment" ImageUrl="~/Resources/Mod/Messaging/Icons/attachment.png"
                        EnableDefaultButton="false">
                    </telerik:RadToolBarSplitButton>
                </Items>
            </infs:WclToolBar>
        </infs:WclPane>
        <infs:WclPane ID="WclPane3" runat="server" Scrolling="Both">
            <div id="msgBodyContent" style="padding-left: 5px;">
                <%--<asp:Literal ID="litSubject" runat="server"></asp:Literal>--%>
                <div style="padding-bottom: 15px">
                </div>
                <asp:Literal ID="litMessageContent" runat="server"></asp:Literal>
            </div>
        </infs:WclPane>
    </infs:WclSplitter>
    <asp:HiddenField runat="server" ID="hdnMessageId" />
</asp:Content>

