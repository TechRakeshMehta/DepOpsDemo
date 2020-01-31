<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstituteHierarchyPackageList.aspx.cs" Inherits="CoreWeb.CommonOperations.Views.InstituteHierarchyPackageList" %>

<%@ Register Src="~/CommonControls/UserControl/PageBreadCrumb.ascx" TagName="PageBreadCrumb" TagPrefix="infsu" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Please Select An Institute Hierarchy Package</title>
    <style type="text/css">
        html {
            width: auto !important;
            height: auto !important;
            overflow: auto !important;
        }

        #treeInstituteHierarchyPackage .rtIn {
            cursor: pointer;
        }


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

</head>
<body>
    <form id="formInstituteHierarchyPackag" runat="server">
        <asp:ScriptManager ID="scmMain" runat="server" ScriptMode="Release">
        </asp:ScriptManager>
        <infs:WclResourceManager ID="InstituteHierarchyPackagManager" runat="server">
            <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
            <infs:LinkedResource Path="~/Resources/Mod/CommonOperations/InstituteHierarchyPackage.js" ResourceType="JavaScript" />
             <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
        </infs:WclResourceManager>
        <asp:HiddenField ID="hdnPackageNodeMappingID" runat="server" />
        <asp:HiddenField ID="hdnPackageName" runat="server" />
        <asp:HiddenField ID="hdnPackageID" runat="server" />
        <asp:HiddenField ID="hdnInstitutionHierarchyNodeID" runat="server" />
        <div style="height: 93%;">
            <div class="col-md-12" id="parentAgencyHierarchy">
                <div class="row">
                    <infs:WclTreeView ID="treeInstituteHierarchyPackage" runat="server" ClientKey="treeInstituteHierarchyPackage" Skin="Silk" OnNodeDataBound="TreeInstituteHierarchyPackage_NodeDataBound"
                        AutoSkinMode="false" OnClientNodeClicking="clientNodeClicking" OnNodeClick="TreeInstituteHierarchyPackage_NodeClick"
                        OnClientContextMenuShowing="ClientContextMenuShowing">
                        <DataBindings>
                            <telerik:RadTreeNodeBinding Expanded="false" />
                        </DataBindings>
                    </infs:WclTreeView>
                    <asp:Button ID="btnUpdateTree" runat="server" UseSubmitBehavior="false" CssClass="btnUpdateTree" />
                </div>
            </div>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </div>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;      
        <div style="width: 50%; z-index: 10; position: fixed; right: 0; bottom: 0; margin-top: 30%;">
            <div class="col-md-12">
                <div class="row text-right">
                    <infsu:CommandBar ID="fsucInstituteHierarchyPackageList" runat="server" DisplayButtons="Extra,Cancel"
                        OnCancelClientClick="ClosePopup" AutoPostbackButtons="Save" OnSaveClick="btnOk_Click" SaveButtonText="OK"
                        CancelButtonText="Cancel" OnExtraClientClick="OnClearClick" ExtraButtonText="Clear Selection"
                        CauseValidationOnCancel="false" EditModeButtons="Extra"
                        UseAutoSkinMode="false" ButtonSkin="Silk" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
