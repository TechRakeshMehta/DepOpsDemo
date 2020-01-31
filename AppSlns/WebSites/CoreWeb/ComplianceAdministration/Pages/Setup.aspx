<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.Setup"
    Title="Setup" MasterPageFile="~/Shared/DefaultMaster.master" CodeBehind="Setup.aspx.cs" %>

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
            /*width: 457px;*/
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
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/SetUp.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/KeyBoardSupport.js" ResourceType="JavaScript" />
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

                        contextMenu.findItemByValue("MenuItemCopyOtherClient").hide();
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

        //UAT-1116: Package selection combo box on package screens
        //To bind tree
        function BindTree(e) {
            $jQuery("[id$=hdnIsSearchClicked]").val('true');
            //if (ValidatePackageDropdown()) { //Commented code to display the right panel after intitution selection package is not required.
            RefreshTree();
            //}
        }

        //UAT-1116: Package selection combo box on package screens
        //Function to validate Package checkbox selection
        function ValidatePackage(Sender, args) {
            var checkedItems = $jQuery("[id$=ddlPackage]")[0].control.get_checkedItems();
            if (checkedItems.length > 0) {
                args.IsValid = true;
                return true;
            }
            else {
                args.IsValid = false;
                return false;
            }
        }

    </script>
    <asp:HiddenField ID="hdnIfDragPositonAbove" runat="server" Value="" />
    <asp:HiddenField ID="hdnScrollPosition" runat="server" Value="" />
    <div id="modwrapi">
        <div id="breadcmb">
            <infsu:PageBreadCrumb ID="breadcrum" runat="server" />
        </div>
        <div id="modhdr">
            <h1>
                <asp:Label Text="Compliance Setup" runat="server" ID="lblModHdr" />
                <asp:Label Text="Manage Mappings" runat="server" ID="lblPageHdr" CssClass="phdr" />
            </h1>
        </div>
    </div>
    <div id="paneTenant" runat="server">
        <div class="pane-client" id="divSearchPanel">
            <asp:Panel runat="server" CssClass="main_drop" ID="pnlTenant">
                <div>
                    <asp:UpdatePanel runat="server" ID="UpdatePanelPackage" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div style="float: left;" id="divTenant" runat="server">
                                <asp:Label ID="lblTenant" runat="server" Text="Institution" CssClass="lbl_inst common-splitter-font">
                                </asp:Label>
                                <%--<span runat="server" class="lbl_inst common-splitter-font">Institution</span>--%><span class='reqd'>*</span>
                                &nbsp;
                            <%-- UAT-1116: <infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="false" DataTextField="TenantName"
                                DataValueField="TenantID" DefaultMessage="--Select--" OnClientSelectedIndexChanged="RefreshTree" width="377px">
                            </infs:WclDropDownList>--%>
                                <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                                    DataValueField="TenantID" EmptyMessage="--Select--" Width="277px" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged" OnClientSelectedIndexChanged="ClearRightPanel"
                                    Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                                </infs:WclComboBox>
                                <%--OnClientSelectedIndexChanged="BindPackageDropdown"--%>
                            </div>
                            <%--UAT-1116--%>
                            <div id="divPackage" runat="server" style="float: left">
                                <div style="float: left; padding-left: 10px;">
                                    <asp:Label ID="lblPackage" runat="server" Text="Package" CssClass="lbl_inst common-splitter-font">
                                    </asp:Label>
                                    <%-- <span class='reqd'>*</span>--%>&nbsp;
                                        <div style="float: right; padding-left: 3px;">
                                            <infs:WclComboBox ID="ddlPackage" runat="server" AutoPostBack="false" DataTextField="PackageName"
                                                DataValueField="CompliancePackageID" EmptyMessage="--Select--" Width="277px"
                                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" CheckBoxes="true" Localization-CheckAllString="Select ALL">
                                            </infs:WclComboBox>
                                            <div class="vldx">
                                                <asp:CustomValidator ID="cvPackage" CssClass="errmsg" Display="Dynamic" runat="server" Enabled="false"
                                                    ErrorMessage="Package is required." EnableClientScript="true" ValidationGroup="grpFormSearch"
                                                    ClientValidationFunction="ValidatePackage">
                                                </asp:CustomValidator>
                                            </div>
                                        </div>
                                </div>
                                <div style="float: left; padding-left: 5px;">
                                    <infs:WclButton ID="btnSearch" runat="server" Text="Search" AutoPostBack="false" OnClientClicked="BindTree"
                                        ValidationGroup="grpFormSearch">
                                        <Icon PrimaryIconCssClass="rbSearch" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                                            PrimaryIconWidth="14" />
                                    </infs:WclButton>
                                    <asp:HiddenField ID="hdnIsSearchClicked" runat="server" Value="" />
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTenant" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
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
                            <infs:WclTreeView ID="treePackages" runat="server" ClientKey="treePackages" OnNodeDataBound="treePackages_NodeDataBound"
                                OnClientContextMenuShowing="ClientContextMenuShowing" OnClientContextMenuItemClicked="ClientContextMenuItemClicked" ForeColor="Black"
                                OnClientLoad="SetTreeNode" EnableDragAndDrop="true" OnClientNodeClicked="GetSelectedNode" CssClass="tree-design"
                                OnClientNodeDropping="ClientNodeDropping"
                                OnNodeDrop="treePackages_NodeDrop">
                                <ContextMenus>
                                    <telerik:RadTreeViewContextMenu ID="mnuTreePackage" runat="server">
                                        <Items>
                                            <telerik:RadMenuItem Value="MenuItemCopy" Text="Create a copy">
                                            </telerik:RadMenuItem>
                                            <telerik:RadMenuItem Value="MenuItemCopyMaster" Text="Copy To Master">
                                            </telerik:RadMenuItem>
                                            <%--<telerik:RadMenuItem Value="MenuItemCopyClient" Text="Copy To Client">
                                            </telerik:RadMenuItem>--%>
                                            <telerik:RadMenuItem Value="MenuItemCopyOtherClient" Text="Copy to Other Client">
                                            </telerik:RadMenuItem>
                                        </Items>
                                    </telerik:RadTreeViewContextMenu>
                                </ContextMenus>
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
