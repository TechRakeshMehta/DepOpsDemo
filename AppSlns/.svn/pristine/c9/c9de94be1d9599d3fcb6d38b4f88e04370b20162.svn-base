<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SeriesDose.ascx.cs" Inherits="CoreWeb.ComplianceAdministration.Views.SeriesDose" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<style type="text/css">
    #tblForm
    {
        border: solid 1px Black;
    }

        #tblForm tr
        {
            line-height: 20px;
        }
</style>
<div class="section">
    <h1 class="mhdr">Series Item</h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel ID="pnlRows" runat="server">
            </asp:Panel>
        </div>
        <div style="float: right; padding-top: 10px">
            <%--<infsu:CommandBar ID="fsucCmdBarMapping" runat="server" DefaultPanel="pnlRows" AutoPostbackButtons="Save" SaveButtonText="Save Mapping"
                DisplayButtons="Save" OnSaveClick="fsucCmdBarMapping_SaveClick">
            </infsu:CommandBar>--%>
            <infs:WclButton ID="btnSave" runat="server" Text="Save Mapping" OnClick="btnSave_Click"
                Icon-PrimaryIconCssClass="rbSave" AutoPostBack="true">
            </infs:WclButton>
        </div>
    </div>
</div>


<asp:Panel ID="pnlSeriesUnMappedAttributes" runat="server">
</asp:Panel>

