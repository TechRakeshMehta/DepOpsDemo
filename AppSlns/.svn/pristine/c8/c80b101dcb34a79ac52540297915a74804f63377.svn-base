<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ManageCustomAttributes" CodeBehind="ManageCustomAttributes.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>
<div class="section">
    <h1 class="mhdr">Manage Custom Attributes
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
                        <span class='cptn'>
                            <asp:Label ID="lblTenant" runat="server" Text="Institution"></asp:Label></span>
                    </div>
                    <div class='sxlm'>
                        <%-- <infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" DefaultMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged">
                        </infs:WclDropDownList>--%>
                        <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" EmptyMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div id="dvNode" runat="server" class="swrap">
            <infs:WclGrid runat="server" ID="grdCustomAttributes" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true"
                GridLines="Both" NonExportingColumns="EditCommandColumn,DeleteColumn" OnNeedDataSource="grdCustomAttributes_NeedDataSource"
                OnItemCreated="grdCustomAttributes_ItemCreated" OnItemCommand="grdCustomAttributes_ItemCommand" OnItemDataBound="grdCustomAttributes_ItemDataBound"
                OnInsertCommand="grdCustomAttributes_InsertCommand" OnUpdateCommand="grdCustomAttributes_UpdateCommand"
                OnDeleteCommand="grdCustomAttributes_DeleteCommand" EnableLinqExpressions="false">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    HideStructureColumns="false" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm">
                    <Excel AutoFitImages="true" />
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="CA_CustomAttributeID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Custom Attribute"
                        ShowExportToPdfButton="true" ShowExportToExcelButton="true" ShowExportToCsvButton="true"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="CA_AttributeName" FilterControlAltText="Filter Name column"
                            HeaderText="Attribute Name" SortExpression="CA_AttributeName" UniqueName="CA_AttributeName" AllowFiltering="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CA_AttributeLabel" FilterControlAltText="Filter AttributeLabel column"
                            HeaderText="Attribute Label" SortExpression="CA_AttributeLabel" UniqueName="AttributeLabel">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="lkpCustomAttributeDataType.Name" FilterControlAltText="Filter DataType column"
                            HeaderText="Data Type" SortExpression="lkpCustomAttributeDataType.Name" UniqueName="DataType">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="lkpCustomAttributeUseType.Name" FilterControlAltText="Filter UseType column"
                            HeaderText="Use Type" SortExpression="lkpCustomAttributeUseType.Name" UniqueName="UseType">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CA_Description" FilterControlAltText="Filter Description column"
                            HeaderText="Description" SortExpression="CA_Description" UniqueName="Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CA_IsActive" FilterControlAltText="Filter IsActive column"
                            HeaderText="Is Active" SortExpression="CA_IsActive" UniqueName="IsActive">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridTemplateColumn DataField="CA_IsActive" FilterControlAltText="Filter State column"
                            HeaderText="Is Active" SortExpression="CA_IsActive" UniqueName="IsActive">
                            <ItemTemplate>
                                <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("CA_IsActive"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
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
                                    <asp:Label ID="lblEHAttr" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Custom Attribute" : "Update Custom Attribute" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlAttr">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class='cptn'>Attribute Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtAttrName" Text='<%# Eval("CA_AttributeName") %>'
                                                        MaxLength="100">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvAttributeName" ControlToValidate="txtAttrName"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Attribute Name is required." ValidationGroup='grpAttribute' />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class='cptn'>Attribute Label</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtAttrLabel" Text='<%# Eval("CA_AttributeLabel") %>'
                                                        MaxLength="100">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div id="divIsDisplayInSearchFilter" runat="server">
                                                    <div class='sxlb'>
                                                        <span class='cptn'>Include in screen filters</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <uc1:IsActiveToggle runat="server" ID="chkDisplayInSearchFilter" IsActiveEnable="true" Checked='<%#  Eval("CA_DisplayInSearchFilter") !=null &&  Eval("CA_DisplayInSearchFilter").Equals(false) ? false : true %>' />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class='cptn'>Is Active</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <%-- <infs:WclButton runat="server" ID="chkActive" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                        Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("CA_IsActive") %>'
                                                        AutoPostBack="false">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                                        </ToggleStates>
                                                    </infs:WclButton>--%>
                                                    <uc1:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="true" Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("CA_IsActive") %>' />
                                                </div>
                                                <div class='sxlb'>
                                                    <span class='cptn'>Description</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <infs:WclTextBox runat="server" ID="txtDescription" Text='<%# Eval("CA_Description") %>'
                                                        MaxLength="250">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class='cptn'>Data Type</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbDataType" runat="server" AutoPostBack="true" DataTextField="Name"
                                                        DataValueField="CustomAttributeDataTypeID">
                                                    </infs:WclComboBox>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class='cptn'>Use Type</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbUseType" runat="server" AutoPostBack="true" DataTextField="Name"
                                                        DataValueField="CustomAttributeUseTypeID">
                                                    </infs:WclComboBox>
                                                </div>
                                                <div id="dvIsRequired" runat="server" visible="false">
                                                    <div class='sxlb'>
                                                        <span class="cptn">Is Required</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <asp:RadioButtonList ID="rblIsRequired" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Yes " Value="True" Selected="True" />
                                                            <asp:ListItem Text="No" Value="False" />
                                                        </asp:RadioButtonList>
                                                    </div>
                                                </div>
                                                <div id="dvRelatedAttribute" runat="server">
                                                    <div class='sxlb'>
                                                        <span class='cptn'>Related Profile Attribute</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclComboBox ID="cmbRelatedAttribute" runat="server" DataTextField="CA_AttributeName"
                                                            DataValueField="CA_CustomAttributeID">
                                                        </infs:WclComboBox>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co' id="dvMaxCharRegExpression" runat="server" visible="false">
                                                <div id="divCharacters" runat="server" visible="false">
                                                    <div class='sxlb'>
                                                        <span class='cptn'>Maximum Characters</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="ntxtTextMaxChars" MaxLength="9"
                                                            runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number" Text='<%# Eval("CA_StringLength") %>'>
                                                            <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                                        </infs:WclNumericTextBox>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvMaximumCharacters" ControlToValidate="ntxtTextMaxChars"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Maximum Character is required."
                                                                ValidationGroup='grpAttribute' />
                                                        </div>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                                <div id="dvValidateRegExp" runat="server" visible="false">
                                                    <div class='sxlb'>
                                                        <span class='cptn'>Regular Expression</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox runat="server" ID="txtRegularExp" Text='<%# Eval("CA_RegularExpression") %>'
                                                            MaxLength="1024">
                                                        </infs:WclTextBox>
                                                    </div>
                                                    <div class='sxlb'>
                                                        <span class='cptn'>Error Message</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox runat="server" ID="txtRegExpErrorMsg" Text='<%# Eval("CA_RegExpErrorMsg") %>'
                                                            MaxLength="1024">
                                                        </infs:WclTextBox>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvRegExpErrMsg" ControlToValidate="txtRegExpErrorMsg"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Error message is required." Enabled="false"
                                                                ValidationGroup='grpAttribute' />
                                                        </div>
                                                    </div>
                                                     <div class='sxroend'>
                                                </div>
                                                </div>
                                               
                                            </div>
                                            <div class='sxro sx3co' id="dvValidater" runat="server" visible="false">
                                                <div>
                                                    <div class='sxlb'>
                                                        <span class='cptn'>Input Text to Validate</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox runat="server" ID="txtValString" MaxLength="1024">
                                                        </infs:WclTextBox>
                                                    </div>

                                                    <div class="sxlm m2spn">
                                                        <infs:WclButton runat="server" ID="btnValidateRegExp" Text="Validate" OnClick="btnValidateRegExp_Click" AutoPostBack="true"></infs:WclButton>
                                                        <asp:Label runat="server" ID="lblValidStatus"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>

                                            <div class='sxro sx3co' id="dvPendingCmpProfile" runat="server" visible="false">
                                                <div class='sxlb'>
                                                    <span class='cptn'>Show in Pending Compliance Profiles Grid</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <uc1:IsActiveToggle runat="server" ID="chkShowPendingCmpProfile" IsAutoPostBack="false" IsActiveEnable="true" Checked='<%# (Container is GridEditFormInsertItem)? false : Eval("CA_ShowInPendingComProfilesGrid") %>' />
                                                </div>
                                                <div class="sxlb">
                                                    <span class='cptn'>Include In Notification</span>
                                                </div>
                                                <div class="sxlm">
                                                    <uc1:IsActiveToggle runat="server" ID="chkIncludeInNotification" IsAutoPostBack="false" IsActiveEnable="true" Checked='<%# (Container is GridEditFormInsertItem)? false : Eval("CA_IncludeInNotification") %>' />
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>

                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlAttr" GridInsertText="Save" GridUpdateText="Save" OnSaveClientClick="Validate"
                                        ValidationGroup="grpAttribute" ExtraButtonIconClass="icnreset" />
                                    <%-- <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" DefaultPanel="pnlAttr" ValidationGroup='grpAttribute'>
                                        <ExtraCommandButtons>
                                            <infs:WclButton runat="server" ID="btnApprove" Text="Approve" Visible="false">
                                                <Icon PrimaryIconCssClass="rbOk" />
                                            </infs:WclButton>
                                            <infs:WclButton runat="server" ID="btnReject" Text="Reject" Visible="false">
                                                <Icon PrimaryIconCssClass="rbRemove" />
                                            </infs:WclButton>
                                            <infs:WclButton runat="server" ID="btnSaveForm" ValidationGroup="grpAttribute" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                                CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'>
                                                <Icon PrimaryIconCssClass="rbSave" />
                                            </infs:WclButton>
                                            <infs:WclButton runat="server" ID="btnCancelForm" Text="Cancel" CommandName="Cancel">
                                                <Icon PrimaryIconCssClass="rbCancel" />
                                            </infs:WclButton>
                                        </ExtraCommandButtons>
                                    </infsu:CommandBar>--%>
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
<script type="text/javascript">
    function Validate(sender, eventargs) {
        //debugger;
        var txtRegExp = $jQuery("[id$=txtRegularExp]")[0];
        var txtRegExpErrMsg = $jQuery("[id$=txtRegExpErrorMsg]")[0];
        if (txtRegExp != undefined && txtRegExpErrMsg != undefined) {
            if (txtRegExpErrMsg.control.get_value() != "" && txtRegExp.control.get_value() == "") {
                //enable required field validator - custom msg
                EnableValidator()
                var rf_validator = $jQuery("[id$=rfvRegExpErrMsg]")[0];
                rf_validator.innerHTML = "Please enter Regular Expression."
                sender.set_autoPostBack(false);
            }
            else {
                if (txtRegExp.control.get_value() == "" || txtRegExpErrMsg.control.get_value() != "") {
                    //disable required field validator
                    DisableValidator()
                    sender.set_autoPostBack(true);
                }
                else {
                    //enable required field validator
                    EnableValidator()
                    sender.set_autoPostBack(false);
                }
            }
        }
    }

    //Function for enable validators
    function EnableValidator() {
        var rfvalidator = $jQuery("[id$=rfvRegExpErrMsg]")[0];
        if (rfvalidator != undefined) {
            ValidatorEnable(rfvalidator, true);
            rfvalidator.innerHTML = "Error message is required";
            if (rfvalidator.style.visibility == "visible") {
                rfvalidator.style.visibility = "hidden";
            }
            else {
                rfvalidator.style.display = "block";
            }
        }
    }

    //function for disable validator
    function DisableValidator() {
        var rfvalidator = $jQuery("[id$=rfvRegExpErrMsg]")[0];
        if (rfvalidator != undefined) {
            ValidatorEnable(rfvalidator, false);
        }
    }

</script>
