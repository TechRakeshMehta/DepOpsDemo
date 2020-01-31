<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.InstitutionHierarchyList" Title="InstitutionHierarchyList"
    CodeBehind="InstitutionHierarchyList.aspx.cs" %>

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
            <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/InstitutionHierarchyList.js"
                ResourceType="JavaScript" />
            <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
        </infs:WclResourceManager>
        <div style="height: 93%; padding-bottom: 50px">
            <div class="col-md-12">
                <div class="row">
                    <infs:WclTreeView ID="treeDepartment" runat="server" ClientKey="treeDepartment" Skin="Silk"
                        AutoSkinMode="false" OnNodeDataBound="TreeDepartment_NodeDataBound"
                        OnClientContextMenuShowing="ClientContextMenuShowing" OnClientNodeCollapsed="clientNodeCollapsed"
                        OnClientNodeClicked="GetSelectedNode">
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
            $jQuery(".rbPrimaryIcon.rbNext").removeClass();
            $jQuery(".fa.fa-arrow-right.right-arrow-color").removeClass();
           
        }
    </script>


</body>
</html>
