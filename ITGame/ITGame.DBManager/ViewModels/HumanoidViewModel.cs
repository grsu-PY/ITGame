using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITGame.DBManager.Navigations;
using ITGame.Models.Entities;

namespace ITGame.DBManager.ViewModels
{
    public class HumanoidViewModel : EntityViewModel<Humanoid>
    {
        public HumanoidViewModel(INavigation navigation, Humanoid entity)
            : base(navigation, entity)
        {
        }

    }
}
