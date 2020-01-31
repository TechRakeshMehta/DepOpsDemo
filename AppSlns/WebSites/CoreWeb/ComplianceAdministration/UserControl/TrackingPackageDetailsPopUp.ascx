<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackingPackageDetailsPopUp.ascx.cs" Inherits="CoreWeb.ComplianceAdministration.Views.TrackingPackageDetailsPopUp" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %>


<style type="text/css">
    table {
      
        border-collapse: collapse;
        width:100%;
    }

    table, th, td {
        border: 1px solid black;
        /*width:50%;*/       
      
    }
    td{
        padding: 3px 0px 3px 3px;
    }
    th {
        font-weight: bold;
        padding: 3px 3px 3px 3px;
    }

    .divcls
    {
        margin:20px 20px 20px 20px;
    }
</style>

<div class="content" style="overflow: visible">
    <div>
        <asp:Panel ID="pnlNameOfPackages" runat="server" overflow-y="scroll">
            <div id="divNameOfPackages" class="bullet divcls" font-size="20px" runat="server"></div>
            <asp:Label ID="lblNameOfPackages" runat="server"></asp:Label>
        </asp:Panel>
    </div>
</div>
