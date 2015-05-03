using System.Windows.Input;

namespace ITGame.DBManager.Navigations
{
    public interface IPagedViewModel
    {
        PageInfo PageInfo { get; set; }
        ICommand CommandNextPage { get; }
        ICommand CommandPreviousPage { get; }
    }
}
