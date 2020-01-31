<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AgencyHierarchy.aspx.cs" Inherits="CoreWeb.AgencyHierarchy.Pages.AgencyHierarchy" %>


<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/CommonControls/UserControl/PageBreadCrumb.ascx" TagName="PageBreadCrumb" TagPrefix="infsu" %>

<asp:Content ID="contHead" ContentPlaceHolderID="PageHeadContent" runat="server">
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
            width: 480px;
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

        .custom-scroll {
            height: calc( 100% - 21px ) !important;
        }

        .lblAgencyHierarchy::after {
            content: '' !important;
        }
    </style>
</asp:Content>
<asp:Content ID="contBodyContentOutsideForm" ContentPlaceHolderID="BodyContentOutsideForm" runat="server">
</asp:Content>
<asp:Content ID="contDeafult" ContentPlaceHolderID="DefaultContent" runat="server">

    <infs:WclResourceManagerProxy runat="server" ID="rprxSetUp">
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/Breadcrumb.css" ResourceType="StyleSheet" />
         <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>

    <infs:WclSplitter ID="sptrContentPage" runat="server" LiveResize="true" Orientation="Horizontal"
        Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true" CssClass="maindiv">
        <infs:WclPane ID="pnMainToolbar" runat="server" Height="60" Width="100%"
            Scrolling="None" Collapsed="false">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="container-fluid" id="dvSharedUserBreadcrumb">
                        <div class="col-md-6 pull-left" style="padding-left: 0px; padding-top: 10px;">
                            <asp:Panel runat="server" CssClass="main_drop" ID="pnlTenant" Width="500px">
                                <asp:Label ID="Label1" CssClass="cptn lblAgencyHierarchy" runat="server" Text="Agency Hierarchy"></asp:Label>
                                <infs:WclComboBox ID="ddlAgencyHierarchy" runat="server" AutoPostBack="false" DataTextField="Value" Style="float: right;"
                                    DataValueField="NodeID" EmptyMessage="--Select--" Width="377px" Skin="Silk" AutoSkinMode="false" CssClass="form-control"
                                    Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged">
                                </infs:WclComboBox>
                            </asp:Panel>
                        </div>
                        <div class="col-md-6 pull-right">
                            <div class="row">
                                <h1 class="text-right">
                                    <asp:Label Text="Agency Hierarchy" runat="server" ID="lblPageHdr" />&nbsp;
                             <asp:Label Text="Agency Hierarchy" runat="server" ID="lblModHdr" CssClass="phdr" />
                                </h1>
                                <div class="breadcrumb padbott10 text-right">
                                    <infsu:PageBreadCrumb ID="breadcrum" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                </div>
                                <div class="col-md-6 text-right">
                                    <a runat="server" onclick="Page.showProgress('Processing...');" id="lnkGoBack">Back to Search</a>
                                </div>
                            </div>
                        </div>
                    </div>
                     <asp:HiddenField ID="hdnIsNewRootNodeAdded" runat="server" Value="false" />
                    <asp:HiddenField ID="hdnAddedRootNodeId" runat="server" Value="0" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnUpdateTree" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <%--<infs:WclButton runat="server" ID="btnRLoadDropDown" OnClick="btnRLoadDropDown_Click" Skin="Silk" AutoSkinMode="false"
                ButtonType="StandardButton">
            </infs:WclButton>--%>
        </infs:WclPane>
        <infs:WclPane ID="pnMain" runat="server" Scrolling="None" Width="100%">
            <infs:WclSplitter ID="sptrPage" runat="server" LiveResize="true" Orientation="Vertical"
                Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
                <infs:WclPane ID="pnHierarchy" runat="server" Height="100%" Width="300" MinWidth="300"
                    Scrolling="Y" Collapsed="false" CssClass="tree-pane">
                    <asp:UpdatePanel runat="server" ID="updpnlTree" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnDefaultScreen" runat="server" />
                            <asp:HiddenField ID="hdnIsRootNodeChanged" runat="server" Value="" />
                            <infs:WclTreeView ID="treeAgencyHierarchy" runat="server" ClientKey="treeAgencyHierarchy" OnNodeDataBound="treeAgencyHierarchy_NodeDataBound"
                                OnClientContextMenuShowing="ClientContextMenuShowing" OnClientLoad="SetTreeNode"
                                OnClientNodeClicked="GetSelectedNode" Width="100%" Skin="Silk" AutoSkinMode="false">
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
                <infs:WclPane ID="pnHierarchyMapping" runat="server" Scrolling="None" Width="100%">
                    <iframe name="childpageframe" id="ifrHierarchyMapping" runat="server" class="child_pageframe"
                        src=""></iframe>
                    <asp:HiddenField ID="hdnSelectedNode" runat="server" Value="" />
                </infs:WclPane>
            </infs:WclSplitter>
        </infs:WclPane>

    </infs:WclSplitter>

    <script type="text/javascript">
        function OpenDefaultScreen() {
            var hdnDefaultScreen = $jQuery("[id$=hdnDefaultScreen]")[0];
            if (hdnDefaultScreen != undefined && hdnDefaultScreen.value != "") {
                $jQuery("[id$=ifrHierarchyMapping]").attr('src', hdnDefaultScreen.value);
            }
            else {
                $jQuery("[id$=ifrHierarchyMapping]").attr('src', "");
            }
            $jQuery("[id$=hdnIsRootNodeChanged]")[0].value = false;
        }

        function OnClientSelectedIndexChanged() {
            $jQuery("[id$=hdnIsRootNodeChanged]")[0].value = true;
            RefreshTree();
        }

        function RefreshTree() {
            Page.showProgress("Processing...");

            $jQuery("[id$=hdnSelectedNode]")[0].value = "";
            var btn = $jQuery('[id$=btnUpdateTree]');
            btn.click();
        }

        function ClientContextMenuShowing(sender, eventArgs) {
            eventArgs.set_cancel(true);
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

        function GetSelectedNode(sender, eventArgs) {
            $jQuery("[id$=hdnSelectedNode]")[0].value = eventArgs.get_node().get_value();
        }
    </script>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
