<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DAndRAttributeGroupMapping.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.DAndRAttributeGroupMapping" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="msgbox">
    <asp:Label ID="lblMessage" runat="server" CssClass="info">
    </asp:Label>
</div>
<div class="section">
    <h1 class="mhdr">D&A/Additional Attribute Group Mapping
    </h1>
    <div class="content">
        <div class="sxform auto">
        </div>
        <div id="Div1" class="swrap" runat="server">
            <infs:WclGrid runat="server" ID="grdDAndRAttributeGroupMapping" AutoGenerateColumns="false"
                AllowSorting="True" AutoSkinMode="True" CellSpacing="0" OnNeedDataSource="grdDAndRAttributeGroupMapping_NeedDataSource" OnItemDataBound="grdDAndRAttributeGroupMapping_ItemDataBound"
                GridLines="Both" ShowAllExportButtons="False" ShowExtraButtons="True" EnableDefaultFeatures="false" OnUpdateCommand="grdDAndRAttributeGroupMapping_UpdateCommand">
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="ID">
                    <CommandItemSettings ShowAddNewRecordButton="false"
                        ShowExportToExcelButton="False" ShowExportToPdfButton="False" ShowExportToCsvButton="False"
                        ShowRefreshButton="False" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="AttributeGroupMappingID" FilterControlAltText="Filter AttributeGroupMappingID column" Visible="false"
                            HeaderText="AttributeGroupMappingID" SortExpression="AttributeGroupMappingID" UniqueName="AttributeGroupMappingID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" Visible="false"
                            HeaderText="ID" SortExpression="ID" UniqueName="ID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="FieldName" FilterControlAltText="Filter Field Name column"
                            HeaderText="Field Name" SortExpression="FieldName" UniqueName="FieldName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AttributeGroup" FilterControlAltText="Filter Attribute Group column"
                            HeaderText="Attribute Group" SortExpression="AttributeGroup" UniqueName="AttributeGroup">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Attribute" FilterControlAltText="Filter Attribute column"
                            HeaderText="Attribute" SortExpression="Attribute" UniqueName="Attribute">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SpecialFieldTypeName" FilterControlAltText="Filter Special Field Name Group column"
                            HeaderText="Special Field Name" SortExpression="SpecialFieldTypeName" UniqueName="SpecialFieldTypeName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TenantName" FilterControlAltText="Filter Institution column"
                            HeaderText="Institution" SortExpression="TenantName" UniqueName="TenantName">
                        </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn DataField="CustomAttributeName" FilterControlAltText="Filter Custom Attribute column"
                            HeaderText="Custom Attribute" SortExpression="CustomAttributeName" UniqueName="CustomAttributeName">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div id="Div2" class="section" visible="true" runat="server">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblEvService" Text='<%# (Container is GridEditFormInsertItem) ? "Add New D&A AttributeGroup Mapping" : "Edit D&A Attribute Group Mapping" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="Panel1">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Applicants Attributes</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:RadioButton CssClass="rbFields" Checked="true" ID="rbApplicantAttr" ClientIDMode="Static" runat="server" GroupName="FieldMapping" AutoPostBack="false" />
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Special Fields</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:RadioButton CssClass="rbFields" ID="rbSpecialField" ClientIDMode="Static" runat="server" GroupName="FieldMapping" AutoPostBack="false" />
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Custom Attributes</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:RadioButton CssClass="rbFields" ID="rbCustomAttribute" ClientIDMode="Static" runat="server" GroupName="FieldMapping" AutoPostBack="false" />
                                                </div>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlDAndRMapping">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Field Name</span>                
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:Label ID="txtFieldName" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? String.Empty: INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("FieldName"))) %>'></asp:Label>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Attribute Group</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbAttributeGroup" ClientIDMode="Static" DataValueField="BSAD_ID" DataTextField="BSAD_Name" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbAttributeGroup_SelectedIndexChanged"></infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfcmbAttributeGroup" ControlToValidate="cmbAttributeGroup" InitialValue="--SELECT--" Enabled="false"
                                                            Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Attribute Group is required." />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Attribute</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbAttribute" ClientIDMode="Static" DataTextField="BSA_Name" DataValueField="BSA_ID" runat="server"></infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvcmbAttribute" ControlToValidate="cmbAttribute" InitialValue="--SELECT--" Enabled="false"
                                                            Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Attribute is required." />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </asp:Panel>

                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlCustomAttributes">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Field Name</span>            
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:Label ID="lblFieldName" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? String.Empty: INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("FieldName"))) %>'></asp:Label>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Institution</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbInstitution" ClientIDMode="Static" DataValueField="TenantID" DataTextField="TenantName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbInstitution_SelectedIndexChanged"></infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvInstitution" ControlToValidate="cmbInstitution" Enabled="false" InitialValue="--SELECT--"
                                                            Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Institution is required." />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Custom Attribute</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbCustomAttribute" ClientIDMode="Static" DataTextField="CA_AttributeName" DataValueField="CA_CustomAttributeID" runat="server"></infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvCustomAttribute" ControlToValidate="cmbCustomAttribute" Enabled="false" InitialValue="--SELECT--"
                                                            Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Custom Attribute is required." />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </asp:Panel>

                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlSpecialFields">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Field Name</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? String.Empty: INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("FieldName"))) %>'></asp:Label>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Special Field Type</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbSpecialFields" DataTextField="DDSFT_Name" DataValueField="DDSFT_ID" runat="server" ClientIDMode="Static"></infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvSpecialFields" ControlToValidate="cmbSpecialFields" InitialValue="--SELECT--"
                                                            Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Special Field is required." Enabled="false" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                            <div class="sxroend"></div>
                            <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlExternalVendor" DisplayButtons="Save" SaveButtonText="Map"
                                ValidationGroup="grpFormSubmit" ExtraButtonIconClass="icnreset" GridUpdateText="Save" />
                        </FormTemplate>
                    </EditFormSettings>
                </MasterTableView>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
        <div id="divCmdButton" runat="server">
            <div class="sxcbar">
                <div class="sxcmds" style="text-align: center">
                    <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false">
                        <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                </div>
            </div>
        </div>
    </div>
</div>



<script type="text/javascript">
    function pageLoad() {
        // $jQuery("[id$=rbApplicantAttr]").prop('checked', true);
        $jQuery("[id$=pnlSpecialFields]").hide();
        $jQuery("[id$=pnlCustomAttributes]").hide();
        if (($jQuery("[id$=rbApplicantAttr]")).prop("checked")) {
            $jQuery("[id$=pnlDAndRMapping]").show();
            $jQuery("[id$=pnlSpecialFields]").hide();
            $jQuery("[id$=pnlCustomAttributes]").hide();
            if ($jQuery("[id$=rfvSpecialFields]").length > 0) {
                EnableValidator($jQuery("[id$=rfvcmbAttribute]")[0].id);
                EnableValidator($jQuery("[id$=rfcmbAttributeGroup]")[0].id);
                DisableValidator($jQuery("[id$=rfvSpecialFields]")[0].id);
                DisableValidator($jQuery("[id$=rfvCustomAttribute]")[0].id);
                DisableValidator($jQuery("[id$=rfvInstitution]")[0].id);
            }
        }
        else if (($jQuery("[id$=rbCustomAttribute]")).prop("checked")) {
            $jQuery("[id$=pnlCustomAttributes]").show();
            $jQuery("[id$=pnlSpecialFields]").hide();
            $jQuery("[id$=pnlDAndRMapping]").hide();
            if ($jQuery("[id$=rfvSpecialFields]").length > 0) {
                EnableValidator($jQuery("[id$=rfvCustomAttribute]")[0].id);
                EnableValidator($jQuery("[id$=rfvInstitution]")[0].id);
                DisableValidator($jQuery("[id$=rfvSpecialFields]")[0].id);
                DisableValidator($jQuery("[id$=rfvcmbAttribute]")[0].id);
                DisableValidator($jQuery("[id$=rfcmbAttributeGroup]")[0].id);
            }
        }
        else {
            $jQuery("[id$=rbSpecialField]").attr("checked", true);
            $jQuery("[id$=pnlDAndRMapping]").hide();
            $jQuery("[id$=pnlCustomAttributes]").hide();
            $jQuery("[id$=pnlSpecialFields]").show();
            if ($jQuery("[id$=rfvSpecialFields]").length > 0) {
                EnableValidator($jQuery("[id$=rfvSpecialFields]")[0].id);
                DisableValidator($jQuery("[id$=rfvcmbAttribute]")[0].id);
                DisableValidator($jQuery("[id$=rfcmbAttributeGroup]")[0].id);
                DisableValidator($jQuery("[id$=rfvCustomAttribute]")[0].id);
                DisableValidator($jQuery("[id$=rfvInstitution]")[0].id);
            }
        }
        $jQuery('input[type=radio]').click(function () {
            if (this.id == $jQuery("[id$=rbApplicantAttr]").val()) {
                $jQuery("[id$=pnlDAndRMapping]").show();
                $jQuery("[id$=pnlSpecialFields]").hide();
                $jQuery("[id$=pnlCustomAttributes]").hide();
                EnableValidator($jQuery("[id$=rfvcmbAttribute]")[0].id);
                EnableValidator($jQuery("[id$=rfcmbAttributeGroup]")[0].id);
                DisableValidator($jQuery("[id$=rfvSpecialFields]")[0].id);
                DisableValidator($jQuery("[id$=rfvCustomAttribute]")[0].id);
                DisableValidator($jQuery("[id$=rfvInstitution]")[0].id);
                var item = ($jQuery("[id$=cmbSpecialFields]"))[0].control.findItemByValue(0);
                var item2 = ($jQuery("[id$=cmbCustomAttribute]"))[0].control.findItemByValue(0);
                var item3 = ($jQuery("[id$=cmbInstitution]"))[0].control.findItemByValue(0);
                if (item != null) {
                    item.select();
                }
                if (item2 != null) {
                    item2.select();
                }
                if (item3 != null) {
                    item3.select();
                }
            }
            else if (this.id == $jQuery("[id$=rbCustomAttribute]").val()) {
                $jQuery("[id$=pnlDAndRMapping]").hide();
                $jQuery("[id$=pnlSpecialFields]").hide();
                $jQuery("[id$=pnlCustomAttributes]").show();
                DisableValidator($jQuery("[id$=rfvcmbAttribute]")[0].id);
                DisableValidator($jQuery("[id$=rfcmbAttributeGroup]")[0].id);
                DisableValidator($jQuery("[id$=rfvSpecialFields]")[0].id);
                EnableValidator($jQuery("[id$=rfvCustomAttribute]")[0].id);
                EnableValidator($jQuery("[id$=rfvInstitution]")[0].id);
                //var item = ($jQuery("[id$=cmbSpecialFields]"))[0].control.findItemByValue(0);
                var item1 = ($jQuery("[id$=cmbAttributeGroup]"))[0].control.findItemByValue(0);
                var item2 = ($jQuery("[id$=cmbAttribute]"))[0].control.findItemByValue(0);
                $find(($jQuery("[id$=cmbSpecialFields]"))[0].id).set_value('0');
                if (item1 != null) {
                    item1.select();
                }
                if (item2 != null) {
                    item2.select();
                }
                //if (item != null) {
                //    item.select();
                //}
            }
            else {
                $jQuery("[id$=pnlDAndRMapping]").hide();
                $jQuery("[id$=pnlSpecialFields]").show();
                $jQuery("[id$=pnlCustomAttributes]").hide();
                EnableValidator($jQuery("[id$=rfvSpecialFields]")[0].id);
                DisableValidator($jQuery("[id$=rfvcmbAttribute]")[0].id);
                DisableValidator($jQuery("[id$=rfcmbAttributeGroup]")[0].id);
                DisableValidator($jQuery("[id$=rfvCustomAttribute]")[0].id);
                DisableValidator($jQuery("[id$=rfvInstitution]")[0].id);
                //var item1 = ($jQuery("[id$=cmbAttributeGroup]"))[0].control.findItemByValue(0);
                var item2 = ($jQuery("[id$=cmbAttribute]"))[0].control.findItemByValue(0);
                var item3 = ($jQuery("[id$=cmbCustomAttribute]"))[0].control.findItemByValue(0);
                var item4 = ($jQuery("[id$=cmbInstitution]"))[0].control.findItemByValue(0);
                $find(($jQuery("[id$=cmbAttributeGroup]"))[0].id).set_value('0');
                //if (item1 != null) {
                //    item1.set_selected(true);
                //    //item1.Selected = true;
                //}
                if (item2 != null) {
                    item2.select();
                }
                if (item3 != null) {
                    item3.select();
                }
                if (item4 != null) {
                    item4.select();
                }
            }
        });
    }
    function Enable() {
        //debugger;
        EnableValidator($jQuery("[id$=rfvcmbAttribute]")[0].id);
        EnableValidator($jQuery("[id$=rfcmbAttributeGroup]")[0].id);
    }

    //Currently not used
    function ShowSpecialFields() {
        $jQuery("[id$=pnlSpecialFields]").prop('checked', true);
        $jQuery("[id$=pnlSpecialFields]").show();
        EnableValidator($jQuery("[id$=rfvSpecialFields]")[0].id);
    }

    // Code:: Validator Enabled::
    function EnableValidator(id) {
        if ($jQuery('#' + id)[0] != undefined) {
            ValidatorEnable($jQuery('#' + id)[0], true);
            $jQuery('#' + id).hide();
        }
    }

    //Code:: Validator Disabled ::
    function DisableValidator(id) {
        if ($jQuery('#' + id)[0] != undefined) {
            ValidatorEnable($jQuery('#' + id)[0], false);
        }
    }
</script>
