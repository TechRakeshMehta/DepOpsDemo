<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminOrderReview.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.AdminOrderReview" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="PersonAlias" Src="~/Shared/Controls/PersonAliasInfo.ascx" %>
<script type="text/javascript">
    function RefeshPage() {
        //debugger;
        var hdnIsCustomDataSaved = $jQuery("[id$=hdnIsCustomDataSaved]", $jQuery(window.parent.document));
        var btnRefeshPage = $jQuery("[id$=btnRefeshPage]", $jQuery(window.parent.document));
        hdnIsCustomDataSaved.attr('value', "true");
        btnRefeshPage[0].click();
    }
</script>

<asp:Panel ID="pnlMain" runat="server" Width="100%" Height="100%">
    <div style="display:none">
            <infs:WclButton ID="dummyButton" runat="server"></infs:WclButton>
        </div>
    <div class="section">

        <h1 class="mhdr" id="headerText" runat="server">Custom Form Review: Please review your custom form details below before saving and transmitting the order.
        </h1>
        <div class="content">
            <div class="section" style="display:none;">
               <%-- <div id="divMhdr" class="mhdr" style="position: relative; bottom: 2px;">
                    <h1 style="font-size: 14px; padding-bottom: 2px;">Personal Detail</h1>
                    <div style="right: 20px; position: absolute; z-index: 99999999999; bottom: 20px;">
                        <infsu:CommandBar ButtonPosition="Center" ID="cmdbarEditProfile" runat="server" DisplayButtons="Extra" ClientIDMode="Static" OnExtraClientClick="stopColapse"
                            ExtraButtonText="Edit Profile" AutoPostbackButtons="Extra" OnExtraClick="btnEditProfile_Click">
                        </infsu:CommandBar>
                    </div>
                </div>--%>
                <%--<h1 class="mhdr">Personal Detail
            </h1>--%>
                <div class="content">
                    <div class="sxform auto">
                        <asp:Panel runat="server" CssClass="sxpnl" ID="Panel1">
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span class='cptn'>First Name</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblFirstName" CssClass="ronly" runat="server"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>Middle Name</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblMiddleName" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>Last Name</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblLastName" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co' id="dvpersonalAlias" runat="server" visible="false">
                                <uc:PersonAlias ID="ucPersonAlias" runat="server" Visible="true" IsReadOnly="true" IsLabelMode="true"></uc:PersonAlias>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span class='cptn'>Gender</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblGender" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>Date of Birth</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblDateOfBirth" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div runat="server" id="divSSN">
                                    <div class='sxlb'>
                                        <span class='cptn'>Social Security Number</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblSSN" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span class='cptn'>Email Address</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblEmail" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>Secondary Email Address</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblSecondaryEmail" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span class='cptn'>Phone Number</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblPhone" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>Secondary Phone Number</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblSecondaryPhone" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span class='cptn'>Address 1</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblAddress1" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>Address 2</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblAddress2" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>Country</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblCountry" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span class='cptn'>City</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblCity" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>State</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblState" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>Zip</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblZip" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co' id="divInternationalCriminalSearchAttributes" runat="server" visible="false">
                                <div runat="server" id="divMothersName" visible="false">
                                    <div class='sxlb'>
                                        <span class='cptn'>Mother's Maiden Name</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblMotherName" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                </div>
                                <div id="divIdentificationNumber" runat="server" visible="false">
                                    <div class='sxlb'>
                                        <span class='cptn'>Identification Number</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblIdentificationNumber" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                </div>
                                <div id="divCriminalLicenseNumber" runat="server" visible="false">
                                    <div class='sxlb'>
                                        <span class='cptn'>Driver License Number</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblCriminalLicenseNumber" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co'>
                                <div id="dvMoveinDate" runat="server" visible="false">
                                    <div class='sxlb'>
                                        <span class='cptn'>Move in Date</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblResidingFrom" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                    <div class='sxlb'>
                                        <span class='cptn'>Residing To</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblResidingTo" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                </div>
                                <div id="dvDriverLicenseNo" runat="server" visible="false">
                                    <div class='sxlb'>
                                        <span class='cptn'>Driver License Number</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblDriverLiscence" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div id="dvDriverLicenseState" runat="server" visible="false">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class='cptn'>Driver License State</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:Label ID="lblDriverLicenceState" runat="server" CssClass="ronly"></asp:Label>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <asp:Panel ID="pnlReviewLoader" runat="server">
            </asp:Panel>
        </div>

        <div style="margin-top: 5px;">
            <infsu:CommandBar ButtonPosition="Center" ID="cmdbarSubmit" runat="server" DisplayButtons="Save"
                AutoPostbackButtons="Save" SaveButtonIconClass="rbPrevious" SaveButtonText="Previous" OnSaveClick="cmdbarSubmit_Back"
                DefaultPanel="pnlMain" DefaultPanelButton="Clear">
            </infsu:CommandBar>
        </div>
    </div>
</asp:Panel>
