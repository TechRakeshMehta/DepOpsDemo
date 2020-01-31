<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ReconciliationDocumentPanel" CodeBehind="ReconciliationDocumentPanel.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/ComplianceOperations/UserControl/PdfDocumentViewer.ascx" TagName="PdfDocumentViewer"
    TagPrefix="uc2" %>
<style type="text/css">
    .cat_highlight {
        color: red !important;
    }

</style>
<infs:WclResourceManagerProxy runat="server" ID="prxMP">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/DocumentPanel.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<asp:UpdatePanel runat="server" ID="updpnlDocument">
    <ContentTemplate>
        <div>
            <asp:HiddenField ID="hdnTenantId" runat="server" />
            <asp:HiddenField ID="hdnViewAll" runat="server" />
            <asp:HiddenField ID="hdnRoratorWidth" runat="server" />
            <asp:HiddenField ID="hdnDockLeft" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnDockTop" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnIsFloatingMode" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnMergingCompletedDocStatusID" runat="server" />
            <infs:WclSplitter ID="sptrMain" runat="server" LiveResize="true" Orientation="Horizontal"
                OnClientResizing="resizing" Height="100%" Width="100%" BorderSize="0" BorderWidth="0">
                <infs:WclPane ID="WclPane1" runat="server" Height="55px">
                    <div class="framebar" title="Applicant's Documents">
                            <div class="title docs-icon" >
                                Documents 
                            </div>
                        <div  class="commands " style="float:left; padding-right:10%;">
                            <div style="float:left;font-weight:bold;margin-right:5px;">  <asp:Label id="lblLabelViewType" runat="server" Text="View Type:"></asp:Label></div>
                                 <div style="float:right;">
                                    <asp:RadioButtonList ID="rdbLstViewType"  runat="server" CellSpacing="2" CellPadding="3" RepeatDirection="Horizontal" onclick="UpdateSettingForDocumentViewType();">
                                    </asp:RadioButtonList>
                                     </div>
                                </div>
                    </div>
                </infs:WclPane>
                <infs:WclPane ID="pnLeft" runat="server" Height="110px" Scrolling="X" Visible="false">
                    <telerik:RadRotator ID="RadRotator1" runat="server" AutoPostBack="false" FrameDuration="100"
                        Width="540px" Height="100px" BorderStyle="None" BorderWidth="0px" ItemHeight="90"
                        ItemWidth="90" RotatorType="Buttons" DataMember="ApplicantDocuments" OnClientItemClicking="itemclicked">
                        <ItemTemplate>
                            <div style="width: 80px; height: 70px; margin-right: 5px;" class='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentId") %>'>
                                <div>
                                    <asp:HiddenField ID="hdnDoc" runat="server"
                                        Value='<%# DataBinder.Eval(Container.DataItem,"UnifiedDocumentStartPageID") %>' />
                                    <asp:Image ID="imgbtnDocuments" CommandName="cmd" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentId") %>'
                                        ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ImageUrl")  %>' runat="server"
                                        Height="40px" Width="50px" />
                                    <asp:HiddenField ID="hdnApplicantDocumentMergingStatusID" runat="server"
                                        Value='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentMergingStatusID") %>' />
                                </div>
                                <br />
                                <div id="testlable" style="word-wrap: break-word; font-family: Verdana; font-size: x-small">
                                    <asp:Label ID='lblDescription' runat="server" name='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentId") %>'> 
                            <%# DataBinder.Eval(Container.DataItem, "ImageText")  %></asp:Label>
                                </div>
                            </div>
                        </ItemTemplate>
                        <ControlButtons OnClientButtonClick="OnClientButtonClick" />
                    </telerik:RadRotator>
                </infs:WclPane>
                <infs:WclPane ID="pnRight" runat="server" Width="100%" Height="100%">
                    <%--<uc2:PdfDocumentViewer runat="server" ID="ucPdfDocumentViewer" />--%>
                    <asp:HiddenField runat="server" ID="hdnDocVwr" />
                    <%--<asp:HiddenField runat="server" ID="hdnLoadIn" />--%>
                    <asp:HiddenField runat="server" ID="hdnApplicantIdDocumentPanel" />
                    <asp:HiddenField runat="server" ID="hdnIsApplicantChanged" />
                    <%--<input type="button" id="btnUndockPdfVwr" onclick="btnUndockClick()" value="UnDock" />--%>
                    <asp:HiddenField runat="server" ID="hdnSelectedCatUnifiedStartPageID" />
                    <infs:WclButton runat="server" AutoPostBack="false" ID="btnUndockPdfVwr" OnClientClicked="btnUndockClick" Text="UnDock"></infs:WclButton>
                    <iframe id="iframePdfDocViewer" runat="server" width="100%" height="100%"></iframe>

                    <asp:HiddenField runat="server" ID="hdnUnifiedDocVwr" />
                    <asp:HiddenField runat="server" ID="hdnSingleDocVwr" />
                    <asp:HiddenField runat="server" ID="hdnCurrentDocID" ClientIDMode="Static" />
                </infs:WclPane>
            </infs:WclSplitter>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<iframe id="iframeDocViewer" runat="server" width="100%" height="100%"></iframe>
<script type="text/javascript">
    var iframeInstance = null;

    function ResetUtilityFeatForDockUnDock() {
        //var currentLoggedInUserId = $("[id$=hdnCurrentLoggedInUserId]")[0].value;
        var dataString = "organizationUserId : '" + '<%=CurrentLoggedInUserId%>' + "',ignoreAlert : '" + "true" + "'";
        var urltoPost = "/ComplianceOperations/Default.aspx/ResetUtilityFeatureForDockUnDock";
        HideDockUndock();
        $jQuery.ajax
         (
          {
              type: "POST",
              url: urltoPost,
              data: "{ " + dataString + " }",
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (data) {
                  var fileIdentifier = data.d;
              }
          });
    }

    function SaveUpdateDocumentViewSetting(viewType) {
        var dataString = "organizationUserId : '" + '<%=CurrentLoggedInUserId%>' + "',viewType : '" + viewType + "'";
        var urltoPost = "/ComplianceOperations/Default.aspx/SaveUpdateDocumentViewSetting";
        $jQuery.ajax
         (
          {
              type: "POST",
              url: urltoPost,
              data: "{ " + dataString + " }",
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (data) {
                  var fileIdentifier = data.d;
              }
          });
    }
</script>
