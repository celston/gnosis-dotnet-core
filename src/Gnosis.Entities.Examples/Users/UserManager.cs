using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities.Examples.Users
{
    public class UserManager : EntityManager
    {
        private const string TYPE_USER = "User";
        
        private IUserDataManager userDataManager;
        
        public UserManager(IUserDataManager userDataManager)
            : base(userDataManager)
        {
            this.userDataManager = userDataManager;
        }

        public T LoadUserByCredentials<T>(IUserCredentials credentials)
            where T : IEntity, IUser
        {
            AssertNotIsNullOrWhiteSpace(credentials.Username, new UsernameMissingException());
            AssertNotIsNullOrWhiteSpace(credentials.Password, new PasswordMissingException());
            
            Guid? id = userDataManager.FindUserByCredentials(credentials);
            AssertHasValue(id, new InvalidCredentialsException(credentials));

            return LoadEntity<T>(id.Value, TYPE_USER);
        }
    }
}
