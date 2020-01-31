<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PackageSelectionForDataEntry.aspx.cs" MasterPageFile="~/Shared/ChildPage.master" Inherits="CoreWeb.ComplianceOperations.Pages.Views.PackageSelectionForDataEntry" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>
        <asp:Literal ID="litMsg" runat="server"></asp:Literal></h3>
    <asp:Panel ID="pnlPackageSubscription" runat="server">
        <h1 class="mhdr">Package Subscription</h1>
        <div class="divPackageSubscriptions">
            <div class="content">
                <div class="sxform auto">
                    <asp:Repeater ID="rptPackageSubscription" runat="server">
                        <ItemTemplate>
                            <div class="content">
                                <div class="sxform auto">
                                    <asp:Panel ID="Panel3" CssClass="sxpnl" runat="server">
                                        <div class='sxro sx1co'>

                                            <div class='sxlb newclass'>
                                                <asp:RadioButton ID="rbtnSubscription" Text='<%# Eval("PackageName") %>' runat="server" onclick="ManageSelection(this)" />
                                                <asp:HiddenField ID="hdnPackageSubscriptionID" Value='<%# Eval("PackageSubscriptionID") %>' runat="server" />

                                            </div>
                                            <div class='sxlm'>
                                                <div id="dvElement" runat="server" style="display: inline-block; word-break: break-all; word-wrap: break-word">

                                                    <span class="para"><%# Eval("InstitutionHierarchy") %></span>
                                                </div>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </asp:Panel>
    <div id="divNextDocumentButtons" runat="server" visible="false">
        <infsu:CommandBar ID="cmdBarMultipleSubscriptions" runat="server" GridMode="false" DefaultPanel="pnlMultipleSubscriptions"
            DisplayButtons="Submit,Cancel" AutoPostbackButtons="Submit,Cancel"
            SubmitButtonText="Ok" SubmitButtonIconClass="" OnSubmitClick="btnOk_Click" OnSubmitClientClick="show_progress_OnSubmit"
            CancelButtonText="Back to Queue" OnCancelClientClick="ClosePopup">
        </infsu:CommandBar>
    </div>
    <div id="divSameDocumentButtons" runat="server" visible="false">
        <infsu:CommandBar ID="cmdSameDocument" runat="server" DisplayButtons="Submit,Save,Cancel"
            CancelButtonText="Back to Queue" AutoPostbackButtons="Submit,Save"
            SaveButtonText="Ok" OnSaveClick="cmdSameDocument_SaveClick" OnCancelClientClick="ClosePopup"
            SubmitButtonText="Complete and Go to Next Document" OnSubmitClick="cmdSameDocument_SubmitClick">
        </infsu:CommandBar>
    </div>
    <asp:HiddenField ID="hdnSubscriptionID" runat="server" />
    <asp:HiddenField ID="hdnApplicantId" runat="server" />
    <asp:HiddenField ID="hdnTenantId" runat="server" />
    <asp:HiddenField ID="hdnIsDocChange" runat="server" />

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
            oArg.PackageSubscriptionID = $jQuery("[id$=hdnSubscriptionID]")[0].value;
            oArg.ApplicantId = $jQuery("[id$=hdnApplicantId]")[0].value;
            oArg.TenantId = $jQuery("[id$=hdnTenantId]")[0].value;

            oArg.IsDocChange = $jQuery("[id$=hdnIsDocChange]")[0].value;
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

        function ManageSelection(currentSelection) {
            var divPackageSubscriptions = $jQuery(".divPackageSubscriptions");

            $jQuery('.divPackageSubscriptions input:radio').each(function (index) {

                if ($jQuery(this).attr("id") != $jQuery(currentSelection).attr("id")) {
                    $jQuery(this).attr('checked', false);
                }
            });
        }

    </script>

</asp:Content>
