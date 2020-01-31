<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageCustomForm.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageCustomForm" %>


<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<telerik:RadScriptBlock runat="server" ID="scriptBlock">
        <script type="text/javascript">
            //<![CDATA[
            function onRowDropping(sender, args) {
                if (sender.get_id() == "<%=grdCustomForm.ClientID %>") {
                    var node = args.get_destinationHtmlElement();
                    if (!isChildOf('<%=grdCustomForm.ClientID %>', node)) {
                        args.set_cancel(true);
                    }
                }
                else {
                    var node = args.get_destinationHtmlElement();
                    if (!isChildOf('trashCan', node)) {
                        args.set_cancel(true);
                    }
                    else {
                        if (confirm("Are you sure you want to delete this custom form?"))
                            args.set_destinationHtmlElement($get('trashCan'));
                        else
                            args.set_cancel(true);
                    }
                }
            }

            function isChildOf(parentId, element) {
                while (element) {
                    if (element.id && element.id.indexOf(parentId) > -1) {
                        return true;
                    }
                    element = element.parentNode;
                }
                return false;
            }
            //]]>
        </script>
    </telerik:RadScriptBlock>
 <div class="section">
    <h1 class="mhdr">Manage Custom Forms
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
            
        </div>
        <div class="swrap" runat="server" id="dvCustomForm">
            <infs:WclGrid runat="server" ID="grdCustomForm" AutoGenerateColumns="False"
                AllowSorting="True" AutoSkinMode="True" CellSpacing="0" 
                GridLines="Both" EnableDefaultFeatures="False" ShowAllExportButtons="False" ShowExtraButtons="True"
                OnNeedDataSource="grdCustomForm_NeedDataSource" OnItemCommand="grdCustomForm_ItemCommand"
                OnItemDataBound="grdCustomForm_ItemDataBound"  OnRowDrop="grdCustomForm_RowDrop" OnItemCreated="grdCustomForm_ItemCreated">
                
                <ClientSettings EnableRowHoverStyle="true" AllowAutoScrollOnDragDrop="true" AllowRowsDragDrop="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                     <ClientEvents OnRowDropping="onRowDropping"></ClientEvents>
                    <%--<Scrolling AllowScroll="true" UseStaticHeaders="true"></Scrolling>--%>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="CF_ID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Custom Form"
                        ShowExportToExcelButton="False" ShowExportToPdfButton="False" ShowExportToCsvButton="False"
                        ShowRefreshButton="False" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="CF_Title" FilterControlAltText="Filter Title column"
                            HeaderText="Title" SortExpression="CF_Title" UniqueName="CF_Title">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CF_Name" FilterControlAltText="Filter Name column"
                            HeaderText="Name" SortExpression="CF_Name" UniqueName="CF_Name">
                        </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn DataField="lkpCustomFormType.CFT_Name" FilterControlAltText="Filter Form Type column"
                            HeaderText="Form Type" SortExpression="lkpCustomFormType.CFT_Name" UniqueName="lkpCustomFormType.CFT_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CF_Description" FilterControlAltText="Filter Description column"
                            HeaderText="Description" SortExpression="CF_Description" UniqueName="CF_Description">                            
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="CF_Sequence" FilterControlAltText="Filter Sequence column"
                            HeaderText="Sequence" SortExpression="CF_Sequence" UniqueName="CF_Sequence">
                            <ItemTemplate>
                                <asp:Label ID="lblCFSequence" runat="server" Text='<%#Eval("CF_Sequence")%>'></asp:Label>
                                <asp:HiddenField ID="hdnfIsEditable" runat="server" Value='<%#Eval("CF_IsEditable")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                       
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Configure">
                            <ItemTemplate>
                                <telerik:RadButton ID="lnkBtnConfigure" ButtonType="LinkButton" CommandName="Configure"
                                    ToolTip="Click to Configure Custom Form" runat="server" Text="Configure Form"
                                     BackColor="Transparent" Font-Underline="true" BorderStyle="None">
                                </telerik:RadButton>
                            </ItemTemplate>
                    </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Custom Form?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section" visible="true" id="divEditFormBlock" runat="server">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblEHCustomForm" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Custom Form" : "Update Custom Form" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlCustomForm">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Title</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtCustomFormTitle" Text='<%# Eval("CF_Title") %>'
                                                        MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvCustomFormTitle" ControlToValidate="txtCustomFormTitle"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Custom Form Title is required." />
                                                    </div>
                                                </div><div class='sxlb'>
                                                    <span class="cptn">Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtCustomFormName" Text='<%# Eval("CF_Name") %>'
                                                        MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvCustomFormName" ControlToValidate="txtCustomFormName"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Custom Form Name is required." />
                                                    </div>
                                                </div>
                                                 <div class='sxlb'>
                                                    <span class="cptn">Form Type</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbFormType" runat="server"
                                                        DataTextField="CFT_Name" DataValueField="CFT_ID" EmptyMessage="--Select--">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvServiceType" ControlToValidate="cmbFormType"
                                                            Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Custom Form Type is required." />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <infs:WclTextBox runat="server" ID="txtCustomFormDesc" Text='<%# Eval("CF_Description") %>'
                                                        MaxLength="1024">
                                                    </infs:WclTextBox>
                                                </div>
                                                 <div class='sxroend'>
                                                </div>
                                            </div>                                          
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlCategory"
                                        ValidationGroup="grpFormSubmit" GridInsertText="Save" GridUpdateText="Save" ExtraButtonIconClass="icnreset" />
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