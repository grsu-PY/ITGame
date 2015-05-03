using System.Collections.Generic;
using ITGame.DBManager.Converters;
using ITGame.DBManager.Data;
using ITGame.DBManager.Navigations;
using ITGame.Infrastructure.Data;
using ITGame.Models.Administration;
using Character = ITGame.Models.Entities.Character;

namespace ITGame.DBManager.ViewModels
{
    public class CharactersViewModel : EntitiesViewModel<Character>
    {
        public CharactersViewModel(INavigation navigation, IEntityRepository repository)
            : base(navigation, repository)
        {

        }

        public IEnumerable<NameValueItem<object>> RolesList
        {
            get { return NameValueEnumCollections.GetCollection(typeof(RoleType)); }
        }

        public override void OnNavigated()
        {
            base.OnNavigated();

            LoadEntities(PageInfo);
        }
    }
}
