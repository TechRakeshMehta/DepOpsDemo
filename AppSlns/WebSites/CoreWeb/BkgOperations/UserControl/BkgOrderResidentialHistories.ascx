<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BkgOrderResidentialHistories.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.BkgOrderResidentialHistories" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Repeater runat="server" ID="rptrResidentialHistory" OnItemDataBound="rptrResidentialHistory_ItemDataBound">
    <ItemTemplate>
        <div class="section">
            <h1 class="mhdr">
                <asp:Label runat="server" ID="lblResHistoryIndex"></asp:Label></h1>
            <div class="content">
                <div class="sxform auto">
                    <div class="sxpnl">
                        <div class='sxro sx3co' id="dvAddressLine" runat="server">
                            <div class='sxlb'>
                                <span class='cptn'>Address 1</span>
                            </div>
                            <div class='sxlm'>
                                <asp:Label runat="server" ID="lblAddress1"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Address1"))) %></asp:Label>
                            </div>
                            <div class='sxlb'>
                                <span class='cptn'>Address 2</span>
                            </div>
                            <div class='sxlm'>
                                <asp:Label runat="server" ID="lblAddress2"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Address2"))) %></asp:Label>
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
                                    <asp:Label runat="server" ID="lblResidentFrom" />
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>Resident Until</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label runat="server" ID="lblResidentTill" />
                                </div>
                            </div>
                            <div class='sxlb'>
                                <span class='cptn'>Is Current Address</span>
                            </div>
                            <div class='sxlm'>
                                <asp:Label runat="server" ID="lblIsCurrentResident"><%# Convert.ToBoolean(Eval("isCurrent"))== true?"Yes" : "No" %></asp:Label>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class='cptn'>Country</span>
                            </div>
                            <div class='sxlm'>
                                <asp:Label runat="server" ID="lblCountry"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Country"))) %></asp:Label>
                            </div>
                            <div class='sxlb'>
                                <asp:Label class='cptn' runat="server" ID="lblStateText" Text="State" />
                            </div>
                            <div class='sxlm'>
                                <asp:Label runat="server" ID="Label2"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("StateName"))) %></asp:Label>
                            </div>
                            <div class='sxlb'>
                                <span class='cptn'>City</span>
                            </div>
                            <div class='sxlm'>
                                <asp:Label runat="server" ID="lblCity"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CityName"))) %></asp:Label>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <asp:Label class='cptn' runat="server" ID="lblZipText" Text="Zip Code"></asp:Label>
                            </div>
                            <div class='sxlm'>
                                <asp:Label runat="server" ID="lblZipCode"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Zipcode"))) %></asp:Label>
                            </div>
                            <div id="dvCounty" runat="server">
                                <div class='sxlb'>
                                    <span class='cptn'>County</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label runat="server" ID="lblCounty"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CountyName"))) %></asp:Label>
                                </div>
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
                    </div>
                </div>
            </div>
        </div>                              
        <hr />
    </ItemTemplate>
</asp:Repeater>
<asp:Repeater runat="server" ID="rptrOrderConfirmation" OnItemDataBound="rptrResidentialHistory_ItemDataBound">
    <ItemTemplate>

        <h6 style="color: #8C1921; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">
            <asp:Label runat="server" ID="lblResHistoryIndex"></asp:Label></h6>
        <div id="dvAddressLine" runat="server">
            <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                <span>Address 1</span>:&nbsp;<span style="font-weight: bold">
                    <asp:Label ID="lblAddress1" runat="server"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Address1"))) %></asp:Label></span>
            </div>
            <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                <span>Address 2</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblAddress2"
                    runat="server"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Address2"))) %></asp:Label></span>
            </div>
            <div style="clear: both">
            </div>
        </div>
        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
            <span>Move in Date</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblResidentFrom"
                runat="server"></asp:Label></span>
        </div>
        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
            <span>Resident Until</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblResidentTill"
                runat="server"></asp:Label></span>
        </div>
        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
            <span>Is Current Address</span>:&nbsp;<span style="font-weight: bold"><asp:Label runat="server" ID="lblIsCurrentResident"><%# Convert.ToBoolean(Eval("isCurrent"))== true?"Yes" : "No" %></asp:Label>
        </div>
        <div style="clear: both">
        </div>
        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
            <span>Country</span>:&nbsp;<span style="font-weight: bold">
                <asp:Label runat="server" ID="lblCountry"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Country"))) %></asp:Label></span>
        </div>
        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
            <span>
                <asp:Label runat="server" ID="lblStateText" Text="State" /></span>:&nbsp;<span style="font-weight: bold">
                    <asp:Label runat="server" ID="Label2"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("StateName"))) %></asp:Label></span>
        </div>
        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
            <span>City</span>:&nbsp;<span style="font-weight: bold">
                <asp:Label runat="server" ID="lblCity"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CityName"))) %></asp:Label></span>
        </div>
        <div style="clear: both">
        </div>
        <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
            <span>
                <asp:Label runat="server" ID="lblZipText" Text="Zip Code"></asp:Label></span>:&nbsp;<span style="font-weight: bold">
                    <asp:Label runat="server" ID="lblZipCode"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Zipcode"))) %></asp:Label></span>
        </div>

        <div id="dvCounty" runat="server">
            <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                <span>County</span>:&nbsp;<span style="font-weight: bold">
                    <asp:Label runat="server" ID="lblCounty"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CountyName"))) %></asp:Label></span>
            </div>
        </div>
        <div style="clear: both">
        </div>
        <div class='sxro sx3co' id="divInternationalCriminalSearchAttributes" runat="server" visible="false">
            <div runat="server" id="divMothersName" visible="false">
                <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                    <span>Mother's Maiden Name</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblMotherName" runat="server"></asp:Label>
                    </span>
                </div>
            </div>
            <div id="divIdentificationNumber" runat="server" visible="false">
                <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                    <span>Identification Number</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblIdentificationNumber" runat="server"></asp:Label>
                    </span>
                </div>
            </div>
            <div id="divCriminalLicenseNumber" runat="server" visible="false">
                <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                    <span>Driver License Number</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblCriminalLicenseNumber" runat="server"></asp:Label>
                    </span>
                </div>
            </div>
        </div>
        <div style="clear: both">
        </div>
        <hr style="border-bottom: solid 1px #c0c0c0;" />

    </ItemTemplate>
</asp:Repeater>

<infs:WclButton ID="WclButton1" runat="server" Style="display: none"></infs:WclButton>
