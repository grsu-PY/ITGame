using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.DBManager.Data
{
    public class EntityViewModelBuilder : ITGame.DBManager.Data.IEntityViewModelBuilder
    {
        public static IDictionary<Type, object> objects = new Dictionary<Type, object>();

        public object Resolve(Type type, params object[] parameters)
        {
            object instanse = null;

            if (!objects.TryGetValue(type, out instanse))
            {
                instanse = Activator.CreateInstance(type, parameters);
                objects.Add(type, instanse);
            }

            return instanse;
        }
    }
}
