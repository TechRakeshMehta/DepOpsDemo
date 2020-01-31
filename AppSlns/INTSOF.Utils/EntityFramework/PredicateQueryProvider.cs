#region Copyright
// **************************************************************************************************
// PredicateQueryProvider.cs
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

   using System.Linq;
   using System.Linq.Expressions;

   #endregion

   /// <summary>
   /// Entity Framework predicate query provider for PredicateBuilder
   /// </summary>
   /// <typeparam name="T">
   /// Entity Type 
   /// </typeparam>
   internal class PredicateQueryProvider<T> : IQueryProvider
   {
      #region Constants and Fields

      /// <summary>
      ///   Local <see cref="_query" />
      /// </summary>
      private readonly PrediateQuery<T> _query;

      #endregion

      #region Constructors and Destructors

      /// <summary>Initializes a new instance of the <see cref="PredicateQueryProvider{T}"/> class.</summary>
      /// <param name="query">The query.</param>
      internal PredicateQueryProvider(PrediateQuery<T> query)
      {
         this._query = query;
      }

      #endregion

      // The following four methods first call ExpressionExpander to visit the expression tree, then call
      // upon the inner query to do the remaining work.
      #region Explicit Interface Methods

      /// <summary>Creates the query.</summary>
      /// <typeparam name="TElement">The type of the element.</typeparam>
      /// <param name="expression">The expression.</param>
      /// <returns>A System.Linq.IQueryable&lt;TElement&gt;</returns>
      IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
      {
         return new PrediateQuery<TElement>(this._query.InnerQuery.Provider.CreateQuery<TElement>(expression.Expand()));
      }

      /// <summary>
      /// Constructs an <see cref="T:System.Linq.IQueryable"/> object that can evaluate the query represented by a specified <paramref name="expression"/> tree.
      /// </summary>
      /// <param name="expression">An expression tree that represents a LINQ query.</param>
      /// <returns>An <see cref="T:System.Linq.IQueryable"/> that can evaluate the query represented by the specified <paramref name="expression"/> tree.</returns>
      IQueryable IQueryProvider.CreateQuery(Expression expression)
      {
         return this._query.InnerQuery.Provider.CreateQuery(expression.Expand());
      }

      /// <summary>Executes the specified expression.</summary>
      /// <typeparam name="TResult">The type of the result.</typeparam>
      /// <param name="expression">The expression.</param>
      /// <returns>A <typeparamref name="TResult"/></returns>
      TResult IQueryProvider.Execute<TResult>(Expression expression)
      {
         return this._query.InnerQuery.Provider.Execute<TResult>(expression.Expand());
      }

      /// <summary>Executes the query represented by a specified <paramref name="expression"/> tree.</summary>
      /// <param name="expression">An expression tree that represents a LINQ query.</param>
      /// <returns>The value that results from executing the specified query.</returns>
      object IQueryProvider.Execute(Expression expression)
      {
         return this._query.InnerQuery.Provider.Execute(expression.Expand());
      }

      #endregion
   }
}