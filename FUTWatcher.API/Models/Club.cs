using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUTWatcher.API.Models {
    public class Club {

        public Club() {
            PlayerVersions = new List<PlayerVersion>();
        }

        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public League League { get; set; }

        [ForeignKey("League")]
        public long LeagueId { get; set; }

        public IEnumerable<PlayerVersion> PlayerVersions { get; set; }
    }
}