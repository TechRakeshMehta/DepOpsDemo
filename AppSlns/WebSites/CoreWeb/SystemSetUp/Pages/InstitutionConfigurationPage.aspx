<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.SystemSetUp.Views.InstitutionConfigurationPage" CodeBehind="InstitutionConfigurationPage.aspx.cs"
    MasterPageFile="~/Shared/DefaultMaster.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/CommonControls/UserControl/PageBreadCrumb.ascx" TagName="PageBreadCrumb"
    TagPrefix="infsu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadContent" runat="server">
    <style type="text/css">
        html, body, #frmmod, #UpdatePanel1, #box_content
        {
            height: 100% !important;
        }

        .child_pageframe
        {
            width: 100%;
            height: 100%;
        }

        .pane-client
        {
            background-color: transparent;
            height: 40px;
            position: absolute;
            top: 0;
            right: 10px;
        }

        .main_drop
        {
            width: 457px;
            margin-top: 12px;
            color: #000;
        }

        .tree-pane
        {
            padding-top: 10px;
        }

            .tree-pane a, .tree-pane a:visited
            {
                color: Black;
                font-family: Arial;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxSetUp">
        <infs:LinkedResource Path="~/Resources/Mod/SystemSetUp/InstitutionConfiguration.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/Breadcrumb.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy> 
    <asp:HiddenField ID="hdnTenantId" runat="server" />

    <infs:WclSplitter ID="sptrContentPage" runat="server" LiveResize="true" Orientation="Horizontal"
        Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">

        <infs:WclPane ID="paneTenant" runat="server" Scrolling="None" Height="50" Width="100%">
            <div class="container-fluid" id="dvSharedUserBreadcrumb">
                <div class="col-md-6" id="dvTenant" runat="server">
                    <div class="row">
                        <asp:Panel runat="server" ID="pnlTenant">
                            <div class="row">
                                <div class="form-group col-md-12">
                                    <span class="cptn">
                                        <asp:Label ID="lblTenant" runat="server" Text="Institution"></asp:Label></span>
                                    <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="false" DataTextField="TenantName"
                                        OnDataBound="ddlTenant_DataBound"
                                        DataValueField="TenantID" Width="377px" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged"
                                        Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" CssClass="form-control" Skin="Silk"
                                        AutoSkinMode="false">
                                    </infs:WclComboBox>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
                <div class="col-md-6 pull-right">
                    <div class="row">
                        <h1 class="text-right">
                            <asp:Label Text="Institution Configuration" runat="server" ID="lblModHdr" />
                            &nbsp;<asp:Label Text="Institution Configuration Details" runat="server" ID="lblPageHdr"
                                CssClass="phdr" />
                        </h1>
                        <div class="breadcrumb padbott10 text-right">
                            <infsu:PageBreadCrumb ID="breadcrum" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </infs:WclPane>
        <infs:WclPane ID="pnMainToolbar" Visible="false" runat="server" MinHeight="50" Height="50"
            Width="100%"
            Scrolling="None" Collapsed="false">
        </infs:WclPane>

        <infs:WclPane ID="pnMain" runat="server" Scrolling="None" Width="100%">
            <infs:WclSplitter ID="sptrPage" runat="server" LiveResize="true" Orientation="Vertical"
                Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
                <infs:WclPane ID="pnUpper" runat="server" Height="100%" Width="350" MinWidth="300"
                    Scrolling="Y" Collapsed="false">
                    <asp:UpdatePanel runat="server" ID="updpnlTree" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnDefaultScreen" runat="server" />
                            <asp:HiddenField ID="hdnTenantIsChanged" runat="server" Value="" />
                            <div class="container-fluid">
                                <infs:WclTreeView ID="treeDepartment" runat="server" ClientKey="treeDepartment" OnNodeDataBound="TreeDepartment_NodeDataBound"
                                    OnClientLoad="SetTreeNode" OnClientNodeClicked="GetSelectedNode"
                                    Width="100%" Skin="Silk" AutoSkinMode="false">
                                </infs:WclTreeView>

                                <asp:Button ID="btnUpdateTree" runat="server" UseSubmitBehavior="false" CssClass="btnUpdateTree"
                                    Width="0%" />
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnUpdateTree" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </infs:WclPane>
                <infs:WclSplitBar ID="WclSplitBar1" runat="server" CollapseMode="forward">
                </infs:WclSplitBar>
                <infs:WclPane ID="pnLower" runat="server" Scrolling="None" Width="100%">
                    <iframe name="childpageframe" id="ifrDetails" runat="server" width="100%" height="100%"
                        src=""></iframe>
                    <asp:HiddenField ID="hdnSelectedNode" runat="server" Value="" />
                </infs:WclPane>
            </infs:WclSplitter>
        </infs:WclPane>
    </infs:WclSplitter>

</asp:Content>

