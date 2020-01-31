<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.SetupCompliancePackages" CodeBehind="SetupCompliancePackages.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="../Shared/Controls/IsActiveToggle.ascx" TagName="IsActiveToggle"
    TagPrefix="uc1" %>

<style type="text/css">
    .reEditorModes a {
        display: none;
    }

    .reToolZone {
        display: none;
    }

    .bullet ul {
        margin-left: 10px;
        padding-left: 10px !important;
    }

    .bullet li {
        list-style-position: inside;
        list-style: disc;
    }

    .bullet ol {
        list-style-type: decimal;
        margin-left: 10px;
        padding-left: 10px;
    }

        .bullet ol li {
            list-style: decimal;
        }
</style>

<asp:XmlDataSource ID="xdtsPackages" runat="server" DataFile="~/App_Data/DB.xml"
    XPath="//MasterCompliance/MasterPackage/*"></asp:XmlDataSource>
<infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/ContentEditor.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="section">
    <h1 class="mhdr">Manage Packages
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
            <infs:WclGrid runat="server" ID="grdPackage" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                GridLines="Both" EnableDefaultFeatures="true" ShowAllExportButtons="False" ShowExtraButtons="true"
                NonExportingColumns="EditCommandColumn,DeleteColumn" OnNeedDataSource="grdPackage_NeedDataSource"
                OnItemCommand="grdPackage_ItemCommand" OnItemCreated="grdPackage_ItemCreated"
                ValidationGroup="grpFormSubmit" OnItemDataBound="grdPackage_ItemDataBound">
                <%--<ExtraCommandButtons>
                    <infs:WclButton ID="WclButton1" Text="Copy Package" CommandName="InitInsert" runat="server"
                        CommandArgument="MakeCopy" Icon-PrimaryIconCssClass="icncopy" Visible="false" />
                </ExtraCommandButtons>--%>
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="CompliancePackageID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Package"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="PackageName" FilterControlAltText="Filter PackageName column"
                            HeaderText="Package Name" SortExpression="PackageName" UniqueName="PackageName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PackageLabel" FilterControlAltText="Filter PackageLabel column"
                            HeaderText="Package Label" SortExpression="PackageLabel" UniqueName="PackageLabel">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                            HeaderText="Description" SortExpression="Description" UniqueName="Description">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn DataField="ExplanatoryNotes" FilterControlAltText="Filter ExplanatoryNotes column"
                            HeaderText="Explanatory Notes" SortExpression="ExplanatoryNotes" UniqueName="ExplanatoryNotes">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ExceptionDescription" FilterControlAltText="Filter ExceptionDescription column"
                            HeaderText="Exception Description" SortExpression="ExceptionDescription" UniqueName="ExceptionDescription">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="ScreenLabel" FilterControlAltText="Filter ScreenLabel column"
                            HeaderText="Screen Label" SortExpression="ScreenLabel" UniqueName="ScreenLabel">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="IsActive" FilterControlAltText="Filter IsActive column"
                            HeaderText="Is Active" SortExpression="IsActive" UniqueName="IsActive">
                            <ItemTemplate>
                                <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("IsActive"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                <asp:HiddenField ID="hdnfIsCreatedByAdmin" runat="server" Value='<%#Eval("IsCreatedByAdmin")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="TenantName" FilterControlAltText="Filter TenantName column"
                            HeaderText="Tenant" SortExpression="TenantName" UniqueName="TenantName">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Package?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section" runat="server" visible="true" id="divEditFormBlock">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblEHPackage" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Package" : "Update Package" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPackage">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Package Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtPackageName" Text='<%# Eval("PackageName") %>'
                                                        MaxLength="100">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvPackageName" ControlToValidate="txtPackageName"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Package Name is required." />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Package Label</span><%--<span class="reqd">*</span>--%>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtPackageLabel" Text='<%# Eval("PackageLabel") %>'
                                                        MaxLength="100">
                                                    </infs:WclTextBox>
                                                    <%-- <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rFvPackageLabel" ControlToValidate="txtPackageLabel"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Package Label is required." />
                                                    </div>--%>
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
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Screen Label is required." />
                                                    </div>--%>
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
                                                        AutoPostBack="false" Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("IsActive") %>'>
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                                        </ToggleStates>
                                                    </infs:WclButton>--%>
                                                    <uc1:IsActiveToggle runat="server" ID="chkActive" IsAutoPostBack="false" IsActiveEnable="true" Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("IsActive") %>' />
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Show Details in Order Flow</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclButton runat="server" ID="chkViewdetails" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                        AutoPostBack="false" Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("IsViewDetailsInOrderEnabled") %>'>
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                            <telerik:RadButtonToggleState Text="No" Value="False" />
                                                        </ToggleStates>
                                                    </infs:WclButton>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtPkgDescription" Text='<%# Eval("Description") %>'
                                                        MaxLength="250">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>

                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Package Type</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm '>
                                                    <infs:WclComboBox ID="cmbCompliancePackageType" runat="server" DataTextField="CPT_Name" AutoPostBack="false"
                                                        DataValueField="CPT_ID">
                                                    </infs:WclComboBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator ID="rfvCompliancePackageType" runat="server"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Package type is required."
                                                            InitialValue="--SELECT--" ControlToValidate="cmbCompliancePackageType"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Checklist URL</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtChkDocumentURL" Text='<%# Eval("ChecklistURL") %>'
                                                        MaxLength="512">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RegularExpressionValidator ID="revCheckListDocument" runat="server" ControlToValidate="txtChkDocumentURL"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please enter a valid URL (i.e. - http://www.Example.com)."
                                                            ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$">
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div id="dvMappingHierarchy" runat="server" style="display:block" class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Mapping Hierarchy</span>
                                                </div>
                                                <div class='sxlm m3spn'>
                                                    <asp:Label ID="lblMappedHierarchy" runat="server"></asp:Label>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Package Detail</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <div class="content" style="height: 160px;">
                                                        <infs:WclEditor ID="rdEditorPackageDetail" Width="99.5%" Height="130px" runat="server" ToolsFile="~/BkgSetup/Data/Tools.xml" OnClientLoad="OnClientLoad" EnableResize="false">
                                                        </infs:WclEditor>
                                                    </div>
                                                    <div class='vldx'>
                                                        <asp:CustomValidator runat="server" ID="cstValEditorPackageDetail" ControlToValidate="rdEditorPackageDetail" ClientValidationFunction="ValidatePackageDetailLength"
                                                            ValidationGroup="grpFormSubmit" class="errmsg" Display="Dynamic" ErrorMessage="Please don't enter more than 1000 characters." />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Package Details Display</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:RadioButtonList ID="rbtnDisplayPosition"  runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                                        <asp:ListItem Text="Above" Value="AAAA" ></asp:ListItem>
                                                        <asp:ListItem Text="Below" Value="AAAB" Selected="True"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co monly'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Explanatory Notes</span>
                                                </div>
                                                <infs:WclTextBox runat="server" ID="txtPkgNotes" TextMode="MultiLine" Height="50px">
                                                </infs:WclTextBox>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co monly'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Exception Description</span>
                                                </div>
                                                <infs:WclTextBox runat="server" ID="txtPkgExceptionDesc" TextMode="MultiLine" Height="50px">
                                                </infs:WclTextBox>
                                                <div class='sxroend'>
                                                </div>
                                            </div>

                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarPackage" runat="server" GridMode="true" DefaultPanel="pnlPackage" GridInsertText="Save" GridUpdateText="Save"
                                        ValidationGroup="grpFormSubmit" ExtraButtonIconClass="icnreset" />
                                    <%-- <infsu:CommandBar ID="fsucCmdBarPackage" runat="server" DefaultPanel="pnlPackage"
                                        GridMode="true" ValidationGroup="grpFormSubmit" DisplayButtons="Save,Cancel">
                                        <ExtraCommandButtons>
                                            <infs:WclButton runat="server" ID="btnApprove" Text="Approve" Visible="false">
                                                <Icon PrimaryIconCssClass="rbOk" />
                                            </infs:WclButton>
                                            <infs:WclButton runat="server" ID="btnReject" Text="Reject" Visible="false">
                                                <Icon PrimaryIconCssClass="rbRemove" />
                                            </infs:WclButton>
                                            <infs:WclButton runat="server" ID="btnSaveForm" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                                CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'
                                                ValidationGroup="grpFormSubmit">
                                                <Icon PrimaryIconCssClass="rbSave" />
                                            </infs:WclButton>
                                            <infs:WclButton runat="server" ID="btnCancelForm" Text="Cancel" CommandName="Cancel">
                                                <Icon PrimaryIconCssClass="rbCancel" />
                                            </infs:WclButton>
                                        </ExtraCommandButtons>
                                    </infsu:CommandBar>--%>
                                    <%-- <infsu:CommandBar ID="fsucCmdBarPackage" runat="server" DefaultPanel="pnlPackage" ValidationGroup="grpFormSubmit">
                                        <ExtraCommandButtons>
                                            <infs:WclButton runat="server" ID="btnApprove" Text="Approve" Visible="false">
                                                <Icon PrimaryIconCssClass="rbOk" />
                                            </infs:WclButton>
                                            <infs:WclButton runat="server" ID="btnReject" Text="Reject" Visible="false">
                                                <Icon PrimaryIconCssClass="rbRemove" />
                                            </infs:WclButton>
                                            <infs:WclButton runat="server" ID="btnSaveForm" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                                CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'
                                                ValidationGroup="grpFormSubmit">
                                                <Icon PrimaryIconCssClass="rbSave" />
                                            </infs:WclButton>
                                            <infs:WclButton runat="server" ID="btnCancelForm" Text="Cancel" CommandName="Cancel">
                                                <Icon PrimaryIconCssClass="rbCancel" />
                                            </infs:WclButton>
                                        </ExtraCommandButtons>
                                    </infsu:CommandBar>--%>
                                </div>
                            </div>
                            <%--  <div class="section" runat="server" id="divCopyBlock" visible="false">
                                <h1 class="mhdr">
                                    <asp:Label ID="Label1" Text='<%# (Container is GridEditFormInsertItem) ? "Copy Packages" : "Update Record" %>'
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
                                                            <telerik:RadComboBoxItem Text="Sophomore Requirements" Value="0" />
                                                            <telerik:RadComboBoxItem Text="Sr. Requirements" Value="1" />
                                                            <telerik:RadComboBoxItem Text="Jr. Requirements" Value="2" />
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
