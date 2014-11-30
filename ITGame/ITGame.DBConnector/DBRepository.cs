using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.DBConnector
{
    public static class DBRepository
    {
        public static EntityDBProjector<T> GetInstance<T>() where T : class,new()
        {
            return new EntityDBProjector<T>(Context);
        }

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
    }
}
