using System;
using System.ComponentModel.DataAnnotations;

namespace FUTWatcher.API.Models {
    public class DailyProfit {

        [Key]
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public long Profit { get; set; }

        public long BronzePacksOpended { get; set; }

        public long MaxCoins { get; set; }

        public long MinCoins { get; set; }

        public long BronzePacksSpent { get; set; }
        
        public long BronzePacksEarned { get; set; }
    }
}