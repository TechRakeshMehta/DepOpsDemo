<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.Shell.Views.UserRegistration" Title="UserRegistration" StylesheetTheme="NoTheme"
    MasterPageFile="~/Shared/PublicPageMaster.master" CodeBehind="UserRegistration.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="ITSRegistration" Src="~/IntsofSecurityModel/UserControl/ITSUserRegistration.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="row dvLanguage" id="dvLanguage" runat="server">
        <div class="col-md-3">
            <infs:WclButton ID="btnLanguage" ControlName="btnLanguage" runat="server" AutoPostBack="true" Visible="true" CausesValidation="false" ButtonType="SkinnedButton" 
                CssClass="BtnLanguage" OnClick="btnLanguage_Click">
            </infs:WclButton>
        </div>
    </div>
    <infsu:ITSRegistration runat="server" />
    <asp:HiddenField ID="hdnLanguageCode" runat="server" />

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

    <script type="text/javascript">
        function ManageButton(sender, args) {
           // debugger;
            var languageCode;
            var language = sender._text;

            if (language != null && language.toLowerCase() == "english") {
                languageCode = 'AAAA';
                sender.set_text("Spanish");
                sender.set_toolTip("Click for Spanish");
            }

            if (language != null && language.toLowerCase() == "spanish") {
                languageCode = 'AAAB';
                sender.set_text("English");
                sender.set_toolTip("Click for English");
            }
            var hdnLanguageCode = $jQuery('[id$=hdnLanguageCode]');

            if (hdnLanguageCode != null && languageCode != null) {
                hdnLanguageCode.val(languageCode);
            }
        }
    </script>

</asp:Content>

