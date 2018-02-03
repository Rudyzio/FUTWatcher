using System.Collections.Generic;
using FUTWatcher.API.Models;

namespace FUTWatcher.API.ViewModels {
    public class LeagueDataViewModel {
        public long Cost { get; set; }
        public long TotalPlayers { get; set; }
        public long OwnedPlayers { get; set; }
        public IEnumerable<Club> Clubs { get; set; }
    }
}