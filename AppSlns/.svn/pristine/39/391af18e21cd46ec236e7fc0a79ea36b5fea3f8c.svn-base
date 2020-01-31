<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RotationCustomAttributes.aspx.cs" Inherits="CoreWeb.ClinicalRotation.Pages.RotationCustomAttributes" %>

<!DOCTYPE html>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register Src="~/ClinicalRotation/UserControl/SharedUserCustomAttributeForm.ascx" TagPrefix="uc" TagName="CustomAttributes" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="overflow-y:auto">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scmMain" runat="server" ScriptMode="Release">
        </asp:ScriptManager>
        <infs:WclResourceManager ID="WclResourceManager" runat="server"></infs:WclResourceManager>
        <infs:WclResourceManagerProxy runat="server" ID="rprxRotationCustomAttribute">
            <infs:LinkedResource Path="../../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="../../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="../../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
        </infs:WclResourceManagerProxy>
        <div class="container-fluid" id="dvCustomAttributes">
            <div class="row">&nbsp;</div>
            <div class="row bgLightGreen" style="overflow-y:auto">
                <div class="col-md-12">
                    <div class="col-md-12">&nbsp;</div>
                    <asp:Panel ID="pnlRotationCustomAttribues" runat="server">
                    </asp:Panel>
                </div>
                <div class="col-md-12">
                    <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                        AutoPostbackButtons="Submit,Save" SubmitButtonIconClass="rbReset"
                        SubmitButtonText="Reset" SaveButtonText="Apply" SaveButtonIconClass="rbSearch"
                        CancelButtonText="Close" OnSubmitClick="fsucCmdBarButton_SubmitClick" OnSaveClick="fsucCmdBarButton_SaveClick"
                        UseAutoSkinMode="False" ButtonSkin="Silk">
                    </infsu:CommandBar>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnSearch" runat="server" Value="0" />
    </form>
    <script type="text/javascript">

       

        $jQuery(document).ready(function () {
            $jQuery("#dvCustomAttributes [id$='btnCancel']").on('click', function () {
                //debugger;
                //var values = [];
                //var IsValuesNull = true; //UAT-3165
                //$('input.riTextBox').each(function () { //UAT-3165
                //    if ($(this).val() != '' && $(this).val() != 'Select a date') {
                //        IsValuesNull = false;
                //    }
                //});
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;

                var oArg = {};
               // oArg.IsValuesNull = IsValuesNull;
                oWindow.Close(oArg);
            });
        });      
    </script>
</body>

</html>
