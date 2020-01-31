#region ... [FileLog] ...

// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.

// Filename:  CommandBar.cs
// Purpose:  Represents a class behind CommandBar UserControl

// Revisions:
// Author           Date                Comment
// ------           ----------          -------------------------------------------------
// Ashish Daniel    12-Dec-2012 0930   - File updated / cleaned up
//
#endregion

#region Used Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTERSOFT.WEB.UI.WebControls;
using Telerik.Web.UI;
using System.ComponentModel;
using INTSOF.Utils;
using Microsoft.Practices.ObjectBuilder;
using System.Configuration;
#endregion

namespace CoreWeb.Shell.Views
{
    public partial class CommandBar : System.Web.UI.UserControl, ICommandBarView
    {

        #region Private variables

        //Presenter
        private CommandBarPresenter _presenter = new CommandBarPresenter();

        //Extra buttons related
        private List<WclButton> _extraButtons;

        //Mode related
        private bool _grdmode = false;
        private bool _treemode = false;

        //====================
        //Appearance related        
        private bool _lnkbuttons = true;
        private short _tabindx = 0;

        //Button Icons
        private string _xtraico = string.Empty;
        private string _cancico = "rbCancel";
        private string _subico = "rbNext";
        private string _clrico = "icnreset";
        private string _saveico = "rbSave";

        //Button Texts                
        private string _xtrabtntxt = "Misc.";
        private string _cancbtntxt = "Cancel";
        private string _subbtntxt = "Submit";
        private string _clrbtntxt = "Reset";
        private string _savebtntxt = "Save";
        private string _grdinstxt = "Insert";
        private string _grdupdtxt = "Update";

        private string _btnSkin = string.Empty;
        private bool _useAutoSkinMode = true;
        private RadButtonType _buttonType = RadButtonType.LinkButton;

        //====================
        //Behavior related

        //Validation group name for default button
        private string _validgp = "";

        //Default panel name
        private string _panel = "";

        //Causes Validation on Cancel 
        private bool _cancvalid = false;

        //Default panel button
        private CommandBarButtons _defpanelbut = CommandBarButtons.Submit;

        //Command bar position
        private CommandBarButtonsPosition _btnposition = CommandBarButtonsPosition.Left;

        //Buttons to display
        private CommandBarButtons _buttons = CommandBarButtons.None;

        //Buttons enabled for post backs
        private CommandBarButtons _postbacks = CommandBarButtons.None;

        #endregion

        #region Public fields

        //Control Events
        public event EventHandler SaveClick;
        public event EventHandler CancelClick;
        public event EventHandler ClearClick;
        public event EventHandler ExtraClick;
        public event EventHandler SubmitClick;

        #endregion

        #region Public Properties & Attributs

        //Button click events
        public string OnSaveClientClick { get; set; }
        public string OnSubmitClientClick { get; set; }
        public string OnClearClientClick { get; set; }
        public string OnCancelClientClick { get; set; }
        public string OnExtraClientClick { get; set; }

        //Command bar core buttons
        public WclButton SaveButton
        {
            get
            {
                if (TreeListMode) return btnTreeGrd;
                else if (GridMode) return btnGrd;
                else return btnSave;

            }
        }
        public WclButton CancelButton { get { return btnCancel; } }
        public WclButton ClearButton { get { return btnClear; } }
        public WclButton SubmitButton { get { return btnSubmit; } }
        public WclButton ExtraButton { get { return btnExtra; } }

        //====================
        //Appearance related

        public bool ShowAsLinkButtons
        {
            get { return _lnkbuttons; }
            set { _lnkbuttons = value; }
        }
        public CommandBarButtonsPosition ButtonPosition
        {
            get { return _btnposition; }
            set { _btnposition = value; }
        }

        public RadButtonType ButtonType
        {
            get { return _buttonType; }
            set { _buttonType = value; }
        }

        //Button Icons
        public string ExtraButtonIconClass { get { return _xtraico; } set { _xtraico = value; } }
        public string CancelButtonIconClass
        {
            get
            {
                return _cancico;
            }
            set
            {
                _cancico = value;
            }
        }
        public string ClearButtonIconClass
        {
            get
            {
                return _clrico;
            }
            set
            {
                _clrico = value;
            }
        }
        public string SubmitButtonIconClass
        {
            get
            {
                return _subico;
            }
            set
            {
                _subico = value;
            }
        }
        public string SaveButtonIconClass
        {
            get
            {
                return _saveico;
            }
            set
            {
                _saveico = value;
            }
        }

        //Button Texts
        public string CancelButtonText
        {
            get
            {
                return _cancbtntxt;
            }
            set
            {
                _cancbtntxt = value;
            }
        }
        public string ClearButtonText
        {
            get { return _clrbtntxt; }
            set { _clrbtntxt = value; }
        }
        public string SubmitButtonText
        {
            get { return _subbtntxt; }
            set { _subbtntxt = value; }
        }
        public string SaveButtonText
        {
            get { return _savebtntxt; }
            set { _savebtntxt = value; }
        }
        public string ExtraButtonText
        {
            get { return _xtrabtntxt; }
            set { _xtrabtntxt = value; }
        }
        public string GridInsertText
        {
            get { return _grdinstxt; }
            set { _grdinstxt = value; }
        }
        public string GridUpdateText
        {
            get { return _grdupdtxt; }
            set { _grdupdtxt = value; }
        }

        /// <summary>
        /// Gets or Sets a string value representing the 'Text' of the 'Save Button'.
        /// Note: This property is the shortcut of 'SaveButton.Text' and works only if 
        /// save button is flagged in DisplayButtons enum.
        /// </summary>
        public String ButtonText
        {
            get
            {
                return btnSave.Text;
            }
            set
            {
                btnSave.Text = value;
            }
        }

        //====================
        //Mode related

        //Grid mode if command bar is in grid's edit form
        public bool GridMode
        {
            get { return _grdmode; }
            set { _grdmode = value; }
        }

        //TreeList mode if command bar is in TreeList's edit form
        public bool TreeListMode
        {
            get { return _treemode; }
            set { _treemode = value; }
        }

        public CommandBarButtons EditModeButtons { get; set; }
        public CommandBarButtons InsertModeButtons { get; set; }

        //====================
        //Behavior related

        //List of buttons to display
        public CommandBarButtons DisplayButtons
        {
            get { return _buttons; }
            set { _buttons = value; }
        }

        //Buttons enabled for post backs
        public CommandBarButtons AutoPostbackButtons
        {
            get { return _postbacks; }
            set { _postbacks = value; }
        }

        //Default Panel name
        public string DefaultPanel { get { return _panel; } set { _panel = value; } }

        //Default panel button which can be one of Submit, Save or Extra
        public CommandBarButtons DefaultPanelButton { get { return _defpanelbut; } set { _defpanelbut = value; } }

        public bool CauseValidationOnCancel { get { return _cancvalid; } set { _cancvalid = value; } }
        public short TabIndexAt { get { return _tabindx; } set { _tabindx = value; } }
        public string ValidationGroup { get { return _validgp; } set { _validgp = value; } }


        //====================
        //Other Properties

        /// <summary>
        /// A collection of extra buttons in the command bar
        /// </summary>
        [
             PersistenceMode(PersistenceMode.InnerProperty),
             Category("Behavior"),
             DefaultValue(null),
             Description("A collection of extra buttons in the command bar")
        ]
        public List<WclButton> ExtraCommandButtons
        {
            get
            {
                if (_extraButtons == null)
                {
                    _extraButtons = new List<WclButton>();
                }
                return _extraButtons;
            }
        }


        public CommandBarPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public string ButtonSkin
        {
            get { return _btnSkin; }
            set { _btnSkin = value; }
        }

        public Boolean UseAutoSkinMode
        {
            get { return _useAutoSkinMode; }
            set { _useAutoSkinMode = value; }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Associates buttons with the panel using 'DefaultPanelButton' Property
        /// </summary>
        /// <param name="p">Panel </param>
        public void SetDefaultButton(Panel p)
        {

            if (p != null)
            {
                if (string.IsNullOrWhiteSpace(p.DefaultButton))
                {
                    if (GridMode)
                    {
                        p.Attributes["onkeypress"] = "javascript:return WebForm_FireDefaultButton(event, '" + btnGrd.ClientID + "')";
                        return;
                    }
                    if (TreeListMode)
                    {
                        p.Attributes["onkeypress"] = "javascript:return WebForm_FireDefaultButton(event, '" + btnTreeGrd.ClientID + "')";
                        return;
                    }
                    if (DefaultPanelButton.HasFlag(CommandBarButtons.Submit))
                    {
                        p.Attributes["onkeypress"] = "javascript:return WebForm_FireDefaultButton(event, '" + btnSubmit.ClientID + "')";
                        return;
                    }
                    if (DefaultPanelButton.HasFlag(CommandBarButtons.Save))
                    {
                        p.Attributes["onkeypress"] = "javascript:return WebForm_FireDefaultButton(event, '" + btnSave.ClientID + "')";
                        return;
                    }
                    if (DefaultPanelButton.HasFlag(CommandBarButtons.Extra))
                    {
                        p.Attributes["onkeypress"] = "javascript:return WebForm_FireDefaultButton(event, '" + btnExtra.ClientID + "')";
                        return;
                    }
                    if (DefaultPanelButton.HasFlag(CommandBarButtons.Clear))
                    {
                        p.Attributes["onkeypress"] = "javascript:return WebForm_FireDefaultButton(event, '" + btnClear.ClientID + "')";
                    }
                }
            }
        }

        /// <summary>
        /// Sets default button for the panel whose ID is set through 'DefaultPanel' property
        /// </summary>
        private void SetDefaultButton()
        {
            if (!string.IsNullOrWhiteSpace(DefaultPanel))
            {
                Panel p = this.Parent.FindControl(DefaultPanel) as Panel;
                SetDefaultButton(p);
            }
        }

        /// <summary>
        /// Sets properties of the buttons being shown in the command bar
        /// </summary>
        private void SetProperties()
        {
            short indx = TabIndexAt;

            #region This part of code is deprecated
            //Setting button style to Link style for all the buttons
            if (ShowAsLinkButtons)
            {
                btnSave.ButtonType = Telerik.Web.UI.RadButtonType.LinkButton;
                btnGrd.ButtonType = Telerik.Web.UI.RadButtonType.LinkButton;
                btnTreeGrd.ButtonType = Telerik.Web.UI.RadButtonType.LinkButton;
                btnSubmit.ButtonType = Telerik.Web.UI.RadButtonType.LinkButton;
                btnExtra.ButtonType = Telerik.Web.UI.RadButtonType.LinkButton;
                btnCancel.ButtonType = Telerik.Web.UI.RadButtonType.LinkButton;
                btnClear.ButtonType = Telerik.Web.UI.RadButtonType.LinkButton;
            }
            #endregion

            //Setting button type for each buttons

            btnSave.ButtonType = ButtonType;
            btnGrd.ButtonType = ButtonType;
            btnTreeGrd.ButtonType = ButtonType;
            btnSubmit.ButtonType = ButtonType;
            btnExtra.ButtonType = ButtonType;
            btnCancel.ButtonType = ButtonType;
            btnClear.ButtonType = ButtonType;

            foreach (WclButton button in ExtraCommandButtons)
            {
                button.ButtonType = ButtonType;
            }

            //----------------------
            //Save Button Settings

            //Setting visibility & validation group
            if (DisplayButtons.HasFlag(CommandBarButtons.Save))
            {
                if (GridMode)
                {
                    btnGrd.Visible = true;
                    if (!string.IsNullOrWhiteSpace(ValidationGroup)) btnGrd.ValidationGroup = this.ValidationGroup;
                }
                else if (TreeListMode)
                {
                    btnTreeGrd.Visible = true;
                    if (!string.IsNullOrWhiteSpace(ValidationGroup)) btnTreeGrd.ValidationGroup = this.ValidationGroup;
                }
                else
                {
                    btnSave.Visible = true;
                    if (!string.IsNullOrWhiteSpace(ValidationGroup)) btnSave.ValidationGroup = this.ValidationGroup;
                }
            }

            //Setting Auto Postback behaviour
            if (AutoPostbackButtons.HasFlag(CommandBarButtons.Save))
            {
                btnSave.AutoPostBack = true;
                btnGrd.AutoPostBack = true;
                btnTreeGrd.AutoPostBack = true;
            }

            //Setting appearance
            if (!GridMode && !TreeListMode) SaveButton.Text = SaveButtonText;
            SaveButton.Icon.PrimaryIconCssClass = SaveButtonIconClass;

            //Adding server/client click event handler
            SaveButton.Click += new EventHandler(btnSave_Click);
            if (!string.IsNullOrWhiteSpace(OnSaveClientClick)) SaveButton.OnClientClicked = OnSaveClientClick;

            //Setting tab index manually
            if (indx > 0)
            {
                SaveButton.TabIndex = indx;
                indx++;
            }


            //----------------------
            //Submit Button Settings

            //Setting visibility & validation group
            if (DisplayButtons.HasFlag(CommandBarButtons.Submit))
            {
                btnSubmit.Visible = true;
                if (DefaultPanelButton.HasFlag(CommandBarButtons.Submit) && !string.IsNullOrWhiteSpace(ValidationGroup))
                {
                    btnSubmit.ValidationGroup = this.ValidationGroup;
                }
            }

            //Setting Appearance
            SubmitButton.Text = SubmitButtonText;
            SubmitButton.Icon.PrimaryIconCssClass = SubmitButtonIconClass;

            //Adding server/client click event handler
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
            if (!string.IsNullOrWhiteSpace(OnSubmitClientClick)) SubmitButton.OnClientClicked = OnSubmitClientClick;

            //Setting Auto Postback behaviour
            if (AutoPostbackButtons.HasFlag(CommandBarButtons.Submit)) btnSubmit.AutoPostBack = true;

            //Setting tab index manually
            if (indx > 0)
            {
                SubmitButton.TabIndex = indx;
                indx++;
            }


            //----------------------
            //Extra Button Settings

            //Setting visibility & validation group
            if (DisplayButtons.HasFlag(CommandBarButtons.Extra))
            {
                btnExtra.Visible = true;
                if (DefaultPanelButton.HasFlag(CommandBarButtons.Extra) && !string.IsNullOrWhiteSpace(ValidationGroup))
                {
                    btnExtra.ValidationGroup = this.ValidationGroup;
                }
            }

            //Setting Appearance
            btnExtra.Text = ExtraButtonText;
            btnExtra.Icon.PrimaryIconCssClass = ExtraButtonIconClass;

            //Adding server/client click event handler
            btnExtra.Click += new EventHandler(btnExtra_Click);
            if (!string.IsNullOrWhiteSpace(OnExtraClientClick)) ExtraButton.OnClientClicked = OnExtraClientClick;

            //Setting Auto Postback behaviour
            if (AutoPostbackButtons.HasFlag(CommandBarButtons.Extra)) btnExtra.AutoPostBack = true;

            //Setting tab index manually
            if (indx > 0)
            {
                ExtraButton.TabIndex = indx;
                indx++;
            }


            //----------------------
            //Cancel Button Settings

            if (DisplayButtons.HasFlag(CommandBarButtons.Cancel)) btnCancel.Visible = true;
            CancelButton.CausesValidation = CauseValidationOnCancel;

            //Setting Appearance
            CancelButton.Text = CancelButtonText;
            CancelButton.Icon.PrimaryIconCssClass = CancelButtonIconClass;

            //Adding server/client click event handler
            btnCancel.Click += new EventHandler(btnCancel_Click);
            if (!string.IsNullOrWhiteSpace(OnCancelClientClick)) CancelButton.OnClientClicked = OnCancelClientClick;

            //Setting Auto Postback behaviour
            if (AutoPostbackButtons.HasFlag(CommandBarButtons.Cancel)) btnCancel.AutoPostBack = true;

            //Setting tab index manually
            if (indx > 0)
            {
                CancelButton.TabIndex = indx;
                indx++;
            }


            //----------------------
            //Clear Button Settings

            if (DisplayButtons.HasFlag(CommandBarButtons.Clear)) btnClear.Visible = true;

            //Setting Appearance
            ClearButton.Text = ClearButtonText;
            ClearButton.Icon.PrimaryIconCssClass = ClearButtonIconClass;

            //Adding server/client click event handler
            btnClear.Click += new EventHandler(btnClear_Click);
            if (!string.IsNullOrWhiteSpace(OnClearClientClick)) ClearButton.OnClientClicked = OnClearClientClick;

            //Setting Auto Postback behaviour
            if (AutoPostbackButtons.HasFlag(CommandBarButtons.Clear)) btnClear.AutoPostBack = true;

            //Setting tab index manually
            if (indx > 0)
            {
                ClearButton.TabIndex = indx;
                indx++;
            }

            //----------------------
            //Settings position of buttons in command bar
            switch (ButtonPosition)
            {
                case CommandBarButtonsPosition.Center:
                    pnlCollection.CssClass = "sxcbar c";
                    break;
                case CommandBarButtonsPosition.Right:
                    pnlCollection.CssClass = "sxcbar r";
                    break;
                case CommandBarButtonsPosition.Left:
                    pnlCollection.CssClass = "sxcbar l";
                    break;
            }
            if (!UseAutoSkinMode)
            {
                btnSubmit.Skin = ButtonSkin;
                btnSubmit.AutoSkinMode = UseAutoSkinMode;
                btnExtra.Skin = ButtonSkin;
                btnExtra.AutoSkinMode = UseAutoSkinMode;
                btnCancel.Skin = ButtonSkin;
                btnCancel.AutoSkinMode = UseAutoSkinMode;
                btnClear.Skin = ButtonSkin;
                btnClear.AutoSkinMode = UseAutoSkinMode;
                btnSave.Skin = ButtonSkin;
                btnSave.AutoSkinMode = UseAutoSkinMode;
                btnGrd.Skin = ButtonSkin;
                btnGrd.AutoSkinMode = UseAutoSkinMode;
                btnTreeGrd.Skin = ButtonSkin;
                btnTreeGrd.AutoSkinMode = UseAutoSkinMode;
            }

        }

        /// <summary>
        /// Shows buttons from list of flagged enumeration
        /// </summary>
        /// <param name="buttons"></param>
        public void ShowButtons(CommandBarButtons buttons)
        {
            if (buttons.HasFlag(CommandBarButtons.Save))
            {
                SaveButton.Visible = true;
            }
            if (buttons.HasFlag(CommandBarButtons.Cancel))
            {
                CancelButton.Visible = true;
            }
            if (buttons.HasFlag(CommandBarButtons.Submit))
            {
                SubmitButton.Visible = true;
            }
            if (buttons.HasFlag(CommandBarButtons.Clear))
            {
                ClearButton.Visible = true;
            }
            if (buttons.HasFlag(CommandBarButtons.Extra))
            {
                ExtraButton.Visible = true;
            }
        }

        /// <summary>
        /// Hides buttons from list of flagged enumeration
        /// </summary>
        /// <param name="buttons"></param>
        public void HideButtons(CommandBarButtons buttons)
        {
            if (buttons.HasFlag(CommandBarButtons.Save))
            {
                SaveButton.Visible = false;
            }
            if (buttons.HasFlag(CommandBarButtons.Cancel))
            {
                CancelButton.Visible = false;
            }
            if (buttons.HasFlag(CommandBarButtons.Submit))
            {
                SubmitButton.Visible = false;
            }
            if (buttons.HasFlag(CommandBarButtons.Clear))
            {
                ClearButton.Visible = false;
            }
            if (buttons.HasFlag(CommandBarButtons.Extra))
            {
                ExtraButton.Visible = false;
            }
        }
        /// <summary>
        /// Reflects the latest changes in text of the buttons being shown in the command bar
        /// </summary>
        public void ReloadButtonText()
        {
            if (ClearButton != null && !String.IsNullOrEmpty(ClearButtonText))
                ClearButton.Text = ClearButtonText;
            if (SaveButton != null && !String.IsNullOrEmpty(SaveButtonText))
                SaveButton.Text = SaveButtonText;
            if (SubmitButton != null && !String.IsNullOrEmpty(SubmitButtonText))
                SubmitButton.Text = SubmitButtonText;
            if (btnExtra != null && !String.IsNullOrEmpty(ExtraButtonText))
                btnExtra.Text = ExtraButtonText;
            if (CancelButton != null && !String.IsNullOrEmpty(CancelButtonText))
                CancelButton.Text = CancelButtonText;
        }
        #endregion

        #region Constructor
        public CommandBar()
        {
            //Loading default position for command bar buttons
            //Appsetting key - CommandBarButtonsPosition
            //Appsetting values - Center, Right, Left
            string position = ConfigurationManager.AppSettings.Get("CommandBarButtonsPosition");
            if (!string.IsNullOrWhiteSpace(position))
            {
                if (Enum.IsDefined(typeof(CommandBarButtonsPosition), position))
                {
                    ButtonPosition = (CommandBarButtonsPosition)Enum.Parse(typeof(CommandBarButtonsPosition), position);
                }
            }

            //Loading default type for command bar buttons
            //Appsetting key - CommandBarButtonType
            //Appsetting values - StandardButton, LinkButton etc

            string buttonType = ConfigurationManager.AppSettings.Get("CommandBarButtonType");
            if (!string.IsNullOrWhiteSpace(buttonType))
            {
                if (Enum.IsDefined(typeof(RadButtonType), buttonType))
                {
                    ButtonType = (RadButtonType)Enum.Parse(typeof(RadButtonType), buttonType);
                }
            }
        }
        #endregion

        #region Control Events

        //---------------------
        //Button's inbuilt click handlers 

        //Extra Button
        void btnExtra_Click(object sender, EventArgs e)
        {
            if (ExtraClick != null) { ExtraClick(sender, e); }
        }

        //Cancel Button
        void btnCancel_Click(object sender, EventArgs e)
        {
            if (CancelClick != null) { CancelClick(sender, e); }
        }

        //Clear Button
        void btnClear_Click(object sender, EventArgs e)
        {
            if (ClearClick != null) { ClearClick(sender, e); }
        }

        //Submit Button
        void btnSubmit_Click(object sender, EventArgs e)
        {
            if (SubmitClick != null) { SubmitClick(sender, e); }
        }

        //Save Button inside TreeList
        void btnTreeGrd_Click(object sender, EventArgs e)
        {
            if (SaveClick != null) { SaveClick(sender, e); }
        }

        //Save Button inside Grid
        void btnGrd_Click(object sender, EventArgs e)
        {
            if (SaveClick != null) { SaveClick(sender, e); }
        }

        //Save Button
        void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveClick != null) { SaveClick(sender, e); }
        }


        //---------------------
        //UserControl's event handlers


        //Page load event
        protected void Page_Load(object sender, EventArgs e)
        {
            //View initialization
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();

            //Checking for correct mode
            if (TreeListMode && GridMode) throw new InvalidAttributeException("TreeListMode and GridMode of the CommandBar cannot be true at the same time.");

            //Creating a list of display buttons to enable default required properties 
            //for command bar in grid or treelist mode
            if (GridMode || TreeListMode)
            {
                bool editmode = false;
                if (GridMode)
                {
                    GridEditFormItem editedItem = this.Parent.NamingContainer as GridEditFormItem;
                    if (editedItem != null) editmode = editedItem is GridEditFormInsertItem ? false : true;
                }
                else
                {
                    TreeListEditFormItem editedItem = this.Parent.NamingContainer as TreeListEditFormItem;
                    if (editedItem != null) editmode = editedItem is TreeListEditFormInsertItem ? false : true;
                }
                DisplayButtons = CommandBarButtons.None;
                if (editmode)
                {
                    foreach (CommandBarButtons button in Enum.GetValues(typeof(CommandBarButtons)))
                    {
                        if (EditModeButtons.HasFlag(button)) DisplayButtons = DisplayButtons | button;
                    }

                }
                else
                {
                    foreach (CommandBarButtons button in Enum.GetValues(typeof(CommandBarButtons)))
                    {
                        if (InsertModeButtons.HasFlag(button)) DisplayButtons = DisplayButtons | button;
                    }
                }
                DisplayButtons = DisplayButtons | CommandBarButtons.Save | CommandBarButtons.Cancel;
                AutoPostbackButtons = AutoPostbackButtons | CommandBarButtons.Save | CommandBarButtons.Cancel;
                btnCancel.CommandName = "Cancel";
            }

            //Setting properties of command bar
            SetProperties();

            //Setting default button for the panel
            SetDefaultButton();
        }

        //Page Init event
        protected void Page_Init(object sender, EventArgs e)
        {
            foreach (WclButton button in ExtraCommandButtons)
            {
                placeExtraButtons.Controls.Add(button);
            }

        }

        #endregion

    }
}