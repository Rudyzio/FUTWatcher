
using System.Collections.Generic;
using System.Linq;
using FUTWatcher.API.Data.Interfaces;
using FUTWatcher.API.Models;

namespace FUTWatcher.API.Data.Repositories
{
    public class ClubRepository : Repository<Club>, IClubRepository
    {
        public ClubRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Club> GetClubsByLeague(long leagueId) {
            var clubs = _context.Clubs.Where(c => c.LeagueId == leagueId).OrderBy(c => c.Name);
            return clubs;
        }
    }
}