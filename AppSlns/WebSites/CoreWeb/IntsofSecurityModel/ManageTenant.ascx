<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.ManageTenant" CodeBehind="ManageTenant.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/LocationInfo.ascx" TagPrefix="uc"
    TagName="Location" %>
<%@ Import Namespace="INTSOF.Utils" %>
<infs:WclResourceManagerProxy runat="server" ID="manageFeatureProxy">
    <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/ManageTenant.js"
        ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="msgbox" id="divSuccessMsg">
    <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
</div>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblManageTenant" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdTenant" AllowPaging="True" PageSize="10" AutoGenerateColumns="False"
                AllowSorting="True" GridLines="Both" OnInsertCommand="grdTenant_InsertCommand"
                ShowAllExportButtons="false" GridViewMode="AutoAddOnly" OnNeedDataSource="grdTenant_NeedDataSource"
                OnUpdateCommand="grdTenant_UpdateCommand" OnDeleteCommand="grdTenant_DeleteCommand"
                OnItemCreated="grdTenant_ItemCreated" OnItemDataBound="grdTenant_ItemDataBound"
                OnDetailTableDataBind="grdTenant_DetailTableBound" OnItemCommand="grdTenant_ItemCommand"
                NonExportingColumns="ManageServiceFootprintColumn,EditCommandColumn, DeleteColumn,ManageSubTenant,ManageCompliance,WebsiteSetup">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    HideStructureColumns="true" Pdf-PageWidth="300mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="TenantID" AllowFilteringByColumn="true">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Institution"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="TenantName" HeaderText="Institution Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TenantTypeDescription" HeaderText="Institution Type"
                            HeaderStyle-Width="100px" UniqueName="tenantTypeDescription" DataType="System.String"
                            SortExpression="TenantTypeDescription">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TenantDesc" HeaderText="Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TenantAddress" FilterControlAltText="Filter Address column"
                            HeaderText="Address" SortExpression="TenantAddress" UniqueName="tenantAddress"
                            DataType="System.String">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TenantCity" FilterControlAltText="Filter city column"
                            HeaderText="City" SortExpression="TenantCity" DataType="System.String" UniqueName="tenantCity">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TenantState" HeaderText="State" SortExpression="TenantState"
                            UniqueName="tenantState" DataType="System.String">
                        </telerik:GridBoundColumn>
                        <telerik:GridMaskedColumn DataField="TenantZipCode" HeaderText="Zip Code" SortExpression="TenantZipCode"
                            UniqueName="tenantZipCode" Mask="#####" FilterControlWidth="50px">
                        </telerik:GridMaskedColumn>
                        <telerik:GridMaskedColumn UniqueName="Phone" HeaderText="Phone" DataField="TenantPhone"
                            SortExpression="TenantPhone" Mask="(###)-###-####" FilterControlWidth="105px">
                        </telerik:GridMaskedColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" Visible="false" UniqueName="ManageServiceFootprintColumn">
                            <ItemTemplate>
                                <%--<asp:HyperLink ID="HyperLink1" NavigateUrl="~/IntsofSecurityModel/default.aspx?ucid=E90F9BC6AA7826117DFC617539EE23C1F911DE437506E26CF6AEC3E0D771768AD0264FB97C797F8123619C9FA058039523866C985B52D29CEB4687F590CFDF1D63D65A9EE1C0D622"
                                    runat="server" Text="Manage Service Footprint" />--%>
                                <a runat="server" id="ancServiceFootprint">Manage Service Footprint</a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageSubTenant">
                            <ItemTemplate>
                                <a id="ancManageSubTenant" runat="server"></a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageCompliance">
                            <ItemTemplate>
                                <a id="ancProCompliance" runat="server">Manage Compliance</a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="WebsiteSetup">
                            <ItemTemplate>
                                <a id="ancWebsiteSetup" runat="server">Website Setup </a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Institution?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblHeading" Text='<%#(Container is GridEditFormInsertItem) ? "Add Institution" : "Edit Institution"%>'
                                        runat="server"></asp:Label></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlMTenant">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" Text="Institution Name" CssClass="cptn"></asp:Label><span
                                                        class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox Width="99%" Text='<%# Bind("TenantName")%>' MaxLength="50" TabIndex="1"
                                                        ID="txtName" runat="server" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" CssClass="errmsg" ID="rfvName" ControlToValidate="txtName"
                                                            Display="Dynamic" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_TENANT_REQUIRED)%>'
                                                            ValidationGroup="grpValdManageTenant" />
                                                        <asp:RegularExpressionValidator runat="server" CssClass="errmsg" ID="regtxtName"
                                                            ControlToValidate="txtName" Display="Dynamic" ValidationExpression="^[\w\d\s\-\.\,\%\(\)\/]{3,50}$"
                                                            ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>'
                                                            ValidationGroup="grpValdManageTenant"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblPhone" runat="server" AssociatedControlID="maskTxtPhone" Text="Phone" CssClass="cptn"></asp:Label><span
                                                        class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclMaskedTextBox Width="99%" ID="maskTxtPhone" runat="server" TabIndex="2"
                                                        Mask="(###)-###-####" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator Display="Dynamic" ID="rfvTxtPhone" runat="server" CssClass="errmsg"
                                                            ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.PHONE_REQUIRED)%>' ControlToValidate="maskTxtPhone"
                                                            ValidationGroup="grpValdManageTenant"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtPhone" CssClass="errmsg"
                                                            runat="server" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.PHONE_INVALID)%>'
                                                            ControlToValidate="maskTxtPhone" ValidationExpression="\(\d{3}\)-\d{3}-\d{4}"
                                                            ValidationGroup="grpValdManageTenant" />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblTenantType" runat="server" Text="Institution Type" AssociatedControlID="cmbTenantType" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbTenantType" runat="server" MarkFirstMatch="true" TabIndex="3"
                                                        Width="60%" DataTextField="TenantTypeDesc" DataValueField="TenantTypeID" Style="z-index: 7002;"
                                                        OnClientSelectedIndexChanged="OnTenantTypeSelectedIndexChanged" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvTenantType" ControlToValidate="cmbTenantType"
                                                            Display="Dynamic" InitialValue="-- SELECT --" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_TENANT_TYPE_REQUIRED)%>'
                                                            ValidationGroup="grpValdManageTenant" />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblTenantDescription" runat="server" AssociatedControlID="txtTenantDescription"
                                                        Text="Description" CssClass="cptn"></asp:Label>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox Width="99%" MaxLength="255" Text='<%# Bind("TenantDesc")%>' TabIndex="4"
                                                        ID="txtTenantDescription" runat="server" />
                                                    <div class="vldx">
                                                        <asp:RegularExpressionValidator runat="server" ID="revTenantDescription" CssClass="errmsg"
                                                            ControlToValidate="txtTenantDescription" Display="Dynamic" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>'
                                                            ValidationExpression="^[\w\d\s\-\.\,\%\@\(\)\/]{3,255}$" ValidationGroup="grpValdManageTenant"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblAddress" runat="server" AssociatedControlID="txtAddress" Text="Address" CssClass="cptn"></asp:Label><span
                                                        class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox Width="99%" Text='<%# (Container is GridEditFormInsertItem) ? null : Eval("TenantAddress")%>'
                                                        MaxLength="256" TabIndex="5" ID="txtAddress" runat="server" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvAddress" CssClass="errmsg" ControlToValidate="txtAddress"
                                                            Display="Dynamic" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.ADDRESS_REQUIRED)%>'
                                                            ValidationGroup="grpValdManageTenant" />
                                                        <asp:RegularExpressionValidator runat="server" ID="revAddress" ValidationGroup="grpValdManageTenant"
                                                            Display="Dynamic" ControlToValidate="txtAddress" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTER)%>'
                                                            ValidationExpression="^[^%^</^>^?^$^@^+^!]*$" />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div id="locationControlTenant">
                                                <uc:Location ID="locationTenant" ZipTabIndex="10" CityTabIndex="11" runat="server"
                                                    ValidationGroup="grpValdManageTenant" NumberOfColumn="Three" />
                                            </div>
                                            <div class='sxro sx3co' runat="server" id="divConnectionString" style="display: inline">
                                                <div class='sxro sx3co'>
                                                    <div class='sxlb'>
                                                        <asp:Label ID="lblDBServer" runat="server" Text="Database Server" AssociatedControlID="txtDBServer" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox Width="99%" MaxLength="256" TabIndex="6" ID="txtDBServer" runat="server" />
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvDBServer" CssClass="errmsg dbserver"
                                                                ControlToValidate="txtDBServer" Display="Dynamic" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_DATABASE_SERVER_REQUIRED)%>'
                                                                ValidationGroup="grpValdManageTenant" />
                                                        </div>
                                                    </div>
                                                    <div class='sxlb'>
                                                        <asp:Label ID="lblDBName" runat="server" Text="Database Name" AssociatedControlID="txtDBName" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox Width="99%" MaxLength="256" TabIndex="7" ID="txtDBName" runat="server" />
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvDBName" CssClass="errmsg dbname"
                                                                ControlToValidate="txtDBName" Display="Dynamic" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_DATABASE_NAME_REQUIRED)%>'
                                                                ValidationGroup="grpValdManageTenant" />
                                                        </div>
                                                    </div>
                                                    <div class='sxlb'>
                                                        <asp:Label ID="lblDBUserName" runat="server" Text="Database User Name" AssociatedControlID="txtDBUserName" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox Width="99%" MaxLength="256" TabIndex="8" ID="txtDBUserName" runat="server" />
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvDBUserName" CssClass="errmsg dbusername"
                                                                ControlToValidate="txtDBUserName" Display="Dynamic" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_DATABASE_USER_NAME_REQUIRED)%>'
                                                                ValidationGroup="grpValdManageTenant" />
                                                        </div>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                                <div class='sxro sx3co'>
                                                    <div class='sxlb'>
                                                        <asp:Label ID="lblPassword" runat="server" Text="Database Password" AssociatedControlID="txtDBPassword" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <%--<infs:WclTextBox Width="99%" MaxLength="256" TabIndex="9"  ID="txtDBPassword" runat="server" />--%>
                                                        <span class="riSingle RadInput RadInput_Web20" style="width: 99%;">
                                                            <asp:TextBox Width="99%" MaxLength="256" TabIndex="9" CssClass="riTextBox riEnabled"
                                                                TextMode="Password" ID="txtDBPassword" runat="server"></asp:TextBox>
                                                        </span>
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvDBPassword" CssClass="errmsg dbpassword"
                                                                ControlToValidate="txtDBPassword" Display="Dynamic" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_DATABASE_PASSWORD_REQUIRED)%>'
                                                                ValidationGroup="grpValdManageTenant" />
                                                        </div>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                            </div>
                                            <h1 class="shdr">
                                                <%#SysXUtils.GetMessage(ResourceConst.SECURITY_TENANT_PRODUCT)%>
                                            </h1>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblProductName" runat="server" AssociatedControlID="txtProductName"
                                                        Text="Product Name" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox Width="99.5%" ID="txtProductName" TabIndex="12" MaxLength="216"
                                                        runat="server" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvProductName" ControlToValidate="txtProductName"
                                                            Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_PRODUCT_NAME_REQUIRED)%>'
                                                            ValidationGroup="grpValdManageTenant" />
                                                        <asp:RegularExpressionValidator runat="server" ID="revProductName" ControlToValidate="txtProductName"
                                                            Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\,\%\@\(\)\/]{3,216}$"
                                                            ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>'
                                                            ValidationGroup="grpValdManageTenant"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblProductDescription" runat="server" AssociatedControlID="txtProductDescription"
                                                        Text="Description" CssClass="cptn"></asp:Label>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <infs:WclTextBox Width="99.5%" MaxLength="256" TabIndex="13" ID="txtProductDescription"
                                                        runat="server" />
                                                    <div class="vldx">
                                                        <asp:RegularExpressionValidator runat="server" ID="revProductDescription" ControlToValidate="txtProductDescription"
                                                            Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>'
                                                            ValidationExpression="^[\w\d\s\-\.\,\%\@\(\)\/]{3,256}$" ValidationGroup="grpValdManageTenant"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <h1 class="shdr">
                                                <%#SysXUtils.GetMessage(ResourceConst.SECURITY_TENANT_ORGANIZATION)%>
                                            </h1>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblOrgName" runat="server" Text="Organization Name" AssociatedControlID="txtOrgName" CssClass="cptn"></asp:Label><span
                                                        class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox Width="99%" TabIndex="14" ID="txtOrgName" MaxLength="50" runat="server" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvOrgName" ControlToValidate="txtOrgName"
                                                            Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_ORGANIZATION_NAME_REQUIRED)%>'
                                                            ValidationGroup="grpValdManageTenant" />
                                                        <asp:RegularExpressionValidator runat="server" ID="revOrgName" ControlToValidate="txtOrgName"
                                                            Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.\,\%\@\(\)]{3,50}$"
                                                            ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>'
                                                            ValidationGroup="grpValdManageTenant"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblOrgAddress" runat="server" AssociatedControlID="txtOrgAddress"
                                                        Text="Address" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox Width="99%" TabIndex="15" ID="txtOrgAddress" MaxLength="216" runat="server" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvOrgAddress" ControlToValidate="txtOrgAddress"
                                                            Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.ADDRESS_REQUIRED)%>'
                                                            ValidationGroup="grpValdManageTenant" />
                                                        <asp:RegularExpressionValidator runat="server" ID="revOrgAddress" ValidationGroup="grpValdManageTenant"
                                                            Display="Dynamic" ControlToValidate="txtOrgAddress" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTER)%>'
                                                            ValidationExpression="^[^%^</^>^?^$^+^!]*$" />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblOrgPhone" runat="server" AssociatedControlID="txtOrgPhone" Text="Phone" CssClass="cptn"></asp:Label><span
                                                        class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclMaskedTextBox Width="99%" ID="txtOrgPhone" runat="server" TabIndex="16"
                                                        Mask="(###)-###-####" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator Display="Dynamic" ID="rfvOrgPhone" runat="server" CssClass="errmsg"
                                                            ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.PHONE_REQUIRED)%>' ControlToValidate="txtOrgPhone"
                                                            ValidationGroup="grpValdManageTenant"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revOrgPhone" runat="server"
                                                            CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.PHONE_INVALID)%>'
                                                            ControlToValidate="txtOrgPhone" ValidationExpression="\(\d{3}\)-\d{3}-\d{4}"
                                                            ValidationGroup="grpValdManageTenant" />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblOrgDescription" runat="server" AssociatedControlID="txtOrgDescription"
                                                        Text="Description" CssClass="cptn"></asp:Label>
                                                </div>
                                                <div class='sxlm m3spn'>
                                                    <infs:WclTextBox Width="99.5%" TabIndex="17" ID="txtOrgDescription" MaxLength="256"
                                                        runat="server" />
                                                    <div class="vldx">
                                                        <asp:RegularExpressionValidator runat="server" ID="revOrgDescription" ControlToValidate="txtOrgDescription"
                                                            Display="Dynamic" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>'
                                                            ValidationExpression="^[\w\d\s\-\.\,\%\@\(\)\/]{3,256}$" ValidationGroup="grpValdManageTenant"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div id="locationControlOrganization">
                                                <%--                                                <uc:Location ID="locationOrganization" runat="server" ZipTabIndex="14" CityTabIndex="15"
                                                    NumberOfColumn="Three" ValidationGroup="grpValdManageTenant" />--%>
                                            </div>
                                            <div>
                                                <div class="msgbox">
                                                    <asp:Label ID="lblprefixmessage" runat="server"></asp:Label>
                                                </div>
                                                <h1 class="shdr">
                                                    <%#SysXUtils.GetMessage(ResourceConst.SECURITY_ORGANIZATION_USERNAME_PREFIX)%>
                                                </h1>
                                                <infs:WclGrid runat="server" ID="grdOrganizationUserNamePrefix" AllowPaging="True"
                                                    PageSize="10" AutoGenerateColumns="False" AllowSorting="True" GridLines="Both"
                                                    GridViewMode="AutoAddOnly" NonExportingColumns="EditCommandColumn, DeleteColumn"
                                                    ShowAllExportButtons="false" OnNeedDataSource="grdOrganizationUserNamePrefix_NeedDataSource"
                                                    OnInsertCommand="grdOrganizationUserNamePrefix_InsertCommand" OnUpdateCommand="grdOrganizationUserNamePrefix_UpdateCommand"
                                                    OnDeleteCommand="grdOrganizationUserNamePrefix_DeleteCommand">
                                                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrganizationUserNamePrefixID"
                                                        AllowFilteringByColumn="true">
                                                        <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New prefix"
                                                            ShowExportToCsvButton="true" ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                                                        <Columns>
                                                            <telerik:GridBoundColumn HeaderText="User Name Prefix" DataField="UserNamePrefix">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderText="Decription" DataField="Description">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                                                                <HeaderStyle CssClass="tplcohdr" />
                                                                <ItemStyle CssClass="MyImageButton" />
                                                            </telerik:GridEditCommandColumn>
                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Institution?"
                                                                Text="Delete" UniqueName="DeleteColumn">
                                                                <HeaderStyle CssClass="tplcohdr" />
                                                                <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                                                            </telerik:GridButtonColumn>
                                                        </Columns>
                                                        <EditFormSettings EditFormType="Template">
                                                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                            </EditColumn>
                                                            <FormTemplate>
                                                                <div class="section">
                                                                    <h1 class="mhdr">
                                                                        <asp:Label ID="lblNewHeading" Text='<%#(Container is GridEditFormInsertItem) ? "Add New Prefix" : "Edit New Prefix"%>'
                                                                            runat="server"></asp:Label></h1>
                                                                    <div class="content">
                                                                        <div class="sxform auto">
                                                                            <div class="msgbox">
                                                                                <asp:Label ID="lblprefixNewErrorMessage" runat="server"></asp:Label>
                                                                            </div>
                                                                            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlMNew">
                                                                                <div class='sxro sx2co'>
                                                                                    <div class='sxlb'>
                                                                                        <asp:Label ID="lblName" runat="server" AssociatedControlID="txtPrefixName" Text="Prefix Name" CssClass="cptn"></asp:Label><span
                                                                                            class="reqd">*</span>
                                                                                    </div>
                                                                                    <div class='sxlm'>
                                                                                        <infs:WclTextBox Width="99%" MaxLength="10" TabIndex="17" ID="txtPrefixName" runat="server"
                                                                                            Text='<%# (Container is GridEditFormInsertItem) ? null : Eval("UserNamePrefix")%>'
                                                                                            onblur="IsPrefixNameExists(this)" />
                                                                                        <div class="vldx">
                                                                                            <asp:RequiredFieldValidator runat="server" CssClass="errmsg" ID="rfvPrefixName" ControlToValidate="txtPrefixName"
                                                                                                Display="Dynamic" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_PREFIX_REQUIRED)%>'
                                                                                                ValidationGroup="prefix" />
                                                                                            <asp:RegularExpressionValidator runat="server" CssClass="errmsg" ID="regtxtprefixName"
                                                                                                ControlToValidate="txtPrefixName" Display="Dynamic" ValidationExpression="^[\w\d\s\-\.\,\%\(\)\/]{1,50}$"
                                                                                                ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTERS_SHOULD_BE_MORE_THAN_TWO_CHARACTERS)%>'
                                                                                                ValidationGroup="prefix"></asp:RegularExpressionValidator>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class='sxlb'>
                                                                                        <asp:Label ID="lblAddress" runat="server" AssociatedControlID="txtdescription" Text="Description" CssClass="cptn"></asp:Label><span
                                                                                            class="reqd">*</span>
                                                                                    </div>
                                                                                    <div class='sxlm'>
                                                                                        <infs:WclTextBox Width="99%" Text='<%# (Container is GridEditFormInsertItem) ? null : Eval("Description")%>'
                                                                                            MaxLength="256" TabIndex="18" ID="txtdescription" runat="server" />
                                                                                        <div class="vldx">
                                                                                            <asp:RequiredFieldValidator runat="server" ID="rfvprefixdesc" CssClass="errmsg" ControlToValidate="txtdescription"
                                                                                                Display="Dynamic" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.PREFIX_DESCRIPTION_REQUIRED)%>'
                                                                                                ValidationGroup="prefix" />
                                                                                            <asp:RegularExpressionValidator runat="server" ID="revdescription" ValidationGroup="prefix"
                                                                                                Display="Dynamic" ControlToValidate="txtdescription" CssClass="errmsg" ErrorMessage='<%#SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTER)%>'
                                                                                                ValidationExpression="^[^%^</^>^?^$^@^+^!]*$" />
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class='sxroend'>
                                                                                    </div>
                                                                                </div>
                                                                            </asp:Panel>
                                                                        </div>
                                                                        <infsu:CommandBar ID="fsucCmdBarNew" runat="server" GridMode="true" ValidationGroup="prefix" GridInsertText="Save" GridUpdateText="Save"
                                                                            DefaultPanel="pnlMNew" />
                                                                    </div>
                                                                </div>
                                                            </FormTemplate>
                                                        </EditFormSettings>
                                                    </MasterTableView>
                                                </infs:WclGrid>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarManageTenant" runat="server" GridMode="true" TabIndexAt="18" GridInsertText="Save" GridUpdateText="Save"
                                        DefaultPanel="pnlMTenant" ValidationGroup="grpValdManageTenant" />
                                </div>
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                    <DetailTables>
                        <telerik:GridTableView runat="server" AllowFilteringByColumn="false" CommandItemDisplay="None"
                            DataKeyNames="TenantProductId" AllowPaging="false" Caption="<h6>Institution's Product</h6>"
                            Width="100%">
                            <ParentTableRelation>
                                <telerik:GridRelationFields DetailKeyField="TenantID" MasterKeyField="TenantID" />
                            </ParentTableRelation>
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="Name" HeaderButtonType="TextButton" HeaderText="Product Name"
                                    ReadOnly="true" SortExpression="Name" UniqueName="Name" AllowSorting="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Description" HeaderButtonType="TextButton" HeaderText="Description"
                                    ReadOnly="true" SortExpression="Description" UniqueName="Description" AllowSorting="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageFeatures">
                                    <ItemTemplate>
                                        <a id="ancProFeature" runat="server">Manage Features</a>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageRoles">
                                    <ItemTemplate>
                                        <a runat="server" id="ancManageRole">Manage Roles</a>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </telerik:GridTableView>
                        <telerik:GridTableView runat="server" CommandItemDisplay="None" DataKeyNames="OrganizationID"
                            AllowPaging="false" AllowFilteringByColumn="false" Caption="<h6>Institution's Organization</h6>"
                            Width="100%">
                            <ParentTableRelation>
                                <telerik:GridRelationFields DetailKeyField="TenantID" MasterKeyField="TenantID" />
                            </ParentTableRelation>
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                            <Columns>
                                <telerik:GridBoundColumn DataField="OrganizationName" HeaderButtonType="TextButton"
                                    HeaderText="Organization Name" ReadOnly="true" SortExpression="OrganizationName"
                                    UniqueName="OrganizationName" AllowSorting="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="OrganizationDesc" HeaderButtonType="TextButton"
                                    HeaderText="Description" ReadOnly="true" SortExpression="OrganizationDesc" UniqueName="OrganizationDesc"
                                    AllowSorting="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Address.Address1" FilterControlAltText="Filter Address column"
                                    HeaderText="Address" SortExpression="Address.Address1" UniqueName="OrganizationAddress"
                                    AllowSorting="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Address.ZipCode.City.CityName" FilterControlAltText="Filter city column"
                                    HeaderText="City" SortExpression="Address.ZipCode.City.CityName" UniqueName="OrganizationCity"
                                    AllowSorting="false" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Address.ZipCode.City.State.StateName" HeaderText="State"
                                    UniqueName="OrganizationState" AllowSorting="false" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridMaskedColumn DataField="Address.ZipCode.ZipCode1" HeaderText="Zip Code"
                                    UniqueName="OrganizationZipCode" AllowSorting="false" Mask="#####" FilterControlWidth="50px"
                                    Visible="false">
                                </telerik:GridMaskedColumn>
                                <telerik:GridMaskedColumn HeaderText="Phone" UniqueName="OrganizationPhone" AllowSorting="false"
                                    Mask="(###)-###-####">
                                </telerik:GridMaskedColumn>
                                <telerik:GridTemplateColumn HeaderText="User Prefix Name" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblprifixName" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn AllowFiltering="false">
                                    <ItemTemplate>
                                        <a runat="server" id="ancManageUser">Manage Users</a>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <%--<telerik:GridTemplateColumn AllowFiltering="false">
                                    <ItemTemplate>
                                        <a runat="server" id="ancManageProgram">Manage Programs</a>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>--%>
                            </Columns>
                        </telerik:GridTableView>
                    </DetailTables>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
    </div>
</div>
