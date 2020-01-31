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
            <%--   <infs:LinkedResource Path="~/Resources/Generic/popup.css" ResourceType="StyleSheet" />--%>
            <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
            <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
            <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/InstitutionNodeHierarchyList.js"
                ResourceType="JavaScript" />
            <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

        </infs:WclResourceManager>
        <div style="height: 93%; padding-bottom:50px">
            <div class="col-md-12" id="divDept">
                <div class="row">
                    <%--Change this if resize the Popup Dimentions--%>
                    <infs:WclTreeView ID="treeDepartment" runat="server" ClientKey="treeDepartment" Skin="Silk"
                        AutoSkinMode="false" OnNodeDataBound="TreeDepartment_NodeDataBound" EnableAriaSupport="true"
                        OnClientContextMenuShowing="ClientContextMenuShowing" CheckBoxes="true" OnClientNodeClicked="GetSelectedNode"
                        OnClientNodeChecked="GetCheckedNode" OnClientNodeCollapsed="clientNodeCollapsed">
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
                <div class="row text-right " style="background-color: white; border-width: 1px; padding: 3px">
                    <%--Change this if resize the Popup Dimentions--%>
                    <infsu:CommandBar ID="fsucInstitutionHierarchyList" runat="server" DisplayButtons="Save,Extra,Cancel,Submit" class="sxcbar r"
                        AutoPostbackButtons="Save" OnCancelClientClick="ClosePopup" OnSaveClick="btnOk_Click"
                        ButtonSkin="Silk" UseAutoSkinMode="false" OnExtraClientClick="OnClearClick" CancelButtonText="Cancel" SubmitButtonText="Collapse" OnSubmitClientClick="clientNodeCollapsing" 
                        ExtraButtonText="Clear Selection" SaveButtonText="OK" ButtonPosition="Right"
                        CauseValidationOnCancel="false" />
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
        var tabKey = 9;
        function pageLoad() {

            $jQuery("[id$=divDept]").focus();
            $jQuery('input[type=checkbox]').each(function (index) {
                var span = $jQuery(this).siblings('.rtIn');
                span.attr('id', 'span_' + index);
                $jQuery(this).attr('aria-describedby', 'span_' + index);
                $jQuery(this).attr('role', 'checkbox');
                //$jQuery(this).on("keyup", function (e) {
                //    if (e.keyCode == 32)
                //    {
                //        var checkedStatus = $jQuery(this).prop('checked');
                //        $jQuery(this).attr('checked', !checkedStatus);
                //    }
                //});
            });

            //For accessibility, we need to prevent focus to go outside after tabbing on last link
            $jQuery(document).on("keydown", "#<%= fsucInstitutionHierarchyList.CancelButton.ClientID %>", function (e) {
                if (e.keyCode == tabKey && !e.shiftKey) {
                    e.preventDefault();
                    ($jQuery("a,button,:input:not([type=hidden]),[tabindex='0']").first()).focus();
                }
            });

            $jQuery("a,button,:input:not([type=hidden]),[tabindex='0']").first().keydown(function (e) {
                if (e.shiftKey && e.keyCode == tabKey) {
                    e.preventDefault();
                    $jQuery("[id$=<%= fsucInstitutionHierarchyList.CancelButton.ClientID %>]").focus();
                }
            });


            $jQuery(".fa.fa-arrow-right.right-arrow-color").removeClass();
            $jQuery(".rbPrimaryIcon.rbNext").removeClass();
     
            //$jQuery("[id$='<%= fsucInstitutionHierarchyList.CancelButton.ClientID %>']").attr('role', 'Button');
            //$jQuery("[id$='<%= fsucInstitutionHierarchyList.CancelButton.ClientID %>']").text('Cancel');
            //$jQuery("[id$='<%= fsucInstitutionHierarchyList.SaveButton.ClientID %>']").attr('role', 'Button');
            //$jQuery("[id$='<%= fsucInstitutionHierarchyList.SaveButton.ClientID %>']").text('OK');
            //$jQuery("[id$='<%= fsucInstitutionHierarchyList.ExtraButton.ClientID %>']").attr('role', 'Button');
            //$jQuery("[id$='<%= fsucInstitutionHierarchyList.ExtraButton.ClientID %>']").text('Clear Selection');
        }
    </script>
</body>
</html>
