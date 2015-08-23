using System;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace ITGame.DBManager.Extensions
{
    public static class UnityExtensions
    {
        public static T ResolveExt<T>(this IUnityContainer container, object parameterOverrides)
        {
            var properties = parameterOverrides
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var overridesArray = properties
                .Select(p => new ParameterOverride(p.Name, p.GetValue(parameterOverrides, null)))
                .Cast<ResolverOverride>()
                .ToArray();
            return container.Resolve<T>(overridesArray); //null needed to avoid a StackOverflow :)
        }

        public static T ResolveExt<T>(this IUnityContainer container, string name, object parameterOverrides)
        {
            var properties = parameterOverrides
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var overridesArray = properties
                .Select(p => new ParameterOverride(p.Name, p.GetValue(parameterOverrides, null)))
                .Cast<ResolverOverride>()
                .ToArray();
            return container.Resolve<T>(name, overridesArray); //null needed to avoid a StackOverflow :)
        }

        public static object ResolveExt(this IUnityContainer container, Type type, object parameterOverrides)
        {
            var properties = parameterOverrides
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var overridesArray = properties
                .Select(p => new ParameterOverride(p.Name, p.GetValue(parameterOverrides, null)))
                .Cast<ResolverOverride>()
                .ToArray();
            return container.Resolve(type, overridesArray); //null needed to avoid a StackOverflow :)
        }

        public static object ResolveExt(this IUnityContainer container, Type type, string name, object parameterOverrides)
        {
            var properties = parameterOverrides
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var overridesArray = properties
                .Select(p => new ParameterOverride(p.Name, p.GetValue(parameterOverrides, null)))
                .Cast<ResolverOverride>()
                .ToArray();
            return container.Resolve(type, name, overridesArray); //null needed to avoid a StackOverflow :)
        }
    }
}
