#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  UserControlFolderListForPolicy.aspx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.IO;
using System.Linq;

#endregion

#region Application Specific

using System.Web.UI;
using Telerik.Web.UI;
using INTSOF.Utils;

#endregion

#endregion

/// <summary>
/// This class handles the operations related to user control folder listing for policies in security module.
/// </summary>
public partial class SysXSecurityModel_UserControl_UserControlFolderListForPolicy : Page
{
    #region Variables

    #region Public Variables

    #endregion

    #region Private Variables

    #endregion

    #endregion

    #region Properties

    #endregion

    #region Events

    /// <summary>
    /// Page load event for initialized event in presenter.
    /// </summary>
    /// <param name="sender">The object firing the event.</param>
    /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                DirectoryInfo directoryInformation = new DirectoryInfo(Server.MapPath("~"));
                RadTreeNode radTreeNode = new RadTreeNode("American Databank", "Complio");
                BindFolderAndFiles(directoryInformation, radTreeNode);
                treeControlID.Nodes.Add(radTreeNode);
                treeControlID.Nodes[0].Expanded = true;
            }
        }
        catch (SysXException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    #region Methods

    #region Public Methods

    #endregion

    #region Private Methods

    private String BindFolderAndFiles(DirectoryInfo directoryInfo, RadTreeNode radTreeNode)
    {
        DirectoryInfo[] dirs = directoryInfo.GetDirectories("*.*");

        foreach (DirectoryInfo dir in dirs)
        {
            if (dir.GetFiles("*.ascx", SearchOption.AllDirectories).Any())
            {
                RadTreeNode newParentNode = new RadTreeNode(dir.Name, dir.FullName.Replace(Server.MapPath("~") + "\\", "").Replace("\\", "\\\\"));
                radTreeNode.Nodes.Add(newParentNode);
                BindFolderAndFiles(dir, newParentNode);
            }
        }

        FileInfo[] fileList = directoryInfo.GetFiles("*.ascx");

        foreach (FileInfo file in fileList)
        {
            radTreeNode.Nodes.Add(new RadTreeNode(file.Name, file.Name));
        }

        return String.Empty;
    }

    #endregion

    #endregion
}