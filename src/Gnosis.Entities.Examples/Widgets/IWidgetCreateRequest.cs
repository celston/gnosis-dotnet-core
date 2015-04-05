using System;

using Gnosis.Entities.Requests;

namespace Gnosis.Entities.Examples.Widgets
{
    public interface IWidgetCreateRequest : IEntityCreateRequest, IWidget
    {
    }
}
