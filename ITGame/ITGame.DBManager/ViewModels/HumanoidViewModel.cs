using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ITGame.DBManager.Navigations;
using ITGame.DBManager.Validators;
using ITGame.Infrastructure.Data;
using ITGame.Models.Creature;
using ITGame.Models.Entities;
using Humanoid = ITGame.Models.Entities.Humanoid;

namespace ITGame.DBManager.ViewModels
{
    public class HumanoidViewModel : EntityViewModel<Humanoid>
    {
        private Character _character;
        private ObservableCollection<Character> _characters;
        private string _name;
        private byte _level;
        private HumanoidRaceType _humanoidRaceType;
        private string _fileName;

        public HumanoidViewModel(INavigation navigation, IEntityRepository repository, Humanoid entity)
            : base(navigation, repository, entity)
        {
            Validator = new HumanoidValidator();
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

        public string Name
        {
            get { return _name; }
            set { _name = value;
                RaisePropertyChanged();
            }
        }

        public byte Level
        {
            get { return _level; }
            set { _level = value;
                RaisePropertyChanged();
            }
        }

        public HumanoidRaceType HumanoidRaceType
        {
            get { return _humanoidRaceType; }
            set { _humanoidRaceType = value;
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

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                RaisePropertyChanged();
            }
        }

        protected override void Save(object obj)
        {
            if (HasErrors) return;

            if (Entity.Id == Guid.Empty)
            {
                Entity.Id = Guid.NewGuid();
            }

            Entity.Level = Level;
            Entity.Name = Name;
            Entity.HumanoidRaceType = HumanoidRaceType;

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
            Name = Entity.Name;
            Level = Entity.Level;
            HumanoidRaceType = Entity.HumanoidRaceType;

            Validate();
        }

        private void LoadCharacters()
        {
            Characters = new ObservableCollection<Character>(Repository.GetInstance<Character>().GetAll());
            SelectedCharacter = Characters.FirstOrDefault(c => c.Id == Entity.CharacterId);
        }
    }
}
