<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ItemDescriptionExplanation" CodeBehind="ItemDescriptionExplanation.ascx.cs" %>
<script>
    var winopen = false;

    function OpenSampleDocWindow(itemName) {
        //debugger;
        if (!winopen) {
            var sampleDocUrl = document.getElementById('hdnUrl' + itemName).value;
            var composeScreenWindowName = "SampleDocumentViewScreen";
            //var url = $page.url.create(sampleDocUrl);
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var win = $window.createPopup(sampleDocUrl, { size: "800,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
            winopen = true;
        }
        return false;
    }

    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        winopen = false;

    }

</script>
<style>
    .bullet ul {
        margin-left: 10px;
        padding-left: 10px !important;
    }

    .bullet li {
        list-style-position: inside;
        list-style: disc;
    }

    .bullet ol {
        list-style-type: decimal;
        margin-left: 10px;
        padding-left: 10px;
    }

        .bullet ol li {
            list-style: decimal;
        }
</style>
<div id="divTabBlockUC" class="tab-block">
    <div class="tabs">
        <%--<span id="spnAdminExplanation" class="tab1 focused" onclick="SaveUpdateExplanationState(this);">Admin's Explanation</span>
        <span id="spnApplicantExplanation" class="tab2">Applicant's IDEP  Explanation</span>       --%>
        <span id="spnApplicantExplanation" class="tab2 focused">Requirement's Explanation</span>
         <span id="spnAdminExplanation" class="tab1" onclick="SaveUpdateExplanationState(this);">Review Standard Explanation</span>
    </div>
    <div class="tab-content tab1">
        <div class="bullet">
            <asp:Literal ID="litAdminExplanation" runat="server"></asp:Literal>            
        </div>

    </div>
    <div class="tab-content tab2 focused">
        <div class="bullet">
            <asp:Literal ID="litApplicantExplanation" runat="server"></asp:Literal>                
        </div>
    </div>
</div>

