using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ITGame.DBManager.Data;
using ITGame.Models.Administration;
using ITGame.Models.Equipment;
using ITGame.Models.Сreature;

namespace ITGame.DBManager.Converters
{
    public static class NameValueEnumCollections
    {
        private static readonly IEnumerable<NameValueItem<object>> _humanoidRacesList = new List<NameValueItem<object>>()
        {
            new NameValueItem<object>(){Name = "Choose Race", Value = HumanoidRaceType.None},
            new NameValueItem<object>(){Name = "Human", Value = HumanoidRaceType.Human},
            new NameValueItem<object>(){Name = "Elf", Value = HumanoidRaceType.Elf},
            new NameValueItem<object>(){Name = "Dwarf", Value = HumanoidRaceType.Dwarf},
            new NameValueItem<object>(){Name = "Orc", Value = HumanoidRaceType.Orc},
        };

        private static readonly IEnumerable<NameValueItem<object>> _armorTypesList = new List<NameValueItem<object>>()
        {
            new NameValueItem<object>() {Name = "Choose Armor Type", Value = ArmorType.None},
            new NameValueItem<object>() {Name = "Helmet", Value = ArmorType.Helmet},
            new NameValueItem<object>() {Name = "Body", Value = ArmorType.Body},
            new NameValueItem<object>() {Name = "Gloves", Value = ArmorType.Gloves},
            new NameValueItem<object>() {Name = "Boots", Value = ArmorType.Boots}
        };

        private static readonly IEnumerable<NameValueItem<object>> _characterTypesList = new List<NameValueItem<object>>()
        {
            new NameValueItem<object>() {Name = "Choose Role", Value = RoleType.None},
            new NameValueItem<object>() {Name = "User", Value = RoleType.User},
            new NameValueItem<object>() {Name = "Editor", Value = RoleType.Editor},
            new NameValueItem<object>() {Name = "Admin", Value = RoleType.Admin}
        };

        public static IEnumerable<NameValueItem<object>> GetCollection(Type enumType)
        {
            if (enumType == typeof (HumanoidRaceType))
            {
                return _humanoidRacesList;
            }
            if (enumType == typeof (ArmorType))
            {
                return _armorTypesList;
            }
            if (enumType == typeof (RoleType))
            {
                return _characterTypesList;
            }

            return new List<NameValueItem<object>>();
        }
    }

    [ValueConversion(typeof(Enum), typeof(IEnumerable<>))]
    public class EnumToCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Enum)) return null;

            var enumType = value.GetType();

            return NameValueEnumCollections.GetCollection(enumType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new object();
        }
    }
}
