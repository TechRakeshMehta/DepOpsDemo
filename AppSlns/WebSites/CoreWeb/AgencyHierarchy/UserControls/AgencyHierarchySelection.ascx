<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgencyHierarchySelection.ascx.cs" Inherits="CoreWeb.AgencyHierarchy.Views.AgencyHierarchySelection" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<%--<infsu:WCLresourcemanagerproxy runat="server">
    <infs:LinkedResource Path="../Resources/Mod/AgencyHierarchy/AgencyHierarchyMultipleSelection.js" ResourceType="JavaScript" />
    </infsu:WCLresourcemanagerproxy>--%>

<div id='<%=this.ID %>'>
    <style type="text/css">
        .buttonHidden {
            display: none;
        }

        .disabled {
            cursor: default !important;
            text-decoration: none;
        }
    </style>
    <infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
        <infs:LinkedResource Path="../Resources/Mod/AgencyHierarchy/AgencyHierarchySelection.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <div class="col-md-12">
        <div class="row">
            <span class="cptn">Agency Hierarchy</span><span id="spnAgencyHierarchy" class="reqd buttonHidden">*</span>
            <a href="javascript:void(0);" id="agencyHierarchy1" onclick="openPopUp(('<%=this.ID %>'));" style="color: blue">Select Agency Hierarchy</a>&nbsp;&nbsp
            <asp:Label ID="lblAgencyHierarchy" runat="server"></asp:Label>
        </div>
    </div>
    <asp:HiddenField ID="hdnNodeId" runat="server" Value="" />
    <asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
    <asp:HiddenField ID="hdnAgencyNodeId" runat="server" Value="" />
    <asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
    <asp:HiddenField ID="hdnAgencyHierarchyNodeIds" runat="server" Value="" />
    <asp:HiddenField ID="hdnInstitutionNodeIds" runat="server" Value="" />
    <asp:HiddenField ID="hdnAgencyName" runat="server" Value="" />
    <asp:HiddenField ID="hdnselectedRootNodeId" runat="server" Value="" />
    <asp:HiddenField ID="hdnReadOnlyMode" runat="server" Value="0" />
    <asp:HiddenField ID="hdnIsInstitutionHierarchyRequired" runat="server" Value="false" />
    <asp:HiddenField ID="hdnlstTenantIds" runat="server" /> 
    <asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />

    <script type="text/javascript" lang="javascript">
        var parentDiv;
        function openPopUp(divID) {
            //if ($jQuery("[id$=hdnValidateFileUploadControl]").length > 0) {
            //    $jQuery("[id$=hdnValidateFileUploadControl]").val("false");
            //}
            //debugger;
            var composeScreenWindowName = "Agency Hierarchy";
            parentDiv = $jQuery("[id$=" + divID + "]");

            if (parseInt($jQuery("[id$=hdnReadOnlyMode]", parentDiv).val()) == 1) {
                return;
            }

            var tenantId = $jQuery("[id$=hdnTenantId]", parentDiv).val();
            //UAT-3245
            var lstTenantIds = $jQuery("[id$=hdnlstTenantIds]", parentDiv).val();

            if ((tenantId != "0" && tenantId != "") || lstTenantIds != null && lstTenantIds != undefined && lstTenantIds != "") {
                var agencyHierarchyNodeIds = $jQuery("[id$=AgencyHierarchyNodeIds]", parentDiv).val();
                var agencyHierarchyRootNodeId = $jQuery("[id$=hdnselectedRootNodeId]", parentDiv).val();
                var agencyId = $jQuery("[id$=hdnAgencyNodeId]", parentDiv).val();
                var isInstitutionRequired = $jQuery("[id$=hdnIsInstitutionHierarchyRequired]", parentDiv).val();
                var institutionNodeIds = $jQuery("[id$=hdnInstitutionNodeIds]", parentDiv).val();

                if (isInstitutionRequired == "True" && institutionNodeIds == "") {
                    $alert("Please Select Institution Hierarchy.");
                    return;
                }

                var popupHeight = $jQuery(window).height() * (100 / 100);
                var url = $page.url.create("~/AgencyHierarchy/Pages/AgencyHierarchyList.aspx?TenantId=" + tenantId + "&AgencyHierarchyNodeIds=" + agencyHierarchyNodeIds + "&AgencyId=" + agencyId + "&InstitutionNodeIds=" + institutionNodeIds + "&AgencyHierarchyRootNodeId=" + agencyHierarchyRootNodeId + "&lstTenantIds=" + lstTenantIds);
                var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
            }
            else {
                $alert("Please Select Institution.");
            }
        }

        function OnClientClose(oWnd, args) {
            //remove the handler on window close.
            //debugger;
            oWnd.remove_close(OnClientClose);
            //get the transferred arguments
            var arg = args.get_argument();
            if (arg) {
                $jQuery("[id$=hdnNodeId]", parentDiv).val(arg.NodeId);
                $jQuery("[id$=hdnHierarchyLabel]", parentDiv).val(arg.HierarchyLabel);
                $jQuery("[id$=hdnAgencyNodeId]", parentDiv).val(arg.AgencyId);
                $jQuery("[id$=hdnAgencyName]", parentDiv).val(arg.AgencyName);
                $jQuery("[id$=hdnselectedRootNodeId]", parentDiv).val(arg.RootNodeId);
                $jQuery("[id$=btnDoPostBack]", parentDiv).click();


                if (typeof CheckPreceptorRequiredForAgency != 'undefined' && $jQuery.isFunction(CheckPreceptorRequiredForAgency)) {
                    CheckPreceptorRequiredForAgency($jQuery("[id$=hdnTenantId]", parentDiv).val(), arg.AgencyId);
                }
                //UAT-2712
                if (typeof GetRotationRequiredFieldOptions != 'undefined' && $jQuery.isFunction(GetRotationRequiredFieldOptions)) {
                    GetRotationRequiredFieldOptions($jQuery("[id$=hdnTenantId]", parentDiv).val(), arg.NodeId);
                }

                if (typeof BindInstitutionLabel != 'undefined' && $jQuery.isFunction(BindInstitutionLabel)) {
                    BindInstitutionLabel();
                }
                //UAT-2784
                //if (typeof ShowHideExpirationCriteria != 'undefined' && $jQuery.isFunction(ShowHideExpirationCriteria)) {
                //    ShowHideExpirationCriteria();
                //}
            }
        }

    </script>
</div>
