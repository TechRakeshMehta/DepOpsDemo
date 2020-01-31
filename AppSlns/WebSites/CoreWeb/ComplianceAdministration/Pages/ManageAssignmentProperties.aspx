<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.ManageAssignmentProperties"
    Title="ManageAssignmentProperties" MasterPageFile="~/Shared/DefaultMaster.master" CodeBehind="ManageAssignmentProperties.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/CommonControls/UserControl/PageBreadCrumb.ascx" TagName="breadcrumb"
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

        .align_pkg {
            vertical-align: middle;
        }

        .tree-pane {
            padding-top: 10px;
        }

            .tree-pane a, .tree-pane a:visited {
                color: Black;
                font-family: Arial;
            }
    </style>
    <script type="text/javascript">
        function createSplitter() {
                $("#vertical").kendoSplitter({

                });

                $("#horizontal").kendoSplitter({
                    panes: [

                        { collapsible: true, resizable: true, size: "300px", scrollable: true , min:"300px"}
                    ]
                });
        }

        function ResetTimer() {
            //debugger;
            var hdntimeout = $jQuery('[id$=hdntimeout]');  //, $jQuery(parent.theForm));
            if (hdntimeout != null) {
                var timeout = hdntimeout.val();
                parent.StartCountDown(timeout);
            }
        }

        function ValidatePackage(Sender, args) {
            //debugger;
            var cntrlToValidate = $jQuery("[id$=ddlTenant]")[0].value;
            if (cntrlToValidate) {
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
            else {
                args.IsValid = false;
                return false;
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <%--<infs:WclResourceManagerProxy runat="server" ID="rprxSetUp">
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Scripts/Kendo/jquery.min.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    --%>
        <div id="modwrapi">
            <div id="breadcmb">
                <infsu:breadcrumb ID="breadcrum" runat="server" />
            </div>
            <div id="modhdr">
                <h1>
                    <asp:Label Text="Compliance Setup" runat="server" ID="lblModHdr" />
                    &nbsp;<asp:Label
                        Text="Assignment Properties" runat="server" ID="lblPageHdr" CssClass="phdr" />
                </h1>
            </div>
        </div>
    <div id="paneTenant" runat="server">
    <div class="pane-client" id="divSearchPanel">
        <asp:Panel runat="server" CssClass="main_drop" ID="pnlTenant" Width="100%">
            <div style="float: left;" id="divTenant" runat="server">
                <asp:Label ID="lblTenant" runat="server" Text="Institution" CssClass="lbl_inst common-splitter-font"></asp:Label><span class='reqd'>*</span>
                &nbsp;
                    <%--<infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                        DataValueField="TenantID" DefaultMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged" width="377px">
                    </infs:WclDropDownList>--%>
                <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                    DataValueField="TenantID" EmptyMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged" Width="277px" AutoSkinMode="false"
                    Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                </infs:WclComboBox>
                <div class="vldx">
                    <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenant"
                        InitialValue="" Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSearch"
                        Text="Institution is required." />
                </div>
            </div>
            <div id="divPackage" runat="server" style="float: left;">
                <div style="float: left; padding-left: 10px;">
                    <asp:Label ID="lblPackage" runat="server" Text="Package" CssClass="lbl_inst align_pkg common-splitter-font">
                    </asp:Label><span class='reqd align_pkg'>*</span>
                    &nbsp;
                                        <div style="float: right; padding-left: 3px;">
                                            <infs:WclComboBox ID="ddlPackage" runat="server" AutoPostBack="false" DataTextField="PackageName"
                                                DataValueField="CompliancePackageID" EmptyMessage="--Select--" Width="277px" ValidationGroup="grpFormSearch"
                                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" CheckBoxes="true" Localization-CheckAllString="Select ALL">
                                            </infs:WclComboBox>
                                            <div class="vldx">
                                                <asp:CustomValidator ID="cvPackage" CssClass="errmsg" Display="Dynamic" runat="server"
                                                    ErrorMessage="Package(s) are required." ValidationGroup="grpFormSearch"
                                                    ClientValidationFunction="ValidatePackage" ControlToValidate="ddlPackage" ValidateEmptyText="true">
                                                </asp:CustomValidator>
                                                <%-- <asp:RequiredFieldValidator runat="server" ID="rfvPackage" ControlToValidate="ddlPackage"   
                                                     Display="Dynamic" ValidationGroup="grpFormSearch" CssClass="errmsg"
                                                      Text="Package is required." />--%>
                                            </div>
                                        </div>
                </div>
                <div style="float: left; padding-left: 5px;">
                    <infs:WclButton ID="btnSearch" runat="server" Text="Search" AutoPostBack="true" OnClick="btnSearch_Click"
                        ValidationGroup="grpFormSearch">
                        <Icon PrimaryIconCssClass="rbSearch" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                </div>
            </div>
        </asp:Panel>
    </div>
         </div>
    <div id="vertical" style="height: 98%">
        <div id="top-pane">
            <div id="horizontal" style="height: 95%; width: 100%;">
                <div id="left-pane">
                    <asp:UpdatePanel runat="server" ID="updpnlTree" UpdateMode="Conditional" Class="tree-pane">
                        <ContentTemplate>
                            <infs:WclTreeView ID="treeAssignmentProperties"
                                runat="server" ClientKey="treeAssignmentProperties" CssClass="tree-design"
                                OnNodeDataBound="treeAssignmentProperties_NodeDataBound">
                            </infs:WclTreeView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="right-pane">
                    <iframe name="childpageframe" id="ifrDetails" runat="server" class="child_pageframe"
                        src=""></iframe>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
