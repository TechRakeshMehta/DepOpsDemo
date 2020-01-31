#region Copyright

// **************************************************************************************************
// DAL - QueryManager.cs
// 
// 
//  Comments
//  -----------------------------------------------------
// 	Initial Coding
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
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;

#endregion

namespace DAL.Repository
{
    /// <summary>Query manager helper</summary>
    /// <summary>Query manager helper</summary>
    internal static class QueryManager
    {
        /// <summary>Instances the specified context.</summary>
        /// <typeparam name="T">Type of the context.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns>A IQueryManager&lt;T&gt;</returns>
        public static IQueryManager<T> GetMockInstance<T>(T context) where T : class
        {
            return new MockQueryManager<T>(context);
        }

        /// <summary>Instances the specified context.</summary>
        /// <typeparam name="T">Type of the context.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns>A IQueryManager&lt;T&gt;</returns>
        public static IQueryManager<T> GetInstance<T>(T context) where T : ObjectContext
        {
            return new QueryManager<T>(context);
        }

        /// <summary>Instances the specified context.</summary>
        /// <typeparam name="T">Type of the context.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns>A IQueryManager&lt;T&gt;</returns>
        public static IQueryManager<T> GetDbContextInstance<T>(T context)
        {
            return new DbContextQueryManager<T>(context);
        }
    }

    /// <summary>Interface for query manager.</summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    public interface IQueryManager<TContext>
    {
        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult Execute<TResult>(Expression<Func<TContext, TResult>> query);

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult ExecuteCached<TResult>(Expression<Func<TContext, TResult>> query);

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult Execute<TArg1, TResult>(TArg1 arg1, Expression<Func<TContext, TArg1, TResult>> query);

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult ExecuteCached<TArg1, TResult>(TArg1 arg1, Expression<Func<TContext, TArg1, TResult>> query);

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult Execute<TArg1, TArg2, TResult>(TArg1 arg1, TArg2 arg2, Expression<Func<TContext, TArg1, TArg2, TResult>> query);

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult ExecuteCached<TArg1, TArg2, TResult>(TArg1 arg1, TArg2 arg2, Expression<Func<TContext, TArg1, TArg2, TResult>> query);

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult Execute<TArg1, TArg2, TArg3, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Expression<Func<TContext, TArg1, TArg2, TArg3, TResult>> query);

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult ExecuteCached<TArg1, TArg2, TArg3, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Expression<Func<TContext, TArg1, TArg2, TArg3, TResult>> query);

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult Execute<TArg1, TArg2, TArg3, TArg4, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TResult>> query);

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TResult>(
           TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TResult>> query);

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(
           TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> query);

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(
           TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> query);

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(
           TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> query);

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(
           TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> query);

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> query);

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> query);

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> query);

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> query);

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>> query);

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>> query);

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>> query);

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>> query);

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           TArg11 arg11,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>> query);

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           TArg11 arg11,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>> query);

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           TArg11 arg11,
           TArg12 arg12,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>> query);

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           TArg11 arg11,
           TArg12 arg12,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>> query);

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TArg13">The type of the <paramref name="arg13"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="arg13">Query argument 13.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           TArg11 arg11,
           TArg12 arg12,
           TArg13 arg13,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>> query);

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TArg13">The type of the <paramref name="arg13"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="arg13">Query argument 13.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           TArg11 arg11,
           TArg12 arg12,
           TArg13 arg13,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>> query);

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TArg13">The type of the <paramref name="arg13"/>.</typeparam>
        /// <typeparam name="TArg14">The type of the <paramref name="arg14"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="arg13">Query argument 13.</param>
        /// <param name="arg14">Query argument 14.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           TArg11 arg11,
           TArg12 arg12,
           TArg13 arg13,
           TArg14 arg14,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>> query);

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TArg13">The type of the <paramref name="arg13"/>.</typeparam>
        /// <typeparam name="TArg14">The type of the <paramref name="arg14"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="arg13">Query argument 13.</param>
        /// <param name="arg14">Query argument 14.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           TArg11 arg11,
           TArg12 arg12,
           TArg13 arg13,
           TArg14 arg14,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>> query);
    }

    /// <summary>Compiles and caches queries</summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <example> This shows how to use the QueryManager class
    /// <code>
    /// // NOTE: the ObjectContext is implicit at the first item in the EXECUTE method, and doesn't need to be passed 
    ///  var queryManager = new QueryManager&gt;LinqDbContextReference&lt;();
    ///  var result = queryManager.Execute(intVariable, stringVarible, (ctx, myInt, myString) => ctx.Table.Where(item => item.Id == myInt &amp;&amp; item.Name == myString));
    /// </code>
    /// </example>  
    public class DbContextQueryManager<TContext> : QueryManagerBase, IQueryManager<TContext>
    {
        #region Constants and Fields

        /// <summary>
        ///   The local context.
        /// </summary>
        private readonly TContext _context;

        /// <summary>
        ///   The local query cache.
        /// </summary>
        private readonly ConcurrentDictionary<object, object> _queryCache;

        /// <summary>
        ///   The local result cache.
        /// </summary>
        private readonly IDictionary<object, object> _resultCache;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="QueryManager{TContext}"/> class.</summary>
        /// <param name="context">The context.</param>
        public DbContextQueryManager(TContext context)
        {
            this._context = context;
            this._queryCache = this.Instance(context);
            this._resultCache = new Dictionary<object, object>();
        }

        #endregion

        #region Public Methods

        // query method overloads for 0 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TResult>(Expression<Func<TContext, TResult>> query)
        {
            this.CheckArguments(query, this._context);
            return query.Compile()(this._context);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TResult>(Expression<Func<TContext, TResult>> query)
        {
            var queryKey = query.ToString();

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
                //  _resultCache[queryKey] = result;

            }

            return (TResult)result;
        }

        // query method overloads for 1 parameter

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TResult>(TArg1 arg1, Expression<Func<TContext, TArg1, TResult>> query)
        {
            this.CheckArguments(query, this._context);
            return query.Compile()(this._context, arg1);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TResult>(TArg1 arg1, Expression<Func<TContext, TArg1, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1>(query.ToString(), arg1);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 2 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TResult>(TArg1 arg1, TArg2 arg2, Expression<Func<TContext, TArg1, TArg2, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            return query.Compile()(this._context, arg1, arg2);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TResult>(TArg1 arg1, TArg2 arg2, Expression<Func<TContext, TArg1, TArg2, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2>(query.ToString(), arg1, arg2);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 3 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Expression<Func<TContext, TArg1, TArg2, TArg3, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            return query.Compile()(this._context, arg1, arg2, arg3);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Expression<Func<TContext, TArg1, TArg2, TArg3, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3>(query.ToString(), arg1, arg2, arg3);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 4 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TResult>(
           TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            return query.Compile()(this._context, arg1, arg2, arg3, arg4);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TResult>(
           TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4>(query.ToString(), arg1, arg2, arg3, arg4);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 5 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(
           TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(
           TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5>(query.ToString(), arg1, arg2, arg3, arg4, arg5);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 6 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(
           TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(
           TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 7 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, arg7);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 8 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8>>(
               query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8>(arg7, arg8));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 9 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9>>(
               query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8, TArg9>(arg7, arg8, arg9));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 10 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9, TArg10>>(
               query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8, TArg9, TArg10>(arg7, arg8, arg9, arg10));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 11 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           TArg11 arg11,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           TArg11 arg11,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9, TArg10, TArg11>>(
               query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8, TArg9, TArg10, TArg11>(arg7, arg8, arg9, arg10, arg11));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 12 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           TArg11 arg11,
           TArg12 arg12,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           TArg11 arg11,
           TArg12 arg12,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>>(
               query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(arg7, arg8, arg9, arg10, arg11, arg12));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 13 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TArg13">The type of the <paramref name="arg13"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="arg13">Query argument 13.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           TArg11 arg11,
           TArg12 arg12,
           TArg13 arg13,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TArg13">The type of the <paramref name="arg13"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="arg13">Query argument 13.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           TArg11 arg11,
           TArg12 arg12,
           TArg13 arg13,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>>(
               query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(arg7, arg8, arg9, arg10, arg11, arg12, arg13));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 14 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TArg13">The type of the <paramref name="arg13"/>.</typeparam>
        /// <typeparam name="TArg14">The type of the <paramref name="arg14"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="arg13">Query argument 13.</param>
        /// <param name="arg14">Query argument 14.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           TArg11 arg11,
           TArg12 arg12,
           TArg13 arg13,
           TArg14 arg14,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TArg13">The type of the <paramref name="arg13"/>.</typeparam>
        /// <typeparam name="TArg14">The type of the <paramref name="arg14"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="arg13">Query argument 13.</param>
        /// <param name="arg14">Query argument 14.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(
           TArg1 arg1,
           TArg2 arg2,
           TArg3 arg3,
           TArg4 arg4,
           TArg5 arg5,
           TArg6 arg6,
           TArg7 arg7,
           TArg8 arg8,
           TArg9 arg9,
           TArg10 arg10,
           TArg11 arg11,
           TArg12 arg12,
           TArg13 arg13,
           TArg14 arg14,
           Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>>(
               query.ToString(),
               arg1,
               arg2,
               arg3,
               arg4,
               arg5,
               arg6,
               new Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        #endregion

        #region Private Methods

        /// <summary>Checks the arguments.</summary>
        /// <param name="query">The query.</param>
        /// <param name="context">The context.</param>
        private void CheckArguments(object query, TContext context)
        {
            if (ReferenceEquals(context, null))
            {
                throw new ArgumentNullException("context");
            }

            if (ReferenceEquals(query, null))
            {
                throw new ArgumentNullException("query");
            }

            if (typeof(TContext).IsInterface)
            {
                return;
            }
        }

        #endregion
    }


    /// <summary>Compiles and caches queries</summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <example> This shows how to use the QueryManager class
    /// <code>
    /// // NOTE: the ObjectContext is implicit at the first item in the EXECUTE method, and doesn't need to be passed 
    ///  var queryManager = new QueryManager&gt;LinqDbContextReference&lt;();
    ///  var result = queryManager.Execute(intVariable, stringVarible, (ctx, myInt, myString) => ctx.Table.Where(item => item.Id == myInt &amp;&amp; item.Name == myString));
    /// </code>
    /// </example>  
    public class QueryManager<TContext> : QueryManagerBase, IQueryManager<TContext> where TContext : ObjectContext
    {
        #region Constants and Fields

        /// <summary>
        ///   The local context.
        /// </summary>
        private readonly TContext _context;

        /// <summary>
        ///   The local query cache.
        /// </summary>
        private readonly ConcurrentDictionary<object, object> _queryCache;

        /// <summary>
        ///   The local result cache.
        /// </summary>
        private readonly IDictionary<object, object> _resultCache;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="QueryManager{TContext}"/> class.</summary>
        /// <param name="context">The context.</param>
        public QueryManager(TContext context)
        {
            this._context = context;
            this._queryCache = this.Instance(context);
            this._resultCache = new Dictionary<object, object>();
        }

        #endregion

        #region Public Methods

        // query method overloads for 0 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TResult>(Expression<Func<TContext, TResult>> query)
        {
            if (this.CheckArguments(query, this._context))
            {
                object cachedQuery;

                if (!this._queryCache.TryGetValue(query.ToString(), out cachedQuery))
                {
                    cachedQuery = CompiledQuery.Compile(query);
                    this._queryCache.TryAdd(query.ToString(), cachedQuery);
                }

                return ((Func<TContext, TResult>)cachedQuery)(this._context);
            }

            return query.Compile()(this._context);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TResult>(Expression<Func<TContext, TResult>> query)
        {
            var queryKey = query.ToString();

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 1 parameter

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TResult>(TArg1 arg1, Expression<Func<TContext, TArg1, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            object cachedQuery;

            if (!this._queryCache.TryGetValue(query.ToString(), out cachedQuery))
            {
                cachedQuery = CompiledQuery.Compile(query);
                this._queryCache.TryAdd(query.ToString(), cachedQuery);
            }

            return ((Func<TContext, TArg1, TResult>)cachedQuery)(this._context, arg1);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TResult>(TArg1 arg1, Expression<Func<TContext, TArg1, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1>(query.ToString(), arg1);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 2 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TResult>(TArg1 arg1, TArg2 arg2, Expression<Func<TContext, TArg1, TArg2, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            object cachedQuery;

            if (!this._queryCache.TryGetValue(query.ToString(), out cachedQuery))
            {
                cachedQuery = CompiledQuery.Compile(query);
                this._queryCache.TryAdd(query.ToString(), cachedQuery);
            }

            return ((Func<TContext, TArg1, TArg2, TResult>)cachedQuery)(this._context, arg1, arg2);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TResult>(TArg1 arg1, TArg2 arg2, Expression<Func<TContext, TArg1, TArg2, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2>(query.ToString(), arg1, arg2);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 3 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Expression<Func<TContext, TArg1, TArg2, TArg3, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            object cachedQuery;

            if (!this._queryCache.TryGetValue(query.ToString(), out cachedQuery))
            {
                cachedQuery = CompiledQuery.Compile(query);
                this._queryCache.TryAdd(query.ToString(), cachedQuery);
            }

            return ((Func<TContext, TArg1, TArg2, TArg3, TResult>)cachedQuery)(this._context, arg1, arg2, arg3);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Expression<Func<TContext, TArg1, TArg2, TArg3, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3>(query.ToString(), arg1, arg2, arg3);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 4 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            object cachedQuery;

            if (!this._queryCache.TryGetValue(query.ToString(), out cachedQuery))
            {
                cachedQuery = CompiledQuery.Compile(query);
                this._queryCache.TryAdd(query.ToString(), cachedQuery);
            }

            return ((Func<TContext, TArg1, TArg2, TArg3, TArg4, TResult>)cachedQuery)(this._context, arg1, arg2, arg3, arg4);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4>(query.ToString(), arg1, arg2, arg3, arg4);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 5 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            object cachedQuery;

            if (!this._queryCache.TryGetValue(query.ToString(), out cachedQuery))
            {
                cachedQuery = CompiledQuery.Compile(query);
                this._queryCache.TryAdd(query.ToString(), cachedQuery);
            }

            return ((Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>)cachedQuery)(this._context, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5>(query.ToString(), arg1, arg2, arg3, arg4, arg5);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 6 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            object cachedQuery;

            if (!this._queryCache.TryGetValue(query.ToString(), out cachedQuery))
            {
                cachedQuery = CompiledQuery.Compile(query);
                this._queryCache.TryAdd(query.ToString(), cachedQuery);
            }

            return ((Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>)cachedQuery)(this._context, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 7 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            object cachedQuery;

            if (!this._queryCache.TryGetValue(query.ToString(), out cachedQuery))
            {
                cachedQuery = CompiledQuery.Compile(query);
                this._queryCache.TryAdd(query.ToString(), cachedQuery);
            }

            return ((Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>)cachedQuery)(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, arg7);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 8 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            object cachedQuery;

            if (!this._queryCache.TryGetValue(query.ToString(), out cachedQuery))
            {
                cachedQuery = CompiledQuery.Compile(query);
                this._queryCache.TryAdd(query.ToString(), cachedQuery);
            }

            return ((Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>)cachedQuery)(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8>>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8>(arg7, arg8));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 9 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            object cachedQuery;

            if (!this._queryCache.TryGetValue(query.ToString(), out cachedQuery))
            {
                cachedQuery = CompiledQuery.Compile(query);
                this._queryCache.TryAdd(query.ToString(), cachedQuery);
            }

            return ((Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>)cachedQuery)(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9>>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8, TArg9>(arg7, arg8, arg9));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 10 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            object cachedQuery;

            if (!this._queryCache.TryGetValue(query.ToString(), out cachedQuery))
            {
                cachedQuery = CompiledQuery.Compile(query);
                this._queryCache.TryAdd(query.ToString(), cachedQuery);
            }

            return ((Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>)cachedQuery)(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9, TArg10>>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8, TArg9, TArg10>(arg7, arg8, arg9, arg10));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 11 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            object cachedQuery;

            if (!this._queryCache.TryGetValue(query.ToString(), out cachedQuery))
            {
                cachedQuery = CompiledQuery.Compile(query);
                this._queryCache.TryAdd(query.ToString(), cachedQuery);
            }

            return ((Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>)cachedQuery)(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9, TArg10, TArg11>>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8, TArg9, TArg10, TArg11>(arg7, arg8, arg9, arg10, arg11));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 12 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            object cachedQuery;

            if (!this._queryCache.TryGetValue(query.ToString(), out cachedQuery))
            {
                cachedQuery = CompiledQuery.Compile(query);
                this._queryCache.TryAdd(query.ToString(), cachedQuery);
            }

            return ((Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>)cachedQuery)(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(arg7, arg8, arg9, arg10, arg11, arg12));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 13 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TArg13">The type of the <paramref name="arg13"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="arg13">Query argument 13.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            object cachedQuery;

            if (!this._queryCache.TryGetValue(query.ToString(), out cachedQuery))
            {
                cachedQuery = CompiledQuery.Compile(query);
                this._queryCache.TryAdd(query.ToString(), cachedQuery);
            }

            return ((Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>)cachedQuery)(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TArg13">The type of the <paramref name="arg13"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="arg13">Query argument 13.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(arg7, arg8, arg9, arg10, arg11, arg12, arg13));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 14 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TArg13">The type of the <paramref name="arg13"/>.</typeparam>
        /// <typeparam name="TArg14">The type of the <paramref name="arg14"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="arg13">Query argument 13.</param>
        /// <param name="arg14">Query argument 14.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>> query)
        {
            this.CheckArguments(query, this._context);

            object cachedQuery;

            if (!this._queryCache.TryGetValue(query.ToString(), out cachedQuery))
            {
                cachedQuery = CompiledQuery.Compile(query);
                this._queryCache.TryAdd(query.ToString(), cachedQuery);
            }

            return ((Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>)cachedQuery)(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TArg13">The type of the <paramref name="arg13"/>.</typeparam>
        /// <typeparam name="TArg14">The type of the <paramref name="arg14"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="arg13">Query argument 13.</param>
        /// <param name="arg14">Query argument 14.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        #endregion

        #region Private Methods

        /// <summary>Checks the arguments.</summary>
        /// <param name="query">The query.</param>
        /// <param name="context">The context.</param>
        /// <returns>A System.Boolean</returns>
        private bool CheckArguments(object query, TContext context)
        {
            if (ReferenceEquals(context, null))
            {
                throw new ArgumentNullException("context");
            }

            if (ReferenceEquals(query, null))
            {
                throw new ArgumentNullException("query");
            }

            if (typeof(TContext).IsInterface)
            {
                return false;
            }

            return true;
        }

        #endregion
    }

    /// <summary>Mock query manager</summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    public class MockQueryManager<TContext> : QueryManagerBase, IQueryManager<TContext> where TContext : class
    {
        #region Constants and Fields

        /// <summary>
        ///   The local context.
        /// </summary>
        private readonly TContext _context;

        /// <summary>
        ///   The local result cache.
        /// </summary>
        private readonly IDictionary<object, object> _resultCache;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="QueryManager{TContext}"/> class.</summary>
        /// <param name="context">The context.</param>
        public MockQueryManager(TContext context)
        {
            this._context = context;
            this._resultCache = new Dictionary<object, object>();
        }

        #endregion

        #region Public Methods

        // query method overloads for 0 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TResult>(Expression<Func<TContext, TResult>> query)
        {
            return query.Compile()(this._context);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TResult>(Expression<Func<TContext, TResult>> query)
        {
            var queryKey = query.ToString();

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 1 parameter

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TResult>(TArg1 arg1, Expression<Func<TContext, TArg1, TResult>> query)
        {
            return query.Compile()(this._context, arg1);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TResult>(TArg1 arg1, Expression<Func<TContext, TArg1, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1>(query.ToString(), arg1);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 2 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TResult>(TArg1 arg1, TArg2 arg2, Expression<Func<TContext, TArg1, TArg2, TResult>> query)
        {
            return query.Compile()(this._context, arg1, arg2);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TResult>(TArg1 arg1, TArg2 arg2, Expression<Func<TContext, TArg1, TArg2, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2>(query.ToString(), arg1, arg2);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 3 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Expression<Func<TContext, TArg1, TArg2, TArg3, TResult>> query)
        {
            return query.Compile()(this._context, arg1, arg2, arg3);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, Expression<Func<TContext, TArg1, TArg2, TArg3, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3>(query.ToString(), arg1, arg2, arg3);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 4 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TResult>> query)
        {
            return query.Compile()(this._context, arg1, arg2, arg3, arg4);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4>(query.ToString(), arg1, arg2, arg3, arg4);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 5 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> query)
        {
            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5>(query.ToString(), arg1, arg2, arg3, arg4, arg5);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 6 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> query)
        {
            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 7 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> query)
        {
            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, arg7);

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 8 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> query)
        {
            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8>>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8>(arg7, arg8));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 9 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>> query)
        {
            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9>>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8, TArg9>(arg7, arg8, arg9));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 10 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>> query)
        {
            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9, TArg10>>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8, TArg9, TArg10>(arg7, arg8, arg9, arg10));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 11 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>> query)
        {
            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9, TArg10, TArg11>>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8, TArg9, TArg10, TArg11>(arg7, arg8, arg9, arg10, arg11));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 12 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>> query)
        {
            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(arg7, arg8, arg9, arg10, arg11, arg12));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 13 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TArg13">The type of the <paramref name="arg13"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="arg13">Query argument 13.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>> query)
        {
            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TArg13">The type of the <paramref name="arg13"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="arg13">Query argument 13.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(arg7, arg8, arg9, arg10, arg11, arg12, arg13));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        // query method overloads for 14 parameters

        /// <summary>Executes a Linq Query.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TArg13">The type of the <paramref name="arg13"/>.</typeparam>
        /// <typeparam name="TArg14">The type of the <paramref name="arg14"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="arg13">Query argument 13.</param>
        /// <param name="arg14">Query argument 14.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult Execute<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>> query)
        {
            return query.Compile()(this._context, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        }

        /// <summary>Executes a Linq Query using result caching.</summary>
        /// <typeparam name="TArg1">The type of the <paramref name="arg1"/>.</typeparam>
        /// <typeparam name="TArg2">The type of the <paramref name="arg2"/>.</typeparam>
        /// <typeparam name="TArg3">The type of the <paramref name="arg3"/>.</typeparam>
        /// <typeparam name="TArg4">The type of the <paramref name="arg4"/>.</typeparam>
        /// <typeparam name="TArg5">The type of the <paramref name="arg5"/>.</typeparam>
        /// <typeparam name="TArg6">The type of the <paramref name="arg6"/>.</typeparam>
        /// <typeparam name="TArg7">The type of the <paramref name="arg7"/>.</typeparam>
        /// <typeparam name="TArg8">The type of the <paramref name="arg8"/>.</typeparam>
        /// <typeparam name="TArg9">The type of the <paramref name="arg9"/>.</typeparam>
        /// <typeparam name="TArg10">The type of the <paramref name="arg10"/>.</typeparam>
        /// <typeparam name="TArg11">The type of the <paramref name="arg11"/>.</typeparam>
        /// <typeparam name="TArg12">The type of the <paramref name="arg12"/>.</typeparam>
        /// <typeparam name="TArg13">The type of the <paramref name="arg13"/>.</typeparam>
        /// <typeparam name="TArg14">The type of the <paramref name="arg14"/>.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>>
        /// <param name="arg1">Query argument 1.</param>
        /// <param name="arg2">Query argument 2.</param>
        /// <param name="arg3">Query argument 3.</param>
        /// <param name="arg4">Query argument 4.</param>
        /// <param name="arg5">Query argument 5.</param>
        /// <param name="arg6">Query argument 6.</param>
        /// <param name="arg7">Query argument 7.</param>
        /// <param name="arg8">Query argument 8.</param>
        /// <param name="arg9">Query argument 9.</param>
        /// <param name="arg10">Query argument 10.</param>
        /// <param name="arg11">Query argument 11.</param>
        /// <param name="arg12">Query argument 12.</param>
        /// <param name="arg13">Query argument 13.</param>
        /// <param name="arg14">Query argument 14.</param>
        /// <param name="query">The Linq Query.</param>
        /// <returns>The result of the Linq Query, of type <typeparamref name="TResult"/></returns>
        public TResult ExecuteCached<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, Expression<Func<TContext, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>> query)
        {
            var queryKey = new Tuple<string, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>>(query.ToString(), arg1, arg2, arg3, arg4, arg5, arg6, new Tuple<TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14));

            object result;

            if (!this._resultCache.TryGetValue(queryKey, out result))
            {
                result = this.Execute(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, query);
                this.AddToCache(this._resultCache, queryKey, (TResult)result);
            }

            return (TResult)result;
        }

        #endregion
    }
}

