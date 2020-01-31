<%@ Page Language="C#" AutoEventWireup="true" Inherits="ShellDefault"
    MasterPageFile="~/Shared/AppMaster.master" CodeBehind="Default.aspx.cs" %>

<asp:Content ID="content" ContentPlaceHolderID="AppPageContent" runat="Server">
    <a href="#" id="outeranchor" target="pageFrame" style="display: none;">Click Here</a>
    <iframe name="pageFrame" id="ifrPage" class="ifrmain"  title="Navigated to new screen" src="Main/Default.aspx" runat="server" clientidmode="Static"></iframe>
    <asp:UpdatePanel runat="server" ID="updDynamic">
        <ContentTemplate>
            <asp:PlaceHolder runat="server" ID="plcDynamic"></asp:PlaceHolder>
            <asp:PlaceHolder runat="server" ID="plcResult"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
