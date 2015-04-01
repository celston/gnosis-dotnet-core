using System;

namespace Gnosis.Entities
{
    public interface IEntity : IEntityRead, IEntityFlags
    {
        void GrantInitializeEntityDelegate(IEntityInitializer initializer);
    }
}