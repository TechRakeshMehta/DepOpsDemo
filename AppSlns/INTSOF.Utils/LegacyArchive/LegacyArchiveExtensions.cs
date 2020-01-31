#region Copyright

// **************************************************************************************************
// LegacyArchiveUtil.cs
// 
// 
//   Comments
// -----------------------------------------------------
//  Initial Coding
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

#endregion
namespace INTSOF.Utils.LegacyArchive
{
    /// <summary>Legacy archive extension methods</summary>
    public static class LegacyArchiveExtensions
    {
        /// <summary>UAs the bid scrub.</summary>
        /// <param name="stringToScrub">The string to scrub.</param>
        /// <returns>A System.String</returns>
        public static string ScrubUnderAllowableBid(this string stringToScrub)
        {
            string retval = stringToScrub.Replace("BID APPROVAL - ", string.Empty).Replace("BID APPROVAL ", string.Empty);

            return retval.Replace("\r", ". ").Replace("\n", ". ");
        }

        /// <summary>masks loan number based on www_dba.get_partial_loan_num_mask function</summary>
        /// <param name="loanNumber">The loan number.</param>
        /// <returns>A System.String</returns>
        public static string MaskLoanNumber(this string loanNumber)
        {
            if (string.IsNullOrEmpty(loanNumber))
            {
                return loanNumber;
            }

            double maskLen = loanNumber.Length / 2d;

            // documentation reads: returns int for math.ceiling but actually returns double...
            int intMaskLen = Convert.ToInt32(Math.Ceiling(maskLen));
            string padded = string.Empty.PadLeft(intMaskLen, 'X');
            string shown = loanNumber.Substring(intMaskLen, loanNumber.Length - intMaskLen);

            return padded + shown;
        }

        /// <summary>Formats the phone number.</summary>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns>A System.String</returns>
        public static string FormatPhoneNumber(this string phoneNumber)
        {
            const string PHONE_NUMBER_FORMAT = @"(\d{3})(\d{3})(\d{4})";

            try
            {
                string formatPhoneNumber;

                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    // remove characters including spaces
                    string currentPhoneNumber = phoneNumber.Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty);

                    // format phone number
                    formatPhoneNumber = Regex.Replace(currentPhoneNumber, PHONE_NUMBER_FORMAT, "($1) $2-$3");
                }
                else
                {
                    return null;
                }

                return formatPhoneNumber;
            }
            catch
            {
                return null;
            }
        }


        public static string FormatYesOrNo(this string YesOrNoField)
        {
            string retval = "";

            try
            {
                if (YesOrNoField == "Y")
                    retval = "Yes";
                else
                    retval = "No";
            }

            catch
            {
                retval = "No";
            }

            return retval;
        }

        #region Property Copier

        /// <summary>Copies the properties from one class to another.</summary>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>A TDestination</returns>
        public static TDestination CopyPropertiesTo<TDestination, TSource>(this TSource source, TDestination destination)
            where TSource : class
        {
            CopyHelper<TSource, TDestination>.CopyProperties(source, destination);

            return destination;
        }

        /// <summary>Copies the properties from.</summary>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="destination">The destination.</param>
        /// <param name="source">The source.</param>
        /// <returns>A TSource</returns>
        public static TDestination CopyPropertiesFrom<TDestination, TSource>(this TDestination destination, TSource source)
            where TSource : class
        {
            return source.CopyPropertiesTo(destination);
        }

        /// <summary>Copies the collection from.</summary>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="sourceCollection">The source collection.</param>
        /// <param name="destinationCollection">The destination collection.</param>
        /// <returns>A System.Collections.Generic.ICollection&lt;TDestination&gt;</returns>
        public static ICollection<TDestination> CopyCollectionPropertiesTo<TDestination, TSource>(this IEnumerable<TSource> sourceCollection, ICollection<TDestination> destinationCollection)
            where TDestination : class
            where TSource : class
        {
            if (destinationCollection.IsNull())
            {
                destinationCollection = new List<TDestination>();
            }

            foreach (var sourceItem in sourceCollection)
            {
                var destinationItem = Activator.CreateInstance(typeof(TDestination)) as TDestination;
                destinationCollection.Add(destinationItem.CopyPropertiesFrom(sourceItem));
            }

            return destinationCollection;
        }

        /// <summary>Copies the collection from.</summary>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="destinationCollection">The destination collection.</param>
        /// <param name="sourceCollection">The source collection.</param>
        /// <returns>A System.Collections.Generic.ICollection&lt;TDestination&gt;</returns>
        public static ICollection<TDestination> CopyCollectionPropertiesFrom<TDestination, TSource>(ICollection<TDestination> destinationCollection, IEnumerable<TSource> sourceCollection)
            where TDestination : class
            where TSource : class
        {
            return sourceCollection.CopyCollectionPropertiesTo(destinationCollection);
        }

        /// <summary>Copy helper</summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        private static class CopyHelper<TSource, TDestination>
           where TSource : class
        {
            /// <summary>
            /// Local cache of actions
            /// </summary>
            private static readonly ConcurrentDictionary<string, IEnumerable<Action<TSource, TDestination>>> copyFunctionCache =
               new ConcurrentDictionary<string, IEnumerable<Action<TSource, TDestination>>>();

            /// <summary>Copies the properties.</summary>
            /// <param name="source">The source.</param>
            /// <param name="destination">The destination.</param>
            public static void CopyProperties(TSource source, TDestination destination)
            {
                // If the source or destination are null, just return.
                if (source.IsNull() || destination.IsNull())
                {
                    return;
                }

                var copyerActions = GetCopierActions();

                // run the action for each mapping
                foreach (var action in copyerActions)
                {
                    action(source, destination);
                }
            }

            /// <summary>Gets the copier.</summary>
            /// <returns>A System.Func&lt;TSource,TDestination&gt;</returns>
            private static IEnumerable<Action<TSource, TDestination>> GetCopierActions()
            {
                var cacheKey = typeof(TSource).Name + "," + typeof(TDestination).Name;
                IEnumerable<Action<TSource, TDestination>> copyExpressions;

                // See if this is cached, if not, generate the collection of actions and cache.
                if (!copyFunctionCache.TryGetValue(cacheKey, out copyExpressions))
                {
                    // parameters for the expression.
                    var sourceParameter = Expression.Parameter(typeof(TSource), "source");
                    var destinationParameter = Expression.Parameter(typeof(TDestination), "destination");

                    // Get the source properties
                    var sourceProperties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

                    // Get the action expressions for all properties in the source that can map to the destination
                    copyExpressions =
                       typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).Where(
                          destinationProperty =>
                          destinationProperty.CanWrite && sourceProperties.Any(s => s.Name == destinationProperty.Name)
                          && sourceProperties.First(s => s.Name == destinationProperty.Name).PropertyType.IsAssignableFrom(destinationProperty.PropertyType)).Select(
                             destinationProperty => new { destinationProperty, sourceProperty = sourceProperties.FirstOrDefault(p => p.Name == destinationProperty.Name) })
                          .Select(exp => Expression.Assign(Expression.Property(destinationParameter, exp.destinationProperty), Expression.Property(sourceParameter, exp.sourceProperty)))
                          .Select(expression => Expression.Lambda<Action<TSource, TDestination>>(expression, sourceParameter, destinationParameter))
                          .Select(action => action.Compile())
                          .ToList();

                    // Add to cache
                    copyFunctionCache.TryAdd(cacheKey, copyExpressions);
                }

                return copyExpressions;
            }
        }

        #endregion
    }
}