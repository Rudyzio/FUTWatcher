using System.Collections.Generic;
using FUTWatcher.API.Models;

namespace FUTWatcher.API.Data.Interfaces {
    public interface IClubRepository : IRepository<Club> {
        IEnumerable<Club> GetClubsByLeague(long leagueId);
    }
}