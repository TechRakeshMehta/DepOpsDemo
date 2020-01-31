<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientAdminBkgOrderDetailPage.aspx.cs"
    Inherits="CoreWeb.BkgOperations.Views.ClientAdminBkgOrderDetailPage" MasterPageFile="~/Shared/DefaultMaster.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<%@ Register TagName="Detail" TagPrefix="infsu" Src="~/BkgOperations/UserControl/ClientAdminOrderDetail.ascx" %>--%>

<%--<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <asp:PlaceHolder runat="server" ID="phDynamic"></asp:PlaceHolder>
    <infsu:Detail ID="Detail1" runat="server" />
</asp:Content>--%>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server"> 
    <style type="text/css">
        #pageoutwr
        {
            overflow: hidden;
        }

        #box_content
        {
            height: 480px;
        }

        a.cat_lslnk
        {
            text-decoration: none;
            width: 100%;
            height: 100%;
            display: block;
            color: #000;
        }

            a.cat_lslnk:visited
            {
                color: #000;
            }

            a.cat_lslnk:hover
            {
                color: blue;
            }

        .list_Order .rlbItem
        {
            border-bottom: 1px solid #D3D3D3;
            padding: 3px;
        }
        #summaryBar .highlighed
        {
            font-weight:bold;
            font-size: 14px;
        }

        #summaryInner
        {
            min-width: 1150px;
        }

        #summaryBar
        {
            font-size: 11.5px;
        }

        .summaryBlock
        {
            margin: 4px;
        }
    </style>

    <script type="text/javascript">

        //function LoadOrderControls(menuID) {
        //    var hiddenField = $jQuery('[id$=hdnName]');
        //    hiddenField.val(menuID);
        //    $jQuery("[id$=pnRight]").scrollTop(-1);
        //    $jQuery("[id$=Iframe1]").attr('src', "/BkgOperations/Pages/ClientAdminBkgOrderDetailPageMain.aspx?menuID=" + menuID);
        //}
        function pageLoad() {
           
            //UAT-1955
            $jQuery("[id$=lnkOrderDetailMenuItem]").on("keyup", function (e) {                
                if (e.keyCode == 13)
                {
                    $jQuery(this).click();
                }
            });
        }
        function LoadOrderControls(encryptedQueryString) {            
            var hiddenField = $jQuery('[id$=hdnName]');
            //hiddenField.val(menuID);
            $jQuery("[id$=pnRight]").scrollTop(-1);
            $jQuery("[id$=Iframe1]").attr('src', "/BkgOperations/Pages/ClientAdminBkgOrderDetailPageMain.aspx?args=" + encryptedQueryString.toString());
            //UAT-1955
            $jQuery("[id$=Iframe1]").focus();
        }

    </script>

    <asp:PlaceHolder runat="server" ID="phDynamic"></asp:PlaceHolder>

    <infs:WclResourceManagerProxy runat="server" ID="rprxAMSOrdersDetails">
        <infs:LinkedResource Path="~/Resources/Mod/AMSModules/BkgOperations/main.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>

    <div id="summaryBar">
        <div id="summaryInner" tabindex="0">
            <div class="icon flag" id="dvBkgOrderColorFlag" runat="server">
                <asp:Image ID="imgOrderFlag" ImageUrl="~/images/status/flag_03.png" runat="server"
                    Height="22px" Width="20px" />
            </div>

            <div class="summaryBlock">
                <span class="w_cptn">Order</span>
                <asp:Label ID="lnkOrderNo" runat="server" CssClass="highlighed" />
                <span class="w_cptn">/</span>
            <span class="w_cptn">Created</span>
                <asp:Label ID="lblCreated" runat="server" CssClass="highlighed" />
                <span class="w_cptn">/</span>
            <span class="w_cptn">Status</span>
                <asp:Label ID="lblStatus" runat="server" CssClass="highlighed" />
            </div>
            <div title="User icon" class="icon user">
            </div>
            <div class="summaryBlock">
                <asp:Label ID="lblUserName" runat="server" CssClass="highlighted" /> 
            <span class="w_cptn">/</span>
            <span class="w_cptn">Gender</span>
                <asp:Label ID="lblGender" runat="server" CssClass="highlighed" />
                <span class="w_cptn">/</span>
            <span class="w_cptn">Birth</span>
                <asp:Label ID="lblDOB" runat="server" CssClass="highlighed" />
            </div>

            <div title="Dollar icon" class="icon payment">
            </div>
            <div class="summaryBlock">
                <span class="w_cptn">Amount</span>
                <asp:Label ID="lblAmount" runat="server" CssClass="highlighed" />
                <span class="w_cptn">/</span>
            <span class="w_cptn">Type</span>
                <asp:Label ID="lblType" runat="server" CssClass="highlighed" />
                <span class="w_cptn">/</span>
            <span class="w_cptn">Status</span>
                <asp:Label ID="lblPaymentStatus" runat="server" CssClass="highlighed" />
            </div>
            <%--<div class="summaryBlock" style="float: right; font-weight: bold; font-size: 11px">
                <a runat="server" id="lnkGoBack" onclick="Page.showProgress('Processing...');">Back to Order Queue</a>
                <a runat="server" id="lnkGoBack" onclick="Page.showProgress('Processing...');">
                    <asp:Label ID="lblLinkGoBack" runat="server" Text=""></asp:Label>
                </a>
            </div>--%>
            <div class="summaryBlock" style="float: right; font-weight: bold; font-size: 11px">
                <a runat="server" id="lnkGoBack" onclick="Page.showProgress('Processing...');">
                    <asp:Label ID="lblLinkGoBack" runat="server" Text=""></asp:Label>
                </a>
            </div>
        </div>
    </div>

    <infs:WclSplitter ID="sptrMain" runat="server" LiveResize="true" Orientation="Horizontal" 
        Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true" Height="100%">
        <infs:WclPane ID="pnLower" runat="server" Scrolling="None" Width="100%" Height="100%">
            <infs:WclSplitter ID="sptrVeriOuter" runat="server" LiveResize="true" Orientation="Vertical"
                Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true"
                ClientKey="sptrVerification">
                <infs:WclPane ID="pnLeft" runat="server" Width="17%" MinWidth="200" PersistScrollPosition="true"
                    Collapsed="false" CssClass="pn-container" Height="100%" 
                    Scrolling="None">
                    <infs:WclListBox runat="server" ID="lstOrderDetailMenuItem" Width="100%" AutoPostBack="false"
                        CssClass="list_Order" OnItemDataBound="lstOrderDetailMenuItem_ItemDataBound" 
                        DataKeyField="Key">
                        <ItemTemplate>
                            <div>
                                <a id="lnkOrderDetailMenuItem" runat="server" class="form-control removeUnderLine" tabindex="0">
                                    <%# DataBinder.Eval(Container.DataItem, "Value")%>
                                </a>
                            </div>
                        </ItemTemplate>
                    </infs:WclListBox>
                    <div id="errorMessageBox" class="msgbox" runat="server">
                        <asp:Label ID="lblError" runat="server" CssClass="error" Text="">
                        </asp:Label>
                    </div>
                </infs:WclPane>
                <infs:WclSplitBar ID="WclSplitBar1" runat="server" CollapseMode="forward" Height="480px">
                </infs:WclSplitBar>
                <infs:WclPane ID="pnRight" runat="server" Scrolling="Both" Width="100%">
                    <iframe runat="server" id="Iframe1" height="81%" width="100%" />
                </infs:WclPane>
            </infs:WclSplitter>
        </infs:WclPane>
    </infs:WclSplitter>
</asp:Content>


