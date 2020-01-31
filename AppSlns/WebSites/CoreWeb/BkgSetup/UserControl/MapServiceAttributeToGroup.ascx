<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MapServiceAttributeToGroup.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.MapServiceAttributeToGroup" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<telerik:RadScriptBlock runat="server" ID="scriptBlock">
    <script type="text/javascript">
        //<![CDATA[
        function onRowDropping(sender, args) {
            if (sender.get_id() == "<%=grdMapSvcAttribute.ClientID %>") {
                var node = args.get_destinationHtmlElement();
                if (!isChildOf('<%=grdMapSvcAttribute.ClientID %>', node)) {
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
    <style type="text/css">
        .chkbutton {
            width:7% !important;
        }

    </style>
</telerik:RadScriptBlock>


<div class="section">
    <h1 class="mhdr">Attribute Group: 
        <asp:Label ID="lblAttributeGroupName" runat="server"></asp:Label></h1>
    <div class="content">
        <infs:WclGrid runat="server" ID="grdMapSvcAttribute"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" AutoGenerateColumns="False"
            AllowFilteringByColumn="false" AllowSorting="True" OnNeedDataSource="grdMapSvcAttribute_NeedDataSource"
            OnItemCommand="grdMapSvcAttribute_ItemCommand" OnItemCreated="grdMapSvcAttribute_ItemCreated" 
            OnItemDataBound="grdMapSvcAttribute_ItemDataBound" OnRowDrop="grdMapSvcAttribute_RowDrop" EnableDefaultFeatures="false">
            <ClientSettings EnableRowHoverStyle="true" AllowRowsDragDrop="true" AllowAutoScrollOnDragDrop="true">
                <Selecting AllowRowSelect="true"></Selecting>
                <%--<ClientEvents OnRowDropping="onRowDropping" />--%>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="AttributeGroupMappingID,AttributeID,AttributeGroupID,AttributeDataTypeCode,SourceAttributeID" AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Mapping" ShowRefreshButton="false"></CommandItemSettings>
                <Columns>
                    <telerik:GridBoundColumn DataField="AttributeName" HeaderText="Attribute"
                        SortExpression="AttributeName" UniqueName="AttributeName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="DisplaySequence" HeaderText="Display Sequence"
                        SortExpression="DisplaySequence" UniqueName="DisplaySequence">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="IsRequired" FilterControlAltText="Filter IsRequired column"
                        HeaderText="Is Required" SortExpression="IsRequired" UniqueName="IsRequired">
                        <ItemTemplate>
                            <asp:Label ID="IsRequired" runat="server" Text='<%# Convert.ToBoolean(Eval("IsRequired"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                            <asp:HiddenField ID="hdnfIsEditable" runat="server" Value='<%#Eval("IsEditable")%>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="IsDisplay" FilterControlAltText="Filter IsDisplay column"
                        HeaderText="Is Display" SortExpression="IsDisplay" UniqueName="IsDisplay">
                        <ItemTemplate>
                            <asp:Label ID="IsDisplay" runat="server" Text='<%# Convert.ToBoolean(Eval("IsDisplay"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn DataField="IsHdnFrmUI" FilterControlAltText="Filter IsHdnFrmUI column"
                        HeaderText="Is Hidden From UI" SortExpression="IsHdnFrmUI" UniqueName="IsHdnFrmUI">
                        <ItemTemplate>
                            <asp:Label ID="IsHdnFrmUI" runat="server" Text='<%# Convert.ToBoolean(Eval("IsHiddenFromUI"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn" Display="true">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" />
                    </telerik:GridEditCommandColumn>
                    <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Attribute Mapping?"
                        Text="Delete Mapping" UniqueName="DeleteColumn">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div class="section" visible="true" id="divEditFormBlock" runat="server" >
                            <h1 class="mhdr">
                                <asp:Label ID="lblEHServiceGroup" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Mapping" : "Update Mapping" %>'
                                    runat="server" /></h1>
                            <div class="content">
                                <div class="sxform auto">
                                    <div class="msgbox">
                                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                    </div>
                                    <asp:Panel runat="server" CssClass="sxpnl" ID="pnlMapAttributes">
                                        <div class='sxro sx3co'>
                                            <div id ="divAttribute" runat="server" visible="false">
                                                <div class='sxlb'>
                                                    <span class="cptn">Attribute</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="ddlAttributes" runat="server" CheckBoxes="true"
                                                        DataTextField="BSA_Name" DataValueField="BSA_ID" EmptyMessage="--Select--">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvAttributes" ControlToValidate="ddlAttributes" Enabled="false"
                                                            Display="Dynamic" CssClass="errmsg" Text="Attribute is required." ValidationGroup="grpFormSubmit" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='sxlb'>
                                                <span class="cptn">Is Required</span>
                                            </div>
                                            <div class='sxlm chkbutton'>
                                                <infs:WclButton runat="server" ID="chkIsRequired" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                    AutoPostBack="false" Checked='<%# (Container is GridEditFormInsertItem)? false : (Eval("IsRequired")==null)?false:Eval("IsRequired") %>'>
                                                    <ToggleStates>
                                                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                                    </ToggleStates>
                                                </infs:WclButton>
                                            </div>
                                            <div class='sxlb'>
                                                <span class="cptn">Is Display</span>
                                            </div>
                                            <div class='sxlm chkbutton'>
                                                <infs:WclButton runat="server" ID="chkIsDisplay" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                    AutoPostBack="false" Checked='<%# (Container is GridEditFormInsertItem)? false : (Eval("IsDisplay")==null)?false:Eval("IsDisplay") %>'>
                                                    <ToggleStates>
                                                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                                    </ToggleStates>
                                                </infs:WclButton>
                                            </div>     
                                              <div class='sxlb'>
                                                <span class="cptn">Is Hidden From UI</span>
                                            </div>
                                             <div class='sxlm chkbutton'>
                                                <infs:WclButton runat="server" ID="chkIsHiddenFromUI" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                    AutoPostBack="false" Checked='<%# (Container is GridEditFormInsertItem)? false : (Eval("IsHiddenFromUI")==null)?false:Eval("IsHiddenFromUI") %>'>
                                                    <ToggleStates>
                                                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                                    </ToggleStates>
                                                </infs:WclButton>
                                            </div>                                               
                                            <div id ="divSourceAttribute" runat="server" visible="false">
                                                <div class='sxlb'>
                                                    <span class="cptn">Source Attribute</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="ddlSourceAttribute" runat="server"
                                                        DataTextField="BSA_Name" DataValueField="BSA_ID" EmptyMessage="--Select--">
                                                    </infs:WclComboBox>                                                    
                                                </div>
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
        </infs:WclGrid>
        <div class="gclr">
        </div>
    </div>
</div>

<div style="width: 100%; text-align: center" id="dvShowBackLink" runat="server">
    <infs:WclButton runat="server" ID="btnGoBack" Text="Go Back To Service Attribute Group" OnClick="CmdBarCancel_Click">
    </infs:WclButton>
</div>

