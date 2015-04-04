using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace ITGame.Infrastructure.Data
{
    public class EntityRepository<TEntityProjector> : IEntityRepository where TEntityProjector : EntityProjector
    {        
        private static readonly string _dbName;
        private static readonly string _dbPath;
        private static readonly string delim = ";";
        private const string DBFileDelim = "DBFileDelim";
        private const string DBLocation = "DBLocation";
        static EntityRepository()
        {
            var dbPath = ConfigurationManager.AppSettings[DBLocation];
            delim = ConfigurationManager.AppSettings[DBFileDelim];

            if (string.IsNullOrEmpty(dbPath) || string.IsNullOrWhiteSpace(dbPath))
            {
                _dbName = string.Empty;
                dbPath = string.Empty;
            }
            else
            {
                _dbName = Path.GetDirectoryName(dbPath);
                if (!Directory.Exists(dbPath)) Directory.CreateDirectory(dbPath);
            }

            _dbPath = Path.Combine(dbPath);

        }

        public static string DELIM { get { return delim; } }
        public static string PATH { get { return _dbPath; } }


        public IEntityProjector<TEntity> GetInstance<TEntity>() where TEntity : class, Identity, new()
        {
            return new EntityProjector<TEntity>(GetInstance(typeof(TEntity)));
        }

        public IEntityProjector GetInstance(Type type)
        {
            return EntityRepositoryBase.GetInstance(type, this);
        }

        private static class EntityRepositoryBase
        {
            private static readonly IDictionary<Type, IEntityProjector> _eps = new Dictionary<Type, IEntityProjector>();

            public static IEntityProjector GetInstance(Type type, IEntityRepository repository)
            {
                IEntityProjector ep;
                if (!_eps.TryGetValue(type, out ep))
                {
                    ep = Activator.CreateInstance(typeof(TEntityProjector), type, repository) as TEntityProjector;
                    _eps.Add(type, ep);
                }

                return ep;
            }
        }
    }
}
