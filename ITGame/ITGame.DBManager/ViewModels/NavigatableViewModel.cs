using ITGame.DBManager.Navigations;

namespace ITGame.DBManager.ViewModels
{
    public class NavigatableViewModel : ValidationViewModel, INavigatableViewModel
    {
        private readonly INavigation _navigation;

        public NavigatableViewModel(INavigation navigation)
        {
            _navigation = navigation;
        }

        public INavigation Navigation
        {
            get { return _navigation; }
        }

        public virtual void OnNavigated() { }
        public virtual void OnBeforeNavigation() { }
    }
}