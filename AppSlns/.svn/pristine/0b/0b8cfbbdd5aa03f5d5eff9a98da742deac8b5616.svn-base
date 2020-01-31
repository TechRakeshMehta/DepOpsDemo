<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigureCustomForm.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ConfigureCustomForm" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<telerik:RadScriptBlock runat="server" ID="scriptBlock">
    <script type="text/javascript">
        //<![CDATA[
        function onRowDropping(sender, args) {
            if (sender.get_id() == "<%=grdCustomFormConfig.ClientID %>") {
                var node = args.get_destinationHtmlElement();
                if (!isChildOf('<%=grdCustomFormConfig.ClientID %>', node)) {
                        args.set_cancel(true);
                    }
                }
                else {
                    var node = args.get_destinationHtmlElement();
                    if (!isChildOf('trashCan', node)) {
                        args.set_cancel(true);
                    }
                    else {
                        if (confirm("Are you sure you want to delete this record?"))
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
    <h1 class="mhdr">Configure Custom Form
    </h1>
    <div class="content">

        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlCustomFormInfo" Enabled="false">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Title</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtCustomFormTitle" Text=""
                            MaxLength="256">
                        </infs:WclTextBox>

                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtCustomFormName" Text=""
                            MaxLength="256">
                        </infs:WclTextBox>
                    </div>

                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class="swrap" runat="server" id="dvCustomFormConfig">
            <infs:WclGrid runat="server" ID="grdCustomFormConfig" AutoGenerateColumns="False"
                AllowSorting="True" AutoSkinMode="True" CellSpacing="0"
                GridLines="Both" EnableDefaultFeatures="False" ShowExtraButtons="True"
                OnNeedDataSource="grdCustomFormConfig_NeedDataSource" OnItemCommand="grdCustomFormConfig_ItemCommand"
                OnItemDataBound="grdCustomFormConfig_ItemDataBound" OnRowDrop="grdCustomFormConfig_RowDrop">
                <ClientSettings EnableRowHoverStyle="true" AllowAutoScrollOnDragDrop="true" AllowRowsDragDrop="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                    <ClientEvents OnRowDropping="onRowDropping"></ClientEvents>
                    <%--<Scrolling AllowScroll="true" UseStaticHeaders="true"></Scrolling>--%>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="CFAG_ID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Attribute Group Configuration"
                        ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"
                        ShowRefreshButton="false" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="Attribute Group" UniqueName="AttributeGroup">
                            <ItemTemplate>
                                <asp:Label ID="lblAttributeGroup" runat="server"></asp:Label>
                                <asp:HiddenField ID="hdnfAttrGrpId" runat="server" Value='<%#Eval("CFAG_BkgSvcAttributeGroupId")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="CFAG_SectionTitle" FilterControlAltText="Filter Section Title column"
                            HeaderText="Section Title" SortExpression="CFAG_SectionTitle" UniqueName="CFAG_SectionTitle">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="Display Column" FilterControlAltText="Filter Display Column column"
                            SortExpression="CFAG_DisplayColumn" UniqueName="CFAG_DisplayColumn">
                            <ItemTemplate>
                                <asp:Label ID="lblDisplayColumn" runat="server"></asp:Label>
                                <asp:HiddenField ID="hdnfDisplayColumn" runat="server" Value='<%#Eval("CFAG_DisplayColumn")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridBoundColumn DataField="CFAG_DisplayColumn" FilterControlAltText="Filter Display Column column"
                            HeaderText="Display Column" SortExpression="CFAG_DisplayColumn" UniqueName="CFAG_DisplayColumn">
                        </telerik:GridBoundColumn> --%>
                        <telerik:GridTemplateColumn AllowFiltering="true" FilterControlAltText="Filter Occurrence column"
                            HeaderText="Occurrence" SortExpression="CFAG_Occurrence" UniqueName="CFAG_Occurrence">
                            <ItemTemplate>
                                <asp:Label ID="lblOccurrence" runat="server"></asp:Label>
                                <asp:HiddenField ID="hdnfOccurrence" runat="server" Value='<%#Eval("CFAG_Occurrence")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- <telerik:GridBoundColumn DataField="CFAG_Occurrence" FilterControlAltText="Filter Occurrence column"
                            HeaderText="Occurrence" SortExpression="CFAG_Occurrence" UniqueName="CFAG_Occurrence">
                        </telerik:GridBoundColumn>--%>
                        <%-- <telerik:GridBoundColumn DataField="CFAG_CustomHTML" FilterControlAltText="Filter Custom HTML column"
                            HeaderText="Custom HTML" SortExpression="CFAG_CustomHTML" UniqueName="CFAG_CustomHTML">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridTemplateColumn DataField="CFAG_Sequence" FilterControlAltText="Filter Sequence column"
                            HeaderText="Sequence" SortExpression="CFAG_Sequence" UniqueName="CFAG_Sequence">
                            <ItemTemplate>
                                <asp:Label ID="lblCFSequence" runat="server" Text='<%#Eval("CFAG_Sequence")%>'></asp:Label>
                                <%-- <asp:HiddenField ID="hdnfCustomHTML" runat="server" Value='<%#Eval("CFAG_CustomHTML")%>' />--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Attribute Group Configuration?"
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
                                    <asp:Label ID="lblEHCustomFormConfig" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Attribute Group Configuration" : "Update Attribute Group Configuration" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlCustomFormConfig">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Choose Attribute Group</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <%-- <infs:WclDropDownList ID="ddlAttrGroup" DataTextField="BSAD_Name" DataValueField="BSAD_ID" runat="server"></infs:WclDropDownList>--%>

                                                    <infs:WclComboBox ID="ddlAttrGroup" DataTextField="BSAD_Name" DataValueField="BSAD_ID" runat="server"></infs:WclComboBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvCustomFormConfigAttrGroup" ControlToValidate="ddlAttrGroup" InitialValue="--Select--"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Attribute Group is required." />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Section Title</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtCustomFormConfigTitle" Text='<%# Eval("CFAG_SectionTitle") %>'
                                                        MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvCustomFormConfigTitle" ControlToValidate="txtCustomFormConfigTitle"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Section Title is required." />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Display Column</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:RadioButtonList ID="rBtnListDisplayColumn" runat="server" RepeatDirection="Horizontal">
                                                    </asp:RadioButtonList>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvCustomFormConfigDisplayColumn" ControlToValidate="rBtnListDisplayColumn"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Display Column is required." />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Multiple Occurrence</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:CheckBox ID="chkOccurence" runat="server" Checked="false" />
                                                </div>
                                                <%--<div class='sxlb'>
                                                    <span class="cptn">Custom HTML</span>
                                                </div>
                                                <div class='sxlm'>
                                                   <telerik:RadButton ID="lnkBtnCustomHTML" ButtonType="LinkButton" CommandName="CustomHTML"
                                                        ToolTip="Click to Add Custom HTML" runat="server" Text="Custom HTML"
                                                        BackColor="Transparent" Font-Underline="true" BorderStyle="None"/>                                                     
                                                </div> --%>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx1co'>

                                                <div class='sxlb '>
                                                    <span class="cptn">Custom HTML</span>
                                                </div>

                                                <%--<h1 id="hdrContent" runat="server" class="mhdr">Custom HTML
                                                </h1>--%>
                                                <div class="content" id="divRadContent" runat="server" style="width: 1200px; height: 220px;">
                                                    <infs:WclEditor ID="radHTMLEditor" ToolsFile="~/WebSite/Data/Tools.xml" runat="server" Content='<%# Eval("CFAG_CustomHTML") %>'
                                                        Width="99%" Height="99%" OnClientLoad="OnClientLoad" EnableResize="false">
                                                        <ImageManager ViewPaths="~/InstitutionImages" UploadPaths="~/InstitutionImages" DeletePaths="~/InstitutionImages" MaxUploadFileSize="7100000" />
                                                    </infs:WclEditor>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>

                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlCustomFormConfig"
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

<div style="width: 100%; text-align: center" id="dvShowBackLink" runat="server">
    <infs:WclButton runat="server" ID="btnGoBack" Text="Go Back To Custom Forms" OnClick="CmdBarCancel_Click">
    </infs:WclButton>
</div>
<script type="text/javascript" language="javascript">
    function OnClientLoad(editor, args) {
        $jQuery('ul.reToolbar').width('auto');
    }
</script>
