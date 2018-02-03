
using System.Collections.Generic;
using System.Linq;
using FUTWatcher.API.Data.Interfaces;
using FUTWatcher.API.Models;

namespace FUTWatcher.API.Data.Repositories
{
    public class DailyProfitRepository : Repository<DailyProfit>, IDailyProfitRepository
    {
        public DailyProfitRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<DailyProfit> GetAllOrdered() {
            return _context.DailyProfits.OrderBy(dp => dp.Date);
        }
    }
}