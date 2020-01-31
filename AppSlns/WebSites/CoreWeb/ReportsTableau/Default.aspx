
<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ReportsTableau.Views.TableauDefault"
    EnableEventValidation="false" Title="Seema Default" MasterPageFile="~/Shared/DefaultMaster.master"
    MaintainScrollPositionOnPostback="true" Codebehind="Default.aspx.cs" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <asp:placeholder runat="server" id="phDynamic"></asp:placeholder>
    <script type="text/javascript">
        function BindReport(url) {
            $jQuery('[id$=ifrPage]', $jQuery(parent.theForm)).attr('src', url);
        }
        function GenerateToken(url) {
            $jQuery('[id$=ifrPage]', $jQuery(parent.theForm)).attr('src', url);
        }
    </script>
</asp:Content>


