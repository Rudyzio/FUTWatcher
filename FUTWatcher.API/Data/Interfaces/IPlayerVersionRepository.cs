using System;
using System.Collections.Generic;
using FUTWatcher.API.Models;

namespace FUTWatcher.API.Data.Interfaces {
    public interface IPlayerVersionRepository : IRepository<PlayerVersion> {
        IEnumerable<PlayerVersion> GetPlayersByLeague(long leagueId);
        IEnumerable<PlayerVersion> GetPlayersByClub(long clubId);
        long TotalCost();
        IEnumerable<PlayerVersion> NameHas(string name);
        long GetTotalPlayersByLeague(long id);
        long GetOwnedPlayersByLeague(long id);
        long GetPlayersByNation(long id);
        IEnumerable<PlayerVersion> GetNationPage(int pageIndex, int pageSize, long nationId);
        IEnumerable<PlayerVersion> GetPlayersByRating(long rating);
    }
}