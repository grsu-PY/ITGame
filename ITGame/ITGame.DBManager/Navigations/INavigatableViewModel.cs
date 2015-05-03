using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.DBManager.Navigations
{
    public interface INavigatableViewModel
    {
        void OnNavigated();
        void OnBeforeNavigation();
    }
}
