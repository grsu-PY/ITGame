using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.DBManager.Data
{
    public class NameValueItem<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }
    }
}
