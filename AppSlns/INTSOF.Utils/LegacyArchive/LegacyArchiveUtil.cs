#region Copyright

// **************************************************************************************************
// LegacyArchiveUtil.cs
// 
// 
// Comments
// 	-----------------------------------------------------
// Initial Coding
// Converted to expression tree
// Add BuildColumnNamesDictionaryFromXml function
// Code Cleanup
// 
//                          Copyright 2011 Intersoft Data Labs.
//      All rights are reserved.  Reproduction or transmission in whole or in part, in any form or
//    by any means, electronic, mechanical or otherwise, is prohibited without the prior written
//    consent of the copyright owner.
// *************************************************************************************************
#endregion

#region Using Directives

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

#endregion

namespace INTSOF.Utils.LegacyArchive
{
    /// <summary>
    /// The utilities.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Static bid column header cache
        /// </summary>
        private static ConcurrentDictionary<string, Dictionary<string, string>> bidColumnHeaderTextCache;

        /// <summary>
        /// Static cache of control names
        /// </summary>
        private static ConcurrentDictionary<string, List<string>> controlNamesCache;

        #region Public Methods

        /// <summary>Determines whether the specified tenant type id is client.</summary>
        /// <param name="tenantTypeId">The tenant type id.</param>
        /// <returns><c>true</c> if the specified tenant type id is client; otherwise, <c>false</c>.</returns>
        public static bool IsClient(int? tenantTypeId)
        {
            return tenantTypeId.HasValue && ((TenantTypeEnum)tenantTypeId.Value == TenantTypeEnum.Client);
        }

        /// <summary>Determines whether the specified tenant type id is supplier.</summary>
        /// <param name="tenantTypeId">The tenant type id.</param>
        /// <returns><c>true</c> if the specified tenant type id is supplier; otherwise, <c>false</c>.</returns>
        public static bool IsSupplier(int? tenantTypeId)
        {
            return tenantTypeId.HasValue && (TenantTypeEnum)tenantTypeId.Value == TenantTypeEnum.Supplier;
        }

        /// <summary>Determines whether the specified tenant type id is company.</summary>
        /// <param name="tenantTypeId">The tenant type id.</param>
        /// <returns><c>true</c> if the specified tenant type id is company; otherwise, <c>false</c>.</returns>
        public static bool IsCompany(int? tenantTypeId)
        {
            return tenantTypeId.HasValue && ((TenantTypeEnum)tenantTypeId.Value == TenantTypeEnum.Company);
        }

        /// <summary>
        /// Gets the display fields.
        /// </summary>
        /// <typeparam name="T">
        /// ViewContract type 
        /// </typeparam>
        /// <param name="obj">
        /// The ViewContract object. 
        /// </param>
        /// <returns>
        /// A System.Collections.Generic.IEnumerable&lt;INTSOF.Utils.LegacyArchive.DisplayField&gt; 
        /// </returns>
        public static IEnumerable<DisplayField> GetDisplayFields<T>(T obj) where T : class
        {
            return PropertyHelper<T>.GetDisplayFields(obj);
        }

        /// <summary>
        /// Load Bids Column Header Text xml to a Dictionary
        /// </summary>
        /// <param name="physicalPath">The physical path to the XML file</param>
        /// <returns></returns>
        public static ConcurrentDictionary<string, Dictionary<string, string>> BuildColumnNameDictionaryFromXml(string physicalPath)
        {
            try
            {
                if (bidColumnHeaderTextCache.IsNull())
                {
                    bidColumnHeaderTextCache = new ConcurrentDictionary<string, Dictionary<string, string>>();

                    var xmlDoc = new XmlDocument();

                    xmlDoc.Load(physicalPath);

                    XmlNodeList xmlNodeElement = xmlDoc.GetElementsByTagName("section");

                    foreach (var item in xmlNodeElement)
                    {
                        var dictionary = new Dictionary<string, string>();

                        XmlNodeList columnNames = (item as XmlNode).ChildNodes;

                        foreach (var columnName in columnNames)
                        {
                            dictionary.Add(((XmlNode)columnName).Attributes[0].Value, ((XmlNode)columnName).InnerText);
                        }

                        bidColumnHeaderTextCache.TryAdd((item as XmlNode).Attributes[0].Value.ToUpper().RemoveWhitespace(), dictionary);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return bidColumnHeaderTextCache;
        }

        /// <summary>
        /// Load control names to a dictionary (either detail or popup)
        /// </summary>
        /// <param name="physicalPath">The physical path to the control name xml</param>
        /// <returns></returns>
        public static ConcurrentDictionary<string, List<string>> BuildControlNameDictionaryFromXml(string physicalPath)
        {
            try
            {
                if (controlNamesCache.IsNull())
                {
                    controlNamesCache = new ConcurrentDictionary<string, List<string>>();

                    var xmlDoc = new XmlDocument();

                    xmlDoc.Load(physicalPath);

                    XmlNodeList xmlNodeElement = xmlDoc.GetElementsByTagName("controls");

                    foreach (var item in xmlNodeElement)
                    {
                        var list = new List<string>();

                        XmlNodeList controls = (item as XmlNode).ChildNodes;

                        foreach (var control in controls)
                        {
                            list.Add(((XmlNode)control).InnerText);
                        }

                        controlNamesCache.TryAdd((item as XmlNode).Attributes[0].Value.ToUpper().RemoveWhitespace(), list);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return controlNamesCache;
        }
        #endregion

        /// <summary>Property helper</summary>
        /// <typeparam name="T">Class type</typeparam>
        internal static class PropertyHelper<T> where T : class
        {
            #region Constants and Fields

            /// <summary>
            /// The _display fields cache.
            /// </summary>
            private static IEnumerable<Tuple<DisplayField, Func<T, object>>> displayFieldsCache;

            /// <summary>
            /// The _property info cache.
            /// </summary>
            private static IEnumerable<Tuple<PropertyInfo, DisplayMappingAttribute>> propertyInfoCache;

            #endregion

            #region Public Methods

            /// <summary>
            /// Gets the display fields.
            /// </summary>
            /// <param name="obj">
            /// The obj. 
            /// </param>
            /// <returns>
            /// A System.Collections.Generic.IEnumerable&lt;INTSOF.Utils.LegacyArchive.DisplayField&gt; 
            /// </returns>
            public static IEnumerable<DisplayField> GetDisplayFields(T obj)
            {
                IEnumerable<Tuple<DisplayField, Func<T, object>>> displayFields = GetDisplayFields();

                foreach (Tuple<DisplayField, Func<T, object>> displayField in displayFields)
                {
                    displayField.Item1.Value = displayField.Item2.Invoke(obj);
                    yield return displayField.Item1;
                }
            }

            #endregion

            #region Methods

            /// <summary>
            /// Caches the display fields.
            /// </summary>
            /// <returns>
            /// A System.Collections.Generic.IEnumerable&lt;System.Tuple&lt;INTSOF.Utils.LegacyArchive.DisplayField,System.Func&lt;System.Object&gt;&gt;&gt; 
            /// </returns>
            private static IEnumerable<Tuple<DisplayField, Func<T, object>>> GetDisplayFields()
            {
                ParameterExpression parameter = Expression.Parameter(typeof(T), "parameter");

                if (displayFieldsCache.IsNull())
                {
                    if (propertyInfoCache.IsNull())
                    {
                        propertyInfoCache = typeof(T).GetProperties().Where(p => p.CanRead).Select(
                           p => new
                           {
                               PropertyInfo = p,
                               Attribute = p.GetCustomAttributes(typeof(DisplayMappingAttribute), false).FirstOrDefault()
                           }).Where(a => a.Attribute.IsNotNull() && ((DisplayMappingAttribute)a.Attribute).IsVisible).Select(
                                      a => new Tuple<PropertyInfo, DisplayMappingAttribute>(a.PropertyInfo, a.Attribute as DisplayMappingAttribute));
                    }

                    displayFieldsCache = from prop in propertyInfoCache
                                         select new
                                         {
                                             PropertyInfo = prop.Item1,
                                             DisplayField = new DisplayField
                                             {
                                                 Name = prop.Item2.Label,
                                                 ColSpan = prop.Item2.ColSpan,
                                                 DisplayOrder = prop.Item2.DisplayOrder,
                                                 CommandName = prop.Item2.CommandName,
                                                 DisplayFlags = prop.Item2.DisplayFlags,
                                                 StaticText = prop.Item2.StaticText,
                                                 DebugFieldName = prop.Item1.Name,
                                                 DataFormat = prop.Item2.DataFormat
                                             }
                                         }
                                             into property
                                             let getter =
                                                Expression.Lambda<Func<T, object>>(
                                                   Expression.Convert(Expression.Property(parameter, property.PropertyInfo), typeof(object)), parameter).Compile()
                                             select new Tuple<DisplayField, Func<T, object>>(property.DisplayField, getter);
                }

                return displayFieldsCache;
            }

            #endregion
        }
    }
}