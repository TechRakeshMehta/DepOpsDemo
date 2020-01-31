<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.ShotSeriesInfo"
    Title="ShotSeriesInfo" MasterPageFile="~/Shared/ChildPage.master" CodeBehind="ShotSeriesInfo.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/ContentEditor.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <style type="text/css">
        .reEditorModes a {
            display: none;
        }

        .reToolZone {
            display: none;
        }
    </style>
    <script type="text/javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
        });

        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }

        function ValidateSeriesDescritptionLength(sender, args) {
            var maxContentLength = 1000;
            var editor = $jQuery("[id$=rdEditorDescription]")[0];
            text = editor.control.get_text();
            text = text.replace(/(?:\\[rn]|[\r\n]+)+/g, "");
            var textLength = text.length;
            if (text != "" && textLength > maxContentLength)
                return args.IsValid = false;
            else
                return args.IsValid = true;
        }

        function ValidateSeriesDetailsLength(sender, args) {
            var maxContentLength = 1000;
            var editor = $jQuery("[id$=rdEditorDetails]")[0];
            text = editor.control.get_text();
            text = text.replace(/(?:\\[rn]|[\r\n]+)+/g, "");
            var textLength = text.length;
            if (text != "" && textLength > maxContentLength)
                return args.IsValid = false;
            else
                return args.IsValid = true;
        }

        var winopen = false;
        function OpenSeriesShuffleTestPopup() {
            parent.ResetTimer();
            var popupWindowName = "Shot Series Shuffle Test";
            var hdnSelectedTenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
            var hdnCategoryID = $jQuery("[id$=hdnCategoryID]").val();
            var hdnSeriesID = $jQuery("[id$=hdnSeriesID]").val();
            winopen = true;
            var width = (window.screen.width) * (90 / 100);
            var height = (window.screen.height) * (80 / 100);
            var popupsize = width + ',' + height;
            var url = $page.url.create("ShotSeriesShuffleTest.aspx?SelectedTenantId=" + hdnSelectedTenantID + "&Id=" + hdnSeriesID + "&CatId=" + hdnCategoryID);
            var win = $window.createPopup(url, { size: popupsize, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName, onclose: OnClientClose }
               );
            return false;
        }

        function OnClientClose(oWnd, args) {
            oWnd.get_contentFrame().src = ''; //This is added for fixing pop-up close issue in Safari browser.
            oWnd.remove_close(OnClientClose);
            if (winopen) {
                winopen = false;
            }
        }

    </script>
    <asp:HiddenField ID="hdnSelectedTenantID" runat="server" />
    <asp:HiddenField ID="hdnCategoryID" runat="server" />
    <asp:HiddenField ID="hdnSeriesID" runat="server" />
    <div class="page_cmd">
        &nbsp;
    </div>
    <div class="section">

        <h1 class="mhdr">Series Information</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="lblSuccess" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlSeries">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Series Name</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtSeriesName" MaxLength="100">
                            </infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvSeriesName" ControlToValidate="txtSeriesName"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Series Name is required." />
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Series Label</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtSeriesLabel" MaxLength="100">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Is Active</span>
                        </div>
                        <div class='sxlm'>
                            <uc1:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="true" IsAutoPostBack="false" />
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>

                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Allow entry after all items are approved</span>
                        </div>
                        <div class='sxlm'>
                            <asp:RadioButtonList ID="rbtnAlloEntry" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Rule Execution Order</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclNumericTextBox ShowSpinButtons="false" Type="Number" ID="txtExecutionOrder" MinValue="1"
                                MaxValue="99" runat="server" InvalidStyleDuration="100" NumberFormat-DecimalDigits="0">
                            </infs:WclNumericTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>

                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Description</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <infs:WclEditor ID="rdEditorDescription" ClientIDMode="Static" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml" Width="99.3%" EnableResize="false"
                                Height="150px">
                            </infs:WclEditor>
                            <div class='vldx'>
                                <asp:CustomValidator runat="server" ID="cstValEditorDescription" ControlToValidate="rdEditorDescription" ClientValidationFunction="ValidateSeriesDescritptionLength" ValidationGroup="grpFormSubmit"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Please don't enter more than 1000 characters." />
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Details</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <infs:WclEditor ID="rdEditorDetails" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml" Width="99.3%" EnableResize="false"
                                Height="150px">
                            </infs:WclEditor>
                            <div class='vldx'>
                                <asp:CustomValidator runat="server" ID="cstValEditorDetails" ControlToValidate="rdEditorDetails" ClientValidationFunction="ValidateSeriesDetailsLength" ValidationGroup="grpFormSubmit"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Please don't enter more than 1000 characters." />
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div>
                <div style="float: right;">
                    <infsu:CommandBar ID="fsucCmdBarCat" runat="server" DefaultPanel="pnlSeries" SaveButtonText="Save"
                        SubmitButtonText="Edit" SubmitButtonIconClass="rbEdit" OnSaveClick="fsucCmdBarCat_SaveClick" ExtraButtonIconClass="rbOk"
                        ExtraButtonText="Test Shuffling" OnExtraClientClick="OpenSeriesShuffleTestPopup"
                        OnSubmitClick="fsucCmdBarCat_SubmitClick" OnCancelClick="fsucCmdBarCat_CancelClick" ValidationGroup="grpFormSubmit"
                        AutoPostbackButtons="Submit,Save,Cancel">
                    </infsu:CommandBar>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
