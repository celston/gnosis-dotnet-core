using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnosis.Entities.Examples.Widgets
{
    public class WidgetManager : EntityManager
    {
        public WidgetManager(IWidgetDataManager dataManager)
            : base(dataManager)
        {
        }

        public async Task<Guid> CreateWidgetAsync(IWidgetCreateRequest request)
        {
            return await CreateEntityAsync(request, "Widget");
        }
        
        public Guid CreateWidget(IWidgetCreateRequest request)
        {
            return CreateEntity(request, "Widget");
        }

        public IEnumerable<T> LoadWidgets<T>(IEnumerable<Guid> ids)
            where T : IEntity, IWidget
        {
            return LoadEntities<T>(ids, typeof(T));
        }

        public async Task<IEnumerable<T>> LoadWidgetsAsync<T>(IEnumerable<Guid> ids)
            where T : IEntity, IWidget
        {
            return await LoadEntitiesAsync<T>(ids);
        }
    }
}
