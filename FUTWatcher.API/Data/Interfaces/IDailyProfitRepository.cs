using System.Collections.Generic;
using FUTWatcher.API.Models;

namespace FUTWatcher.API.Data.Interfaces {
    public interface IDailyProfitRepository : IRepository<DailyProfit>
    {
        IEnumerable<DailyProfit> GetAllOrdered();
    }
}