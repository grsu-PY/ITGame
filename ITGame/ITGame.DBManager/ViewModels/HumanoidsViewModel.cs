using ITGame.Infrastructure.Data;
using System;
using System.Collections.Generic;
using ITGame.DBManager.Converters;
using ITGame.DBManager.Data;
using ITGame.DBManager.Navigations;
using ITGame.Models.Creature;

namespace ITGame.DBManager.ViewModels
{
    public class HumanoidsViewModel : EntitiesViewModel<Models.Entities.Humanoid>
    {
        public HumanoidsViewModel(INavigation navigation, IEntityRepository repository)
            : base(navigation, repository)
        {
        }
        
        protected override void EditEntity(object obj)
        {
            Navigation.SwitchToViewNotCached(typeof (HumanoidViewModel),
                new {entity = obj ?? new Models.Entities.Humanoid()});
        }

        protected override void CreateEntity(object obj)
        {
            Navigation.SwitchToViewNotCached(typeof (HumanoidViewModel), 
                new {entity = new Models.Entities.Humanoid()});
        }

        public IEnumerable<NameValueItem<object>> HumanoidRacesList
        {
            get { return NameValueEnumCollections.GetCollection(typeof(HumanoidRaceType)); }
        }

        public override void OnNavigated()
        {
            base.OnNavigated();

            LoadEntities(PageInfo);
        }
    }
}
