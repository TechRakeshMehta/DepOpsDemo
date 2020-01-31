#region Copyright
// **************************************************************************************************
// Extensions.cs
// 
// 
// 
//  Comments
//  -----------------------------------------------------
// 	Initial Coding
// 
// 
//                          Copyright 2011 Intersoft Data Labs.
// 
//      All rights are reserved.  Reproduction or transmission in whole or in part, in any form or
//    by any means, electronic, mechanical or otherwise, is prohibited without the prior written
//    consent of the copyright owner.
// *************************************************************************************************
#endregion
namespace INTSOF.Utils.EntityFramework
{ 
   #region Using Directives

   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;

   #endregion

   /// <summary>
   /// Entity Framework PredicateBuilder extensions
   /// </summary>
   public static class Extensions
   {
      #region Public Methods and Operators

      /// <summary>Returns the query supporting a predicate</summary>
      /// <typeparam name="T">Entity Type</typeparam>
      /// <param name="query">The query.</param>
      /// <returns>A System.Linq.IQueryable&lt;T&gt;</returns>
      public static IQueryable<T> WithPredicate<T>(this IQueryable<T> query)
      {
         if (query is PrediateQuery<T>)
         {
            return query;
         }

         return new PrediateQuery<T>(query);
      }

      /// <summary>Returns the given anonymous method as a lambda expression</summary>
      /// <typeparam name="T">Entity Type</typeparam>
      /// <typeparam name="TResult">The type of the result.</typeparam>
      /// <param name="expression">The expr.</param>
      /// <returns>A Expression&lt;System.Func&lt;T,TResult&gt;&gt;</returns>
      public static Expression<Func<T, TResult>> AsExpression<T, TResult>(this Expression<Func<T, TResult>> expression)
      {
         return expression;
      }

      /// <summary>Returns the given anonymous function as a Func delegate</summary>
      /// <typeparam name="T">Entity Type</typeparam>
      /// <typeparam name="TResult">The type of the result.</typeparam>
      /// <param name="expression">The expr.</param>
      /// <returns>A Func&lt;T,TResult&gt;</returns>
      public static Func<T, TResult> AsFunc<T, TResult>(this Func<T, TResult> expression)
      {
         return expression;
      }

      /// <summary>Expands the specified expr.</summary>
      /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
      /// <param name="expr">The expr.</param>
      /// <returns>A Expression&lt;TDelegate&gt;</returns>
      public static Expression<TDelegate> Expand<TDelegate>(this Expression<TDelegate> expr)
      {
         return (Expression<TDelegate>)new ExpressionExpander().Visit(expr);
      }

      /// <summary>Expands the specified expr.</summary>
      /// <param name="expr">The expr.</param>
      /// <returns>A Expression</returns>
      public static Expression Expand(this Expression expr)
      {
         return new ExpressionExpander().Visit(expr);
      }

      /// <summary>Fors the each.</summary>
      /// <typeparam name="T">Entity type</typeparam>
      /// <param name="source">The source.</param>
      /// <param name="action">The action.</param>
      public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
      {
         foreach (T element in source)
         {
            action(element);
         }
      }

      /// <summary>Invokes the specified expression.</summary>
      /// <typeparam name="TResult">The type of the result.</typeparam>
      /// <param name="expression">The expr.</param>
      /// <returns>A TResult</returns>
      public static TResult Invoke<TResult>(this Expression<Func<TResult>> expression)
      {
         return expression.Compile().Invoke();
      }

      /// <summary>Invokes the specified expression.</summary>
      /// <typeparam name="T1">The type of the 1.</typeparam>
      /// <typeparam name="TResult">The type of the result.</typeparam>
      /// <param name="expression">The expr.</param>
      /// <param name="arg1">The arg1.</param>
      /// <returns>A TResult</returns>
      public static TResult Invoke<T1, TResult>(this Expression<Func<T1, TResult>> expression, T1 arg1)
      {
         return expression.Compile().Invoke(arg1);
      }

      /// <summary>Invokes the specified expression.</summary>
      /// <typeparam name="T1">The type of the 1.</typeparam>
      /// <typeparam name="T2">The type of the 2.</typeparam>
      /// <typeparam name="TResult">The type of the result.</typeparam>
      /// <param name="expression">The expr.</param>
      /// <param name="arg1">The arg1.</param>
      /// <param name="arg2">The arg2.</param>
      /// <returns>A TResult</returns>
      public static TResult Invoke<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> expression, T1 arg1, T2 arg2)
      {
         return expression.Compile().Invoke(arg1, arg2);
      }

      /// <summary>Invokes the specified expression.</summary>
      /// <typeparam name="T1">The type of the 1.</typeparam>
      /// <typeparam name="T2">The type of the 2.</typeparam>
      /// <typeparam name="T3">The type of the 3.</typeparam>
      /// <typeparam name="TResult">The type of the result.</typeparam>
      /// <param name="expression">The expr.</param>
      /// <param name="arg1">The arg1.</param>
      /// <param name="arg2">The arg2.</param>
      /// <param name="arg3">The arg3.</param>
      /// <returns>A TResult</returns>
      public static TResult Invoke<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> expression, T1 arg1, T2 arg2, T3 arg3)
      {
         return expression.Compile().Invoke(arg1, arg2, arg3);
      }

      /// <summary>Invokes the specified expression.</summary>
      /// <typeparam name="T1">The type of the 1.</typeparam>
      /// <typeparam name="T2">The type of the 2.</typeparam>
      /// <typeparam name="T3">The type of the 3.</typeparam>
      /// <typeparam name="T4">The type of the 4.</typeparam>
      /// <typeparam name="TResult">The type of the result.</typeparam>
      /// <param name="expression">The expr.</param>
      /// <param name="arg1">The arg1.</param>
      /// <param name="arg2">The arg2.</param>
      /// <param name="arg3">The arg3.</param>
      /// <param name="arg4">The arg4.</param>
      /// <returns>A TResult</returns>
      public static TResult Invoke<T1, T2, T3, T4, TResult>(
         this Expression<Func<T1, T2, T3, T4, TResult>> expression, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
      {
         return expression.Compile().Invoke(arg1, arg2, arg3, arg4);
      }
      
      #endregion
   }
}