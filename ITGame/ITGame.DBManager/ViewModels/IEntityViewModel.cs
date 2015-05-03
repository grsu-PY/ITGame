using System.Windows.Input;

namespace ITGame.DBManager.ViewModels
{
    public interface IEntityViewModel
    {
        object SelectedEntity { get; }
        ICommand CommandSave { get; }
        ICommand CommandCancel { get; }
        ICommand CommandDelete { get; }
    }
}