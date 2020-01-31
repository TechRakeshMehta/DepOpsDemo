<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgencyHierarchyList.aspx.cs" Inherits="CoreWeb.AgencyHierarchy.Views.AgencyHierarchyList" %>

<%@ Register Src="~/CommonControls/UserControl/PageBreadCrumb.ascx" TagName="PageBreadCrumb" TagPrefix="infsu" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Please Select An Agency Hierarchy </title>
    <style type="text/css">
        html {
            width: auto !important;
            height: auto !important;
            overflow: auto !important;
        }

        #treeAgencyHierarchy .rtIn {
            cursor: pointer;
        }
    </style>

</head>
<body>
    <form id="formAgencyHierarchy" runat="server">
        <asp:ScriptManager ID="scmMain" runat="server" ScriptMode="Release">
        </asp:ScriptManager>
        <infs:WclResourceManager ID="AgencyHierarchyListManager" runat="server">
            <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
            <infs:LinkedResource Path="~/Resources/Mod/AgencyHierarchy/AgencyHierarchyList.js" ResourceType="JavaScript" />
            <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
        </infs:WclResourceManager>
        <div style="height: 93%; padding-bottom: 50px">
            <div class="col-md-12" id="parentAgencyHierarchy">
                <div class="row">
                    <infs:WclTreeView ID="treeAgencyHierarchy" runat="server" ClientKey="treeAgencyHierarchy" Skin="Silk"
                        AutoSkinMode="false" OnNodeClick="TreeAgency_NodeClick" OnNodeDataBound="TreeAgencyHierarchy_NodeDataBound"
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

                    <infs:WclTreeView ID="treeChildAgencyHierarchy" runat="server" ClientKey="treeChildAgencyHierarchy" Skin="Silk" Visible="false"
                        AutoSkinMode="false" OnNodeClick="TreeChildAgencyHierarchy_NodeClick" OnClientNodeClicking="clientNodeClicking" OnClientNodeCollapsed="clientNodeCollapsed"
                        OnNodeDataBound="TreeChildAgencyHierarchy_NodeDataBound"
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
                    <infsu:CommandBar ID="fsucAgencyHierarchyList" runat="server" DisplayButtons="Cancel,Clear,Submit"
                        OnCancelClientClick="ClosePopup" ClearButtonText="Back" AutoPostbackButtons="Clear" OnClearClick="fsucAgencyHierarchyList_ClearClick"
                        CancelButtonText="Cancel" OnExtraClientClick="OnClearClick" ExtraButtonText="Clear Selection" SubmitButtonText="Collapse" OnSubmitClientClick="clientNodeCollapsing"
                        CauseValidationOnCancel="false"
                        UseAutoSkinMode="false" ButtonSkin="Silk" />
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnLabel" runat="server" />
        <asp:HiddenField ID="hdnNodeId" runat="server" />
        <asp:HiddenField ID="hdnAgencyId" runat="server" />
        <asp:HiddenField ID="hdnAgencyName" runat="server" />
        <asp:HiddenField ID="hdnRootNodeId" runat="server" />
        <asp:HiddenField ID="hdnIsHierarchyCollapsed" runat="server" Value="" />
    </form>
</body>
</html>
