<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlacementDashboard.ascx.cs" Inherits="CoreWeb.PlacementMatching.Views.PlacementDashboard" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/PlacementMatching/UserControl/CalenderViewControl.ascx" TagPrefix="infsu" TagName="CalenderViewControl" %>
<%@ Register Src="~/PlacementMatching/UserControl/GridViewControl.ascx" TagPrefix="infsu" TagName="GridViewControl" %>


<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<style type="text/css">
    .dvcss {
        text-align: center;
        margin-right: -3px;
        width: 16.3%;
        cursor: pointer;
    }

    .toptext {
        font-size: medium;
        font-weight: 600;
    }

    .bottomtext {
        float: left;
        width: 100%;
    }

    .btngridcss {
        background: none;
        border: 1px solid black;
    }

    .statusBackground {
        background-color: #e2e9ec;
    }

    .togglebutton {
        width: 50%;
        height: 100%;
        font-size: 16px;
        color: white;
        float: left;
        text-align: center;
    }

    #box_content i, #box_content em {
        font-style: normal;
    }
</style>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-3">
                    <span class='cptn'>Agency</span>
                    <infs:WclComboBox ID="ddAgency" runat="server" DataTextField="AgencyName" EmptyMessage="--Select--" OnSelectedIndexChanged="ddAgency_SelectedIndexChanged"
                        AllowCustomText="true" CausesValidation="false" DataValueField="AgencyID"
                        Filter="Contains" Width="100%" Skin="Silk" AutoSkinMode="false" AutoPostBack="true">
                    </infs:WclComboBox>
                </div>
            </div>
        </div>
    </div>

    <div class="row">&nbsp;</div>
    <div class="row">
        <div class="col-md-3">
        </div>
        <div class="col-md-12">
            <div id="Requested" class="dvcss" style="display: inline-block; border-style: solid; border-width: 1px; border-spacing: 0px; background-color: #F9E79F;" runat="server" >
                <span class="toptext">REQUESTED </span>
                <span class="bottomtext" id="spnPendingRequestCount" style="font-size: xx-large; font-weight: 900" runat="server">0</span>
            </div>
            <div id="Modified" class="dvcss" style="display: inline-block; border-style: solid; border-width: 1px; border-spacing: 0px; background-color: #b1e1f9" runat="server" >
                <span class="toptext">MODIFIED </span>
                <span class="bottomtext" id="spnModifiedRequestCount" style="font-size: xx-large; font-weight: 900" runat="server">0</span>
            </div>
            <div id="Approved" runat="server" class="dvcss"  style="display: inline-block; border-style: solid; border-width: 1px; border-spacing: 0px; background-color: #ceffba">
                <span class="toptext">APPROVED </span>
                <span class="bottomtext" id="SpnApprovedRequestCount" style="font-size: xx-large; font-weight: 900" runat="server">0</span>
            </div>
            <div id="Rejected" class="dvcss" style="display: inline-block; border-style: solid; border-width: 1px; border-spacing: 0px; background-color: #ffc27e;" runat="server" >
                <span class="toptext">REJECTED </span>
                <span class="bottomtext" id="SpnRejectedRequestCount" style="font-size: xx-large; font-weight: 900" runat="server">0</span>
            </div>
            <div id="Archived" class="dvcss" style="display: inline-block; border-style: solid; border-width: 1px; border-spacing: 0px; background-color: #bababa;" runat="server" >
                <span class="toptext">ARCHIVED </span>
                <span class="bottomtext" id="SpnArchivedRequestCount" style="font-size: xx-large; font-weight: 900" runat="server">0</span>
            </div>
            <div id="Cancelled" class="dvcss" style="display: inline-block; border-style: solid; border-width: 1px; border-spacing: 0px; background-color: #ebccd1;" runat="server" >
                <span class="toptext">CANCELLED </span>
                <span class="bottomtext" id="SpnCancelledRequestCount" style="font-size: xx-large; font-weight: 900" runat="server">0</span>
            </div>

        </div>
    </div>

    <div class="row">&nbsp;</div>
    <div class="row">
        <div class="col-md-3" style="margin-left: 45px">
            <span id="spnStatus"  runat="server" style="font-size: medium"></span>
            <asp:Label runat="server" Style="font-weight: bold; font-size: medium;" Text="10" ID="lblstatusCount"></asp:Label>
        </div>
        <%-- <div class="col-md-9">
            <div class="col-md-6"></div>
            <div class="col-md-3">
                <asp:Button ID="btnGrid" CssClass="togglebutton" runat="server" Text="Grid" OnClick="btnGrid_Click" />
                <asp:Button ID="btnCalender" CssClass="togglebutton" runat="server" Text="Calender" OnClick="btnCalender_Click" />
            </div>
        </div>--%>
    </div>
    <div class="row">&nbsp;</div>
    <div class="row">
        <div class="col-md-12">
            <div>
                <div id="dvCalenderView" runat="server">
                    <infsu:CalenderViewControl runat="server" ID="CalenderViewControl" />
                </div>
                <div id="dvGridView" runat="server">
                    <infsu:GridViewControl runat="server" ID="GridViewControl" />
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnSelectedStatus" runat="server" />
    <asp:HiddenField ID="hdnStatusCount" runat="server" />
    <asp:HiddenField ID="hdnSelectedStatusCode" runat="server" />
    <asp:Button CssClass="buttonHidden" ID="btnDoPostBack" runat="server"  OnClick="btnDoPostBack_Click"/>
    <%--OnClick="btnDoPostBack_Click"--%>
    <%--<infs:WclComboBox ID="ddlTenant" Visible="false" runat="server" EnableCheckAllItemsCheckBox="false" CheckBoxes="true"
        CssClass="form-control" Width="100%" AutoPostBack="false" DataTextField="TenantName"
        DataValueField="TenantID" EmptyMessage="--Select--" Skin="Silk" AutoSkinMode="false"
        Filter="Contains">
    </infs:WclComboBox>--%>
</div>
<script type="text/javascript">
    function statusClick(status,statuscode,count) {
        //debugger;
        $jQuery("[id$=hdnSelectedStatusCode]").val(statuscode);
        $jQuery("[id$=hdnSelectedStatus]").val(status);
        $jQuery("[id$=hdnStatusCount]").val(count);
        $jQuery("[id$=spnStatus]").text(status.toUpperCase() + ":");
        $jQuery("[id$=lblstatusCount]").text(count);
        $jQuery("#<%=btnDoPostBack.ClientID %>").trigger('click');
<%--        __doPostBack("<%= btnDoPostBack.ClientID %>", "");--%>

    }

    function SearchRequestPopUp(requestId, opportunityId, RequestedPage) {
        var composeScreenWindowName = "Request Details";
        var popupHeight = $jQuery(window).height() * (100 / 100);
        var url = $page.url.create("~/PlacementMatching/Pages/CreateRequestPopup.aspx?RequestId=" + requestId + "&OpportunityId=" + opportunityId + "&RequestedPage=" + RequestedPage);
        var win = $window.createPopup(url, { size: "1000," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
        winopen = true;
        return false;
    }

    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
                //do button postback to show success message.
                //$jQuery("[id$=hdnIsSavedSuccessfully]").val(arg.IsSavedSuccessfully);
                //$jQuery("[id$=hdnIsPublishedSuccessfully]").val(arg.IsPublishedSuccessfully);
                // $jQuery("#<%=btnDoPostBack.ClientID %>").trigger('click');
            }
            winopen = false;
        }

    }

    function pageLoad() {
        //debugger;
        var status = $jQuery("[id$=hdnSelectedStatus]").val();
        var count = $jQuery("[id$=hdnStatusCount]").val();
        //  $jQuery("[id$=" + status + "]").addClass("statusBackground");
        $jQuery("[id$=spnStatus]").text(status.toUpperCase() + ":");
        var adc = $jQuery("[id$="+status+"]")
        $jQuery("[id$=lblstatusCount]").text(count);


    }
</script>


