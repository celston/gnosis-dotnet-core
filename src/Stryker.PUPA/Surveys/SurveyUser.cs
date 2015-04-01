using System;

using Gnosis.Entities;

namespace Stryker.PUPA.Surveys
{
    public class SurveyUser : Entity, ISurveyUser
    {
        public Guid Survey { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
    }
}
