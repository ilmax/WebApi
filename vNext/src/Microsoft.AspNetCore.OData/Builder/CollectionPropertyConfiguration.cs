// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.AspNetCore.OData.Common;
using System.Reflection;

namespace Microsoft.AspNetCore.OData.Builder
{
    using System;

    /// <summary>
    /// CollectionPropertyConfiguration represents a CollectionProperty on either an EntityType or ComplexType.
    /// </summary>
    public class CollectionPropertyConfiguration : StructuralPropertyConfiguration
    {
        private readonly Type _elementType;

        /// <summary>
        /// Constructs a CollectionPropertyConfiguration using the <paramref name="property">property</paramref> provided.
        /// </summary>
        public CollectionPropertyConfiguration(PropertyInfo property, StructuralTypeConfiguration declaringType)
            : base(property, declaringType)
        {
            if (!property.PropertyType.IsCollection(out _elementType))
            {
                throw Error.Argument("property", SRResources.CollectionPropertiesMustReturnIEnumerable, property.Name, property.DeclaringType.FullName);
            }
        }

        /// <inheritdoc />
        public override PropertyKind Kind => PropertyKind.Collection;

        /// <inheritdoc />
        public override Type RelatedClrType => ElementType;

        /// <summary>
        /// Returns the type of Elements in the Collection
        /// </summary>
        public Type ElementType => _elementType;

        /// <summary>
        /// Sets the CollectionProperty to optional (i.e. nullable).
        /// </summary>
        public CollectionPropertyConfiguration IsOptional()
        {
            OptionalProperty = true;
            return this;
        }

        /// <summary>
        /// Sets the CollectionProperty to required (i.e. non-nullable).
        /// </summary>
        public CollectionPropertyConfiguration IsRequired()
        {
            OptionalProperty = false;
            return this;
        }
    }
}
