using System;

namespace Gnosis.Entities
{
    public interface IEntityMinimumRead
    {
        Guid Id { get; }
        Guid? Author { get; }
    }
}
