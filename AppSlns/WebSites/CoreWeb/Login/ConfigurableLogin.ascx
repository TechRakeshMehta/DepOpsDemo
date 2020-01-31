<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigurableLogin.ascx.cs" Inherits="CoreWeb.Shell.Views.ConfigurableLogin" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="uc" TagName="CentrePanel" Src="~/Login/UserControl/LoginCenterPanel.ascx" %>


<style type="text/css">
    #imgBtnVideo {
        background-image: url("/Resources/Mod/WebSite/video_icon_red.png");
        background-size: 32px 32px;
        height: 32px;
        margin: auto;
        position: absolute;
        right: 190px;
        top: 0;
        width: 32px;
        cursor: pointer;
    }

    .rwStatusbarRow {
        display: none !important;
    }
</style>

<div class="msgbox">
    <div class="m_content">
        <asp:Label ID="lblVerificationMessage" runat="server" CssClass="reg-complete"></asp:Label>
        <a href="#" class="msg_cmd">Continue</a>
    </div>
</div>
<asp:Image ID="imgLogo" runat="server" Visible="false" />
<%--<div class="base_wrap">--%>
    <table class="tbl_main">
        <tr>
            <td>
                <div class="content">
                    <div class="abilogo">
                        <%--<asp:Image id="ImgTopLeftLogo" runat="server"/>--%>
                    </div>
                    <div class="logo_b2">
                    </div>
                    <div class="coloradologo">
                        <%--   <asp:Image id="ImgTopRightLogo" runat="server"/>--%>
                    </div>
                    <div class="orgname_b2" style="width: 67%; margin-left: 17%">
                        <asp:Label Text="" runat="server" ID="lblInstituteName" CssClass="lblInstitute" />
                    </div>
                    <div class="form_bl">
                        <div class="errors">
                            <asp:Label aria-atomic="true" role="alert" ID="lblErrorMessage" runat="server" CssClass="spnEr"></asp:Label>
                            <infs:WclButton runat="server" ID="btnResendActivationLink" Text="Click Here" UseSubmitBehavior="false" AutoPostBack="true" ToolTip="click here" aria-atomic="true" role="alert"
                                ButtonType="LinkButton" CssClass="u-button-resendActivationLink" Style="margin-left: -3px; margin-right: -3px" OnClick="btnResendActivationLink_Click" Visible="false" OnClientClicked="ResendActivationCodeClicked">
                            </infs:WclButton>
                            <asp:Label aria-atomic="true" role="alert" ID="lblErrorMessageExtended" runat="server" CssClass="spnEr" Visible="false"></asp:Label>
                            <asp:ValidationSummary aria-atomic="true" role="alert" ID="valSumLogin" DisplayMode="List" ValidationGroup="grpLogin"
                                runat="server" />
                        </div>
                        <div id="dvShibbolethMessage" runat="server" style="display: none">
                            <span style="font-size: 11px; color: red; text-align: center; background-color: #fff;">It looks like you may already have a Complio account</span>
                        </div>

                        <%--<div class="login_wrap">
                                <div class="mascot">
                                </div>--%>
                        <uc:CentrePanel ID="ucCentrePanel" runat="server" />

                        <%--</div>--%>
                        <%-- <infs:WclButton runat="server" ID="HyperLinkForgotPassword" Text="Can't access your account?"
                            ButtonType="LinkButton" CssClass="u-button" NavigateUrl="~/ForgotPassword.aspx"
                            Height="30px">
                        </infs:WclButton>--%>
                        <infs:WclButton runat="server" ID="HyperLinkForgotPassword" Text="<%$Resources:Language,CANTACCESSACCOUNT%>"
                            ButtonType="LinkButton" CssClass="u-button" NavigateUrl="~/ForgotPassword.aspx"
                            Height="30px">
                        </infs:WclButton>


                    </div>
                    <div class="register" id="dvCreateAccount" runat="server" visible="false">
                        <%-- <h2>New to Complio?</h2>--%>
                        <h2><%=Resources.Language.NEWTOCOMPLIO %></h2>
                        <div style="text-align: center; position: relative;">
                            <%--  <infs:WclButton runat="server" ID="WclButton1" Text="Create an account" ButtonType="LinkButton"
                                CssClass="button" NavigateUrl="~/UserRegistration.aspx">
                            </infs:WclButton>--%>
                            <infs:WclButton runat="server" ID="WclButton1" Text="<%$Resources:Language,CRTACCOUNT %>" ButtonType="LinkButton"
                                CssClass="button" NavigateUrl="~/UserRegistration.aspx">
                            </infs:WclButton>
                            <%--  UAT-1078 WB: Add a Icon to the login screens to view the create account help video--%>
                            <div id="imgBtnVideo" title="<%=Resources.Language.WATCHVIDEOTOOLTIP %>" onclick="openRadWin()"></div>
                        </div>
                        <infs:WclWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
                            <Windows>
                                <infs:WclWindow ID="radWndVideo" Skin="Web20" runat="server" ShowContentDuringLoad="false" Width="400px"
                                    Height="400px" Title="Video" Behaviors="Default" VisibleOnPageLoad="false" VisibleStatusbar="false">
                                </infs:WclWindow>
                            </Windows>
                        </infs:WclWindowManager>
                    </div>
                    <div class="register" id="divCentralCreateAccount" runat="server" visible="false">
                        <infs:WclButton runat="server" ID="btnCentralCreateAccount" Text="New to Complio?" ButtonType="LinkButton"
                            CssClass="button" NavigateUrl="~/ComplioInstructions.aspx">
                        </infs:WclButton>
                    </div>
                    <%--<div class="register" id="divSharedUserCreateAccount" runat="server" visible="false">
                                <infs:WclButton runat="server" ID="btnSharedUserCreateAccount" Text="Create An account" ButtonType="LinkButton"
                                    CssClass="button" NavigateUrl="~/SharedUserRegistration.aspx">
                                </infs:WclButton>
                            </div>--%>
                    <div class="supportedBrowser">
                        <%--  <div class="browserText">Preferred Browsers:</div>--%>
                        <div class="browserText"><%=Resources.Language.PREFERREDBROWSER %></div>
                        <div class="supportedBrowserPostion">
                            <img alt="Internet Explorer Version 9 +" src="Resources/Mod/WebSite/icn_internet_explorer_48.png" /><span>9+ </span>
                        </div>
                        <div class="supportedBrowserPostion">
                            <img alt="Safari Version 8.0 +" src="Resources/Mod/WebSite/icn_safari_48.png" /><span>8.0+ </span>
                        </div>
                        <div class="supportedBrowserPostion">
                            <img alt="Goole Chrome Version 44 +" src="Resources/Mod/WebSite/icn_chrome_48.png" /><span>44+ </span>
                        </div>
                        <div class="supportedBrowserPostion">
                            <img alt="Mozilla Firefox Version 36 +" src="Resources/Mod/WebSite/icn_firefox_48.png" /><span>36+ </span>
                        </div>
                        <div class="supportedBrowserPostion">
                            <img alt="Microsoft Edge" style="height: 14px; margin-top: 0.2px;" src="Resources/Mod/WebSite/icn_edge_48.png" /><span>12+ </span>
                        </div>
                    </div>

                    <%--<div class="address">
                            <strong><%=FooterGroupName%></strong>, Copyright ©<%=CopyRightYear %>. All Rights Reserved       
                            <br />
                            110 16th Street 8th Fl. Denver, CO 80202
                            <br />
                            Business Hours: 8:00am - 6:00pm (MT) Mon - Fri
                        </div>--%>

                    <div class="address">
                        <strong><%=FooterGroupName%></strong>
                        <br />
                        110 16th Street 8th Floor, Denver, CO 80202
                            <br />
                        720-292-2722, info@coloradofingerprinting.com
                    </div>

                    <div class="links" style="position: relative;">
                        <%--<div class="hipaacompliant"></div>--%>
                        <asp:Literal ID="litFooter" runat="server"><a>Privacy</a> | <a>Terms and conditions</a></asp:Literal>
                        <div class="pcilogo"></div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
<%--</div>--%>
<%--<asp:HiddenField runat="server" ID="hdnAccountVerificationPopup" />
    <asp:HiddenField runat="server" ID="hdnVerificationCode" />--%>
<infs:WclWindowManager ID="WclWindowManager3" runat="server" EnableShadow="true">
    <Windows>
        <infs:WclWindow ID="WclWindow2" Skin="Web20" runat="server" ShowContentDuringLoad="false" Width="400px"
            Height="400px" Title="Video" Behaviors="Default" VisibleOnPageLoad="false" VisibleStatusbar="false">
        </infs:WclWindow>
    </Windows>
</infs:WclWindowManager>

