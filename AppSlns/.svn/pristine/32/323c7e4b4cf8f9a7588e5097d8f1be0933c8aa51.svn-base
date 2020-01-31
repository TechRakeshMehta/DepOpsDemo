<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="SetupUniversalMapping.aspx.cs"
    Inherits="CoreWeb.ComplianceAdministration.Views.SetupUniversalMapping"
    MasterPageFile="~/Shared/DefaultMaster.master" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxetupUniversalMapping">
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/Breadcrumb.css" ResourceType="StyleSheet" />
    </infs:WclResourceManagerProxy>
    <script type="text/javascript">
        function OpenDefaultScreen() {
            var hdnDefaultScreen = $jQuery("[id$=hdnDefaultScreen]")[0];
            if (hdnDefaultScreen != undefined && hdnDefaultScreen.value != "") {
                $jQuery("[id$=ifrDetails]").attr('src', hdnDefaultScreen.value);
            }
            else {
                $jQuery("[id$=ifrDetails]").attr('src', "");
            }
        }

        function OpenSelectedNodeScreen() {
            var hdnSelectedNodeScreen = $jQuery("[id$=hdnSelectedNodeScreen]")[0];
            if (hdnSelectedNodeScreen != undefined && hdnSelectedNodeScreen.value != "") {
                $jQuery("[id$=ifrDetails]").attr('src', hdnSelectedNodeScreen.value);
            }
            else {
                $jQuery("[id$=ifrDetails]").attr('src', "");
            }
        }

        function ClientContextMenuShowing(sender, eventArgs) {
            eventArgs.set_cancel(true);
        }

        function GetSelectedNode(sender, eventArgs) {
            $jQuery("[id$=hdnSelectedNode]")[0].value = eventArgs.get_node().get_value();
            var node = sender.findNodeByValue($jQuery("[id$=hdnSelectedNode]")[0].value);
            if (node != undefined && node != null) {
                node.select();
                var element = $jQuery("a[href$=" + node.get_value() + "]");
                if (element != null) {
                    element.focus();
                }
                $jQuery("[id$=hdnSelectedNodeScreen]")[0].value = sender.get_selectedNode().get_navigateUrl();
            }
        }

        function SetTreeNode(sender, eventArgs) {
            if ($jQuery("[id$=hdnSelectedNode]")[0].value != "") {
                var node = sender.findNodeByValue($jQuery("[id$=hdnSelectedNode]")[0].value);
                if (node != undefined && node != null) {
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
            }
        }

        function RefreshTree() {
            Page.showProgress("Processing...");

            $jQuery("[id$=hdnSelectedNode]")[0].value = "";
            var btn = $jQuery('[id$=btnUpdateTree]');
            btn.click();
        }

        function ResetTimer() {
            var hdntimeout = $jQuery('[id$=hdntimeout]');
            if (hdntimeout != null) {
                var timeout = hdntimeout.val();
                parent.StartCountDown(timeout);
            }
        }

        function NavigateToSelectedNode(url) {
            $jQuery("[id$=ifrDetails]").attr('src', url);
            loadDefaultDetailPage = false;
        }

    </script>
    <infs:WclSplitter ID="sptrContentPage" runat="server" LiveResize="true" Orientation="Horizontal"
        Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true" CssClass="maindiv">

        <infs:WclPane ID="pnMainToolbar" runat="server" Height="50" Width="100%"
            Scrolling="None" Collapsed="false">
            <div class="container-fluid" id="dvSharedUserBreadcrumb">
                <div class="col-md-6 pull-right">
                    <div class="row">
                        <h1 class="text-right">
                            <asp:Label Text="Universal Mapping" runat="server" ID="lblPageHdr" />&nbsp;
                             <asp:Label Text="Universal Mapping Setup" runat="server" ID="lblModHdr" CssClass="phdr" />
                        </h1>
                        <div class="breadcrumb padbott10 text-right">
                            <infsu:PageBreadCrumb ID="breadcrum" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </infs:WclPane>
        <infs:WclPane ID="pnMain" runat="server" Scrolling="None" Width="100%">
            <infs:WclSplitter ID="sptrPage" runat="server" LiveResize="true" Orientation="Vertical"
                Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
                <infs:WclPane ID="pnUpper" runat="server" Height="100%" Width="300" MinWidth="300"
                    Scrolling="Y" Collapsed="false" CssClass="tree-pane">
                    <asp:UpdatePanel runat="server" ID="updpnlTree" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnDefaultScreen" runat="server" />
                            <asp:HiddenField ID="hdnSelectedNodeScreen" runat="server" />
                            <infs:WclTreeView ID="treeUniversalMapping" runat="server" ClientKey="treeDepartment" OnNodeDataBound="treeUniversalMapping_NodeDataBound"
                                OnClientLoad="SetTreeNode" OnClientNodeClicked="GetSelectedNode" Skin="Silk" AutoSkinMode="false" Width="100%">
                                <ContextMenus>
                                    <telerik:RadTreeViewContextMenu ID="mnuTreeCategory" runat="server">
                                        <Items>
                                            <telerik:RadMenuItem Value="MenuItem1" Text="MenuItem1">
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
                </infs:WclPane>
                <infs:WclSplitBar ID="WclSplitBar1" runat="server" CollapseMode="forward">
                </infs:WclSplitBar>
                <infs:WclPane ID="pnLower" runat="server" Scrolling="None" Width="100%">
                    <iframe name="childpageframe" id="ifrDetails" runat="server" class="child_pageframe" src=""></iframe>
                    <asp:HiddenField ID="hdnSelectedNode" runat="server" Value="" />
                </infs:WclPane>
            </infs:WclSplitter>
        </infs:WclPane>
    </infs:WclSplitter>
</asp:Content>
