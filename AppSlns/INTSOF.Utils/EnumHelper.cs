#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  EnumHelper.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Utils
{
    /// <summary>
    ///   Shared constants used by Flags and Enums.
    /// </summary>
    internal static class EnumHelper<T> where T : struct, IComparable, IFormattable, IConvertible
    {
        internal static readonly Boolean IsFlags;
        internal static readonly Func<T, T, T> Or;
        internal static readonly Func<T, T, T> And;
        internal static readonly Func<T, T> Not;
        internal static readonly T UsedBits;
        internal static readonly T AllBits;
        internal static readonly T UnusedBits;
        internal static Func<T, T, Boolean> Equality;
        internal static readonly Func<T, Boolean> IsEmpty;
        internal static readonly IList<T> Values;
        internal static readonly IList<String> Names;
        internal static readonly Type UnderlyingType;
        internal static readonly Dictionary<T, String> ValueToDescriptionMap;
        internal static readonly Dictionary<String, T> DescriptionToValueMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        static EnumHelper()
        {
            Values = new ReadOnlyCollection<T>((T[])Enum.GetValues(typeof(T)));

            Names = new ReadOnlyCollection<String>(Enum.GetNames(typeof(T)).ToList().ConvertAll(n => n.ToLower()));

            ValueToDescriptionMap = new Dictionary<T, String>();

            DescriptionToValueMap = new Dictionary<String, T>();

            foreach (T value in Values)
            {
                String description = GetDescription(value);

                ValueToDescriptionMap[value] = description;

                if (!description.IsNull() && !DescriptionToValueMap.ContainsKey(description))
                {
                    DescriptionToValueMap[description] = value;
                }
            }
            UnderlyingType = Enum.GetUnderlyingType(typeof(T));

            IsFlags = typeof(T).IsDefined(typeof(FlagsAttribute), false);

            // Parameters for various expression trees
            ParameterExpression param1 = Expression.Parameter(typeof(T), "x");

            ParameterExpression param2 = Expression.Parameter(typeof(T), "y");

            Expression convertedParam1 = Expression.Convert(param1, UnderlyingType);

            Expression convertedParam2 = Expression.Convert(param2, UnderlyingType);

            Equality = Expression.Lambda<Func<T, T, bool>>(Expression.Equal(convertedParam1, convertedParam2), param1, param2).Compile();

            Or = Expression.Lambda<Func<T, T, T>>(Expression.Convert(Expression.Or(convertedParam1, convertedParam2), typeof(T)), param1, param2).Compile();

            And = Expression.Lambda<Func<T, T, T>>(Expression.Convert(Expression.And(convertedParam1, convertedParam2), typeof(T)), param1, param2).Compile();

            Not = Expression.Lambda<Func<T, T>>(Expression.Convert(Expression.Not(convertedParam1), typeof(T)), param1).Compile();

            IsEmpty = Expression.Lambda<Func<T, bool>>(Expression.Equal(convertedParam1, Expression.Constant(Activator.CreateInstance(UnderlyingType))), param1).Compile();

            UsedBits = default(T);

            foreach (T value in Extensions.GetValues<T>())
            {
                UsedBits = Or(UsedBits, value);
            }

            AllBits = Not(default(T));

            UnusedBits = And(AllBits, Not(UsedBits));
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static String GetDescription(T value)
        {
            FieldInfo field = typeof(T).GetField(value.ToString());

            return field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .Cast<DescriptionAttribute>()
                    .Select(x => x.Description)
                    .FirstOrDefault();
        }
    }
}