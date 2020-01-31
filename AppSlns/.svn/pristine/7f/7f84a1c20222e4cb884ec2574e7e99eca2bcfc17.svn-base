<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.SetupComplianceItems" CodeBehind="SetupComplianceItems.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>
<%@ Register TagPrefix="uc2" TagName="CategoriesItemsNodes" Src="~/Shared/Controls/CategoriesItemsNodes.ascx" %>
<%@ Register TagPrefix="uc" TagName="DocumentUrlInfo" Src="~/Shared/Controls/DocumentUrlInfo.ascx" %>
<%--<asp:XmlDataSource ID="xdtsItems" runat="server" DataFile="~/App_Data/DB.xml"
    XPath="//MasterCompliance/MasterItems/*"></asp:XmlDataSource>--%>
<style>
    .breakword {
        word-break: break-all;
    }

    .reEditorModes a {
        display: none;
    }

    .reToolZone {
        display: none;
    }

    .breakword ul {
        margin-left: 10px;
        padding-left: 10px !important;
    }

    .breakword li {
        list-style-position: inside;
        list-style: disc;
    }
</style>
<script type="text/javascript">
    function HideShowPanel(args, isPageReady) {
        var divAmount = $jQuery('[id$=divAmount]');
        var txtAmount = $jQuery('[id$=txtAmount]');
        var rfvAmount = $jQuery("[id$=rfvAmount]");
        var isChecked;

        if (isPageReady != undefined && isPageReady == "true") {
            var chkPaymentType = $jQuery('[id$=chkPaymentType]');
            if (chkPaymentType.length > 0) {
                isChecked = chkPaymentType[0].checked;
            }

        }
        else {
            isChecked = args.checked;
        }
        if (isChecked) {
            divAmount[0].style.display = "block";
            if (rfvAmount[0] != undefined)
                ValidatorEnable(rfvAmount[0], true);
            rfvAmount.hide();
        }
        else {
            txtAmount.val("");
            if (divAmount != undefined && divAmount.length > 0) {
                divAmount[0].style.display = "none";
            }
            if (rfvAmount[0] != undefined)
            ValidatorEnable(rfvAmount[0], false);
        }
    }

    function EnableDisableValidator(isEnabled) {
        var rfvAmount = $jQuery("[id$=rfvAmount]");
        if (isEnabled.toLowerCase() == "false") {
            if (rfvAmount[0] != undefined)
            ValidatorEnable(rfvAmount[0], false);
        }
        else {
            if (rfvAmount[0] != undefined)
            ValidatorEnable(rfvAmount[0], true);
        }
    }
</script>
<div class="section">
    <infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/ContentEditor.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>

    <h1 class="mhdr">Manage Items
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
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlInstitutionHierarchy" Visible="false">
                <div class='sxro sx12co'>
                    <div class="sxlb" style="max-width:15% !important">
                        <asp:Label ID="lblInstitutionHierarchy" runat="server" Text="Institution Hierarchy" CssClass="cptn"></asp:Label>
                    </div>
                    <div class="sxlm" style="max-width:85% !important">
                        <a href="#" id="instituteHierarchy" onclick="openPopUp();">Select Institution Hierarchy</a>&nbsp;&nbsp
                        <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
            AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbRefresh" SubmitButtonText="Reset"
            SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel"
            OnSubmitClick="CmdBarReset_Click" 
            OnSaveClick="CmdBarSearch_Click" 
            OnCancelClick="CmdBarCancel_Click">
        </infsu:CommandBar>
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdItem" AllowPaging="True" Visible="false" OnItemCommand="grdItem_ItemCommand"
                OnItemDataBound="grdItem_ItemDataBound" OnNeedDataSource="grdItem_NeedDataSource"
                OnItemCreated="grdItem_ItemCreated" AutoGenerateColumns="False" AllowSorting="True"
                AllowFilteringByColumn="True" NonExportingColumns="EditCommandColumn,DeleteColumn"
                AutoSkinMode="True" CellSpacing="0" EnableDefaultFeatures="True" ShowAllExportButtons="False"
                EnableLinqExpressions="false" ShowExtraButtons="true" GridLines="Both">
                <GroupingSettings CaseSensitive="false" />
                <%-- <ExtraCommandButtons>
                    <infs:WclButton ID="WclButton1" Text="Copy Item" CommandName="InitCopy" runat="server"
                        CommandArgument="MakeCopy" Icon-PrimaryIconCssClass="icncopy" Visible="false" />
                </ExtraCommandButtons>--%>
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <ClientEvents />
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="ComplianceItemID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Item"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter ItemName column"
                            HeaderText="Item Name" SortExpression="Name" UniqueName="Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemLabel" FilterControlAltText="Filter ItemLabel column"
                            HeaderText="Item Label" SortExpression="ItemLabel" UniqueName="ItemLabel">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ScreenLabel" FilterControlAltText="Filter ScreenLabel column"
                            HeaderText="Screen Label" SortExpression="ScreenLabel" UniqueName="ScreenLabel">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Description" ItemStyle-CssClass="breakword" FilterControlAltText="Filter Description column"
                            HeaderText="Description" SortExpression="Description" UniqueName="Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="HasMoreInfoUrls" ItemStyle-CssClass="bullet" FilterControlAltText="Filter HasMoreInfoUrls column"
                            HeaderText="URL Configured" SortExpression="HasMoreInfoUrls" UniqueName="HasMoreInfoUrls">
                        </telerik:GridBoundColumn>
                        <%--  <telerik:GridBoundColumn DataField="Details" ItemStyle-CssClass="breakword" FilterControlAltText="Filter Details column"
                            HeaderText="Details" SortExpression="Details" UniqueName="Details" HeaderStyle-Width="75px">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="TenantName" FilterControlAltText="Filter TenantName column"
                            HeaderText="Tenant" SortExpression="TenantName" UniqueName="TenantName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IsWithMultipleNodes" FilterControlAltText="Filter TenantName column"
                            HeaderText="Multiple Nodes" SortExpression="IsWithMultipleNodes" UniqueName="IsWithMultipleNodes">
                        </telerik:GridBoundColumn>
                        <%-- <telerik:GridBoundColumn DataField="EffectiveDate" FilterControlAltText="Filter EffectiveDate column"
                            DataFormatString="{0:MM/dd/yyyy}" HeaderText="Effective Date" SortExpression="EffectiveDate"
                            UniqueName="EffectiveDate">
                        </telerik:GridBoundColumn>--%>
                        <%-- <telerik:GridBoundColumn DataField="ExplanatoryNotes" FilterControlAltText="Filter ExplanatoryNotes column"
                            HeaderText="Explanatory Notes" SortExpression="ExplanatoryNotes" UniqueName="ExplanatoryNotes">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="IsActive" FilterControlAltText="Filter IsActive column"
                            HeaderText="Is Active" SortExpression="IsActive" UniqueName="IsActive">
                            <%--<asp:HiddenField ID="hdnfIsCreatedByAdmin" runat="server" Value='<%#Eval("IsCreatedByAdmin")%>' />--%>
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn Display="false">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnfIsCreatedByAdmin" runat="server" Value='<%#Eval("IsCreatedByAdmin")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- <telerik:GridTemplateColumn DataField="IsActive" FilterControlAltText="Filter State column"
                            HeaderText="Is Active" SortExpression="IsActive" UniqueName="IsActive">
                            <ItemTemplate>
                                <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("IsActive"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                <asp:HiddenField ID="hdnfIsCreatedByAdmin" runat="server" Value='<%#Eval("IsCreatedByAdmin")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Item?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <asp:Panel ID="pnl" runat="server" Visible="true">
                            </asp:Panel>
                            <div class="section" visible="true" id="divEditFormBlock" runat="server">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblEHItem" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Item" : "Update Item" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlAttr">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Item Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtName" ClientEvents-OnLoad="SetFocus" ClientIDMode="Static" Text='<%# Eval("Name") %>' MaxLength="100">
                                                    </infs:WclTextBox><div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvItemName" ControlToValidate="txtName"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Item Name is required." />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Item Label</span><%--<span class="reqd">*</span>--%>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtLabel" Text='<%# Eval("ItemLabel") %>' MaxLength="100">
                                                    </infs:WclTextBox>
                                                    <%-- <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvItemLabel" ControlToValidate="txtLabel"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Item Label is required." /></div>--%>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Screen Label</span><%--<span class="reqd">*</span>--%>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtScreenLabel" Text='<%# Eval("ScreenLabel") %>'
                                                        MaxLength="100">
                                                    </infs:WclTextBox>
                                                    <%--<div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvScreenLabel" ControlToValidate="txtScreenLabel"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Screen Label is required." /></div>--%>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Is Active</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <%--<infs:WclButton runat="server" ID="chkActive" Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("IsActive") %>'
                                                        ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                                        </ToggleStates>
                                                    </infs:WclButton>--%>
                                                    <uc1:IsActiveToggle runat="server" ID="chkActive" IsAutoPostBack="false" IsActiveEnable="true" Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("IsActive") %>' />
                                                </div>
                                               <%-- <div class='sxlb'>
                                                    <span class="cptn">Sample Document Form URL</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtSampleDocFormURL" Text='<%# Eval("SampleDocFormURL") %>' MaxLength="2048">
                                                    </infs:WclTextBox>
                                                </div>--%>
                                                <%--<div class='sxlb'>
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtDescription" Text='<%# Eval("Description") %>'
                                                        MaxLength="500">
                                                    </infs:WclTextBox>
                                                </div>--%>
                                                <div class='sxroend'>
                                                </div>
                                                <div id="divEffectiveDateOuter" class='sxro sx3co' runat="server">
                                                    <div id="divEffectiveDate" runat="server">
                                                        <div class='sxlb'>
                                                            <span class="cptn">Effective Date</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <infs:WclDatePicker ID="dpkrEffectiveDate" SelectedDate='<%# (Container is GridEditFormInsertItem)? null : Eval("EffectiveDate") %>'
                                                                runat="server" DateInput-EmptyMessage="Select a date">
                                                                <%--<Calendar>
                                                            <SpecialDays>
                                                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                                            </SpecialDays>
                                                        </Calendar>--%>
                                                            </infs:WclDatePicker>
                                                        </div>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                            </div>
                                           
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Is a Payment Item</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:CheckBox ID="chkPaymentType" runat="server" Text="" onclick="HideShowPanel(this);" />
                                                </div>
                                                <div id="divAmount" runat="server" style="display: none;">
                                                    <div class='sxlb'>
                                                        <span class="cptn">Amount</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclNumericTextBox ID="txtAmount" Type="Currency" NumberFormat-DecimalDigits="2" runat="server" MinValue="0" MaxLength="9">
                                                        </infs:WclNumericTextBox>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvAmount" ControlToValidate="txtAmount"
                                                                class="errmsg" Display="Dynamic" ValidationGroup="grpFormSubmit" ErrorMessage="Amount is required." />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                             <div class='sxro sx3co'>
                                                <uc:DocumentUrlInfo ID="ucDocumentUrlInfo" runat="server" Visible="true" IsEditProfile="true" IsLabelMode="true" IsUcOnComplianceCategoryScreen="false"></uc:DocumentUrlInfo>
                                            </div>
                                            <%--<div class='sxro sx3co'>--%>
                                                <div id="dvMappingHierarchy" runat="server" visible="false" class='sxro sx3co'>
                                                    <div class='sxlb'>
                                                        <span class="cptn">Item Nodes</span>
                                                    </div>
                                                    <uc2:CategoriesItemsNodes runat="server" ID="ucCategoriesItemsNodes" />
                                                </div>
                                            <%--</div>--%>
                                            
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <div class="content" style="height: 160px;">
                                                        <infs:WclEditor ID="rdEditorDescription" ClientIDMode="Static" EditModes="Design" runat="server" Width="99.5%" ToolsFile="~/ComplianceAdministration/Data/Tools.xml" OnClientLoad="OnClientLoad" EnableResize="false"
                                                            Height="130px">
                                                        </infs:WclEditor>
                                                    </div>
                                                    <div class='vldx'>
                                                        <asp:CustomValidator runat="server" ID="cstValEditorDescription" ControlToValidate="rdEditorDescription" ClientValidationFunction="ValidateLength"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please don't enter more than 500 characters." />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>


                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Details</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <div class="content" style="height: 160px;">
                                                        <infs:WclEditor ID="rdEditorDetails" ClientIDMode="Static" EditModes="Design" runat="server" Width="99.5%" ToolsFile="~/ComplianceAdministration/Data/Tools.xml" OnClientLoad="OnClientLoad" EnableResize="false"
                                                            Height="130px">
                                                        </infs:WclEditor>
                                                    </div>
                                                    <div class='vldx'>
                                                        <asp:CustomValidator runat="server" ID="cstValEditorDetails" ControlToValidate="rdEditorDetails" ClientValidationFunction="ValidateDetailsLength"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please don't enter more than 500 characters." />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>


                                            <div class='sxro sx3co '>
                                                <div class='sxlb'>
                                                    <span class="cptn">Explanatory Notes</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <div class="content" style="height: 160px;">
                                                        <infs:WclEditor ID="rdEditorEcplanatoryNotes" Width="99.5%" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml" OnClientLoad="OnClientLoad" EnableResize="false"
                                                            Height="130px">
                                                        </infs:WclEditor>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <%-- <div class='sxro sx3co monly'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Explanatory Notes</span>
                                                </div>
                                            </div>
                                            <div class='sxro sx1co'>
                                                <div class="content" style="height: 130px;">
                                                    <infs:WclEditor ID="rdEditorEcplanatoryNotes" Width="99.5%" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml" OnClientLoad="OnClientLoad"  EnableResize="false"
                                                        Height="85%" >
                                                    </infs:WclEditor>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>--%>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlAttr" GridInsertText="Save" GridUpdateText="Save"
                                        ValidationGroup="grpFormSubmit" ExtraButtonIconClass="icnreset" />
                                    <%-- <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" DefaultPanel="pnlItem">
                                        <ExtraCommandButtons>
                                            <infs:WclButton runat="server" ID="btnApprove" Text="Approve" Visible="false">
                                                <Icon PrimaryIconCssClass="rbOk" />
                                            </infs:WclButton>
                                            <infs:WclButton runat="server" ID="btnReject" Text="Reject" Visible="false">
                                                <Icon PrimaryIconCssClass="rbRemove" />
                                            </infs:WclButton>
                                            <infs:WclButton runat="server" ID="btnSaveForm" ValidationGroup="grpFormSubmit" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
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
                                    <asp:Label ID="Label1" Text='<%# (Container is GridEditFormInsertItem) ? "Copy Item" : "Update Record" %>'
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
                                                            <telerik:RadComboBoxItem Text="MMR1" Value="0" />
                                                            <telerik:RadComboBoxItem Text="MMR2" Value="1" />
                                                            <telerik:RadComboBoxItem Text="Rubella 1" Value="2" />
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

<asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
<asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
<asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
<asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
<asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />
<%--<asp:HiddenField ID="hfTenantId" runat="server" />--%>
<asp:HiddenField ID="hfCurrentUserID" runat="server" />
<asp:HiddenField ID="hdnIsPagePostBack" runat="server" Value="" />

<script type="text/javascript">
    function openPopUp() {
        debugger
        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "CommonScreen";
        //var tenantId = 2;
        var tenantId = $jQuery("[id$=hdnTenantId]").val();
        if (tenantId != "0" && tenantId != "") {
            //UAT-1923
            $jQuery("[id$=hdnIsPagePostBack]").val("Focus Set");
            $jQuery("[id$=instituteHierarchy]").focusout();
            var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName=" + screenName + "&DelemittedDeptPrgMapIds=" + DepartmentProgramId);
            var win = $window.createPopup(url, { size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
            winopen = true;
        }
        else {
            $alert("Please select Institution.");
        }
        return false;
    }

    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
                $jQuery("[id$=hdnDepartmntPrgrmMppng]").val(arg.DepPrgMappingId);
                $jQuery("[id$=hdnHierarchyLabel]").val(arg.HierarchyLabel);
                $jQuery("[id$=hdnInstitutionNodeId]").val(arg.InstitutionNodeId);
                __doPostBack("<%= btnDoPostBack.ClientID %>", "");
            }
            winopen = false;
            //UAT-1923
            setTimeout(function () { $jQuery("[id$=instituteHierarchy]").focus(); }, 100);
        }
    }
</script>
