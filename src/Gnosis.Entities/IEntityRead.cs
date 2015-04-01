using System;

namespace Gnosis.Entities
{
    public interface IEntityRead : IEntityMinimumRead, IEntityFlags
    {
        Guid Revision { get; }
        DateTime Created { get; }
        DateTime Updated { get; }
    }
}