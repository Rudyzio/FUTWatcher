using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUTWatcher.API.Models {
    public class PlayerVersion {

        [Key]
        public long Id { get; set; }

        public string Position { get; set; }

        public string Quality { get; set; } 

        public string Color { get; set; }

        public long Rating { get; set; }

        public bool IsSpecialType { get; set; }

        public bool OwnedByMe { get; set; }

        public long Cost { get; set; }

        public long LastPrice { get; set; }
        public long Amount { get; set; }

        public long MarketCost { get; set; }
        public string UpdateDate { get; set; }

        public PlayerType PlayerType { get; set; }

        [ForeignKey("PlayerType")]
        public long PlayerTypeId { get; set; }

        public League League { get; set; }

        [ForeignKey("League")]
        public long LeagueId { get; set; }

        public Club Club { get; set; }

        [ForeignKey("Club")]
        public long ClubId { get; set; }
        
        [ForeignKey("Nation")]
        public long? NationId { get; set; }

        public Nation Nation { get; set; }

    }
}