<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgencyHierarchyNodeAvailabilitySetting.ascx.cs" Inherits="CoreWeb.AgencyHierarchy.Views.AgencyHierarchyNodeAvailabilitySetting" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rmpHierarchyControls">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~//Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Scripts/bootstrap.min.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="container-fluid" tabindex="-1" id="dvAgencyHierarchyNodeAvailabilitySetting" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Availability of Nodes</h2>
        </div>
    </div>
    <div class="clearfix"></div>    
    <div id="dvNodeAvailablitySetting" runat="server" class="row">
        <div class="col-md-12">
            <div class="form-inline">
                <span class="cptn">Is Node Available</span>
                <asp:RadioButton ID="rbtnNodeAvailablitySettingCheckYes" runat="server" GroupName="NodeAvailablitySetting" Text="Yes" />
                <asp:RadioButton ID="rbtnNodeAvailablitySettingCheckNo" runat="server" GroupName="NodeAvailablitySetting" Text="No" />                
                <asp:RadioButton ID="rbtnNodeAvailablitySettingDefault" runat="server" GroupName="NodeAvailablitySetting" Text="Default" />
            </div>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="form-group col-md-12">
        <div class=" pull-right">
            <infsu:CommandBar ID="fsucCmdBarNodeAvailablitySetting" runat="server" ButtonPosition="Right" DisplayButtons="Submit" UseAutoSkinMode="false" ButtonSkin="Silk"
                AutoPostbackButtons="Submit" OnSubmitClick="fsucCmdBarNodeAvailablitySetting_SubmitClick" SubmitButtonIconClass="rbSave" SubmitButtonText="Save" ValidationGroup="grpFormNodeAvailablitySettingSubmit">
            </infsu:CommandBar>
        </div>
    </div>

</div>
