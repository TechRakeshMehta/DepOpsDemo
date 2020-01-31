#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  DynamicCopy.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Utils
{
    #region Using Directives

    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    #endregion

    /// <summary>
    /// Helper class to dynamically copy one object to another
    /// </summary>
    public static class DynamicCopy
    {
        #region Public Methods

        /// <summary>The copy properties.</summary>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public static void CopyProperties<TSource, TDestination>(TSource source, TDestination target)
        {
            Helper<TSource, TDestination>.CopyProperties(source, target);
        }

        /// <summary>The copy properties.</summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public static void CopyProperties<T>(T source, T target)
        {
            Helper<T>.CopyProperties(source, target);
        }

        #endregion

        #region Nested Class Helper

        private static class Helper<TSource, TTarget>
        {
            /// <summary>
            /// The _copy properties.
            /// </summary>
            private static readonly Action<TSource, TTarget>[] _copyProperties = Prepare();


            /// <summary>
            /// Copies the properties.
            /// </summary>
            /// <param name="source">
            /// The source.
            /// </param>
            /// <param name="target">
            /// The target.
            /// </param>
            public static void CopyProperties(TSource source, TTarget target)
            {
                foreach (Action<TSource, TTarget> copyProperties in _copyProperties)
                {
                    copyProperties(source, target);
                }
            }


            /// <summary>The prepare.</summary>
            /// <returns>A Action&lt;T,T&gt;[]</returns>
            private static Action<TSource, TTarget>[] Prepare()
            {
                Type sourceType = typeof(TSource);
                Type targetType = typeof(TTarget);

                ParameterExpression source = Expression.Parameter(sourceType, "source");
                ParameterExpression target = Expression.Parameter(targetType, "target");

                var targetProperties = targetType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(property => property.CanWrite);

                var copyProperties = new List<Action<TSource, TTarget>>();

                foreach (var targetProperty in targetProperties)
                {
                    var sourceProperty = sourceType.GetProperty(targetProperty.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    if (sourceProperty.IsNull() || !sourceProperty.CanRead)
                    {
                        continue;
                    }
                    else
                    {
                        var getExpression = Expression.Property(source, sourceProperty);

                        var setExpression = Expression.Call(target, targetProperty.GetSetMethod(true), getExpression);

                        copyProperties.Add(Expression.Lambda<Action<TSource, TTarget>>(setExpression, source, target).Compile());
                    }
                }

                return copyProperties.ToArray();
            }
        }

        /// <summary>
        /// The helper.
        /// </summary>
        /// <typeparam name="T">Type
        /// </typeparam>
        private static class Helper<T>
        {
            #region Constants and Fields

            /// <summary>
            /// The _copy properties.
            /// </summary>
            private static readonly Action<T, T>[] _copyProperties = Prepare();


            #endregion

            #region Public Methods

            /// <summary>
            /// Copies the properties.
            /// </summary>
            /// <param name="source">
            /// The source.
            /// </param>
            /// <param name="target">
            /// The target.
            /// </param>
            public static void CopyProperties(T source, T target)
            {
                foreach (Action<T, T> copyProperties in _copyProperties)
                {
                    copyProperties(source, target);
                }
            }

            #endregion

            #region Private Methods

            /// <summary>The prepare.</summary>
            /// <returns>A Action&lt;T,T&gt;[]</returns>
            private static Action<T, T>[] Prepare()
            {
                Type type = typeof(T);

                ParameterExpression source = Expression.Parameter(type, "source");

                ParameterExpression target = Expression.Parameter(type, "target");

                return
                   type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                       .Where(property => property.CanRead && property.CanWrite)
                       .Select(property => new
                       {
                           property,
                           getExpression = Expression.Property(source, property)
                       })
                       .Select(args => new
                       {
                           args,
                           setExpression = Expression.Call(target, args.property.GetSetMethod(true), args.getExpression)
                       })
                      .Select(args => Expression.Lambda<Action<T, T>>(args.setExpression, source, target).Compile()).ToArray();

            }

            #endregion
        }

        #endregion
    }

    public static class DeepObjectCopy
    {
        public static T DeepCopy<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;

                return (T)formatter.Deserialize(stream);
            }
        }

    }
}