using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FUTWatcher.API.Models {
    public class PlayerType {

        public PlayerType() {
            PlayerVersions = new List<PlayerVersion>();
        }

        [Key]
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string CommonName { get; set; }

        IEnumerable<PlayerVersion> PlayerVersions { get; set; }
    }
}