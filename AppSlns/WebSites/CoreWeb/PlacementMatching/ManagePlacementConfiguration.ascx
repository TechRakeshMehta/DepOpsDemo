<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManagePlacementConfiguration.ascx.cs" Inherits="CoreWeb.PlacementMatching.Views.ManagePlacementConfiguration" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/PlacementMatching/UserControl/PlacementDepartment.ascx" TagPrefix="infsu" TagName="DepartmentControl" %>
<%@ Register Src="~/PlacementMatching/UserControl/PlacementStudentType.ascx" TagPrefix="infsu" TagName="StudentTypeControl" %>
<%@ Register Src="~/PlacementMatching/UserControl/PlacementSpecialty.ascx" TagPrefix="infsu" TagName="SpecialtyControl" %>

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
            <ul class="nav nav-tabs">
                <li runat="server" id="liDepartment"><a runat="server" id="tabDepartment" style="cursor:pointer"  onclick="SetupClick('Department')" >Department</a></li>
                <li runat="server" id="liStudentType"><a runat="server" id="tabStudentType" style="cursor:pointer" onclick="SetupClick('StudentType')" >Student Type</a></li>
                <li runat="server" id="liSpecialty"><a runat="server" id="tabSpecialty" style="cursor:pointer" onclick="SetupClick('Specialty')" >Specialty</a></li>

            </ul>
           <%-- <div id="Department"  class="dvcss" onclick="SetupClick('Department')" style="display: inline-block; border-style: solid; border-width: 1px; border-spacing: 0px; background-color: #F9E79F;"  runat="server" >
                <span class="toptext">DEPARTMENT </span>
              
            </div>
            <div id="StudentType" class="dvcss"  onclick="SetupClick('StudentType')" style="display: inline-block; border-style: solid; border-width: 1px; border-spacing: 0px; background-color: #b1e1f9" runat="server" >
                <span class="toptext">STUDENTTYPE </span>
              
            </div>
            <div id="Specialty" runat="server" class="dvcss" onclick="SetupClick('Specialty')" style="display: inline-block; border-style: solid; border-width: 1px; border-spacing: 0px; background-color: #ceffba">
                <span class="toptext">SPECIALTY </span>
          
            </div>--%>
        </div>
    </div>
    <div>
        <%--     <asp:PlaceHolder ID="phDynamic" runat="server"></asp:PlaceHolder>--%>
        <infsu:DepartmentControl runat="server" ID="cnPlacementDepartment" />
        <infsu:SpecialtyControl runat="server" ID="cnPlacementSpecialty" />
        <infsu:StudentTypeControl runat="server" ID="cnPlacementStudentType" />
    </div>
    <asp:HiddenField ID="hdnSelected" runat="server" />
    <asp:Button CssClass="buttonHidden" ID="btnDoPostBack" runat="server" />
</div>
<script type="text/javascript">
    function SetupClick(data) {
        $jQuery("[id$=hdnSelected]").val(data);
        __doPostBack("<%= btnDoPostBack.ClientID %>", "");
    }


</script>
<script src="../Resources/Mod/Dashboard/Scripts/bootstrap.min.js" type="text/javascript"></script>