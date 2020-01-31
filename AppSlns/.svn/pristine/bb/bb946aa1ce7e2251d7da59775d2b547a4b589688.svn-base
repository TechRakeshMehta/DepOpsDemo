<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.AMS_Main.MainDefault"
    Title="Default" MasterPageFile="~/Shared/DefaultMaster.master" CodeBehind="Default.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadContent" runat="server">
    <!-- Mobile Specific Metas
  ================================================== -->
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <!--[if lt IE 9]>
		<script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
	<![endif]-->
    <!-- Favicons
	================================================== --
	<link rel="shortcut icon" href="images/favicon.ico">
	<link rel="apple-touch-icon" href="images/apple-touch-icon.png">
	<link rel="apple-touch-icon" sizes="72x72" href="images/apple-touch-icon-72x72.png">
	<link rel="apple-touch-icon" sizes="114x114" href="images/apple-touch-icon-114x114.png"> -->

    <style>
        .wg_chklist td
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">

    <%-- <div class="section" >
       <h1 class="mhdr">
            Subscription Setting
        </h1>
        <div class="content">
            <div class="sxform auto">
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlName1">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            Notifications
                        </div>
                        <div class='sxlm'>
                            <asp:CheckBoxList ID="cblNotification" runat="server" DataTextField="Name" DataValueField="CommunicationEventId" RepeatLayout="UnorderedList">
                                <asp:ListItem Text="Order" />
                                <asp:ListItem Text="Subscriptions" />
                                <asp:ListItem Text="Profile Changes" />
                                <asp:ListItem Text="Account Status" />
                                <asp:ListItem Text="Internal Messages" />
                            </asp:CheckBoxList>
                        </div>
                        <div class='sxlb'>
                            Reminders
                        </div>
                        <div class='sxlm'>
                            <asp:CheckBoxList ID="cblReminder" runat="server" DataTextField="Name" DataValueField="CommunicationEventId"
                                RepeatLayout="UnorderedList">
                                <asp:ListItem Text="Order" />
                                <asp:ListItem Text="Subscriptions" />
                                <asp:ListItem Text="Profile Changes" />
                                <asp:ListItem Text="Account Status" />
                                <asp:ListItem Text="Internal Messages" />
                                <asp:ListItem Text="Schedule Events" />
                            </asp:CheckBoxList>
                        </div>
                        <div class='sxlb'>
                            Alerts
                        </div>
                        <div class='sxlm'>
                            <asp:CheckBoxList ID="cblAlert" runat="server" DataTextField="Name" DataValueField="CommunicationEventId"
                                RepeatLayout="UnorderedList">
                                <asp:ListItem Text="Subscriptions" />
                                <asp:ListItem Text="Internal Messages" />
                            </asp:CheckBoxList>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <infsu:CommandBar ID="fsucCmdBar1" runat="server" DisplayButtons="Submit" DefaultPanel="pnlName1" ButtonPosition="Center">
                <%--OnSubmitClick="btnSubmit_Click"
            </infsu:CommandBar>
        </div>
    </div>
  <div style="display: none">
        <div class="msgbox" id="divSuccessMsg">
            <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
        </div>
        <infs:WclResourceManagerProxy runat="server" ID="rprxDash">
            <infs:LinkedResource Path="~/Resources/Mod/Dashboard/mod.css" ResourceType="StyleSheet" />
        </infs:WclResourceManagerProxy>
        <div class="dashwrap">
            
            <asp:PlaceHolder runat="server" ID="plcDynamic"></asp:PlaceHolder>
        </div>
    </div>--%>
    <infs:WclResourceManagerProxy runat="server" ID="rprxDash">
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/mod.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Dashboard.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/main.css" ResourceType="StyleSheet" />
    </infs:WclResourceManagerProxy>
    <div>
        <div class="content">
            <div class="auto">
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlName1">
                    <asp:PlaceHolder runat="server" ID="phDynamic"></asp:PlaceHolder>
                </asp:Panel>
            </div>
        </div>
        <div id="mascot_d"></div>
    </div>
</asp:Content>
