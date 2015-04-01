using System;
using System.Collections.Generic;
using System.Linq;

namespace Gnosis.Entities.Exceptions
{
    public class EntityExistsException : GException
    {
        public EntityExistsException(Guid id)
            : base("An entity already exists for ID {0}", id)
        {
        }
    }

    public class InvalidEntityTypeException : GException
    {
        public InvalidEntityTypeException(string type)
            : base("The entity type '{0}' is invalid", type)
        {
        }
    }

    public class EntityNotFoundException : GException
    {
        public EntityNotFoundException(Guid id)
            : base("No entity exists for ID {0}", id)
        {
        }
    }

    public class MissingEntityTypeException : GException
    {
        public MissingEntityTypeException(string entityType, Type type)
            : base("No entity type found for {0}, {1}", entityType, type.FullName)
        {

        }
    }

    public class MissingEntityReferenceTypeException : GException
    {
        public MissingEntityReferenceTypeException(string entityType, Type type)
            : base("No entity reference type found for {0}, {1}", entityType, type.FullName)
        {

        }
    }

    public class MultipleEntityTypesException : GException
    {
        public MultipleEntityTypesException(string entityType, Type type, IEnumerable<Type> matchingTypes)
            : base("More than one entity type found for {0}, {1}: {2}", entityType, type.FullName, string.Join(", ", matchingTypes.Select(x => x.FullName)))
        {
        }
    }

    public class MultipleEntityReferenceTypesException : GException
    {
        public MultipleEntityReferenceTypesException(string entityType, Type type, IEnumerable<Type> matchingTypes)
            : base("More than one entity reference type found for {0}, {1}: {2}", entityType, type.FullName, string.Join(", ", matchingTypes.Select(x => x.FullName)))
        {
        }
    }

    public class UnpermittedEntityTypeException : GException
    {
        public UnpermittedEntityTypeException(string entityType, IEnumerable<string> allowedEntityTypes)
            : base("Entity type {0} is not in the list of allowed entity types ({1})", entityType, string.Join(", ", allowedEntityTypes))
        {
        }
    }

    public class MissingFieldSetMethodException : GException
    {
        public MissingFieldSetMethodException(Type entityType, string fieldName)
            : base("The type {0} has no set method for the field {1}", entityType.FullName, fieldName)
        {
        }
    }
}