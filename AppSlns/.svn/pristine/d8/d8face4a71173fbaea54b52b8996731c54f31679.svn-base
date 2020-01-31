<%-- 
Copyright 2011 Intersoft Data Labs.
All rights are reserved.  Reproduction or transmission in whole or in part, in
any form or by any means, electronic, mechanical or otherwise, is prohibited
without the prior written consent of the copyright owner.

Filename:  CommandBar.ascx
Purpose:  Creates a master layout for the top page of the application

Revisions:
Author           Date               Comment
------           ----------         -------------------------------------------------
Ashish Daniel    27-Mar-2012 1230   Added place holder named 'placeExtraButtons' for extrabuttons

--%>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Shell.Views.CommandBar" Codebehind="CommandBar.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Panel class="sxcbar" runat="server" ID="pnlCollection">
    <div class="sxcmds">
        <infs:WclButton ID="btnSave" runat="server" Text="Save" Visible="false" AutoPostBack="false">
            <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                PrimaryIconWidth="14" />
        </infs:WclButton>
        <infs:WclButton ID="btnGrd" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? GridInsertText : GridUpdateText %>'
            CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'
            Visible="false" AutoPostBack="true">
            <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                PrimaryIconWidth="14" />
        </infs:WclButton>
        <infs:WclButton ID="btnTreeGrd" runat="server" Text='<%# (Container is TreeListEditFormInsertItem) ? GridInsertText : GridUpdateText %>'
            CommandName='<%# (Container is TreeListEditFormInsertItem) ? "PerformInsert" : "Update" %>'
            Visible="false" AutoPostBack="true">
            <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                PrimaryIconWidth="14" />
        </infs:WclButton>
        <infs:WclButton ID="btnSubmit" runat="server" Text="Submit" Visible="false" AutoPostBack="false">
            <Icon PrimaryIconCssClass="rbNext" PrimaryIconLeft="4" PrimaryIconTop="5" PrimaryIconHeight="14"
                PrimaryIconWidth="14" />
        </infs:WclButton>
        <infs:WclButton ID="btnExtra" runat="server" Text="" Visible="false" AutoPostBack="false">
            <Icon PrimaryIconCssClass="" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                PrimaryIconWidth="14" />
        </infs:WclButton>
        <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" Visible="false" AutoPostBack="false"
            CausesValidation="false">
            <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                PrimaryIconWidth="14" />
        </infs:WclButton>
        <infs:WclButton ID="btnClear" runat="server" Text="Clear" Visible="false" AutoPostBack="false">
            <Icon PrimaryIconCssClass="rbRemove" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                PrimaryIconWidth="14" />
        </infs:WclButton>
        <asp:PlaceHolder runat="server" ID="placeExtraButtons"></asp:PlaceHolder>
    </div>
</asp:Panel>
