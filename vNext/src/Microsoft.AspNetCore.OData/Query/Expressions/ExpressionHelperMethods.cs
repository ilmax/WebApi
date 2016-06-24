// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System;

namespace Microsoft.AspNetCore.OData.Query.Expressions
{
    internal class ExpressionHelperMethods
    {
        public static MethodInfo QueryableOrderByGeneric { get; } = GenericMethodOf(_ => Queryable.OrderBy<int, int>(default(IQueryable<int>), default(Expression<Func<int, int>>)));

        public static MethodInfo EnumerableOrderByGeneric { get; } = GenericMethodOf(_ => Enumerable.OrderBy<int, int>(default(IEnumerable<int>), default(Func<int, int>)));

        public static MethodInfo QueryableOrderByDescendingGeneric { get; } = GenericMethodOf(_ => Queryable.OrderByDescending<int, int>(default(IQueryable<int>), default(Expression<Func<int, int>>)));

        public static MethodInfo QueryableThenByGeneric { get; } = GenericMethodOf(_ => Queryable.ThenBy<int, int>(default(IOrderedQueryable<int>), default(Expression<Func<int, int>>)));

        public static MethodInfo EnumerableThenByGeneric { get; } = GenericMethodOf(_ => Enumerable.ThenBy<int, int>(default(IOrderedEnumerable<int>), default(Func<int, int>)));

        public static MethodInfo QueryableThenByDescendingGeneric { get; } = GenericMethodOf(_ => Queryable.ThenByDescending<int, int>(default(IOrderedQueryable<int>), default(Expression<Func<int, int>>)));

        public static MethodInfo QueryableCountGeneric { get; } = GenericMethodOf(_ => Queryable.LongCount<int>(default(IQueryable<int>)));

        public static MethodInfo QueryableTakeGeneric { get; } = GenericMethodOf(_ => Queryable.Take<int>(default(IQueryable<int>), default(int)));

        public static MethodInfo EnumerableTakeGeneric { get; } = GenericMethodOf(_ => Enumerable.Take<int>(default(IEnumerable<int>), default(int)));

        public static MethodInfo QueryableSkipGeneric { get; } = GenericMethodOf(_ => Queryable.Skip<int>(default(IQueryable<int>), default(int)));

        public static MethodInfo QueryableWhereGeneric { get; } = GenericMethodOf(_ => Queryable.Where<int>(default(IQueryable<int>), default(Expression<Func<int, bool>>)));

        public static MethodInfo QueryableSelectGeneric { get; } = GenericMethodOf(_ => Queryable.Select<int, int>(default(IQueryable<int>), i => i));

        public static MethodInfo EnumerableSelectGeneric { get; } = GenericMethodOf(_ => Enumerable.Select<int, int>(default(IEnumerable<int>), i => i));

        public static MethodInfo QueryableEmptyAnyGeneric { get; } = GenericMethodOf(_ => Queryable.Any<int>(default(IQueryable<int>)));

        public static MethodInfo QueryableNonEmptyAnyGeneric { get; } = GenericMethodOf(_ => Queryable.Any<int>(default(IQueryable<int>), default(Expression<Func<int, bool>>)));

        public static MethodInfo QueryableAllGeneric { get; } = GenericMethodOf(_ => Queryable.All(default(IQueryable<int>), default(Expression<Func<int, bool>>)));

        public static MethodInfo EnumerableEmptyAnyGeneric { get; } = GenericMethodOf(_ => Enumerable.Any<int>(default(IEnumerable<int>)));

        public static MethodInfo EnumerableNonEmptyAnyGeneric { get; } = GenericMethodOf(_ => Enumerable.Any<int>(default(IEnumerable<int>), default(Func<int, bool>)));

        public static MethodInfo EnumerableAllGeneric { get; } = GenericMethodOf(_ => Enumerable.All<int>(default(IEnumerable<int>), default(Func<int, bool>)));

        public static MethodInfo EnumerableOfType { get; } = GenericMethodOf(_ => Enumerable.OfType<int>(default(IEnumerable)));

        public static MethodInfo QueryableOfType { get; } = GenericMethodOf(_ => Queryable.OfType<int>(default(IQueryable)));

        public static MethodInfo QueryableAsQueryable { get; } = GenericMethodOf(_ => Queryable.AsQueryable<int>(default(IEnumerable<int>)));

        private static MethodInfo GenericMethodOf<TReturn>(Expression<Func<object, TReturn>> expression)
        {
            return GenericMethodOf(expression as Expression);
        }

        private static MethodInfo GenericMethodOf(Expression expression)
        {
            LambdaExpression lambdaExpression = expression as LambdaExpression;

            Contract.Assert(expression.NodeType == ExpressionType.Lambda);
            Contract.Assert(lambdaExpression != null);
            Contract.Assert(lambdaExpression.Body.NodeType == ExpressionType.Call);

            return (lambdaExpression.Body as MethodCallExpression).Method.GetGenericMethodDefinition();
        }
    }
}
