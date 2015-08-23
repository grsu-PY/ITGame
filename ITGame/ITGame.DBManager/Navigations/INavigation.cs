using System;

namespace ITGame.DBManager.Navigations
{
    public interface INavigation
    {
        event EventHandler<ViewChangedEventArgs> ViewChanged;
        Type SelectedEntityViewType { get; }
        INavigatableViewModel CurrentEntityViewModel { get; }
        void SwitchToView(Type viewType);
        void SwitchToView(Type viewType, object parameters);
        void SwitchToViewNotCached(Type viewType);
        void SwitchToViewNotCached(Type viewType, object parameters);
    }
}