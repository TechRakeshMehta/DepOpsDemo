<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageServiceAttribute.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageServiceAttribute" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="../Shared/Controls/IsActiveToggle.ascx" TagName="IsActiveToggle"
    TagPrefix="uc1" %>
<div class="section">
    <h1 class="mhdr">Manage Service Attributes
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenant">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <asp:Label ID="lblTenant" runat="server" Text="Institution" CssClass="cptn"></asp:Label>
                    </div>
                    <div class='sxlm'>
                        <%--<infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" DefaultMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged">
                        </infs:WclDropDownList>--%>
                        <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID"  EmptyMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                            Filter="None" OnClientKeyPressing="openCmbBoxOnTab"> 
                        </infs:WclComboBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdAttributes" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true"
                GridLines="Both" NonExportingColumns="EditCommandColumn,DeleteColumn" OnItemCreated="grdAttributes_ItemCreated"
                OnUpdateCommand="grdAttributes_UpdateCommand" OnItemDataBound="grdAttributes_ItemDataBound"
                OnInsertCommand="grdAttributes_InsertCommand" OnNeedDataSource="grdAttributes_NeedDataSource"
                OnDeleteCommand="grdAttributes_DeleteCommand" OnItemCommand="grdAttributes_ItemCommand"
                
                EnableLinqExpressions="false">
                <ExportSettings ExportOnlyData="false" IgnorePaging="True" OpenInNewWindow="True"
                    HideStructureColumns="false" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm">
                    <Excel AutoFitImages="true" />
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="BSA_ID,BSA_Name">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Attribute"
                        ShowExportToPdfButton="true" ShowExportToExcelButton="true" ShowExportToCsvButton="true"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="BSA_Name" FilterControlAltText="Filter Name column"
                            HeaderText="Attribute Name" SortExpression="BSA_Name" UniqueName="BSA_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BSA_Label" FilterControlAltText="Filter AttributeLabel column"
                            HeaderText="Attribute Label" SortExpression="BSA_Label" UniqueName="BSA_Label">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn DataField="lkpSvcAttributeDataType.SADT_Name" FilterControlAltText="Filter DataType column"
                            HeaderText="Data Type" SortExpression="lkpSvcAttributeDataType.SADT_Name" UniqueName="DataType">

                        </telerik:GridBoundColumn>--%>

                        <telerik:GridTemplateColumn AllowFiltering="true" DataField="lkpSvcAttributeDataType.SADT_Name" FilterControlAltText="Filter DataType column"
                            HeaderText="Data Type" SortExpression="lkpSvcAttributeDataType.SADT_Name" UniqueName="DataType">
                            <ItemTemplate>
                                <asp:LinkButton ID="cascadingDetail" Enabled="false" CommandName="cascadingDetail" runat="server" Text='<%#Eval("lkpSvcAttributeDataType.SADT_Name")%>'></asp:LinkButton>                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridBoundColumn DataField="BSA_Description" FilterControlAltText="Filter Description column"
                            HeaderText="Description" SortExpression="BSA_Description" UniqueName="BSA_Description">
                        </telerik:GridBoundColumn>
                        <%-- <telerik:GridBoundColumn DataField="BSA_IsRequired" FilterControlAltText="Filter Is Required column"
                            HeaderText="Is Required" SortExpression="BSA_IsRequired" UniqueName="BSA_IsRequired">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="BSA_Active" FilterControlAltText="Filter IsActive column"
                            HeaderText="Is Active" SortExpression="BSA_Active" UniqueName="BSA_Active">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn Display="false">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnfIsEditable" runat="server" Value='<%#Eval("BSA_IsEditable")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn DataField="BSA_Active" FilterControlAltText="Filter Is Active column"
                            HeaderText="Is Active" SortExpression="BSA_Active" UniqueName="BSA_Active">
                            <ItemTemplate>
                                <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("BSA_Active"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                <asp:HiddenField ID="hdnfIsEditable" runat="server" Value='<%#Eval("BSA_IsEditable")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Attribute?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section" runat="server" id="divEditBlock" visible="true">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblEHAttr" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Attribute" : "Update Attribute" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlAttr">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Attribute Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtAttrName" Text='<%# Eval("BSA_Name") %>' MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvAttributeName" ControlToValidate="txtAttrName"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Attribute Name is required." ValidationGroup='grpAttribute' />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Attribute Label</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtAttrLabel" Text='<%# Eval("BSA_Label") %>'
                                                        MaxLength="256">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Is Active</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <uc1:IsActiveToggle runat="server" ID="chkActive" IsAutoPostBack="false" IsActiveEnable="true" Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("BSA_Active") %>' />
                                                
                                                    <%--<infs:WclButton runat="server" ID="chkActive" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                        Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("BSA_Active") %>'
                                                        AutoPostBack="false">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                                        </ToggleStates>
                                                    </infs:WclButton>--%>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <infs:WclTextBox runat="server" ID="txtDescription" Text='<%# Eval("BSA_Description") %>'
                                                        MaxLength="1024">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <%--<div class='sxlb'>
                                                    <span class="cptn">Is Required</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclButton runat="server" ID="btnIsReq" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                        Checked='<%# (Container is GridEditFormInsertItem)? false : Eval("BSA_IsRequired") %>'
                                                        AutoPostBack="false" OnClientCheckedChanged="IsRequiredClicked">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                                        </ToggleStates>
                                                    </infs:WclButton>
                                                </div>--%>
                                                <div class='sxlb'>
                                                    <span class="cptn">Data Type</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbDataType" runat="server" AutoPostBack="true" DataTextField="SADT_Name"
                                                        DataValueField="SADT_ID">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvDataType" ControlToValidate="cmbDataType"
                                                            InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpAttribute" CssClass="errmsg"
                                                            Text="Data Type is required." />
                                                    </div>
                                                </div>
                                                <div id="divOption" runat="server" visible="false">
                                                    <div class='sxlb'>
                                                        <span class="cptn">Options</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox runat="server" ID="txtOptOptions" EmptyMessage="E.g. Positive=1|Negative=2"
                                                            Text='<%# Eval("FormatOptions") %>'>
                                                        </infs:WclTextBox>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvOptions" ControlToValidate="txtOptOptions"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Option is required." ValidationGroup='grpAttribute' />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="divCharacters" runat="server" visible="false">
                                                    <div class='sxlb'>
                                                        <span class="cptn">Minimum Characters</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="ntxtTextMinChars"
                                                            MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                                            Text='<%# Eval("BSA_MinLength") %>' MinValue="0">
                                                            <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                                        </infs:WclNumericTextBox>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvTextMinChars" ControlToValidate="ntxtTextMinChars"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Minimum Character is required."
                                                                ValidationGroup='grpAttribute' />
                                                        </div>
                                                    </div>
                                                    <div class='sxlb'>
                                                        <span class="cptn">Maximum Characters</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="ntxtTextMaxChars"
                                                            MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                                            Text='<%# Eval("BSA_MaxLength") %>' MinValue="0">
                                                            <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                                        </infs:WclNumericTextBox>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvMaximumCharacters" ControlToValidate="ntxtTextMaxChars"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Maximum Character is required."
                                                                ValidationGroup='grpAttribute' />
                                                            <asp:CompareValidator ID="cmpvCharacters" Display="Dynamic" ControlToValidate="ntxtTextMinChars"
                                                                ControlToCompare="ntxtTextMaxChars" Type="Double" Operator="LessThanEqual" class="errmsg" ValidationGroup='grpAttribute'
                                                                ErrorMessage="Maximum Characters must be greater than Minimum Characters." runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="divNumeric" runat="server" visible="false">
                                                    <div class='sxlb'>
                                                        <span class="cptn">Minimum Value</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="ntxtMinInt"
                                                            MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                                            Text='<%# Eval("BSA_MinIntValue") %>' MinValue="0">
                                                            <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                                        </infs:WclNumericTextBox>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvMinInt" ControlToValidate="ntxtMinInt"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Minimum Value is required."
                                                                ValidationGroup='grpAttribute' />
                                                        </div>
                                                    </div>
                                                    <div class='sxlb'>
                                                        <span class="cptn">Maximum Value</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="ntxtMaxInt"
                                                            MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                                            Text='<%# Eval("BSA_MaxIntValue") %>' MinValue="0">
                                                            <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                                        </infs:WclNumericTextBox>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvMaxInt" ControlToValidate="ntxtMaxInt"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Maximum Value is required."
                                                                ValidationGroup='grpAttribute' />
                                                            <asp:CompareValidator ID="cmpvNumeric" Display="Dynamic" ControlToValidate="ntxtMinInt"
                                                                ControlToCompare="ntxtMaxInt" Type="Double" Operator="LessThanEqual" class="errmsg" ValidationGroup='grpAttribute'
                                                                ErrorMessage="Maximum Value must be greater than Minimum Value." runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="divDate" runat="server" visible="false">
                                                    <div class='sxlb'>
                                                        <span class="cptn">Minimum Date</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclDatePicker ID="dpMinDate" runat="server" DateInput-EmptyMessage="Select Minimum Date"
                                                            ClientEvents-OnDateSelected="CorrectMinMaxDate" SelectedDate='<%#Eval("BSA_MinDateValue")==DBNull.Value? DateTime.Now:Eval("BSA_MinDateValue") %>'>
                                                        </infs:WclDatePicker>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvMinDate" ControlToValidate="dpMinDate"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Minimum Date is required."
                                                                ValidationGroup='grpAttribute' />
                                                        </div>
                                                    </div>
                                                    <div class='sxlb'>
                                                        <span class="cptn">Maximum Date</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclDatePicker ID="dpMaxDate" runat="server" DateInput-EmptyMessage="Select Maximum Date"
                                                            ClientEvents-OnPopupOpening="SetMinDate" SelectedDate='<%#Eval("BSA_MaxDateValue")==DBNull.Value? DateTime.Now:Eval("BSA_MaxDateValue") %>'>
                                                        </infs:WclDatePicker>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvMaxDate" ControlToValidate="dpMaxDate"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Maximum Date is required."
                                                                ValidationGroup='grpAttribute' />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <%--<div id="divIsRequired" runat="server" style="display: none">
                                                    <div class='sxlb'>
                                                        <span class="cptn">Required Validation Message</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox runat="server" ID="txtReqdMsg" Text='<%# Eval("BSA_ReqValidationMessage") %>' MaxLength="1024">
                                                        </infs:WclTextBox>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvReqdMsg" ControlToValidate="txtReqdMsg"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Validation Message is required." ValidationGroup='grpAttribute' Enabled="false" />
                                                        </div>
                                                    </div>
                                                </div>--%>

                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlAttr"
                                        ValidationGroup="grpAttribute" GridInsertText="Save" GridUpdateText="Save" ExtraButtonIconClass="icnreset" />
                                </div>
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
    </div>
</div>
<script type="text/javascript" language="javascript">
    var winopen = false;
    var minDate = new Date("01/01/1980");
    function CorrectMinMaxDate(picker) {
        var date1 = $jQuery("[id$=dpMinDate]")[0].control.get_selectedDate();
        var date2 = $jQuery("[id$=dpMaxDate]")[0].control.get_selectedDate();
        if (date1 != null && date2 != null) {
            if (date1 > date2)
                $jQuery("[id$=dpMaxDate]")[0].control.set_selectedDate(null);
        }
    }

    function SetMinDate(picker) {
        var date = $jQuery("[id$=dpMinDate]")[0].control.get_selectedDate();
        if (date != null) {
            picker.set_minDate(date);
        }
        else {
            picker.set_minDate(minDate);
        }
    }

    //function IsRequiredClicked(isReq) {
    //    //var isReq = $jQuery("[id$=btnIsReq]");
    //    var isReqDiv = $jQuery("[id$=divIsRequired]");
    //    var rfvReqdMsg = $jQuery("[id$=rfvReqdMsg]");
    //    if (isReq._checked) {
    //        //rfvReqdMsg[0].enabled = true;
    //        ValidatorEnable(rfvReqdMsg[0], true);
    //        rfvReqdMsg.hide();
    //        isReqDiv.show();
    //    }
    //    else {
    //        var txtReqdMsg = $jQuery("[id$=txtReqdMsg]");
    //        txtReqdMsg.val('');
    //        ValidatorEnable(rfvReqdMsg[0], false);
    //        isReqDiv.hide();
    //    }
    //}

    //function EnableIsRequired(isTrue) {
    //    var isReqDiv = $jQuery("[id$=divIsRequired]");
    //    var rfvReqdMsg = $jQuery("[id$=rfvReqdMsg]");
    //    if (isTrue == true) {
    //        ValidatorEnable(rfvReqdMsg[0], true);
    //        rfvReqdMsg.hide();
    //        isReqDiv.show();
    //    }
    //    else {
    //        ValidatorEnable(rfvReqdMsg[0], false);
    //        isReqDiv.hide();
    //    }
    //}

</script>

