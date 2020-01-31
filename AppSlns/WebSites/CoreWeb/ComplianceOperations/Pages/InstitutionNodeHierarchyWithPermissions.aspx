<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstitutionNodeHierarchyWithPermissions.aspx.cs" Inherits="CoreWeb.ComplianceOperations.Views.InstitutionNodeHierarchyWithPermissions"
    Title="InstitutionHierarchyListWithPermissions" %>

<%@ Register Src="~/CommonControls/UserControl/PageBreadCrumb.ascx" TagName="PageBreadCrumb"
    TagPrefix="infsu" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Institution Hierarchy </title>
    <style type="text/css">
        html {
            width: auto !important;
            height: auto !important;
            overflow: auto !important;
        }

        #treeDepartment .rtIn {
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="formInstHierarchy" runat="server">
        <asp:ScriptManager ID="scmMain" runat="server" ScriptMode="Release">
        </asp:ScriptManager>
        <infs:WclResourceManager ID="InstHierarchyListManager" runat="server">
            <%--<infs:LinkedResource Path="~/Resources/Generic/popup.css" ResourceType="StyleSheet" />--%>
            <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
            <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/InstitutionHierarchyList.js" ResourceType="JavaScript" />
            <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
        </infs:WclResourceManager>
        <div style="height: 93%; padding-bottom: 50px">
            <div class="col-md-12" id="divDept">
                <div class="row">
                    <%--Change this if resize the Popup Dimentions--%>
                    <infs:WclTreeView ID="treeDepartment" runat="server" ClientKey="treeDepartment" OnNodeDataBound="treeDepartment_NodeDataBound" Skin="Silk" EnableAriaSupport="true" OnClientNodeCollapsed="clientNodeCollapsed"
                        AutoSkinMode="false" CheckBoxes="true" OnClientNodeClicked="GetSelectedNode" OnClientNodeChecked="GetCheckedNode" OnClientContextMenuShowing="ClientContextMenuShowing">
                        <ContextMenus>
                            <telerik:RadTreeViewContextMenu ID="mnuTreeCategory" runat="server">
                                <Items>
                                    <telerik:RadMenuItem Value="MenuItem1" Text="MenuItem1">
                                    </telerik:RadMenuItem>
                                </Items>
                            </telerik:RadTreeViewContextMenu>
                        </ContextMenus>
                        <DataBindings>
                            <telerik:RadTreeNodeBinding Expanded="True" />
                        </DataBindings>
                    </infs:WclTreeView>
                </div>
            </div>
        </div>
        <div style="width: 100%; z-index: 10; position: fixed; right: 0; bottom: 0;">
            <div class="col-md-12">
                <div class="row text-right" style="background-color: white; border-width: 1px; padding: 3px">
                    <%--Change this if resize the Popup Dimentions--%>
                    <infsu:CommandBar ID="fsucInstitutionHierarchyList" runat="server" DisplayButtons="Save,Extra,Cancel,Submit"
                        AutoPostbackButtons="Save" OnCancelClientClick="ClosePopup" OnSaveClick="btnOk_Click"
                        OnExtraClientClick="OnClearClick" CancelButtonText="Cancel" ExtraButtonText="Clear Selection" SubmitButtonText="Collapse" OnSubmitClientClick="clientNodeCollapsing"
                        SaveButtonText="OK" SaveButtonIconClass="rbSave" CauseValidationOnCancel="false"
                        UseAutoSkinMode="false" ButtonSkin="Silk" />
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnDefaultScreen" runat="server" />
        <asp:HiddenField ID="hdnTenantId" runat="server" />
        <asp:HiddenField ID="hdnSelectedNode" runat="server" />
        <asp:HiddenField ID="hdnLabel" runat="server" />
        <asp:HiddenField ID="hdnInstitutionNodeId" runat="server" />
        <asp:HiddenField ID="hdnIsHierarchyCollapsed" runat="server" Value="" />
    </form>
    <script type="text/javascript">

        function pageLoad() {
            $jQuery(".fa.fa-arrow-right.right-arrow-color").removeClass();
            $jQuery(".rbPrimaryIcon.rbNext").removeClass();
        }

        function OpenDefaultScreen() {
            var hdnDefaultScreen = $jQuery("[id$=hdnDefaultScreen]")[0];

            if (hdnDefaultScreen != undefined && hdnDefaultScreen.value != "") {
                window.setTimeout(function () { $jQuery("[id$=ifrDetails]").attr('src', hdnDefaultScreen.value); }, 500);
            }
        }

        function ClientContextMenuShowing(sender, eventArgs) {
            eventArgs.set_cancel(true);
        }

        function GetSelectedNode(sender, eventArgs) {
            //$jQuery("[id$=hdnSelectedNode]")[0].value = eventArgs.get_node().get_value();
        }

        function GetCheckedNode(sender, eventArgs) {
            if (eventArgs.get_node().get_checkState() == 1) {
                // When node is Checked
                var checkedNode = eventArgs.get_node();
                var checkedNodeValue = eventArgs.get_node().get_value();
                var checkedNodeValueArr = checkedNodeValue.split('_');
                $jQuery("[id$=hdnSelectedNode]")[0].value += checkedNodeValueArr[2] + ",";
                ManageParentAndChilds(checkedNode);

            }
            else if (eventArgs.get_node().get_checkState() == 0) {
                //When node is Unchecked
                var UncheckedNode = eventArgs.get_node();
                ManageParentAndChilds(UncheckedNode);
                var unCheckedNodeValue = eventArgs.get_node().get_value();
                var checkedNodeValueArr = unCheckedNodeValue.split('_');
                var valueToRemove = checkedNodeValueArr[2];
                var tmpArr = $jQuery("[id$=hdnSelectedNode]")[0].value.split(',');
                $jQuery("[id$=hdnSelectedNode]")[0].value = "";
                for (var i = 0; i < tmpArr.length; i++) {
                    if (tmpArr[i].trim() != valueToRemove && tmpArr[i] != "") {
                        $jQuery("[id$=hdnSelectedNode]")[0].value += tmpArr[i] + ",";
                    }
                }
            }
        }

        function SetTreeNode(sender, eventArgs) {
            if ($jQuery("[id$=hdnSelectedNode]")[0].value != "") {
                var node = sender.findNodeByValue($jQuery("[id$=hdnSelectedNode]")[0].value);
                if (node != undefined && node != null) {
                    var parentNode = node.get_parent();
                    while (parentNode.get_expanded != undefined) {
                        parentNode.expand();
                        parentNode = parentNode.get_parent();
                    }
                    node.select();
                    var element = $jQuery("a[href$=" + node.get_value() + "]");
                    if (element != null) {
                        element.focus();
                    }
                    $jQuery("[id$=hdnDefaultScreen]")[0].value = sender.get_selectedNode().get_navigateUrl();
                    //OpenDefaultScreen();
                }
            }
            else {
                if (sender.get_selectedNode() != null && sender.get_selectedNode().get_value() != "") {
                    $jQuery("[id$=hdnSelectedNode]")[0].value = sender.get_selectedNode().get_value();
                }
                //                $jQuery("[id$=hdnDefaultScreen]")[0].value = sender.get_selectedNode().get_navigateUrl();
                //                OpenDefaultScreen();
            }
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function returnToParent() {
            var hdnSelectedNode = $jQuery("[id$=hdnSelectedNode]")[0].value;
            var hdnLabel = $jQuery("[id$=hdnLabel]")[0].value;
            var hdnTenantId = $jQuery("[id$=hdnTenantId]")[0].value;
            var hdnInstitutionNodeId = $jQuery("[id$=hdnInstitutionNodeId]")[0].value;
            var oArg = {};
            var count = 0;
            if (hdnSelectedNode != "") {
                for (var i = 0; i < hdnSelectedNode.length; i++) {
                    if (hdnSelectedNode[i] == "_") {
                        count++;
                        break;
                    }
                }
                if (count > 0) {
                    var strControlName = hdnSelectedNode.split('_');
                    oArg.DepPrgMappingId = strControlName[2];
                }
                else {
                    oArg.DepPrgMappingId = hdnSelectedNode;
                }

            }
            else {
                oArg.DepPrgMappingId = "";
            }
            oArg.HierarchyLabel = hdnLabel;
            oArg.InstitutionNodeId = hdnInstitutionNodeId;
            oArg.TenantID = hdnTenantId;
            //get a reference to the current RadWindow
            var oWnd = GetRadWindow();
            oWnd.Close(oArg);
        }

        // To close the popup.
        function ClosePopup() {
            top.$window.get_radManager().getActiveWindow().close();
        }

        function OnClearClick() {
            $jQuery("[id$=hdnSelectedNode]")[0].value = "";
            $jQuery("[id$=hdnLabel]")[0].value = "";
            $jQuery("[id$=hdnInstitutionNodeId]")[0].value = "";
            returnToParent();
        }

        function ManageParentAndChilds(obj) {
            var allChildNodes = obj.get_allNodes();
            var allChildNodesCount = allChildNodes.length;
            var nodeLevel = obj.get_level();

            //Parent to child
            if (obj != null && obj != undefined && obj.get_checked()) {
                for (var i = 0; i < allChildNodesCount; i++) {
                    allChildNodes[i].set_checked(true);
                    var childNodeValue = allChildNodes[i].get_value();
                    var childNodeValueArr = childNodeValue.split('_');
                    $jQuery("[id$=hdnSelectedNode]")[0].value += childNodeValueArr[2] + ",";
                }
            }
            else {
                for (var i = 0; i < allChildNodesCount; i++) {
                    allChildNodes[i].set_checked(false);
                    var childNodeValue = allChildNodes[i].get_value();
                    var childNodeValueArr = childNodeValue.split('_');
                    var valueToRemove = childNodeValueArr[2];
                    var tmpArr = $jQuery("[id$=hdnSelectedNode]")[0].value.split(',');
                    $jQuery("[id$=hdnSelectedNode]")[0].value = "";
                    for (var j = 0; j < tmpArr.length; j++) {
                        if (tmpArr[j].trim() != valueToRemove && tmpArr[j] != "") {
                            $jQuery("[id$=hdnSelectedNode]")[0].value += tmpArr[j] + ",";
                        }
                    }
                }
            }

            //Child to parent
            var parent = obj.get_parent();
            if (obj != null && obj != undefined && !obj.get_checked()) {
                //var parent = obj.get_parent();
                if (parent != null && parent != undefined) {
                    for (var i = nodeLevel; i > 0; i--) {
                        var parentNode = parent;
                        parentNode.set_checked(false);
                        var parentNodeValue = parentNode.get_value();
                        var parentNodeValueArr = parentNodeValue.split('_');
                        var parentValueToRemove = parentNodeValueArr[2];
                        var tmpArr = $jQuery("[id$=hdnSelectedNode]")[0].value.split(',');
                        $jQuery("[id$=hdnSelectedNode]")[0].value = "";
                        for (var j = 0; j < tmpArr.length; j++) {
                            if (tmpArr[j].trim() != parentValueToRemove && tmpArr[j] != "") {
                                $jQuery("[id$=hdnSelectedNode]")[0].value += tmpArr[j] + ",";
                            }
                        }
                        parent = parentNode.get_parent();
                    }
                }
            }
            else {
                var isAllSiblingsChecked = true;
                //var parent = obj.get_parent();
                for (var i = nodeLevel; i > 0; i--) {
                    var parentNode = parent;
                    var childNodes = parentNode.get_allNodes();
                    var childNodeCount = childNodes.length;
                    for (var j = 0; j < childNodeCount; j++) {
                        if (!childNodes[j].get_checked()) {
                            isAllSiblingsChecked = false;
                            break;
                        }
                    }
                    if (isAllSiblingsChecked) {
                        parentNode.set_checked(true);
                        var parentNodeValue = parentNode.get_value();
                        var parentNodeValueArr = parentNodeValue.split('_');
                        $jQuery("[id$=hdnSelectedNode]")[0].value += parentNodeValueArr[2] + ",";
                    }
                    else
                        break;
                    parent = parentNode.get_parent();
                }
            }
        }

        function clientNodeCollapsing(sender, args) {
            //var treeViewDepartment = $find(FSObject.$("[id$=treeDepartment]")[0].id);

            //if (treeViewDepartment != null && typeof (treeViewDepartment) != "undefined" && treeViewDepartment.get_allNodes()[0].get_level() == '0') {

            //    var childNodes = treeViewDepartment.get_allNodes()[0]._getChildren();

            //    for (var i = 0; i < childNodes._array.length; i++) {

            //        childNodes._array[i].set_expanded(false)
            //    }
            //}
            var treeViewDepartment = $find(FSObject.$("[id$=treeDepartment]")[0].id);
            var nodes = treeViewDepartment.get_allNodes();
            if ($jQuery("[id$=fsucInstitutionHierarchyList_btnSubmit_input]").val() == "Collapse") {
                for (var i = 1; i < nodes.length; i++) {
                    if (nodes[i].get_nodes() != null) {
                        nodes[i].collapse();
                    }
                }
                $jQuery("[id$=hdnIsHierarchyCollapsed]")[0].value = "True";
                $jQuery("[id$=fsucInstitutionHierarchyList_btnSubmit_input]").val('Expand');
            }
            else {
                for (var i = 1; i < nodes.length; i++) {
                    if (nodes[i].get_nodes() != null) {
                        nodes[i].expand();
                    }
                }
                $jQuery("[id$=hdnIsHierarchyCollapsed]")[0].value = "False";
                $jQuery("[id$=fsucInstitutionHierarchyList_btnSubmit_input]").val('Collapse');
            }
        }


        function clientNodeCollapsed(sender, eventArgs) {
            var node = eventArgs.get_node();
            var childnodes = node._children._array;
            if (childnodes != null && typeof (childnodes) != "undefined" && childnodes.length > 0) {
                for (var i = 0; i < childnodes.length; i++) {
                    if (childnodes[i].get_nodes() != null) {
                        childnodes[i].collapse();
                    }
                }
            }
        }

    </script>
</body>
</html>
