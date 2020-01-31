<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceOperations.Views.DocumentViewer"
    Title="DocumentViewer" CodeBehind="DocumentViewer.aspx.cs" %>

<%@ Register Assembly="RadPdf" Namespace="RadPdf.Web.UI" TagPrefix="radPdf" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RAD PDF Sample</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="dvRadPdf">
            <div hidden="hidden">
                <asp:Button ID="Button1" runat="server" Text="Save" OnClientClick="return PdfSave();" />
            </div>


            <radPdf:PdfWebControl ID="PdfWebControl1" runat="server" Height="500px"
                Width="100%"
                OnSaved="PdfWebControl1_Saved"
                HideBottomBar="false"
                HideThumbnails="true"
                HideBookmarks="true"
                HideSaveButton="true"
                HideToolsTabs="true"
                HideFileMenu="true"
                CollapseTools="true"
                HideSearchText="True"
                HideEditMenu="true"
                HideObjectPropertiesBar="true"
                HideToolsPageTab="true"
                HideToolsAnnotateTab="true"
                HideToolsInsertTab="true"
                HideSideBar="true"
                HideToolsMenu="true"
                HideRightClickMenu="true"
                ViewerPageLayoutDefault="SinglePageContinuous" />
            <script type="text/javascript">
                function PdfSave() {
                    //debugger;
                    (new PdfWebControlApi("<%=this.PdfWebControl1.ClientID%>")).saveAndWait();
                }
            </script>
        </div>
    </form>
</body>
</html>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadContent" runat="server">
    <style type="text/css">
        #pnlModHeader {
            display: none;
        }
    </style>

</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <h1>Document Viewer</h1>
     <div>
        <radPdf:PdfWebControl ID="PdfWebControl1" runat="server" Height="650px"
                                    Width="100%"
                                    OnSaved="PdfWebControl1_Saved"          
                                    HideBottomBar="false"
                                    HideThumbnails="true"
                                    HideBookmarks="true"
                                    CollapseTools="true"
                                    HideSearchText="True"
                                    HideEditMenu="true"
                                    HideObjectPropertiesBar="true"
                                    HideToolsPageTab="true"
                                    HideToolsAnnotateTab="true"
                                    HideToolsInsertTab="true"
                                    HideSideBar="true"
                                    HideToolsMenu="true"
                                    ViewerPageLayoutDefault="SinglePageContinuous" />
    </div>
</asp:Content>--%>
