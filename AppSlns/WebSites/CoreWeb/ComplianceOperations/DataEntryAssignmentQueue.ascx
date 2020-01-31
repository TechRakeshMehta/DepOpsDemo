<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEntryAssignmentQueue.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.DataEntryAssignmentQueue" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagName="DataEntryQueue" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/DataEntryQueue.ascx" %>

<div id="dvDataEntryQueue" runat="server">
    <infsu:DataEntryQueue ID="ucDataEntryQueue" runat="server" />
</div>
