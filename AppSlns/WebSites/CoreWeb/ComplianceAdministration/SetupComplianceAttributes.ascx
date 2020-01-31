<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.SetupComplianceAttributes" CodeBehind="SetupComplianceAttributes.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>
<div class="section">
    <h1 class="mhdr">Manage Attributes
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
                            DataValueField="TenantID" EmptyMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
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
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    HideStructureColumns="false" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm">
                    <Excel AutoFitImages="true" />
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <%-- <ExtraCommandButtons>
                    <infs:WclButton ID="WclButton1" Text="Copy Attribute" CommandName="InitInsert" runat="server"
                        CommandArgument="MakeCopy" Icon-PrimaryIconCssClass="icncopy" Visible="false" />
                </ExtraCommandButtons>--%>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="ComplianceAttributeID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Attribute"
                        ShowExportToPdfButton="true" ShowExportToExcelButton="true" ShowExportToCsvButton="true"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter Name column"
                            HeaderText="Attribute Name" SortExpression="Name" UniqueName="Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AttributeLabel" FilterControlAltText="Filter AttributeLabel column"
                            HeaderText="Attribute Label" SortExpression="AttributeLabel" UniqueName="AttributeLabel">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ScreenLabel" FilterControlAltText="Filter ScreenLabel column"
                            HeaderText="Screen Label" SortExpression="ScreenLabel" UniqueName="ScreenLabel">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="lkpComplianceAttributeType.Name" FilterControlAltText="Filter AttributeType column"
                            HeaderText="Attribute Type" SortExpression="lkpComplianceAttributeType.Name"
                            UniqueName="AttributeType">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="lkpComplianceAttributeDatatype.Name" FilterControlAltText="Filter DataType column"
                            HeaderText="Data Type" SortExpression="lkpComplianceAttributeDatatype.Name" UniqueName="DataType">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ComplianceAttributeGroup.CAG_Name" FilterControlAltText="Filter AttributeGroup column"
                            HeaderText="Attribute Group" SortExpression="ComplianceAttributeGroup.CAG_Name"
                            UniqueName="AttributeGroup">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                            HeaderText="Description" SortExpression="Description" UniqueName="Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TenantName" FilterControlAltText="Filter TenantName column"
                            HeaderText="Tenant" SortExpression="TenantName" UniqueName="TenantName">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="IsActive" FilterControlAltText="Filter State column"
                            HeaderText="Is Active" SortExpression="IsActive" UniqueName="IsActive">
                            <ItemTemplate>
                                <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("IsActive"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                <asp:HiddenField ID="hdnfIsCreatedByAdmin" runat="server" Value='<%#Eval("IsCreatedByAdmin")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
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
                                                    <infs:WclTextBox runat="server" ID="txtAttrName" Text='<%# Eval("Name") %>' MaxLength="500">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvAttributeName" ControlToValidate="txtAttrName"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Attribute Name is required." ValidationGroup='grpAttribute' />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Attribute Label</span>
                                                    <%--<span class="reqd">*</span>--%>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtAttrLabel" Text='<%# Eval("AttributeLabel") %>'
                                                        MaxLength="500">
                                                    </infs:WclTextBox>
                                                    <%-- <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvAttributeLabel" ControlToValidate="txtAttrLabel"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Attribute Label is required."
                                                            ValidationGroup='grpAttribute' /></div>--%>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Screen Label</span><%-- <span class="reqd">*</span>--%>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtScreenLabel" Text='<%# Eval("ScreenLabel") %>'
                                                        MaxLength="100">
                                                    </infs:WclTextBox>
                                                    <%-- <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvScreenLabel" ControlToValidate="txtScreenLabel"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Screen Label is required." ValidationGroup='grpAttribute' /></div>--%>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Is Active</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <%--<infs:WclButton runat="server" ID="chkActive" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                        Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("IsActive") %>'
                                                        AutoPostBack="false">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                                        </ToggleStates>
                                                    </infs:WclButton>--%>
                                                    <uc1:IsActiveToggle runat="server" ID="chkActive" IsAutoPostBack="false" IsActiveEnable="true" Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("IsActive") %>' />
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <infs:WclTextBox runat="server" ID="txtDescription" Text='<%# Eval("Description") %>'
                                                        MaxLength="250">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Attribute Type</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbAttrType" runat="server" AutoPostBack="false" DataTextField="Name"
                                                        DataValueField="ComplianceAttributeTypeID">
                                                    </infs:WclComboBox>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Data Type</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbDataType" runat="server" AutoPostBack="true" DataTextField="Name"
                                                        DataValueField="ComplianceAttributeDatatypeID">
                                                    </infs:WclComboBox>
                                                </div>
                                                <div id="divAttributeGroup" runat="server" visible="true">
                                                    <div class='sxlb'>
                                                        <span class="cptn">Attribute Group</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclComboBox ID="cmbAttributeGroup" runat="server" DataTextField="CAG_Name"
                                                            DataValueField="CAG_ID">
                                                        </infs:WclComboBox>
                                                    </div>
                                                </div>


                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
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
                                                        <span class="cptn">Maximum Characters</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="ntxtTextMaxChars"
                                                            MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                                            Text='<%# Eval("MaximumCharacters") %>'>
                                                            <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                                        </infs:WclNumericTextBox>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvMaximumCharacters" ControlToValidate="ntxtTextMaxChars"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Maximum Character is required."
                                                                ValidationGroup='grpAttribute' />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="divDocuments" runat="server" visible="false">
                                                    <div class='sxlb'>
                                                        <span class="cptn">Document</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclComboBox ID="cmbDocument" runat="server" DataTextField="CSD_FileName"
                                                            DataValueField="CSD_ID">
                                                        </infs:WclComboBox>
                                                    </div>
                                                </div>
                                                <div id="dvAdditionalDocuments" runat="server" visible="false">
                                                    <div class='sxlb'>
                                                        <span class="cptn">Additional Document(s)</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <%--   <infs:WclComboBox ID="cmbAdditionalDocument" runat="server" DataTextField="FileName" DataValueField="SystemDocumentID"
                                                            AutoPostBack="false" CheckBoxes="true"  Filter="Contains"
                                                            OnClientKeyPressing="openCmbBoxOnTab"  EmptyMessage="--SELECT--">
                                                            <Localization CheckAllString="All" />
                                                        </infs:WclComboBox>--%>
                                                        <infs:WclComboBox ID="cmbAdditionalDocument" runat="server" AutoPostBack="false" DataTextField="FileName"
                                                            DataValueField="SystemDocumentID" EmptyMessage="--Select--"  EnableCheckAllItemsCheckBox="true"
                                                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" CheckBoxes="true" Localization-CheckAllString="Check All">
                                                        </infs:WclComboBox>

                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Trigger(s) Reconciliation</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <uc1:IsActiveToggle runat="server" ID="chkTriggerRecon" IsActiveEnable="true" IsAutoPostBack="false" Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("IsTriggersReconciliation") %>' />
                                                </div>
                                                <div runat="server" id="divIsSendForIntegration">
                                                    <div class='sxlb'>
                                                        <span class="cptn">Override Date to send for Integration</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <uc1:IsActiveToggle runat="server" ID="ChkSendForIntegration" IsActiveEnable="true" IsAutoPostBack="false" Checked='<%# (Container is GridEditFormInsertItem)? false :Convert.ToBoolean(Eval("IsSendForIntegration")) %>' />
                                                        <%--Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("IsTriggersReconciliation") %>' />--%>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co monly'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Explanatory Notes</span>
                                                </div>
                                                <infs:WclTextBox runat="server" ID="txtExplanatoryNotes" TextMode="MultiLine" Height="50px"
                                                    Text="">
                                                </infs:WclTextBox>
                                                <div class='sxroend'>
                                                </div>
                                            </div>

                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlAttr" GridInsertText="Save" GridUpdateText="Save"
                                        ValidationGroup="grpAttribute" ExtraButtonIconClass="icnreset" />
                                    <%--  <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" DefaultPanel="pnlAttr" ValidationGroup='grpAttribute'>
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
                            <%--    <div class="section" runat="server" id="divCopyBlock" visible="false">
                                <h1 class="mhdr">
                                    <asp:Label ID="Label1" Text='<%# (Container is GridEditFormInsertItem) ? "Copy Attribute" : "Update Attribute" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="Label2" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="Panel1">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    Copy from
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbFrom" runat="server">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Text="Start Date" Value="0" />
                                                            <telerik:RadComboBoxItem Text="Provider's Name" Value="1" />
                                                            <telerik:RadComboBoxItem Text="Effective Date" Value="2" />
                                                        </Items>
                                                    </infs:WclComboBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    New Name
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="WclTextBox1">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxlb'>
                                                    New Label
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="WclTextBox2">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxlb'>
                                                    New Screen Label
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="WclTextBox3">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="CommandBar1" runat="server" DefaultPanel="pnlAttr" GridMode="true">
                                    </infsu:CommandBar>
                                </div>
                            </div>--%>
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
