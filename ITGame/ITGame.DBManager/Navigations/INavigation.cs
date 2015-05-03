using System;

namespace ITGame.DBManager.Navigations
{
    public interface INavigation
    {
        event EventHandler<ViewChangedEventArgs> ViewChanged;
        Type SelectedEntityViewType { get; set; }
        object CurrentEntityViewModel { get; set; }
        void SwitchToView(Type viewType, params object[] parameters);
    }
}