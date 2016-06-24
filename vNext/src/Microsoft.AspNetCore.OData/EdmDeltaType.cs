// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using Microsoft.AspNetCore.OData.Common;

namespace Microsoft.AspNetCore.OData
{
    /// <summary>
    /// Implementing IEdmType to identify objects which are part of DeltaFeed Payload.
    /// </summary>
    internal class EdmDeltaType : IEdmType
    {
        internal EdmDeltaType(IEdmEntityType entityType, EdmDeltaEntityKind deltaKind)
        {
            if (entityType == null)
            {
                throw Error.ArgumentNull("entityType");
            }
            EntityType = entityType;
            DeltaKind = deltaKind;
        }

        /// <inheritdoc />
        public EdmTypeKind TypeKind => EdmTypeKind.Entity;

        public IEdmEntityType EntityType { get; }

        /// <summary>
        /// Returning DeltaKind of the object within DeltaFeed payload
        /// </summary>
        public EdmDeltaEntityKind DeltaKind { get; }
    }
}
