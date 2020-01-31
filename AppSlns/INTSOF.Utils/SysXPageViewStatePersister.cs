#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXPageViewStatePersister.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.IO.Compression;
using System.Configuration;
#endregion

#region Application Specific
#endregion

#endregion

namespace INTSOF.Utils
{
    /// <summary>
    /// This class handles SysXPageViewStatePersister.
    /// </summary>
    /// <remarks></remarks>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class SysXPageViewStatePersister : PageStatePersister
    {
        #region Variables
        IPersistViewState state = new SysXPersistViewStateProvider();
        LosFormatter formatter = new LosFormatter();
        byte[] bytesViewState;
        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties
        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region public Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="SysXPageViewStatePersister"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <remarks></remarks>
        public SysXPageViewStatePersister(Page p)
            : base(p)
        {
            // TODO: Needs to be implemented.
            if(ConfigurationManager.AppSettings[SysXCachingConst.CUSTOMVIEWSTATEPROVIDER].IsNotNull())
            {
                switch (ConfigurationManager.AppSettings[SysXCachingConst.CUSTOMVIEWSTATEPROVIDER].ToString().ToUpper())
                {
                    case "REDIS":
                        {
                            state = new RedisPersistViewStateProvider();
                            break;
                        }
                    case "SQL":
                        {
                            state = new SysXPersistViewStateProvider();
                            break;
                        }

                }
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <remarks></remarks>
        public override void Load()
        {
            try
            {
                if (!Page.Session.SessionID.IsNullOrEmpty())
                {
                    string pageSessionID = Page.Session.SessionID;
                    if (Page.MasterPageFile.Contains(SysXCachingConst.MASTERPAGEFILE))
                        pageSessionID = SysXCachingConst.MASTER + pageSessionID;

                    string viewstate = state.Load(pageSessionID, Page.Request.Url.ToString());
                    if (!viewstate.IsNullOrEmpty())
                    {
                        bytesViewState = System.Convert.FromBase64String(viewstate);
                        bytesViewState = DecompressViewState(bytesViewState);
                        Pair pair = (Pair)formatter.Deserialize(System.Convert.ToBase64String(bytesViewState));
                        ViewState = pair.First;
                        ControlState = pair.Second;
                    } 
                }
                else
                {
                    throw new InvalidOperationException(SysXCachingConst.NULLID_NOTIFICATION);
                }
            }
            catch(Exception ex)
            {
                
            }
        }
        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <remarks></remarks>
        public override void Save()
        {
            try
            {
                if (ViewState.IsNullOrEmpty() || ControlState.IsNullOrEmpty())
                    return;

                if (!Page.Session.SessionID.IsNullOrEmpty())
                {

                    StringWriter writer = new StringWriter();
                    Boolean isOverWritable = true;
                    string pageSessionID = Page.Session.SessionID;
                    if (Page.MasterPageFile.Contains(SysXCachingConst.MASTERPAGEFILE))
                    {
                        isOverWritable = false;
                        pageSessionID = SysXCachingConst.MASTER + pageSessionID;
                    }

                    formatter.Serialize(writer, new Pair(ViewState, ControlState));
                    bytesViewState = System.Convert.FromBase64String(writer.ToString());
                    bytesViewState = CompressViewState(bytesViewState);
                    String stateString = System.Convert.ToBase64String(bytesViewState);

                    state.Save(pageSessionID, Page.Request.Url.ToString(), stateString, isOverWritable);

                    ViewState = null;
                    ControlState = null;
                }
                else
                {
                    throw new InvalidOperationException(SysXCachingConst.NULLID_NOTIFICATION);
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region private Methods
        /// <summary>
        /// Compress the ViewState.
        /// </summary>
        /// <param name="uncompData">value for uncompData.</param>
        /// <returns></returns>
        private Byte[] CompressViewState(Byte[] compData)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (GZipStream zipStream = new GZipStream(stream, CompressionMode.Compress, true))
                {
                    zipStream.Write(compData, AppConsts.NONE, compData.Length);
                }
                return stream.ToArray();
            }
        }
        /// <summary>
        /// Decompress the ViewState.   
        /// </summary>
        /// <param name="compData">value for compData.</param>
        /// <returns></returns>
        private Byte[] DecompressViewState(Byte[] decompData)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(decompData, AppConsts.NONE, decompData.Length);
                stream.Position = AppConsts.NONE;
                GZipStream zipStream = new GZipStream(stream, CompressionMode.Decompress, true);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Read 1024 bytes at a time
                    Byte[] bytes = new Byte[1024];
                    Int32 read = -AppConsts.ONE;
                    read = zipStream.Read(bytes, AppConsts.NONE, bytes.Length);

                    while (read > AppConsts.NONE)
                    {
                        memoryStream.Write(bytes, AppConsts.NONE, read);
                        read = zipStream.Read(bytes, AppConsts.NONE, bytes.Length);
                    }

                    zipStream.Close();
                    return memoryStream.ToArray();
                }
            }
        }
        #endregion

        #endregion
    }
}
