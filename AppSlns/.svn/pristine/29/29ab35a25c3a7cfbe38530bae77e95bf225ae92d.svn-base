<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.ManagePrograms" Codebehind="ManagePrograms.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="msgbox" id="divSuccessMsg">
    <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
</div>
<div class="msgbox">
    <asp:Label ID="lblInfoMessage" runat="server" CssClass="info"></asp:Label>
</div>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblManageProgram" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdPrograms" AllowPaging="True" OnNeedDataSource="grdPrograms_NeedDataSource"
                AutoGenerateColumns="False" AllowSorting="True" AllowFilteringByColumn="True" GridLines="Both"
                AutoSkinMode="True" CellSpacing="0" EnableDefaultFeatures="True" ShowAllExportButtons="False"
                NonExportingColumns="EditCommandColumn,DeleteColumn" ShowExtraButtons="False"
                OnInsertCommand="grdPrograms_InsertCommand" OnUpdateCommand="grdPrograms_UpdateCommand"
                OnItemCommand="grdPrograms_ItemCommand" OnDeleteCommand="grdPrograms_DeleteCommand"
                OnItemCreated="grdPrograms_ItemCreated" OnItemDataBound="grdPrograms_ItemDataBound">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                 Pdf-PageWidth="300mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="DPM_ID,AdminProgramStudy.AdminProgramStudyID,AdminProgramStudy.GradeLevelID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Program"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"/>
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="AdminProgramStudy.AdminProgramStudyID" FilterControlAltText="Filter Name column"
                            HeaderText="ID" SortExpression="AdminProgramStudy.AdminProgramStudyID" UniqueName="ProgramID"
                            Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AdminProgramStudy.ProgramStudy" FilterControlAltText="Filter Name column"
                            HeaderText="Program Name" SortExpression="AdminProgramStudy.ProgramStudy" UniqueName="ProgramStudy">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridTemplateColumn  AllowFiltering="true" DataType="System.Decimal">
                        <ItemTemplate>
                        <asp:Label ID="lblManagement" Text='<%# "$"+ Convert.ToString(Eval("AdminProgramStudy.ManagementFee")) %>' runat="Server"></asp:Label>
                        </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridNumericColumn DataField="AdminProgramStudy.ManagementFee" FilterControlAltText="Filter ManagementFee column"
                            HeaderText="Management Fee ($)" SortExpression="AdminProgramStudy.ManagementFee"
                            DataFormatString="{0:###,##0.00}" DecimalDigits="2" DataType="System.Decimal"
                            FilterControlWidth="35%" UniqueName="ManagementFee">
                        </telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="AdminProgramStudy.GradeLevelID" FilterControlAltText="Filter GradeLevelID column"
                            HeaderText="GradeLevelID" SortExpression="AdminProgramStudy.GradeLevelID" UniqueName="GradeLevelID"
                            Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="AdminProgramStudy.DurationMonth" FilterControlAltText="Filter DurationMonth column"
                            HeaderText="Duration (Month)" SortExpression="AdminProgramStudy.DurationMonth"
                            DataType="System.Int32" FilterControlWidth="35%" UniqueName="DurationMonth" DecimalDigits="0">
                        </telerik:GridNumericColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" Width="30" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this record?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" Width="30" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblProgramEdit" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Program" : "Update Program" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <!-- Note: Please donot insert anything here. There should be nothing between content and form divs -->
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblProgramEditMsg" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlName1">
                                            <!-- MD: Added for Program ID -->
                                            <%--<div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    Program ID<span class="reqd">*</span>
                                                </div>--%>
                                            <div class='sxlm'>
                                                <infs:WclTextBox runat="server" Text='<%# (Container is GridEditFormInsertItem) ? null:Eval("AdminProgramStudy.AdminProgramStudyID") %>'
                                                    ID="txtProgramID" Visible="false">
                                                </infs:WclTextBox>
                                                <infs:WclTextBox runat="server" Text='<%# (Container is GridEditFormInsertItem) ? null:Eval("DPM_ID") %>'
                                                    ID="txtDepProgramId" Visible="false">
                                                </infs:WclTextBox>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Program Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    
                                                    <infs:WclTextBox runat="server" Text='<%# (Container is GridEditFormInsertItem) ? null: Eval("AdminProgramStudy.ProgramStudy") %>'
                                                        ID="txtProgramName">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvProgramName" ControlToValidate="txtProgramName"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Program Name is required." ValidationGroup="grpProgramme" />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <infs:WclTextBox runat="server" MaxLength="250" ID="txtDescription" Text='<%# (Container is GridEditFormInsertItem) ? null: Eval("AdminProgramStudy.Description")%>'>
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <%--<div class='sxlb'>
                                                    Renewal Term (Month)<span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtRenewalTerm" Text='<%# (Container is GridEditFormInsertItem) ? null: Eval("AdminProgramStudy.RenewalTerm") %>'>
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvRenewalTerm" ControlToValidate="txtRenewalTerm"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Renewal Term (month) is required."
                                                            ValidationGroup="grpProgramme" />
                                                    </div>
                                                    <div class='vldx'>
                                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revRenewalTerm" runat="server"
                                                            ValidationExpression="^[0-9]*$" class="errmsg" ValidationGroup="grpProgramme"
                                                            ControlToValidate="txtRenewalTerm" ErrorMessage="Only numeric value is allowed.">
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                </div>--%>
                                                <div class='sxlb'>
                                                    <span class="cptn">Management Fee</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclNumericTextBox ShowSpinButtons="True" Type="Currency" Text='<%# (Container is GridEditFormInsertItem) ? null: Eval("AdminProgramStudy.ManagementFee") %>'
                                                        ID="ntxtMgmtFee" runat="server" MinValue="0" InvalidStyleDuration="100" EmptyMessage="Enter a number">
                                                        <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                                                    </infs:WclNumericTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvMgmtFee" ControlToValidate="ntxtMgmtFee"
                                                            class="errmsg" Display="Dynamic" ErrorMessage=" Management Fee is required."
                                                            ValidationGroup="grpProgramme" />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Duration (month)</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtDuration" Text='<%# (Container is GridEditFormInsertItem) ? null: Eval("AdminProgramStudy.DurationMonth") %>'>
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvDuration" ControlToValidate="txtDuration"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Duration is required." ValidationGroup="grpProgramme" />
                                                    </div>
                                                    <div class='vldx'>
                                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revDuration" runat="server"
                                                            ValidationExpression="^[0-9]*$" class="errmsg" ValidationGroup="grpProgramme"
                                                            ControlToValidate="txtDuration" ErrorMessage="Only numeric value is allowed.">
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Grade</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbGrades" runat="server">
                                                    </infs:WclComboBox>
                                                    <div class='vldx'>
                                                        <%--<asp:RequiredFieldValidator runat="server" ID="rfvCmbGrades" ControlToValidate="cmbGrades"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Grade is required."
                                                            ValidationGroup="grpProgramme" />--%>
                                                    </div>
                                                    <%--CheckBoxes="true" EnableCheckAllItemsCheckBox="true" --%>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Payment Option</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <asp:CheckBoxList ID="chkPaymentOption" RepeatDirection="Horizontal" runat="server"
                                                        DataTextField="Name" DataValueField="PaymentOptionID">
                                                    </asp:CheckBoxList>
                                                    <div class='vldx'>
                                                        <asp:CustomValidator ID="cvPaymentOption" CssClass="errmsg" Display="Dynamic" runat="server"
                                                            EnableClientScript="true" ErrorMessage="Payment Option is required." ValidationGroup="grpProgramme"
                                                            ClientValidationFunction="ValidatePaymentOption">
                                                        </asp:CustomValidator>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBar1" runat="server" GridMode="true" DefaultPanel="pnlName1" GridInsertText="Save" GridUpdateText="Save"
                                        ValidationGroup="grpProgramme" />
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
    <script type="text/javascript" language="javascript">
        // Function to validate Payment options

        function ValidatePaymentOption(sender, args) {
            var options = $jQuery("[id$=chkPaymentOption] input:checked");
            for (var i = 0; i < options.length; i++) {
                if (options[i].checked) {
                    args.IsValid = true;
                    return false;
                }
            }
            args.IsValid = false;
        }
        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }
    </script>
</div>
