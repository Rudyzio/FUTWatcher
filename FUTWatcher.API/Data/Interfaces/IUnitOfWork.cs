using System;

namespace FUTWatcher.API.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IClubRepository Clubs { get; }
        ILeagueRepository Leagues { get; }
        INationRepository Nations { get; }
        IPlayerTypeRepository PlayerTypes { get; }
        IPlayerVersionRepository PlayerVersions { get; }
        IDailyProfitRepository DailyProfits { get; }
        IUserRepository Users { get; }
        int Complete();
        long GetTotalPlayers();
        long GetOwnedPlayers();
        void FillDatabase();
        void UpdateAllPrices();
        void UpdateBronzePrices();
        long MyPlayers(string message);
        long Bought(string message);
        long Sold(string message);
        void ResetOwned();
        void BronzePackOpened(long value);
        long ConsumablesSold(long amount, long costEach, bool bronze);
        void UpdateCoins(long coins);
        void SoldPlayer(long id, long value);
        void BoughtPlayer(long id, long value);
    }
}