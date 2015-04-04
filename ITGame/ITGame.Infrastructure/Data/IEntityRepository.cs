using System;
namespace ITGame.Infrastructure.Data
{
    public interface IEntityRepository
    {
        IEntityProjector GetInstance(Type type);
        IEntityProjector<TEntity> GetInstance<TEntity>() where TEntity : class, Identity, new();
    }
}
