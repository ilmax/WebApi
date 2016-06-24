// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using Microsoft.OData.Edm;
using Microsoft.AspNetCore.OData.Common;

namespace Microsoft.AspNetCore.OData.Builder
{
    /// <summary>
    /// Represents a PrimitiveType
    /// </summary>
    public class PrimitiveTypeConfiguration : IEdmTypeConfiguration
    {
        private readonly IEdmPrimitiveType _edmType;

        /// <summary>
        /// This constructor is public only for unit testing purposes.
        /// To get a PrimitiveTypeConfiguration use ODataModelBuilder.GetTypeConfigurationOrNull(Type)
        /// </summary>
        public PrimitiveTypeConfiguration(ODataModelBuilder builder, IEdmPrimitiveType edmType, Type clrType)
        {
            if (builder == null)
            {
                throw Error.ArgumentNull("builder");
            }
            if (edmType == null)
            {
                throw Error.ArgumentNull("edmType");
            }
            if (clrType == null)
            {
                throw Error.ArgumentNull("clrType");
            }
            ModelBuilder = builder;
            ClrType = clrType;
            _edmType = edmType;
        }

        /// <summary>
        /// Gets the backing CLR type of this EDM type.
        /// </summary>
        public Type ClrType { get; }

        /// <summary>
        /// Gets the full name of this EDM type.
        /// </summary>
        public string FullName => _edmType.FullName();

        /// <summary>
        ///  Gets the namespace of this EDM type.
        /// </summary>
        public string Namespace => _edmType.Namespace;

        /// <summary>
        /// Gets the name of this EDM type.
        /// </summary>
        public string Name => _edmType.Name;

        /// <summary>
        /// Gets the <see cref="EdmTypeKind"/> of this EDM type.
        /// </summary>
        public EdmTypeKind Kind => EdmTypeKind.Primitive;

        /// <summary>
        /// Gets the <see cref="ODataModelBuilder"/> used to create this configuration.
        /// </summary>
        public ODataModelBuilder ModelBuilder { get; }

        /// <summary>
        /// Returns the IEdmPrimitiveType associated with this PrimitiveTypeConfiguration
        /// </summary>
        public IEdmPrimitiveType EdmPrimitiveType => _edmType;
    }
}
