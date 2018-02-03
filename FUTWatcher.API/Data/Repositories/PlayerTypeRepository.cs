
using FUTWatcher.API.Data.Interfaces;
using FUTWatcher.API.Models;

namespace FUTWatcher.API.Data.Repositories
{
    public class PlayerTypeRepository : Repository<PlayerType>, IPlayerTypeRepository
    {
        public PlayerTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}