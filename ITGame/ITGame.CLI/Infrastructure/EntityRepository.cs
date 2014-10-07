using ITGame.CLI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.CLI.Infrastructure
{
    public static class EntityRepository
    {        
        private static readonly string _dbName;
        private static readonly string _dbPath;
        private static readonly string ext;
        private static readonly string delim = ";";
        private const string DBFileDelim = "DBFileDelim";
        private const string DBFileExt = "DBFileExt";
        private const string DBLocation = "DBLocation";
        static EntityRepository()
        {
            var dbPath = ConfigurationManager.AppSettings[DBLocation];
            ext = ConfigurationManager.AppSettings[DBFileExt];
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

        public static string EXT { get { return ext; } }
        public static string DELIM { get { return delim; } }
        public static string PATH { get { return _dbPath; } }


        public static IEntityProjector<T> GetInstance<T>() where T : class,Identity, new()
        {
            return new EntityProjector<T>(GetInstance(typeof(T)));
        }

        public static IEntityProjector GetInstance(Type type)
        {
            return EntityRepositoryBase.GetInstance(type);
        }

        private static class EntityRepositoryBase
        {
            private static readonly IDictionary<Type, IEntityProjector> _eps = new Dictionary<Type, IEntityProjector>();

            public static IEntityProjector GetInstance(Type type)
            {
                IEntityProjector ep;
                if (!_eps.TryGetValue(type, out ep))
                {
                    ep = new EntityProjector(type);
                    _eps.Add(type, ep);
                }

                return ep;
            }
        }
    }
}
