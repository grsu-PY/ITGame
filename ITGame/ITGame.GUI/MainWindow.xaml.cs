using ITGame.Infrastructure.Extensions;
using ITGame.Infrastructure.Data;
using ITGame.Infrastructure.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Data;
using Microsoft.Win32;
namespace ITGame.GUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ComboBoxItem_Humanoid(object sender, RoutedEventArgs e)
        {
            SetHeaderDataGrid("Humanoid");
        }

        private void ComboBoxItem_Weapon(object sender, RoutedEventArgs e)
        {
            SetHeaderDataGrid("Weapon");
        }

        private void ComboBoxItem_Armor(object sender, RoutedEventArgs e)
        {
            SetHeaderDataGrid("Armor");
        }

        private void ComboBoxItem_Spell(object sender, RoutedEventArgs e)
        {
            SetHeaderDataGrid("Spell");
        }

        private void dataGrid_Initialized(object sender, EventArgs e)
        {
            SetHeaderDataGrid("Humanoid");
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
                    dataTable.Rows.RemoveAt(selectedItem);
                    dataGrid.ItemsSource = dataTable.DefaultView;
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
            var entities = EntityRepository.GetInstance(type).GetAll();

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

        private void readMenuItem_Click(object sender, RoutedEventArgs e)
        {
            modifyGroupBox.Visibility = Visibility.Hidden;
            //readGroupBox.Visibility = Visibility.Visible;
            modifyMenuItem.IsChecked = false;
            readMenuItem.IsChecked = true;
        }

        private void modifyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            modifyGroupBox.Visibility = Visibility.Visible;
            //readGroupBox.Visibility = Visibility.Hidden;
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
                repository = EntityRepository.GetInstance(entityType);
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
    }
}
