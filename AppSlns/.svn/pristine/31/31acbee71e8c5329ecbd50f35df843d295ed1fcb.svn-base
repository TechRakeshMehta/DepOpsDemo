<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CIMAccountSelection.aspx.cs" Inherits="CoreWeb.ComplianceOperations.Views.CIMAccountSelection"
    MasterPageFile="~/Shared/DefaultMaster.master" %>

<%@ Register Src="~/CommonControls/UserControl/PageBreadCrumb.ascx" TagName="PageBreadCrumb"
    TagPrefix="infsu" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadContent" runat="server">
    <style type="text/css">
        HTML {
            overflow-x: hidden;
            overflow-y: hidden;
        }
    </style>
    <link href="../../Resources/Mod/CIM/paymentShipping.css" rel="stylesheet" />
    <script type="text/javascript" src="../../Resources/Mod/CIM/popup.js"></script>
    <%--<script type="text/javascript">

        function SelectSingleRadiobutton(rdbtnid) {
            var rdBtn = document.getElementById(rdbtnid);
            var rdBtnList = document.getElementsByTagName("input");
            for (i = 0; i < rdBtnList.length; i++) {
                if (rdBtnList[i].type == "radio" && rdBtnList[i].id != rdBtn.id) {
                    rdBtnList[i].checked = false;
                }
            }
        }

        var winopen = false;


        //function to Send Message
        function OpenPopup(sender, eventArgs) {
            var hfProfileID = $jQuery("[id$=hfProfileID]").val();
            var hfUrl = $jQuery("[id$=hfUrl]").val();
            var composeScreenWindowName = "Online Payment";
            if (hfProfileID != "0" && hfProfileID != "") {
                var url = $page.url.create("ManagePaymentProfile.aspx?CustomerProfileId=" + hfProfileID + "&PreviousPageUrl=" + hfUrl);
                var win = $window.createPopup(url, { size: "480,560", behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
                winopen = true;
                $telerik.$(".rwCloseButton", win.get_popupElement()).hide();
                return false;
            }
        }

        function OnClientClose(oWnd, args) {
            oWnd.remove_close(OnClientClose);
            if (winopen) {
                winopen = false;
            }
        }

    </script>--%>

    <script language="javascript" type="text/javascript">
        //Function to select one Radio Button at a time 
        function SelectSingleRadiobutton(rdbtnid) {
            var rdBtn = document.getElementById(rdbtnid);
            var rdBtnList = document.getElementsByTagName("input");
            for (i = 0; i < rdBtnList.length; i++) {
                if (rdBtnList[i].type == "radio" && rdBtnList[i].id != rdBtn.id) {
                    rdBtnList[i].checked = false;
                }
            }
        }

        var winopen = false;
        //function to open Authorize .net hosted form
        function OpenPopup() {

            //  alert('Hello');
            //window.history.forward();
            //document.getElementById("formAuthorizeNetPopup").submit();

            //<![CDATA[
            // Uncomment this line if eCheck is enabled. This does not affect functionality, only the initial sizing of the popup page for add payment.
            AuthorizeNetPopup.options.eCheckEnabled = true;

            // Uncomment these lines to define a function that will be called when the popup is closed.<a href="CIMAccountSelection.aspx">CIMAccountSelection.aspx</a>
            // For example, you may want to refresh your page and/or call the GetCustomerProfile API method from your server.
            AuthorizeNetPopup.options.onPopupClosed = function () {

                //debugger;
                //window.setTimeout(function () { window.location = 'https://dev.adbhome.com/ComplianceOperations/Pages/CIMAccountSelection.aspx' }, 1000);
                window.location.href = window.location.href;
            };

            // Uncomment this line if you do not have absolutely positioned elements on your page that can obstruct the view of the popup.
            // This can speed up the processing of the page slightly.
            //AuthorizeNetPopup.options.skipZIndexCheck = true;

            // Uncomment this line to use test.authorize.net instead of secure.authorize.net.
            var hiddenIsTestMode = $jQuery("[id$=hiddenIsTestMode]").val();
            if (hiddenIsTestMode.toLowerCase() == "false" || hiddenIsTestMode == "0") {
                AuthorizeNetPopup.options.useTestEnvironment = false;
            }
            else {
                AuthorizeNetPopup.options.useTestEnvironment = true;
            }

            //]]>

            //var customerProfileId = $jQuery("[id$=hfProfileID]").val();
            var token = $jQuery("[id$=token]");
            var hiddenToken = $jQuery("[id$=hiddenToken]").val();
            token.val(hiddenToken);
            //var data = '{"customerProfileId":' + customerProfileId + '}';

            //$jQuery.ajax({
            //    type: 'POST',
            //    url: 'CIMAccountSelection.aspx/GetToken',
            //    data: data,
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (result) {
            //        token.val(result.d);
            //        AuthorizeNetPopup.openAddPaymentPopup();
            //    },
            //    error: function () {
            //        alert('There was an error, please try again...');
            //    }
            //});
            AuthorizeNetPopup.openAddPaymentPopup();
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <div class="section">
        <h1 class="mhdr"><%=Resources.Language.AVLBLCRD%>
        </h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="lblMessage" runat="server" CssClass="info">
                    </asp:Label>
                </div>
            </div>
            <div class="swrap">
                <infs:WclGrid runat="server" ID="grdPaymentProfiles" AutoGenerateColumns="False"
                    AutoSkinMode="True" CellSpacing="0"
                    GridLines="Both" EnableDefaultFeatures="false" ShowAllExportButtons="False" OnNeedDataSource="grdPaymentProfiles_NeedDataSource"
                    OnDeleteCommand="grdPaymentProfiles_DeleteCommand" OnItemDataBound="grdPaymentProfiles_ItemDataBound">

                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="CustomerPaymentProfileId">
                        <CommandItemSettings ShowAddNewRecordButton="false" />
                        <CommandItemSettings RefreshText ="<%$ Resources:Language, REFRESH %>" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="<%$ Resources:Language, CRDTYPE %>">
                                <ItemTemplate>
                                    <asp:RadioButton Text="" ID="rdbCardNumber" onclick="SelectSingleRadiobutton(this.id);" GroupName="grpCardNumber" runat="server" />
                                    <asp:Label ID="lblCardNumberAndType" runat="server" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CardTypeAndNumber"))) %> '></asp:Label>
                                    <asp:HiddenField ID="hdnCustomerPaymentProfileId" runat="server" Value='<%# Eval("CustomerPaymentProfileId") %> ' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="NameOnCard" HeaderText="<%$ Resources:Language, NAMEONCARD %>" UniqueName="CardName">
                            </telerik:GridBoundColumn>
                            <%--<telerik:GridBoundColumn DataField="Expirydate" HeaderText="Expiration Date" UniqueName="ExpirationDate">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="Delete" ConfirmText="<%$ Resources:Language, CNFMTORMVCRD %>"
                                Text="<%$ Resources:Language, REMOVE %>" UniqueName="DeleteColumn">
                                <HeaderStyle CssClass="tplcohdr" />
                            </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                </infs:WclGrid>
            </div>
            <div style="margin-top: 5px">
                <infs:WclButton runat="server" ID="btnAddAccount" Text="<%$ Resources:Language, ADDNEWCARD %>" AutoPostBack="false" OnClientClicked="OpenPopup"></infs:WclButton>
            </div>
            <div style="clear: both"></div>
            <div>
                <infs:WclButton runat="server" ID="btnSubmitPayment" Text="<%$ Resources:Language, SUBMIT %>" Style="display: table; margin: 0 auto" OnClick="btnSubmitPayment_Click"></infs:WclButton>
                </div>
            <div style="float:right">
                <infs:WclButton runat="server" ID="btnGOTODASHBOARD" Visible="false" Text="<%$ Resources:Language, GOTODASHBOARD %>"  OnClick="GoToDashboard_Click"></infs:WclButton>
            </div>

            <asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
        </div>
    </div>
    <asp:HiddenField ID="hfProfileID" runat="server" />
    <input type='hidden' runat="server" name='hiddenToken' id='hiddenToken' />
    <input type='hidden' runat="server" name='hiddenIsTestMode' id='hiddenIsTestMode' />
</asp:Content>


<asp:Content ID="cntPayment" ContentPlaceHolderID="BodyContentOutsideForm" runat="server">
    <form id="formAuthorizeNetPopup" name="formAuthorizeNetPopup" method="post"
        target="iframeAuthorizeNet">
        <input type='hidden' name='token' id='token' />
        <input type='hidden' runat="server" name='paymentProfileId' id='paymentProfileId' />
        <input type='hidden' runat="server" name='shippingAddressId' id='shippingAddressId' />
    </form>

    <div id="divAuthorizeNetPopup" style="display: none;" class="AuthorizeNetPopupSimpleTheme">
        <div style="text-align: center; color: red; padding: 5px; font-size: 15px;">
            <span>
                <b><%=Resources.Language.NTCMPNYFLDNOTREQ%></b>
            </span>
        </div>
        <div class="AuthorizeNetPopupOuter">
            <iframe name="iframeAuthorizeNet" id="iframeAuthorizeNet"
                frameborder="0" scrolling="yes"></iframe>
        </div>

        <div class="AuthorizeNetShadow AuthorizeNetShadowT">
        </div>
        <div class="AuthorizeNetShadow AuthorizeNetShadowR">
        </div>
        <div class="AuthorizeNetShadow AuthorizeNetShadowB">
        </div>
        <div class="AuthorizeNetShadow AuthorizeNetShadowL">
        </div>
        <div class="AuthorizeNetShadow AuthorizeNetShadowTR">
        </div>
        <div class="AuthorizeNetShadow AuthorizeNetShadowBR">
        </div>
        <div class="AuthorizeNetShadow AuthorizeNetShadowBL">
        </div>
        <div class="AuthorizeNetShadow AuthorizeNetShadowTL">
        </div>
    </div>

    <div id="divAuthorizeNetPopupScreen" style="display: none;">
    </div>
</asp:Content>
