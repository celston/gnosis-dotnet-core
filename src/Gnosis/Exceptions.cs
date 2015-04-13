using System;

namespace Gnosis
{
    public abstract class GException : Exception
    {
        public GException()
        {
        }   
        
        public GException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }
    }

    public class MissingEnvironmentHostnameException : GException
    {
        public MissingEnvironmentHostnameException(string key)
            : base("No hostname found for environment \"{0}\"", key)
        {
        }
    }

    public class UnknownEnvironmentHost : GException
    {
        public UnknownEnvironmentHost(string host)
            : base("Unknown host {0}", host)
        {
        }
    }

    public class MissingEnvironmentConnectionStringException : GException
    {
        public MissingEnvironmentConnectionStringException(string currentEnvironment)
            : base("No connection string found for {0}", currentEnvironment)
        {
        }
    }

    public class MissingConnectionStringException : GException
    {
        public MissingConnectionStringException(string key)
            : base("No connection string found for {0}", key)
        {
        }
    }
}
