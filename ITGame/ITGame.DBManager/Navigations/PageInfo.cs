using System.ComponentModel;
using System.Runtime.CompilerServices;
using ITGame.DBManager.Annotations;

namespace ITGame.DBManager.Navigations
{
    public class PageInfo:INotifyPropertyChanged
    {
        private int _page = 1;
        private int _pagesCount = int.MaxValue;
        private int _itemsCount;

        public int Page
        {
            get { return _page; }
            set
            {
                int newPage = value;
                if (newPage < 1) newPage = 1;
                if (newPage > PagesCount) newPage = PagesCount;
                _page = newPage;

                OnPropertyChanged();
            }
        }

        public int PagesCount
        {
            get { return _pagesCount; }
            set
            {
                _pagesCount = value < 0 ? 0 : value;
                OnPropertyChanged();
            }
        }

        public int ItemsCount
        {
            get { return _itemsCount; }
            set
            {
                _itemsCount = value < 0 ? 0 : value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}