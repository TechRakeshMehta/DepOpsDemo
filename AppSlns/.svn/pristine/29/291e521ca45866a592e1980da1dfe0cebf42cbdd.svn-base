<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="SysXSecurityModel_UserControl_UserControlFolderListForPolicy" Codebehind="UserControlFolderListForPolicy.aspx.cs" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HeadUserControlFolderListPolicy" runat="server">
    <title>User Control Folder List for managing Policy</title>
</head>
<body>
    <form id="formUserControlFolderListPolicy" runat="server">
    <asp:ScriptManager ID="scmMain" runat="server" ScriptMode="Release">
    </asp:ScriptManager>
    <infs:WclResourceManager ID="FolderListPolicyManager" runat="server">
        <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/UserControlFolderListForPolicy.js"
            ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Generic/popup.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Generic/popup.js" ResourceType="JavaScript" />
    </infs:WclResourceManager>
    <div class="popupContent">
        <input type="hidden" id="hdnCurrentNode" name="hdnSelectedNode" />
        <infs:WclTreeView ID="treeControlID" runat="server" OnClientNodeClicked="ClientNodeClicked">
            <DataBindings>
                <telerik:RadTreeNodeBinding Expanded="True" />
            </DataBindings>
        </infs:WclTreeView>
        <infsu:CommandBar ID="fsucUserControlFolderListPolicy" runat="server" DisplayButtons="Save,Cancel"
            OnCancelClientClick="ClosePopup" CancelButtonText="Close" CauseValidationOnCancel="false"
            OnSaveClientClick="returnToParent" ButtonPosition="Right" />
    </div>
    </form>
</body>
</html>
