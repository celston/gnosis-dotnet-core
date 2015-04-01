using System;

using Gnosis.Entities.Attributes;

namespace Stryker.PUPA.Surveys
{
    [EntityFieldsInterface]
    public interface ISurveyUser
    {
        [EntityField]
        Guid Survey { get; }
        [EntityField]
        string FirstName { get; }
        [EntityField]
        string LastName { get; }
    }
}
