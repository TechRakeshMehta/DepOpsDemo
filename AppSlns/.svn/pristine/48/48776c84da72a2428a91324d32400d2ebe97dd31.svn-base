
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgencyHierarchyMultipleRootNodes.aspx.cs" Inherits="CoreWeb.AgencyHierarchy.Views.AgencyHierarchyMultipleRootNodes" %>

<%@ Register Src="~/CommonControls/UserControl/PageBreadCrumb.ascx" TagName="PageBreadCrumb"
    TagPrefix="infsu" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Please Select An Agency Hierarchy</title>
    <style type="text/css">
        html {
            width: auto !important;
            height: auto !important;
            overflow: auto !important;
        }

        #treeAgencyHierarchy .rtIn {
            cursor: pointer;
        }

        .DisableNode {
            opacity: 0.5;
            cursor: not-allowed;
        }
    </style>
    <script type="text/javascript">

        function ClientContextMenuShowing(sender, eventArgs) {
            eventArgs.set_cancel(true);
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function returnToParentFromAgencyHierarchy(jsonObj) {
               debugger;
            var oArg = {};
            if (typeof jsonObj != 'undefined' && jsonObj != '') {
                var agencyHierarchyJsonObj = JSON.stringify(jsonObj);
                oArg.AgencyHierarchyJsonObj = agencyHierarchyJsonObj;
            }

            var hdnSelectedAgecnyIds = $jQuery("[id$=hdnSelectedAgecnyIds]")[0].value; //jsonObj.agencyhierarchy.AgencyID;// $jQuery("[id$=hdnSelectedAgecnyIds]")[0].value;
            var hdnSelectedNodeIds = $jQuery("[id$=hdnSelectedNodeIds]")[0].value;
            var hdnSelectedRootNodeId = $jQuery("[id$=hdnSelectedRootNodeId]")[0].value;
            var hdnIsChildTreeNodeChecked = $jQuery("[id$=hdnIsChildTreeNodeChecked]")[0].value;


            oArg.SelectedAgecnyIds = hdnSelectedAgecnyIds;
            oArg.SelectedNodeIds = hdnSelectedNodeIds;
            oArg.SelectedRootNodeId = hdnSelectedRootNodeId;
            oArg.IsChildTreeNodeChecked = hdnIsChildTreeNodeChecked;
            //get a reference to the current RadWindow
            var oWnd = GetRadWindow();
            oWnd.Close(oArg);
        }

        // To close the popup.
        function ClosePopup() {
            top.$window.get_radManager().getActiveWindow().close();
        }

        function OnClearClick() {
            $jQuery("[id$=hdnSelectedAgecnyIds]")[0].value = "";
            $jQuery("[id$=hdnLabel]")[0].value = "";
            $jQuery("[id$=hdnSelectedNodeIds]")[0].value = "";
            $jQuery("[id$=hdnSelectedRootNodeId]")[0].value = "";
            $jQuery("[id$=hdnIsChildTreeNodeChecked]")[0].value = "";
            var jsonObj = ''
            returnToParentFromAgencyHierarchy(jsonObj);
        }
        function clientNodeChecking(sender, args) {
            //debugger;
            if (args.get_node().get_nodes().get_count() == 0 && args.get_node().get_level() != 0) {
                args.set_cancel(false);
            }
            else {
                args.set_cancel(true);
            }
        }
        function ClientNodeClicked(sender, args) {

            var currentNode = args.get_node();
            var allNodes = currentNode.get_allNodes();
            var allNodesCount = allNodes.length;
            if (currentNode.get_checked()) {

                for (var i = 0; i < allNodesCount; i++) {
                    allNodes[i].set_checked(false);
                    // allNodes[i].set_enabled(false);

                    //here you may check what is the current node level or child nodes count
                }

            }
            else {
                for (var i = 0; i < allNodesCount; i++) {

                    // allNodes[i].set_enabled(true);
                    allNodes[i].set_checked(false);

                    //here you may check what is the current node level or child nodes count
                }
            }

        }

        function clientNodeCollapsing(sender, args) {
            var isDisabled = $jQuery("input[type=submit]").prop('disabled');
            if (!isDisabled) {
                var treeViewDepartment = $find(FSObject.$("[id$=treeChildAgencyHierarchy]")[0].id);
                var nodes = treeViewDepartment.get_allNodes();

                //UAT-3952
                if ($jQuery("[id$=fsucAgencyHierarchyMultipleNodes_btnSubmit_input]").val() == "Collapse") {
                    for (var i = 1; i < nodes.length; i++) {
                        if (nodes[i].get_nodes() != null) {
                            nodes[i].collapse();
                        }
                    }
                    $jQuery("[id$=hdnIsHierarchyCollapsed]")[0].value = "True";
                    $jQuery("[id$=fsucAgencyHierarchyMultipleNodes_btnSubmit_input]").val('Expand');
                }
                else {
                    for (var i = 1; i < nodes.length; i++) {
                        if (nodes[i].get_nodes() != null) {
                            nodes[i].expand();
                        }
                    }
                    $jQuery("[id$=hdnIsHierarchyCollapsed]")[0].value = "False";
                    $jQuery("[id$=fsucAgencyHierarchyMultipleNodes_btnSubmit_input]").val('Collapse');
                }
            }
        }
        function DisabledCollaspeButton() {
            if ($jQuery("[id$=hdnIsHierarchyCollapsed]")[0].value == "True") {
                $jQuery("[id$=fsucAgencyHierarchyMultipleNodes_btnSubmit_input]").val('Expand');
            } else {
                $jQuery("[id$=fsucAgencyHierarchyMultipleNodes_btnSubmit_input]").val('Collapse');
            }

            if ($("[id$=treeChildAgencyHierarchy]") != null && $("[id$=treeChildAgencyHierarchy]").length > 0) {

                var treeViewDepartment = $find(FSObject.$("[id$=treeChildAgencyHierarchy]")[0].id);

                if (treeViewDepartment != null && typeof (treeViewDepartment) != "undefined" && treeViewDepartment.get_allNodes()[0].get_level() == '0') {

                    var childNodes = treeViewDepartment.get_allNodes()[0]._getChildren();

                    if (childNodes._array[0].get_level() == '1' && childNodes._array[0]._getChildren()._array.length == 0) {
                        $jQuery("input[type=submit]").attr('disabled', 'disabled');
                        $jQuery("[id$=fsucAgencyHierarchyMultipleNodes_btnSubmit]").addClass('rbDisabled')
                    }
                }
            }
        }
        function pageLoad() {
            $jQuery(".fa.fa-arrow-right.right-arrow-color").removeClass();
            $jQuery(".rbPrimaryIcon.rbNext").removeClass();
            //  $jQuery(".rbPrimaryIcon").removeClass().addClass("fa fa-compress");
            DisabledCollaspeButton();
        }
        function ClientNodeCollapsed(sender, eventArgs) {
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
</head>
<body>
    <form id="formAgencyHierarchy" runat="server">
        <asp:ScriptManager ID="scmMain" runat="server" ScriptMode="Release">
        </asp:ScriptManager>
        <infs:WclResourceManager ID="AgencyHierarchyListManager" runat="server">
            <%--<infs:LinkedResource Path="~/Resources/Generic/popup.css" ResourceType="StyleSheet" />--%>
            <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
            <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
            <%--          <infs:LinkedResource Path="~/Resources/Mod/AgencyHierarchy/AgencyHierarchyMultipleRootNodes.js"
                ResourceType="JavaScript" />--%>
        </infs:WclResourceManager>
        <div style="height: 93%; padding-bottom: 50px">
            <div class="col-md-12" id="parentAgencyHierarchy">
                <div class="row">
                    <infs:WclTreeView ID="treeAgencyHierarchy" runat="server" ClientKey="treeAgencyHierarchy" Skin="Silk" OnNodeClick="treeAgencyHierarchy_NodeClick"
                        AutoSkinMode="false" CheckBoxes="false" OnNodeDataBound="TreeAgencyHierarchy_NodeDataBound"
                        OnClientContextMenuShowing="ClientContextMenuShowing">
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

                    <infs:WclTreeView ID="treeChildAgencyHierarchy" runat="server" ClientKey="treeChildAgencyHierarchy" AutoCheckChildNodes="True" Skin="Silk" Visible="false"
                        AutoSkinMode="false" CheckBoxes="true" OnNodeDataBound="TreeChildAgencyHierarchy_NodeDataBound" OnClientNodeChecked="ClientNodeClicked" OnClientNodeCollapsed="ClientNodeCollapsed"
                        OnClientContextMenuShowing="ClientContextMenuShowing">
                        <ContextMenus>
                            <telerik:RadTreeViewContextMenu ID="RadTreeViewContextMenu1" runat="server">
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
                    <infsu:CommandBar ID="fsucAgencyHierarchyMultipleNodes" runat="server" DisplayButtons="Save,Cancel,Clear,Submit"
                        OnCancelClientClick="ClosePopup" OnSaveClick="btnOk_Click" SaveButtonText="OK" AutoPostbackButtons="Save,Clear" ClearButtonText="Back" OnClearClick="fsucAgencyHierarchyMultipleNodes_ClearClick"
                        CancelButtonText="Cancel" OnExtraClientClick="OnClearClick" ExtraButtonText="Clear Selection" SubmitButtonText="Collapse" OnSubmitClientClick="clientNodeCollapsing"
                        CauseValidationOnCancel="false"
                        UseAutoSkinMode="false" ButtonSkin="Silk" />
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnTenantId" runat="server" />
        <asp:HiddenField ID="hdnLabel" runat="server" />
        <asp:HiddenField ID="hdnSelectedAgecnyIds" runat="server" />
        <asp:HiddenField ID="hdnSelectedNodeIds" runat="server" />
        <asp:HiddenField ID="hdnSelectedRootNodeId" runat="server" />
        <asp:HiddenField ID="hdnIsChildTreeNodeChecked" runat="server" Value="" />
        <asp:HiddenField ID="hdnIsHierarchyCollapsed" runat="server" Value="" />
    </form>

    <script type="text/javascript">
        //UAT-2772 || Bug ID: 17095
        $jQuery(document).ready(function () {
            $jQuery(".DisableNode").each(function () {
                var chkBox = $jQuery(this).siblings("input[type='checkbox']");
                $jQuery(chkBox).attr('disabled', 'disabled');
                $jQuery(chkBox).attr('class', 'DisableNode');
            });
        });
    </script>
</body>
</html>

