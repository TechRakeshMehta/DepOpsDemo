<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.AssignmentPropertiesDetail"
    Title="Update Assignment Options" MasterPageFile="~/Shared/ChildPage.master" CodeBehind="AssignmentPropertiesDetail.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxAssignment">
        <infs:LinkedResource ResourceType="JavaScript" Path="~/Resources/Mod/Compliance/AssignmentPropertiesDetail.js" />
    </infs:WclResourceManagerProxy>
    <div class="section">
        <h1 class="mhdr">Update Assignment Options</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                    <asp:HiddenField ID="hdnRuleSetTreeTypeCode" runat="server" />
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlAssignment">
                    <div class='sxro sx3co'>
                        <div id="divEffectiveDate" runat="server">
                            <div class='sxlb'>
                                <span class="cptn">Effective Date</span><span id="spanEffectiveDate" class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclDatePicker ID="dpkrEffectiveDate" runat="server" DateInput-EmptyMessage="Select a date">
                                    <%--<Calendar>
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="None" ItemStyle-CssClass="rcToday" />
                                    </SpecialDays>
                                </Calendar>--%>
                                </infs:WclDatePicker>
                                <div id="dvEffectiveDate" class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvEffectiveDate" ControlToValidate="dpkrEffectiveDate"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Effective Date is required." ValidationGroup='grpAssignment'
                                        Enabled="false" />
                                </div>
                            </div>
                        </div>
                        <div id="divEditableBy" runat="server">
                            <div class='sxlb'>
                                <span class="cptn">Editable By</span><span id="spanEditableBy" class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbEditableBy" runat="server" CheckBoxes="true" DataTextField="Name" AllowCustomText="false"
                                    DataValueField="ComplianceItemEditableByID" EmptyMessage="--Select--">
                                </infs:WclComboBox>
                                <div id="dvEditableBy" class='vldx'>
                                    <asp:CustomValidator ID="rfvEditableBy" CssClass="errmsg" Display="Dynamic" runat="server" Enabled="false"
                                        ErrorMessage="Editable By is required." EnableClientScript="true" ValidationGroup="grpAssignment"
                                        ClientValidationFunction="ValidateEditableBy">
                                    </asp:CustomValidator>
                                    <%-- <asp:RequiredFieldValidator runat="server" ID="rfvEditableBy" ControlToValidate="cmbEditableBy" ClientIDMode="Static"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Editable By is required." ValidationGroup='grpAssignment'
                                        Enabled="false" />--%>
                                </div>
                            </div>
                            <%--[Comments:Div that visible only for Is Active check for attribute Type result set tree node] --%>
                            <div id="dvIsActiveAttr" runat="server" style="display: none">
                                <div class='sxlb'>
                                    <span class="cptn">Is Active</span>
                                </div>
                                <div class='sxlm'>
                                    <%--<infs:WclButton runat="server" ID="chkIsActiveAttr" ToggleType="CheckBox" ButtonType="ToggleButton" OnClientCheckedChanged="HideShowEditableByValidator"
                                        AutoPostBack="false">
                                        <ToggleStates>
                                            <telerik:RadButtonToggleState Text="Yes" Value="True" Selected="true" />
                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                        </ToggleStates>
                                    </infs:WclButton>--%>
                                    <uc1:IsActiveToggle runat="server" ClientIDMode="Static" ID="chkIsActiveAttr" Checked="false" IsAutoPostBack="false" IsActiveEnable="true" OnClientCheckedChanged="HideShowEditableByValidator" />
                                </div>
                            </div>
                        </div>
                        <div id="divReviewedBy" runat="server">
                            <div class='sxlb'>
                                <span class="cptn">Reviewed By</span><span id="spanReviewedBy" class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbReviewedBy" runat="server" CheckBoxes="true" DataTextField="Name" AllowCustomText="false"
                                    DataValueField="ReviewerTypeID" EmptyMessage="--Select--" OnClientItemChecked="OnClientItemChecked"
                                    OnItemChecked="cmbReviewedBy_ItemChecked" AutoPostBack="false">
                                </infs:WclComboBox>
                                <div id="dvReviewedBy" class='vldx'>
                                    <asp:CustomValidator ID="rfvReviewedBy" CssClass="errmsg" Display="Dynamic" runat="server" Enabled="false"
                                        ErrorMessage="Reviewed By is required." EnableClientScript="true" ValidationGroup="grpAssignment"
                                        ClientValidationFunction="ValidateReviewedBy">
                                    </asp:CustomValidator>
                                    <%-- <asp:RequiredFieldValidator runat="server" ID="rfvReviewedBy" ControlToValidate="cmbReviewedBy"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Reviewed By is required." ValidationGroup='grpAssignment'
                                        Enabled="false" />--%>
                                </div>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div id="divThirdPartyReviewer" runat="server">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Approval Required</span><span id="spanApprovalReqd" class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rdoApprovalReqd" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                                    onClick="OnRdoApprovalReqdClick();">
                                    <asp:ListItem Text="Yes " Value="True" />
                                    <asp:ListItem Text="No" Value="False" />
                                </asp:RadioButtonList>
                                <div id="dvApprovalReqd" class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvApprovalReqd" ControlToValidate="rdoApprovalReqd"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Approval Required is required."
                                        ValidationGroup='grpAssignment' Enabled="false" />
                                </div>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Is Active</span>
                            </div>
                            <div class='sxlm'>
                                <%--     <infs:WclButton runat="server" ID="chkActive" ToggleType="CheckBox" ButtonType="ToggleButton"
                                    OnClientCheckedChanged="OnChkActiveCheckedChanged" AutoPostBack="false">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                    </ToggleStates>
                                </infs:WclButton>--%>
                                <uc1:IsActiveToggle runat="server" ClientIDMode="Static" ID="chkActive" Checked="false" IsAutoPostBack="false" IsActiveEnable="true" OnClientCheckedChanged="OnChkActiveCheckedChanged" />
                            </div>
                            <div class='sxlb' id="dvLableThirdPartyReviewer" runat="server" visible="false">
                                <span class="cptn">Third Party Reviewer</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbThirdPartyReviewer" runat="server" DataTextField="TenantName" AllowCustomText="false"
                                    DataValueField="TenantID" Visible="false" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged"
                                    OnSelectedIndexChanged="cmbThirdPartyReviewer_SelectedIndexChanged" AutoPostBack="true">
                                </infs:WclComboBox>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co' id="dvThirdPartyUser" runat="server">
                            <div class='sxlb' id="dvLabelThirdPartyUser">
                                <span class="cptn">Third Party User</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbThirdPartyUser" runat="server" DataTextField="FirstName" AllowCustomText="false"
                                    DataValueField="OrganizationUserID">
                                </infs:WclComboBox>
                                <div id="dvThirdPartyUserVldx" class='vldx'>
                                    <asp:CustomValidator ID="rfvThirdPartyUser" CssClass="errmsg" Display="Dynamic" runat="server" Enabled="false"
                                        ErrorMessage="Third Party User is required." EnableClientScript="true" ValidationGroup="grpAssignment"
                                        ClientValidationFunction="ValidateThirdPartyUser">
                                    </asp:CustomValidator>
                                    <%--<asp:RequiredFieldValidator runat="server" ID="rfvThirdPartyUser" ControlToValidate="cmbThirdPartyUser"
                                        class="errmsg" ErrorMessage="Third Party User is required." InitialValue="--SELECT--"
                                        Display="Dynamic" Enabled="false" ValidationGroup='grpAssignment' />--%>
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                    <div class='sxro sx3co' runat="server" id="divAllowExcep" visible="false">
                        <div class="sxgrp">
                            <div class='sxlb'>
                                <span class="cptn">Allow Exceptions</span><span id="spnAllowException" class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList runat="server" ID="rdAllowCatException" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes&nbsp;" Value="true" Selected="True" />
                                    <asp:ListItem Text="No" Value="false" />
                                </asp:RadioButtonList>
                                <div id="divAllowException" class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvAllowException" ControlToValidate="rdAllowCatException"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Allow Exceptions is required."
                                        ValidationGroup='grpAssignment' Enabled="false" />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                    <div class='sxro sx3co' runat="server" id="divItemDataEntry" visible="false">
                        <div class="sxgrp">
                            <div class='sxlb'>
                                <span class="cptn">Item Data Entry</span><span id="spnItemDataEntry" class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList runat="server" ID="rdbItemDataEntry" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Allowed&nbsp;" Value="true" Selected="True" />
                                    <asp:ListItem Text="Not Allowed" Value="false" />
                                </asp:RadioButtonList>
                                <div id="divItemDataEntryVldxMsg" class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvItemDataEntry" ControlToValidate="rdbItemDataEntry"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Item Data Entry is required."
                                        ValidationGroup='grpAssignment' Enabled="false" />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>

                    <div class='sxro sx3co' runat="server" id="divAllowDataEntry" visible="false">
                        <div class="sxgrp">
                            <div class='sxlb'>
                                <span class="cptn">Allow Admin Data Entry</span><span id="spnAllowDataEntry" class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList runat="server" ID="rdAllowDataEntry" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes&nbsp;" Value="true" Selected="True" />
                                    <asp:ListItem Text="No" Value="false" />
                                </asp:RadioButtonList>
                                <div id="divRfvAllowDataEntry" class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvAllowDataEntry" ControlToValidate="rdAllowDataEntry"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Allow Data Entry is required."
                                        ValidationGroup='grpAssignment' Enabled="false" />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                    <div class='sxro sx3co' runat="server" id="dvEnableUpdateAllTime" visible="false">
                        <div class="sxgrp">
                            <div class='sxlb'>
                                <span class="cptn">Enable Update All Time</span><span id="spnEnableUpdateAllTime" class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList runat="server" ID="rdbEnableUpdateAllTime" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes&nbsp;" Value="true" />
                                    <asp:ListItem Text="No" Value="false" Selected="True"/>
                                </asp:RadioButtonList>                         
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <infsu:CommandBar ID="fsucCmdBarAssignment" runat="server" DefaultPanel="pnlAssignment"
                DisplayButtons="Save" SaveButtonText="Save" AutoPostbackButtons="Save" OnSaveClick="fsucCmdBarAssignment_SaveClick"
                ValidationGroup="grpAssignment">
            </infsu:CommandBar>
        </div>
        <asp:HiddenField ID="hdnIsActiveYesClientIDAttr" runat="server" Value="" />
        <asp:HiddenField ID="hdnIsActiveYesPAKCAT" runat="server" Value="" />
    </div>
</asp:Content>
