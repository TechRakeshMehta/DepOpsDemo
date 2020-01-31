<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.IntsofSecurityModel.Views.ManageGrade" Codebehind="ManageGrade.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div class="msgbox" id="divSuccessMsg">
    <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
</div>
<div class="section">
    <h1 class="mhdr">
        Manage Grade</h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdGrades" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="True" ShowAllExportButtons="false" ShowExtraButtons="False" GridLines ="Both"
                OnDeleteCommand="grdGrades_DeleteCommand" OnInsertCommand="grdGrades_InsertCommand"
                OnNeedDataSource="grdGrades_NeedDataSource" OnUpdateCommand="grdGrades_UpdateCommand"
                OnItemCreated="grdGrades_ItemCreated" OnItemCommand="grdGrades_ItemCommand" OnItemDataBound="grdGrades_ItemDataBound">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                 Pdf-PageWidth="300mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="GradeLevelID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Grade" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowRefreshButton="true"/>
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="GradeLevelID" FilterControlAltText="Filter Name column"
                            HeaderText="ID" SortExpression="GradeLevelID" UniqueName="GradeID" Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter name column"
                            HeaderText="Grade Name" SortExpression="Description" UniqueName="Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="GradeLevelGroupID" FilterControlAltText="Filter name column"
                            HeaderText="Group" SortExpression="GradeLevelGroupID" UniqueName="GradeLevelGroupID"
                            Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="GradeLevelGroupDescription" FilterControlAltText="Filter Description column"
                            HeaderText="Description" SortExpression="GradeLevelGroupDescription" UniqueName="GradeLevelGroupDescription"
                            Display="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SEQ" FilterControlAltText="Filter name column"
                            HeaderText="SEQ" SortExpression="name" UniqueName="SEQ" Display="false">
                        </telerik:GridBoundColumn>
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
                                    <asp:Label ID="lblGradeEditTitle" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Record" : "Update Record" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <!-- Note: Please donot insert anything here. There should be nothing between content and form divs -->
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblGradeEditMsg" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlName1">
                                            <!-- MD: Added for Grade ID -->
                                            <%--<div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    Program ID<span class="reqd">*</span>
                                                </div>--%>
                                            <infs:WclTextBox runat="server" Text='<%# Eval("GradeLevelID") %>' ID="txtGradeID"
                                                Visible="false">
                                            </infs:WclTextBox>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Grade Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <!--In database name is saved in 'Description' column and description is saved in 'GradeLevelGroupDescription' column-->
                                                    <infs:WclTextBox runat="server" Text='<%# Eval("Description") %>' ID="txtGradeName">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvGradeName" ControlToValidate="txtGradeName"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Grade Name is required." ValidationGroup="grpGrade" />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <infs:WclTextBox runat="server" Text='<%# Eval("GradeLevelGroupDescription") %>'
                                                        ID="txtDescription">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <%--<div class='sxlb'>
                                                    Group<span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbGradeGroup" runat="server" >                                                        
                                                    </infs:WclComboBox>
                                                </div>
                                                <div class='sxlb'>
                                                    Group Display Order<span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclNumericTextBox runat="server" Text = '<%# Eval("SEQ") %>' ID="txtGrpDispOdr">
                                                    </infs:WclNumericTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvGrpDispOdr" ControlToValidate="txtGrpDispOdr"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Grade Name is required."
                                                            ValidationGroup="grpGrade" />
                                                    </div>
                                                </div>--%>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBar1" runat="server" GridMode="true" DefaultPanel="pnlName1" GridInsertText="Save" GridUpdateText="Save"
                                        ValidationGroup="grpGrade" />
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
