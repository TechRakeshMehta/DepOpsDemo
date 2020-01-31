<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewPreviousAddressControl.ascx.cs"
    Inherits="CoreWeb.ComplianceOperations.Views.NewPreviousAddressControl" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="Location" Src="~/CommonControls/UserControl/NewLocationInfo.ascx" %>

<input type="checkbox" runat="server" id="chkShowHideResidenceHistory" class="chkShowHideResidence"
    onchange="HideShowResidence(this)" title="Click here to add residential history."
    value="I have Previous Residence" />
<span id="lblChkShowHide" runat="server" onclick="CheckUncheckResidentialHistory();"></span>
<%--<asp:CheckBox ID="chkShowResidential" AutoPostBack="true" runat="server" Checked="true" />--%>
<infs:WclResourceManagerProxy runat="server" ID="rprxPreviousAddressControl">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/PreviousAddressControl.js"
        ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css"
        ResourceType="StyleSheet" />
</infs:WclResourceManagerProxy>
<div id="divResidentialShowHide" runat="server" style="display: none;">
    <div class="row">
        <infs:WclGrid runat="server" ID="grdResidentialHistory" AutoGenerateColumns="False"
            AllowPaging="false" AllowSorting="false" EnableAriaSupport="true"
            AutoSkinMode="True" CellSpacing="0" GridLines="Both" ShowClearFiltersButton="false"
            OnItemCreated="grdResidentialHistory_ItemCreated" OnItemDataBound="grdResidentialHistory_ItemDataBound"
            OnNeedDataSource="grdResidentialHistory_NeedDataSource" OnItemCommand="grdResidentialHistory_ItemCommand">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ValidationSettings ValidationGroup="grpFormEdit" EnableValidation="true" />
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="ID,TempId" AllowPaging="false"
                PagerStyle-Visible="false" AllowSorting="false" AllowFilteringByColumn="false"
                InsertItemDisplay="Bottom">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Address"
                    ShowRefreshButton="false" />
                <Columns>
                    <telerik:GridDateTimeColumn DataField="ResidenceStartDate" FilterControlAltText="Filter ResidenceStartDate column"
                        HeaderText="Move in Date" SortExpression="ResidenceStartDate" UniqueName="ResidenceStartDate"
                        HeaderStyle-Width="15%"
                        AllowFiltering="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                        DataFormatString="{0:MM/dd/yyyy}">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridDateTimeColumn DataField="ResidenceEndDate" FilterControlAltText="Filter ResidenceEndDate column"
                        HeaderText="Resident Until" SortExpression="ResidenceEndDate" UniqueName="ResidenceEndDate"
                        HeaderStyle-Width="15%"
                        AllowFiltering="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                        DataFormatString="{0:MM/dd/yyyy}">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="PreviousAddress" FilterControlAltText="Filter PreviousAddress column"
                        AllowFiltering="false"
                        HeaderText="Address" SortExpression="PreviousAddress" UniqueName="PreviousAddress"
                        HeaderStyle-Width="50%"
                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Address?"
                        Text="Delete" UniqueName="DeleteColumn" ImageUrl="~/Resources/Mod/Dashboard/images/CancelGrid.gif">
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="~/Resources/Mod/Dashboard/images/editGrid.gif"
                        UniqueName="EditCommandColumn">
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridEditCommandColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div class="container-fluid" runat="server" visible="true" id="divEditFormBlock">
                            <div class="row">
                                <div class="col-md-12">
                                    <h2 class="header-color" tabindex="0">
                                        <asp:Label ID="lblEHPrevAddress" runat="server" />
                                    </h2>
                                </div>
                            </div>
                            <div class="row bgLightGreen">
                                <div class="msgbox">
                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                </div>
                                <asp:Panel runat="server" ID="pnlPrevAddress">
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
                                    <uc:Location ID="locationTenant" ZipTabIndex="6" CityTabIndex="7" runat="server"
                                        NumberOfColumn="Three" IsReverselookupControl="true"
                                        ValidationGroup="grpFormEdit" IsParentEditTemplate="true" ControlsExtensionId="ARH" />
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class='form-group col-md-3'>
                                                <label id="lblMoveInDate_AddNewAdd" class='cptn'>Move in Date</label><span class="reqd">*</span>
                                                <infs:WclDatePicker ID="dpResFrm" runat="server" DateInput-EmptyMessage="Select a date (From)"
                                                    Width="100%" CssClass="form-control" ClientEvents-OnDateSelected="CorrectFrmToPrevResDate"
                                                    DbSelectedDate='<%# Bind( "ResidenceStartDate") %>'>
                                                </infs:WclDatePicker>
                                                <div class="valdx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvResFrm" CssClass="errmsg" ControlToValidate="dpResFrm"
                                                        Display="Dynamic" ErrorMessage="Move in Date is required." ValidationGroup="grpFormEdit" />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <label id="lblResidentUntil_AddNewAdd" class='cptn'>Resident Until</label><span class="reqd">*</span>
                                                <infs:WclDatePicker ID="dpResTill" runat="server" DateInput-EmptyMessage="Select a date (To)"
                                                    Width="100%" CssClass="form-control" ClientEvents-OnPopupOpening="SetMinDatePrevRes"
                                                    DbSelectedDate='<%# Bind( "ResidenceEndDate") %>'>
                                                </infs:WclDatePicker>
                                                <div class="valdx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvResTill" CssClass="errmsg" ControlToValidate="dpResTill"
                                                        Display="Dynamic" ErrorMessage="Resident Until is required." ValidationGroup="grpFormEdit" />
                                                    <asp:CompareValidator ID="cfvResTill" runat="server" CssClass="errmsg" Display="Dynamic"
                                                        ValidationGroup="grpFormEdit"></asp:CompareValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </asp:Panel>
                            </div>
                            <div class="col-md-12 text-right">
                                <infsu:CommandBar ID="fsucCmdBarPrevRes" runat="server" GridMode="true" DefaultPanel="pnlPrevAddress"
                                    ValidationGroup="grpFormEdit" ExtraButtonIconClass="icnreset" GridInsertText="Save"
                                    GridUpdateText="Save" ButtonSkin="Silk" UseAutoSkinMode="false" />
                            </div>
                            <div class="col-md-12">&nbsp;</div>
                        </div>
                    </FormTemplate>
                </EditFormSettings>
            </MasterTableView>
        </infs:WclGrid>
    </div>
</div>
<asp:HiddenField ID="hdnControlToSetFocus" runat="server" Value="" />