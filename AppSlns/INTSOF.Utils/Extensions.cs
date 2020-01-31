#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  Extensions.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Xml.Serialization;
using System.Data;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Utils
{
    /// <summary>
    /// Extensions
    /// </summary>
    /// <remarks></remarks>
    public static class Extensions
    {
        #region Object and Generic Extensions

        [ThreadStatic] public static bool HTMLEncodeOverride;

        /// <summary>
        /// Deep Clones an Object
        /// </summary>
        /// <typeparam name="T">Type of Object</typeparam>
        /// <param name="obj">Actual Object</param>
        /// <returns>Cloned Object</returns>
        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static T Clone<T>(this T source)
        {
            var dcs = new System.Runtime.Serialization
              .DataContractSerializer(typeof(T));
            using (var ms = new System.IO.MemoryStream())
            {
                dcs.WriteObject(ms, source);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                return (T)dcs.ReadObject(ms);
            }
        }

        /// <summary>
        /// Checks if an object is null
        /// </summary>
        /// <param name="o">Object to check</param>
        /// <returns>True if null, False is not null.</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean IsNull(this object o)
        {
            return ReferenceEquals(o, null);
        }

        /// <summary>
        /// Returns a new instance of a type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>A T</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static T Instance<T>(this T type) where T : new()
        {
            return type.IsNull() ? (T)Activator.CreateInstance(typeof(T)) : type;
        }

        /// <summary>
        /// Returns a new instance of a type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>A T</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static T Instance<T>(this Type type) where T : new()
        {
            if (!(typeof(Type) is T))
            {
                throw new ArgumentException("The generic type must match the type instance.");
            }

            return Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Creates an instance of the generic
        /// type specified using the parameters
        /// specified.
        /// </summary>
        /// <typeparam name="T">The type to
        /// instantiate.</typeparam>
        /// <param name="type">The System.Type
        /// being instantiated.</param>
        /// <param name="parameters">The array
        /// of parameters to use when calling
        /// the constructor.</param>
        /// <returns>An instance of the specified
        /// type.</returns>
        /// <exception cref="System.Exception">
        ///   </exception>
        ///   
        /// <example>
        /// typeof(MyObject).CreateInstance(
        /// new object[] { 1, 3.0M, "Final Parameter" });
        ///   </example>
        /// <remarks>If there is not a constructor that
        /// matches the parameters then an
        /// <see cref="System.Exception"/> is
        /// thrown.</remarks>
        [DebuggerStepThrough]
        public static T Instance<T>(this Type type, object[] parameters)
        {
            if (parameters.IsNull())
            {
                throw new ArgumentException("The parameters array must not be null.");
            }

            if (!(typeof(Type) is T))
            {
                throw new ArgumentException("The generic type must match the type instance.");
            }

            T result;

            ConstructorInfo ctor = type.GetConstructor(parameters.Select(parameter => !parameter.IsNull() ? parameter.GetType() : typeof(object)).ToArray());

            if (!ctor.IsNull())
            {
                result = (T)ctor.Invoke(parameters);
            }
            else
            {
                throw new ArgumentException(String.Format("There are no constructors for{0} that match the types provided.", type.FullName));
            }

            return result;
        }
        /// <summary>
        /// Checks if an object is not null
        /// </summary>
        /// <param name="o">Object to evaluate</param>
        /// <returns>True if NOT null, false if null</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean IsNotNull(this object o)
        {
            return !ReferenceEquals(o, null);
        }


        /// <summary>
        /// Returns an instance of a derived class from a base class
        /// </summary>
        /// <typeparam name="TBase">The type of the base.</typeparam>
        /// <typeparam name="TDerived">The type of the derived.</typeparam>
        /// <param name="baseType">The type of the base class.</param>
        /// <returns>A TDerived</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static TDerived ConvertToDerived<TBase, TDerived>(TBase baseType) where TDerived : TBase, new()
        {
            var derived = new TDerived();

            foreach (PropertyInfo propBase in typeof(TBase).GetProperties())
            {
                PropertyInfo propDerived = typeof(TDerived).GetProperty(propBase.Name);

                propDerived.SetValue(derived, propBase.GetValue(baseType, null), null);
            }

            return derived;
        }

        /// <summary>
        /// Returns the result of a function, or null if the object is null
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="obj">The object</param>
        /// <param name="func">The function</param>
        /// <param name="elseValue">The else value.</param>
        /// <returns>A TResult</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static TResult NullOr<TSource, TResult>(this TSource obj, Func<TSource, TResult> func, TResult elseValue = default(TResult)) where TSource : class
        {
            return !obj.IsNull() ? func(obj) : elseValue;
        }

        /// <summary>
        /// Converts an object of to type T
        /// </summary>
        /// <typeparam name="T">Type to convert to</typeparam>
        /// <param name="obj">object to convert</param>
        /// <returns>A T</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static T To<T>(this object obj)
        {
            Type t = typeof(T);

            return t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(Nullable<>))
                      ? (obj.IsNull() ? (T)(object)null : (T)Convert.ChangeType(obj, Nullable.GetUnderlyingType(t)))
                      : (obj == DBNull.Value
                            ? (T)((object)default(T) ?? String.Empty)
                            : (obj is IConvertible) ? (T)Convert.ChangeType(obj, t) : (T)obj);
        }

        /// <summary>
        /// Evaluates whether the input String exists in an array of type T
        /// </summary>
        /// <typeparam name="T">A type</typeparam>
        /// <param name="input">The input.</param>
        /// <param name="compareArray">The compare array.</param>
        /// <returns><c>true</c> if [is A member of] [the specified input]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean IsAMemberOf<T>(this T input, params T[] compareArray) where T : struct
        {
            if (!(compareArray.Length > 0))
            {
                throw new ArgumentException("CompareArray cannot be empty.");
            }

            try
            {
                return compareArray.Any(x => x.Equals(input));
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        /// <summary>
        /// Evaluates whether the input String exists in an array of type T
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="isCaseSensitive">if set to <c>true</c> [is case sensitive].</param>
        /// <param name="compareArray">The compare array.</param>
        /// <returns><c>true</c> if [is A member of] [the specified input]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean IsAMemberOf(this String input, Boolean isCaseSensitive = true, params String[] compareArray)
        {
            if (!(compareArray.Length > 0))
            {
                throw new ArgumentException("CompareArray cannot be empty.");
            }

            StringComparison comparison = isCaseSensitive
                                             ? StringComparison.InvariantCulture
                                             : StringComparison.InvariantCultureIgnoreCase;

            try
            {
                return compareArray.Any(x => x.Equals(input, comparison));
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        /// <summary>
        /// Evaluates whether the input String exists in an array of type T
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="compareArray">The compare array.</param>
        /// <returns><c>true</c> if [is A member of] [the specified input]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean IsAMemberOf(this String input, params String[] compareArray)
        {
            if (!(compareArray.Length > 0))
            {
                throw new ArgumentException("CompareArray cannot be empty.");
            }

            try
            {
                return compareArray.Any(x => x.Equals(input));
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the value or type default for a <see cref="Nullable"/> type
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="input"><c>Nullable</c> type</param>
        /// <returns>Value of input or type default if null</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static T ValueOrDefault<T>(this T? input) where T : struct
        {
            return input.HasValue ? input.Value : default(T);
        }

        /// <summary>
        /// Clones the specified object as a specific type
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns>A T</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static T Clone<T>(this ICloneable obj)
        {
            return (T)obj.Clone();
        }

        /// <summary>
        /// Returns if a collection is null or Empty i.e does not contain any items in it.
        /// </summary>
        /// <param name="item">A collection</param>
        /// <returns>Boolean Value</returns>
        public static Boolean IsNullOrEmptyCollection(this ICollection item)
        {
            if (item != null && item.Count > 0)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Math Extensions

        /// <summary>
        /// Returns an exponent
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="@base">The base.</param>
        /// <param name="exponent">The exponent.</param>
        /// <returns>A System.Double</returns>
        /// <remarks></remarks>
        public static T Exponent<T>(this T @base, Int32 exponent) where T : struct, IComparable, IFormattable, IConvertible
        {
            if (!(exponent >= AppConsts.NONE))
            {
                throw new ArgumentException("Exponent must be >=0.");
            }

            return Math.Pow(@base.To<double>(), exponent).To<T>();
        }

        #endregion

        #region Messaging Extension

        /// <summary>
        /// This is the method just to get body of message from xml message
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static String GetMessageBody(this String messageBody)
        {
            String messageContent = String.Empty;
            using (XmlReader reader = XmlReader.Create(new StringReader(messageBody.ToString())))
            {
                reader.ReadToFollowing("MessageBody");
                if (reader.IsNull())
                {
                    return null;
                }
                messageContent = reader.ReadElementContentAsString();
            }

            return messageContent;
        }

        /// <summary>
        /// This is the method just to get body of message from xml message
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static String GetNodeContent(this String messageBody, String NodeName)
        {
            String messageContent = String.Empty;
            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(messageBody.ToString())))
                {
                    reader.ReadToFollowing(NodeName);
                    if (reader.IsNull())
                    {
                        return null;
                    }
                    messageContent = reader.ReadElementContentAsString();
                }
            }
            catch
            {
                return String.Empty;
            }

            return messageContent;
        }

        [DebuggerStepThrough]
        public static String ConvertEmailList(this List<String> emailList)
        {
            StringBuilder emailChain = new StringBuilder();
            Boolean skipColon = true;
            foreach (String email in emailList)
            {
                if (skipColon == false)
                {
                    emailChain.Append(" ; ");
                }
                emailChain.Append(email);
                skipColon = false;
            }

            return emailChain.ToString();
        }

        ///// <summary>
        ///// Invoked to Mask a string upto given number of places
        ///// </summary>
        ///// <param name="stringToBeMasked"></param>
        ///// <param name="noOfCharactersToBeMasked"></param>
        ///// <returns></returns>
        //[DebuggerStepThrough]
        //public static String Mask(this String stringToBeMasked, Int32 noOfCharactersToBeMasked)
        //{
        //    return Regex.Replace(stringToBeMasked.Substring(0, noOfCharactersToBeMasked), @"[0-9]", "X") + stringToBeMasked.Substring(noOfCharactersToBeMasked, stringToBeMasked.Length - noOfCharactersToBeMasked);
        //}

        /// <summary>
        /// Invoked to mask a string excluding last 4 characters/digits.
        /// </summary>
        /// <param name="stringToBeMasked"></param>
        /// <returns>Masked string</returns>
        [DebuggerStepThrough]
        public static String Mask(this String stringToBeMasked)
        {
            if (stringToBeMasked.Length > 4)
                return Regex.Replace(stringToBeMasked.Substring(0, stringToBeMasked.Length - 4), @"[0-9a-zA-Z]", "X") + stringToBeMasked.Substring(stringToBeMasked.Length - 4);
            else
                return stringToBeMasked;
        }

        #endregion

        #region Delegate Extensions

        /// <summary>
        /// Runs the async.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A System.Boolean</returns>
        /// <remarks></remarks>
        public static Boolean RunAsync(this Action action, Int32 timeout)
        {
            if (!(timeout > 0))
            {
                throw new ArgumentException("Timeout must be > 0.");
            }

            try
            {
                var tokenSource = new CancellationTokenSource();

                CancellationToken cancellationToken = tokenSource.Token;

                var newTask = new Task(action, cancellationToken);

                newTask.Start();

                if (!newTask.Wait(timeout))
                {
                    tokenSource.Cancel();

                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region IEnumerable Extension

        /// <summary>
        /// Returns the first item in an IEnumerable, or a null object if no items exist
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="enumerable">The IEnumerable.</param>
        /// <param name="nullFunc">The null function.</param>
        /// <returns>A T</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static T FirstOrNullObject<T>(this IEnumerable<T> enumerable, Func<T> nullFunc) where T : class
        {
            return enumerable.FirstOrDefault() ?? nullFunc();
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="destination">The D.</param>
        /// <param name="source">The V.</param>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> destination,
                                                  IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            foreach (KeyValuePair<TKey, TValue> kvp in source)
            {
                if (destination.ContainsKey(kvp.Key))
                {
                    throw new ArgumentException("An item with the same key has already been added.");
                }

                destination[kvp.Key] = kvp.Value;
            }
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="T">Output conversion type.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>A TOut</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean TryGetValue<TKey, TValue, T>(this IDictionary<TKey, TValue> dictionary, TKey key, out T value)
        {
            if (key.IsNull())
            {
                throw new ArgumentNullException("Key");
            }

            TValue output;

            Boolean isValid = dictionary.TryGetValue(key, out output);

            value = isValid ? output.To<T>() : default(T);

            return isValid;
        }

        /// <summary>
        /// Gets a dictionary value from a key;
        /// If the key doesn't exist, adds a new entry for that key and creates a new value
        /// </summary>
        /// <typeparam name="TKey">Key Type</typeparam>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns>A T</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static T GetValue<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key)
        {
            if (key.IsNull())
            {
                throw new ArgumentNullException("Key");
            }

            T value;

            if (!dictionary.TryGetValue(key, out value))
            {
                dictionary.Add(key, value = typeof(T).IsValueType ? default(T) : (T)Activator.CreateInstance(typeof(T)));
            }

            return value;
        }

        /// <summary>
        /// Gets a dictionary value from a key;
        /// If the key doesn't exist, adds a new entry for that key and creates a new value
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultType">The default type.</param>
        /// <returns>A T</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static T GetValue<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key, Type defaultType)
        {
            if (key.IsNull())
            {
                throw new ArgumentNullException("Key");
            }

            T value;

            if (!dictionary.TryGetValue(key, out value))
            {
                dictionary.Add(key, value = typeof(T).IsValueType ? default(T) : (T)Activator.CreateInstance(defaultType.GetType()));
            }

            return value;
        }

        /// <summary>
        /// Gets a dictionary value from a key;
        /// If the key doesn't exist, adds a new entry for the provided default
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The value to add if the key doesn't exist</param>
        /// <returns>A T</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static T GetOrAdd<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key, T defaultValue)
        {
            if (key.IsNull())
            {
                throw new ArgumentNullException("Key");
            }

            T value;

            if (!dictionary.TryGetValue(key, out value))
            {
                dictionary.Add(key, defaultValue);
            }

            return value;
        }

        /// <summary>
        /// Gets a dictionary value from a key;
        /// If the key doesn't exist, adds a new entry for the provided default
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValueFunction">The default value function.</param>
        /// <returns>A T</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static T GetOrAdd<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key, Func<T> defaultValueFunction)
        {
            if (key.IsNull())
            {
                throw new ArgumentNullException("Key");
            }

            T value;

            if (!dictionary.TryGetValue(key, out value))
            {
                value = defaultValueFunction.Invoke();

                dictionary.Add(key, value);
            }

            return value;
        }

        /// <summary>
        /// Checks whether the contents of two Dictionaries are equal
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns>A System.Boolean</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean DictionaryEqual<TKey, TValue>(this IDictionary<TKey, TValue> first,
                                                         IDictionary<TKey, TValue> second)
        {
            if (first == second)
            {
                return true;
            }
            else if (first.IsNull() || second.IsNull())
            {
                return false;
            }
            else if (first.Count != second.Count)
            {
                return false;
            }

            EqualityComparer<TValue> comparer = EqualityComparer<TValue>.Default;

            foreach (KeyValuePair<TKey, TValue> kvp in first)
            {
                TValue secondValue;

                if (!second.TryGetValue(kvp.Key, out secondValue))
                {
                    return false;
                }
                else if (!comparer.Equals(kvp.Value, secondValue))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Adds a range of items to an IList.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="items">The items.</param>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            if (list.IsNull())
            {
                throw new ArgumentNullException("List");
            }

            if (items.IsNull())
            {
                throw new ArgumentNullException("Items");
            }

            foreach (T item in items)
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// Executes an action for each item in an array.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="action">The action.</param>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            if (array.IsNull())
            {
                throw new ArgumentNullException("Array");
            }

            if (action.IsNull())
            {
                throw new ArgumentNullException("Action");
            }

            foreach (T t in array)
            {
                action(t);
            }
        }

        /// <summary>
        /// Returns the first item in an IEnumerable, or a null object if no items exist
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="func">The function.</param>
        /// <param name="nullFunc">The null function.</param>
        /// <returns>A T</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static T FirstOrNullObject<T>(this IEnumerable<T> enumerable, Func<T, Boolean> func, Func<T> nullFunc)
           where T : class
        {
            return enumerable.FirstOrDefault(func) ?? nullFunc();
        }

        /// <summary>
        /// Returns the first item in an IEnumerable, or a new instance of T
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="enumerable">The IEnumerable.</param>
        /// <param name="func">The function.</param>
        /// <param name="newFunc">The new function.</param>
        /// <returns>A T</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static T FirstOrNew<T>(this IEnumerable<T> enumerable, Func<T, Boolean> func, Func<T> newFunc) where T : class
        {
            return enumerable.FirstOrNullObject(func, newFunc);
        }

        /// <summary>
        /// Returns the first item in an IEnumerable, or a new instance of T
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="enumerable">The IEnumerable.</param>
        /// <param name="newFunc">The new function.</param>
        /// <returns>A T</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static T FirstOrNew<T>(this IEnumerable<T> enumerable, Func<T> newFunc) where T : class
        {
            return enumerable.FirstOrNullObject(newFunc);
        }

        /// <summary>
        /// IEnumerable ForEach extension
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="enumerable">The IEnumerable object.</param>
        /// <param name="mapFunction">The map function.</param>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> mapFunction)
        {
            if (enumerable.IsNull())
            {
                throw new ArgumentNullException("Enumerable");
            }

            if (mapFunction.IsNull())
            {
                throw new ArgumentNullException("MapFunction");
            }

            foreach (T item in enumerable)
            {
                mapFunction(item);
            }
        }

        /// <summary>
        /// Returns all distinct elements of the given source, where "distinctness"
        /// is determined via a projection and the default equality comparer for the projected type.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="keySelector">Projection for determining "distinctness"</param>
        /// <returns>A sequence consisting of distinct elements from the source sequence,
        /// comparing them by the specified key projection.</returns>
        /// <remarks>This operator uses deferred execution and streams the results, although
        /// a set of already-seen keys is retained. If a key is seen multiple times,
        /// only the first element with that key is returned.</remarks>
        [DebuggerStepThrough]
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
                                                                     Func<TSource, TKey> keySelector)
        {
            return source.DistinctBy(keySelector, null);
        }

        /// <summary>
        /// Returns all distinct elements of the given source, where "distinctness"
        /// is determined via a projection and the specified comparer for the projected type.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="keySelector">Projection for determining "distinctness"</param>
        /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <c>TSource</c> is used.</param>
        /// <returns>A sequence consisting of distinct elements from the source sequence,
        /// comparing them by the specified key projection.</returns>
        /// <remarks>This operator uses deferred execution and streams the results, although
        /// a set of already-seen keys is retained. If a key is seen multiple times,
        /// only the first element with that key is returned.</remarks>
        [DebuggerStepThrough]
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
                                                                     Func<TSource, TKey> keySelector,
                                                                     IEqualityComparer<TKey> comparer)
        {
            if (source.IsNull())
            {
                throw new ArgumentNullException("Source");
            }

            if (keySelector.IsNull())
            {
                throw new ArgumentNullException("KeySelector");
            }

            var knownKeys = new HashSet<TKey>(comparer);

            return source.Where(element => knownKeys.Add(keySelector(element)));
        }

        /// <summary>
        /// Removes all of the items that match the provided condition
        /// </summary>
        /// <typeparam name="T">The type of the items in the list</typeparam>
        /// <param name="list">The list to modify</param>
        /// <param name="whereEvaluator">The test to determine if an item should be removed</param>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static void RemoveAll<T>(this IList<T> list, Func<T, Boolean> whereEvaluator)
        {
            for (var count = list.Count - AppConsts.ONE; count >= AppConsts.NONE; count--)
            {
                if (whereEvaluator(list[count]))
                {
                    list.RemoveAt(count);
                }
            }
        }

        /// <summary>
        /// Combines a Linq Where and Select.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="enumerable">The i enumerable.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>A System.Collections.Generic.IEnumerable&lt;TResult&gt;</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static IEnumerable<TResult> WhereSelect<TSource, TResult>(this IEnumerable<TSource> enumerable,
                                                                         Predicate<TSource> filter,
                                                                         Converter<TSource, TResult> selector)
        {
            return enumerable.Where(t => filter(t)).Select(t => selector(t));
        }

        /// <summary>
        /// Combines a Linq Where and Select.
        /// </summary>
        /// <typeparam name="T">The Type.</typeparam>
        /// <param name="enumerable">The i enumerable.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>A System.Collections.Generic.IEnumerable&lt;TResult&gt;</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static IEnumerable<T> WhereSelect<T>(this IEnumerable<T> enumerable, Predicate<T> filter)
        {
            return enumerable.Where(t => filter(t)).Select(t => t);
        }

        /// <summary>
        /// Check IEnumerable of type T for empty and null
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="enumerable">IEnumberabl</param>
        /// <returns><c>true</c> if [is null or empty] [the specified enumerable]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.IsNull() || enumerable.None();
        }

        /// <summary>
        /// Check array of type T for empty and null
        /// </summary>
        /// <typeparam name="T">The Type</typeparam>
        /// <param name="array">IEnumberabl</param>
        /// <returns><c>true</c> if [is null or empty] [the specified array]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean IsNullOrEmpty<T>(this T[] array)
        {
            return array.IsNull() || array.Length == AppConsts.NONE;
        }

        /// <summary>
        /// Indicates that the  IEnumerable has no items
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>A Boolean</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean None<T>(this IEnumerable<T> list)
        {
            return !list.Any();
        }

        /// <summary>
        /// Clones a List collection of ICloneable objects
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="listToClone">The List collection to clone</param>
        /// <returns>A List&lt;T&gt;</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static List<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.IsNull() ? null : listToClone.Select(item => (T)((ICloneable)item).Clone()).ToList();
        }

        /// <summary>
        /// Clones the specified dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>A Dictionary&lt;TKey,TValue&gt;</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static IDictionary<TKey, TValue> Clone<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary.IsNullOrEmpty())
            {
                return new Dictionary<TKey, TValue>();
            }

            var clone = new Dictionary<TKey, TValue>();

            Boolean keyIsCloneable = default(TKey) is ICloneable;

            Boolean valueIsCloneable = default(TValue) is ICloneable;

            foreach (KeyValuePair<TKey, TValue> keyValuePair in dictionary)
            {
                TKey key = default(TKey);
                TValue value = default(TValue);

                key = keyIsCloneable ? (TKey)((ICloneable)keyValuePair.Key).Clone() : keyValuePair.Key;

                value = valueIsCloneable ? (TValue)((ICloneable)keyValuePair.Value).Clone() : keyValuePair.Value;

                clone.Add(key, value);
            }

            return clone;
        }

        /// <summary>
        /// Differences from the specified actual.
        /// </summary>
        /// <param name="actual">The actual.</param>
        /// <param name="expected">The expected.</param>
        /// <returns>A Dictionary&lt;System.String,INTSOF.Utils.Extensions.Difference&gt;</returns>
        /// <remarks></remarks>
        public static Dictionary<String, Difference> Differences(this IDictionary<String, Int32> actual,
                                                                 IDictionary<String, Int32> expected)
        {
            // If the items are the same object, return nothing.
            if (actual == expected)
            {
                return new Dictionary<String, Difference>();
            }

            // If actual is empty (or null), return all expected.
            if (actual.IsNullOrEmpty())
            {
                return expected.ToDictionary(item => item.Key, item => new Difference(0, item.Value));
            }

            // If expected is empty(or null), return all actual.
            if (expected.IsNullOrEmpty())
            {
                return actual.ToDictionary(item => item.Key, item => new Difference(item.Value, 0));
            }

            // Create a new Concurrent dictionary to house the differences, pre-populated with actual
            // (ConcurrentDictionary is used because it supports modifying the collection in a loop
            var mergeDictionary = new ConcurrentDictionary<String, Difference>(expected.WhereSelect(dict => dict.Value > 0)
                                                                                       .ToDictionary(item => item.Key, item => new Difference(item.Value, 0)));
            foreach (KeyValuePair<String, Int32> item in actual)
            {
                Difference value;
                Difference nullOut;

                if (mergeDictionary.TryGetValue(item.Key, out value))
                {
                    if (value.ActualValue == item.Value)
                    {
                        mergeDictionary.TryRemove(item.Key, out nullOut);
                    }
                    else
                    {
                        mergeDictionary[item.Key].ExpectedValue = item.Value;
                    }
                }
                else if (item.Value == AppConsts.NONE)
                {
                    mergeDictionary.TryRemove(item.Key, out nullOut);
                }
                else
                {
                    mergeDictionary.TryAdd(item.Key, new Difference(0, item.Value));
                }
            }

            return mergeDictionary.ToDictionary(item => item.Key, item => item.Value);
        }

        /// <summary>
        /// Checks if an object exists in an array at a specific location (index)
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="index">Integer position in the array</param>
        /// <returns>A System.Boolean</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean IndexExists<T>(this T[] list, Int32 index)
        {
            if (index < AppConsts.NONE)
            {
                throw new ArgumentException("Index must be >= 0.");
            }

            return !list.IsNull() && list.GetUpperBound(0) >= index;
        }

        /// <summary>
        /// Checks if an object exists in an IEnumerable at a specific location (index)
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="index">Integer position in the IEnumerable</param>
        /// <returns>A System.Boolean</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean IndexExists<T>(this IEnumerable<T> list, Int32 index)
        {
            if (index < AppConsts.NONE)
            {
                throw new ArgumentException("Index must be >= 0.");
            }

            return list.ToArray().IndexExists(index);
        }

        /// <summary>
        /// Returns the first element T in a collection, or a new instance of T.
        /// </summary>
        /// <typeparam name="T">Class with non-parameterized constructor</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>A T</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static T FirstOrNew<T>(this IEnumerable<T> enumerable) where T : class, new()
        {
            return enumerable.FirstOrDefault() ?? new T();
        }

        #region IEnumerable Index Helper

        /// <summary>
        /// Provides a ForEach loop with an accessible index.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="handler">The handler.</param>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static void ForEachWithIndex<T>(this IEnumerable<T> enumerable, Action<T, Int32> handler)
        {
            Int32 index = AppConsts.NONE;

            foreach (T item in enumerable)
            {
                handler(item, index++);
            }
        }

        /// <summary>
        /// Converts the enumerable to an Index dictionary.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>A System.Collections.Generic.IDictionary&lt;T,System.Int32&gt;</returns>
        /// <remarks>The dictionary key will be the object, and the value will be it's index position in the IEnumerable.</remarks>
        [DebuggerStepThrough]
        public static IDictionary<T, Int32> ToIndexDictionary<T>(this IEnumerable<T> source)
        {
            return source.SelectWithIndex((item, index) => new { item, index }).ToDictionary(a => a.item, a => a.index);
        }

        /// <summary>
        /// Returns a collection with an index.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>A collection of TResult.</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static IEnumerable<TResult> SelectWithIndex<T, TResult>(this IEnumerable<T> source,
                                                                       Func<T, Int32, TResult> selector)
        {
            return source.Select(selector);
        }

        /// <summary>
        /// Adds INDEX to en IEnumerable collection
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="enumerable">The original collection.</param>
        /// <returns>A collection of Item of type T.</returns>
        /// <example>
        /// This code shows how to use the WithIndex extension method.
        ///   <code>
        /// ienumerable.WithIndex()
        /// .ForEach(item=&gt;
        /// {  console.write(item.index);
        /// if (item.IsFirst) console.write("First Item");
        /// if (item.IsLast) console.write("Last Item");
        /// }
        ///   </code>
        ///   </example>
        /// <remarks>This allows you to iterate through an IEnumberable collection and reference the index of each item using a foreach loop</remarks>
        [DebuggerStepThrough]
        public static IEnumerable<Item<T>> WithIndex<T>(this IEnumerable<T> enumerable)
        {
            Item<T> item = null;

            foreach (Item<T> next in enumerable.Select(value => new Item<T> { Index = AppConsts.NONE, Value = value, IsLast = false }))
            {
                if (!item.IsNull())
                {
                    next.Index = item.Index + 1;
                    yield return item;
                }

                item = next;
            }

            if (!item.IsNull())
            {
                item.IsLast = true;
                yield return item;
            }
        }

        /// <summary>
        /// Class to implement index functions in an IEnumerable collection (
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <seealso cref="Extensions.WithIndex{T}"/>
        /// <remarks></remarks>
        [DefaultProperty("Value")]
        public sealed class Item<T>
        {
            /// <summary>
            /// Gets or sets Index.
            /// </summary>
            /// <value>The index.</value>
            /// <remarks></remarks>
            public Int32 Index
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets Value.
            /// </summary>
            /// <value>The value.</value>
            /// <remarks></remarks>
            public T Value
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets a value indicating whether IsLast.
            /// </summary>
            /// <value><c>true</c> if this instance is last; otherwise, <c>false</c>.</value>
            /// <remarks></remarks>
            public Boolean IsLast
            {
                get;
                set;
            }

            /// <summary>
            /// Determines if the item is first in a collection
            /// </summary>
            /// <value><c>true</c> if this instance is first; otherwise, <c>false</c>.</value>
            /// <remarks></remarks>
            public Boolean IsFirst
            {
                get
                {
                    return this.Index == AppConsts.NONE;
                }
            }

            /// <summary>
            /// Determines if an item is first or last in a collection
            /// </summary>
            /// <value><c>true</c> if this instance is outer; otherwise, <c>false</c>.</value>
            /// <remarks></remarks>
            public Boolean IsOuter
            {
                get
                {
                    return this.IsFirst || this.IsLast;
                }
            }

            /// <summary>
            /// Determines if an item is not first and not last in a collection
            /// </summary>
            /// <value><c>true</c> if this instance is inner; otherwise, <c>false</c>.</value>
            /// <remarks></remarks>
            public Boolean IsInner
            {
                get
                {
                    return !this.IsOuter;
                }
            }

            /// <summary>
            /// Determines if an item has an even index in a collection
            /// </summary>
            /// <remarks></remarks>
            public Boolean IsEven
            {
                get
                {
                    return this.Index % 2 == AppConsts.NONE;
                }
            }

            /// <summary>
            /// Determines if an item has an odd index in a collection
            /// </summary>
            /// <value><c>true</c> if this instance is odd; otherwise, <c>false</c>.</value>
            /// <remarks></remarks>
            public Boolean IsOdd
            {
                get
                {
                    return !this.IsEven;
                }
            }

            /// <summary>
            /// Returns the Value or a default value for a nullable value
            /// </summary>
            /// <param name="item">The item.</param>
            /// <returns>The result of the conversion.</returns>
            /// <remarks></remarks>
            public static implicit operator T(Item<T> item)
            {
                return item.Value;
            }
        }

        #endregion

        #endregion

        #region String Extensions

        /// <summary>
        /// Returns the Indexes of all occurrences of a search String..
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="search">The search.</param>
        /// <returns>A System.Collections.IEnumerable</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static IEnumerable<Int32> IndexOfAll(this String source, String search)
        {
            if (search.IsNullOrEmpty())
            {
                throw new ArgumentException("Search cannot be null or empty.");
            }

            Int32 position;

            Int32 offset = AppConsts.NONE;

            Int32 length = search.Length;

            while ((position = source.IndexOf(search, offset)) != -AppConsts.ONE)
            {
                yield return position;

                offset = position + length;
            }
        }

        /// <summary>
        /// Removes a list of characters from a String
        /// </summary>
        /// <param name="input">Source String</param>
        /// <param name="pattern">String of characters to remove</param>
        /// <returns>Original String minus removed characters</returns>
        /// <example>
        /// myString ="This$Is[My,String";
        /// myString.Remove(",""\$]")
        ///   </example>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static String Remove(this String input, String pattern)
        {
            if (pattern.IsNullOrEmpty())
            {
                throw new ArgumentException("Pattern cannot be null or empty.");
            }

            if (!pattern.IsValidRegex())
            {
                throw new ArgumentException("Pattern must be a valid RegEx String.");
            }

            return Regex.Replace(input, String.Format(@"[{0}]", Regex.Escape(pattern)), String.Empty);
        }

        /// <summary>
        /// Determines whether [is valid RegEx] [the specified pattern].
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns><c>true</c> if [is valid RegEx] [the specified pattern]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean IsValidRegex(this String pattern)
        {
            if (pattern.IsNullOrEmpty())
            {
                return false;
            }

            try
            {
                Regex.Match(String.Empty, pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Removes the whitespace.
        /// </summary>
        /// <param name="inputString">The input String.</param>
        /// <returns>A System.String</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static String RemoveWhitespace(this String inputString)
        {
            try
            {
                return new Regex(@"\s*").Replace(inputString, String.Empty);
            }
            catch (Exception)
            {
                return inputString;
            }
        }

        /// <summary>
        /// Trims the whitespace.
        /// </summary>
        /// <param name="inputString">The input String.</param>
        /// <returns>A System.String</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static String TrimWhiteSpace(this String inputString)
        {
            return TrimHelper(inputString, true, true);
        }

        /// <summary>
        /// Trims the start whitespace.
        /// </summary>
        /// <param name="inputString">The input String.</param>
        /// <returns>A System.String</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static String TrimStartWhiteSpace(this String inputString)
        {
            return TrimHelper(inputString, true);
        }

        /// <summary>
        /// Trims the end whitespace.
        /// </summary>
        /// <param name="inputString">The input String.</param>
        /// <returns>A System.String</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static String TrimEndWhiteSpace(this String inputString)
        {
            return TrimHelper(inputString, trimEnd: true);
        }


        /// <summary>
        /// Removes extra spaces between words
        /// </summary>
        /// <param name="inputString">The   input   String.</param>
        /// <returns>A System.String</returns>
        /// <remarks></remarks>

        public static String RemoveExtraSpaces(this String str)
        {
            String result = "";
            Regex regulEx = new Regex(@"[\s]+");
            result = regulEx.Replace(str, " ");
            return result;
        }

        /// <summary>
        /// Determines whether the specified input String is empty.
        /// </summary>
        /// <param name="inputString">The input String.</param>
        /// <returns><c>true</c> if the specified input String is empty; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean IsEmpty(this String inputString)
        {
            return inputString.All(char.IsWhiteSpace);
        }

        /// <summary>
        /// Trims the helper.
        /// </summary>
        /// <param name="inputString">The input String.</param>
        /// <param name="trimStart">if set to <c>true</c> [trim start].</param>
        /// <param name="trimEnd">if set to <c>true</c> [trim end].</param>
        /// <returns>A System.String</returns>
        /// <remarks></remarks>
        private static String TrimHelper(this String inputString, Boolean trimStart = false, Boolean trimEnd = false)
        {
            // End will point to the first non-trimmed character on the right
            // Start will point to the first non-trimmed character on the Left
            Int32 end = inputString.Length - 1;
            Int32 start = AppConsts.NONE;

            // Trim specified characters. 
            if (trimStart)
            {
                for (start = AppConsts.NONE; start < inputString.Length; start++)
                {
                    if (!inputString[start].IsWhiteSpace())
                    {
                        break;
                    }
                }
            }

            if (trimEnd)
            {
                for (end = inputString.Length - AppConsts.ONE; end >= start; end--)
                {
                    if (!inputString[end].IsWhiteSpace())
                    {
                        break;
                    }
                }
            }

            Int32 length = end - start + 1;

            return length == inputString.Length
                      ? inputString
                      : (length == AppConsts.NONE ? String.Empty : inputString.Substring(start, length));
        }

        /// <summary>
        /// Determines whether [is white space] [the specified character].
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="isUnicode">if set to <c>true</c> [is unicode].</param>
        /// <returns><c>true</c> if [is white space] [the specified character]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        private static Boolean IsWhiteSpace(this char character, Boolean isUnicode = false)
        {
            if (isUnicode)
            {
                // todo: write UniCode check, if needed
                return false;
            }

            return (character == ' ') || (character >= '\x0000' && character <= '\x001f') || character >= '\x007f'
                   || character == '\x0085';
        }

        /// <summary>
        /// Performs a case-insensitive comparison of to strings
        /// </summary>
        /// <param name="compareFrom">The first String.</param>
        /// <param name="compareTo">The String to compare to.</param>
        /// <returns>True if the strings are equal.</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean TextEquals(this String compareFrom, String compareTo)
        {
            return String.Equals(compareFrom, compareTo, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets a String, or null if the String is empty
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A System.String</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static String NullIfEmpty(this String input)
        {
            return input.IsNullOrEmpty() ? null : input;
        }

        /// <summary>
        /// Check String for empty and null.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns><c>true</c> if [is null or empty] [the specified obj]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean IsNullOrEmpty(this object obj)
        {
            return obj is String ? ((String)obj).IsNullOrEmpty() : obj.IsNull();
        }

        /// <summary>
        /// Check String for empty and null.
        /// </summary>
        /// <param name="stringObject">The String to check.</param>
        /// <returns>True if null or empty, otherwise False</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean IsNullOrEmpty(this String stringObject)
        {
            return String.IsNullOrWhiteSpace(stringObject);
        }

        [DebuggerStepThrough]
        public static bool IsNullOrWhiteSpace(this string stringObject)
        {
            return String.IsNullOrWhiteSpace(stringObject);
        }

        /// <summary>
        /// Formats the String with the supplied data.
        /// </summary>
        /// <param name="source">The format of the String to return.</param>
        /// <param name="data">The data to apply to the format.</param>
        /// <returns>The formatted String.</returns>
        /// <exception cref="System.ArgumentNullException">
        ///   </exception>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static String Format(this String source, params object[] data)
        {
            if (data.IsNullOrEmpty())
            {
                throw new ArgumentException("Data must not be null or empty.");
            }

            if (source.IndexOfAll(@"{").Count() != data.Length)
            {
                throw new ArgumentException("The number of data elements must match the number of placeholders.");
            }

            return String.Format(source, data);
        }

        private static IEnumerable<ITextExpression> SplitFormat(string format)
        {
            var exprEndIndex = -1;
            int expStartIndex;

            do
            {
                expStartIndex = format.IndexOfExpressionStart(exprEndIndex + 1);

                if (expStartIndex < 0)
                {
                    //everything after last end brace index.
                    if (exprEndIndex + 1 < format.Length)
                    {
                        yield return new LiteralFormat(format.Substring(exprEndIndex + 1));
                    }
                    break;
                }

                if (expStartIndex - exprEndIndex - 1 > 0)
                {
                    //everything up to next start brace index
                    yield return new LiteralFormat(format.Substring(exprEndIndex + 1, expStartIndex - exprEndIndex - 1));
                }

                var endBraceIndex = format.IndexOfExpressionEnd(expStartIndex + 1);

                if (endBraceIndex < 0)
                {
                    //rest of string, no end brace (could be invalid expression)
                    yield return new FormatExpression(format.Substring(expStartIndex));
                }
                else
                {
                    exprEndIndex = endBraceIndex;

                    //everything from start to end brace.
                    yield return new FormatExpression(format.Substring(expStartIndex, endBraceIndex - expStartIndex + 1));

                }
            }
            while (expStartIndex > -1);
        }

        /// <summary>Indexes the of expression start.</summary>
        /// <param name="format">The format.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>A System.Int32</returns>
        private static int IndexOfExpressionStart(this string format, int startIndex)
        {
            var index = format.IndexOf('{', startIndex);

            if (index == -1)
            {
                return index;
            }

            //peek ahead.
            if (index + 1 < format.Length)
            {
                var nextChar = format[index + 1];

                return nextChar == '{' ? IndexOfExpressionStart(format, index + 2) : index;
            }

            return index;
        }

        /// <summary>Indexes the of expression end.</summary>
        /// <param name="format">The format.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>A System.Int32</returns>
        private static int IndexOfExpressionEnd(this string format, int startIndex)
        {
            var endBraceIndex = format.IndexOf('}', startIndex);

            if (endBraceIndex == -1)
            {
                return endBraceIndex;
            }
            //start peeking ahead until there are no more braces...
            // }}}}
            int braceCount = 0;

            for (var i = endBraceIndex + 1; i < format.Length; i++)
            {
                if (format[i] == '}')
                {
                    braceCount++;
                }
                else
                {
                    break;
                }
            }

            return braceCount % 2 == 1 ? IndexOfExpressionEnd(format, endBraceIndex + braceCount + 1) : endBraceIndex;
        }

        /// <summary>
        /// Get String within String array for particular pattern.
        /// </summary>
        /// <param name="stringArray">String array.</param>
        /// <param name="pattern">Pattern for find the String.</param>
        /// <returns>Return complete String.</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static String ControlIDForPattern(this String[] stringArray, String pattern)
        {

            String returnValue = null;
            foreach (String searchString in stringArray)
            {
                if (!searchString.IsNull() && searchString.IndexOf(pattern) >= AppConsts.NONE)
                {
                    returnValue = searchString;
                    break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Convert the value to particular type if value exist otherwise return null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueAsString"></param>
        /// <returns></returns>
        public static T? GetValueOrNull<T>(this String valueAsString) where T : struct
        {
            if (valueAsString.IsNullOrEmpty())
            {
                return null;
            }
            return (T)Convert.ChangeType(valueAsString, typeof(T));
        }

        public static string ToFormatApostrophe(this String value)
        {
            var countAposNo = value.Split('\'').Length - 1;

            if (countAposNo > 0)
            {
                return (value.Replace("'", "''"));
            }
            else
            {
                return value;
            }
        }

        #region HTML Encoding
        private static Boolean? _isHtmlEncodingActivated = null;
        public static Boolean IsHtmlEncodingActivated
        {
            get
            {
                if (_isHtmlEncodingActivated == null)
                {
                    _isHtmlEncodingActivated = System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("ActivateHtmlEncoding") ?
                              Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ActivateHtmlEncoding"]) : false;
                }
                return Convert.ToBoolean(_isHtmlEncodingActivated);
            }
        }

        [DebuggerStepThrough]
        public static String HtmlEncode(this String value)
        {
            if (String.IsNullOrEmpty(value))
                return value;

            if (IsHtmlEncodingActivated && !HTMLEncodeOverride)
                return System.Web.HttpUtility.HtmlEncode(value);
            return value;
        }

        [DebuggerStepThrough]
        public static String HtmlDecode(this String value)
        {
            if (String.IsNullOrEmpty(value))
                return value;

            if (IsHtmlEncodingActivated && !HTMLEncodeOverride)
                return System.Web.HttpUtility.HtmlDecode(value);
            return value;
        }

        #endregion

        #endregion

        #region Stream Extensions

        /// <summary>
        /// Converts an array to a memory stream
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns>A System.IO.Stream</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Stream ToStream(this byte[] array)
        {
            using (Stream stream = new MemoryStream())
            {
                stream.Write(array, 0, array.Length);

                return stream;
            }
        }

        #endregion

        #region Enumeration Extensions and Helpers

        /// <summary>
        /// Returns an array of values in the enum.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns>An array of values in the enum</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static T[] GetValuesArray<T>() where T : struct, IComparable, IFormattable, IConvertible
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        /// <summary>
        /// Returns the values for the given enum as an immutable list.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns></returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static IList<T> GetValues<T>() where T : struct, IComparable, IFormattable, IConvertible
        {
            return EnumHelper<T>.Values;
        }

        /// <summary>
        /// Returns an array of names in the enum.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns>An array of names in the enum</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static String[] GetNamesArray<T>() where T : struct, IComparable, IFormattable, IConvertible
        {
            return Enum.GetNames(typeof(T));
        }

        /// <summary>
        /// Returns the names for the given enum as an immutable list.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns>An array of names in the enum</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static IList<String> GetNames<T>() where T : struct, IComparable, IFormattable, IConvertible
        {
            return EnumHelper<T>.Names;
        }

        /// <summary>
        /// Checks whether the value is a named value for the type.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Value to test</param>
        /// <returns>True if this value has a name, False otherwise.</returns>
        /// <remarks>For flags enums, it is possible for a value to be a valid
        /// combination of other values without being a named value
        /// in itself. To test for this possibility, use IsValidCombination.</remarks>
        [DebuggerStepThrough]
        public static Boolean IsNamedValue<T>(this T value) where T : struct, IComparable, IFormattable, IConvertible
        {
            return GetValues<T>().Contains(value);
        }

        /// <summary>
        /// Returns the description for the given value,
        /// as specified by DescriptionAttribute, or null
        /// if no description is present.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="item">Value to fetch description for</param>
        /// <returns>The description of the value, or null if no description
        /// has been specified (but the value is a named value).</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="item"/>
        /// is not a named member of the enum
        ///   </exception>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static String GetDescription<T>(this T item) where T : struct, IComparable, IFormattable, IConvertible
        {
            String description;

            if (!EnumHelper<T>.ValueToDescriptionMap.TryGetValue(item, out description))
            {
                throw new ArgumentOutOfRangeException("item");
            }

            return description;
        }

        /// <summary>
        /// Returns the description for the given value,
        /// as specified by DescriptionAttribute, or value
        /// if no description is present.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="item">Value to fetch description for</param>
        /// <returns>The description of the value, or value if no description
        /// has been specified (but the value is a named value).</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="item"/>
        /// is not a named member of the enum
        ///   </exception>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static String ToDescription<T>(this T item) where T : struct, IComparable, IFormattable, IConvertible
        {
            String description;

            return EnumHelper<T>.ValueToDescriptionMap.TryGetValue(item, out description)
                      ? (description ?? item.ToString())
                      : item.ToString();
        }

        /// <summary>
        /// Attempts to find a value with the given description.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="description">Description to find</param>
        /// <param name="value">Enum value corresponding to given description (on return)</param>
        /// <returns>True if a value with the given description was found,
        /// false otherwise.</returns>
        /// <remarks>More than one value may have the same description. In this unlikely
        /// situation, the first value with the specified description is returned.</remarks>
        [DebuggerStepThrough]
        public static Boolean TryParseDescription<T>(String description, out T value)
           where T : struct, IComparable, IFormattable, IConvertible
        {
            if (description.IsNullOrEmpty())
            {
                throw new ArgumentException("Description cannot be null or empty.");
            }

            return EnumHelper<T>.DescriptionToValueMap.TryGetValue(description, out value);
        }

        /// <summary>
        /// Parses the name of an enum value.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="@enum">The enum.</param>
        /// <param name="name">The name.</param>
        /// <returns>The parsed value</returns>
        /// <exception cref="ArgumentException">
        /// The name could not be parsed.
        ///   </exception>
        /// <remarks>This method only considers named values: it does not parse comma-separated
        /// combinations of flags enums.</remarks>
        [DebuggerStepThrough]
        public static T ParseName<T>(this T @enum, String name) where T : struct, IComparable, IFormattable, IConvertible
        {
            if (name.IsNullOrEmpty())
            {
                throw new ArgumentException("name");
            }

            T value;

            if (!TryParseName(name, out value))
            {
                throw new ArgumentException("Unknown name", name);
            }

            return value;
        }

        /// <summary>
        /// Parses the name of an enum value.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>The parsed value</returns>
        /// <exception cref="ArgumentException">
        /// The name could not be parsed.
        ///   </exception>
        /// <remarks>This method only considers named values: it does not parse comma-separated
        /// combinations of flags enums.</remarks>
        [DebuggerStepThrough]
        public static T ParseName<T>(String name) where T : struct, IComparable, IFormattable, IConvertible
        {
            if (name.IsNullOrEmpty())
            {
                throw new ArgumentException("name");
            }

            T value;

            if (!TryParseName(name, out value))
            {
                throw new ArgumentException("Unknown name", name);
            }

            return value;
        }

        /// <summary>
        /// Parses the name of an enum value.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>The parsed value</returns>
        /// <exception cref="ArgumentException">
        /// The name could not be parsed.
        ///   </exception>
        /// <remarks>This method only considers named values: it does not parse comma-separated
        /// combinations of flags enums.</remarks>
        [DebuggerStepThrough]
        public static T ParseNameOrDefault<T>(String name) where T : struct, IComparable, IFormattable, IConvertible
        {
            if (name.IsNullOrEmpty())
            {
                throw new ArgumentException("name");
            }

            T value;

            return !TryParseName(name, out value) ? default(T) : value;
        }

        /// <summary>
        /// Attempts to find a value for the specified name.
        /// Only names are considered - not numeric values.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="name">Name to parse</param>
        /// <param name="value">Enum value corresponding to given name (on return)</param>
        /// <returns>Whether the parse attempt was successful or not</returns>
        /// <remarks>If the name is not parsed, <paramref name="value"/> will
        /// be set to the zero value of the enum. This method only
        /// considers named values: it does not parse comma-separated
        /// combinations of flags enums.</remarks>
        [DebuggerStepThrough]
        public static Boolean TryParseName<T>(String name, out T value)
           where T : struct, IComparable, IFormattable, IConvertible
        {
            if (name.IsNullOrEmpty())
            {
                throw new ArgumentException("name");
            }

            Int32 index = EnumHelper<T>.Names.IndexOf(name.ToLower(CultureInfo.CurrentCulture));

            if (index == -1)
            {
                value = default(T);

                return false;
            }

            value = EnumHelper<T>.Values[index];

            return true;
        }

        /// <summary>
        /// Verifies whether a passed String is a valid Enum of type T
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>The try parse name.</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean TryParseName<T>(this String name) where T : struct, IComparable, IFormattable, IConvertible
        {
            if (name.IsNullOrEmpty())
            {
                throw new ArgumentException("name");
            }

            Int32 index = EnumHelper<T>.Names.IndexOf(name.ToLower(CultureInfo.CurrentCulture));

            return index != -1;
        }

        /// <summary>
        /// Returns the underlying type for the enum
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns>The underlying type (Byte, Int32 etc) for the enum</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Type GetUnderlyingType<T>() where T : struct, IComparable, IFormattable, IConvertible
        {
            return EnumHelper<T>.UnderlyingType;
        }

        /// <summary>
        /// The described.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        public struct Described<T> where T : struct
        {
            /// <summary>
            /// The _value.
            /// </summary>
            private T _value;

            /// <summary>
            /// Initializes a new instance of the <see cref="Described{T}"/> struct.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <remarks></remarks>
            public Described(T value)
            {
                this._value = value;
            }

            /// <summary>
            /// The to String.
            /// </summary>
            /// <returns>The to String.</returns>
            /// <remarks></remarks>
            public override String ToString()
            {
                String text = this._value.ToString();

                object[] attr = typeof(T).GetField(text).GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attr.Length == 1)
                {
                    text = ((DescriptionAttribute)attr[0]).Description;
                }

                return text;
            }

            /// <summary>
            /// The op_ implicit.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns>The result of the conversion.</returns>
            /// <remarks></remarks>
            public static implicit operator Described<T>(T value)
            {
                return new Described<T>(value);
            }

            /// <summary>
            /// The op_ implicit.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns>The result of the conversion.</returns>
            /// <remarks></remarks>
            public static implicit operator T(Described<T> value)
            {
                return value._value;
            }
        }

        #endregion

        #region XDocument Extensions

        /// <summary>Attributes the value.</summary>
        /// <param name="element">The element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>A System.String</returns>
        public static string AttributeValue(this XElement element, string attributeName)
        {
            var attribute = element.Attribute(attributeName);

            if (attribute == null)
            {
                return string.Empty;
            }

            return attribute.Value;
        }

        #endregion

        #region Encrypt and Decrypt Query String

        /// <summary>
        /// This is an extension method for decrypting the query string.
        /// </summary>
        /// <param name="sourceDictionary">Source dictionary value.</param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static void ToDecryptedQueryString(this Dictionary<String, String> sourceDictionary, String args)
        {
            EncryptedQueryString queryString = new EncryptedQueryString(args);

            foreach (KeyValuePair<String, String> kValue in queryString)
            {
                sourceDictionary.Add(kValue.Key, kValue.Value);
            }
        }

        /// <summary>
        /// This is an extension method for EncryptedQueryString which returns the encrypted string for string type dictionary.
        /// </summary>
        /// <param name="sourceDictionary">Source dictionary value.</param>
        /// <returns></returns>
        public static String ToEncryptedQueryString(this Dictionary<String, String> sourceDictionary)
        {
            EncryptedQueryString encryptedQueryString = new EncryptedQueryString(sourceDictionary);
            return encryptedQueryString.ToString();
        }

        #endregion

        #region Search Extensions

        /// <summary>
        /// Create advance search query string.
        /// </summary>
        /// <param name="searchOptions">Search options</param>
        /// <param name="quickSearchOption">Specify search type</param>
        /// <returns>Advance search URL with encrypted query string</returns>
        public static String AdvanceSearchUrl(this Dictionary<String, String> searchOptions, QuickSearchOption quickSearchOption)
        {
            // Convert dictionary search option to string
            StringBuilder stringOptions = new StringBuilder();
            foreach (var key in searchOptions.Keys)
            {
                stringOptions.AppendFormat("{0}{1}{2}{3}", key, SysXSearchConsts.SEARCH_VALUE_SEPERATOR, searchOptions[key], SysXSearchConsts.SEARCH_FIELD_SEPERATOR);
            }

            // Create encrypted query string to for search and added required search elements.
            Dictionary<String, String> encryptedQueryString = new Dictionary<String, String>();
            encryptedQueryString.Add(SysXSearchConsts.QUICK_SEARCH_OPTION, quickSearchOption.ToString());
            encryptedQueryString.Add(SysXSearchConsts.VIEW_MODE, ViewMode.Search.ToString());
            encryptedQueryString.Add(SysXSearchConsts.SEARCH_MODE, SearchViewMode.AdvanceSearch.ToString());
            encryptedQueryString.Add(SysXSearchConsts.LOAD_RESULTS, true.ToString());
            encryptedQueryString.Add(SysXSearchConsts.SEARCH_OPTIONS, stringOptions.ToString());

            return String.Format("default.aspx?{0}={1}", AppConsts.QUERYSTRING_ARGUMENT, encryptedQueryString.ToEncryptedQueryString());
        }

        /// <summary>
        /// This is an extension method for extract search option of query string.
        /// </summary>
        /// <param name="sourceDictionary">Source dictionary value.</param>
        /// <param name="args">encrypted query string</param>
        public static void FromEncryptedQueryString(this Dictionary<String, String> sourceDictionary, String args)
        {
            if (!args.Equals(String.Empty))
            {
                String[] keyValues = args.TrimEnd(SysXSearchConsts.SEARCH_FIELD_SEPERATOR).Split(SysXSearchConsts.SEARCH_FIELD_SEPERATOR);

                foreach (String keyValue in keyValues)
                {
                    String key = keyValue.Split(SysXSearchConsts.SEARCH_VALUE_SEPERATOR)[AppConsts.NONE];
                    String value = keyValue.Split(SysXSearchConsts.SEARCH_VALUE_SEPERATOR)[AppConsts.ONE];
                    sourceDictionary.Add(key, value);
                }
            }
        }

        #endregion

        #region Guidelines

        public static String GetValue(this Dictionary<String, String> list, String fieldName)
        {
            String strValue;
            list.TryGetValue(fieldName, out strValue);
            return strValue;
        }

        #endregion

        #region Interface ITextExpression

        public interface ITextExpression
        {
            string Eval(object o);
        }

        #endregion

        #region Class FormatExpression

        public class FormatExpression : ITextExpression
        {
            private readonly bool _invalidExpression;

            public FormatExpression(string expression)
            {
                if (!expression.StartsWith("{") || !expression.EndsWith("}"))
                {
                    this._invalidExpression = true;
                    this.Expression = expression;
                    return;
                }

                string expressionWithoutBraces = expression.Substring(1, expression.Length - 2);

                int colonIndex = expressionWithoutBraces.IndexOf(':');

                if (colonIndex < 0)
                {
                    this.Expression = expressionWithoutBraces;
                }
                else
                {
                    this.Expression = expressionWithoutBraces.Substring(0, colonIndex);
                    this.Format = expressionWithoutBraces.Substring(colonIndex + 1);
                }
            }

            public string Expression { get; private set; }

            public string Format { get; private set; }

            public string Eval(object o)
            {
                if (this._invalidExpression)
                {
                    throw new FormatException("Invalid expression");
                }
                try
                {
                    if (String.IsNullOrEmpty(this.Format))
                    {
                        return (Eval(o, this.Expression) ?? string.Empty).ToString();
                    }

                    return (Eval(o, this.Expression, "{0:" + this.Format + "}") ?? string.Empty);
                }
                catch (Exception)
                {
                    throw new FormatException();
                }
            }

            #region Eval methods

            private static readonly char[] ExpressionPartSeparator = new[] { '.' };

            private static readonly char[] IndexExpressionStartChars = new[] { '[', '(' };

            private static readonly char[] IndexExprEndChars = new[] { ']', ')' };

            private static readonly ConcurrentDictionary<Type, PropertyDescriptorCollection> PropertyCache = new ConcurrentDictionary<Type, PropertyDescriptorCollection>();

            internal static string Eval(object container, string expression, string format)
            {
                object value = Eval(container, expression);

                if ((value.IsNull()) || (value == DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return String.IsNullOrEmpty(format) ? value.ToString() : String.Format(format, value);
                }
            }

            internal static object Eval(object container, string expression)
            {
                if (expression.IsNull())
                {
                    throw new ArgumentNullException("expression");
                }

                expression = expression.Trim();

                if (expression.Length == 0)
                {
                    throw new ArgumentNullException("expression");
                }

                if (container.IsNull())
                {
                    return null;
                }

                string[] expressionParts = expression.Split(ExpressionPartSeparator);

                return Eval(container, expressionParts);
            }

            private static object Eval(object container, string[] expressionParts)
            {
                Debug.Assert((!expressionParts.IsNull()) && (expressionParts.Length != 0), "Invalid expressionParts parameter");

                object property;
                int index;

                for (property = container, index = 0; (index < expressionParts.Length) && (!property.IsNull()); index++)
                {
                    string expressionPart = expressionParts[index];

                    bool isIndexedExpression = expressionPart.IndexOfAny(IndexExpressionStartChars) >= 0;

                    property = isIndexedExpression == false ? GetPropertyValue(property, expressionPart) : GetIndexedPropertyValue(property, expressionPart);
                }

                return property;
            }

            private static PropertyDescriptorCollection GetPropertiesFromCache(object container)
            {
                // Don't cache if the object implements ICustomTypeDescriptor. 
                if (!(container is ICustomTypeDescriptor))
                {
                    PropertyDescriptorCollection properties;

                    Type containerType = container.GetType();

                    if (!PropertyCache.TryGetValue(containerType, out properties))
                    {
                        properties = TypeDescriptor.GetProperties(containerType);
                        PropertyCache.TryAdd(containerType, properties);
                    }
                    return properties;
                }

                return TypeDescriptor.GetProperties(container);
            }

            private static object GetPropertyValue(object container, string propName)
            {
                if (container.IsNull())
                {
                    throw new ArgumentNullException("container");
                }
                if (String.IsNullOrEmpty(propName))
                {
                    throw new ArgumentNullException("propName");
                }

                object property;

                // get a PropertyDescriptor using case-insensitive lookup 
                PropertyDescriptor pd = GetPropertiesFromCache(container).Find(propName, true);

                if (!pd.IsNull())
                {
                    property = pd.GetValue(container);
                }
                else
                {
                    throw new Exception(string.Format("Property '{1}' not found in '{0}'.", container.GetType().FullName, propName));
                }

                return property;
            }

            private static object GetIndexedPropertyValue(object container, string expression)
            {
                if (container.IsNull())
                {
                    throw new ArgumentNullException("container");
                }

                if (String.IsNullOrEmpty(expression))
                {
                    throw new ArgumentNullException("expression");
                }

                object property = null;
                bool isIntIndex = false;

                int indexExpressionStart = expression.IndexOfAny(IndexExpressionStartChars);
                int indexExpressionEnd = expression.IndexOfAny(IndexExprEndChars, indexExpressionStart + 1);

                if ((indexExpressionStart < 0) || (indexExpressionEnd < 0) || (indexExpressionEnd == indexExpressionStart + 1))
                {
                    throw new ArgumentException(expression);
                }

                string propName = null;
                object indexValue = null;

                string index = expression.Substring(indexExpressionStart + 1, indexExpressionEnd - indexExpressionStart - 1).Trim();

                if (indexExpressionStart != 0)
                {
                    propName = expression.Substring(0, indexExpressionStart);
                }

                if (index.Length != 0)
                {
                    if (((index[0] == '"') && (index[index.Length - 1] == '"')) || ((index[0] == '\'') && (index[index.Length - 1] == '\'')))
                    {
                        indexValue = index.Substring(1, index.Length - 2);
                    }
                    else
                    {
                        if (Char.IsDigit(index[0]))
                        {
                            // treat it as a number
                            int parsedIndex;

                            isIntIndex = Int32.TryParse(index, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedIndex);

                            if (isIntIndex)
                            {
                                indexValue = parsedIndex;
                            }
                            else
                            {
                                indexValue = index;
                            }
                        }
                        else
                        {
                            // treat as a string 
                            indexValue = index;
                        }
                    }
                }

                if (indexValue.IsNull())
                {
                    throw new ArgumentException(expression);
                }

                object collectionProperty = string.IsNullOrEmpty(propName) ? container : GetPropertyValue(container, propName);

                if (!collectionProperty.IsNull())
                {
                    var arrayProperty = collectionProperty as Array;

                    if (!arrayProperty.IsNull() && isIntIndex)
                    {
                        property = arrayProperty.GetValue((int)indexValue);
                    }
                    else if ((collectionProperty is IList) && isIntIndex)
                    {
                        property = ((IList)collectionProperty)[(int)indexValue];
                    }
                    else
                    {
                        PropertyInfo propertyInfo = collectionProperty.GetType().GetProperty(
                           "Item", BindingFlags.Public | BindingFlags.Instance, null, null, new[] { indexValue.GetType() }, null);

                        if (!propertyInfo.IsNull())
                        {
                            property = propertyInfo.GetValue(collectionProperty, new[] { indexValue });
                        }
                        else
                        {
                            throw new ArgumentException(string.Format("No accessor was found in '{0}'", collectionProperty.GetType().FullName));
                        }
                    }
                }

                return property;
            }

            #endregion
        }

        #endregion

        #region Class LiteralFormat.


        public class LiteralFormat : ITextExpression
        {
            public LiteralFormat(string literalText)
            {
                LiteralText = literalText;
            }

            public string LiteralText
            {
                get;
                private set;
            }

            public string Eval(object o)
            {
                return this.LiteralText
                   .Replace("{{", "{")
                   .Replace("}}", "}");
            }
        }

        #endregion

        public static SysXClientContext ContextID(this AdminQueueSubContext adminQueueSubContext)
        {
            #region New Enum

            SysXClientContext sysXClientContext = SysXClientContext.Client;

            switch (adminQueueSubContext)
            {
                case AdminQueueSubContext.Client:
                    sysXClientContext = SysXClientContext.Client;
                    break;
                case AdminQueueSubContext.ClientSOW:
                    sysXClientContext = SysXClientContext.ClientSOW;
                    break;
                case AdminQueueSubContext.ClientInspection:
                    sysXClientContext = SysXClientContext.ClientInspection;
                    break;
                case AdminQueueSubContext.ClientContacts:
                    sysXClientContext = SysXClientContext.ClientContacts;
                    break;
                case AdminQueueSubContext.ClientSubClient:
                    sysXClientContext = SysXClientContext.ClientSubClients;
                    break;
                case AdminQueueSubContext.ClientLSCI:
                    sysXClientContext = SysXClientContext.ClientLSCI;
                    break;
                case AdminQueueSubContext.ClientRate:
                    sysXClientContext = SysXClientContext.ClientRate;
                    break;
                case AdminQueueSubContext.ClientMBACode:
                    sysXClientContext = SysXClientContext.ClientMBACode;
                    break;
                case AdminQueueSubContext.ClientOtherService:
                    sysXClientContext = SysXClientContext.ClientOtherService;
                    break;
                case AdminQueueSubContext.ClientServiceSchedule:
                    sysXClientContext = SysXClientContext.ClientServiceSchedule;
                    break;
                case AdminQueueSubContext.ServiceItem:
                    sysXClientContext = SysXClientContext.ClientServiceItem;
                    break;
            }

            return sysXClientContext;

            #endregion

            //SysXClientContext sysXClientContext = SysXClientContext.CMSACTX001;

            //switch (adminQueueSubContext)
            //{
            //    case AdminQueueSubContext.Client:
            //        sysXClientContext = SysXClientContext.CMSACTX001;
            //        break;
            //    case AdminQueueSubContext.ClientSOW:
            //        sysXClientContext = SysXClientContext.CMSWCTX005;
            //        break;
            //    case AdminQueueSubContext.ClientInspection:
            //        sysXClientContext = SysXClientContext.CMINCTX006;
            //        break;
            //    case AdminQueueSubContext.ClientContacts:
            //        sysXClientContext = SysXClientContext.CMSACTX007;
            //        break;
            //    case AdminQueueSubContext.ClientSubClient:
            //        sysXClientContext = SysXClientContext.CMSACTX008;
            //        break;
            //    case AdminQueueSubContext.ClientLSCI:
            //        sysXClientContext = SysXClientContext.CMSACTX009;
            //        break;
            //    case AdminQueueSubContext.ClientRate:
            //        sysXClientContext = SysXClientContext.CMSACTX010;
            //        break;
            //    case AdminQueueSubContext.ClientMBACode:
            //        sysXClientContext = SysXClientContext.CMSACTX011;
            //        break;
            //    case AdminQueueSubContext.ClientOtherService:
            //        sysXClientContext = SysXClientContext.CMSACTX012;
            //        break;
            //    case AdminQueueSubContext.ClientServiceSchedule:
            //        sysXClientContext = SysXClientContext.CMSACTX013;
            //        break;
            //    case AdminQueueSubContext.ServiceItem:
            //        sysXClientContext = SysXClientContext.CMSACTX014;
            //        break;
            //}

            //return sysXClientContext;
        }

        /// <summary>
        /// This is the method just to get body of message from xml message
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static T ConvertStringToEnum<T>(this String parameter)
        {
            return (T)Enum.Parse(typeof(T), parameter);
        }

        /// <summary>
        /// The difference.
        /// </summary>
        /// <remarks></remarks>
        public class Difference
        {
            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Difference"/> class.
            /// </summary>
            /// <param name="actualValue">The actual value.</param>
            /// <param name="expectedValue">The expected value.</param>
            /// <remarks></remarks>
            public Difference(Int32 actualValue, Int32 expectedValue)
            {
                this.ActualValue = actualValue;
                this.ExpectedValue = expectedValue;
            }

            #endregion

            #region Public Properties

            /// <summary>
            /// The actual value.
            /// </summary>
            /// <value>The actual value.</value>
            /// <remarks></remarks>
            public Int32 ActualValue
            {
                get;
                set;
            }

            /// <summary>
            /// The expected value.
            /// </summary>
            /// <value>The expected value.</value>
            /// <remarks></remarks>
            public Int32 ExpectedValue
            {
                get;
                set;
            }

            #endregion
        }

        public static List<Int32> ConvertIntoIntList(this String[] stringArray)
        {
            List<Int32> listInt = new List<Int32>();
            foreach (String str in stringArray)
            {
                listInt.Add(Convert.ToInt32(str));
            }
            return listInt;
        }

        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
               typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }

        public static string GetEnumCodeByXmlAttribute<T>(this Enum value) where T : struct
        {
            Type type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidEnumArgumentException("The type specified is not an Enum.");
            }
            else
            {
                T obj;
                String item = Enum.GetName(type, value);
                // Get fieldinfo for this type
                FieldInfo fieldInfo = type.GetField(item.ToString());

                // Get the stringvalue attributes
                XmlEnumAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(XmlEnumAttribute), false) as XmlEnumAttribute[];

                if (attribs.Length > 0)
                {
                    return attribs[0].Name.Split(',')[0];
                }
            }
            return String.Empty;
        }

        public static T ParseEnumbyCode<T>(this String codevalue) where T : struct
        {
            Type type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidEnumArgumentException("The type specified is not an Enum.");
            }
            else
            {
                T obj;
                foreach (String item in Enum.GetNames(type))
                {
                    // Get fieldinfo for this type
                    FieldInfo fieldInfo = type.GetField(item.ToString());

                    // Get the stringvalue attributes
                    StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

                    // Checking for Value and getting an enum out of it.
                    if (attribs.Any(c => c.StringValue.ToUpper().Equals(codevalue.ToUpper())) && Enum.TryParse<T>(item.ToString(), out obj))
                    {
                        return obj;
                    }
                }
            }

            throw new KeyNotFoundException("Specified Code cannot be found in Enum.");
        }

        public static string GetXmlEnumAttribute(this Enum e)
        {
            // Get the Type of the enum
            Type t = e.GetType();

            // Get the FieldInfo for the member field with the enums name
            FieldInfo info = t.GetField(e.ToString("G"));

            // Check to see if the XmlEnumAttribute is defined on this field
            if (!info.IsDefined(typeof(XmlEnumAttribute), false))
            {
                // If no XmlEnumAttribute then return the string version of the enum.
                return e.ToString("G");
            }

            // Get the XmlEnumAttribute
            object[] o = info.GetCustomAttributes(typeof(XmlEnumAttribute), false);
            XmlEnumAttribute att = (XmlEnumAttribute)o[0];
            return att.Name;
        }


        #region Serialize

        public static string Serialize(this EntityObject target)
        {
            using (var writer = new StringWriter())
            {
                using (XmlWriter xmlWriter = new XmlTextWriter(writer))
                {
                    DataContractSerializer ser = new DataContractSerializer(target.GetType());

                    ser.WriteObject(xmlWriter, target);
                    return writer.ToString();
                }
            }
        }

        public static string Serialize(this object target)
        {
            using (var writer = new StringWriter())
            {
                using (XmlWriter xmlWriter = new XmlTextWriter(writer))
                {
                    DataContractSerializer ser = new DataContractSerializer(target.GetType());

                    ser.WriteObject(xmlWriter, target);
                    return writer.ToString();
                }
            }
        }



        public static string SerializeACFServiceOrder(this Object target)
        {
            XmlSerializer serializer = new XmlSerializer(target.GetType());

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(sb);

            serializer.Serialize(writer, target);
            return writer.ToString();
        }

        public static EntityObject CloneEntity(this EntityObject entityObject)
        {
            DataContractSerializer dcSerializer = new DataContractSerializer(entityObject.GetType());

            EntityObject newObject;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                dcSerializer.WriteObject(memoryStream, entityObject);
                memoryStream.Position = 0;
                newObject = (EntityObject)dcSerializer.ReadObject(memoryStream);
                memoryStream.Flush();
                memoryStream.Close();
            }

            return newObject;
        }

        public static String SerializedEntity(this Object entity)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, entity);
            return System.Convert.ToBase64String(memoryStream.ToArray());
        }

        /// <summary>
        /// Serializes an object to Xml as a string.
        /// </summary>
        /// <typeparam name="T">Datatype T.</typeparam>
        /// <param name="ToSerialize">Object of type T to be serialized.</param>
        /// <returns>Xml string of serialized type T object.</returns>
        public static string SerializeToXmlString(this object target)
        {
            string xmlstream = String.Empty;

            using (MemoryStream memstream = new MemoryStream())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(target.GetType());
                XmlTextWriter xmlWriter = new XmlTextWriter(memstream, Encoding.UTF8);

                xmlSerializer.Serialize(xmlWriter, target);
                xmlstream = UTF8ByteArrayToString(((MemoryStream)xmlWriter.BaseStream).ToArray());
            }

            return xmlstream;
        }

        public static String UTF8ByteArrayToString(Byte[] ArrBytes)
        { return new UTF8Encoding().GetString(ArrBytes); }

        #endregion

        #region Deserialize

        public static T Deserialize<T>(this String targetString, IEnumerable<Type> knownTypes)
        {

            XmlDictionaryReaderQuotas myReaderQuotas = new XmlDictionaryReaderQuotas();

            myReaderQuotas.MaxStringContentLength = int.MaxValue;

            myReaderQuotas.MaxArrayLength = int.MaxValue;

            myReaderQuotas.MaxBytesPerRead = int.MaxValue;

            myReaderQuotas.MaxDepth = int.MaxValue;

            myReaderQuotas.MaxNameTableCharCount = int.MaxValue;

            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(targetString)))
            {
                using (var reader = XmlDictionaryReader.CreateTextReader(stream, myReaderQuotas))
                {
                    var ser = new DataContractSerializer(typeof(T), knownTypes);
                    // Deserialize the data and read it from the instance.
                    return (T)ser.ReadObject(reader);
                }
            }
        }

        public static Object DeserializedEntity(this String str)
        {
            String content = str;
            byte[] b = Convert.FromBase64String(content);
            MemoryStream ms = new MemoryStream(b);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            return (binaryFormatter.Deserialize(ms) as Object);
        }

        #endregion

        /// <summary>
        /// Finds a control nested within another control or possibly further down in the hierarchy.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static System.Web.UI.Control FindServerControlRecursively(this System.Web.UI.Control root, String id)
        {
            if (!root.IsNull())
            {
                System.Web.UI.Control controlFound = root.FindControl(id);

                if (!controlFound.IsNull())
                {
                    return controlFound;
                }

                foreach (System.Web.UI.Control control in root.Controls)
                {
                    controlFound = control.FindServerControlRecursively(id);

                    if (!controlFound.IsNull())
                    {
                        if (controlFound.ID.Trim() == id.Trim())//UAT-3052
                        {
                            return controlFound;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            return null;
        }

        public static DataTable ToDataTable<TSource>(this IList<TSource> data)
        {
            DataTable dataTable = new DataTable(typeof(TSource).Name);
            PropertyInfo[] props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ??
                    prop.PropertyType);
            }

            foreach (TSource item in data)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static List<T> ConvertDataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
