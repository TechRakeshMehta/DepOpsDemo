<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.CustomAttributeLoaderSearchMultipleNodes" CodeBehind="CustomAttributeLoaderSearchMultipleNodes.ascx.cs" %>
<%@ Register TagName="RowControl" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/CustomAttributeRowControlSearchMultipleNodes.ascx" %>
<%@ Register TagName="AttributeControl" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/CustomAttributeControlSearchMultipleNodes.ascx" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxBkgOrderSearchQueue">
    <infs:LinkedResource Path="~/Resources/Mod/Accessibility/Main-Accessibility.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div id='<%=this.ID %>'>
    <style type="text/css">
        .buttonHidden {
            display: none;
        }
    </style>
    <%-- <h6>
        <asp:Literal ID="litTitle" runat="server"></asp:Literal>
    </h6>--%>
    <div class="col-md-12">
        <div class="row">
            <div class='form-group col-md-12' title="Click the link and select a node to restrict search results to the selected node">
                <span class="cptn">Institution Hierarchy</span>
                <a href="#" id="instituteHierarchy" title="Click the link and select a node to restrict search results to the selected node" onclick="openINHPopUp('<%=this.ID %>');">Select Institution
                Hierarchy</a>&nbsp;&nbsp
            <asp:Label ID="lblinstituteHierarchy"  runat="server"></asp:Label>
            </div>
        </div>
    </div>
    <%--<infsu:InstituteHierarchy ID="ucInstituteHierarchy" runat="server" />--%>
    <div id="divForm" runat="server">
        <asp:Panel ID="pnlRows" runat="server">
        </asp:Panel>
    </div>
    <asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
    <asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
    <asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
    <asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
    <asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />
    <asp:HiddenField ID="hdnScreenType" runat="server" Value="" />
    <asp:HiddenField ID="hdnIsPagePostBack" runat="server" Value="" />
    <asp:HiddenField ID="hdnIsTreeHierarchyChanged" runat="server" Value="" />
    <script type="text/javascript" language="javascript">
        var winopen = false;
        var parentDiv;
        function SetFocusInstitutionLink() {
            //UAT-1923
            var hdnIsPagePostBack = $jQuery("[id$=hdnIsPagePostBack]");
            if (hdnIsPagePostBack.val() == "Focus Set") {
                setTimeout(function () { $jQuery("[id$=instituteHierarchy]").focus(); }, 100);
                hdnIsPagePostBack.val("");
            }
        }
        function openINHPopUp(divID) {
            var composeScreenWindowName = "Institution Hierarchy";
            //var tenantId = 2;
            parentDiv = $jQuery("[id$=" + divID + "]");
            var tenantId = $jQuery("[id$=hdnTenantId]", parentDiv).val();
            var screenType = $jQuery("[id$=hdnScreenType]", parentDiv).val();
            if (tenantId != "0" && tenantId != "") {
                //UAT-1923
                $jQuery("[id$=hdnIsPagePostBack]").val("Focus Set");
                $jQuery("[id$=instituteHierarchy]").focusout();
                //var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppng]", parentDiv).val();
                //UAT-2364
                //var popupHeight = (window.screen.height) * (30 / 100);
                var popupHeight = $jQuery(window).height() * (100 / 100);

                var DelemittedDeptPrgMapIds = $jQuery("[id$=hdnDepartmntPrgrmMppng]", parentDiv).val();
                var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&DelemittedDeptPrgMapIds=" + DelemittedDeptPrgMapIds + "&ScreenName=" + screenType);
                var win = $window.createPopup(url, { size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnINHClientClose });
                winopen = true;
                $jQuery("[id$=instituteHierarchy]").focusout();
            }
            else {
                $alert("Please select Institution.");
            }
            return false;
        }

        function OnINHClientClose(oWnd, args) {
            oWnd.remove_close(OnINHClientClose);
            if (winopen) {
                var arg = args.get_argument();
                if (arg) {
                    //debugger;
                    $jQuery("[id$=hdnDepartmntPrgrmMppng]", parentDiv).val(arg.DepPrgMappingId);
                    $jQuery("[id$=hdnHierarchyLabel]", parentDiv).val(arg.HierarchyLabel);
                    $jQuery("[id$=hdnInstitutionNodeId]", parentDiv).val(arg.InstitutionNodeId);
                    $jQuery("[id$=hdnIsTreeHierarchyChanged]", parentDiv).val(arg.IsHierarchyChanged);
                    $jQuery("[id$=btnDoPostBack]", parentDiv).click();
                    //__doPostBack("<%= btnDoPostBack.ClientID %>", "");
                }
                winopen = false;
                //UAT-1923
                setTimeout(function () { $jQuery("[id$=instituteHierarchy]").focus(); }, 100);
            }
        }


    </script>
</div>
