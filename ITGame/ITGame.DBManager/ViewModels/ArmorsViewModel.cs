using ITGame.DBConnector.ITGameDBModels;
using ITGame.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.DBManager.ViewModels
{
    public class ArmorsViewModel : EntitiesViewModel<Armor>
    {
        public ArmorsViewModel(IEntityRepository repository)
            : base(repository)
        {

        }
    }
}
