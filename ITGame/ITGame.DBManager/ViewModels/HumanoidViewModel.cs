using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITGame.DBManager.Navigations;
using ITGame.Infrastructure.Data;
using ITGame.Models.Entities;

namespace ITGame.DBManager.ViewModels
{
    public class HumanoidViewModel : EntityViewModel<Humanoid>
    {
        private Character _character;
        private ObservableCollection<Character> _characters;

        public HumanoidViewModel(INavigation navigation, IEntityRepository repository, Humanoid entity)
            : base(navigation, repository, entity)
        {
        }

        public Character SelectedCharacter
        {
            get { return _character; }
            set
            {
                _character = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Character> Characters
        {
            get { return _characters; }
            set
            {
                _characters = value;
                RaisePropertyChanged();
            }
        }

        protected override void Save(object obj)
        {
            if (Entity.Id == Guid.Empty)
            {
                Entity.Id = Guid.NewGuid();
            }

            Entity.Character = SelectedCharacter;
            EntityContext.AddOrUpdate(Entity);
            EntityContext.SaveChanges();
            Navigation.SwitchToView(typeof(HumanoidsViewModel));
        }

        protected override void Cancel(object obj)
        {
            Navigation.SwitchToView(typeof(HumanoidsViewModel));
        }

        protected override void Delete(object obj)
        {
            EntityContext.Delete(Entity);
            EntityContext.SaveChanges();
            Navigation.SwitchToView(typeof(HumanoidsViewModel));
        }

        public override void OnNavigated()
        {
            base.OnNavigated();
            LoadCharacters();
        }

        private void LoadCharacters()
        {
            Characters = new ObservableCollection<Character>(Repository.GetInstance<Character>().GetAll());
            SelectedCharacter = Characters.FirstOrDefault(c => c.Id == Entity.CharacterId);
        }
    }
}
