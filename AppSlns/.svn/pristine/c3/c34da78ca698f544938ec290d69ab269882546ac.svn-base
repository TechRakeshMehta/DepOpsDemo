<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnnouncementPopup.aspx.cs" Inherits="CoreWeb.CommonOperations.Views.AnnouncementPopup"
    MasterPageFile="~/Shared/PopupMaster.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
    <style type="text/css">
        .setfontbold strong, .setfontbold b {
            font-weight: bold !important;
        }
        .setfontitalic em, .setfontitalic i {
            font-style: italic !important;
        }
        .sxpnl { padding-right:5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblHeader" runat="server" Text="Announcement"></asp:Label>
        </h1>
        <div class="content">
            <div class="sxform auto">
                <asp:Panel ID="pnlAnnouncement" CssClass="sxpnl" runat="server">
                    <div class='sxro sx3co'>
                        <div class='m4spn'>
                            <div class='setfontbold setfontitalic'>
                                <%--<infs:WclEditor ID="rdEditorNotes" ClientIDMode="Static" runat="server" Width="99.5%"
                                ToolsFile="~/Templates/Data/Tools.xml" EnableResize="false" OnClientLoad="OnClientLoad" Enabled="false">
                                </infs:WclEditor>--%>
                                <asp:Literal ID="litHTML" runat="server" ></asp:Literal>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        function OnClientLoad(editor, args) {
            $jQuery('ul.reToolbar').width('auto');
        }

        //Function to close popup window
        //function ClosePopup() {
        //    var oArg = {};
        //    oArg.Action = "Cancel";
        //    top.$window.get_radManager().getActiveWindow().close();
        //}

    </script>
</asp:Content>

