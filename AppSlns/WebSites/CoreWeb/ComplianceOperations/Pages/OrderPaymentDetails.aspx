<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Shared/PopupMaster.master" CodeBehind="OrderPaymentDetails.aspx.cs" Inherits="CoreWeb.ComplianceOperations.Pages.OrderPaymentDetails" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="uc" TagName="ScheduleLocationUpdateControl" Src="~/FingerPrintSetUp/UserControl/AppointmentScheduleLocationUpdate.ascx" %>
<%@ Register TagPrefix="uc" TagName="AppointmentRescheduler" Src="~/FingerPrintSetUp/UserControl/AppointmentReschedulerOld.ascx" %>
<%@ Register TagPrefix="uc" TagName="OrderPaymentDetails" Src="~/ComplianceOperations/UserControl/OrderPaymentDetails.ascx" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
     <style type="text/css">
         .languageTextAlign input {
            text-align:right !important;
         }
     </style>
</asp:Content>
   
<asp:Content ID="Content2" style="height: 80%" ContentPlaceHolderID="PoupContent" runat="server">
   
    
    <%-- <infs:WclResourceManagerProxy runat="server" ID="rprxClinicalRotationMappingPopup">
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="../Resources/Generic/popup.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="../Resources/Mod/ClinicalRotation/ManageRotation.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>--%>

    <%--  <script type="text/javascript">

        $page.add_pageLoad(function () {
            var $ = $jQuery;
            $(".grdCmdBar .RadButton").each(function () {
                if ($(this).text().toLowerCase() == "add new rotation") {
                    $(this).attr("title", "Click to add a new rotation");
                }
            });
        });

    </script>--%> 
  

    <script type="text/javascript">

   function returnToParent() {
            var oArg = {};
            oArg.Action = "Submit";
            var oWnd = GetRadWindow();
            oWnd.Close(oArg);
        }

        //function to get current popup window
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function EnableSubmitOrder() {
            var btnSubmitOrder = $find("<%= btnSelectAppointment.ClientID %>");
            if (btnSubmitOrder != null)
                btnSubmitOrder.set_enabled(true);
        }
          function cmdBarStartOrder_Click() {
            var btnSubmitOrder = $find("<%= btnSelectAppointment.ClientID %>");
            btnSubmitOrder.click();
        }
</script>
   
 <div class="msgbox" id="dvMsgBox" runat="server">
        <asp:Label ID="lblErrorMessage" runat="server" CssClass="error"></asp:Label>
        <asp:Label ID="lblSuccessMessage" runat="server" CssClass="sucs"></asp:Label>
        <asp:Label ID="lblInfoMessage" runat="server" CssClass="info"> </asp:Label>
    </div>
    <div class='sxro sx3co' id="dvUCAppointmentRescheduler" visible="false" runat="server">
        <uc:AppointmentRescheduler ID="ucAppointmentRescheduler" runat="server" />
    </div>
    <div class='sxro sx3co' id="divUCScheduleLocationUpdateControl" runat="server" style="display: block">
        <uc:ScheduleLocationUpdateControl runat="server" ID="ScheduleLocationUpdateControl" />
    </div>

   
    <asp:HiddenField ID="hdnLocId" runat="server" />
   
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CommandContent" runat="server">
    <div id="dvAppointmentButtons" style="width: 100%; padding-top: 10px; text-align: center; bottom:35px;  position: relative;" runat="server" visible="false">
        <infs:WclButton runat="server" ID="btnSelectAppointment" CssClass="padRight2 marginTop30" Icon-PrimaryIconCssClass="rbNext" Visible="true"
            Text="<%$ Resources:Language, NEXT %> " OnClick="btnSelectAppointment_Click">
        </infs:WclButton>
        <infs:WclButton runat="server" ID="btnSaveAppointment" CssClass="padRight2 marginTop30 languageTextAlign" Icon-PrimaryIconCssClass="rbSave" Visible="false"
            Text="<%$ Resources:Language, SAVE %>" OnClick="btnSaveAppointment_Click">
        </infs:WclButton>
        <infs:WclButton runat="server" ID="btnCancel" CssClass="marginTop30 languageTextAlign" Icon-PrimaryIconCssClass="rbCancel" OnClick="btnCancel_Click"
            Text="<%$ Resources:Language, CNCL %>">
            <%--OnClientClicked="returnToParent"--%>
        </infs:WclButton>
         <infs:WclButton runat="server" ID="btnGoBack" CssClass="padRight2 marginTop30"  Visible="false"
            Text="<%$ Resources:Language, GOBCKTODSHBRD %> " OnClick="btnCancel_Click">
        </infs:WclButton>
       
    </div>
</asp:Content>


  
   