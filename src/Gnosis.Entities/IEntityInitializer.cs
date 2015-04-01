using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities
{
    public interface IEntityInitializer
    {
        void AcceptInitializeDelegate(InitializeEntityDelegate d);
    }
}