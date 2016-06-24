// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.AspNetCore.OData
{
    using System;

    internal class ExpressionHelperMethods
    {
        private static MethodInfo _orderByMethod = GenericMethodOf(_ => Queryable.OrderBy<int, int>(default(IQueryable<int>), default(Expression<Func<int, int>>)));
        private static MethodInfo _enumerableOrderByMethod = GenericMethodOf(_ => Enumerable.OrderBy<int, int>(default(IEnumerable<int>), default(Func<int, int>)));
        private static MethodInfo _orderByDescendingMethod = GenericMethodOf(_ => Queryable.OrderByDescending<int, int>(default(IQueryable<int>), default(Expression<Func<int, int>>)));
        private static MethodInfo _thenByMethod = GenericMethodOf(_ => Queryable.ThenBy<int, int>(default(IOrderedQueryable<int>), default(Expression<Func<int, int>>)));
        private static MethodInfo _enumerableThenByMethod = GenericMethodOf(_ => Enumerable.ThenBy<int, int>(default(IOrderedEnumerable<int>), default(Func<int, int>)));
        private static MethodInfo _thenByDescendingMethod = GenericMethodOf(_ => Queryable.ThenByDescending<int, int>(default(IOrderedQueryable<int>), default(Expression<Func<int, int>>)));
        private static MethodInfo _countMethod = GenericMethodOf(_ => Queryable.LongCount<int>(default(IQueryable<int>)));
        private static MethodInfo _skipMethod = GenericMethodOf(_ => Queryable.Skip<int>(default(IQueryable<int>), default(int)));
        private static MethodInfo _whereMethod = GenericMethodOf(_ => Queryable.Where<int>(default(IQueryable<int>), default(Expression<Func<int, bool>>)));

        private static MethodInfo _queryableEmptyAnyMethod = GenericMethodOf(_ => Queryable.Any<int>(default(IQueryable<int>)));
        private static MethodInfo _queryableNonEmptyAnyMethod = GenericMethodOf(_ => Queryable.Any<int>(default(IQueryable<int>), default(Expression<Func<int, bool>>)));
        private static MethodInfo _queryableAllMethod = GenericMethodOf(_ => Queryable.All(default(IQueryable<int>), default(Expression<Func<int, bool>>)));

        private static MethodInfo _enumerableEmptyAnyMethod = GenericMethodOf(_ => Enumerable.Any<int>(default(IEnumerable<int>)));
        private static MethodInfo _enumerableNonEmptyAnyMethod = GenericMethodOf(_ => Enumerable.Any<int>(default(IEnumerable<int>), default(Func<int, bool>)));
        private static MethodInfo _enumerableAllMethod = GenericMethodOf(_ => Enumerable.All<int>(default(IEnumerable<int>), default(Func<int, bool>)));

        private static MethodInfo _enumerableOfTypeMethod = GenericMethodOf(_ => Enumerable.OfType<int>(default(IEnumerable)));
        private static MethodInfo _queryableOfTypeMethod = GenericMethodOf(_ => Queryable.OfType<int>(default(IQueryable)));

        private static MethodInfo _enumerableSelectMethod = GenericMethodOf(_ => Enumerable.Select<int, int>(default(IEnumerable<int>), i => i));
        private static MethodInfo _queryableSelectMethod = GenericMethodOf(_ => Queryable.Select<int, int>(default(IQueryable<int>), i => i));

        private static MethodInfo _queryableTakeMethod = GenericMethodOf(_ => Queryable.Take<int>(default(IQueryable<int>), default(int)));
        private static MethodInfo _enumerableTakeMethod = GenericMethodOf(_ => Enumerable.Take<int>(default(IEnumerable<int>), default(int)));

        private static MethodInfo _queryableAsQueryableMethod = GenericMethodOf(_ => Queryable.AsQueryable<int>(default(IEnumerable<int>)));

        public static MethodInfo QueryableOrderByGeneric => _orderByMethod;

        public static MethodInfo EnumerableOrderByGeneric => _enumerableOrderByMethod;

        public static MethodInfo QueryableOrderByDescendingGeneric => _orderByDescendingMethod;

        public static MethodInfo QueryableThenByGeneric => _thenByMethod;

        public static MethodInfo EnumerableThenByGeneric => _enumerableThenByMethod;

        public static MethodInfo QueryableThenByDescendingGeneric => _thenByDescendingMethod;

        public static MethodInfo QueryableCountGeneric => _countMethod;

        public static MethodInfo QueryableTakeGeneric => _queryableTakeMethod;

        public static MethodInfo EnumerableTakeGeneric => _enumerableTakeMethod;

        public static MethodInfo QueryableSkipGeneric => _skipMethod;

        public static MethodInfo QueryableWhereGeneric => _whereMethod;

        public static MethodInfo QueryableSelectGeneric => _queryableSelectMethod;

        public static MethodInfo EnumerableSelectGeneric => _enumerableSelectMethod;

        public static MethodInfo QueryableEmptyAnyGeneric => _queryableEmptyAnyMethod;

        public static MethodInfo QueryableNonEmptyAnyGeneric => _queryableNonEmptyAnyMethod;

        public static MethodInfo QueryableAllGeneric => _queryableAllMethod;

        public static MethodInfo EnumerableEmptyAnyGeneric => _enumerableEmptyAnyMethod;

        public static MethodInfo EnumerableNonEmptyAnyGeneric => _enumerableNonEmptyAnyMethod;

        public static MethodInfo EnumerableAllGeneric => _enumerableAllMethod;

        public static MethodInfo EnumerableOfType => _enumerableOfTypeMethod;

        public static MethodInfo QueryableOfType => _queryableOfTypeMethod;

        public static MethodInfo QueryableAsQueryable => _queryableAsQueryableMethod;

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
