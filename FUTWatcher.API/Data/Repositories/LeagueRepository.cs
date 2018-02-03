
using System.Collections.Generic;
using System.Linq;
using FUTWatcher.API.Data.Interfaces;
using FUTWatcher.API.Models;

namespace FUTWatcher.API.Data.Repositories
{
    public class LeagueRepository : Repository<League>, ILeagueRepository
    {
        public LeagueRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<League> GetAllOrdered() {
            return _context.Leagues.OrderBy(l => l.Name);
        }

        public long GetLeagueCost(long leagueId) {
            return _context.PlayerVersions.Where(pv => pv.LeagueId == leagueId).Sum(pv => pv.Cost);
        }
    }
}