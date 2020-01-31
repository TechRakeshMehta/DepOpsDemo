<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Shared/PopupMaster.master" CodeBehind="MapTableauReport.aspx.cs" Inherits="CoreWeb.IntsofSecurityModel.MapTableauReport" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/CommonControls/UserControl/PageBreadCrumb.ascx" TagName="PageBreadCrumb" TagPrefix="infsu" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
    <script type="text/javascript" src="~/Resources/Mod/IntsofSecurityModel/Scripts/MapTableauReport.js"></script>
    <script type="text/javascript">

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function returnToParent() {
            var viewValue = $jQuery('#<%=txtViewName.ClientID%>').val();
            var sheetValue = $jQuery('#<%=txtSheetName.ClientID%>').val();
            $jQuery('#<%=hdnCurrentNode.ClientID%>').val(viewValue + "," + sheetValue);

            if (viewValue != "" && sheetValue != "") {
                var hdnCurrentNode = $jQuery('#<%=hdnCurrentNode.ClientID%>').attr("value");
                var oArg = {};

                if (hdnCurrentNode != "" && hdnCurrentNode != undefined) {
                    var strControlName = hdnCurrentNode.toString().split(',');
                    oArg.controlName = "View=" + strControlName[0] + ";Sheet=" + strControlName[1];
                    oArg.NavigationUrl = "ReportsTableau" //strControlName[1];
                }

                //get a reference to the current RadWindow
                var oWnd = GetRadWindow();

                //Close the RadWindow and send the argument to the parent page
                if (oArg.controlName) {
                    oWnd.Close(oArg);
                }
            }
            else {
                //alert("Select enter tableau view and sheet value.");
                return false;
            }
        }

        // To close the popup.
        function ClosePopup() {
            //AD: Changing code to use latest lib function
            //parent.Page.closeWindow();    
            top.$window.get_radManager().getActiveWindow().close();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">
    <div class="section">
        <h1 class="mhdr">Report Details</h1>
        <div class="content">
            <input type="hidden" id="hdnCurrentNode" name="hdnSelectedNode" runat="server" />
            <div class="sxform auto">
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlMReportDetails">
                    <div class='sxro sx1co'>
                        <div class='sxlb'>
                            <span class='cptn'>
                                <asp:Label ID="lblView" runat="server" Text="Report View Name"></asp:Label></span><span class="reqd"> *</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtViewName"   CssClass="width" MaxLength="125" runat="server"></infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator Width="445" runat="server" ID="rfvviewName" ControlToValidate="txtViewName"
                                    Display="Dynamic" class="errmsg" ErrorMessage="Report view name is required." ValidationGroup='grpReport'
                                    Enabled="true" />
                            </div>
                        </div>
                    </div>
                    <div class='sxro sx1co'>
                        <div class='sxlb'>
                            <span class='cptn'>
                                <asp:Label ID="lblSheetName" runat="server" Text="Report Sheet Name"></asp:Label></span><span class="reqd"> *</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtSheetName"  CssClass="width" MaxLength="125" runat="server"></infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator Width="445" runat="server" ID="rfvtxtSheetName" ControlToValidate="txtSheetName"
                                    Display="Dynamic" class="errmsg" ErrorMessage="Report sheet name is required." ValidationGroup='grpReport'
                                    Enabled="true" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <infsu:CommandBar ID="btnCommandBar" runat="server" DisplayButtons="Save,Cancel"
                OnCancelClientClick="ClosePopup" ValidationGroup="grpReport" OnSaveClientClick="returnToParent"
                CancelButtonText="Close"
                ButtonPosition="Right" CauseValidationOnCancel="false" />
        </div>
    </div>
</asp:Content>
