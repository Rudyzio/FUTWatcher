
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FUTWatcher.API.Data.Interfaces;
using FUTWatcher.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FUTWatcher.API.Data.Repositories
{
    public class PlayerVersionRepository : Repository<PlayerVersion>, IPlayerVersionRepository
    {
        public PlayerVersionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<PlayerVersion> GetPlayersByLeague(long leagueId)
        {
            return _context.PlayerVersions.Include(pv => pv.PlayerType)
                                          .Include(pv => pv.Nation)
                                          .Where(pv => pv.LeagueId == leagueId);
        }
        public IEnumerable<PlayerVersion> GetPlayersByClub(long clubId)
        {
            return _context.PlayerVersions.Include(pv => pv.PlayerType)
                                          .Include(pv => pv.Nation)
                                          .Where(pv => pv.ClubId == clubId);
        }

        public long TotalCost()
        {
            return _context.PlayerVersions.Sum(pv => pv.Cost);
        }

        public IEnumerable<PlayerVersion> NameHas(string name)
        {
            var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
            var players = _context.PlayerVersions.AsNoTracking()
                                .Include(pv => pv.PlayerType)
                                .Include(pv => pv.League)
                                .Include(pv => pv.Club)
                                .Where(pv => compareInfo.IndexOf(pv.PlayerType.FirstName, name, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) > -1 ||
                                      compareInfo.IndexOf(pv.PlayerType.LastName, name, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) > -1 ||
                                      compareInfo.IndexOf(pv.PlayerType.CommonName, name, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) > -1
                                ).OrderByDescending(pv => pv.Rating);
            return players;
        }

        public long GetTotalPlayersByLeague(long id)
        {
            return _context.PlayerVersions.Where(pv => pv.LeagueId == id).ToList().Count();
        }

        public long GetOwnedPlayersByLeague(long id)
        {
            return _context.PlayerVersions.Where(pv => pv.OwnedByMe == true && pv.LeagueId == id).ToList().Count();
        }

        public long GetPlayersByNation(long id)
        {
            return _context.PlayerVersions.Where(pv => pv.NationId == id)
                                          .Count();
        }

        public IEnumerable<PlayerVersion> GetNationPage(int pageIndex, int pageSize, long nationId)
        {
            return _context.PlayerVersions.Include(pv => pv.PlayerType)
                                          .Include(pv => pv.League)
                                          .Include(pv => pv.Club)
                                          .Where(pv => pv.NationId == nationId)
                                          .OrderByDescending(pv => pv.Rating)
                                          .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public IEnumerable<PlayerVersion> GetPlayersByRating(long rating)
        {
            return _context.PlayerVersions.Include(pv => pv.PlayerType)
                                          .Include(pv => pv.League)
                                          .Include(pv => pv.Club)
                                          .Where(pv => pv.Rating == rating)
                                          .ToList();

        }
    }
}