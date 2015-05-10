using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITGame.Infrastructure.Data;
using ITGame.Models.Entities;

namespace ITGame.DBConnector
{
    public class HumanoidsDbProjector : EntityDbProjector<Humanoid>
    {
        public HumanoidsDbProjector(DbContext context)
            : base(context)
        {

        }

        public IEnumerable<Humanoid> GetHumanoidsWithCharacter()
        {
            return DbSet.Include(x => x.Character).ToList();
        }

        public async Task<IEnumerable<Humanoid>> GetHumanoidsWithCharacterAsync()
        {
            return await DbSet.Include(x => x.Character).ToListAsync();
        }

        public IEnumerable<Humanoid> GetHumanoidsWithAllItems()
        {
            return
                DbSet.Include(x => x.Character)
                    .Include(x => x.Weapons)
                    .Include(x => x.Armors)
                    .Include(x => x.Spells).ToList();
        }

        public async Task<IEnumerable<Humanoid>> GetHumanoidsWithAllItemsAsync()
        {
            return await DbSet
                .Include(x => x.Character)
                .Include(x => x.Weapons)
                .Include(x => x.Armors)
                .Include(x => x.Spells).ToListAsync();
        }
    }
}
