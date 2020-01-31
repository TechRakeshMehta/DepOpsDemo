<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceOperations.Views.OrderReview" CodeBehind="OrderReview.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagName="PackageDetails" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/PackageDetails.ascx" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="OtherDetails" Src="~/ComplianceAdministration/UserControl/CustomAttributeLoader.ascx" %>
<%@ Register TagPrefix="uc" TagName="PersonAlias" Src="~/Shared/Controls/PersonAliasInfo.ascx" %>
<%@ Register TagPrefix="uc" TagName="PrivacyActNotification" Src="~/Shared/Controls/PrivacyActNotification.ascx" %>
<infs:WclResourceManagerProxy ID="rmpOrderReview" runat="server">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/OrderReview.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<style type="text/css">
    .sxRushOrder {
        padding-top: 0px !important;
    }

    .newMhdr {
        border-bottom-color: #adadad;
        border-bottom-style: solid;
        border-bottom-width: 0px;
        border-left-color: #adadad;
        border-left-style: solid;
        border-left-width: 0px;
        border-right-color: #adadad;
        border-left-style: solid;
        border-bottom-width: 0px;
        border-top-color: #adadad;
        border-top-style: solid;
        border-top-width: 0px;
        font-family: Helvetia, Arial, sans-serif;
        font-size: 150%;
        font-weight: 700;
        padding: 0 0 8px 0;
        margin: 0 !important;
        position: relative;
        word-spacing: 2px;
    }

    .sxcbar {
        overflow: visible;
    }

    #dvNextBtnStyleInSpanish.nextBtnStyleInSpanish .rbNext + .rbPrimary {
        padding-left: 0px !important;
    }
   
</style>
<script type="text/javascript">

    function stopColapse(sender, args) {
        $jQuery("#divMhdr").removeClass("mhdr");
        $jQuery("#divMhdr").addClass("newMhdr");
    }
    function openImageSliderPopup() {
        debugger
        var LocationId = $jQuery("[id$=hdnLocId]").val();
        var composeScreenWindowName = "Location Images";
        var popupHeight = $jQuery(window).height() * (100 / 100);
        var url = $page.url.create("~/FingerPrintSetUp/Pages/ImageViewer.aspx?LocationId=" + LocationId);
        var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
    }
    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        winopen = false;
    }

    function pageLoad() {
       // debugger;
        var LanguageCode = $jQuery("[id$=hdnLanguageCode]").val();
        if (LanguageCode == 'AAAA')
            $jQuery("[id$=dvNextBtnStyleInSpanish]").removeClass("nextBtnStyleInSpanish");
        if (LanguageCode == 'AAAB')
            $jQuery("[id$=dvNextBtnStyleInSpanish]").addClass("nextBtnStyleInSpanish");
    }

</script>
<asp:Panel ID="pnlMain" runat="server" Width="100%" Height="100%">
    <div class="section">
        <asp:HiddenField ID="hdnConfirmMsg" Value="<%$Resources:Language,CONFIRMMSG%>" runat="server" />
        <%--<h1 class="mhdr" id="h1" runat="server">Order Review: Please review your order details below. Make changes as necessary.--%>
        <h1 class="mhdr" id="headerText" runat="server"></h1>
        <div class="content">
            <infsu:PackageDetails ID="ucPackageDetails" runat="server"></infsu:PackageDetails>


            <div class="section">
                <div id="divMhdr" class="mhdr" style="position: relative; bottom: 2px;">
                    <%--<h1 style="font-size: 14px; padding-bottom: 2px;" > Profile Details </h1>--%>
                    <h1 style="font-size: 14px; padding-bottom: 2px;"><%= Resources.Language.PROFILEDETAILS %></h1>
                    <div style="right: 20px; position: absolute; z-index: 99999999999; bottom: 20px;">
                        <infsu:CommandBar ButtonPosition="Center" ID="cmdbarEditProfile" runat="server" DisplayButtons="Extra" ClientIDMode="Static" OnExtraClientClick="stopColapse"
                            ExtraButtonText="<%$Resources:Language,EDITPROFILE %>" AutoPostbackButtons="Extra" OnExtraClick="btnEditProfile_Click">
                        </infsu:CommandBar>
                    </div>
                </div>
                <%--<h1 class="mhdr">Personal Detail
            </h1>--%>
                <div class="content">
                    <div class="sxform auto">
                        <asp:Panel runat="server" CssClass="sxpnl" ID="Panel1">
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.FIRSTNAME%></span>    <%--<span class='cptn'>First Name</span>--%>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblFirstName" CssClass="ronly" runat="server"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.MIDDLENAME%></span>  <%--<span class='cptn'>Middle Name</span>--%>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblMiddleName" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.LASTNAME%></span>  <%-- <span class='cptn'>Last Name</span>--%>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblLastName" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co' id="dvpersonalAlias" runat="server" visible="false">
                                <uc:PersonAlias ID="ucPersonAlias" runat="server" Visible="true" IsReadOnly="true" IsLabelMode="true"></uc:PersonAlias>
                                <div class='sxroend'>
                                </div>
                                <%--<div class='sxlb'>
                                <span class='cptn'>Alias 1</span>
                            </div>
                            <div class='sxlm'>
                                <asp:Label ID="lblAlias1" runat="server" CssClass="ronly"></asp:Label>
                            </div>
                            <div class='sxlb'>
                                <span class='cptn'>Alias 2</span>
                            </div>
                            <div class='sxlm'>
                                <asp:Label ID="lblAlias2" runat="server" CssClass="ronly"></asp:Label>
                            </div>
                            <div class='sxlb'>
                                <span class='cptn'>Alias 3</span>
                            </div>
                            <div class='sxlm'>
                                <asp:Label ID="lblAlias3" runat="server" CssClass="ronly"></asp:Label>
                            </div>
                            <div class='sxroend'>
                            </div>--%>
                            </div>
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.GENDER%></span>  <%-- <span class='cptn'>Gender</span>--%>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblGender" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.DOB%></span>  <%--<span class='cptn'>Date of Birth</span>--%>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblDateOfBirth" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div runat="server" id="divSSN">
                                    <div class='sxlb'>
                                        <span class='cptn'><%=Resources.Language.SSN%></span>   <%-- <span class='cptn'>Social Security Number</span>--%>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblSSN" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.EMAILADD%></span>       <%--    <span class='cptn'>Email Address</span>--%>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblEmail" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.SECEMAIL%></span>     <%-- <span class='cptn'>Secondary Email Address</span>--%>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblSecondaryEmail" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.PHONENUM%></span><%--   <span class='cptn'>Phone Number</span>--%>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblPhone" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.SECPHONE%></span>    <%--   <span class='cptn'>Secondary Phone Number</span>--%>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblSecondaryPhone" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <asp:Label ID="lblAddress1Cptn" runat="server" class='cptn'></asp:Label>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblAddress1" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div runat="server" id="dvAddress2">
                                    <div class='sxlb'>
                                        <span class='cptn'><%=Resources.Language.ADDRESS2%></span>   <%-- <span class='cptn'>Address 2</span>--%>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblAddress2" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.COUNTRY%></span>  <%--<span class='cptn'>Country</span>--%>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblCountry" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.CITY%></span>    <%--   <span class='cptn'>City</span>--%>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblCity" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div runat="server" id="dvState">
                                    <div class='sxlb'>
                                        <span class='cptn'><%=Resources.Language.STATE%></span>  <%-- <span class='cptn'>State</span>--%>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblState" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>
                                        <asp:Label ID="lblShowZIPAndPostal" runat="server"></asp:Label></span>   <%--<span class='cptn'>Zip</span>--%>
                                </div>
                                    
                                  
                                <div class='sxlm'>
                                    <asp:Label ID="lblZip" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co' id="divInternationalCriminalSearchAttributes" runat="server" visible="false">
                                <div runat="server" id="divMothersName" visible="false">
                                    <div class='sxlb'>
                                        <span class='cptn'>Mother's Maiden Name</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblMotherName" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                </div>
                                <div id="divIdentificationNumber" runat="server" visible="false">
                                    <div class='sxlb'>
                                        <span class='cptn'>Identification Number</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblIdentificationNumber" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                </div>
                                <div id="divCriminalLicenseNumber" runat="server" visible="false">
                                    <div class='sxlb'>
                                        <span class='cptn'>Driver License Number</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblCriminalLicenseNumber" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co'>
                                <div id="dvMoveinDate" runat="server" visible="false">
                                    <div class='sxlb'>
                                        <span class='cptn'>Move in Date</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblResidingFrom" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                    <div class='sxlb'>
                                        <span class='cptn'>Residing To</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblResidingTo" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                </div>
                                <div id="dvDriverLicenseNo" runat="server" visible="false">
                                    <div class='sxlb'>
                                        <span class='cptn'>Driver License Number</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblDriverLiscence" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div id="dvDriverLicenseState" runat="server" visible="false">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class='cptn'>Driver License State</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblDriverLicenceState" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>

            <div id="residentialHistory" runat="server" visible="false"></div>

            <infsu:OtherDetails ID="caOtherDetails" runat="server" />
                        <%--Mailing Address Section--%>
            <div class="section" id="dvMailingAddress" runat="server" visible="false">
                <div id="dv1" class="mhdr" style="position: relative; bottom: 2px;">
                    <h1 style="font-size: 14px; padding-bottom: 2px; margin: 0px;"><%=Resources.Language.MAILINGADDRESS%></h1>
                    <div id="Div7" style="right: 20px; position: absolute; z-index: 99999999999; bottom: 5px;" runat="server">
                         <infsu:CommandBar ButtonPosition="Center" ID="cmdMailingAddress" runat="server" DisplayButtons="Extra" ClientIDMode="Static" OnExtraClientClick="stopColapse"
                            ExtraButtonText="<%$Resources:Language,EDITMILADD%>" AutoPostbackButtons="Extra" OnExtraClick="btnEditMailingAddress_Click">
                        </infsu:CommandBar>
                    </div>
                </div>
                <div class="content">
                    <div class="sxform auto">
                        <asp:Panel runat="server" CssClass="sxpnl" ID="Panel2">
                             <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.MAILINGOPTION%></span>  
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblMailingOption" CssClass="ronly" runat="server"></asp:Label>
                                </div>                        
                            </div>

                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.ADDRESS%></span>  
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblMailingAddress" CssClass="ronly" runat="server"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.CITY%></span>   
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblMailingCity" CssClass="ronly" runat="server"></asp:Label>
                                </div>
                                 <div class='sxlb'>
                                        <span class='cptn'><%=Resources.Language.COUNTRY%></span>   
                                    </div>
                                    <div class='sxlm' >
                                        <asp:Label ID="lblMailingCountry" CssClass="ronly" runat="server"></asp:Label>
                                    </div>
                               
                            </div>
                            <div class='sxro sx3co'>
                                <div ID="dvMailingState"  runat="server" visible="false">
                                    <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.STATE%></span>   
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblMailingState" CssClass="ronly" runat="server"></asp:Label>
                                </div>
                               </div>                            


                                 <div class='sxlb'>
                                    <span class='cptn'><asp:Label ID="lblShowZIPAndPostalCode" runat="server"></asp:Label></span> 
                                </div>

                                <div class='sxlm'>
                                    <asp:Label ID="lblMailingZipCode" CssClass="ronly" runat="server"></asp:Label>
                                </div>
                            </div>                            

                            <div class='sxro sx3co'>
                                <div class='sxlm' style="width: 100%">
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            
            <asp:HiddenField ID="hdnLocId" runat="server" />
            <asp:HiddenField ID="hdnIsLocTen" runat="server" />
            <%--<infsu:CommandBar ButtonPosition="Center" ID="cmdbarEditProfile" runat="server" DisplayButtons="Extra"
            ExtraButtonText="Edit Profile" AutoPostbackButtons="Extra" OnExtraClick="btnEditProfile_Click">
        </infsu:CommandBar>--%>
            <%--<div class="section">
            <h1 class="mhdr">
                Payment Detail
            </h1>
            <div class="content">
                <div class="sxform auto">
                    <asp:Panel runat="server" CssClass="sxpnl" ID="Panel2">
                        <div class='sxro sx2co' runat="server" id="divRushOrder">
                            <div runat="server" id="dvRushOrderSrvc">
                                <div class='sxlb'>
                                    <span class='cptn'>Rush Order Service</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:CheckBox ID="chkRushOrder" runat="server" AutoPostBack="true" OnCheckedChanged="chkRushOrder_CheckedChanged" />
                                </div>
                            </div>
                            <div id="divRush" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class='cptn'>Rush Order Price</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclNumericTextBox ShowSpinButtons="false" Type="Currency" Enabled="false" ID="txtRushOrderPrice"
                                        runat="server" MinValue="0" InvalidStyleDuration="100">
                                        <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                                    </infs:WclNumericTextBox>
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx2co'>
                            <div class='sxlb' id="dvPaymentTypelb" title="Select a payment method">
                                <span class='cptn'>Payment Type</span>
                            </div>
                            <div class='sxlm' id="dvPaymentTypelm">
                                <infs:WclComboBox ID="cmbPaymentModes" runat="server" DataTextField="Name" DataValueField="PaymentOptionID"
                                    AutoPostBack="true" OnSelectedIndexChanged="cmbPaymentModes_SelectedIndexChanged">
                                </infs:WclComboBox>
                            </div>
                            <div class='sxlb'>
                                <span class='cptn'>Total Price</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclNumericTextBox ShowSpinButtons="false" Type="Currency" ReadOnly="true" ID="txtTotalPrice"
                                    runat="server" MinValue="0" InvalidStyleDuration="100">
                                    <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                                </infs:WclNumericTextBox>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                       
                    </asp:Panel>
                </div>
            </div>
        </div>--%>
            <%--<infsu:OtherDetails ID="caOtherDetails" runat="server" />--%>
            <asp:Panel ID="pnlLoader" runat="server">
            </asp:Panel>
            <div class="section" id="dvAppointmentDetails" runat="server" visible="false">
                <div id="div1" class="mhdr" style="position: relative; bottom: 2px;">
                    <h1 style="font-size: 14px; padding-bottom: 2px; margin: 0px;"><%=Resources.Language.APPMNTDETAILS%></h1>
                    <%-- <h1 style="font-size: 14px; padding-bottom: 2px; margin: 0px;">Appointment Details</h1>--%>
                    <div id="dvChangeApp" style="right: 20px; position: absolute; z-index: 99999999999; bottom: 5px;" runat="server">
                        <infsu:CommandBar ButtonPosition="Center" ID="cmdAppointment" runat="server" DisplayButtons="Extra" ClientIDMode="Static" OnExtraClientClick="stopColapse"
                            ExtraButtonText="<%$Resources:Language,CHANGEAPPMNT%>" AutoPostbackButtons="Extra" OnExtraClick="btnEditAppointment_Click">
                            <%-- ExtraButtonText="Change Appointment" --%>
                        </infsu:CommandBar>
                    </div>
                </div>
                <div class="content">
                    <div class="sxform auto">
                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlAppointmentDetail">
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.NAME%></span>   <%-- <span class='cptn'>Name</span>--%>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblLocationName" CssClass="ronly" runat="server"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.APPMNTTIME%></span>   <%--   <span class='cptn'>Appointment Time</span>--%>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblAppointmentTiming" CssClass="ronly" runat="server"></asp:Label>
                                </div>
                                <div class='sxlm'>
                                    <asp:LinkButton ID="btnViewLocImage" runat="server" Text="<%$Resources:Language,VWLOCIMAGES%>" ToolTip="<%$Resources:Language,CLKTOVWLOCIMGS%>" OnClientClick="openImageSliderPopup(); return false;"></asp:LinkButton>
                                </div>
                            </div>
                            <div class='sxro sx3co'>
                                <div id="dvLocAdd" runat="server">
                                    <div class='sxlb'>
                                        <span class='cptn'><%=Resources.Language.ADDRESS%></span>      <%--<span class='cptn'>Address</span>--%>
                                    </div>
                                    <div class='sxlm' style="width: 18.4%">
                                        <asp:Label ID="lblLocationAddress" CssClass="ronly" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'><%=Resources.Language.DESCRIPTION%></span>      <%-- <span class='cptn'>Description</span>--%>
                                </div>

                                <div class='sxlm'>
                                    <asp:Label ID="lblSiteDescription" CssClass="ronly" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class='sxro sx3co'>
                                <div class='sxlm' style="width: 100%">
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>

            


            <div class="section" id="dvOutOfStateAppointmentDetails" runat="server" visible="false">
                <div class="mhdr" style="position: relative; bottom: 2px;">
                    <h1 style="font-size: 14px; padding-bottom: 2px; margin: 0px;"><%--<%=Resources.Language.APPMNTDETAILS %>--%></h1>
                    <%--<h1 style="font-size: 14px; padding-bottom: 2px; margin: 0px;">Appointment Details</h1>--%>
                </div>
                <div class="content">
                    <div class="sxform auto">
                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlOutOfStateAppointmentDetails">

                            <div class='sxro sx3co' id="Div2" runat="server">
                                <div class='sxlb'>
                                    <label id="Label4" runat="server" for="txtLocationAddress" class="cptn"><%= Resources.Language.APPMNTSTATUS%></label>
                                    <%-- <label id="Label4" runat="server" for="txtLocationAddress" class="cptn">Appointment Status</label>--%>
                                </div>
                                <%--Text="Mail in Processing"--%>
                                <div class='sxlm' style="width: 50%">
                                    
                                    <infs:WclTextBox runat="server" ClientIDMode="Static" ID="txtApptStatus" ReadOnly="true" Text="<% $Resources:Language, MAILINPROCESS %>" EnableAriaSupport="true" Width="100%">
                                    </infs:WclTextBox>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <%--3804--%>
            <div class="section" id="DivPrivacyActNotification" runat="server">
                <div class="mhdr" style="position: relative; bottom: 2px;">
                    <h1 style="font-size: 14px; padding-bottom: 2px; margin: 0px;"><%= Resources.Language.PRVCYACTSTMNT%></h1>
                </div>
                <div class="content">
                    <div class="sxform auto">
                        <uc:PrivacyActNotification ID="PrivacyActNotification" runat="server" Visible="true" IsReadOnly="true" IsLabelMode="true"></uc:PrivacyActNotification>
                    </div>
                </div>
                <div class='m4spn' style="padding: 0px !important; text-align: center">
                    <asp:CheckBox runat="server" ClientIDMode="Static" ID="chkAcceptPrivacy" Text="<%$Resources:Language,READANDACCPTPRVCYACTSTMNT%>" AutoPostBack="false" />
                    <div class='vldx'>
                        <asp:Label ID="lblValidationMsg" CssClass="lblValidationMsg errmsg" runat="server"> </asp:Label>
                    </div>
                </div>
            </div>


        </div>
        <div style="margin-top: 2px;" class="">
            <div id="dvNextBtnStyleInSpanish" class="nextBtnStyleInSpanish">
                <infsu:CommandBar ButtonPosition="Center" ID="cmdbarSubmit" runat="server" DisplayButtons="Submit,Clear" OnClearClientClick="ConfirmSubmit"
                    ClearButtonText="Continue" AutoPostbackButtons="Submit,Clear" OnClearClick="cmdbarSubmit_SubmitClick" 
                    ClearButtonIconClass="rbNext" SubmitButtonIconClass="rbPrevious"
                    OnSubmitClick="cmdbarSubmit_SaveClick" SubmitButtonText="Restart Order"
                    DefaultPanel="pnlMain" DefaultPanelButton="Clear">
                    <ExtraCommandButtons>
                        <infs:WclButton ID="btnCancelOrder" runat="server" Text="<%$Resources:Language,CNCL %>" UseSubmitBehavior="false" CssClass="margin-2 cancelposition"
                            AutoPostBack="true" OnClick="cmdbarSubmit_CancelClick">
                            <Icon PrimaryIconCssClass="rbCancel" />
                        </infs:WclButton>
                    </ExtraCommandButtons>
                </infsu:CommandBar>
            </div>
        </div>
    </div>
</asp:Panel>

<asp:HiddenField ID="hdnLanguageCode" Value="AAAA" runat="server" />
<asp:HiddenField ID="hdnPlsReadVldanMsg"  Value="<%$ Resources:Language, PLZREADUSERAGRMNT %>" runat="server" />
<asp:HiddenField ID="hdnIsAnyOptionsApprovalReq" runat="server" Value="" />
