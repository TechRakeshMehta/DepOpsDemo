<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.ManageFeature" CodeBehind="ManageFeature.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Import Namespace="INTSOF.Utils" %>
<infs:WclResourceManagerProxy runat="server" ID="manageFeatureProxy">
    <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/ManageFeature.js"
        ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<input type="hidden" id="hdnCurrentNode" name="hdnSelectedNode" />
<asp:HiddenField runat="server" ID="hfldUpdStat" />
<div class="msgbox" id="divSuccessMsg">
    <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
</div>

<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblManageFeature" runat="server" Text=""></asp:Label>
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="sxpnl">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <asp:Label ID="lblBusinessChannel" runat="server" Text="Business Channel" AssociatedControlID="cmbBusinessChannel" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbBusinessChannel" runat="server" MarkFirstMatch="true" TabIndex="0" AutoPostBack="true" OnSelectedIndexChanged="cmbBusinessChannel_SelectedIndexChanged"
                            Width="60%" DataTextField="Name" DataValueField="BusinessChannelTypeID" Style="z-index: 7002;" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="section">
    <div class="content">
        <infs:WclTreeList runat="server" ID="treeListFeature" AllowPaging="true" PageSize="10"
            DataKeyNames="ProductFeatureID,ParentProductFeatureID,IsSystem" ParentDataKeyNames="ProductFeature2.ProductFeatureID, ProductFeature2.ParentProductFeatureID, ProductFeature2.IsSystem"
            AutoGenerateColumns="false" OnNeedDataSource="treeListFeature_NeedDataSource"
            OnDeleteCommand="treeListFeature_DeleteCommand" OnInsertCommand="treeListFeature_InsertCommand"
            OnUpdateCommand="treeListFeature_UpdateCommand" OnItemCreated="treeListFeature_ItemCreated" OnPageSizeChanged="treeListFeature_PageSizeChanged"
            OnItemDataBound="treeListFeature_ItemDataBound" PagerStyle-Position="TopAndBottom">
            <Columns>
                <telerik:TreeListEditCommandColumn UniqueName="EditCommandColumn" EditText="Edit"
                    AddRecordText="Add New Feature" ButtonType="ImageButton">
                </telerik:TreeListEditCommandColumn>
                <telerik:TreeListBoundColumn DataField="ProductFeatureID" UniqueName="ProductFeatureID"
                    Visible="false" HeaderText="ProductFeatureID" ReadOnly="true" />
                <telerik:TreeListBoundColumn DataField="Name" UniqueName="Name" HeaderText="Feature Name" />
                <telerik:TreeListBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description" />
                <telerik:TreeListBoundColumn DataField="UIControlID" UniqueName="UIControlID" HeaderText="Web Page Name" />
                <telerik:TreeListBoundColumn DataField="IconImageName" UniqueName="IconImageName"
                    HeaderText="Icon Image Name" />
                <telerik:TreeListBoundColumn DataField="NavigationURL" Visible="false" UniqueName="NavigationURL"
                    HeaderStyle-Wrap="true" HeaderText="Navigation URL" />
                <telerik:TreeListBoundColumn DataField="DisplayOrder" UniqueName="DisplayOrder" HeaderText="Display Order" />
                <telerik:TreeListBoundColumn DataField="ForExternalUser" Visible="false" UniqueName="ForExternalUser"
                    HeaderStyle-Wrap="true" HeaderText="For External User" />
                <telerik:TreeListBoundColumn DataField="IsDashboardFeature" Visible="false" UniqueName="IsDashboardFeature"
                    HeaderStyle-Wrap="true" HeaderText="Is Dashboard Feature?" />
                <telerik:TreeListButtonColumn UniqueName="deleteButtonColumn" CommandName="Delete"
                    Text="Delete" ButtonType="ImageButton">
                    <ItemStyle CssClass="MyImageButton" />
                </telerik:TreeListButtonColumn>
            </Columns>
            <EditFormSettings EditFormType="Template">
                <FormTemplate>
                    <div class="section">
                        <h1 class="mhdr">
                            <asp:Label ID="lblHeading" Text='<%# (Container is TreeListEditFormInsertItem) ? "Add Feature" : "Edit Feature"%>'
                                runat="server"></asp:Label></h1>
                        <div class="content">
                            <div class="sxform auto">
                                <div class="msgbox">
                                    <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                                </div>
                                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlManageFeature">
                                    <div class='sxro sx2co'>
                                        <div class='sxlb'>
                                            <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" Text="Feature Name" CssClass="cptn"></asp:Label><span
                                                class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox Text='<%# Bind("Name")%>' ID="txtName" MaxLength="25" TabIndex="1"
                                                runat="server" CssClass="l50" />
                                            <div class="vldx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                                                    Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_FEATURE_NAME_REQUIRED)%>'
                                                    ValidationGroup="grpValdManageFeature" />
                                                <asp:RegularExpressionValidator runat="server" ID="revName" ControlToValidate="txtName"
                                                    Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\,\%\@\(\)]{3,25}$"
                                                    ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>'
                                                    ValidationGroup="grpValdManageFeature"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class='sxlb'>
                                            <asp:Label ID="lblDisplayOrder" runat="server" AssociatedControlID="txtDisplayOrder"
                                                CssClass="cptn" Text="Display Order"></asp:Label><span class="reqd">*</span>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclNumericTextBox Text='<%# Bind("DisplayOrder")%>' MaxLength="4" ShowSpinButtons="false"
                                                IncrementSettings-InterceptArrowKeys="false" IncrementSettings-InterceptMouseWheel="false"
                                                TabIndex="2" ID="txtDisplayOrder" Label="" MinValue="0" runat="server" Type="Number"
                                                CssClass="l10">
                                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                            </infs:WclNumericTextBox>
                                            <div class="vldx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvDisplayOrder" ControlToValidate="txtDisplayOrder"
                                                    Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_FEATURE_DISPLAY_REQUIRED)%>'
                                                    ValidationGroup="grpValdManageFeature" />
                                                <asp:RegularExpressionValidator runat="server" ID="revDisplayOrder" ControlToValidate="txtDisplayOrder"
                                                    Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_FOUR_DIGIT_REQUIRED)%>'
                                                    ValidationExpression="^[\d]{1,4}$" ValidationGroup="grpValdManageFeature"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx2co'>
                                        <div class='sxlb'>
                                            <asp:Label ID="lblDescription" runat="server" AssociatedControlID="txtDescription" CssClass="cptn"
                                                Text="Description"></asp:Label>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox Text='<%# Bind("Description")%>' ID="txtDescription" TabIndex="3"
                                                MaxLength="255" runat="server">
                                            </infs:WclTextBox>
                                            <div class="vldx">
                                                <asp:RegularExpressionValidator runat="server" ID="revDescription" ControlToValidate="txtDescription"
                                                    Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_DESCRIPTION_INVALID)%>'
                                                    ValidationExpression="^[\w\d\s\-\.\,\%\@\(\)]{3,255}$" ValidationGroup="grpValdManageFeature"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class='sxlb'>
                                            <asp:Label ID="lblIconImageName" runat="server" AssociatedControlID="txtIconImageName" CssClass="cptn"
                                                Text="Icon Image Name"></asp:Label>
                                        </div>
                                        <div class='sxlm'>
                                            <infs:WclTextBox Text='<%#Bind("IconImageName") %>' MaxLength="100" TabIndex="4"
                                                ID="txtIconImageName" runat="server" />
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx2co' id="dvFeatureAreaType" runat="server">
                                        <div class='sxlb'>
                                            <asp:Label ID="Label1" runat="server" Text="Feature Area Type" CssClass="cptn"></asp:Label>
                                            <%--AssociatedControlID="rblFeatureAreaType"--%>
                                        </div>
                                        <div class='sxlm m2spn'>
                                            <asp:RadioButtonList ID="rblFeatureAreaType" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblFeatureAreaType_SelectedIndexChanged"
                                                DataTextField="FAT_Name" DataValueField="FAT_Code" CssClass="w_cptn" AutoPostBack="true">
                                            </asp:RadioButtonList>
                                            <%--<asp:CheckBox runat="server" ID="rblFeatureAreaType" TabIndex="8" OnCheckedChanged="chkIsParent_CheckedChanged"
                                                AutoPostBack="true" Checked='<%# SetCheckbox(DataBinder.Eval(Container.DataItem, "UIControlID").ToString(),
                                         DataBinder.Eval(Container.DataItem, "Name").IsNull()?String.Empty:DataBinder.Eval(Container.DataItem, "Name").ToString())%>' />--%>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx2co'>
                                        <div class='sxlb'>
                                            <asp:Label ID="lblIsParent" runat="server" AssociatedControlID="chkIsParent" Text="Is this a parent Feature?" CssClass="cptn"></asp:Label>
                                        </div>
                                        <div class='sxlm m2spn'>
                                            <asp:CheckBox runat="server" ID="chkIsParent" TabIndex="8" OnCheckedChanged="chkIsParent_CheckedChanged"
                                                AutoPostBack="true" Checked='<%# SetCheckbox(DataBinder.Eval(Container.DataItem, "UIControlID").ToString(),
                                         DataBinder.Eval(Container.DataItem, "Name").IsNull()?String.Empty:DataBinder.Eval(Container.DataItem, "Name").ToString())%>' />
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx2co'>
                                        <div class='sxlb'>
                                            <asp:Label ID="lblIsReportFeature" runat="server" AssociatedControlID="chkIsReportFeature" Text="Is this a Tableau report Feature?" CssClass="cptn"></asp:Label>
                                        </div>
                                        <div class='sxlm m2spn'>
                                            <asp:CheckBox runat="server" ID="chkIsReportFeature" TabIndex="8" OnCheckedChanged="chkIsReportFeature_CheckedChanged"
                                                AutoPostBack="true" />
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx2co'>
                                        <div class='sxlb'>
                                            <asp:Label ID="lblUIControlID" runat="server" AssociatedControlID="txtUIControlID" CssClass="cptn"
                                                Text="Web Page Name"></asp:Label><span class="reqd" id="reqdSpanWebPageName" hidden="hidden">*</span>
                                        </div>
                                        <div class='sxlm m2spn'>
                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                <tr>
                                                    <td style="padding-right: 2px;">
                                                        <div style="width: 100% !important">
                                                            <div style="float: left">
                                                                <infs:WclButton ID="btnUIControlID" Text="..." runat="server" TabIndex="7" AutoPostBack="false"
                                                                    Style="vertical-align: top;" CausesValidation="True">
                                                                </infs:WclButton>
                                                            </div>
                                                            <div style="float: left; width: 90%;">
                                                                <infs:WclTextBox Width="99%" Text='<%# Bind("UIControlID")%>' TabIndex="6" MaxLength="150"
                                                                    ID="txtUIControlID" runat="server" />
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <%-- <td width="29px">
                                                        <infs:WclButton ID="btnUIControlID" Text="..." runat="server" TabIndex="7" AutoPostBack="false"
                                                            Style="vertical-align: top;" CausesValidation="True">
                                                        </infs:WclButton>
                                                    </td>--%>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div class="vldx">
                                                            <%--<asp:Label ID="lblUiCtrError" CssClass="errmsg" runat="server"></asp:Label>--%>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvUIControlID" ControlToValidate="txtUIControlID"
                                                                Display="Dynamic" CssClass="errmsg" Enabled="false" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_WEB_PAGE_NAME_REQUIRED)%>'
                                                                ValidationGroup="grpValdManageFeature" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx2co'>
                                        <div class='sxlb'>
                                            <asp:Label ID="lblNavigationURL" runat="server" AssociatedControlID="txtNavigationURL" CssClass="cptn"
                                                Text="Navigation URL"></asp:Label>
                                        </div>
                                        <div class='sxlm m2spn'>
                                            <infs:WclTextBox Text='<%# Bind("NavigationURL")%>' MaxLength="1500" TabIndex="5"
                                                ID="txtNavigationURL" runat="server">
                                            </infs:WclTextBox>
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>
                                    <div class='sxro sx2co'>
                                        <div class='sxlb'>
                                            <asp:Label ID="lblIsDashboard" runat="server" AssociatedControlID="chkIsDashboard" Text="Is Dashboard Feature?" CssClass="cptn"></asp:Label>
                                        </div>
                                        <div class='sxlm'>
                                            <asp:CheckBox runat="server" ID="chkIsDashboard" TabIndex="8" OnCheckedChanged="chkIsDashboard_CheckedChanged"
                                                AutoPostBack="true" Checked='<%# (String.IsNullOrEmpty(Eval("IsDashboardFeature").ToString()) ? false : Eval("IsDashboardFeature"))%>' />
                                        </div>
                                        <div class='sxlb' id="dvIsExternal" runat="server" visible='<%# (String.IsNullOrEmpty(Eval("IsDashboardFeature").ToString()) ? false : Eval("IsDashboardFeature"))%>'>
                                            <asp:Label ID="lblIsExternal" runat="server" AssociatedControlID="chkIsExternal" Text="For External Users?" CssClass="cptn"></asp:Label>
                                        </div>
                                        <div class='sxlm'>
                                            <asp:CheckBox runat="server" ID="chkIsExternal" TabIndex="8"
                                                AutoPostBack="false" Visible='<%# (String.IsNullOrEmpty(Eval("IsDashboardFeature").ToString()) ? false : Eval("IsDashboardFeature"))%>'
                                                Checked='<%# (String.IsNullOrEmpty(Convert.ToString(Eval("ForExternalUser"))) ? false : Eval("ForExternalUser"))%>' />
                                        </div>
                                        <div class='sxroend'>
                                        </div>
                                    </div>


                                </asp:Panel>
                            </div>
                            <infsu:CommandBar ID="fsuCBarFeat" runat="server" TabIndexAt="9" DefaultPanel="pnlManageFeature" GridInsertText="Save" GridUpdateText="Save"
                                TreeListMode="true" ValidationGroup="grpValdManageFeature" />
                        </div>
                    </div>
                </FormTemplate>
            </EditFormSettings>
            <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric"></PagerStyle>
        </infs:WclTreeList>
    </div>
</div>
