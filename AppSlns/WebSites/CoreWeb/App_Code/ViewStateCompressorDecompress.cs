#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ViewStateCompressorDecompress.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.IO;
using System.IO.Compression;

#endregion

#region Application Specific

using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb
{
    /// <summary>
    /// Summary description for ViewStateCompressorDecompress
    /// </summary>
    public class ViewStateCompressorDecompress
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

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

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public ViewStateCompressorDecompress()
        {
            // TODO: Add constructor logic here.
        }

        /// <summary>
        /// Compresses the state of the view.
        /// </summary>
        /// <param name="uncompData">The uncomp data.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Byte[] CompressViewState(Byte[] uncompData)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                CompressionMode mode = CompressionMode.Compress;

                // Use the newly created memory stream for the compressed data.
                using (GZipStream gzip = new GZipStream(mem, mode, true))
                {
                    //Writes compressed byte to the underlying
                    //stream from the specified byte array.
                    gzip.Write(uncompData, AppConsts.NONE, uncompData.Length);
                }

                return mem.ToArray();
            }
        }

        /// <summary>
        /// Decompresses the state of the view.
        /// </summary>
        /// <param name="compData">The comp data.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Byte[] DecompressViewState(Byte[] compData)
        {
            GZipStream gzip;

            using (MemoryStream inputMem = new MemoryStream())
            {
                inputMem.Write(compData, AppConsts.NONE, compData.Length);
                // Reset the memory stream position to begin decompression.
                inputMem.Position = AppConsts.NONE;
                CompressionMode mode = CompressionMode.Decompress;
                gzip = new GZipStream(inputMem, mode, true);

                using (MemoryStream outputMem = new MemoryStream())
                {
                    // Read 1024 bytes at a time
                    Byte[] buf = new Byte[1024];
                    Int32 byteRead = -AppConsts.ONE;
                    byteRead = gzip.Read(buf, AppConsts.NONE, buf.Length);

                    while (byteRead > AppConsts.NONE)
                    {
                        //write to memory stream
                        outputMem.Write(buf, AppConsts.NONE, byteRead);
                        byteRead = gzip.Read(buf, AppConsts.NONE, buf.Length);
                    }

                    gzip.Close();
                    return outputMem.ToArray();
                }
            }
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}