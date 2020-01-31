<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BkgPackageCopy.aspx.cs" Title="Package Copy" MasterPageFile="~/Shared/PopupMaster.master"
    Inherits="CoreWeb.BkgSetup.Views.BkgPackageCopy" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">

    <script type="text/javascript">

        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function ReturnToParent() {
            //get a reference to the current RadWindow
            var oWnd = GetRadWindow();
            oWnd.Close();
        }

        //Show Progress Bar
        function ShowProgressBar() {
            if ($jQuery('[id$=rfvPackageName]')[0].isvalid)
                Page.showProgress('Processing...');
        }

        function openPopUp() {
            var composeScreenWindowName = "Institution Hierarchy";
            //var tenantId = 2;
            var tenantId = $jQuery("[id$=hdnTenantId]").val();
            if (tenantId != "0" && tenantId != "") {
                var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();
                //UAT-2364
                var popupHeight = $jQuery(window).height() * (100 / 100);

                var url = $page.url.create("~/ComplianceOperations/Pages/InstitutionHierarchyList.aspx?TenantId=" + tenantId + "&DepartmentProgramId=" + DepartmentProgramId);
                var win = $window.createPopup(url, { size: "500,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
                winopen = true;
            }
            else {
                $alert("Please select Institution.");
            }
            return false;
        }

        function OnClientClose(oWnd, args) {
            oWnd.remove_close(OnClientClose);
            if (winopen) {
                var arg = args.get_argument();
                if (arg) {
                    $jQuery("[id$=hdnDepartmntPrgrmMppng]").val(arg.DepPrgMappingId);
                    $jQuery("[id$=hdnHierarchyLabel]").val(arg.HierarchyLabel);
                    $jQuery("[id$=hdnInstitutionNodeId]").val(arg.InstitutionNodeId);
                    __doPostBack("<%= btnDoPostBack.ClientID %>", "");
                }
                winopen = false;
            }
        }
    </script>
    <asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
    <asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
    <asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
    <asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
    <asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />
    <div class="section" id="divPackageName" runat="server">
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
                <asp:Panel ID="pnlPackageName" CssClass="sxpnl" runat="server">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Package Name</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <infs:WclTextBox ID="txtPackageName" runat="server" MaxLength="100">
                            </infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvPackageName" ControlToValidate="txtPackageName"
                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Package Name is required." />
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb' title="Click the link and select a node to restrict search results to the selected node">
                            <span class='cptn'>Institution Hierarchy</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <a href="#" id="instituteHierarchy" onclick="openPopUp();">Select Institution Hierarchy</a>&nbsp;&nbsp
                        <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div id="divButton" runat="server">
                <infsu:CommandBar ID="fsucCmdBarCopyPkg" runat="server" DefaultPanel="pnlPackageName" 
                    DisplayButtons="Save,Cancel" AutoPostbackButtons="Save" OnSaveClick="fsucCmdBarCopyPkg_SaveClick" OnCancelClientClick="ReturnToParent" CancelButtonText="Close"
                    ValidationGroup="grpFormSubmit">
                </infsu:CommandBar>
            </div>
        </div>
    </div>
</asp:Content>
