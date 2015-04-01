using System;
using System.Collections.Generic;

namespace Gnosis.Entities
{
    public delegate void InitializeEntityDelegate(Guid id, Guid revision, Guid? author, DateTime created, DateTime updated, IEnumerable<EntityFieldValue> fields);
}