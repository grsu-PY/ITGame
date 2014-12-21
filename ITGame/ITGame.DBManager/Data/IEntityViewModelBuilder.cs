using System;
namespace ITGame.DBManager.Data
{
    public interface IEntityViewModelBuilder
    {
        object Resolve(Type type, params object[] parameters);
    }
}
