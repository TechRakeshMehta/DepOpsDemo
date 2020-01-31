<%@ Page Language="C#" AutoEventWireup="true" Inherits="ForgotPassword"
    StylesheetTheme="NoTheme" MasterPageFile="~/Shared/PublicPageMaster.master" CodeBehind="ForgotPassword.aspx.cs" %>

<%@ Register Src="IntsofSecurityModel/ForgotPassword.ascx" TagName="ForgotPassword"
    TagPrefix="ForgotPasswordControl" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Import Namespace="INTSOF.Utils" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeadContent" runat="Server">
    <%--<title>Complio :: Recover Account</title>--%>
    <title>Complio :: <%=Resources.Language.RECOVERACCOUNT %></title>    
    <style type="text/css">
        .reqd {
            color: Red;
        }

        .fesum {
            color: Red;
            padding-left: 00px;
        }

        .fgtfrm ul {
            list-style: none;
            background-color: #EFEFEF;
            padding: 20px;
        }

        .fgtfrm li {
            margin-bottom: 5px;
            min-height: 25px;
        }

        .flb {
            display: inline-block;
            *display: inline;
            *zoom: 1;
            width: 120px;
        }

        .radio_list {
            height: 10px;
        }

            .radio_list input, .sxlm .radio_list label {
                padding: 0!important;
                margin: 0!important;
                vertical-align: top!important;
            }

            .radio_list input {
                margin-right: 3px!important;
            }

            .radio_list label {
                margin-right: 5px!important;
            }

            .radio_list td {
                padding-top: 5px!important;
            }

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
                CssClass="BtnLanguage"  OnClick="btnLanguage_Click">
            </infs:WclButton>
        </div>
    </div>
    <h1 class="page_header">
        <%--Recover Account--%>
        <%=Resources.Language.RECOVERACCOUNT %>
    </h1>
    <p class="page_ins">
        <%=Resources.Language.RECOVERUSERPSWD %>
        <%--You can recover your username or password here. --%>
        <br />

        <%--Please enter your registered email
        address and a verification code will be sent to you. The items with--%>
        <%=Resources.Language.PLSENTEREMAIL %>
        <span class="reqd">*</span><%-- mark are required fields.--%>
        <%=Resources.Language.REQUIREDMARK %>
    </p>
    <%--<%#SysXUtils.GetMessage(ResourceConst.SECURITY_FORGOT_PASSWORD)%>--%>
    <ForgotPasswordControl:ForgotPassword ID="forgotPassword" runat="server" />
    <asp:HiddenField ID="hdnLanguageCode" runat="server" />

    <script type="text/javascript">
        //function ManageButton(sender, args) {
        //    //debugger;
        //    var languageCode;
        //    var language = sender._text;

        //    if (language != null && language.toLowerCase() == "english") {
        //        languageCode = 'AAAA';
        //        sender.set_text("Spanish");
        //        sender.set_toolTip("Click for Spanish");
        //    }
        //    if (language != null && language.toLowerCase() == "spanish") {
        //        languageCode = 'AAAB';
        //        sender.set_text("English");
        //        sender.set_toolTip("Click for English");
        //    }
        //    var hdnLanguageCode = $jQuery('[id$=hdnLanguageCode]');

        //    if (hdnLanguageCode != null && languageCode != null) {
        //        hdnLanguageCode.val(languageCode);
        //    }
        //}
    </script>

</asp:Content>
