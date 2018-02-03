using System.Collections.Generic;
using FUTWatcher.API.Models;

namespace FUTWatcher.API.Data.Interfaces {
    public interface ILeagueRepository : IRepository<League> {
        IEnumerable<League> GetAllOrdered();
        long GetLeagueCost(long leagueId);
    }
}