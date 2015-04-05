using ITGame.Infrastructure.Data;

namespace ITGame.DBManager.ViewModels
{
    public class ArmorsViewModel : EntitiesViewModel<Models.Entities.Armor>
    {
        public ArmorsViewModel(IEntityRepository repository)
            : base(repository)
        {

        }
    }
}
