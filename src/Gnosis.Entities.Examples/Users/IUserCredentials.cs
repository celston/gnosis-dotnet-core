using System;

using Gnosis.Entities.Attributes;

namespace Gnosis.Entities.Examples.Users
{
    [EntityFieldsInterface]
    public interface IUserCredentials
    {
        [EntityField]
        string Username { get; }
        [EntityField]
        string Password { get; }
    }
}
