<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminEntryPersonAliasInfo.ascx.cs" Inherits="CoreWeb.AdminEntryPortal.Views.AdminEntryPersonAliasInfo" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="~/Resources/Mod/AdminEntryPortal/AdminEntryPersonAliasInfo.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<style>
    .btnUpdate {
        color: black;
        background-color: #e9e9e9;
        padding: 4px 9px;
        font-size: 12px;
        vertical-align: middle;
        -ms-touch-action: manipulation;
        cursor: pointer;
        -moz-user-select: none;
        -ms-user-select: none;
        border: 1px solid #0000006e;
        border-radius: 4px;
        text-decoration: none !important;
    }

    .rbSaveIcon {
        position: absolute;
        width: 16px;
        height: 16px;
        background-repeat: no-repeat;
        cursor: default;
    }

    .addAlias {
        display: inline-block;
        outline: none;
        cursor: pointer;
        color: black;
        background-color: #EFEFEF;
        text-align: center;
        text-decoration: none;
        padding: 0.2em 1.3em 0.2em;
        text-shadow: 0 1px 1px rgba(0,0,0,.3);
        -webkit-border-radius: .3em !important;
        -moz-border-radius: .5em !important;
        border-radius: .3em !important;
        -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2) !important;
        -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
        box-shadow: 0 1px 2px rgba(0,0,0,.2);
        margin-right: 4px;
    }

        .addAlias:hover {
            background: #c7c6c6;
            background: -webkit-gradient(linear, left top, left bottom, from(#efefef), to(#c7c6c6));
            background: -moz-linear-gradient(top, #efefef, #c7c6c6);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#efefef', endColorstr='#c7c6c6');
        }

    .aspNetDisabled {
        cursor: not-allowed !important;
    }
</style>
<input type="checkbox" runat="server" id="chkShowHideAlias" class="chkShowHideAlias" onchange="HideShow(this)" value="<%$Resources:Language,ISALIASNAME %>" /><span id="lblChkShowHide" runat="server" onclick="CheckUncheck();"><%=Resources.Language.ISALIASNAME %></span>
<div id="divPersonalAlias" runat="server">
    <asp:Repeater runat="server" ID="rptrAliasName" OnItemCommand="rptrAliasName_ItemCommand" OnItemDataBound="rptrAliasName_ItemDataBound">
        <ItemTemplate>
            <div class='sxro sx3co aliasDiv'>
                <div class='sxlb' id="dvSpanAliasFN" runat="server">
                    <span class='cptn'><%=Resources.Language.ALIASFIRSTNAME %></span>
                </div>
                <div class='sxlm' id="dvAliasFirstName" runat="server">
                    <asp:Label runat="server" ID="lblfirstName"><%# Eval("FirstName") %> </asp:Label>
                    <infs:WclTextBox runat="server" ID="txtFirstName1" Text='<%# Eval("FirstName") %>' Enabled="false" Visible="false"></infs:WclTextBox>
                    <infs:WclTextBox runat="server" CssClass="helloAni" ID="txtFirstName" Text='<%# Eval("FirstName") %>' Visible="false"></infs:WclTextBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvFirstName" ControlToValidate="txtFirstName"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,ALIASFIRSTNAMEREQ %>" ValidationGroup="grpPersonAliasEdit"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator runat="server" ID="revFirstName" Visible="false" ControlToValidate="txtFirstName"
                            Display="Dynamic" CssClass="errmsg"
                            ErrorMessage="<%$Resources:Language,ALIASFIRSTMIDNAMEREGEX %>" ValidationGroup="grpPersonAliasEdit" ValidationExpression="^(?=.{1,30}$)(([a-zA-Z])+( ?[a-zA-Z]+)|([a-zA-Z]{1}))$" />
                    </div>
                </div>
                <!--UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality-->
                <div class='sxlb' id="dvSpanAliasMN" runat="server">
                    <span class='cptn'><%=Resources.Language.ALIASMIDDLENAME %></span>
                </div>
                <div class='sxlm' id="dvAliasMiddleName" runat="server">
                    <asp:Label runat="server" ID="lblMiddleName"><%# Eval("MiddleName") %> </asp:Label>
                    <infs:WclTextBox runat="server" ID="txtMiddleName1" placeholder="<%$Resources:Language,IFYOUDONTHAVEMIDDLENAME %>"
                        ToolTip="<%$Resources:Language, IFYOUDONTHAVEMIDDLENAME %>" Text='<%# Eval("MiddleName") %>' Enabled="false" Visible="false">
                    </infs:WclTextBox>
                    <div class="mddName">
                        <infs:WclTextBox runat="server" CssClass="helloAni" placeholder="<%$Resources:Language,IFYOUDONTHAVEMIDDLENAME %>"
                            ToolTip="<%$Resources:Language, IFYOUDONTHAVEMIDDLENAME %>" ID="txtMiddleName" Text='<%# Eval("MiddleName") %>' Visible="false">
                        </infs:WclTextBox>
                    </div>
                    <div class="vldx vlMiddelName">
                        <asp:RequiredFieldValidator runat="server" ID="rfvMiddleName" ControlToValidate="txtMiddleName"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,ALIASMIDDLENAMEREQ %>" ValidationGroup="grpPersonAliasEdit" />
                        <%-- <asp:RegularExpressionValidator runat="server" ID="revMiddleName" ControlToValidate="txtMiddleName"
                            Display="Dynamic" CssClass="errmsg" Visible="false"
                            ErrorMessage="<%$Resources:Language,ALIASFIRSTMIDNAMEREGEX %>" ValidationGroup="grpPersonAliasEdit" ValidationExpression="^(?=.{1,30}$)(([a-zA-Z])+( ?[a-zA-Z]+)|([a-zA-Z]{1}))$" />--%>
                    </div>
                </div>
                <div class='sxlb' id="dvSpanAliasLN" runat="server">
                    <span class='cptn'><%=Resources.Language.ALIASLASTNAME %></span>
                </div>
                <div class='sxlm' id="dvAliasLastName" runat="server">
                    <asp:Label runat="server" ID="lblLastName"><%# Eval("LastName") %></asp:Label>
                    <infs:WclTextBox runat="server" ID="txtLastName1" Text='<%# Eval("LastName") %>' Enabled="false" Visible="false"></infs:WclTextBox>
                    <infs:WclTextBox runat="server" ID="txtLastName" Text='<%# Eval("LastName") %>' Visible="false"></infs:WclTextBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                            Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,ALIASLASTNAMEREQ %>" ValidationGroup="grpPersonAliasEdit" />
                        <asp:RegularExpressionValidator runat="server" ID="revLastName" ControlToValidate="txtLastName"
                            Display="Dynamic" CssClass="errmsg" Visible="false"
                            ErrorMessage="<%$Resources:Language,ALIASLASTNAMEREGEX %>" ValidationGroup="grpPersonAliasEdit" ValidationExpression="^(?=.{1,30}$)(([a-zA-Z])+(-?[a-zA-Z]+)|([a-zA-Z]{1}))$" />
                    </div>
                </div>
                <div class='sxlm' id="dvAliasSuffix" runat="server">
                    <asp:Label runat="server" ID="lblSuffix"><%# Eval("Suffix") %></asp:Label>
                    <infs:WclComboBox ID="cmbAliasSuffix1" runat="server" Enabled="false" Visible="false" DataTextField="LAES_Suffix" DataValueField="LAES_ID" Width="80%"></infs:WclComboBox>
                    <infs:WclComboBox ID="cmbAliasSuffix" runat="server" DataTextField="LAES_Suffix" DataValueField="LAES_ID" Visible="false" Width="80%"></infs:WclComboBox>
                </div>
                <div class='sxroend'>
                </div>
            </div>
            <div class="sxro sx3co" runat="server" id="divMiddleNameCheckBoxRepeater">
                <div class='sxlb nobg toHide'>
                </div>
                <div class='sxlm'>
                </div>
                <infs:WclCheckBox runat="server" ID="chkMiddleNameRequiredRepeater" onclick="AliasMiddleNameEnableDisableForRepeater(this)"></infs:WclCheckBox>
                <asp:Label ID="lblChkMiddleName" Style="color: red; font-weight: bold" runat="server"><%=Resources.Language.MIDDLENAMENA %></asp:Label>
                <div class='sxroend'>
                </div>
            </div>
            <div id="divButtons" runat="server" class="sxro sx3co" style="margin-top: 7px; margin-bottom: 7px;">
                <div style="margin-top: 10px; background: none;">
                    <asp:LinkButton CommandName="edit" CssClass="addAlias" runat="server" ID="btnEdit" CausesValidation="true" Text="<%$Resources:Language,EDIT %>" ValidationGroup="grpPersonAliasEdit">
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" CssClass="addAlias" OnClientClick="return OnAliasDelete_ClientClicked();" Text="<%$Resources:Language,DLT %>" runat="server" CommandName="delete" ValidationGroup="grpPersonAliasEdit" CausesValidation="false">
                    </asp:LinkButton>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div id="divFooter" runat="server">
        <div class='sxro sx3co' runat="server">
            <div class='sxlb' runat="server" id="dvSpanNewAliasFN">
                <span class='cptn'><%=Resources.Language.ALIASFIRSTNAME %></span>
            </div>
            <div class='sxlm' id="dvAliasNewFirstName" runat="server">
                <infs:WclTextBox runat="server" ID="txtNewFirstName"></infs:WclTextBox>
                <div class="vldx">
                    <asp:RequiredFieldValidator runat="server" ID="rfvNewFirstName" ControlToValidate="txtNewFirstName"
                        Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,ALIASFIRSTNAMEREQ %>" ValidationGroup="grpPersonAlias" />
                    <asp:RegularExpressionValidator runat="server" ID="revNewFirstName" Visible="false" ControlToValidate="txtNewFirstName"
                        Display="Dynamic" CssClass="errmsg"
                        ErrorMessage="<%$ Resources:Language, FIRSTNAMEVALDT %>" ValidationGroup="grpPersonAlias" ValidationExpression="^(?=.{1,30}$)(([a-zA-Z])+( ?[a-zA-Z]+)|([a-zA-Z]{1}))$" />
                </div>
            </div>
            <div class='sxlb' id="dvSpanNewAliasMN" runat="server">
                <span class='cptn'><%=Resources.Language.ALIASMIDDLENAME %></span>
            </div>
            <div class='sxlm' id="dvAliasNewMiddleName" runat="server">
                <infs:WclTextBox runat="server" ID="txtNewMiddleName" placeholder="<%$Resources:Language,IFYOUDONTHAVEMIDDLENAME %>"
                    ToolTip="<%$Resources:Language, IFYOUDONTHAVEMIDDLENAME %>">
                </infs:WclTextBox>
                <div class="vldx">
                    <asp:RequiredFieldValidator runat="server" ID="rfvNewMiddleName" ControlToValidate="txtNewMiddleName"
                        Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,ALIASMIDDLENAMEREQ %>" ValidationGroup="grpPersonAlias" />
                    <%--<asp:RegularExpressionValidator runat="server" ID="revNewMiddleName" ControlToValidate="txtNewMiddleName"
                        Display="Dynamic" CssClass="errmsg" Visible="false"
                        ErrorMessage="<%$ Resources:Language, MIDDLENAMEVALDT %>" ValidationGroup="grpPersonAlias" ValidationExpression="^(?=.{1,30}$)((([a-zA-Z])+( ?[a-zA-Z]+))|(-{5})|([a-zA-Z]{1}))$" />--%>
                </div>
            </div>
            <div class='sxlb' runat="server" id="dvSpanNewAliasLN">
                <span class='cptn'><%=Resources.Language.ALIASLASTNAME %></span>
            </div>
            <div class='sxlm' id="dvAliasNewLastName" runat="server">
                <infs:WclTextBox runat="server" ID="txtNewLastName"></infs:WclTextBox>
                <div class="vldx">
                    <asp:RequiredFieldValidator runat="server" ID="rfvNewLastName" ControlToValidate="txtNewLastName"
                        Display="Dynamic" CssClass="errmsg" ErrorMessage="<%$Resources:Language,ALIASLASTNAMEREQ %>" ValidationGroup="grpPersonAlias" />
                    <asp:RegularExpressionValidator runat="server" ID="revNewLastName" ControlToValidate="txtNewLastName"
                        Display="Dynamic" CssClass="errmsg" Visible="false"
                        ErrorMessage="<%$ Resources:Language, LASTNAMEVALDT %>" ValidationGroup="grpPersonAlias" ValidationExpression="^(?=.{1,30}$)(([a-zA-Z])+(-?[a-zA-Z]+)|([a-zA-Z]{1}))$" />

                </div>
            </div>
            <div class='sxlm' id="dvAliasNewSuffix" runat="server" style="display: none; width: 10%">
                <asp:HiddenField ID="hdnNewSuffixId" runat="server" Value="" />
                <infs:WclComboBox ID="cmbAliasNewSuffix" runat="server" DataTextField="LAES_Suffix" DataValueField="LAES_ID" Width="80%"></infs:WclComboBox>
            </div>
            <div class='sxroend'>
            </div>
        </div>
        <div class="sxro sx3co" runat="server" id="divMiddleNameCheckBox">
            <div id="dvHasGreyBackground" runat="server" class='sxlb nobg'>
            </div>
            <div class='sxlm'>
            </div>
            <infs:WclCheckBox runat="server" ID="chkMiddleNameRequired" onclick="AliasMiddleNameEnableDisable(this)"></infs:WclCheckBox>
            <asp:Label ID="lblChkMiddleName" Style="color: red; font-weight: bold" runat="server"><%=Resources.Language.MIDDLENAMENA %></asp:Label>
            <div class='sxroend'>
            </div>
        </div>
        <div class="sxro sx3co">
            <div class="aliasBottmMargin" style="margin-bottom: 3px; margin-top: 5px; background: none;">
                <asp:LinkButton ID="btnAddNewRecord" CssClass="addAlias" runat="server" OnClientClick="ValidateVerifyAlias()" OnClick="OnAddRecord" Text="<%$Resources:Language,ADDALIAS %>" CausesValidation="true" ValidationGroup="grpPersonAlias">
                </asp:LinkButton>
            </div>
        </div>
    </div>
    <div class='sxro sx3co' id="divErrorMessage" runat="server" visible="false">
        <asp:Label ID="lblErrorMsg" runat="server" Text="" CssClass="errmsg"></asp:Label>
        <div class='sxroend'>
        </div>
    </div>

    <asp:HiddenField ID="hdnIFYOUDONTHAVEMIDDLENAME" runat="server" Value="<%$Resources:Language,IFYOUDONTHAVEMIDDLENAME %>" />
    <asp:HiddenField ID="hdnIsPersonAliasLocationTenant" runat="server" Value="false" />
</div>

<script type="text/javascript">
    function OnAliasDelete_ClientClicked() {
        var _aliasDelConfirmMsg = "<%=Resources.Language.CONFMALIASDEL %>";
        return confirm(_aliasDelConfirmMsg);
    }
</script>
