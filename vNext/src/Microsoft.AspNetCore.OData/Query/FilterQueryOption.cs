﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.OData.Core.UriParser;
using Microsoft.OData.Core.UriParser.Semantic;
using Microsoft.AspNetCore.OData.Common;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.AspNetCore.OData.Query.Validators;
using Microsoft.OData.Edm;

namespace Microsoft.AspNetCore.OData.Query
{
    /// <summary>
    /// This defines a $filter OData query option for querying.
    /// </summary>
    public class FilterQueryOption
    {
        private FilterClause _filterClause;
        private readonly ODataQueryOptionParser _queryOptionParser;

        /// <summary>
        /// Initialize a new instance of <see cref="FilterQueryOption"/> based on the raw $filter value and 
        /// an EdmModel from <see cref="ODataQueryContext"/>.
        /// </summary>
        /// <param name="rawValue">The raw value for $filter query. It can be null or empty.</param>
        /// <param name="context">The <see cref="ODataQueryContext"/> which contains the <see cref="IEdmModel"/> and some type information</param>
        /// <param name="queryOptionParser">The <see cref="ODataQueryOptionParser"/> which is used to parse the query option.</param>
        public FilterQueryOption(string rawValue, ODataQueryContext context, ODataQueryOptionParser queryOptionParser)
        {
            if (context == null)
            {
                throw Error.ArgumentNull("context");
            }

            if (string.IsNullOrEmpty(rawValue))
            {
                throw Error.ArgumentNullOrEmpty("rawValue");
            }

            if (queryOptionParser == null)
            {
                throw Error.ArgumentNull("queryOptionParser");
            }

            Context = context;
            RawValue = rawValue;
            Validator = new FilterQueryValidator();
            _queryOptionParser = queryOptionParser;
        }

        // This constructor is intended for unit testing only.
        internal FilterQueryOption(string rawValue, ODataQueryContext context)
        {
            if (context == null)
            {
                throw Error.ArgumentNull("context");
            }

            if (string.IsNullOrEmpty(rawValue))
            {
                throw Error.ArgumentNullOrEmpty("rawValue");
            }

            Context = context;
            RawValue = rawValue;
            Validator = new FilterQueryValidator();
            _queryOptionParser = new ODataQueryOptionParser(
                context.Model,
                context.ElementType,
                context.NavigationSource,
                new Dictionary<string, string> { { "$filter", rawValue } });
        }

        /// <summary>
        ///  Gets the given <see cref="ODataQueryContext"/>.
        /// </summary>
        public ODataQueryContext Context { get; }

        /// <summary>
        /// Gets or sets the Filter Query Validator
        /// </summary>
        public FilterQueryValidator Validator { get; set; }

        /// <summary>
        /// Gets the parsed <see cref="FilterClause"/> for this query option.
        /// </summary>
        public FilterClause FilterClause
        {
            get
            {
                if (_filterClause == null)
                {
                    _filterClause = _queryOptionParser.ParseFilter();
                    SingleValueNode filterExpression = _filterClause.Expression.Accept(
                        new ParameterAliasNodeTranslator(_queryOptionParser.ParameterAliasNodes)) as SingleValueNode;
                    filterExpression = filterExpression ?? new ConstantNode(null);
                    _filterClause = new FilterClause(filterExpression, _filterClause.RangeVariable);
                }

                return _filterClause;
            }
        }

        /// <summary>
        ///  Gets the raw $filter value.
        /// </summary>
        public string RawValue { get; private set; }

        /// <summary>
        /// Apply the filter query to the given IQueryable.
        /// </summary>
        /// <remarks>
        /// The <see cref="ODataQuerySettings.HandleNullPropagation"/> property specifies
        /// how this method should handle null propagation.
        /// </remarks>
        /// <param name="query">The original <see cref="IQueryable"/>.</param>
        /// <param name="querySettings">The <see cref="ODataQuerySettings"/> that contains all the query application related settings.</param>
        /// <returns>The new <see cref="IQueryable"/> after the filter query has been applied to.</returns>
        public IQueryable ApplyTo(IQueryable query, ODataQuerySettings querySettings)
        {
            // TODO
            throw new NotImplementedException();
            //return ApplyTo(query, querySettings, _defaultAssembliesResolver);
        }

        /// <summary>
        /// Apply the filter query to the given IQueryable.
        /// </summary>
        /// <remarks>
        /// The <see cref="ODataQuerySettings.HandleNullPropagation"/> property specifies
        /// how this method should handle null propagation.
        /// </remarks>
        /// <param name="query">The original <see cref="IQueryable"/>.</param>
        /// <param name="querySettings">The <see cref="ODataQuerySettings"/> that contains all the query application related settings.</param>
        /// <param name="assembliesProvider">The <see cref="IAssemblyProvider"/> to use.</param>
        /// <returns>The new <see cref="IQueryable"/> after the filter query has been applied to.</returns>
        public IQueryable ApplyTo(IQueryable query, ODataQuerySettings querySettings, IAssemblyProvider assembliesProvider)
        {
            if (query == null)
            {
                throw Error.ArgumentNull("query");
            }
            if (querySettings == null)
            {
                throw Error.ArgumentNull("querySettings");
            }
            if (assembliesProvider == null)
            {
                throw Error.ArgumentNull("assembliesResolver");
            }
            if (Context.ElementClrType == null)
            {
                throw Error.NotSupported(SRResources.ApplyToOnUntypedQueryOption, "ApplyTo");
            }

            FilterClause filterClause = FilterClause;
            Contract.Assert(filterClause != null);

            // Ensure we have decided how to handle null propagation
            ODataQuerySettings updatedSettings = querySettings;
            if (querySettings.HandleNullPropagation == HandleNullPropagationOption.Default)
            {
                updatedSettings = new ODataQuerySettings(updatedSettings);
                updatedSettings.HandleNullPropagation = HandleNullPropagationOptionHelper.GetDefaultHandleNullPropagationOption(query);
            }

            Expression filter = FilterBinder.Bind(filterClause, Context.ElementClrType, Context.Model, assembliesProvider, updatedSettings);
            query = ExpressionHelpers.Where(query, filter, Context.ElementClrType);
            return query;
        }

        /// <summary>
        /// Validate the filter query based on the given <paramref name="validationSettings"/>. It throws an ODataException if validation failed.
        /// </summary>
        /// <param name="validationSettings">The <see cref="ODataValidationSettings"/> instance which contains all the validation settings.</param>
        public void Validate(ODataValidationSettings validationSettings)
        {
            if (validationSettings == null)
            {
                throw Error.ArgumentNull("validationSettings");
            }

            if (Validator != null)
            {
                Validator.Validate(this, validationSettings);
            }
        }
    }
}