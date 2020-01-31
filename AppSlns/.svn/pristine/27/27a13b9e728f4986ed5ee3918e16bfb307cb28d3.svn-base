<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequiredDocumentationLoader.ascx.cs" 
    Inherits="CoreWeb.ComplianceOperations.Views.RequiredDocumentationLoader" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Repeater ID="rptAdditionalDocuments" runat="server" OnItemDataBound="rptAdditionalDocuments_OnItemDataBound">
    <ItemTemplate>
        <div id="Div1" runat="server" style="height: 500px;">
            <iframe id="iframePdfDocViewer" runat="server" width="100%" height="100%"></iframe>
        </div>
        <center>
            <div>
       </div>
                 </center>
        <br />
    </ItemTemplate>
</asp:Repeater>
<script>
    function callDocumentViewerSaveButton() {
        //debugger;
        $jQuery("[id$=DefaultContent_ucDynamicControl_ucRequiredDocumentationLoader_rptAdditionalDocuments_iframePdfDocViewer_0]").contents().find('#Button1').click();  
    }

</script>
