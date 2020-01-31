<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ManageIntitutionNode" CodeBehind="ManageIntitutionNode.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div class="section">
    <h1 class="mhdr">Manage Institution Node</h1>
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
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"> 
                        </infs:WclComboBox>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div id="dvNode" runat="server" class="swrap">
            <infs:WclGrid runat="server" ID="grdNode" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                GridLines="Both" EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true"
                PageSize="10" NonExportingColumns="EditCommandColumn,DeleteColumn" OnNeedDataSource="grdNode_NeedDataSource"
                OnItemCommand="grdNode_ItemCommand" OnInsertCommand="grdNode_InsertCommand" OnUpdateCommand="grdNode_UpdateCommand"
                OnDeleteCommand="grdNode_DeleteCommand" OnItemCreated="grdNode_ItemCreated" OnItemDataBound="grdNode_ItemDataBound">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    HideStructureColumns="true" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm">
                    <Excel AutoFitImages="true" />
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="IN_ID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Node"
                        ShowExportToCsvButton="true" ShowExportToExcelButton="true" ShowExportToPdfButton="true"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="IN_Name" FilterControlAltText="Filter Name column"
                            HeaderText="Name" SortExpression="IN_Name" UniqueName="Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IN_Label" FilterControlAltText="Filter Label column"
                            HeaderText="Label" SortExpression="IN_Label" UniqueName="Label">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IN_Description" FilterControlAltText="Filter Description column"
                            HeaderText="Description" SortExpression="IN_Description" UniqueName="Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="InstitutionNodeType.INT_Name" FilterControlAltText="Filter NodeType column"
                            HeaderText="Node Type" SortExpression="InstitutionNodeType.INT_Name" UniqueName="NodeType">
                        </telerik:GridBoundColumn>
                        <%-- <telerik:GridNumericColumn DataField="IN_Duration" FilterControlAltText="Filter Duration column"
                            HeaderText="Duration (Months)" SortExpression="IN_Duration" DataType="System.Int32"
                            FilterControlWidth="20%" UniqueName="Duration" DecimalDigits="0">
                        </telerik:GridNumericColumn>--%>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" Width="3%" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Node?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" Width="3%" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section" runat="server" id="divEditBlock" visible="true">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblTitleNode" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Node" : "Update Node" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlNode">
                                            <infs:WclTextBox runat="server" Text='<%# Eval("IN_ID") %>' ID="txtNodeId" Visible="false">
                                            </infs:WclTextBox>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox ID="txtName" Width="100%" runat="server" Text='<%# Eval("IN_Name") %>'
                                                        MaxLength="50">
                                                    </infs:WclTextBox>
                                                    <div id="Div1" class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                                                            Display="Dynamic" class="errmsg" ErrorMessage="Name is required." ValidationGroup='grpNode'
                                                            Enabled="true" />
                                                        <%--<asp:RegularExpressionValidator runat="server" ID="revName" ControlToValidate="txtName"
                                                            Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\,\%\@\&\(\)\/\-]$"
                                                            ErrorMessage="Special characters are not allowed." ValidationGroup="grpNode"></asp:RegularExpressionValidator>--%>
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm m2spn '>
                                                    <infs:WclTextBox Width="100%" ID="txtDescription" runat="server" Text='<%# Eval("IN_Description") %>'
                                                        MaxLength="255">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Label</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox ID="txtLabel" Width="100%" runat="server" Text='<%# Eval("IN_Label") %>'
                                                        MaxLength="50">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Node Type</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbNodeType" runat="server" MarkFirstMatch="true" Width="60%"
                                                        DataTextField="INT_Name" DataValueField="INT_ID" Style="z-index: 7002;" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvNodeType" ControlToValidate="cmbNodeType"
                                                            Display="Dynamic" InitialValue="--SELECT--" CssClass="errmsg" ErrorMessage="Node Type is required."
                                                            ValidationGroup="grpNode" />
                                                    </div>
                                                </div>
                                                <div id="dvProgramDuration" runat="server" style="display: none;">
                                                    <div class='sxlb'>
                                                        <span class="cptn">Duration (Months)</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox runat="server" ID="txtProgramDuration" MaxLength="3" Text='<%# (Container is GridEditFormInsertItem) ? null: Eval("IN_Duration") %>'>
                                                        </infs:WclTextBox>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvProgramDuration" ControlToValidate="txtProgramDuration"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Duration is required." ValidationGroup="grpNode" />
                                                        </div>
                                                        <div class='vldx'>
                                                            <asp:RegularExpressionValidator Display="Dynamic" ID="revProgramDuration" runat="server"
                                                                ValidationExpression="^[0-9]*$" class="errmsg" ControlToValidate="txtProgramDuration"
                                                                ValidationGroup="grpNode" ErrorMessage="Only numeric value is allowed.">
                                                            </asp:RegularExpressionValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Custom Attribute</span>
                                                </div>
                                                <div class='sxlm m2spn '>
                                                    <span id="spnEmptyRecord" runat="server" style="display: none;">No attribute to display.</span>
                                                    <asp:Repeater ID="rptrCustomAttribute" runat="server">
                                                        <HeaderTemplate>
                                                            <%-- <table cellspacing="0" width="100%" align="center">
                                                                <tr>
                                                                    <td style="width: 25%;">
                                                                    </td>
                                                                    <td >
                                                                        <span >
                                                                            Required</span>
                                                                    </td>
                                                                </tr>
                                                            </table>--%>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table id="mytable" cellspacing="0" width="100%" align="center">
                                                                <tr>
                                                                    <td style="width: 20%;">
                                                                        <span>
                                                                            <%#Eval("CA_AttributeName")%></span>
                                                                    </td>
                                                                    <td style="width: 10%;">
                                                                        <asp:CheckBox ID="chkBxAttributeName" runat="server" onclick="EnableDisableRequiredField();" />
                                                                        <asp:HiddenField ID="hdnCustomAttributeId" Value='<%#Eval("CA_CustomAttributeID")%>'
                                                                            runat="server" />
                                                                        <asp:HiddenField ID="hdnCstAttrMappedId" Value="0" runat="server" />
                                                                    </td>
                                                                    <td style="width: 10%;">
                                                                        <span>Required</span>
                                                                    </td>
                                                                    <td id="tdRadioAttribute" runat="server">
                                                                        <asp:RadioButton ID="rdbCstAttributeRequiredYes" runat="server" GroupName="grpCustomAttribute"
                                                                            Text="Yes &nbsp" />
                                                                        <asp:RadioButton ID="rdbCstAttributeRequiredNo" runat="server" Checked="true" GroupName="grpCustomAttribute"
                                                                            Text="No" />
                                                                        <%--<asp:RadioButtonList ID="rdblCustomAttribute" runat="server" RepeatDirection="Horizontal">
                                                                            <asp:ListItem Text="Yes &nbsp" Value="True">
                                                                            </asp:ListItem>
                                                                            <asp:ListItem Text="No" Selected="True" Value="False">
                                                                            </asp:ListItem>
                                                                        </asp:RadioButtonList>--%>
                                                                    </td>
                                                                     <td>
                                                                          <span>Editable by Applicant</span>
                                                                     </td>
                                                                    <td>
                                                                          <asp:CheckBox ID="chkEditableByApplicant" runat="server" Checked="true"/>
                                                                     </td>
                                                                   
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarNode" runat="server" GridMode="true" DefaultPanel="pnlNode" GridInsertText="Save" GridUpdateText="Save"
                                        ValidationGroup="grpNode" ExtraButtonIconClass="icnreset" />
                                    <%-- <infsu:CommandBar ID="fsucCmdBarNode" runat="server" DefaultPanel="Node" ValidationGroup='grpNode'>
                                        <ExtraCommandButtons>
                                            <infs:WclButton runat="server" ID="btnSaveForm" ValidationGroup="grpNode" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
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
<script language="javascript" type="text/javascript">

    function OnClientSelectedIndexChanged(sender, args) {
        var dvProgramDuration = $jQuery("[id$=dvProgramDuration]");
        var rfvProgramDuration = $jQuery("[id$=rfvProgramDuration]");
        var revProgramDuration = $jQuery("[id$=revProgramDuration]");

        if (sender.get_value() == '<%= NodeTypeProgramId %>') {
            dvProgramDuration.show();
            ValidatorEnable(rfvProgramDuration[0], true);
            ValidatorEnable(revProgramDuration[0], true);
            rfvProgramDuration.hide();
            revProgramDuration.hide();
        }
        else {
            ValidatorEnable(rfvProgramDuration[0], false);
            ValidatorEnable(revProgramDuration[0], false);
            dvProgramDuration.hide();
        }
    }

    function EnableDisableRequiredField() {
        var checkBoxId = $jQuery("[id$=chkBxAttributeName]");
        var radioButtonYes = $jQuery("[id$=rdbCstAttributeRequiredYes]");
        var radioButtonNo = $jQuery("[id$=rdbCstAttributeRequiredNo]");
         var chkEditableByApplicant = $jQuery("[id$=chkEditableByApplicant]"); //UAT-4997
        for (var i = 0; i < checkBoxId.length; i++) {
            if (checkBoxId[i].checked) {
                radioButtonYes[i].removeAttribute("disabled");
                radioButtonNo[i].removeAttribute("disabled");
                chkEditableByApplicant[i].disabled = false; //UAT-4997
            }
            else {
                radioButtonYes[i].setAttribute("disabled", "true");
                radioButtonNo[i].setAttribute("disabled", "true");
                radioButtonNo[i].checked = true;
                radioButtonYes[i].checked = false;
                chkEditableByApplicant[i].checked = true; //UAT 4829
                chkEditableByApplicant[i].disabled = true; //UAT-4997
                
            }
        }

    }
</script>
