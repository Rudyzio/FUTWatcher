
using System.Collections.Generic;
using System.Linq;
using FUTWatcher.API.Data.Interfaces;
using FUTWatcher.API.Models;

namespace FUTWatcher.API.Data.Repositories
{
    public class NationRepository : Repository<Nation>, INationRepository
    {
        public NationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Nation> GetAllOrdered() {
            return _context.Nations.OrderBy(n => n.Name);
        }
    }
}