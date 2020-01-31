<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceOperations.Views.PdfDocumentViewer" Codebehind="PdfDocumentViewer.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Assembly="RadPdf" Namespace="RadPdf.Web.UI" TagPrefix="radPdf" %>

<infs:WclResourceManagerProxy runat="server" ID="prxMP">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/PdfDocumentViewer.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<style type="text/css">
    .cat_highlight
    {
        color: red !important;
    }
</style>
<div class="section">    
    <div class="content">
        <div class="msgbox" id="dvMsgBox">
            <asp:Label ID="lblPdfMessage" runat="server">                             
            </asp:Label>
        </div>
        <div class="swrap">
            <asp:Panel runat="server" ID="pnlUnifiedPdfDoc">
                <asp:HiddenField ID="hdnCurrentLoggedInUserId" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnIsPdfDocLoaded" runat="server" Value="1" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnDockLeftDoc" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnDockTopDoc" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnIsFloatingModeDoc" runat="server" ClientIDMode="Static" />
                <infs:WclDockZone ID="dockZone" runat="server" Orientation="Vertical" Width="100%"
                    BackColor="ActiveBorder" MinHeight="380px">
                    <infs:WclDock ID="Dock" runat="server" Collapsed="false" DockMode="Default" EnableDrag="true" DockHandle="Grip" OnClientDockPositionChanged="OnDragResetUtilityFeature"
                        Title="Dock/UnDock" DefaultCommands="All" OnClientInitialize="OnClientInitialized" EnableRoundedCorners="true" Resizable="true">
                        <ContentTemplate>
                            <div class="dvPdfDocView">
                                <infs:WclButton runat="server" ID="btnReloadPdfDoc" Text="Reload" OnClientClicked="SetDockPosition" OnClick="btnReloadPdfDoc_Click"></infs:WclButton>
                                <infs:WclButton AutoPostBack="false" runat="server" ID="btnRotatePdfDoc" Text="Rotate" OnClientClicked="RotatePageView"></infs:WclButton>
                            </div>
                            <div id="dvPdfDocuViewer" runat="server">
                                <radPdf:PdfWebControl ID="PdfWebControl1" runat="server" Height="450px"
                                    Width="100%" OnClientLoad="ClientLoadPdfDocViewer();"
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
                        </ContentTemplate>
                    </infs:WclDock>
                </infs:WclDockZone>

            </asp:Panel>
        </div>
        <div class="gclr">
        </div>
    </div>
</div>




