using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ITGame.DBManager.DataTemplateSelectors
{
    public class MainGridDataTemplateSelector : DataTemplateSelector
    {
        private readonly IDictionary<string, DataTemplate> Templates = new Dictionary<string, DataTemplate>();

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                var key = GetItemTemplateFilename(item);
                if (Templates.ContainsKey(key))
                {
                    return Templates[key];
                }
                Templates.Add(key, Application.Current.TryFindResource(key) as DataTemplate);
                return Templates[key];
            }
            return null;
        }

        private string GetItemTemplateFilename(object item)
        {
            return item.GetType().Name + "DataTemplate";
        }
    }
}
