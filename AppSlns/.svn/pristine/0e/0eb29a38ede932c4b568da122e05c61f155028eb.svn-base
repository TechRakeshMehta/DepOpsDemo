<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.Student.Views.StudentRegistration" Codebehind="StudentRegistration.ascx.cs" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div class="section">
    <h1 class="mhdr">
        Applicant Registration Form
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div id="poster" runat="server" />
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlName1">
                <h2 class="shdr">
                    Account Information</h2>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        Username<span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtUser" EmptyMessage="Enter Username" ToolTip="Enter Username">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtUser"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage='Please enter a username!' ValidationGroup="grpValdManageDepartment" />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        Password<span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="WclTextBox1" EmptyMessage="Enter Passoword" ToolTip="Password tooltip">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="WclTextBox1"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage='Please enter a password!' ValidationGroup="grpValdManageDepartment" />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        Confirm Password<span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="WclTextBox2" EmptyMessage="Re-enter Password">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="WclTextBox2"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage='Please re-enter password!'
                                ValidationGroup="grpValdManageDepartment" />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        Primary Email<span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="WclTextBox3" EmptyMessage="Enter your Email ID">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="WclTextBox3"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage='Please enter your email!' ValidationGroup="grpValdManageDepartment" />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        Confirm Primary Email<span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="WclTextBox4" EmptyMessage="Re-enter Email">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="WclTextBox4"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage='Please re-enter your email!'
                                ValidationGroup="grpValdManageDepartment" />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        Secondary Email
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="WclTextBox5" EmptyMessage="Enter Alternate Email ID">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <h2 class="shdr" style="padding-top: 20px;">
                    Personal Information</h2>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        Applicant ID
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="WclTextBox6">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        Program Study
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="WclTextBox7">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        Grade Level
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="WclTextBox8">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        First Name
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="WclTextBox9">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        Middle Name
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="WclTextBox10">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        Last Name
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="WclTextBox11">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        Social Security Number
                    </div>
                    <div class='sxlm'>
                        <%--<infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="ntxtName1" runat="server"
                            InvalidStyleDuration="100" EmptyMessage="Enter your SSN">
                            <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="-" GroupSizes="3" />
                        </infs:WclNumericTextBox>--%>
                        <infs:WclMaskedTextBox Mask="###-##-####" runat="server">
                        </infs:WclMaskedTextBox>
                    </div>
                    <div class='sxlb'>
                        Date of Birth
                    </div>
                    <div class='sxlm'>
                        <infs:WclDatePicker ID="dpkrName1" runat="server" AutoPostBack="true" DateInput-EmptyMessage="Select a date">
                            <%--<Calendar>
                                <SpecialDays>
                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                </SpecialDays>
                            </Calendar>--%>
                        </infs:WclDatePicker>
                    </div>
                    <div class='sxlb'>
                        Gender
                    </div>
                    <div class='sxlm'>
                        <infs:WclButton runat="server" ID="rdbName1" ToggleType="Radio" ButtonType="ToggleButton"
                            GroupName="Group1" Text="Male" />
                        <infs:WclButton runat="server" ID="WclButton1" ToggleType="Radio" ButtonType="ToggleButton"
                            GroupName="Group1" Text="Female" />
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        Address 1
                    </div>
                    <div class='sxlm m3spn'>
                        <infs:WclTextBox runat="server" ID="WclTextBox13">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        Address 2
                    </div>
                    <div class='sxlm m2spn'>
                        <infs:WclTextBox runat="server" ID="WclTextBox14">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        City
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="WclTextBox15">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        State
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="WclTextBox16">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        Zip / Postal
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="WclTextBox17">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        Telephone
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="WclTextBox18">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <infsu:CommandBar ID="fsucCmdBar1" runat="server" DisplayButtons="Submit,Cancel" AutoPostbackButtons="Submit"
            SubmitButtonText="Register" DefaultPanel="pnlName1" ButtonPosition="Center" OnSubmitClick="btnPayment_Click"/>
        <!-- Move the following line to register command bar, if not registered already -->
        <%-- <%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/CommonControls/UserControl/CommandBar.ascx" %> --%>
    </div>
</div>

<div style="font-size: 0.8em;position: absolute;right: 83px;top: 123px;width: 50%;padding:10px; color:navy !important;  background-color: lavender;border-radius: 20px 20px 20px 20px;">
<h2 style="margin:10px 0;margin-bottom:5px">Username</h2>
Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas non lacus et elit dignissim aliquam. Nullam et dui eu lorem posuere lobortis. Nullam arcu sem, fringilla eu posuere sed, pharetra vel dui. 
<h2 style="margin:10px 0;margin-bottom:5px">Password</h2>
Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas non lacus et elit dignissim aliquam. Nullam et dui eu lorem posuere lobortis. Nullam arcu sem, fringilla eu posuere sed, pharetra vel dui. 
<h2 style="margin:10px 0;margin-bottom:5px">Primary Email</h2>
Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas non lacus et elit dignissim aliquam. Nullam et dui eu lorem posuere lobortis. Nullam arcu sem, fringilla eu posuere sed, pharetra vel dui.    
</div>

<telerik:RadToolTipManager ID="RadToolTipManager1" runat="server" Skin="Web20" AutoTooltipify="true">

</telerik:RadToolTipManager>
