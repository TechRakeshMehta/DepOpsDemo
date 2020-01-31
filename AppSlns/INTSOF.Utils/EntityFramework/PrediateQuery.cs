#region Copyright
// **************************************************************************************************
// PrediateQuery.cs
// 
// 
// 
//  Comments
// ------------------------------------------------
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
   using System.Collections;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;

   #endregion

   /// <summary>
   /// EntityFramework ExpandableQuery (IQueryable) for PredicateBuilder
   /// </summary>
   /// <typeparam name="T">Entity Type</typeparam>
   public class PrediateQuery<T> : IOrderedQueryable<T>
   {
      #region Constants and Fields

      /// <summary>Local _inner</summary>
      private readonly IQueryable<T> _innerQuery;

      /// <summary>Local _provider</summary>
      private readonly PredicateQueryProvider<T> _provider;

      #endregion

      // Original query, that we're wrapping
      #region Constructors and Destructors

      /// <summary>Initializes a new instance of the <see cref="PrediateQuery{T}"/> class.</summary>
      /// <param name="innerQuery">The inner.</param>
      internal PrediateQuery(IQueryable<T> innerQuery)
      {
         this._innerQuery = innerQuery;
         this._provider = new PredicateQueryProvider<T>(this);
      }

      #endregion

      #region Explicit Interface Properties

      /// <summary>
      /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable"/> is executed.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.Type"/> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.
      /// </returns>
      Type IQueryable.ElementType
      {
         get
         {
            return typeof(T);
         }
      }

      /// <summary>Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable"/>.</summary>
      /// <returns>
      /// The <see cref="T:System.Linq.Expressions.Expression"/> that is associated with this instance of <see cref="T:System.Linq.IQueryable"/>.
      /// </returns>
      Expression IQueryable.Expression
      {
         get
         {
            return this._innerQuery.Expression;
         }
      }

      /// <summary>Gets the query provider that is associated with this data source.</summary>
      /// <returns>
      /// The <see cref="T:System.Linq.IQueryProvider"/> that is associated with this data source.
      /// </returns>
      IQueryProvider IQueryable.Provider
      {
         get
         {
            return this._provider;
         }
      }

      #endregion

      #region Properties

      /// <summary>Gets the inner query.</summary>
      internal IQueryable<T> InnerQuery
      {
         get
         {
            return this._innerQuery;
         }
      }

      #endregion

      #region Public Methods and Operators

      /// <summary>Returns an enumerator that iterates through the collection.</summary>
      /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.</returns>
      public IEnumerator<T> GetEnumerator()
      {
         return this._innerQuery.GetEnumerator();
      }

      /// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
      /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
      public override string ToString()
      {
         return this._innerQuery.ToString();
      }

      #endregion

      #region Explicit Interface Methods

      /// <summary>Returns an enumerator that iterates through a collection.</summary>
      /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
      IEnumerator IEnumerable.GetEnumerator()
      {
         return this._innerQuery.GetEnumerator();
      }

      #endregion
   }
}