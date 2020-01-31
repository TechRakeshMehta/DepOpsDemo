#region Copyright
// **************************************************************************************************
// PredicateBuilder.cs
// 
// 
// 
// Comments
// -----------------------------------------------------
// Initial Coding
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
   using System.Linq;
   using System.Linq.Expressions;

   #endregion

   /// <summary>
   /// Entity Framework PredicateBuilder
   /// </summary>
   public static class PredicateBuilder
   {
      #region Public Methods and Operators

      /// <summary>
      /// Returns an AND predicate expression
      /// </summary>
      /// <typeparam name="T">
      /// Entity type 
      /// </typeparam>
      /// <param name="expression1">
      /// The expr1. 
      /// </param>
      /// <param name="expression2">
      /// The expr2. 
      /// </param>
      /// <returns>
      /// A Expression &lt; System.Func &lt; T,System.Boolean &gt; &gt; 
      /// </returns>
      public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
      {
         InvocationExpression invocationExpression = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());

         return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expression1.Body, invocationExpression), expression1.Parameters);
      }

      /// <summary>
      /// Returns an OR predicate expression
      /// </summary>
      /// <typeparam name="T">
      /// Entity type 
      /// </typeparam>
      /// <param name="expression1">
      /// The expr1. 
      /// </param>
      /// <param name="expression2">
      /// The expr2. 
      /// </param>
      /// <returns>
      /// A Expression &lt; System.Func &lt; T,System.Boolean &gt; &gt; 
      /// </returns>
      public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
      {
         InvocationExpression invocationExpression = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());

         return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expression1.Body, invocationExpression), expression1.Parameters);
      }


      public static Expression<Func<T, bool>> New<T>(bool @default = false)
      {
         return func => @default;
      }

      #endregion
   }
}