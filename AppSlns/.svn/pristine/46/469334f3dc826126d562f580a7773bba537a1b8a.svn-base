<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PreviousAddressControl.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.PreviousAddressControl" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="Location" Src="~/CommonControls/UserControl/LocationInfo.ascx" %>
<input type="checkbox" runat="server" id="chkShowHideResidenceHistory" class="chkShowHideResidence" onchange="HideShowResidence(this)" title="Click here to add residential history." value="I have Previous Residence" />
<span id="lblChkShowHide" runat="server" onclick="CheckUncheckResidentialHistory();"></span>
<%--<asp:CheckBox ID="chkShowResidential" AutoPostBack="true" runat="server" Checked="true" />--%>
<infs:WclResourceManagerProxy runat="server" ID="rprxPreviousAddressControl">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/PreviousAddressControl.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div id="divResidentialShowHide" runat="server" style="display: none;">
    <div class="swrap">
        <infs:WclGrid runat="server" ID="grdResidentialHistory" AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false"
            AutoSkinMode="True" CellSpacing="0" GridLines="Both" ShowClearFiltersButton="false" OnItemCreated="grdResidentialHistory_ItemCreated"
            OnNeedDataSource="grdResidentialHistory_NeedDataSource" OnItemCommand="grdResidentialHistory_ItemCommand">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ValidationSettings ValidationGroup="grpFormEdit" EnableValidation="true" />
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="ID,TempId" AllowPaging="false" PagerStyle-Visible="false" AllowSorting="false" AllowFilteringByColumn="false"
                InsertItemDisplay="Bottom">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Address" ShowRefreshButton="false" />
                <Columns>
                    <telerik:GridDateTimeColumn DataField="ResidenceStartDate" FilterControlAltText="Filter ResidenceStartDate column"
                        HeaderText="Move in Date" SortExpression="ResidenceStartDate" UniqueName="ResidenceStartDate" HeaderStyle-Width="15%"
                        AllowFiltering="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:MM/dd/yyyy}">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridDateTimeColumn DataField="ResidenceEndDate" FilterControlAltText="Filter ResidenceEndDate column"
                        HeaderText="Resident Until" SortExpression="ResidenceEndDate" UniqueName="ResidenceEndDate" HeaderStyle-Width="15%"
                        AllowFiltering="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:MM/dd/yyyy}">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="PreviousAddress" FilterControlAltText="Filter PreviousAddress column" AllowFiltering="false"
                        HeaderText="Address" SortExpression="PreviousAddress" UniqueName="PreviousAddress" HeaderStyle-Width="50%"
                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                    </telerik:GridBoundColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn" HeaderStyle-Width="5%">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" />
                    </telerik:GridEditCommandColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Address?"
                        Text="Delete" UniqueName="DeleteColumn" HeaderStyle-Width="5%">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div class="section" runat="server" style="padding:5px;" visible="true" id="divEditFormBlock">
                            <h1 class="mhdr">
                                <asp:Label ID="lblEHPrevAddress"
                                    runat="server" /></h1>
                            <div class="content">
                                <div class="sxform auto">
                                    <div class="msgbox">
                                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                    </div>
                                    <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPrevAddress">
                                        <%-- <div class='sxro sx3co'>
                                        <div class='sxlb'>
                                            <span class="cptn">Address 1</span><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm m2spn'>
                                            <asp:HiddenField runat="server" ID="ResHistorySeqOrderID" Value='<%# Eval("ResHistorySeqOrdID") %>' />
                                            <infs:WclTextBox runat="server" ID="txtAddress1" MaxLength="256" Text='<%# Eval("Address1") %>'>
                                            </infs:WclTextBox>
                                            <div class='vldx'>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvAddress1" ControlToValidate="txtAddress1"
                                                    class="errmsg" ValidationGroup="grpFormEdit" Display="Dynamic" ErrorMessage="Address 1 is required." />
                                            </div>
                                        </div>
                                        <div class='sxlb'>
                                            <span class="cptn">Address 2</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox runat="server" ID="txtAddress2" MaxLength="256" Text='<%# Eval("Address2") %>'>
                                            </infs:WclTextBox>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>--%>
                                        <div class='sxro sx3co'>
                                            <uc:Location ID="locationTenant" ZipTabIndex="6" CityTabIndex="7" runat="server" NumberOfColumn="Three" IsReverselookupControl="true"
                                                ValidationGroup="grpFormEdit" IsParentEditTemplate="true" ControlsExtensionId="ARH" />
                                            <div class='sxroend'>
                                            </div>
                                        </div>
                                        <div class='sxro sx3co'>
                                            <div class='sxlb'>
                                                <span class='cptn'>Move in Date</span><span class="reqd">*</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclDatePicker ID="dpResFrm" runat="server" DateInput-EmptyMessage="Select a date (From)"
                                                    ClientEvents-OnDateSelected="CorrectFrmToPrevResDate" DbSelectedDate='<%# Bind( "ResidenceStartDate") %>'>
                                                </infs:WclDatePicker>
                                                <div class="valdx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvResFrm" CssClass="errmsg" ControlToValidate="dpResFrm"
                                                        Display="Dynamic" ErrorMessage="Move in Date is required." ValidationGroup="grpFormEdit" />
                                                </div>
                                            </div>
                                            <div class='sxlb'>
                                                <span class='cptn'>Resident Until</span><span class="reqd">*</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclDatePicker ID="dpResTill" runat="server" DateInput-EmptyMessage="Select a date (To)"
                                                    ClientEvents-OnPopupOpening="SetMinDatePrevRes" DbSelectedDate='<%# Bind( "ResidenceEndDate") %>'>
                                                </infs:WclDatePicker>
                                                <div class="valdx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvResTill" CssClass="errmsg" ControlToValidate="dpResTill"
                                                        Display="Dynamic" ErrorMessage="Resident Until is required." ValidationGroup="grpFormEdit" />
                                                    <asp:CompareValidator ID="cfvResTill" runat="server" CssClass="errmsg" Display="Dynamic" ValidationGroup="grpFormEdit"></asp:CompareValidator>
                                                </div>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </div>

                                        <div class='sxro sx3co' id="divInternationalCriminalSearchAttributes_grd" runat="server">
                                            <div runat="server" id="divMothersName_grd">
                                                <div class='sxlb'>
                                                    <span class='cptn'>Mother's Maiden Name</span><span class="reqd" runat="server" id="spnMotherName_grd" style="display: none">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox ID="txtMotherName_grd" runat="server" MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class="valdx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvMotherName_grd" CssClass="errmsg" ControlToValidate="txtMotherName_grd"
                                                            Enabled="false" Display="Dynamic" ErrorMessage="Mother's Maiden Name is required." ValidationGroup="grpFormEdit" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="divIdentificationNumber_grd" runat="server">
                                                <div class='sxlb'>
                                                    <span class='cptn'>Identification Number</span><span runat="server" id="spnIdentificationNumber_grd" style="display: none" class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox ID="txtIdentificationNumber_grd" runat="server" MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class="valdx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvIdentificationNumber_grd" Enabled="false" CssClass="errmsg" ControlToValidate="txtIdentificationNumber_grd"
                                                            Display="Dynamic" ErrorMessage="Identification is required." ValidationGroup="grpFormEdit" />
                                                    </div>
                                                    <a href="#" style="font-size:11px !important; color: #0B0080 !important;" id="help" onclick="openPopUp();">Help</a>&nbsp;&nbsp  
                                                </div>
                                            </div>
                                            <div id="divCriminalLicenseNumber_grd" runat="server">
                                                <div class='sxlb'>
                                                    <span class='cptn'>Driver License Number</span><span class="reqd" runat="server" id="spnCriminalLicenseNumber_grd" style="display: none">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox ID="txtCriminalLicenseNumber_grd" runat="server" MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class="valdx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvCriminalLicenseNumber_grd" Enabled="false" CssClass="errmsg" ControlToValidate="txtCriminalLicenseNumber_grd"
                                                            Display="Dynamic" ErrorMessage="License Number is required." ValidationGroup="grpFormEdit" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <infsu:CommandBar ID="fsucCmdBarPrevRes" runat="server" GridMode="true" DefaultPanel="pnlPrevAddress"
                                    ValidationGroup="grpFormEdit" ExtraButtonIconClass="icnreset" GridInsertText="Save" GridUpdateText="Save" />

                            </div>
                        </div>
                    </FormTemplate>
                </EditFormSettings>
            </MasterTableView>
        </infs:WclGrid>
    </div>
</div>
<input type="hidden" runat="server" id="hdnShowCriminalAttribute_MotherName" />
<input type="hidden" runat="server" id="hdnShowCriminalAttribute_Identification" />
<input type="hidden" runat="server" id="hdnShowCriminalAttribute_License" />
<div class="gclr">
</div>
