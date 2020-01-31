<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ManageDownloadPdf" Codebehind="ManageDownloadPdf.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%--<div style="direction: rtl; margin-left: 20px">
    <asp:LinkButton ID="lnkDownloadPdf" OnClick="lnkDownloadPdf_click" runat="server"
        Text="...Save as PDF"></asp:LinkButton>
    </div>--%>
<div id="printBox" style="z-index: 100000; text-align: right; -webkit-box-sizing: border-box;
    -moz-box-sizing: border-box; box-sizing: border-box; background-color: #D5E5FF;
    padding: 5px; position: fixed; width: 100%;">
    <infs:WclButton runat="server" ID="lnkDownloadPdf" Text="Print" OnClick="lnkDownloadPdf_click">
        <Icon PrimaryIconCssClass="rbPrint" />
    </infs:WclButton>
</div>
<asp:HiddenField ID="hdnFileIdentifier" runat="server" />
<iframe id="iframe" runat="server" width="100%" height="0px"></iframe>
<script type="text/javascript">
    function DownloadPdf() {
        var fileIdentifier = $jQuery("[id$=hdnFileIdentifier]").val();
        if (fileIdentifier != "") {
            $jQuery("[id$=iframe]").attr('src', "/ComplianceOperations/Pages/DownloadPdf.aspx?fileIdentifier=" + fileIdentifier);
            $jQuery("[id$=hdnFileIdentifier]").val("");
        }
    }
</script>
<script type="text/javascript">

    var resizeP = function () {
        $jQuery("#printBox").width($jQuery("#updpPopupContent").width() - 10);
    }
    $page.add_pageReady(function () {
        resizeP();
        $findByKey("pageSplitter", function () {
            this.add_resized(function () {
                resizeP()
            });
            top.abc = [document, window, this]
        });
    });
</script>

