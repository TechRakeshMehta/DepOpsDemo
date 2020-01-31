<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditServiceDetail.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.EditServiceDetail" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<h1 class="mhdr">Update Service</h1>
<div class="content">
    <div class="sxform auto">
        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPackage">
            <div class="sxgrp" runat="server" id="divCreate" visible="true">
                <div class='sxro sx2co'>
                    <div class='sxlb'>
                        <span class="cptn">Services</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtServiceName" runat="server" Enabled="false" />
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Display Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtSvcDisplayName" Text="" MaxLength="256">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx2co'>
                    <%--UAT-3109--%>
                    <div class='sxlb'>
                        <span class="cptn">External Code</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtSvcAMERNumber" runat="server" Enabled="false" />
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Notes</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtSvcNotes" Text="" MaxLength="1024">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx2co' id="divSettings" runat="server" style="display: none;">
                    <div id="divYears" runat="server" visible="false">
                        <div class='sxlb'>
                            <span class="cptn">Number Of Years Of Residence</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtResidenceDuration" Text="" MaxLength="9">
                            </infs:WclTextBox>
                            <div class="vldx">
                                <asp:RegularExpressionValidator ID="revResidenceDuration" runat="server" ControlToValidate="txtResidenceDuration"
                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please enter valid number."
                                    ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                    </div>
                    <div id="divDocToStud" runat="server" visible="false">
                        <div class='sxlb'>
                            <span class="cptn">Send Documents To Applicant</span>
                        </div>
                        <div class='sxlm'>
                            <asp:CheckBox ID="chkSendDocsToStudent" runat="server" Checked="false" />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <%-- <div class='sxro sx2co' id="divSettings2" runat="server" style="display: none;">
                     <div id="divMinOcc" runat="server" visible="false">
                        <div class='sxlb'>
                            <span class="cptn">Min Occurrences</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtMinOccurrences" Text="" MaxLength="9">
                            </infs:WclTextBox>
                            <div class="vldx">
                                <asp:RegularExpressionValidator ID="revMinOccurrences" runat="server" ControlToValidate="txtMinOccurrences"
                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please enter valid number."
                                    ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                    </div>
                    <div id="divMaxOcc" runat="server" visible="false">
                        <div class='sxlb'>
                            <span class="cptn">Max Occurrences</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtMaxOccurrences" Text="" MaxLength="9">
                            </infs:WclTextBox>
                            <div class="vldx">
                                <asp:RegularExpressionValidator ID="revMaxOccurrences" runat="server" ControlToValidate="txtMaxOccurrences"
                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please enter valid number."
                                    ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                <asp:CompareValidator ID="cvMaxOcc" runat="server" Operator="GreaterThan" ControlToCompare="txtMinOccurrences" ControlToValidate="txtMaxOccurrences"
                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Max Occurrence should be greater than Min Occurence." />
                            </div>
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>--%>
                <div class='sxro sx2co' id="divSettings3" runat="server" style="display: none;">
                    <div id="divIsSupplemental" runat="server" visible="false">
                        <div class='sxlb'>
                            <span class="cptn">Is Supplemental</span>
                        </div>
                        <div class='sxlm'>
                            <asp:CheckBox ID="ChkIsSupplemental" runat="server" Checked="false" />
                        </div>
                    </div>
                    <div id="divIgnoreRHSuppl" runat="server" visible="false">
                        <div class='sxlb'>
                            <span class="cptn">Ignore Residential History On Supplement</span>
                        </div>
                        <div class='sxlm'>
                            <asp:CheckBox ID="ChkIgnoreRHSuppl" runat="server" Checked="false" />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>

            </div>
        </asp:Panel>
    </div>
    <div class="sxcbar">
        <div class="sxcmds" style="text-align: right">
            <infs:WclButton ID="btnEdit" runat="server" Text="Edit" OnClick="btnEdit_Click">
                <Icon PrimaryIconCssClass="rbEdit" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                    PrimaryIconWidth="14" />
            </infs:WclButton>
            <infs:WclButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"
                ValidationGroup="grpFormSubmit">
                <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                    PrimaryIconWidth="14" />
            </infs:WclButton>
            <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click">
                <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                    PrimaryIconWidth="14" />
            </infs:WclButton>
        </div>
    </div>
    <%--<infsu:CommandBar ID="fsucCmdBarPackage" runat="server" DefaultPanel="pnlPackage"
                DisplayButtons="Save,Cancel" AutoPostbackButtons="Save,Cancel" OnSaveClick="fsucCmdBarPackage_SaveClick"
                OnCancelClick="fsucCmdBarPackage_CancelClick" ValidationGroup="grpFormSubmit">
            </infsu:CommandBar>--%>
</div>




