#region Copyright

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  DataFormat.cs
// Purpose:   
//

#endregion

namespace INTSOF.Utils.LegacyArchive
{
   #region Using Directives

   using System.ComponentModel;

   #endregion

   /// <summary>
   /// Data Format Options
   /// </summary>
   public enum DataFormat
   {
      /// <summary>
      ///   The general format indicator.
      /// </summary>
      [Description("{0}")]
      General = 0, 

      /// <summary>
      ///   The currency format indicator.
      /// </summary>
      [Description("{0:c}")]
      Currency, 

      /// <summary>
      ///   The decimal format indicator.
      /// </summary>
      [Description("{0:f}")]
      Decimal, 

      /// <summary>
      ///   The percent format indicator.
      /// </summary>
      [Description("{0:p}")]
      Percent, 

      /// <summary>
      ///   The short date format indicator..
      /// </summary>
      [Description("{0:d}")]
      ShortDate, 

      /// <summary>
      ///   The long date format indicator.
      /// </summary>
      [Description("{0:D}")]
      LongDate, 

      /// <summary>
      ///   The short time format indicator.
      /// </summary>
      [Description("{0:t}")]
      ShortTime, 

      /// <summary>
      ///   The long time format indicator.
      /// </summary>
      [Description("{0:T}")]
      LongTime, 

      /// <summary>
      ///   The short date time format indicator.
      /// </summary>
      [Description("{0:g}")]
      ShortDateTime, 

      /// <summary>
      ///   The long date time format indicator.
      /// </summary>
      [Description("{0:f}")]
      LongDateTime, 
   }
}