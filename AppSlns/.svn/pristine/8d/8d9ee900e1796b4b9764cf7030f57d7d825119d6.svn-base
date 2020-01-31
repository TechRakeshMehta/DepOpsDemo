<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataEntryDocumentDiscardReason.aspx.cs" MasterPageFile="~/Shared/ChildPage.master"
    Inherits="CoreWeb.ComplianceOperations.Views.DataEntryDocumentDiscardReason" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="section">
        <div class="content">
            <div class="sxform auto">
                    <asp:Panel runat="server" CssClass="sxpnl" ID="pnlDiscardReason">
                        <div class='sxro sx2co'>
                            <div class='sxlb'>
                                <span class="cptn">Discard Reason</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclComboBox ID="ddlDiscardReason" runat="server" AutoPostBack="false"
                                    DataTextField="DDR_Name" DataValueField="DDR_ID"
                                    Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                                </infs:WclComboBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvDiscardReason" ControlToValidate="ddlDiscardReason"
                                        InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" ValidationGroup="vgDiscardReason"
                                        Text="Discard Reason is required." />
                                </div>

                            </div>

                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx2co'>
                            <div class='sxlb'>
                                <span class="cptn">Additional Notes</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclTextBox runat="server" ID="txtAdditionalNotes" TextMode="MultiLine" Height="50px" MaxLength="700">
                                </infs:WclTextBox>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <infsu:CommandBar ID="cmdBarDiscardReason" runat="server" GridMode="false" DefaultPanel="pnlDiscardReason"
                            DisplayButtons="Submit,Cancel" AutoPostbackButtons="Submit" 
                            SubmitButtonText="Continue" SubmitButtonIconClass="" OnSubmitClick="Continue_Click" 
                            CancelButtonText="Cancel" OnCancelClientClick="ClosePopup" CancelButtonIconClass="">
                        </infsu:CommandBar>
                    </asp:Panel>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnDocumentDiscardReasonId" runat="server" />
    <script type="text/javascript">
        function show_progress_OnSubmit() {
            Page.showProgress('Processing...');
        }

        //Function to close popup window
        function ClosePopup() {
            var oArg = {};
            oArg.Action = "Cancel";
            top.$window.get_radManager().getActiveWindow().close();
        }

        //Function to redirect to parent 
        function returnToParent() {
            var oArg = {};
            oArg.Action = "Submit";
            oArg.DiscardReasonId = $jQuery("[id$=hdnDocumentDiscardReasonId]")[0].value;
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
    </script>
</asp:Content>
