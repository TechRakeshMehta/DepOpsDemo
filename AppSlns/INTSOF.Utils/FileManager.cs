#region Copyright

// **************************************************************************************************
// FileManager.cs
// 
// 
// 
// Comments
// ---------------
// Initial Coding
// 
// 
//                         Copyright 2011 Intersoft Data Labs.
// 
//      All rights are reserved.  Reproduction or transmission in whole or in part, in any form or
//    by any means, electronic, mechanical or otherwise, is prohibited without the prior written
//    consent of the copyright owner.
// *************************************************************************************************

#endregion

#region Using Directives

#endregion

namespace INTSOF.Utils
{
   #region Using Directives

   using System;
   using System.Collections.Generic;
   using System.IO;
   using System.Linq;
   using System.Security.Cryptography;

   #endregion

   /// <summary>
   ///   Utility to handle common file operations.
   /// </summary>
   public class FileManager
   {
      #region Constants and Fields

      /// <summary>
      ///   The backslash.
      /// </summary>
      private const string BACKSLASH = @"\";

      private const string FORWARDSLASH = @"/";

      private string _path;

      #endregion

      #region Constructors and Destructors

 

      /// <summary>
      ///   Initializes a new instance of the <see cref="FileManager" /> class.
      /// </summary>
      /// <param name="filepath"> The file path. </param>
      /// <param name="filename"> The file name. </param>
      public FileManager(string filepath, string filename = null)
      {
         this.Filename = filename;
         this.Filepath = filepath;
      }

      #endregion

      #region Public Properties

      /// <summary>
      ///   Gets a value indicating whether Exists.
      /// </summary>
      public bool Exists
      {
         get
         {
            return !this.Filename.IsNullOrWhiteSpace() && !this.Filepath.IsNullOrWhiteSpace() && File.Exists(this.FullFilepath);
         }
      }

   /// <summary>
      ///   Gets or sets Filename.
      /// </summary>
      public string Filename { get; set; }

      /// <summary>
      ///   Gets or sets Path.
      /// </summary>
      public string Filepath
      {
         get
         {
            return this.FixedPath(this._path);
         }
         set
         {
            this._path = value;
         }
      }

      /// <summary>
      ///   Gets File path.
      /// </summary>
      public string FullFilepath
      {
         get
         {
            return this.Filepath + this.Filename;
         }
      }

      #endregion

      #region Public Methods

      public string FixedPath(string path)
      {
         return (path + (path.EndsWith(BACKSLASH) ? string.Empty : BACKSLASH)).Replace(FORWARDSLASH, BACKSLASH);
      }

      /// <summary>
      ///   Gets the files.
      /// </summary>
      /// <param name="fileMask"> The file mask. </param>
      /// <returns> A System.Collections.Generic.IEnumerable&lt;System.String&gt; </returns>
      public IEnumerable<string> GetFiles(string fileMask)
      {
         string filenameMask = fileMask.NullIfEmpty() ?? "*.*";

         string[] files = Directory.GetFiles(this.Filepath, filenameMask);

         return files.Select(Path.GetFileName);
      }

      public bool IsValidPath(string path)
      {
         return Directory.Exists(this.FixedPath(path));
      }

      /// <summary>
      ///   Moves the file.
      /// </summary>
      /// <param name="destinationPath"> The destination path. </param>
      /// <param name="destinationFilename"> The destination filename. </param>
      /// <param name="overwrite"> if set to <c>true</c> [overwrite]. </param>
      /// <returns> A System.Boolean </returns>
      public bool MoveFile(string destinationPath, string destinationFilename, bool overwrite = false)
      {
         try
         {
            if (!this.IsValidPath(this.FixedPath(destinationPath)))
            {
               return false;
            }

            if (!this.Exists)
            {
               return false;
            }

            string destination = this.FixedPath(destinationPath) + destinationFilename;

            if (File.Exists(destination))
            {
               if (overwrite)
               {
                  File.Delete(destination);
               }
               else
               {
                  return false;
               }
            }

            File.Move(this.FullFilepath, destination);
            return true;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public static string GetContentHashCode(string filePath)
      {
          string hashCode = string.Empty;
          using (FileStream fs = new FileStream(filePath, FileMode.Open))
          {
              MD5 md = new MD5CryptoServiceProvider();
              byte[] hashData = md.ComputeHash(fs);
              hashCode = BitConverter.ToString(hashData);
          }
          return hashCode;
      }
      #endregion
   }
}