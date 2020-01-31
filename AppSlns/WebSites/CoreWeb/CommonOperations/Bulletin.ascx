<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Bulletin.ascx.cs" Inherits="CoreWeb.CommonOperations.Views.Bulletin" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<style>
    .reContentCell {
        height:86% !important;
    }
</style>

<div class="section">
    <h1 class="mhdr">Bulletin</h1>

    <div class="content">
        <div class="sxform auto">
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenant">
                <div class='sxro sx2co'>
                    <div class='sxlb' title="Select the Institution whose data you want to view.">
                        <span class="cptn">Institution</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="ddlSearchTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" CheckBoxes="true"
                            EnableCheckAllItemsCheckBox="true">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlSearchTenant"
                                Display="Dynamic" CssClass="errmsg" Text="Institution is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                    <div runat="server" id="lblHieararchy" class='sxlb' title="Click the link and select a node to restrict search results to the selected node">
                        <span class='cptn'>Institution Hierarchy</span>
                    </div>
                    <div class='sxlm' runat="server" id="lnkHierarchy">
                        <a href="#" id="instituteHierarchy" onclick="openPopUp();">Select Institution Hierarchy</a>&nbsp;&nbsp
                        <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
            AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbRefresh" OnSaveClick="fsucCmdBarSearch_Click"
            SubmitButtonText="Reset" OnSubmitClick="fsucCmdBarReset_Click" SaveButtonText="Search" SaveButtonIconClass="rbSearch" ValidationGroup="grpFormSubmit"
            CancelButtonText="Cancel" OnCancelClick="fsucCmdBarCancel_Click">
        </infsu:CommandBar>
        <div class="swrap">
            <infs:WclGrid ID="grdBulletin" runat="server" AllowPaging="true" AutoGenerateColumns="false"
                AllowSorting="true" AllowFilteringByColumn="true" AutoSkinMode="true" CellSpacing="0" OnNeedDataSource="grdBulletin_NeedDataSource"
                EnableDefaultFeatures="true" ShowAllExportButtons="false" ShowExtraButtons="false" OnItemDataBound="grdBulletin_ItemDataBound"
                OnItemCommand="grdBulletin_ItemCommand" GridLines="Both" EnableLinqExpressions="false" NonExportingColumns="EditCommandColumn,DeleteColumn">
                <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="BulletinID,InstitutionIds,HieararchyIds" AllowFilteringByColumn="true">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Bulletin" />
                    <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="BulletinTitle" FilterControlAltText="Filter BulletinTitle Column"
                            HeaderText="Bulletin Title" SortExpression="BulletinTitle" UniqueName="BulletinTitle">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BulletinContent" FilterControlAltText="Filter BulletinContent column"
                            HeaderText="Bulletin Content" SortExpression="BulletinContent" UniqueName="BulletinContent">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="InstitutionName" FilterControlAltText="Filter Institution column"
                            HeaderText="Institution" SortExpression="InstitutionName" UniqueName="Institution">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DPMLabel" FilterControlAltText="Filter InstitutionHierarchy column"
                            HeaderText="Institution Hierarchy" SortExpression="DPMLabel" UniqueName="DPMLabel">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Record?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>

                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section" id="divAddForm" runat="server" visible="true">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblRotation" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Bulletin" : "Update Bulletin" %>'
                                        runat="server" />
                                </h1>
                                <div class="content" onclick="HideContentValidation()">
                                    <div class="sxform auto">
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlItem">
                                            <div class="msgbox">
                                                <asp:Label ID="lblGridMessage" runat="server"></asp:Label>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class="sxlb">
                                                    <span class="cptn">Institution</span><span class="reqd">*</span>
                                                </div>
                                                <div class="sxlm">
                                                    <infs:WclComboBox runat="server" ID="ddlTenant" AutoPostBack="false" EnableCheckAllItemsCheckBox="true"
                                                        CheckBoxes="true" DataTextField="TenantName" DataValueField="TenantID" EmptyMessage="--Select--"
                                                        OnClientKeyPressing="openCmbBoxOnTab" Localization-CheckAllString="Check All" Filter="Contains">
                                                    </infs:WclComboBox>

                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenant" Enabled="true"
                                                            Display="Dynamic" CssClass="errmsg" Text="Institution is required." ValidationGroup="grpFormSubmit1" />
                                                    </div>
                                                </div>
                                                <div id="lblHierarchyWhileAddUpdate" runat="server" class='sxlb' title="Click the link and select a node to restrict search results to the selected node">
                                                    <span class="cptn">Institution Hierarchy</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm m2spn' id="lnkHierarchyWhileAddUpdate" runat="server">
                                                    <a href="#" id="instituteHierarchyWhileAddUpdate" onclick="openPopUpWhileAddUpdate();">Select Institution Hierarchy</a>&nbsp;&nbsp
                                                    <asp:Label ID="lblinstituteHierarchyWhileAddUpdate" runat="server"></asp:Label>
                                                    <asp:HiddenField ID="hdnDepartmntPrgrmMppngWhileAddUpdate" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnHierarchyLabelWhileAddUpdate" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnInstitutionNodeIdWhileAddUpdate" runat="server" Value="" />
                                                </div>
                                                <div class='vldx'>
                                                    <asp:CustomValidator ID="cvHierarchySelection" CssClass="errmsg" Display="Dynamic" runat="server" Enabled="false"
                                                        EnableClientScript="true" ErrorMessage="Institution Hierarchy is required." ValidationGroup="grpFormSubmit1"
                                                        ClientValidationFunction="validateHeirarchy">
                                                    </asp:CustomValidator>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class="sxlb">
                                                    <span class="cptn">Bulletin Title</span><span class="reqd">*</span>
                                                </div>
                                                <div class="sxlm">
                                                    <infs:WclTextBox runat="server" ID="txtBulletinTitle" MaxLength="50">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvItemTitle"
                                                            ControlToValidate="txtBulletinTitle" class="errmsg" Display="Dynamic" Enabled="true"
                                                            ValidationGroup="grpFormSubmit1" ErrorMessage="Bulletin Title is required." />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Bulletin Content</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm m3spn'>
                                                     <div style="height:405px;">
                                                    <infs:WclEditor ID="rdEditorNotes" ClientIDMode="Static" runat="server" Width="99.5%"
                                                        ToolsFile="~/Templates/Data/Tools.xml" EnableResize="false" OnClientLoad="OnClientLoad1">
                                                    </infs:WclEditor>
                                                         </div>
                                                    <div class='vldx'>
                                                        <asp:Label ID="lblErrorMsg" class="errmsg" runat="server" Text="Bulletin Content is required." Visible="false"></asp:Label>
                                                        <asp:CustomValidator ID="cvContent" CssClass="errmsg" Display="Dynamic" runat="server"
                                                            ErrorMessage="Bulletin Content is required." ValidationGroup="grpFormSubmit1"
                                                            ClientValidationFunction="ValidateContent">
                                                        </asp:CustomValidator>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarPermission" runat="server" GridMode="true" DefaultPanel="pnlItem" GridInsertText="Save" GridUpdateText="Save"
                                        ValidationGroup="grpFormSubmit1" />
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
            <asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
            <asp:HiddenField ID="hdnTenantId" runat="server" Value="" />

            <asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
            <asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
            <asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />


        </div>
    </div>
</div>
<script type="text/javascript">
    var winopen = false;

    function openPopUp() {
        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "";//OrderQueue
        //var tenantId = 2;
        var tenantId = $jQuery("[id$=hdnTenantId]").val();
        if (tenantId != "0" && tenantId != "") {
            var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceOperations/Pages/InstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName=" + screenName + "&DelemittedDeptPrgMapIds=" + DepartmentProgramId);
            var win = $window.createPopup(url, { size: "600,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
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
                //$jQuery("[id$=lblinstituteHierarchy]").text(arg.HierarchyLabel);
            }
            winopen = false;
        }
    }



    function openPopUpWhileAddUpdate() {
        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "";
        var tenantId = $jQuery("[id$=hdnTenantId]").val();
        if (tenantId != "0" && tenantId != "") {
            var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppngWhileAddUpdate]").val();
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceOperations/Pages/InstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName=" + screenName + "&DelemittedDeptPrgMapIds=" + DepartmentProgramId);
            var win = $window.createPopup(url, { size: "600,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientCloseWhileAddUpdate });
            winopen = true;
        }
        else {
            $alert("Please select Institution.");
        }
        return false;
    }

    function OnClientCloseWhileAddUpdate(oWnd, args) {

        oWnd.remove_close(OnClientCloseWhileAddUpdate);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
                $jQuery("[id$=hdnDepartmntPrgrmMppngWhileAddUpdate]").val(arg.DepPrgMappingId);
                $jQuery("[id$=hdnHierarchyLabelWhileAddUpdate]").val(arg.HierarchyLabel);
                $jQuery("[id$=hdnInstitutionNodeIdWhileAddUpdate]").val(arg.InstitutionNodeId);
                $jQuery("[id$=lblinstituteHierarchyWhileAddUpdate]").text(arg.HierarchyLabel);
                <%-- __doPostBack("<%= btnDoPostBack.ClientID %>", "");--%>
                HideValidationMessage();
            }
            winopen = false;
        }
    }

    function validateHeirarchy(oSrc, args) {

        var HeirarchyId = $jQuery("[id$=hdnDepartmntPrgrmMppngWhileAddUpdate]").val();
        if (HeirarchyId != null && HeirarchyId != "") {
            args.IsValid = true;
        }
        else {
            args.IsValid = false;
        }
    }

    function OnClientLoad1(editor, args) {
        $jQuery('ul.reToolbar').width('auto');
    }

    function ValidateContent(sender, args) {
        var editor = $jQuery("[id$=rdEditorNotes]")[0];
        text = editor.control.get_text();
        text = text.replace(/(?:\\[rn]|[\r\n]+)+/g, "");
        var textLength = text.length;
        if (text == "" || textLength == 0)
            return args.IsValid = false;
        else
            return args.IsValid = true;
    }

    function HideValidationMessage() {
       // debugger;
        var HeirarchyId = $jQuery("[id$=hdnDepartmntPrgrmMppngWhileAddUpdate]").val();
        var cvHierarchySelection = $jQuery("[id$=cvHierarchySelection]");
        if (HeirarchyId != null && HeirarchyId != "") {
            cvHierarchySelection.hide();
        }
        else {
            cvHierarchySelection.show();
        }
    }

    function HideContentValidation() {
        var editor = $jQuery("[id$=rdEditorNotes]")[0];
        var cvContent = $jQuery("[id$=cvContent]");
        text = editor.control.get_text();
        text = text.replace(/(?:\\[rn]|[\r\n]+)+/g, "");
        var textLength = text.length;
        if (text != "" || textLength > 0)
            cvContent.hide();
    }

</script>
