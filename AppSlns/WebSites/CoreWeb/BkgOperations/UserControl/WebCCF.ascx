<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebCCF.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.WebCCF" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%--<link href="https://widget.webccf.com/webccfpoc/widgetstyles.css" rel="stylesheet" />--%>
<div class="section" id="divCapture" runat="server">
    <div class="content">
        <div class="sxform auto">
            <asp:Panel ID="pnlCapture" runat="server" DefaultButton="btnDrugScreenError">
                <h3 class="mhdr">Electronic Drug Screening Registration - Select a Drug Screening Location</h3>
                <div class="widgetbox iframeMIS">
                    <iframe id="MIS" runat="server"></iframe>
                    <div id="hiddenInstance" runat="server">
                        <asp:HiddenField ID="hdnFirstName" runat="server" />
                        <asp:HiddenField ID="hdnMiddleName" runat="server" />
                        <asp:HiddenField ID="hdnLastName" runat="server" />
                        <asp:HiddenField ID="hdnSuffix" runat="server" />
                        <asp:HiddenField ID="hdnAddress1" runat="server" />
                        <asp:HiddenField ID="hdnAddress2" runat="server" />
                        <asp:HiddenField ID="hdnCity" runat="server" />
                        <asp:HiddenField ID="hdnState" runat="server" />
                        <asp:HiddenField ID="hdnZip" runat="server" />
                        <asp:HiddenField ID="hdnEmail" runat="server" />
                        <asp:HiddenField ID="hdnPhoneDay" runat="server" />
                        <asp:HiddenField ID="hdnPhoneEvening" runat="server" />
                        <asp:HiddenField ID="hdnDOB" runat="server" />
                        <asp:HiddenField ID="hdnSSN" runat="server" />
                        <asp:HiddenField ID="hdnGender" runat="server" />
                        <asp:HiddenField ID="hdnTest" runat="server" />
                        <asp:HiddenField ID="hdnLab" runat="server" />
                        <asp:HiddenField ID="hdnReason" runat="server" />
                        <asp:HiddenField ID="hdnBOID" runat="server" />
                        <asp:HiddenField ID="hdnCustomerID" runat="server" />
                        <asp:HiddenField ID="hdnRegistrationID" runat="server" />
                        <asp:HiddenField ID="hdnErrorMessage" runat="server" />
                        <asp:HiddenField ID="hdnLabName" runat="server" />
                        <asp:HiddenField ID="hdnSiteCollection" runat="server" />
                        <asp:HiddenField ID="hdnIsUSCitizen" runat="server" />
                        <asp:HiddenField ID="hdnIsHavingSSN" runat="server" />

                        <script type="application/javascript">
                            // General Parameters
                            BOID = document.getElementById("<%= hdnBOID.ClientID %>").value;
                            CustomerID = document.getElementById("<%= hdnCustomerID.ClientID %>").value;
                            ServiceID = document.getElementById("<%= hdnTest.ClientID %>").value;
                            //Old Params
                            ApplicantFirstName = $jQuery("[id$=hdnFirstName]")[0].value;
                            ApplicantMiddleName = $jQuery("[id$=hdnMiddleName]")[0].value;
                            ApplicantLastName = $jQuery("[id$=hdnLastName]")[0].value;
                            ApplicantSuffix = $jQuery("[id$=hdnSuffix]")[0].value;
                            ApplicantAddress = $jQuery("[id$=hdnAddress1]")[0].value;
                            ApplicantAptSte = $jQuery("[id$=hdnAddress2]")[0].value;
                            ApplicantCity = $jQuery("[id$=hdnCity]")[0].value;
                            ApplicantState = $jQuery("[id$=hdnState]")[0].value;
                            ApplicantZipCode = $jQuery("[id$=hdnZip]")[0].value;
                            ApplicantSSN = $jQuery("[id$=hdnSSN]")[0].value;
                            ApplicantDOB = $jQuery("[id$=hdnDOB]")[0].value;
                            ApplicantGender = $jQuery("[id$=hdnGender]")[0].value;
                            ApplicantEmail = $jQuery("[id$=hdnEmail]")[0].value;
                            ApplicantDayTimePhone = $jQuery("[id$=hdnPhoneDay]")[0].value;
                            ApplicantEveningPhone = $jQuery("[id$=hdnPhoneEvening]")[0].value;
                            ApplicantReason = $jQuery("[id$=hdnReason]")[0].value;
                            IsUsCitizen = $jQuery("[id$=hdnIsUSCitizen]")[0].value; //UAT-4720
                            //UAT-4503 starts
                            IsHavingSSN = $jQuery("[id$=hdnIsHavingSSN]")[0].value;
                            ApplicantDOBAsDonorID = '';
                            //UAT-4503 ends
                            ApplicantDOB = formatDate(ApplicantDOB)
                            function formatDate(date) {
                                var d = new Date(date),
                                    month = '' + (d.getMonth() + 1),
                                    day = '' + d.getDate(),
                                    year = d.getFullYear();

                                if (month.length < 2) month = '0' + month;
                                if (day.length < 2) day = '0' + day;
                                ApplicantDOBAsDonorID = [month, day, year].join(''); //UAT-4503
                                return [year, month, day].join('-');
                            }
                            window.onload = function () {
                                getAuthTicket();

                            }

                            function getAuthTicket() {
                                Page.showProgress('Processing...');
                                var url = "Default.aspx/GetAuthTicket";
                                $jQuery.ajax({
                                    type: "POST",
                                    url: url,
                                    data: '',
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (Result) {
                                        if (Result.d != null) {
                                            loadWidget(Result.d);
                                        }
                                    },
                                    error: function (Result) {

                                      //  alert("Error" + dataurl);
                                    }
                                });
                            }
                            function loadWidget(ticket) {
                                listenToMessageFromChildWindow();
                                loadParameters(ticket);
                            }

                            function loadParameters(password) {
                               // debugger;
                                if (IsUsCitizen == "No")
                                {
                                    ApplicantZipCode = "";
                                    ApplicantState = "";
                                    ApplicantCity = "";
                                    ApplicantAptSte = "";
                                    ApplicantAddress = "";
                                }
                                var model = {
                                    //access_token: "hkJvOqwfHtVuP1qN5Fc1z3hDMbfI6gUJDksL_6ps1XRzbpO6c3rCA0MbMGgvc3rk9zZJfJCV_O8v44YafrkIXirtjdfgjQCVUx4bEdOg4QAo5o57Iii54PMuAZicSzCaRVIl8GSbo2-xfPH4VoVcV0qxp5RLbxFXiY3t1uCIqRQcfg3hzoxeWB6inGrfxHyD4-A-S_jdG6CvGDxvGKfaOEOjfg2zcddGAoVNK_A2AaA9VoqPA0WB2BIgtbJLjM5fp9_rqwKTxmGHFkkrDiJlNryJtPwV9C1tiS--Kla_8Hqef1XkMoASUTJ1a7bLFQEbyQId9H5Zl9fAC_WU7lrOHmIhpVEK6nH3-V_uj6z9yfTQz-ErmYdM11ntFCxJuFryWJk7nZsR5GZcDX0Bv-7BcYVgjVUhTC6MtXrr2qdesh_IzGjDVhUL6P720gbcDOBLYEB1MQmtsXR5J8p_6erjua1QyY_42NdMPT_kcdc_tJZ6PR5C0hJBIufzA_8SQEa2U2ISXGwZIetLs3cs1EDJZvCoueJzEFYKzxiLFUYv6-Ul6luaHgt7F4r7ZWapZQlahGEVT742W4BI-xnP7pXspmwj7JoeI7dXWVIbimRY8MV0rc-pL_kLRK4ku5961vs82glQqaRet1RfJ3IBWu02fri0cCCRo7qY79F7iyU6tZl2uIug9_0_T7tldhFaRXIqvcD2kal2CD_xE4YXTsgQuDoOMjBucnwEBd3Q0y-Ah1EIPtEWw7EMJJoN9wzdBOT-B4oNjpW7hycf05-Xzgd_wuiLbULvSGdTajd5IhoC0QM",
                                    access_token: password,
                                    BOID: BOID,
                                    CustomerID: CustomerID,
                                    ServiceID: ServiceID,

                                    //CONFIGURATION
                                    DonorInformation: 0,
                                    ShowConfirmationScreen: false,
                                    ShowInstructionScreen: false,
                                    ShowDonorNotesScreen: false,
                                    ShowContinueButton: false, //Change button label
                                    ShowBackButton: false,
                                    ShowTestReason: false,
                                    HidePaperProcess: true,
                                    ShowExpirationDetailsOption: false,
                                    ShowExpirationDateTime: "no",
                                    ShowNotificationOptions: true,
                                    SendEmailToDonor: false, //Remove parameter
                                    ShowMultipleEmailMessage: false,
                                    ReadOnlyPII: false,
                                    MaskSSNOption: "",
                                    MaskDOBOption: "",
                                    FormFoxReasonCode: 'PRE',
                                    LabCorpReasonCode: 'PE',
                                    //Extras
                                    ChargeDonorCreditCard: false,
                                    ApplicantEveningPhone: ApplicantEveningPhone,
                                    ApplicantDayTimePhone: ApplicantDayTimePhone,
                                    ApplicantEmail: ApplicantEmail,
                                    ApplicantGender: ApplicantGender,
                                    ApplicantDOB: ApplicantDOB,
                                    ApplicantSSN: IsHavingSSN==="True"?ApplicantSSN:ApplicantDOBAsDonorID,
                                    ApplicantZipCode: ApplicantZipCode,
                                    ApplicantState: ApplicantState,
                                    ApplicantCity: ApplicantCity,
                                    ApplicantAptSte: ApplicantAptSte,
                                    ApplicantAddress: ApplicantAddress,
                                    ApplicantSuffix: ApplicantSuffix,
                                    ApplicantLastName: ApplicantLastName,
                                    ApplicantMiddleName: ApplicantMiddleName,
                                    ApplicantFirstName: ApplicantFirstName

                                };
                                postModel(1, model);
                            }

                            function postModel(message_type, model) {
                                $jQuery("[id$=MIS]")[0].contentWindow.postMessage({ type: message_type, data: model }, "*");
                            }

                            function resize(size) {
                                $jQuery("[id$=MIS]")[0].style.height = size + "px";
                            }

                            function scrollTop() {
                                window.setTimeout(function () { window.scrollTo(0, 0); }, 0);
                            }

                            function finish() {
                                //     console.log('widget finished!');
                            }
                            function GetRegistrationId(data) {
                                //console.log('widget finished!');
                                $jQuery("[id$=hdnRegistrationID]")[0].value = data;
                                //Click the drug screen complete button
                                $jQuery("[id$=btnDrugScreenComplete]")[0].click();
                            }


                            function listenToMessageFromChildWindow() {
                                // Create IE + others compatible event handler
                                var eventMethod = window.addEventListener ? "addEventListener" : "attachEvent";
                                var eventer = window[eventMethod];
                                var messageEvent = eventMethod == "attachEvent" ? "onmessage" : "message";

                                // Listen to message from child window
                                eventer(messageEvent, function (e) {

                                    if (!e) return;

                                    if (e.data instanceof Array) {
                                    } else {
                                        if (e.data.type == 5) {
                                            resize(e.data.data);
                                        }

                                        if (e.data.type == 7) {
                                            scrollTop();
                                        }
                                        if (e.data.type == 3) {

                                            GetRegistrationId(e.data.data);
                                        }
                                    }
                                }, false);
                            }
                        </script>

                        <script type="text/javascript">
                        </script>
                        <script type="text/javascript">
                            function Reload() {
                                location.reload(true);
                            }
                        </script>
                    </div>
                    <div style="display: none;">
                        <asp:Button ID="btnDrugScreenComplete" runat="server" Text="Drug Screen Complete >>" OnClick="btnDrugScreenComplete_Click" />
                        <asp:Button ID="btnDrugScreenError" runat="server" Text="Drug Screen Error" OnClick="btnDrugScreenError_Click" />
                    </div>
                </div>
            </asp:Panel>
            <div id="divHiddenFields">
                <asp:HiddenField ID="hdnAttributeGroupId" runat="server" />
                <asp:HiddenField ID="hdnCustomFormId" runat="server" />
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    function OpenCustomForm(url) {
        var popupHeight = $jQuery(window).height() * (110 / 100);
        var win = $window.createPopup(url, { size: "1000," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnClose }
            , function () {
                this.set_title("Applicant Details");
            });
    }

    function OnClose(oWnd, args) {
        oWnd.remove_close(OnClose);

        var btnRefeshPage = $jQuery("[id$=btnRefeshPage]", $jQuery(window.parent.document));
        btnRefeshPage[0].click();
    }
</script>
<div class="section" id="divReadOnly" runat="server">
    <div>
        <div id="divEditButton" runat="server" style="right: 20px; position: absolute; z-index: 99999; display: none;">
            <infs:WclButton ID="cmdbarEdit" Text="Edit Drug Screen Form" runat="server" AutoPostBack="true" OnClick="cmdbarEdit_Click" />
        </div>
    </div>
    <h3 class="mhdr">Drug Screen Information</h3>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel ID="pnlReadOnly" CssClass="sxpnl" runat="server">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Status</span>
                    </div>
                    <div class='sxlm'>
                        <asp:Label ID="lblSuccess" runat="server" Text="Registered" />
                        <asp:Label ID="lblFailure" runat="server" CssClass="error" Text="NOT SCHEDULED - Please contact American DataBank to schedule your drug screen at the end of the order." />
                    </div>
                    <div id="divRegistrationID" runat="server" style="display: none">
                        <div class='sxlb'>
                            <span class="cptn">Registration ID</span>
                        </div>
                        <div class='sxlm'>
                            <asp:Label ID="lblRegistrationId" runat="server" />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>
<div id="divOrderConfirmation" runat="server">
    <h6 style="color: #8C1921; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">Drug Screen Information </h6>
    <div>

        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
            <span>Status</span>:&nbsp;<span style="font-weight: bold">
                <asp:Label ID="lblSuccessConfirm" runat="server" Text="Registered"></asp:Label></span>
            <asp:Label ID="lblFailureConfirm" runat="server" Visible="false" CssClass="error" Text="NOT SCHEDULED - Please contact American DataBank to schedule your drug screen at the end of the order." />
        </div>
        <div id="divRegistrationIdConfrm" runat="server" style="display: none; width: 30%; vertical-align: top; float: left">
            <span>Registration ID</span>:&nbsp;<span style="font-weight: bold"><asp:Label
                ID="lblRegistrationIdConfirm" runat="server"></asp:Label></span>
        </div>
        <div style="clear: both">
        </div>
    </div>
    <hr style="border-bottom: solid 1px #c0c0c0;" />
</div>
<%-- UAT:4513 - Start --%>
<style type="text/css">
    .iframeMIS {
     position: relative;
     padding-bottom: 65.25%;
     padding-top: 30px;
     height: 0;
     overflow: auto; 
     -webkit-overflow-scrolling:touch;
     /*border: solid black 1px;*/
} 
.iframeMIS iframe {
     position: absolute;
     top: 0;
     left: 0;
     width: 100%;
     height: 100%;
}
</style>
<%-- UAT:4513 - End --%>