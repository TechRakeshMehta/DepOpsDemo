<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BkgOrderDetailPage.aspx.cs"
    Inherits="CoreWeb.BkgOperations.Views.BkgOrderDetailPage" MasterPageFile="~/Shared/DefaultMaster.master" %>

<%--<%@ Register TagName="Detail" TagPrefix="infsu" Src="~/BkgOperations/BkgOrderDetail.ascx" %>--%>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


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
            font-weight: bold;
            font-size: 13px;
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

        //function NavigateTo(iframeURL) {
        //    debugger;
        //    var hiddenField = $jQuery('[id$=hdnName]');
        //    hiddenField.val(menuID);
        //    $jQuery("[id$=pnRight]").scrollTop(-1);
        //    $jQuery("[id$=Iframe1]").attr('src', iframeURL);
        //}
       
        function NavigateTo(encryptedQueryString) {
            //debugger;
            var hiddenField = $jQuery('[id$=hdnName]');
            //hiddenField.val(menuID);
            $jQuery("[id$=pnRight]").scrollTop(-1);
            $jQuery("[id$=Iframe1]").attr('src', "/BkgOperations/Pages/BkgOrderDetailMain.aspx?args=" + encryptedQueryString.toString());
        }
    </script>
    <asp:PlaceHolder runat="server" ID="phDynamic"></asp:PlaceHolder>
    <%-- <infsu:Detail ID="Detail1" runat="server" />--%>


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
        <div class="col-md-12">
            <div class="row">
                <div id="summaryInner" tabindex="0">
                    <div class="icon flag">
                        <asp:Image ID="imgOrderFlag" ImageUrl="~/images/status/flag_06.png" Visible="false"
                            runat="server" Height="22px" Width="20px" />
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
                    <div class="icon user">
                    </div>
                    <div class="summaryBlock">
                        <asp:Label ID="lblUserName" runat="server" CssClass="highlighed" />
                        <span class="w_cptn">/</span>
                        <span class="w_cptn">Gender</span>
                        <asp:Label ID="lblGender" runat="server" CssClass="highlighed" />
                        <span class="w_cptn">/</span>
                        <span class="w_cptn">Birth</span>
                        <asp:Label ID="lblDOB" runat="server" CssClass="highlighed" />
                    </div>

                    <div class="icon payment">
                    </div>
                    <div class="summaryBlock">
                        <span class="w_cptn">Amount</span>
                        <asp:Label ID="lblAmount" runat="server" CssClass="highlighed" />
                        <span class="w_cptn">/</span>
                        <span class="w_cptn">Type</span>
                        <asp:Label ID="lblType" runat="server" CssClass="highlighed" TabIndex="0" />
                        <span class="w_cptn">/</span>
                        <span class="w_cptn">Status</span>
                        <asp:Label ID="lblPaymentStatus" runat="server" CssClass="highlighed" />
                    </div>
                    <div class="summaryBlock" style="float: right; font-weight: bold; font-size: 11px">
                        <a runat="server" id="lnkGoBack" onclick="Page.showProgress('Processing...');">
                            <asp:Label ID="lblLinkGoBack" runat="server" Text="" ></asp:Label>
                        </a>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <%--<table id="main-page">
    <tr>
        <td id="left-page">
            <telerik:RadListBox runat="server" Width="100%">
                <Items>
                    <telerik:RadListBoxItem Text="Order Details" Selected="true" />
                    <telerik:RadListBoxItem Text="External Vendor Services" />
                    <telerik:RadListBoxItem Text="AMS Packages Ordered" />
                    <telerik:RadListBoxItem Text="Pricing Info" />
                </Items>
            </telerik:RadListBox>
        </td>
        <td id="right-page">
            <iframe class="frame_orderpages" runat="server" id="ifrOrdPages" src="~/BkgOperations/Pages/OrdDetailsPage.aspx" />

        </td>
    </tr>
</table>--%>


    <infs:WclSplitter ID="sptrMain" runat="server" LiveResize="true" Orientation="Horizontal"
        Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true" Height="100%">
        <infs:WclPane ID="pnLower" runat="server" Scrolling="None" Width="100%" Height="100%">
            <infs:WclSplitter ID="sptrVeriOuter" runat="server" LiveResize="true" Orientation="Vertical"
                Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true"
                ClientKey="sptrVerification">
                <infs:WclPane ID="pnLeft" runat="server" Width="17%" MinWidth="200" PersistScrollPosition="true"
                    Collapsed="false" CssClass="pn-container" Height="100%"
                    Scrolling="None">
                    <%--<div class="section">
                    <div class="content">
                        <div class="sxform auto">--%>
                    <infs:WclListBox runat="server" ID="lstOrderDetailMenuItem" Width="100%" AutoPostBack="false"
                        CssClass="list_Order" OnItemDataBound="lstOrderDetailMenuItem_ItemDataBound"
                        DataKeyField="MenuID">
                        <ItemTemplate>
                            <div>
                                <a id="lnkOrderDetailMenuItem" runat="server" class="form-control removeUnderLine">
                                    <%# DataBinder.Eval(Container.DataItem, "MenuName")%>
                                </a>
                            </div>
                        </ItemTemplate>

                        <%--<HeaderTemplate>
                        <div style="padding: 5px; font-size: 14px">
                            Custom Forms
                        </div>
                    </HeaderTemplate>--%>
                    </infs:WclListBox>
                    <%--<telerik:RadListBox ID="RadListBox2" runat="server" Width="100%">
                                <Items>
                                    <telerik:RadListBoxItem Text="Order Details" Selected="true" />
                                    <telerik:RadListBoxItem Text="External Vendor Services" />
                                    <telerik:RadListBoxItem Text="AMS Packages Ordered" />
                                    <telerik:RadListBoxItem Text="Pricing Info" />
                                </Items>
                            </telerik:RadListBox>--%>
                    <%--</div>
                    </div>
                </div>--%>
                    <div id="errorMessageBox" class="msgbox" runat="server">
                        <asp:Label ID="lblError" runat="server" CssClass="error" Text="">
                        </asp:Label>
                    </div>
                </infs:WclPane>
                <infs:WclSplitBar ID="WclSplitBar1" runat="server" CollapseMode="forward" Height="480px">
                </infs:WclSplitBar>
                <infs:WclPane ID="pnRight" runat="server" Scrolling="Both" Width="100%">
                    <iframe height="81%" width="100%" runat="server" id="Iframe1" />
                </infs:WclPane>
            </infs:WclSplitter>
        </infs:WclPane>
    </infs:WclSplitter>
    <asp:HiddenField ID="hdnName" runat="server" />


</asp:Content>

