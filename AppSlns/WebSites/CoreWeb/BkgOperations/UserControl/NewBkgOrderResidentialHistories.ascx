<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BkgOrderResidentialHistories.ascx.cs"
    Inherits="CoreWeb.BkgOperations.Views.BkgOrderResidentialHistories" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxManageInvitationExpiration">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<style>
    .container-fluid h1 span {
        color: #eb986a;
        font-size: 18px !important;
    }

    .sr-only {
        border: 0 none;
        clip: rect(0px, 0px, 0px, 0px);
        height: 1px;
        margin: -1px;
        overflow: hidden;
        padding: 0;
        position: absolute;
        width: 1px;
    }
</style>
<div class="container-fluid">
    <asp:Repeater runat="server" ID="rptrResidentialHistory" OnItemDataBound="rptrResidentialHistory_ItemDataBound">
        <ItemTemplate>
            <div class="row">
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-12">
                            <h1 class="header-color" tabindex="0">
                                <asp:Label runat="server" ID="lblResHistoryIndex"></asp:Label></h1>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row bgLightGreen">&nbsp;</div>      
            <div class="row bgLightGreen">                                      
                <div id="dvAddressLine" runat="server">
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Address 1</span>
                        <asp:Label runat="server" CssClass="form-control" ID="lblAddress1"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Address1"))) %></asp:Label>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Address 2</span>
                        <asp:Label runat="server" CssClass="form-control" ID="lblAddress2"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Address2"))) %></asp:Label>
                    </div>
                </div>
                <div id="dvMoveinDate" runat="server" visible="false">
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Move in Date</span>
                        <asp:Label runat="server" CssClass="form-control" ID="lblResidentFrom" />
                    </div>
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Resident Until</span>

                        <asp:Label runat="server" CssClass="form-control" ID="lblResidentTill" />
                    </div>
                </div>
            </div>                                                                  
            <div class="row bgLightGreen">
                <div class='form-group col-md-3'>
                    <span class='cptn'>Is Current Address</span>
                    <asp:Label runat="server" CssClass="form-control" ID="lblIsCurrentResident"><%# Convert.ToBoolean(Eval("isCurrent"))== true?"Yes" : "No" %></asp:Label>
                </div>
                <div class='form-group col-md-3'>
                    <span class='cptn'>Country</span>
                    <asp:Label runat="server" CssClass="form-control" ID="lblCountry"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Country"))) %></asp:Label>
                </div>
                <div class='form-group col-md-3'>
                    <asp:Label class='cptn' runat="server" ID="lblStateText" Text="State" />
                    <asp:Label runat="server" CssClass="form-control" ID="Label2"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("StateName"))) %></asp:Label>
                </div>
                <div class='form-group col-md-3'>
                    <span class='cptn'>City</span>
                    <asp:Label runat="server" CssClass="form-control" ID="lblCity"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CityName"))) %></asp:Label>
                </div>
            </div>
            <div class="row bgLightGreen">
                <div class='form-group col-md-3'>
                    <asp:Label class='cptn' runat="server" ID="lblZipText" Text="Zip Code"></asp:Label>
                    <asp:Label runat="server" CssClass="form-control" ID="lblZipCode"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Zipcode"))) %></asp:Label>
                </div>
                <div id="dvCounty" runat="server">
                    <div class='form-group col-md-3'>
                        <span class='cptn'>County</span>
                        <asp:Label runat="server" CssClass="form-control" ID="lblCounty"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CountyName"))) %></asp:Label>
                    </div>
                </div>
            </div>

            <div class="row bgLightGreen" id="divInternationalCriminalSearchAttributes" runat="server" visible="false">
                <div class='form-group col-md-3' runat="server" id="divMothersName" visible="false">
                    <span class='cptn'>Mother's Maiden Name</span>
                    <asp:Label runat="server" CssClass="form-control" ID="lblMotherName"></asp:Label>
                </div>
                <div id="divIdentificationNumber" runat="server" visible="false">
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Identification Number</span>
                        <asp:Label runat="server" CssClass="form-control" ID="lblIdentificationNumber"></asp:Label>
                    </div>
                </div>
                <div id="divCriminalLicenseNumber" runat="server" visible="false">
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Driver License Number</span>
                        <asp:Label runat="server" CssClass="form-control" ID="lblCriminalLicenseNumber"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="row bgLightGreen">&nbsp;</div>
        </ItemTemplate>
    </asp:Repeater>

    <asp:Repeater runat="server" ID="rptrOrderConfirmation" OnItemDataBound="rptrResidentialHistory_ItemDataBound">
        <ItemTemplate>
            <h6 style="color: #8C1921; font-family: Helvetia, Arial, sans-serif; font-size: 100%; font-weight: 700; line-height: 150%; word-spacing: 2px">
                <asp:Label runat="server" ID="lblResHistoryIndex"></asp:Label></h6>
            <div id="dvAddressLine" runat="server">
                <div style="display: inline-block; width: 30%; vertical-align: top; float: left">
                    <span>Address 1</span>:&nbsp;<span style="font-weight: bold"><asp:Label ID="lblAddress1"
                        runat="server"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Address1"))) %></asp:Label></span>
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
                <span>Is Current Address</span>:&nbsp;<span style="font-weight: bold"><asp:Label
                    runat="server" ID="lblIsCurrentResident"><%# Convert.ToBoolean(Eval("isCurrent"))== true?"Yes" : "No" %></asp:Label>
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
                    <asp:Label runat="server" ID="lblZipText" Text="Zip Code"></asp:Label></span>:&nbsp;<span
                        style="font-weight: bold">
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
</div>
<script type="text/javascript">
    function pageLoad() {
        $jQuery('div span:nth-child(2)').each(function (i, e) {
            $jQuery(e).attr("tabindex", "0");
            $jQuery(e).prepend("<span class='sr-only'>" + $jQuery($jQuery(e).siblings()[0]).text() + "&nbsp;</span>");
        });
    }
</script>
