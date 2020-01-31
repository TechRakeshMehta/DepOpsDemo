<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.SetupShotSeries"
    Title="SetupShotSeries" MasterPageFile="~/Shared/DefaultMaster.master" CodeBehind="SetupShotSeries.aspx.cs" %>

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
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/SetUpShotSeries.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <script type="text/javascript">

        //To bind tree
        function BindTree(e) {
            $jQuery("[id$=hdnIsSearchClicked]").val('true');
            RefreshTree();
        }
    </script>


    <infs:WclSplitter ID="sptrContentPage" runat="server" LiveResize="true" Orientation="Horizontal"
        Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
        <infs:WclPane ID="pnMainToolbar" runat="server" MinHeight="50" Height="50" Width="100%"
            Scrolling="None" Collapsed="false">
            <div id="modwrapo">
                <div id="modwrapi">
                    <div id="breadcmb">
                        <infsu:PageBreadCrumb ID="breadcrum" runat="server" />
                    </div>
                    <div id="modhdr">
                        <h1>
                            <asp:Label Text="Shot Series Setup" runat="server" ID="lblModHdr" />
                            <asp:Label Text="Manage Shot Series" runat="server" ID="lblPageHdr" CssClass="phdr" />
                        </h1>
                    </div>
                </div>
            </div>
        </infs:WclPane>
        <infs:WclPane ID="paneTenant" runat="server" Scrolling="None" Height="1px" Width="100%">
            <div class="pane-client">
                <asp:Panel runat="server" CssClass="main_drop" ID="pnlTenant">
                    <div>
                        <asp:UpdatePanel runat="server" ID="UpdatePanelPackage" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div style="float: left;" id="divTenant" runat="server">
                                    <asp:Label ID="lblTenant" runat="server" Text="Institution" CssClass="lbl_inst">
                                    </asp:Label><span class='reqd'>*</span>
                                    <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                                        DataValueField="TenantID" EmptyMessage="--Select--" Width="277px" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                                        Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                                    </infs:WclComboBox>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlTenant" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>
            </div>
        </infs:WclPane>
        <infs:WclPane ID="pnMain" runat="server" Scrolling="None" Width="100%">
            <infs:WclSplitter ID="sptrPage" runat="server" LiveResize="true" Orientation="Vertical"
                Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
                <infs:WclPane ID="pnUpper" runat="server" Height="100%" Width="300" MinWidth="300"
                    Scrolling="Y" Collapsed="false" CssClass="tree-pane">
                    <asp:UpdatePanel runat="server" ID="updpnlTree" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnStoredData" runat="server" />

                            <infs:WclTreeView ID="treeShotSeries" runat="server" ClientKey="treeShotSeries" OnNodeDataBound="treeShotSeries_NodeDataBound"
                               OnClientLoad="SetTreeNode" OnClientNodeClicked="GetSelectedNode">
                            </infs:WclTreeView>
                            <asp:Button ID="btnUpdateTree" runat="server" UseSubmitBehavior="false" CssClass="btnUpdateTree" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnUpdateTree" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTenant" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </infs:WclPane>
                <infs:WclSplitBar ID="WclSplitBar1" runat="server" CollapseMode="forward">
                </infs:WclSplitBar>
                <infs:WclPane ID="pnLower" runat="server" Scrolling="None" Width="100%">
                    <iframe name="childpageframe" id="ifrDetails" runat="server" class="child_pageframe"
                        src=""></iframe>
                    <asp:HiddenField ID="hdnSelectedNode" runat="server" Value="" />
                </infs:WclPane>
            </infs:WclSplitter>
        </infs:WclPane>
    </infs:WclSplitter>
</asp:Content>
