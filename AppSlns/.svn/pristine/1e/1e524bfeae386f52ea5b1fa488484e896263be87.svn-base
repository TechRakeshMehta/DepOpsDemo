<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequirementVerificationItemControl.ascx.cs"
    Inherits="CoreWeb.ClinicalRotation.Views.RequirementVerificationItemControl" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/ClinicalRotation/UserControl/RequirementVerificationFieldControl.ascx"
    TagName="FieldControl" TagPrefix="uc" %>
<%@ Register Src="RequirementVerificationDetailsDocumentConrol.ascx" TagName="RequirementVerificationDetailsDocumentConrol"
    TagPrefix="uc3" %>


<style type="text/css">
    .statusImg {
        padding-right: 7px;
        vertical-align: sub;
    }

    .marginRadioButtons {
        float: left;
        width: 100%;
    }

        .marginRadioButtons td {
            padding-right: 7px;
        }

    .resizetxtbox {
        resize: vertical !important;
        width: 100% !important;
        height: 100%;
        border-width: 1px !important;
        border-color: #6788be !important;
    }
     .highlightDetailBorder {
        border: 3px solid Gray;
    }
      .highlightAssignedItem {
        background: #adadad;
    }
</style>


<%--<div class="row bgLightGreen">
    <asp:Panel ID="pnl" runat="server">
        <div class="col-md-12">
            <div class="row h2text">
                <div class="col-md-12">
                    <div class="ico">
                        <asp:Image ID="imgItemStatus" runat="server" CssClass="statusImg" />
                        <asp:Literal ID="litItemName" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
   
        <div id="divFieldContainer" runat="server">
        </div>
   
        <div class="col-md-12">
            <div class="row">
                <div class='form-group col-md-6'>
                    <span class='cptn'>Status</span>
                    <asp:RadioButtonList ID="rbtnListStatus" runat="server" RepeatDirection="Horizontal"
                        CssClass="form-control">
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <div class="row">
                <div class='form-group col-md-6'>
                    <span class='cptn'>Rejection Reason</span>
                    <infs:WclTextBox ID="txtRejectionreason" runat="server" MaxLength="2048" TextMode="MultiLine"
                        Height="80px" Width="100%" CssClass="borderTextArea">
                    </infs:WclTextBox>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>

--%>

<div class="msgbox">
    <asp:Label ID="lblMessage" runat="server">
    </asp:Label>
</div>
<div class="section divPendingReview itemInfoContainer">
    <h1 class="mhdr nocolps">
        <div id="dvlnkItemName" runat="server">
            <a runat="server" style="text-decoration: none;" href="javascript:void(0);return false;" id="lnkItemName">
                <asp:Literal ID="litItemName" runat="server"></asp:Literal>
            </a> 
             <asp:Image ID="imageADEdisabled" ImageUrl="~/Resources/Mod/RotationPackages/icon/ExceptionsOffIcon-DataMovement.png" Visible="false" Style="vertical-align: text-bottom;"
                runat="server" ToolTip="Turned off Data Movement on this Item" />
            <div class="sec_cmds" onclick="ShowHideItemNotes(this);">
                <span class="bar_icon ihelp" title="View explanatory notes"></span>
            </div>
        </div>
    </h1>
    <div class="content">
        <div class="" >
            <div class="tab-block">
                <div class="tabs">
                    <%--<span class="tab1 focused">Admin's Explanation</span>
                    <span class="tab2">Applicant's ROT1 Explanation</span>--%>
                    
                    <span class="tab2">Requirement's Explanation</span>
                    <span class="tab1 focused">Review Standard Explanation</span>
                </div>
                <div class="tab-content tab1 focused">
                    <div class="bullet">
                        <asp:Panel runat="server" ID="AdminExplanation">
                      <%--  <asp:Literal ID="litItemDesc" runat="server"></asp:Literal>--%>
                            </asp:Panel>
                    </div>
                </div>
                <div class="tab-content tab2">
                    <div class="bullet">
                        <asp:Literal ID="litExplanatoryNotes" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
        <div id="itemSubmissionDate">
            <span style="color:#8C1921; font-weight: 700; word-spacing: 2px;" class="cptn">Submission Date</span>
            <asp:Label ID="lblSubmissionDate" runat="server"></asp:Label>
        </div>
        <div id="dvDetailPanel" class="sxform auto" runat="server">
            <div class="sxpnl">
                <asp:CheckBox ID="chkDeleteItem" runat="server" Text="Delete Item"></asp:CheckBox>
                <div class='sxro sx1co'>
                    <div class='sxlb' title="The current verification status for this Item">
                        <span class="cptn">Current Status</span>
                    </div>
                    <div class='sxlm'>
                        <span class="ronly">
                            <asp:Literal ID="litStatus" runat="server"></asp:Literal></span>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <!--UAT-3077 style="display: none;"-->
                 <div id="divItemPaymentPanel" runat="server" >
                    <div class='sxro sx1co'>
                        <div class='sxlb' title="The amount of item to be paid.">
                            <span class="cptn">Amount</span>
                        </div>
                        <div class='sxlm'>
                            <span class="ronly">
                                <asp:Literal ID="litItemAmount" runat="server"></asp:Literal></span>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx1co'>
                        <div class='sxlb' title="Item payment current status">
                            <span class="cptn">Payment Status</span>
                        </div>
                        <div class='sxlm'>
                            <span class="ronly">
                                <asp:Literal ID="litItemPaymentStatus" runat="server"></asp:Literal></span>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                </div>
                <%--  <div class='sxro sx1co'>--%>
                <div id="divFieldContainer" runat="server">
                </div>
                <%-- </div>--%>

                <!-- Document repater-->
                <uc3:RequirementVerificationDetailsDocumentConrol ID="ucRequirementVerificationDetailsDocumentConrol"
                    runat="server"></uc3:RequirementVerificationDetailsDocumentConrol>

                <div class='sxro monly'>
                    <div class='sxlb'>
                        <span class="cptn">Change Status</span>
                    </div>
                    <div class="marginRadioButtons">
                        <asp:RadioButtonList ID="rbtnListStatus" runat="server" RepeatDirection="Horizontal"
                            CssClass="form-control">
                        </asp:RadioButtonList>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro monly'>
                    <div class='sxlb'>
                        <span class="cptn">Rejection Reason</span>
                    </div>
                    <div>
                        <%--<infs:WclTextBox ID="txtRejectionreason" runat="server" MaxLength="2048" TextMode="MultiLine"
                            Height="150px" Width="100%" CssClass="borderTextArea">
                        </infs:WclTextBox>--%>
                        <asp:TextBox runat="server" ID="txtRejectionreason" TextMode="MultiLine" CssClass="borderTextArea resizetxtbox" MaxLength="2048">
                        </asp:TextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdfNotApproved" runat="server" />
    <asp:HiddenField ID="hdfIncomplete" runat="server" />
    <asp:HiddenField ID="hdfFileUploadExists" runat="server" />
    <asp:HiddenField ID="hdfFileUploadFieldId" runat="server" />
</div>
<script type="text/javascript">
    function ShowHideItemNotes(sender) {
        $jQuery(sender).parents('.itemInfoContainer').find('span').toggleClass('help_on');
        $jQuery(sender).parents('.itemInfoContainer').find('.showHideContent').toggle('slow');
    } 
</script>
