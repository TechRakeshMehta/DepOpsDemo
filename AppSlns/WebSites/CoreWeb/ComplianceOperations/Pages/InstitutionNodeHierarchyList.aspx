<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.InstitutionNodeHierarchyList" Title="InstitutionHierarchyList"
    CodeBehind="InstitutionNodeHierarchyList.aspx.cs" %>

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
            <infs:LinkedResource Path="~/Resources/Generic/popup.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/InstitutionNodeHierarchyList.js" ResourceType="JavaScript" />
        </infs:WclResourceManager>
        <%--   <div class="popupContent">
            <div style="height: 90%; margin-right: -10px; overflow: auto"> <%--Change this if resize the Popup Dimentions--%>
        <div style="height: 93%; padding-bottom:50px">
            <div class="col-md-12" id="divDept">
                <div class="row">
                    <infs:WclTreeView ID="treeDepartment" runat="server" ClientKey="treeDepartment" OnNodeDataBound="TreeDepartment_NodeDataBound" OnClientNodeCollapsed="clientNodeCollapsed"
                        OnClientContextMenuShowing="ClientContextMenuShowing" CheckBoxes="true" OnClientNodeClicked="GetSelectedNode" OnClientNodeChecked="GetCheckedNode" 
                        >
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

        <%--  <div style="height: 30px; margin-right: -10px; margin-left: -9px;">--%>
        <%--Change this if resize the Popup Dimentions--%>
       
        <div style="width: 100%; z-index: 10; position: fixed; right: 0; bottom: 0;" >
            <div class="col-md-8">
                <div class="row text-right">
                    <infsu:CommandBar ID="fsucInstitutionHierarchyList" runat="server" DisplayButtons="Save,Extra,Cancel,Submit"
                        AutoPostbackButtons="Save" OnCancelClientClick="ClosePopup" OnSaveClick="btnOk_Click" SubmitButtonText="Collapse" OnSubmitClientClick="clientNodeCollapsing"
                        OnExtraClientClick="OnClearClick" CancelButtonText="Cancel" ExtraButtonText="Clear Selection"
                        SaveButtonText="OK" ButtonPosition="Right" CauseValidationOnCancel="false" />
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnDefaultScreen" runat="server" />
        <asp:HiddenField ID="hdnTenantId" runat="server" />
        <asp:HiddenField ID="hdnSelectedNode" runat="server" />
        <asp:HiddenField ID="hdnLabel" runat="server" />
        <asp:HiddenField ID="hdnInstitutionNodeId" runat="server" />
        <asp:HiddenField ID="hdnIsHierarchyCollapsed" runat="server" Value=""/>
    </form>
    <script type="text/javascript">
        function pageLoad() {
            $jQuery(".fa.fa-arrow-right.right-arrow-color").removeClass();
            $jQuery(".rbPrimaryIcon.rbNext").removeClass();
            $jQuery("[id$=fsucInstitutionHierarchyList_btnSubmit_input]").removeClass('rbPrimary');
        }
    </script>
</body>
</html>
