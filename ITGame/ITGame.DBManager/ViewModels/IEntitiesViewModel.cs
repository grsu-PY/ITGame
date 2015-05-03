using System.Windows.Input;

namespace ITGame.DBManager.ViewModels
{
    public interface IEntitiesViewModel
    {
        object SelectedEntity { get; }
        ICommand CommandLoadEntities { get; }
        ICommand CommandSaveEntities { get; }
        ICommand CommandEditEntity { get; }
        ICommand CommandUpdateEntity { get; }
        ICommand CommandDeleteEntity { get; }
        ICommand CommandDeleteSelectedEntities { get; }
        ICommand CommandDeleteAllEntities { get; }
        ICommand CommandCreateEntity { get; }
    }
}