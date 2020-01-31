<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ApplicantModule.Views.ApplicantModuleDefault"
    Title="Default" MasterPageFile="~/Shared/DefaultMaster.master" Codebehind="Default.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="msgbox" id="divSuccessMsg">
        <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
    </div>
    <infs:WclResourceManagerProxy runat="server" ID="rprxDash">
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/mod.css" ResourceType="StyleSheet" />
    </infs:WclResourceManagerProxy>
    <div class="dashwrap">
        <asp:PlaceHolder runat="server" ID="plcDynamic"></asp:PlaceHolder>
    </div>
</asp:Content>
