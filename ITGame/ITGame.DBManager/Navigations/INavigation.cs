using System;

namespace ITGame.DBManager.Navigations
{
    public interface INavigation
    {
        event EventHandler<ViewChangedEventArgs> ViewChanged;
        Type SelectedEntityViewType { get; }
        INavigatableViewModel CurrentEntityViewModel { get; }
        void SwitchToView(Type viewType, params object[] parameters);
        void SwitchToViewNotCached(Type viewType, params object[] parameters);
    }
}