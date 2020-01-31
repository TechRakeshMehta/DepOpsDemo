<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Pages.AdminPortalComponentList" Codebehind="AdminPortalComponentList.aspx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HeadUserControlFolderList" runat="server">
    <title>List of user control</title>
</head>
<body>
    <form id="formUserControlFolderList" runat="server">
    <asp:ScriptManager ID="scmMain" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <infs:WclResourceManager ID="FolderListManager" runat="server">
        <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/RComponentsList.js"
            ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Generic/popup.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Generic/popup.js" ResourceType="JavaScript" />
    </infs:WclResourceManager>
    <div class="popupContent">
        <input type="hidden" id="hdnCurrentNode" name="hdnSelectedNode" runat="server" />
        <infs:WclTreeView ID="treeControlID" runat="server" OnClientNodeClicked="ClientTreeNodeClicked"
            ClientKey="treecontrolfolder">
            <DataBindings>
                <telerik:RadTreeNodeBinding Expanded="True" />
            </DataBindings>
        </infs:WclTreeView>
        <infsu:CommandBar ID="fsucUserControlFolderList" runat="server" DisplayButtons="Save,Cancel"
            OnCancelClientClick="ClosePopup" OnSaveClientClick="returnToParent" CancelButtonText="Close"
            ButtonPosition="Right" CauseValidationOnCancel="false" />
    </div>
    </form>
</body>
</html>
