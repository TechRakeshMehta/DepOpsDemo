<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Authenticator.aspx.cs" Inherits="CoreWeb.Authenticator" StylesheetTheme="NoTheme" MasterPageFile="~/Shared/PublicPageMaster.master" %>

<%@ Register Src="~/CommonOperations/GoogleAuthenticator.ascx" TagName="GoogleAuthenticator"
    TagPrefix="infsu" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="INTSOF.Utils" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeadContent" runat="Server">
    <style type="text/css">
        .BtnLanguage {
            width: 70% !important;
            background-color: #EFEFEF !important;
            text-align: center !important;
            border-radius: 10px !important;
            font-size: medium !important;
            font-style: normal !important;
            font-family: bold !important;
            color: #333 !important;
        }

        .dvLanguage {
            width: 10% !important;
            float: right !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="row dvLanguage" id="dvLanguage" runat="server">
        <div class="col-md-3">
            <infs:WclButton ID="btnLanguage" ControlName="btnLanguage" runat="server" AutoPostBack="true" Visible="true" CausesValidation="false" ButtonType="SkinnedButton"
                CssClass="BtnLanguage" OnClick="btnLanguage_Click">
            </infs:WclButton>
        </div>
    </div>

    <asp:HiddenField ID="hdnLanguageCode" runat="server" />
    <%--<h1 class="page_header">Two Factor Authentication</h1>--%>
    <h1 class="page_header"><%=Resources.Language.TWOFACTORAUTHEN%></h1>

    <infsu:GoogleAuthenticator ID="GoogleAuthenticator" Visible="true" runat="server" />

    <asp:Literal ID="litFooter" runat="server" Visible="false"></asp:Literal>
</asp:Content>

