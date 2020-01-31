<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManagePackageServiceHierarchy.aspx.cs"
    Title="Package Setup" MasterPageFile="~/Shared/DefaultMaster.master" Inherits="CoreWeb.BkgSetup.Views.ManagePackageServiceHierarchy" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/CommonControls/UserControl/PageBreadCrumb.ascx" TagName="PageBreadCrumb"
    TagPrefix="infsu" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadContent" runat="server">
    <style type="text/css">
        html, body, #frmmod, #UpdatePanel1, #box_content {
            height: 100% !important;
        }

        #pnlModHeader {
            display: none;
        }

        .child_pageframe {
            width: 100%;
            height: 95%;
        }

        .pane-client {
            background-color: transparent;
            height: 40px;
            position: absolute;
            top: 0;
            right: 10px;
        }

        .main_drop {
            /*width: 600px;*/
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

        .lbl_inst {
            display: inline-block;
            padding-top: 2px;
            vertical-align: top;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxSetUp">
        <infs:LinkedResource Path="~/Resources/Mod/BkgSetup/BkgSetup.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>

    <script type="text/javascript">

        $(document).ready(function () {
            $("#vertical").kendoSplitter({

            });

            $("#horizontal").kendoSplitter({
                panes: [

                    { collapsible: true, resizable: true, size: "300px", scrollable: true, min:"300px"}
                ]
            });
        });

    </script>
    <%--  <script type="text/javascript">
         function ClientContextMenuShowing(sender, eventArgs) {
             var treeNode = eventArgs.get_node();
             var parametersValue = treeNode.get_navigateUrl();

             if (parametersValue.indexOf("CompliancePackageID") > 0 && parametersValue.indexOf("SelectedTenantId") > 0) {
                 var parameters = treeNode.get_navigateUrl().split("&");
                 //Get selectedTenantId value
                 selectedTenantId = parameters[2].replace("SelectedTenantId=", '');
                 //Get compliancePackageID value CompliancePackageID for Package Node
                 compliancePackageID = parameters[3].replace("CompliancePackageID=", '');
                 treeNode.set_selected(true);
                 var _isAdminLoggedIn = "<%=IsAdminLoggedIn %>";

                //Hide "Create Master Copy for Master Tenant" and "Create  Client Copy for Tenant" conditionally
                if (selectedTenantId == "<%=DefaultTenantId %>" || _isAdminLoggedIn == "False") {
                    //Hide "Create Master Copy for Master Tenant"
                    var contextMenu = $find("<%=mnuTreePackage.ClientID %>");
                    contextMenu.findItemByValue("MenuItemCopyMaster").hide();
                }
                //                else {
                //                    //Hide "Create Client Copy for Tenant"
                //                    var contextMenu = $find("<%=mnuTreePackage.ClientID %>");
                //                    contextMenu.findItemByValue("MenuItemCopyClient").hide();
                //                }
            }
            else {
                //Disable the right click on the node 
                eventArgs.set_cancel(true);
            }
        }

    </script>--%>
    <asp:HiddenField ID="hdnIfDragPositonAbove" runat="server" Value="" />
    <asp:HiddenField ID="hdnScrollPosition" runat="server" Value="" />

    <div id="modwrapi">
        <div id="breadcmb">
            <infsu:PageBreadCrumb ID="breadcrum" runat="server" />
        </div>
        <div id="modhdr">
            <h1>
                <asp:Label Text="Screening Package Setup" runat="server" ID="lblModHdr" />
                <asp:Label Text="Screening Package Setup" runat="server" ID="lblPageHdr" CssClass="phdr" />
            </h1>
        </div>
    </div>
   
    <div id="paneTenant" runat="server">
        <div class="pane-client">
            <asp:Panel runat="server" CssClass="main_drop" ID="pnlTenant">
                <div>
                    <asp:UpdatePanel runat="server" ID="UpdatePanelPackage" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div style="float: left;" id="divTenant" runat="server">
                                <asp:Label ID="lblTenant" runat="server" Text="Institution" CssClass="lbl_inst common-splitter-font">
                                </asp:Label><span class='reqd'>*</span>
                                &nbsp;
                    <%--<infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="false" DataTextField="TenantName" OnDataBound="ddlTenant_DataBound"
                        DataValueField="TenantID" DefaultMessage="--Select--" OnClientSelectedIndexChanged="RefreshTree" Width="377px">
                    </infs:WclDropDownList>--%>
                                <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName" OnDataBound="ddlTenant_DataBound"
                                    DataValueField="TenantID" EmptyMessage="--SELECT--" Width="277px"
                                    Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged" OnClientSelectedIndexChanged="ClearRightPanel">
                                </infs:WclComboBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvTenant" ControlToValidate="ddlTenant"
                                        InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                        Text="Institution is required." />
                                </div>
                                <%--<asp:Panel runat="server"  ID="pnlBkgPackages">--%>
                            </div>
                            <div id="divPackage" style="float: left">
                                <div style="float: left; padding-left: 10px;">
                                    <%-- <asp:UpdatePanel runat="server" ID="UpdPackages" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                    <asp:Label ID="lblBkgPackages" runat="server" Text="Package" CssClass="lbl_inst common-splitter-font">
                                    </asp:Label>
                                    <%--<span class='reqd'>*</span>--%>
                                        &nbsp;
                                 <div style="float: right; padding-left: 3px;">
                                     <infs:WclComboBox ID="chkBkgPackages" runat="server" Width="277px" CheckBoxes="true" EmptyMessage="--SELECT--"
                                         AutoPostBack="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                                         <Localization CheckAllString="Select All" />
                                     </infs:WclComboBox>
                                     <%--Filter="Contains" EnableLoadOnDemand="true" OnClientKeyPressing="openCmbBoxOnTab"--%>
                                     <%-- <infs:WclDropDownList ID="chkBkgPackages" runat="server" Width="300px" Height="300px" EmptyMessage="--SELECT--"
                                         AutoPostBack="false">
                                     </infs:WclDropDownList>--%>

                                     <%--DataTextField="NameToDisplay" DataValueField="PackageID"--%>
                                     <div class="vldx">
                                         <asp:CustomValidator ID="rfvchkBkgPackages" CssClass="errmsg" Display="Dynamic" runat="server"
                                             ErrorMessage="Package is required." EnableClientScript="true" ValidationGroup="grpFormSubmit" Enabled="false"
                                             ClientValidationFunction="ValidateBackgroundPackage">
                                         </asp:CustomValidator>
                                     </div>
                                 </div>
                                    <%-- <asp:HiddenField ID="hdnBkgPackagesBind" runat="server" Value="0" />--%>

                                    <asp:HiddenField ID="hdnIsSearchClicked" runat="server" Value="0" />
                                    <%-- <asp:Button ID="btnBkgPackages" runat="server" UseSubmitBehavior="false" CssClass="btnUpdateTree" />--%>
                                    <%--  </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnBkgPackages" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>--%>
                                </div>
                                <div style="float: left; padding-left: 5px;">
                                    <infs:WclButton ID="fsucCmdBar" runat="server" AutoPostBack="false" Text="Search" ValidationGroup="grpFormSubmit" OnClientClicked="LoadTree">
                                        <Icon PrimaryIconCssClass="rbSearch" />
                                    </infs:WclButton>
                                    <%--<infsu:CommandBar ID="fsucCmdBar" runat="server" ButtonPosition="Center" DisplayButtons="Save" OnSaveClientClick="LoadTree"
                                            SaveButtonText="Search" SaveButtonIconClass="rbSearch" 
                                            ValidationGroup="grpFormSubmit">
                                        </infsu:CommandBar>--%>
                                    <%--OnSaveClick="CmdBarSearch_Click">--%>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTenant" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
            <%-- </asp:Panel>--%>
        </div>
    </div>
    <div id="vertical" style="height: 98%">
        <div id="top-pane">
            <div id="horizontal" style="height: 95%; width: 100%;">
                <div id="left-pane">
                    <asp:UpdatePanel runat="server" ID="updpnlTree" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnStoredData" runat="server" />
                            <asp:HiddenField ID="hdnIfDragEventIsFired" runat="server" Value="" />
                            <asp:HiddenField ID="hdnAsynchronousRequest" runat="server" Value="0" />
                            <infs:WclTreeView ID="treePackages" DataTextField="Name" DataFieldParentID="ParentNodeId" runat="server" ClientKey="treePackages" CssClass="tree-design"
                                OnNodeDataBound="treePackages_NodeDataBound" OnClientNodeClicked="GetSelectedNode">
                                <%--OnClientContextMenuShowing="ClientContextMenuShowing" OnClientContextMenuItemClicked="ClientContextMenuItemClicked"
                                OnClientLoad="SetTreeNode" EnableDragAndDrop="true" OnClientNodeClicked="GetSelectedNode"
                                OnClientNodeDropping="ClientNodeDropping" 
                                OnNodeDrop="treePackages_NodeDrop" >
                                <ContextMenus>
                                    <telerik:RadTreeViewContextMenu ID="mnuTreePackage" runat="server">
                                        <Items>
                                            <telerik:RadMenuItem Value="MenuItemCopy" Text="Create a copy">
                                            </telerik:RadMenuItem>
                                            <telerik:RadMenuItem Value="MenuItemCopyMaster" Text="Copy To Master">
                                            </telerik:RadMenuItem>
                                            <%--<telerik:RadMenuItem Value="MenuItemCopyClient" Text="Copy To Client">
                                            </telerik:RadMenuItem>
                                        </Items>
                                    </telerik:RadTreeViewContextMenu>
                                </ContextMenus>--%>
                            </infs:WclTreeView>
                            <asp:Button ID="btnUpdateTree" runat="server" UseSubmitBehavior="false" CssClass="btnUpdateTree" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnUpdateTree" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTenant" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>

                </div>
                <div id="right-pane">
                    <iframe name="childpageframe" id="ifrDetails" runat="server" class="child_pageframe"
                        src=""></iframe>
                    <asp:HiddenField ID="hdnSelectedNode" runat="server" Value="" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
