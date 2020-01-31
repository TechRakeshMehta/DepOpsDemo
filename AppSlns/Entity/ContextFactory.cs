#region Copyright

// **************************************************************************************************
//ContextFactory.cs
// 
//  Comments
// 	-----------------------------------------------------
// 	Initial Coding
// 
//                          Copyright 2011 Intersoft Data Labs.
//      All rights are reserved.  Reproduction or transmission in whole or in part, in any form or
//    by any means, electronic, mechanical or otherwise, is prohibited without the prior written
//    consent of the copyright owner.
// *************************************************************************************************
#endregion

namespace Entity
{
   #region Using Directives

    using INTSOF.Utils;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using System;
    using System.Configuration;
    using System.Web;

   #endregion

   /// <summary>
   /// TODO: Update summary.
   /// </summary>
   public static class ContextFactory
   {
      #region Constants and Fields

      /// <summary>
      /// The local unity container.
      /// </summary>
      private static IUnityContainer _container;

      #endregion

      #region Public Methods

      /// <summary>
      /// Gets the archive context.
      /// </summary>
      /// <typeparam name="T">
      /// Interface or type for the context.
      /// </typeparam>
      /// <returns>
      /// A OVRSTE.Entity.<see cref="ISysXArchiveDbEntities"/> 
      /// </returns>
      public static T GetContext<T>()
      {
         T instance;

         if (HttpContext.Current.IsNull() || !HttpContext.Current.Items.Contains(typeof(T).Name))
         {
            if (typeof(T).IsInterface)
            {
               instance = GetContextFromUnity<T>();
            }
            else
            {
               instance = (T)Activator.CreateInstance(typeof(T));
            }

            HttpContext.Current.Items.Add(typeof(T).Name, instance);
         }

         return (T)HttpContext.Current.Items[typeof(T).Name];
      }

      #endregion

      #region Methods

      /// <summary>
      /// The get context from unity.
      /// </summary>
      /// <typeparam name="T">
      /// Interface for the context.
      /// </typeparam>
      /// <returns>
      /// </returns>
      private static T GetContextFromUnity<T>()
      {
         InitializeContainer();
         return _container.Resolve<T>();
      }

      /// <summary>
      /// Initializes the container.
      /// </summary>
      private static void InitializeContainer()
      {
         if (_container.IsNull())
         {
            _container = new UnityContainer();

            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Configure(_container);
          //  section.Configure(_container, "objectContextContainer");
         }
      }

      #endregion
   }
}