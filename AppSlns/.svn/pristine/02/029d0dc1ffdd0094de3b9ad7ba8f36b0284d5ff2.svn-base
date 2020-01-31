<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.BkgSetup.Views.SetupInstitutionHierarchyBkg" Title="SetupInstitutionHierarchyBkg"
    MasterPageFile="~/Shared/DefaultMaster.master" CodeBehind="SetupInstitutionHierarchyBkg.aspx.cs" %>

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
            cursor: not-allowed !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxSetUp">
        <infs:LinkedResource Path="~/Resources/Mod/BkgSetup/SetUpInstituteHierarchy.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
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
                    <asp:Label Text="Background Package Mapping" runat="server" ID="lblModHdr" />
                    &nbsp;<asp:Label
                        Text="Institution Hierarchy" runat="server" ID="lblPageHdr" CssClass="phdr" />
                </h1>
            </div>
        </div>
    <%--</div>--%>
    <%--</infs:WclPane>--%>
    <%--<infs:WclPane ID="paneTenant" runat="server" Scrolling="None" Height="1px" Width="100%">--%>
    <div id="paneTenant" runat="server">
        <div class="pane-client">
            <asp:Panel runat="server" CssClass="main_drop" ID="pnlTenant">
                <asp:Label ID="lblTenant" runat="server" Text="Institution  " CssClass="common-splitter-font"></asp:Label>
                &nbsp;
                    <%--<infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="false" DataTextField="TenantName" OnDataBound="ddlTenant_DataBound"
                        DataValueField="TenantID"  Width="377px" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged">
                    </infs:WclDropDownList>--%>
                <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="false" DataTextField="TenantName" OnDataBound="ddlTenant_DataBound"
                    DataValueField="TenantID" Width="377px" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged"
                    Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
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
                                OnClientLoad="SetTreeNode" OnClientNodeClicked="GetSelectedNode" OnClientContextMenuShowing="ClientContextMenuShowing"
                                OnClientContextMenuItemClicked="ClientContextMenuItemClicked" CssClass="tree-design">
                                <ContextMenus>
                                    <telerik:RadTreeViewContextMenu ID="mnuTreePackage" runat="server">
                                        <Items>
                                            <telerik:RadMenuItem Value="MenuCopyPk" Text="Copy Package">
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
                <%--<infs:WclSplitBar ID="WclSplitBar1" runat="server" CollapseMode="forward">
                </infs:WclSplitBar>--%>
                <%--<infs:WclPane ID="pnLower" runat="server" Scrolling="None" Width="100%">--%>
                <div id="right-pane">
                    <iframe name="childpageframe" id="ifrDetails" runat="server" class="child_pageframe"
                        src=""></iframe>
                    <asp:HiddenField ID="hdnSelectedNode" runat="server" Value="" />
                </div>
            </div>
        </div>
    </div>
    <%--</infs:WclPane>
            </infs:WclSplitter>
        </infs:WclPane>
    </infs:WclSplitter>--%>
</asp:Content>
