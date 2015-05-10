using ITGame.Infrastructure.Extensions;
using ITGame.Infrastructure.Data;
using ITGame.Infrastructure.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using Microsoft.Win32;
using ITGame.DBConnector;
using ITGame.Models.Сreature;

namespace ITGame.GUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Type currentEntityType;
        private List<string> c = new List<string> { "Humanoid", "Weapon", "Armor", "Spell" };
        private static IEntityRepository _repository;
        private static IEntityRepository _dbrepository;
        static MainWindow()
        {
            _repository = new EntityRepository<EntityProjectorXml>();
            _dbrepository = new DbRepository();
        }
        public MainWindow()
        {
            InitializeComponent();
            entityComboBox.ItemsSource = c;

            //WorkWithDb();
        }

        private static void WorkWithDb()
        {
            ////////////////////////////////////////////////////////////////////////////////////
            ///Все типы имеют полное имя, чтоб было видно из какого пространства имен они взяты.
            ///DBConnector.ITGameDBModels.Armor для работы с базой
            ///ITGame.Models.Equipment.Armor для работы в приложении
            ///И так для всех типов, которые должны хранится в базе. Или почти для всех.. хз ;)
            ////////////////////////////////////////////////////////////////////////////////////
            var armor = new DBConnector.ITGameDBModels.Armor
            {
                ArmorType = ITGame.Models.Equipment.ArmorType.Body,
                Equipped = true,
                Id = Guid.NewGuid(),
                MagicalDef = 23,
                PhysicalDef = 43,
                Weight = 100,
                Name = "Good Name"
            };

            Guid charId;
            bool wasNull = false;
            var characterDB = _dbrepository.GetInstance<DBConnector.ITGameDBModels.Character>().GetAll().FirstOrDefault();
            if (characterDB == null)
            {
                wasNull = true;
                var _charId = Guid.NewGuid();
                characterDB = new DBConnector.ITGameDBModels.Character
                {
                    Id = _charId,
                    Name = "aliaksei  " + _charId.ToString().Substring(0, 10),//из-за ограничения в 40 char в базе для стринги, обрезаю строку
                    Password = "password",
                    Role = 1
                };
            }
            charId = characterDB.Id;

            var humanoid = new DBConnector.ITGameDBModels.Humanoid
            {
                Id = Guid.NewGuid(),
                CharacterId = charId,
                Name = "der4mInc  " + charId.ToString().Substring(0, 10),
                Level = 1,
                Strength = 10,
                Wisdom = 20,
                Constitution = 10,
                HumanoidRaceType = HumanoidRaceType.Human,
                Agility = 10
            };
            humanoid.Armors.Add(armor);
            characterDB.Humanoids.Add(humanoid);
            armor.Humanoids.Add(humanoid);
            if (wasNull)
            {
                _dbrepository.GetInstance<DBConnector.ITGameDBModels.Character>().Add(characterDB);
            }
            else
            {
                _dbrepository.GetInstance<DBConnector.ITGameDBModels.Character>().Update(characterDB);
            }
            _dbrepository.GetInstance<DBConnector.ITGameDBModels.Character>().SaveChanges();

            #region Add Fixed Races
            /* //Run only one time
            var Races = new List<DBConnector.ITGameDBModels.HumanoidRace>
            {
                new DBConnector.ITGameDBModels.HumanoidRace
                {
                    HumanoidRaceType=HumanoidRaceType.Human,
                    Name="Human"
                },
                new DBConnector.ITGameDBModels.HumanoidRace
                {
                    HumanoidRaceType=HumanoidRaceType.Elf,
                    Name="Elf"
                },
                new DBConnector.ITGameDBModels.HumanoidRace
                {
                    HumanoidRaceType=HumanoidRaceType.Dwarf,
                    Name="Dwarf"
                },
                new DBConnector.ITGameDBModels.HumanoidRace
                {
                    HumanoidRaceType=HumanoidRaceType.Orc,
                    Name="Orc"
                },
                new DBConnector.ITGameDBModels.HumanoidRace
                {
                    HumanoidRaceType=HumanoidRaceType.None,
                    Name="None"
                },
            };

            DBRepository.GetInstance<DBConnector.ITGameDBModels.HumanoidRace>().DbSet.AddRange(Races);
            DBRepository.GetInstance<DBConnector.ITGameDBModels.HumanoidRace>().SaveChanges();
            */
            #endregion
        }

        private void SetHeaderDataGrid(string entity) 
        {
            var type = TypeExtension.GetTypeFromModelsAssembly(entity);
            var props = TypeExtension.GetSetGetProperties(type);

            if (dataGrid != null)
            {
                if (dataGrid.ItemsSource != null)
                    DataGridClear(true);
                else dataGrid.Items.Clear();

                DataTable dataTable = new DataTable();
                foreach (var property in props)
                {
                    if (!property.Name.Equals("Id"))
                        dataTable.Columns.Add(new DataColumn()
                        {
                            ColumnName = property.Name,
                            DataType = typeof(string)
                        });
                }

                dataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void DataGridClear(bool isColumnsClear = false) 
        {
            var table = dataGrid.ItemsSource as DataView;
            DataTable dataTable = table.Table;
            dataTable.Rows.Clear();
            if(isColumnsClear)
                dataTable.Columns.Clear();

            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void DataGridAddRow() 
        {
            var table = dataGrid.ItemsSource as DataView;
            DataTable dataTable = table.Table;
            dataTable.Rows.Add(dataTable.NewRow());

            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void DataGridRemoveRow() 
        {
            int selectedItem = dataGrid.SelectedIndex;
            if (selectedItem != -1)
            {
                var table = dataGrid.ItemsSource as DataView;
                DataTable dataTable = table.Table;
                if (selectedItem < table.Count)
                {
                    string id = dataTable.Rows[selectedItem]["Id"] as string;
                    Guid entityId;
                    if (Guid.TryParse(id, out entityId))
                    {
                        dataTable.Rows.RemoveAt(selectedItem);
                        dataGrid.ItemsSource = dataTable.DefaultView;

                        _repository.GetInstance(currentEntityType).Delete(entityId);
                    }
                }
            }
        }

        private void addRowButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridAddRow();
        }

        private void removeRowButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridRemoveRow();
        }

        private void removeAllButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridClear();
        }

        private void loadTableButton_Click(object sender, RoutedEventArgs e)
        {
            string entity = entityComboBox.Text;
            var type = TypeExtension.GetTypeFromModelsAssembly(entity);
            var entities = _repository.GetInstance(type).GetAll();

            var properties = TypeExtension.GetSetGetProperties(type);

            DataTable dataTable = new DataTable();
            foreach (var prop in properties)
                dataTable.Columns.Add(new DataColumn()
                {
                    ColumnName = prop.Name,
                    DataType = typeof(string)
                });

            foreach (var ent in entities)
            {
                DataRow drow = dataTable.NewRow();
                foreach (var prop in properties)
                {
                    string propValue = string.Empty;
                    if(prop.GetValue(ent) != null)
                        propValue = prop.GetValue(ent).ToString();
                    drow[prop.Name] = propValue;
                    
                }
                dataTable.Rows.Add(drow);
            }

            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void FillExpander(string entity, Grid eGrid) 
        {
            var type = TypeExtension.GetTypeFromModelsAssembly(entity);
            var entities = _repository.GetInstance(type).GetAll();

            int row = 0;
            foreach (var ent in entities)
            {
                eGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                var expander = new Expander();
                expander.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                expander.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                expander.ExpandDirection = ExpandDirection.Down;
                expander.Header = (ent.Name != null) ? ent.Name : "Unnamed";
                expander.SetValue(Grid.RowProperty, row);
                expander.Margin = new Thickness(20, 0, 0, 0);

                Label eData = new Label();
                eData.Content = ent.ToString();
                eData.Margin = new Thickness(20, 0, 0, 0);
                eData.FontSize = 17;
                eData.FontWeight = FontWeights.Thin;
                expander.Content = eData;

                eGrid.Children.Add(expander);

                row++;
            }
        }

        private void readMenuItem_Click(object sender, RoutedEventArgs e)
        {
            modifyGroupBox.Visibility = Visibility.Hidden;
            readGroupBox.Visibility = Visibility.Visible;
            modifyMenuItem.IsChecked = false;
            readMenuItem.IsChecked = true;

            FillAllExpanders();
        }

        private void FillAllExpanders() 
        {
            FillExpander("Humanoid", humanoidEGrid);
            FillExpander("Weapon", weaponEGrid);
            FillExpander("Armor", armorEGrid);
            FillExpander("Spell", spellEGrid);
        
        }

        private void modifyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            modifyGroupBox.Visibility = Visibility.Visible;
            readGroupBox.Visibility = Visibility.Hidden;
            modifyMenuItem.IsChecked = true;
            readMenuItem.IsChecked = false;
        }

        private void saveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string entity = entityComboBox.Text;
            GuiParser parser = new GuiParser(entity, ((DataView)dataGrid.ItemsSource).Table);
            List<GuiData> guiArgs = parser.Parse();
            IEntityProjector repository;
            if (guiArgs.Any())
            {
                var entityType = TypeExtension.GetTypeFromModelsAssembly(guiArgs.First().EntityType);
                repository = _repository.GetInstance(entityType);
            }
            else
            {
                return;
            }             

            foreach (var gData in guiArgs)
            {
                var newEntity = repository.Create(gData.Properties);
                if (newEntity.Id == Guid.Empty)
                    newEntity.Id = Guid.NewGuid();

                try
                {
                    repository.AddOrUpdate(newEntity);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }

            repository.SaveChanges();
        }

        private void savetoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sFile = new SaveFileDialog();
            if (sFile.ShowDialog().HasValue) 
            {
                //
            }
        }

        private void entityComboBox_ChangeEntity(object sender, SelectionChangedEventArgs e)
        {
            string type = e.AddedItems[0].ToString();
            currentEntityType = TypeExtension.GetTypeFromModelsAssembly(type);
            SetHeaderDataGrid(type);
        }
    }
}
