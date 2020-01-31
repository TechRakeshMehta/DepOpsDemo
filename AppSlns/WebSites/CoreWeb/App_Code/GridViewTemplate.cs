#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename: GridViewTemplate.cs
// Purpose:   
//

#endregion

#region Namespace

#region System defined

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Linq;

#endregion

#region Application Specific

using CoreWeb.CommonControls.Views;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
#endregion

#endregion


namespace CoreWeb
{
    /// <summary>
    /// A customized class for displaying the Template Column
    /// </summary>
    public class GridViewTemplate : ITemplate, IBindableTemplate
    {
        #region Variable

        #region Private
        //A variable to hold the type of ListItemType.
        private ListItemType _templateType;
        private WclComboBox _radControlCombo = null;
        private WclTextBox _radControlTextBox = null;
        private Control _userControl = null;

        //A variable to hold the column name.
        private string _columnName;

        #endregion

        #region Public

        #endregion

        #endregion


        #region Constructor

        //Constructor where we define the template type and column name.
        public GridViewTemplate(ListItemType type, string colname)
        {
            //Stores the template type.
            _templateType = type;

            //Stores the column name.
            _columnName = colname;

            //TextBox Template
            _radControlTextBox = new WclTextBox();
        }
        //Constructor where we define the template type and column name. for DropDown
        public GridViewTemplate(ListItemType type, string colname, WclComboBox radControlCombo)
        {
            //Stores the template type.
            _templateType = type;

            //Stores the column name.
            _columnName = colname;

            //DropDownlist in Edit Template.
            _radControlCombo = radControlCombo;
        }

        public GridViewTemplate(ListItemType type, string colname, Control locControl)
        {
            //Stores the template type.
            _templateType = type;

            //Stores the column name.
            _columnName = colname;

            //DropDownlist in Edit Template.
            _userControl = locControl;
        }

        #endregion

        #region Method

        void ITemplate.InstantiateIn(System.Web.UI.Control container)
        {
            switch (_templateType)
            {
                case ListItemType.Header:
                    //Creates a new label control and add it to the container.
                    Label lbl = new Label();            //Allocates the new label object.
                    lbl.Text = _columnName;             //Assigns the name of the column in the lable.
                    container.Controls.Add(lbl);        //Adds the newly created label control to the container.
                    break;

                case ListItemType.Item:
                    //Creates a new text box control and add it to the container.
                    Label lblItem = new Label();                            //Allocates the new text box object.
                    lblItem.DataBinding += new EventHandler(tb1_DataBinding);   //Attaches the data binding event.
                    //lblItem.Columns = 4;                                        //Creates a column with size 4.
                    container.Controls.Add(lblItem);                            //Adds the newly created textbox to the container.
                    break;

                case ListItemType.EditItem:
                    // EditItem, I didnot added any code here.
                    //Creates a new text box control and add it to the container.
                    if (_radControlTextBox != null)
                    {
                        _radControlTextBox.ID = "txt" + _columnName;
                        _radControlTextBox.DataBinding += new EventHandler(tb2_DataBinding);   //Attaches the data binding event.
                        _radControlTextBox.MaxLength = 20;
                        _radControlTextBox.Columns = 84;                                        //Creates a column with size 84.
                        container.Controls.Add(_radControlTextBox);                            //Adds the newly created textbox to the container.

                    }
                    else if (_radControlCombo != null)
                    {
                        _radControlCombo.ID = "cmb" + _columnName;
                        container.Controls.Add(_radControlCombo);
                    }
                    else if (_userControl != null)
                    {
                        container.Controls.Add(_userControl);
                    }

                    break;


            }
        }

        /// <summary>
        /// This is the event, which will be raised when the binding happens.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tb1_DataBinding(object sender, EventArgs e)
        {
            Label lbldata = sender as Label;
            GridDataItem container = (GridDataItem)lbldata.NamingContainer;
            DataRow dr = container.DataItem as DataRow;
            if (dr.Table.Columns.Contains(_columnName))
            {
                object dataValue = dr[_columnName];

                if (dataValue != DBNull.Value)
                {
                    lbldata.Text = dataValue.ToString();
                }
            }
        }

        /// <summary>
        /// This is the event, which will be raised when the binding happens.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tb2_DataBinding(object sender, EventArgs e)
        {
            WclTextBox txtdata = sender as WclTextBox;
            if (txtdata.NamingContainer is GridEditFormItem)
            {
                GridEditFormItem container = txtdata.NamingContainer as GridEditFormItem;
                if (container.DataItem is DataRow)
                {
                    DataRow dr = container.DataItem as DataRow;
                    if (dr.Table.Columns.Contains(_columnName))
                    {
                        object dataValue = dr[_columnName];
                        if (dataValue != DBNull.Value)
                        {
                            txtdata.Text = dataValue.ToString();
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Extract value 
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public IOrderedDictionary ExtractValues(Control container)
        {
            OrderedDictionary od = new OrderedDictionary();
            if (_columnName != "Location")
                od.Add(_columnName, ((WclTextBox)(((GridEditFormItem)(container)).FindControl("txt" + _columnName))).Text);
            else
                od.Add(_columnName, ((Control)(((GridEditFormItem)(container)).FindControl("divLocation"))).AppRelativeTemplateSourceDirectory);
            return od;
        }
        #endregion
    }
}