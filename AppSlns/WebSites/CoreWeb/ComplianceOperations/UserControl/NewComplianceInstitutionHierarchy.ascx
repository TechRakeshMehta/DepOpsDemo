<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceOperations.Views.ComplianceInstitutionHierarchy"
    CodeBehind="ComplianceInstitutionHierarchy.ascx.cs" %>
<%@ Register TagName="RowControl" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/NewCustomAttributeRowControlSearch.ascx" %>
<%@ Register TagName="AttributeControl" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/NewCustomAttributeControlSearch.ascx" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<style type="text/css">
    .buttonHidden
    {
        display: none;
    }
</style>
<div class="col-md-12">
    <div class="row">
        <span class="cptn">Institution Hierarchy</span>
        <a href="#" id="instituteHierarchy" onclick="openPopUp();">Select Institute Hierarchy</a>&nbsp;&nbsp
        <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
    </div> 
</div>
<asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
<asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
<asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
<asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />
<asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
<script type="text/javascript" language="javascript">
    function openPopUp() {
        var composeScreenWindowName = "Institution Hierarchy";
        var tenantId = $jQuery("[id$=hdnTenantId]").val();
        if (tenantId != "0" && tenantId != "") {
            var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionHierarchyList.aspx?TenantId=" + tenantId + "&DepartmentProgramId=" + DepartmentProgramId);
            var win = $window.createPopup(url, { size: "600,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
        }
        else {
            alert("Please Select Institute.");
        }
    }

    function OnClientClose(oWnd, args) {
        //remove the handler on window close.
        oWnd.remove_close(OnClientClose);
        //get the transferred arguments
        var arg = args.get_argument();
        if (arg) {
            $jQuery("[id$=hdnDepartmntPrgrmMppng]").val(arg.DepPrgMappingId);
            $jQuery("[id$=hdnHierarchyLabel]").val(arg.HierarchyLabel);
            $jQuery("[id$=hdnInstitutionNodeId]").val(arg.InstitutionNodeId);
            $jQuery("[id$=btnDoPostBack]").click();

        }
    }

    function Refresh() {
        $jQuery("[id$=btnDoPostBack]").click();
    }
</script>


