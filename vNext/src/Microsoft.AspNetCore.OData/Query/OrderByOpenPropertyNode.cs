﻿using Microsoft.AspNetCore.OData.Common;
using Microsoft.OData.Core;
using Microsoft.OData.Core.UriParser.Semantic;

namespace Microsoft.AspNetCore.OData.Query
{
    /// <summary>
    /// Represents ordering on a dynamic property
    /// </summary>
    public class OrderByOpenPropertyNode : OrderByNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderByOpenPropertyNode"/> class.
        /// </summary>
        /// <param name="orderByClause">The order by clause for this open property.</param>
        public OrderByOpenPropertyNode(OrderByClause orderByClause)
            : base(orderByClause.Direction)
        {
            if (orderByClause == null)
            {
                throw Error.ArgumentNull("orderByClause");
            }

            OrderByClause = orderByClause;

            var openPropertyExpression = orderByClause.Expression as SingleValueOpenPropertyAccessNode;
            if (openPropertyExpression == null)
            {
                throw new ODataException(SRResources.OrderByClauseNotSupported);
            }
            PropertyName = openPropertyExpression.Name;
        }

        /// <summary>
        /// The order by clause
        /// </summary>
        public OrderByClause OrderByClause { get; private set; }

        /// <summary>
        /// The name of the dynamic property
        /// </summary>
        public string PropertyName { get; private set; }
    }
}