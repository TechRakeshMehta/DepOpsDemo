<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageMasterStateSearch.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageMasterServiceItemRule" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<style type="text/css">
    th.thState {
        font-size: 13px;
        font-weight: bold;
        padding: 2px 41px 4px 52px;
    }

    th.thStateAbb {
        font-size: 13px;
        font-weight: bold;
        padding: 2px 41px 4px 52px;
    }

    td.tdState {
        border-color: #dedede;
        background-color: #eeeeee;
        color: black;
        border-right-width: 0px;
        padding: 3px 5px 4px;
        border-style: solid;
        width: 35%;
        line-height: 10px !important;
    }

    td.tdStateAbb {
        border-color: #dedede;
        background-color: #eeeeee;
        color: black;
        text-align: center;
        border-style: solid;
        line-height: 10px !important;
    }
</style>
<div class="section">
    <div id="dvStateSearchMain" class="mhdr">
        <h1 style="font-size: 14px;">Master State Search Criteria</h1>
    </div>
    <div class="content" >
        <div class="swrap">
            <div id="divStateCounty" class="sxform auto" style="background-color: #DEDEDE; padding-top: 7px;">
                <div class="page_cmd" style="float: right; padding-top: 3px !important;">
                    <infs:WclButton runat="server" ID="btnEditMasterStateSearchCriteria" Icon-PrimaryIconCssClass="rbEdit" Text="Edit Master State Search Criteria" OnClick="btnEditMasterStateSearchCriteria_Click"
                        ButtonType="LinkButton" Height="30px">
                    </infs:WclButton>
                </div>
                <asp:Repeater ID="rptStateCounty" runat="server" OnItemDataBound="rptStateCounty_ItemDataBound" OnItemCommand="rptStateCounty_ItemCommand">
                    <HeaderTemplate>
                        <table id="tblStateCounty" style="padding-top: 5px;">
                            <tr>
                                <th class="thState" style="padding-bottom: 10px !important; background-color: #808080">State Name</th>
                                <th class="thStateAbb" style="padding-bottom: 10px !important; background-color: #808080">State Abbreviation</th>
                                <th class="allStateCheckbox" style="padding-right: 40px; padding-bottom: 10px; font-weight: bold; font-size: 13px; background-color: #808080">
                                    <asp:CheckBox ID="chkAllState" Text="Select All" runat="server" Enabled="false" onclick="checkAll('stateSearch')" />
                                </th>
                                <th class="allCountyCheckbox" style="padding-bottom: 10px; font-weight: bold; font-size: 13px; background-color: #808080">
                                    <asp:CheckBox ID="chkAllCounty" Text="Select All" runat="server" Enabled="false" onclick="checkAll('countySearch')" />
                                </th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="tdState">            
                                <asp:Label ID="lblStateName" Text='<%# Eval("StateName") %>' runat="server"></asp:Label>
                                <asp:HiddenField ID="hdnStateID" Value='<%# Eval("StateID") %>' runat="server" />
                            </td>
                            <td class="tdStateAbb">     
                                <asp:Label ID="lblStateAbbreviation" Text='<%# "(" + Eval("StateAbbreviation") + ")" %>' runat="server"></asp:Label>
                            </td>
                            <td class="stateCheckbox">
                                <asp:CheckBox ID="chkStateSearch" runat="server" Enabled="false" Text="State Search" onclick="UnCheckHeader(this)" />
                            </td>
                            <td class="countyCheckbox">
                                <asp:CheckBox ID="chkCountySearch" runat="server" Enabled="false" Text="County Search" onclick="UnCheckHeader(this)" />
                            </td>
                        </tr>
                        <tr style="height: 3px !important;">
                            <td colspan="4"></td>
                        </tr>
                        <%--<div class="content" style="margin-bottom: 0px !important;" id="#dvRptContent">
                            <div class="sxform auto">
                                <asp:Panel ID="pnlInternal" CssClass="sxpnl" runat="server">
                                    <div class='sxro sx1co' style="margin: 0px !important">
                                        <div class="sxlb">
                                            <asp:Label ID="lblStateName" Text='<%#Eval("StateName")%>' runat="server"></asp:Label>
                                            <asp:HiddenField ID="hdnStateID" Value='<%# Eval("StateID") %>' runat="server" />
                                            <asp:Label ID="Label1" Text='<%#"(" + Eval("StateAbbreviation") + ")"%>' runat="server"></asp:Label>
                                        </div>
                                        <div class="sxlm" style="margin: 0px !important; padding: 0px !important; height: 25px;">
                                            <asp:Label ID="lblStateSearch" Text="State Search" runat="server"></asp:Label>
                                            <ins:checkbox id="chkStateSearch" runat="server" enabled="false" />
                                            &nbsp;&nbsp;
                                            <asp:Label ID="lblCountySearch" Text="County Search" runat="server"></asp:Label>
                                            <asp:CheckBox ID="chkCountySearch" runat="server" Enabled="false" Checked="false" />
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>--%>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <div class="sxcbar">
                <div class="sxcmds" style="text-align: center">
                    <infs:WclButton ID="btnSaveStateSearchCriteria" runat="server" Visible="false" Text="Save" OnClick="btnSaveStateSearchCriteria_Click">
                        <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                    <infs:WclButton ID="btnCancelStateSearchCriteria" runat="server" Visible="false" Text="Cancel" OnClick="btnCancelStateSearchCriteria_Click">
                        <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">

    //Function to check alll checkboxes if header is checked
    function checkAll(searchType) {
        //debugger;
        var allCheckbox;
        var checkboxes;
        if (searchType == "stateSearch") {
            allCheckbox = $jQuery('.allStateCheckbox :checkbox');
            checkboxes = $jQuery('.stateCheckbox :checkbox');
        }
        else {
            allCheckbox = $jQuery('.allCountyCheckbox :checkbox');
            checkboxes = $jQuery('.countyCheckbox :checkbox');
        }
        if (allCheckbox[0].checked) {
            for (var i = 0; i < checkboxes.length; i++) {
                if (!checkboxes[i].checked) {
                    checkboxes[i].checked = true;
                }
            }
        }
        else {
            for (var i = 0; i < checkboxes.length ; i++) {
                if (checkboxes[i].checked) {
                    checkboxes[i].checked = false;
                }
            }
        }
    };

    //Function to uncheck header if any checkbox is unchecked and also rechecked header if again all checkboxes are checked
    function UnCheckHeader(sender) {
        //debugger;
        var btnID = sender.id;
        var allCheckbox;
        var searchType;
        if (btnID.indexOf("StateSearch") > 0) {
            allCheckbox = $jQuery('.allStateCheckbox :checkbox');
            searchType = "StateSearch";
        }
        else {
            allCheckbox = $jQuery('.allCountyCheckbox :checkbox');
        }
        if (!$jQuery('#' + sender.id)[0].checked) {
            allCheckbox[0].checked = false;
            searchType = "CountySearch";
        }
        else {
            CheckHeader(searchType);
        }
    }

    //Page-load function 
    function pageLoad() {
        //debugger;
        CheckHeader('StateSearch');
        CheckHeader('CountySearch');
    }

    //Function to check header checkbox
    function CheckHeader(searchType) {
        var allCheckbox;
        var checkboxes;
        var isUncheckFound = false;
        if (searchType == "StateSearch") {
            allCheckbox = $jQuery('.allStateCheckbox :checkbox');
            checkboxes = $jQuery('.stateCheckbox :checkbox');
        }
        else {
            allCheckbox = $jQuery('.allCountyCheckbox :checkbox');
            checkboxes = $jQuery('.countyCheckbox :checkbox');
        }
        for (var i = 0; i < checkboxes.length; i++) {
            if (!checkboxes[i].checked) {
                isUncheckFound = true;
                break;
            }
        }
        if (!isUncheckFound) {
            allCheckbox[0].checked = true;
        }
    }

</script>


