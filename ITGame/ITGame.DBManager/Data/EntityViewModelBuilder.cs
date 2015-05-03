using System;
using System.Collections.Generic;

namespace ITGame.DBManager.Data
{
    public class EntityViewModelBuilder : IEntityViewModelBuilder
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

        public object Resolve(Type type, bool cached, params object[] parameters)
        {
            object viewModel = null;
            if (cached) viewModel = Resolve(type, parameters);
            else
            {
                viewModel = Activator.CreateInstance(type, parameters);
                if (objects.ContainsKey(type))
                {
                    objects[type] = viewModel;
                }
                else
                {
                    objects.Add(type, viewModel);
                }
            }

            return viewModel;
        }
    }
}
