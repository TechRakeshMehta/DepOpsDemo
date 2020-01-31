<%@ Page Title="Two Factor Authentication" MasterPageFile="~/Shared/PopupMaster.master" Language="C#" AutoEventWireup="true" CodeBehind="AuthenticationPopup.aspx.cs" Inherits="CoreWeb.CommonOperations.Pages.AuthenticationPopup" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pageLoad() {
            scrollToTop();
        }
        //function to get current popup window
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
        function ClosePopup(oArg) {
            var oWnd = GetRadWindow();
            if (oArg)
                oWnd.Close(oArg);
            else
                oWnd.Close();
        }
        function scrollToTop() {
            $jQuery("[id$=twoFaMainContent]")[0].offsetParent.scrollTop = 0;
        }
    </script>


    <infs:WclResourceManagerProxy runat="server" ID="rprxScheduleInvitation">
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Applicant/editprofile.css" ResourceType="StyleSheet" />
    </infs:WclResourceManagerProxy>
    <asp:Panel runat="server" CssClass="sxpnl" ID="pnlGoogleAuth">
        <div id="twoFaMainContent" class="section">
            <h1 class="mhdr"><%= Resources.Language.TWOFACTORAUTHEN%></h1>
            <div class="msgbox" id="dvMsgBox" runat="server">
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="error"></asp:Label>
                <asp:Label ID="lblSuccessMessage" runat="server" CssClass="sucs"></asp:Label>
            </div>
            <div class="section">
                <h1 class="mhdr"><%= Resources.Language.STEP1%>
                </h1>
                <div class="content">
                    <p><%= Resources.Language.DOWNLOADGOOGLEAUTH%></p>
                </div>
            </div>
            <div class="section">
                <h1 class="mhdr"><%= Resources.Language.STEP2%>
                </h1>
                <div class="content">
                    <p><%= Resources.Language.LINKYOURDEVICE%></p>
                    &nbsp
                    <p><b><%= Resources.Language.USINGQRCODE%></b> <%= Resources.Language.SELECT%><b> <%= Resources.Language.SCANBARCODE%></b>. <%=Resources.Language.QRCODESESCRIPTION%></p>
                    &nbsp;
                    <p><b><%=Resources.Language.USINGSECRETKEY%></b> <%= Resources.Language.SELECT%><b> <%= Resources.Language.ENTERPROVIDEDKEY%></b> <%= Resources.Language.ENTERACCNAME%>
                        <b> <%=Resources.Language.ENTERACCNAME%></b> <%= Resources.Language.ENTERSECKEYDESC%> <b><%= Resources.Language.ENTERYOURKEY%></b> <%= Resources.Language.SECRETKEYDESC%></p>
                    &nbsp;&nbsp;
                <div style="width: 100%; height: 100%">
                    <div style="width: 49%; height: 100%; float: left; border-right: black 2px solid;">
                        <div>
                            <span style="font-weight: bold; font-size: 14px;"><%=Resources.Language.QRCODE%></span>
                        </div>
                        <div>
                            <asp:Image ID="imgQrCode" runat="server" />
                        </div>
                    </div>
                    <div style="width: 49%; height: 100%; float: right;">
                        <div>
                            <span style="font-weight: bold; font-size: 14px;"><%= Resources.Language.ACCOUNTNAME%></span>
                        </div>
                        <div>
                            <asp:Label runat="server" ID="lblAccountName"></asp:Label>
                        </div>
                        &nbsp;
                        <div>
                            <span style="font-weight: bold; font-size: 14px;"><%= Resources.Language.SECRETKEY%></span>
                        </div>
                        <div>
                            <asp:Label runat="server" ID="lblManualSetupCode"></asp:Label>
                        </div>
                    </div>
                </div>
                </div>
            </div>
            <div class="section">
                <h1 class="mhdr"><%= Resources.Language.STEP3VERIFICATIONCODE%>
                </h1>
                <div class="content">
                    <p><%= Resources.Language.VERIFICATIONCODE%></p>
                    &nbsp;
                <div class="sxform auto">
                    <div class="sxpnl">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn"><%= Resources.Language.VERIFICATIONCODE%></span><span class='reqd'>*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtSecurityCode" MaxLength="50" ToolTip="<%$Resources:Language,PLZENTRSCRTYCODEMSG%>">
                                </infs:WclTextBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator ID="rfvSecurityCode" runat="server" ControlToValidate="txtSecurityCode"
                                        Display="Dynamic" ValidationGroup="grpverification" CssClass="errmsg"
                                        Text="<%$Resources:Language,VERIFICATIONCODEREQ%>" />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <infsu:CommandBar ID="cmdVerification" runat="server" DisplayButtons="Save,Cancel" AutoPostbackButtons="Save,Cancel" ValidationGroup="grpverification"
        ButtonPosition="Left" SaveButtonText="<%$Resources:Language,VERIFY%>" DefaultPanel="pnlGoogleAuth" OnSaveClick="cmdVerification_SaveClick" SaveButtonIconClass="" CancelButtonIconClass=""
        OnCancelClientClick="ClosePopup">
    </infsu:CommandBar>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CommandContent" runat="server">
</asp:Content>

