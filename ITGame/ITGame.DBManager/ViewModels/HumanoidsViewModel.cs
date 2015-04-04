using ITGame.DBConnector.ITGameDBModels;
using ITGame.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITGame.DBManager.Converters;
using ITGame.DBManager.Data;
using ITGame.Models.Сreature;

namespace ITGame.DBManager.ViewModels
{
    public class HumanoidsViewModel : EntitiesViewModel<Models.Entities.Humanoid>
    {

        public HumanoidsViewModel(IEntityRepository repository)
            : base(repository)
        {

        }

        public ObservableCollection<NameValueItem<HumanoidRaceType>> HumanoidRacesList
        {
            get { return _humanoidRacesList; }
        }
        
    }
}
