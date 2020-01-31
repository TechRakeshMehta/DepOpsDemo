<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.SetupDepartmentProgram" Title="SetupDepartmentProgram"
    MasterPageFile="~/Shared/DefaultMaster.master" CodeBehind="SetupDepartmentProgram.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/CommonControls/UserControl/PageBreadCrumb.ascx" TagName="PageBreadCrumb"
    TagPrefix="infsu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadContent" runat="server">
    <style type="text/css">
        html, body, #frmmod, #UpdatePanel1, #box_content {
            height: 100% !important;
        }

        .child_pageframe {
            width: 100%;
            height: 100%;
        }

        .pane-client {
            background-color: transparent;
            height: 40px;
            position: absolute;
            top: 0;
            right: 10px;
        }

        .main_drop {
            width: 457px;
            margin-top: 12px;
            color: #000;
        }

        .tree-pane {
            padding-top: 10px;
        }

            .tree-pane a, .tree-pane a:visited {
                color: Black;
                font-family: Arial;
            }

        .DisableNode {
          
            cursor: not-allowed!important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {          
            $("#vertical").kendoSplitter({

            });

            $("#horizontal").kendoSplitter({
                panes: [

                    { collapsible: true, resizable: true, size: "300px", scrollable: true, min:"300px" }
                ]
            });
        });

        var compliancePackageID;
        var selectedTenantId;
        var NodeID;

        //        $jQuery(document).ready(function () {
        //            //            if($jQuery("[id$=hdnTenantId]").val() > 0)
        //            //                window.setTimeout(function () { $jQuery("[id$=ifrDetails]").attr('src', "InstituteHierarchyNodePackage.aspx?Id=" + $jQuery("[id$=hdnDeptProgramMappingId]").val() + "&TenantId=" + $jQuery("[id$=hdnTenantId]").val()); }, 500);
        //            var hdnDefaultScreen = $jQuery("[id$=hdnDefaultScreen]")[0];

        //            if (hdnDefaultScreen != undefined && hdnDefaultScreen.value != "") {
        //                window.setTimeout(function () { $jQuery("[id$=ifrDetails]").attr('src', hdnDefaultScreen.value); }, 500);
        //            }
        //        });
        $jQuery(document).ready(function () {
            //  debugger;
            var hdnIsDefaultPreferredTenant = $jQuery("[id$=hdnIsDefaultPreferredTenant]").val();
            if (hdnIsDefaultPreferredTenant) {
                btn = $jQuery('[id$=btnUpdateTree]');
                btn.click();
            }
        });

        function OpenDefaultScreen() {
            var hdnDefaultScreen = $jQuery("[id$=hdnDefaultScreen]")[0];
            if (hdnDefaultScreen != undefined && hdnDefaultScreen.value != "") {
                $jQuery("[id$=ifrDetails]").attr('src', hdnDefaultScreen.value);
            }
            else {
                $jQuery("[id$=ifrDetails]").attr('src', "");
            }
            $jQuery("[id$=hdnTenantIsChanged]")[0].value = false;
        }

        //UAT-2386
        function ClientContextMenuShowing(sender, eventArgs) {
            var treeNode = eventArgs.get_node();
            var parametersValue = treeNode.get_navigateUrl();

            if (parametersValue.indexOf("CompliancePkgId") > 0 && parametersValue.indexOf("TenantId") > 0) {
                var parameters = treeNode.get_navigateUrl().split("&");
                //Get selectedTenantId value
                selectedTenantId = parameters[6].replace("TenantId=", '');
                //Get compliancePackageID value CompliancePackageID for Package Node
                compliancePackageID = parameters[5].replace("CompliancePkgId=", '');
                NodeID = parameters[2].replace("MappingID=", '');
                treeNode.set_selected(true);
            }
            else {
                //Disable the right click on the node 
                eventArgs.set_cancel(true);
            }
        }

        //UAT-2386
        function ClientContextMenuItemClicked(sender, eventArgs) {
            openPopUp(compliancePackageID, selectedTenantId, NodeID);
        }

        //UAT-2386
        function openPopUp(compliancePackageID, selectedTenantId, NodeID) {
            var packageCopyScreenWindowName = "PackageCopyScreen";
            ResetTimer();
            var url = $page.url.create("~/ComplianceAdministration/Pages/PackageCopyToLowerNode.aspx?TenantID=" + selectedTenantId + "&CompliancePackageID=" + compliancePackageID + "&NodeID=" + NodeID);

            var popupHeight = $jQuery(window).height() * (60 / 100);
            var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move, name: packageCopyScreenWindowName, onclose: OnClientClose });
            winopen = true;
        }

        function OnClientClose(oWnd, args) {
            oWnd.remove_close(OnClientClose);
            if (winopen) {
                winopen = false;
                var btn = $jQuery('[id$=btnUpdateTree]');
                btn.click();
            }
        }

        function GetSelectedNode(sender, eventArgs) {
            $jQuery("[id$=hdnSelectedNode]")[0].value = eventArgs.get_node().get_value();
        }

        function SetTreeNode(sender, eventArgs) {
            // debugger;
            if ($jQuery("[id$=hdnSelectedNode]")[0].value != "") {
                var node = sender.findNodeByValue($jQuery("[id$=hdnSelectedNode]")[0].value);
                if (node != undefined && node != null) {
                    //    var parentNode = node.get_parent();
                    //    while (parentNode.get_expanded != undefined) {
                    //        parentNode.expand();
                    //        parentNode = parentNode.get_parent();
                    //    }
                    node.select();
                    var element = $jQuery("a[href$=" + node.get_value() + "]");
                    if (element != null) {
                        element.focus();
                    }
                    $jQuery("[id$=hdnDefaultScreen]")[0].value = sender.get_selectedNode().get_navigateUrl();
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

        function OnClientSelectedIndexChanged() {
            //debugger;
            $jQuery("[id$=hdnTenantIsChanged]")[0].value = true;
            RefreshTree();
        }

        function RefreshTree() {
            // debugger;
            Page.showProgress("Processing...");

            $jQuery("[id$=hdnSelectedNode]")[0].value = "";
            var btn = $jQuery('[id$=btnUpdateTree]');
            btn.click();
        }

        function ResetTimer() {
            var hdntimeout = $jQuery('[id$=hdntimeout]');  //, $jQuery(parent.theForm));
            if (hdntimeout != null) {
                var timeout = hdntimeout.val();
                parent.StartCountDown(timeout);
            }
        }

    </script>

    <asp:HiddenField ID="hdnTenantId" runat="server" />
    <asp:HiddenField ID="hdnIsDefaultPreferredTenant" runat="server" Value="" />
    <%--<infs:WclSplitter ID="sptrContentPage" runat="server" LiveResize="true" Orientation="Horizontal"
        Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">--%>
    <%--<infs:WclPane ID="pnMainToolbar" runat="server" MinHeight="50" Height="50" Width="100%"
            Scrolling="None" Collapsed="false">--%>
    <%--<div id="modwrapo">--%>
        <div id="modwrapi">
            <div id="breadcmb">
                <infsu:PageBreadCrumb ID="breadcrum" runat="server" />
            </div>
            <div id="modhdr">
                <h1>
                    <asp:Label Text="Institution Hierarchy Setup" runat="server" ID="lblModHdr" />&nbsp;<asp:Label
                        Text="Institution Hierarchy" runat="server" ID="lblPageHdr" CssClass="phdr" /></h1>
            </div>
        </div>
    <%--</div>--%>
    <%--</infs:WclPane>--%>
    <%--<infs:WclPane ID="paneTenant" runat="server" Scrolling="None" Height="1px" Width="100%">--%>
    <div id="paneTenant" runat="server">
        <div class="pane-client">
            <asp:Panel runat="server" CssClass="main_drop" ID="pnlTenant">
                <asp:Label ID="lblTenant" runat="server" Text="Institution  " CssClass="common-splitter-font"></asp:Label>&nbsp;
                    <%--<infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                        DataValueField="TenantID" DefaultMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged" width="377px">
                    </infs:WclDropDownList>--%>
                <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="false" DataTextField="TenantName"
                    DataValueField="TenantID" EmptyMessage="--Select--" Width="377px"
                    Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged">
                </infs:WclComboBox>
            </asp:Panel>
        </div>
    </div>
    <%--</infs:WclPane>--%>
    <%--<infs:WclPane ID="pnMain" runat="server" Scrolling="None" Width="100%">--%>
    <%--<infs:WclSplitter ID="sptrPage" runat="server" LiveResize="true" Orientation="Vertical"
                Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">--%>
    <%--<infs:WclPane ID="pnUpper" runat="server" Height="100%" Width="300" MinWidth="300"
                    Scrolling="Y" Collapsed="false">--%>

    <%--style="min-width:300px;"--%>
    <div id="vertical" style="height: 98%">
        <div id="top-pane">
            <div id="horizontal" style="height: 95%; width: 100%;">
                <div id="left-pane">
                    <asp:UpdatePanel runat="server" ID="updpnlTree" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnDefaultScreen" runat="server" />
                            <asp:HiddenField ID="hdnTenantIsChanged" runat="server" Value="" />
                            <asp:HiddenField ID="hdnIsAvailableforOrder" runat="server" Value="true" />
                            <asp:HiddenField ID="hdnIsPackageBundleAvailableforOrder" runat="server" Value="true" />
                            <infs:WclTreeView ID="treeDepartment" runat="server" ClientKey="treeDepartment" OnNodeDataBound="TreeDepartment_NodeDataBound"
                                OnClientContextMenuShowing="ClientContextMenuShowing" OnClientLoad="SetTreeNode" OnClientContextMenuItemClicked="ClientContextMenuItemClicked"
                                OnClientNodeClicked="GetSelectedNode" CssClass="tree-design">
                                <ContextMenus>
                                    <telerik:RadTreeViewContextMenu ID="mnuTreeCategory" runat="server">
                                        <Items>
                                            <telerik:RadMenuItem Value="MenuItemPackageCopy" Text="Copy Package">
                                            </telerik:RadMenuItem>
                                        </Items>
                                    </telerik:RadTreeViewContextMenu>
                                </ContextMenus>
                            </infs:WclTreeView>
                            <asp:Button ID="btnUpdateTree" runat="server" UseSubmitBehavior="false" CssClass="btnUpdateTree" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnUpdateTree" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <%--</infs:WclPane>--%>
                <div id="right-pane">
                   <%-- <infs:WclSplitBar ID="WclSplitBar1" runat="server" CollapseMode="forward">
                    </infs:WclSplitBar>--%>
                    <%--<infs:WclPane ID="pnLower" runat="server" Scrolling="None" Width="100%">--%>
                    <iframe name="childpageframe" id="ifrDetails" runat="server" class="child_pageframe"
                        src=""></iframe>
                    <asp:HiddenField ID="hdnSelectedNode" runat="server" Value="" />
                </div>

                <%--</infs:WclPane>--%>
            </div>
        </div>
    </div>
    <%--</infs:WclSplitter>
        </infs:WclPane>
    </infs:WclSplitter>--%>
</asp:Content>
