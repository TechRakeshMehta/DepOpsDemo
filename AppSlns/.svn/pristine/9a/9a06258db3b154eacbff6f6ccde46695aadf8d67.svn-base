<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageApplicantDocument.ascx.cs" Inherits="CoreWeb.ComplianceOperations.ManageApplicantDocument" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/ComplianceOperations/UserControl/ManageUploadDocument.ascx" TagPrefix="infsu" TagName="ManageUploadDocument" %>
<%@ Register Src="~/ComplianceOperations/UserControl/ManagePersonalDocument.ascx" TagPrefix="infsu" TagName="ManagePersonalDocument" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <%--<infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />--%>
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <%--<infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />--%>
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<style type="text/css">
    .nav-tabs > li > a {
        border-radius: 4px 4px 0 0;
        display: inline;
        float: left;
        color: #ccc;
        font-size: 22px;
        font-weight: 600;
        margin-right: 2px;
        line-height: 1.42857143;
        border: 1px solid transparent;
        padding: 10px;
        text-decoration: none;
    }

    .nav-tabs > li {
        float: left;
        margin-bottom: -1px;
    }

        .nav-tabs > li > a:hover {
            border-color: #eee #eee #ddd;
        }

        .nav-tabs > li.active > a, .nav-tabs > li.active > a:focus, .nav-tabs > li.active > a:hover {
            color: #8C1921;
            cursor: default;
            background-color: #fff;
            border: 1px solid #ddd;
            border-bottom-color: transparent;
            font-size: 22px;
            font-weight: 600;
        }

    .col-md-12 {
        width: 100%;
        position: relative;
        min-height: 1px;
        padding-right: 15px;
        padding-left: 15px;
        float: left;
    }

    .nav-tabs {
        border-bottom: 1px solid #ddd !important;
        float: left;
        width: 100%;
    }
</style>
<div class="col-md-12">
    <div style="float: right; margin-right: 20px;" runat="server" id="dvBackToCompTracking">
        <asp:LinkButton ID="lnkBacKToComplianceTracking" runat="server" OnClick="lnkBacKToComplianceTracking_Click">Back to Compliance Tracking</asp:LinkButton>
    </div>

    <ul class="nav nav-tabs">
        <li runat="server" id="liHome"><a runat="server" id="tabHome" onserverclick="tabHome_Click" onclick="setTab('1')">Manage Compliance Documents</a></li>
        <li runat="server" id="liRequirement"><a runat="server" id="tabRequirement" onserverclick="tabRequirement_ServerClick" onclick="setTab('2')">Manage Professional Documents</a></li>
    </ul>
</div>
<div class="tab-content">
    <div class="row">&nbsp;</div>
    <div id="home">
        <infsu:ManageUploadDocument runat="server" ID="ucManageUploadDocument" Visible="true" />
    </div>
    <div id="Requirement">
        <infsu:ManagePersonalDocument runat="server" ID="ucManagePersonalDocument" Visible="false" />
    </div>

 


</div>

<div style="display: none">
    <infs:WclButton runat="server" ID="btnDummy"></infs:WclButton>
</div>
<asp:HiddenField ID="hdnSelectedTab" runat="server" />
<script type="text/javascript">
    function pageLoad() {

        //var hdntimeout = $jQuery('[id$=hdntimeout]');  //, $jQuery(parent.theForm));
        //if (hdntimeout != null) {
        //    var timeout = hdntimeout.val();
        //    parent.StartCountDown(timeout);
        //} 
    }

    function setTab(tabNo) {
        $jQuery("[id$=hdnSelectedTab]").val(tabNo);
    }


</script>
<%--<script src="../Resources/Mod/Dashboard/Scripts/bootstrap.min.js" type="text/javascript"></script>--%>

