using ITGame.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.DBConnector
{
    public class DBRepository : IEntityRepository
    {
        private static DbContext _context;

        public static DbContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new ITGameDBModels.ITGameDBContext();
                }
                return _context;
            }
        }

        public IEntityProjector GetInstance(Type type)
        {
            throw new NotImplementedException();
        }

        public IEntityProjector<TEntity> GetInstance<TEntity>() where TEntity : class, Identity, new()
        {
            return new EntityDBProjector<TEntity>(Context);
        }
    }
}
