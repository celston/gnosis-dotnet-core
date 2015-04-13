using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities.Examples.Users
{
    public interface IUserDataManager : IEntityDataManager
    {
        Guid? FindUserByCredentials(IUserCredentials credentials);
    }
}
