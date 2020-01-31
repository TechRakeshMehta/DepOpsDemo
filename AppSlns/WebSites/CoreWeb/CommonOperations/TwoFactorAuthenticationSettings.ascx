<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TwoFactorAuthenticationSettings.ascx.cs" Inherits="CoreWeb.CommonOperations.Views.TwoFactorAuthenticationSettings" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
<script type="text/javascript">

    function OpenAuthenticationPopUpTest() {
        var url = $page.url.create("~/CommonOperations/Pages/AuthenticationPopup.aspx");
        var popupHeight = $jQuery(window).height() * (80 / 100);
        var win = $window.createPopup(url,
                                        {
                                            size: "1100," + popupHeight,
                                            behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Maximize, onclose: OnPopupClientClose
                                        });
    }

    function OnPopupClientClose(oWnd, args) {
        oWnd.remove_close(OnPopupClientClose);
        $.ajax({
            type: "POST",
            url: "Default.aspx/TwofactorAuthenticationLabelupdate",
            data: '',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                // alert(response.d);
                SetAuthenticationLabels(response);
            },
            failure: function (response) {
                // alert(response.d);
            }
        });
    }

    function SetAuthenticationLabels(response) {
        // debugger;
        var lblAuthentication = $jQuery("[id$=lblAuthentication]")[0];
        var lnkAuthentication = $jQuery("[id$=lnkAuthentication]");
        var lnkDisableAuthentication = $jQuery("[id$=lnkDisableAuthentication]");
        var rb = $jQuery("[id$=rdbSpecifyAuthentication]")[0];
        if (response.d != undefined && response.d != "") {
            if (lblAuthentication != undefined && lnkAuthentication != undefined && lnkDisableAuthentication != undefined) {
                if (response.d == "<%=Resources.Language.NOTENABLED%>") {
                    lblAuthentication.innerHTML = response.d + "&nbsp;";
                    lnkAuthentication[0].innerHTML = "<%= Resources.Language.CLCKTOENBLE%>";
                    lnkAuthentication.show();
                    lnkDisableAuthentication.hide();

                    var rbItems = rb.getElementsByTagName('input');
                    rbItems[2].disabled = true;
                    $jQuery("[id$=lblAuthLabel]")[0].innerHTML = " - To use this setting please make sure to enable Google Authenticator." + "&nbsp;";
                }
                else if (response.d == "<%= Resources.Language.ENABLED%>") {
                    lblAuthentication.innerHTML = response.d + "&nbsp;";
                    lnkAuthentication.hide();
                    lnkDisableAuthentication.show();
                    var rbItems = rb.getElementsByTagName('input');
                    rbItems[2].disabled = false;

                    //var rbItems = rb.getElementsByTagName('input');
                    //rbItems[1].disabled = false;

                    $jQuery("[id$=lblAuthLabel]")[0].innerHTML = "- <%=Resources.Language.PLEASE%>" + "&nbsp;";
                }
                else if (response.d == "<%= Resources.Language.ENABLENOTVERIFY%>") {
                    lblAuthentication.innerHTML = response.d + "&nbsp;";
                    lnkAuthentication[0].innerHTML = "<%= Resources.Language.CLICKTOVERIFY%>";
                    lnkAuthentication.show();
                    lnkDisableAuthentication.hide();

                    var rbItems = rb.getElementsByTagName('input');
                    rbItems[2].disabled = true;
                }
            }
        }
    }


    function OpenDisableAuthConfirmation() {
        $jQuery(".disableAuthConfirmation").css("display", "block");
        var dialog = $window.showDialog($jQuery(".disableAuthConfirmation").clone().show(), {
            approvebtn: {
                autoclose: true, text: "Yes", click: function () {
                    //debugger;
                    $jQuery("#<%=btnHiddenDisable.ClientID %>").trigger('click');
                }
            }, closeBtn: {
                autoclose: true, text: "No", click: function () {
                    return false;
                }
            }
        }, 475, 'Complio');
        //debugger;
        $jQuery(".disableAuthConfirmation").css("display", "none");
    }

    function AddGoogleAuthenticatorLinkInRadioButton() {
        var radioButtonList = $jQuery("[id$=rdbSpecifyAuthentication]")[0];
        var rbInnerHTML = radioButtonList.getElementsByTagName('td')[2].innerHTML;
        var anchorTag = $jQuery("[id$=dvGoogleAuthenticationTags]")[0].innerHTML;
        if (rbInnerHTML != undefined && anchorTag != undefined && radioButtonList.getElementsByTagName('td')['2'].getElementsByTagName('a').length < 1) {
            var rbInnerHTMLNew = rbInnerHTML + anchorTag;
            radioButtonList.getElementsByTagName('td')[2].innerHTML = rbInnerHTMLNew;
            var NoOfLabels = radioButtonList.getElementsByTagName('label').length;
            if (NoOfLabels > 0 && NoOfLabels != undefined) {
                for (i = 0; i < NoOfLabels; i++) {
                    radioButtonList.getElementsByTagName('label')[i].style.float = "left";
                }
            }
        }
    }

    $jQuery(document).ready(function () {
        SetOnChangeEventOnRadioButtonList();
    });
    function SetOnChangeEventOnRadioButtonList() {
        if ($jQuery("[id$=rdbSpecifyAuthentication]").length > 0) {
            $jQuery("[id$=rdbSpecifyAuthentication]").off('change');
            $jQuery("[id$=rdbSpecifyAuthentication]").on('change', function () {
                var selectedValue = $jQuery("[id$=rdbSpecifyAuthentication]").find(":checked").val();
                var txtPhoneNumberIsRequired = false;

                if (selectedValue == 'AAAB') {
                    txtPhoneNumberIsRequired = true;
                }
                else {
                    if ($jQuery("[id$=rdbTextNotification]").length > 0) {
                        var selectedTextNotificationValue = $jQuery("[id$=rdbTextNotification]").find(":checked").val();

                        if (selectedTextNotificationValue == "True") {
                            txtPhoneNumberIsRequired = true;
                        }
                    }
                }

                if (txtPhoneNumberIsRequired) {
                    ValidatorEnable($jQuery('[id$=rfvPhoneNumber]')[0], true);
                    $jQuery('[id$=spnPhoneNumberReq').show();
                }
                else {
                    ValidatorEnable($jQuery('[id$=rfvPhoneNumber]')[0], false);
                    $jQuery('[id$=spnPhoneNumberReq').hide();
                }

                $jQuery("[id$=hdnrdbSpecifyAuthenticationCalculatedValue]").val(selectedValue);
            });
        }
    }

</script>
<style>
    table input[type=radio], [label] {
        float: left;
    }

    table td {
        padding: 2px;
    }
</style>

<div id="dvSpecifyAuthentication" class='sxro sx3co' runat="server" style="display: none;">
    <div class='sxlb'>
        <span class='cptn'>Two Factor Authentication Setting</span>
    </div>
    <div class='sxlm m3spn'>
        <%--<asp:RadioButtonList ID="rdbSpecifyAuthentication" runat="server" RepeatDirection="Vertical">
            <asp:ListItem Text="Disable" Value="NONE"></asp:ListItem>
            <asp:ListItem Text="Google Authenticator &nbsp;" Value="AAAA" Enabled="false" title="Before selecting this you need to enable google authentication by clicking below link."></asp:ListItem>
            <asp:ListItem Text="Text Message" Value="AAAB" Enabled="false" title="Before selecting this you need to confirm Text Message Notification."></asp:ListItem>
        </asp:RadioButtonList>--%>

        <asp:RadioButtonList ID="rdbSpecifyAuthentication" runat="server" RepeatDirection="Vertical" Width="100%">
            <asp:ListItem Text="<% $Resources:Language, DISABLE %>" Value="NONE"></asp:ListItem>
            <asp:ListItem Text="<% $Resources:Language, TWOFCTRAUTHTMSG %>" Value="AAAB"></asp:ListItem>
            <asp:ListItem Text="Google Authenticator &nbsp;" Value="AAAA" Enabled="false"></asp:ListItem>
        </asp:RadioButtonList>
    </div>
</div>

<div id="dvGoogleAuthentication" class='sxro sx3co' runat="server" style="display: none;">
    <div class='sxlb'>
        <span class='cptn'>Google Authenticator</span>
    </div>
    <div class='sxlm' id="dvGoogleAuthenticationTags" runat="server">
        <asp:Label ID="lblAuthentication" runat="server" Text="" Style="float: left;"></asp:Label>
        <asp:Label ID="lblAuthLabel" runat="server" Text=" - To use this setting please make sure to enable Google Authenticator." Style="display: block; float: left; display: none;"></asp:Label>

        <a id="lnkAuthentication" runat="server" href="#" onclick="OpenAuthenticationPopUpTest();return false;"></a>
        <a id="lnkDisableAuthentication" runat="server" href="#" onclick="OpenDisableAuthConfirmation();return false;"><%= Resources.Language.CLICKTODISABLE%></a>

    </div>
    <div style="display: none">
        <asp:Button ID="btnHiddenDisable" runat="server" OnClick="btnHiddenDisable_Click" />
    </div>

    <div class='sxroend'>
    </div>
</div>
<div id="dvDisableAuthConfirmation" class="disableAuthConfirmation" title="Complio" runat="server" style="display: none">
    <p style="text-align: center"><%= Resources.Language.DISABLETWOFACTORAUTH%></p>
</div>
