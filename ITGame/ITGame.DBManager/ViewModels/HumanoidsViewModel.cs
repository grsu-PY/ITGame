using ITGame.DBConnector.ITGameDBModels;
using ITGame.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITGame.DBManager.Data;
using ITGame.Models.Сreature;

namespace ITGame.DBManager.ViewModels
{
    public class HumanoidsViewModel : EntitiesViewModel<Models.Сreature.Humanoid>
    {
        private readonly ObservableCollection<NameValueItem<HumanoidRaceType>> _humanoidRacesList = new ObservableCollection<NameValueItem<HumanoidRaceType>>()
        {
            new NameValueItem<HumanoidRaceType>(){Name = "Choose Race", Value = HumanoidRaceType.None},
            new NameValueItem<HumanoidRaceType>(){Name = "Human", Value = HumanoidRaceType.Human},
            new NameValueItem<HumanoidRaceType>(){Name = "Elf", Value = HumanoidRaceType.Elf},
            new NameValueItem<HumanoidRaceType>(){Name = "Dwarf", Value = HumanoidRaceType.Dwarf},
            new NameValueItem<HumanoidRaceType>(){Name = "Orc", Value = HumanoidRaceType.Orc},
        };

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
