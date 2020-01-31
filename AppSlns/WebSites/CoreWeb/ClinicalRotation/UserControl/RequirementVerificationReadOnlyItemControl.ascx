<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequirementVerificationReadOnlyItemControl.ascx.cs"
    Inherits="CoreWeb.ClinicalRotation.Views.RequirementVerificationReadOnlyItemControl" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register Src="~/ClinicalRotation/UserControl/RequirementVerificationFieldControl.ascx"
    TagName="FieldControl" TagPrefix="uc" %>

<style type="text/css">
    .statusImg {
        padding-right: 7px;
        vertical-align: sub;
    }

    .rdonly {
        background: #fff none repeat scroll 0 0 !important;
    }
</style>

<div class="row bgLightGreen">
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

        <!--UAT-3077 style="display: none;"-->
        <div id="divItemPaymentPanel" runat="server">
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

        <%-- <div class='sxro sx3co'>--%>
        <div id="divFieldContainer" runat="server">
        </div>
        <%--<div class='sxroend'>
                    </div>
                </div>--%>
        <div class="col-md-12">
            <div class="row">
                <div class='form-group col-md-6'>
                    <span class='cptn'>Status</span>
                    <asp:RadioButtonList ID="rbtnListStatus" runat="server" RepeatDirection="Horizontal"
                        CssClass="radio_list">
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <div class="row">
                <div class='form-group col-md-6'>
                    <span class='cptn'>Rejection Reason</span>
                    <infs:WclTextBox ID="txtRejectionreason" runat="server" Enabled="false" TextMode="MultiLine"
                        Width="100%" Height="80px" CssClass="borderTextArea">
                    </infs:WclTextBox>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>

