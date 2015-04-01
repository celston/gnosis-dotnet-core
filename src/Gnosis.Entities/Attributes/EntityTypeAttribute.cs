using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityTypeAttribute : Attribute
    {
        public string Name { get; private set; }

        public EntityTypeAttribute(string name)
        {
            this.Name = name;
        }
    }
}