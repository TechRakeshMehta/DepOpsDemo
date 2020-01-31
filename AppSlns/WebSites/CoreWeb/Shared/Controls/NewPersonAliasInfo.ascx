<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewPersonAliasInfo.ascx.cs"
    Inherits="CoreWeb.Shell.Views.NewPersonAliasInfo" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="~/Resources/Mod/Shared/PersonAliasInfo.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="row bgLightGreen">
    <div class='col-md-12'>
        <input type="checkbox" runat="server" id="chkShowHideAlias" class="chkShowHideAlias "
            style="margin-bottom: 4px;"
            onchange="HideShow(this)" title="Click here to show hide Alias or Maiden." value="I have an Alias or Maiden" /><span
                id="lblChkShowHide" runat="server" class="font14" onclick="CheckUncheck();">I have an Alias or Maiden</span>
    </div>
    <div id="divPersonalAlias" runat="server">
        <asp:Repeater runat="server" ID="rptrAliasName" OnItemCommand="rptrAliasName_ItemCommand"
            OnItemDataBound="rptrAliasName_ItemDataBound">
            <ItemTemplate>
                <div class='col-md-12 aliasDiv'>
                    <div class="row">
                        <div class='form-group col-md-3'>
                            <span class='cptn'>Alias/Maiden First Name</span>
                            <asp:Label runat="server" ID="lblfirstName" Width="100%" CssClass="form-control"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("FirstName"))) %> </asp:Label>
                            <infs:WclTextBox runat="server" ID="txtFirstName1" Text='<%# Eval("FirstName") %>'
                                Enabled="false" Visible="false" Width="100%" CssClass="form-control">
                            </infs:WclTextBox>
                            <infs:WclTextBox runat="server" CssClass="helloAni form-control" ID="txtFirstName"
                                Text='<%# Eval("FirstName") %>' Visible="false" Width="100%">
                            </infs:WclTextBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvFirstName" ControlToValidate="txtFirstName"
                                    Display="Dynamic" CssClass="errmsg" ErrorMessage="Alias/Maiden First Name is required."
                                    ValidationGroup="grpPersonAliasEdit" />
                                <asp:RegularExpressionValidator runat="server" ID="revFirstName" Visible="false" ControlToValidate="txtFirstName"
                                    Display="Dynamic" CssClass="errmsg"
                                    ErrorMessage="First Name must be between 1 to 30 characters and must contains letters." ValidationGroup="grpPersonAliasEdit" ValidationExpression="^[a-zA-Z ]{1,30}$" />
                            </div>
                        </div>
                        <div class='form-group col-md-3'>
                            <span class='cptn'>Alias/Maiden Middle Name</span>
                            <asp:Label runat="server" ID="lblMiddleName" Width="100%" CssClass="form-control"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("MiddleName"))) %> </asp:Label>
                            <infs:WclTextBox runat="server" ID="txtMiddleName1" Width="100%" CssClass="form-control"
                                placeholder="If you don't have a middle name,check the box below."
                                ToolTip="If you don't have a middle name,check the box below." Text='<%# Eval("MiddleName") %>' Enabled="false" Visible="false">
                            </infs:WclTextBox>
                            <div class="mddName">
                                <infs:WclTextBox runat="server" Width="100%" CssClass="form-control" ID="txtMiddleName"
                                    placeholder="If you don't have a middle name,check the box below."
                                    ToolTip="If you don't have a middle name,check the box below."
                                    Text='<%# Eval("MiddleName") %>' Visible="false">
                                </infs:WclTextBox>
                            </div>
                            <div class="vldx vlMiddelName">
                                <asp:RequiredFieldValidator runat="server" ID="rfvMiddleName" ControlToValidate="txtMiddleName"
                                    Display="Dynamic" CssClass="errmsg" ErrorMessage="Alias/Maiden Middle Name is required." ValidationGroup="grpPersonAliasEdit" />
                                <asp:RegularExpressionValidator runat="server" ID="revMiddleName" ControlToValidate="txtMiddleName"
                                    Display="Dynamic" CssClass="errmsg" Visible="false"
                                    ErrorMessage="Middle Name must be between 1 to 30 characters and must contains letters." ValidationGroup="grpPersonAliasEdit" ValidationExpression="^[a-zA-Z ]{1,30}$" />
                            </div>
                        </div>
                        <div class='form-group col-md-3'>
                            <span class='cptn'>Alias/Maiden Last Name</span>
                            <asp:Label runat="server" ID="lblLastName" Width="100%" CssClass="form-control"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("LastName"))) %></asp:Label>
                            <infs:WclTextBox runat="server" ID="txtLastName1" Text='<%# Eval("LastName") %>'
                                Enabled="false" Visible="false" Width="100%" CssClass="form-control">
                            </infs:WclTextBox>
                            <infs:WclTextBox runat="server" ID="txtLastName" Text='<%# Eval("LastName") %>' Visible="false"
                                Width="100%" CssClass="form-control">
                            </infs:WclTextBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                                    Display="Dynamic" CssClass="errmsg" ErrorMessage="Alias/Maiden Last Name is required."
                                    ValidationGroup="grpPersonAliasEdit" />
                                <asp:RegularExpressionValidator runat="server" ID="revLastName" ControlToValidate="txtLastName"
                                    Display="Dynamic" CssClass="errmsg" Visible="false"
                                    ErrorMessage="Last Name must be between 1 to 30 characters and must contains letters, hyphen(-)." ValidationGroup="grpPersonAliasEdit" ValidationExpression="^([a-zA-Z])+(-[a-zA-Z]+)?$" />
                            </div>
                        </div>
                        <div class='form-group col-md-3' style="margin-top: 22px">
                            <div class="row">
                                <div class='form-group col-md-6' id="dvAliasSuffix" runat="server" visible="false">
                                    <asp:Label runat="server" ID="lblSuffix"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Suffix"))) %></asp:Label>
                                    <%--<asp:Label ID="hdnSuffixId" CssClass="form-control" runat="server" Visible="false"><%# Eval("SuffixID") %></asp:Label>--%>
                                    <infs:WclTextBox ID="txtAliasSuffix1" runat="server" Enabled="false" Visible="false" MaxLength="10"></infs:WclTextBox>
                                    <infs:WclTextBox ID="txtAliasSuffix" runat="server" Visible="false" MaxLength="10"></infs:WclTextBox>
                                    <infs:WclComboBox ID="cmbAliasSuffix" Skin="Silk" runat="server" EmptyMessage="--Suffix, if Any--" DataTextField="Suffix" DataValueField="SuffixID" Width="100%" Visible="false" AutoSkinMode="false"></infs:WclComboBox>
                                    <div class="vldx">
                                        <asp:RegularExpressionValidator runat="server" ID="rfvAliasSuffix" ControlToValidate="txtAliasSuffix"
                                            Display="Dynamic" CssClass="errmsg"
                                            ErrorMessage="Only alphabets or single hyphen(-) is allowed in the last name up to maximum of 10 characters." ValidationGroup="grpPersonAlias"
                                            ValidationExpression="^[a-z A-Z]*-?[a-z A-Z]*$" />
                                    </div>
                                </div>
                                <%--<div class='form-group col-md-3'></div>--%>
                                <div class='form-group col-md-6' id="divButtons" runat="server">
                                    <div class="row">
                                        <div class="col-md-2" id="dvlocationAlias" runat="server" visible="false"></div>
                                        <div class="col-md-3">
                                            <asp:LinkButton CommandName="edit" runat="server" Text="Edit" ID="btnEdit" CausesValidation="true"
                                                ValidationGroup="grpPersonAliasEdit" CssClass="form-control blueText"></asp:LinkButton>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:LinkButton ID="btnDelete" OnClientClick="return confirm('Are you sure you want to delete the Alias/Maiden ?')"
                                                runat="server" CommandName="delete" Text="Delete" ValidationGroup="grpPersonAliasEdit"
                                                CausesValidation="false" CssClass="form-control blueText" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div runat="server" id="divMiddleNameCheckBoxRepeater">
                    <div class='col-md-3'></div>
                    <div class='col-md-4'>
                        <infs:WclCheckBox runat="server" ID="chkMiddleNameRequiredRepeater" Style="color: red; font-weight: bold" Text="Middle Name not applicable to this Alias"
                            onclick="AliasMiddleNameEnableDisableForPortfolioRepeater(this)"></infs:WclCheckBox>
                        <%-- <asp:Label ID="lblChkMiddleName" class="font14" runat="server"></asp:Label>--%>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <div id="divFooter" runat="server">
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <label id="lblAliasFirstName" class='cptn'>Alias/Maiden First Name</label>
                        <infs:WclTextBox EnableAriaSupport="true" aria-labelledby="lblAliasFirstName" runat="server" ID="txtNewFirstName" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvNewFirstName" ControlToValidate="txtNewFirstName"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Alias/Maiden First Name is required."
                                ValidationGroup="grpPersonAlias" />
                            <asp:RegularExpressionValidator runat="server" ID="revNewFirstName" Visible="false" ControlToValidate="txtNewFirstName"
                                Display="Dynamic" CssClass="errmsg"
                                ErrorMessage="First Name must be between 1 to 30 characters and must contains letters." ValidationGroup="grpPersonAlias" ValidationExpression="^[a-zA-Z ]{1,30}$" />
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <label id="lblAliasMiddleName" class='cptn'>Alias/Maiden Middle Name</label>
                        <infs:WclTextBox runat="server" EnableAriaSupport="true" aria-labelledby="lblAliasMiddleName"
                            placeholder="If you don't have a middle name,check the box below."
                            ToolTip="If you don't have a middle name,check the box below." ID="txtNewMiddleName"
                            Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvNewMiddleName" ControlToValidate="txtNewMiddleName"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Alias/Maiden Middle Name is required." ValidationGroup="grpPersonAlias"
                                placeholder="If you don't have a middle name,check the box below."
                                ToolTip="If you don't have a middle name,check the box below." />
                            <asp:RegularExpressionValidator runat="server" ID="revNewMiddleName" ControlToValidate="txtNewMiddleName"
                                Display="Dynamic" CssClass="errmsg" Visible="false"
                                ErrorMessage="Middle Name must be between 1 to 30 characters and must contains letters." ValidationGroup="grpPersonAlias" ValidationExpression="^[a-zA-Z ]{1,30}$" />
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <label id="lblAliasLastName" class='cptn'>Alias/Maiden Last Name</label>
                        <infs:WclTextBox runat="server" EnableAriaSupport="true" aria-labelledby="lblAliasLastName" ID="txtNewLastName" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvNewLastName" ControlToValidate="txtNewLastName"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Alias/Maiden Last Name is required."
                                ValidationGroup="grpPersonAlias" />
                            <asp:RegularExpressionValidator runat="server" ID="revNewLastName" ControlToValidate="txtNewLastName"
                                Display="Dynamic" CssClass="errmsg" Visible="false"
                                ErrorMessage="Last Name must be between 1 to 30 characters and must contains letters, hyphen(-)." ValidationGroup="grpPersonAlias" ValidationExpression="^([a-zA-Z]+)?-?([a-zA-Z]+)?$" />
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <div class="row">
                            <div class='form-group col-md-7' style="margin-top: 29px" id="dvAliasNewSuffix" runat="server">
                                <asp:HiddenField ID="hdnNewSuffixId" runat="server" Value="" />
                                <infs:WclTextBox ID="txtAliasNewSuffix" runat="server" Width="100%" MaxLength="10" ToolTip="Enter Suffix if Applicable" placeholder="Enter Suffix if Applicable" Visible="false"></infs:WclTextBox>
                                <infs:WclComboBox ID="cmbAliasNewSuffix" Skin="Silk" runat="server" DataTextField="Suffix" DataValueField="SuffixID" Width="100%" Visible="false" AutoSkinMode="false"></infs:WclComboBox>
                                <div class="vldx">
                                    <asp:RegularExpressionValidator runat="server" ID="rfvAliasNewSuffix" ControlToValidate="txtAliasNewSuffix"
                                        Display="Dynamic" CssClass="errmsg"
                                        ErrorMessage="Only alphabets or single hyphen(-) is allowed in the last name up to maximum of 10 characters." ValidationGroup="grpPersonAlias"
                                        ValidationExpression="^[a-z A-Z]*-?[a-z A-Z]*$" />
                                </div>
                            </div>

                            <div class='form-group col-md-5' style="margin-top: 9px">
                                <span class="cptn" style="color: transparent !important;"></span>
                                <asp:LinkButton ID="btnAddNewRecord" runat="server" OnClick="OnAddRecord" OnClientClick="ValidateVerifyAlias()" Text="Add" ToolTip="Add an Alias/Maiden"
                                    CausesValidation="true" ValidationGroup="grpPersonAlias"
                                    CssClass="form-control blueText" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div runat="server" id="divMiddleNameCheckBox">
                <div class='col-md-3'></div>
                <div class='col-md-4'>
                    <infs:WclCheckBox runat="server" ID="chkMiddleNameRequired" Style="color: red; font-weight: bold" Text="Middle Name not applicable to this Alias" onclick="AliasMiddleNameEnableDisableForPortfolio(this)"></infs:WclCheckBox>
                    <%--<asp:Label ID="lblChkMiddleName" class="font14" runat="server">I don't have Alias/Maiden Middle Name.</asp:Label>--%>
                </div>
            </div>
        </div>
        <div class='col-md-12' id="divErrorMessage" runat="server" visible="false">
            <asp:Label ID="lblErrorMsg" runat="server" Text="" CssClass="errmsg"></asp:Label>
        </div>
    </div>
    <asp:HiddenField ID="hdnIsPersonAliasLocationTenant" runat="server" Value="false" />
    <asp:HiddenField ID="hdnIFYOUDONTHAVEMIDDLENAME" runat="server" Value="<%$Resources:Language,IFYOUDONTHAVEMIDDLENAME %>" />

</div>

<script type="text/javascript">

    function ValidateVerifyAlias() {
       
        var IsLocationTenant = $jQuery("[id$=hdnIsPersonAliasLocationTenant]")[0].value;
        var hdnIFYOUDONTHAVEMIDDLENAME = $jQuery("[id$=hdnIFYOUDONTHAVEMIDDLENAME]")[0].value;
        var middlename = $find($jQuery("[id$=txtNewMiddleName]")[0].id);
        middlename._element.setAttribute("Placeholder", hdnIFYOUDONTHAVEMIDDLENAME);
        middlename._element.setAttribute("title", hdnIFYOUDONTHAVEMIDDLENAME);
        if (IsLocationTenant.toLowerCase() == "true") {
            if ($jQuery("[id$=chkMiddleNameRequired]")[1].checked) {
                ValidatorEnable($jQuery("[id$=rfvNewMiddleName]")[0], false);
            }
        }
    }
</script>
