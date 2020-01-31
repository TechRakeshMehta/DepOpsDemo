<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgencyLocationDepartment.ascx.cs" Inherits="CoreWeb.PlacementMatching.Views.AgencyLocationDepartment" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxAgencyLocationDepartment">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Generic/popup.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style type="text/css">
    .upperPane {
        border-bottom-style: double;
        border-bottom-width: 1px;
        border-color: #8a8a8a;
    }

    .rootNode {
        margin: 10px;
        color: #000;
    }

    .lblRootNode {
        font-weight: bold;
        font-size: medium;
        color: DarkBlue;
    }

    a:hover {
        background-color: yellow;
    }

    .btnAgencyRootNodehover:hover {
        background-color: #2dabc1;
    }

    .btnLocationhover:hover {
        background-color: #2dabc1;
    }

    button, input, select, textarea {
        font-family: inherit;
        font-size: medium;
        line-height: inherit;
        margin-left: 10px;
    }

    .btnStyle {
        box-shadow: inset 0px 0px 3px #00687a;
        border-radius: 3px;
        width: auto;
        box-sizing: border-box;
        padding: 3px 10px;
    }

    .btnRepeaterStyle {
        box-shadow: inset 0px 0px 3px #00687a;
        border-radius: 3px;
        width: auto;
        box-sizing: border-box;
        padding: 3px 10px;
        margin-left: 30px;
        margin-bottom: 10px;
    }

    .bkgColor {
        background-color: #2dabc1;
    }

    .buttonHidden {
        display: none;
    }
</style>
<div class="container-fluid">
    <%--  <div class="row">
        <div class="col-md-12">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
        </div>
    </div>--%>
    <div class="row">
        <div class="col-md-12">
            <div class="page-header" style="background-color: #f2f2f2; margin-top: 0%">
                <h1 style="padding-left: 1%;">Agency Location Setup
                </h1>
            </div>
        </div>
    </div>

    <div class="container-fluid" style="height: 100vh">
        <infs:WclSplitter ID="sptrContentPage" runat="server" LiveResize="true" Orientation="Horizontal" Height="100%"
            Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
            <%--  <infs:WclPane ID="pnMainToolbar" runat="server" Width="100%" Height="15%" CssClass=""
                Scrolling="None" Collapsed="false">
               
            </infs:WclPane>--%>
            <%-- <infs:WclPane ID="paneTenant" runat="server" Scrolling="None" Width="100%" Height="10%">
            </infs:WclPane>--%>
            <infs:WclPane ID="pnMain" runat="server" Scrolling="None" Width="100%" Height="85%">
                <infs:WclSplitter ID="spltr" runat="server" LiveResize="true" Orientation="Vertical" Height="100%"
                    Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
                    <infs:WclPane ID="pnUpper" runat="server" Width="20%" Height="100%"
                        Scrolling="Y" Collapsed="false">
                        <div style="margin-top: 20px">
                            <div class="rootNode">
                                <asp:HiddenField ID="hdnAgencyRootNode" runat="server" Value='<%# Eval("AgencyRootNodeID") %>' />
                                <button id="btnAgencyRootNode" runat="server" type="button" class="btnStyle btnAgencyRootNodehover" onserverclick="btnAgencyRootNode_ServerClick" onclick='ChangeStyle(this,true);'><%#Eval("AgencyRootNode") %></button>
                            </div>
                            <asp:Repeater ID="rptrAgencyLocation" runat="server">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div>
                                        <asp:HiddenField ID="hdnLocationId" runat="server" Value='<%# Eval("AgencyLocationID") %>' />
                                        <button id="btnLocation" runat="server" type="button" class="btnRepeaterStyle btnLocationhover" onserverclick="btnLocation_ServerClick" style="font-size: medium;" onclick='ChangeStyle(this,false);'><%#Eval("Location") %></button>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        
                    </infs:WclPane>
                    <infs:WclSplitBar ID="spltBar" runat="server" CollapseMode="forward">
                    </infs:WclSplitBar>
                    <infs:WclPane ID="pnLower" runat="server" Scrolling="None" Width="80%">
                        <iframe name="childpageframe" id="ifrDetails" runat="server" class="child_pageframe" style="height: 100%; width: 100%"></iframe>
                        <asp:HiddenField ID="hdnSelectedNode" runat="server" Value="" />
                    </infs:WclPane>
                </infs:WclSplitter>
            </infs:WclPane>

        </infs:WclSplitter>
    </div>
</div>
<asp:HiddenField Value="0" runat="server" ID="hdnSelectedNodeID" />

<%--<Triggers>
    <asp:AsyncPostBackTrigger ></asp:AsyncPostBackTrigger>--%>
<asp:Button Id="btnDoPostBack" runat="server" CssClass="buttonHidden"/>
<%--</Triggers>--%>

<script type="text/javascript">

    function GetLocation(sender, eventArgs) {
        debugger;

    }
    $jQuery(document).ready(function () {
        //debugger;
        var btnAgencyRootNode = $jQuery("[id$=btnAgencyRootNode]");
        btnAgencyRootNode[0].style.backgroundColor = "#2dabc1";
        $jQuery("[id$=hdnSelectedNodeID]").val(btnAgencyRootNode[0].id);
    });

    function pageLoad() {
        //debugger;
        if ($jQuery("[id$=hdnSelectedNodeID]").val() != "0") {
            var selectedNode = $jQuery("[id$=" + $jQuery("[id$=hdnSelectedNodeID]").val() + "]");
            selectedNode[0].style.backgroundColor = "#2dabc1";
        }
    }

    function ChangeStyle(sender, isRootNodeButton) {
        //debugger;
        if (isRootNodeButton == true) {
            var btnAgencyRootNode = $jQuery("[id$=btnAgencyRootNode]");
            $jQuery("[id$=hdnSelectedNodeID]").val(btnAgencyRootNode[0].id);
            //btnAgencyRootNode[0].style.backgroundColor = "#2dabc1";
        }
        else {
            if ($jQuery(sender).length > 0) {
                var currentButton = $jQuery(sender)[0]; //$jQuery("[$id=" + $jQuery(sender)[0].id + "]");//
                //currentButton.style.backgroundColor = "#2dabc1";
                $jQuery("[id$=hdnSelectedNodeID]").val(currentButton.id);
                //var btnAgencyRootNode = $jQuery("[id$=btnAgencyRootNode]");
                //btnAgencyRootNode[0].style.backgroundColor = "";
            }
        }
    }


</script>
