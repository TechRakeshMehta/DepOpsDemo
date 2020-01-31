<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.CustomAttributeLoaderSearch" Codebehind="CustomAttributeLoaderSearch.ascx.cs" %>
<%@ Register TagName="RowControl" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/CustomAttributeRowControlSearch.ascx" %>
<%@ Register TagName="AttributeControl" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/CustomAttributeControlSearch.ascx" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div id='<%=this.ID %>'>
    <style type="text/css">
        .buttonHidden
        {
            display: none;
        }
    </style>
    <h6>
        <asp:Literal ID="litTitle" runat="server"></asp:Literal>
    </h6>
    <div class='sxro sx3co'>
        <div class='sxlb' title="Click the link and select a node to restrict search results to the selected node">
            <span class="cptn">Institution Hierarchy</span>
        </div>
        <div class='sxlm m2spn'>
            <a href="#" id="instituteHierarchy" onclick="openPopUp('<%=this.ID %>');">Select Institution
                Hierarchy</a>&nbsp;&nbsp
            <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
        </div>
        <div class='sxroend'>
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
    <script type="text/javascript" language="javascript">
        var winopen = false;
        var parentDiv;
        function openPopUp(divID) { 
            var composeScreenWindowName = "Institution Hierarchy";
            //var tenantId = 2;
            parentDiv = $jQuery("[id$=" + divID + "]");
            var tenantId = $jQuery("[id$=hdnTenantId]", parentDiv).val();
            if (tenantId != "0" && tenantId != "") {
                var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppng]", parentDiv).val();
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
                    $jQuery("[id$=hdnDepartmntPrgrmMppng]", parentDiv).val(arg.DepPrgMappingId);
                    $jQuery("[id$=hdnHierarchyLabel]", parentDiv).val(arg.HierarchyLabel);
                    $jQuery("[id$=hdnInstitutionNodeId]", parentDiv).val(arg.InstitutionNodeId);
                    $jQuery("[id$=btnDoPostBack]", parentDiv).click();
                    //__doPostBack("<%= btnDoPostBack.ClientID %>", "");
                }
                winopen = false;
            }
        }

   
    </script>
</div>
