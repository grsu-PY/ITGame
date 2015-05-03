using ITGame.DBManager.Navigations;
using ITGame.Infrastructure.Data;

namespace ITGame.DBManager.ViewModels
{
    public class ArmorsViewModel : EntitiesViewModel<Models.Entities.Armor>
    {
        public ArmorsViewModel(INavigation navigation, IEntityRepository repository)
            : base(navigation, repository)
        {
            
        }

        public override void OnNavigated()
        {
            base.OnNavigated();

            LoadEntities(PageInfo);
        }
    }
}
