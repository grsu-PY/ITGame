using ITGame.DBConnector.ITGameDBModels;
using ITGame.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.DBManager.ViewModels
{
    public class HumanoidsViewModel : EntitiesViewModel<Humanoid>
    {
        public HumanoidsViewModel(IEntityRepository repository)
            : base(repository)
        {

        }
    }
}
