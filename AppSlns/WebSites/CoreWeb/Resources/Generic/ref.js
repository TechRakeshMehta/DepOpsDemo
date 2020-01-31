///<reference name="INTERSOFT.WEB.UI.Resources.IWeb.Ref.js" assembly="INTERSOFT.WEB.UI" />

//----------------------------------------------------------
// Copyright (C) Copyright Intersoft Data Labs, Inc. All rights reserved.
//----------------------------------------------------------
// File: ref.js
// Version: 2012.11.2.1100
// Purpose:  Contains dummy code for Intellisense support for module scripts
//
// Current app.js version: 2012.11.2.1100
// Current page.js version: 2012.11.2.1100

/////////////////////////////////////////////////////////////////
//APP.JS DOC

var Page = {
    showProgress: function (msg) {
        /// <summary>Shows page progress bar</summary>
        /// <param name="msg" type="String">Message to display on progress bar</param>
    },
    hideProgress: function () {
        /// <summary>Hides page progress bar</summary>
    }

};

IWeb.UI.Page.prototype.app = {};
/// <field name="app" type="Object" integer="true" static="true">
///    describes a enumeration value
/// </field>

$page.app = { leftPanel: new IWeb.UI.AjaxRegion(), menu: new IWeb.UI.AjaxRegion(), modFrame: new IWeb.UI.Frame(),
    setTitle: function (title) {
        /// <summary>Sets the title of the application window.</summary>
        /// <param name="title" type="String">A string that is used as a title. If not provided the title will be auto-generated based on the page context</param>
    }
}

$page.app.leftPanel.collapse = function () {
    /// <summary>Collapses the left panel</summary>
}

$page.app.leftPanel.expand = function () {
    /// <summary>Expands the left panel</summary>
}

$page.app.leftPanel.deselect = function () {
    /// <summary>Deselects the selected item of a panel bar within left panel</summary>
}
/////////////////////////////////////////////////////////////////
//PAGE.JS DOC


IWeb.UI.Page.prototype.msgTypes = {};
$page.msgTypes = {
    'ERROR': 'error',
    'INFO': 'info',
    'SUCCESS': 'sucs'
};

IWeb.UI.Page.prototype.showMessage = function (msg, msgtype) {
    /// <summary>Shows message box on the page</summary>
    /// <param name="msg" type="String">Message to be displayed</param>
    /// <param name="msgtype" type="$page.msgTypes">Type of message box</param>
}
