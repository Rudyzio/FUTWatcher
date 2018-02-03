using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FUTWatcher.API.Models {
    public class Nation {

        public Nation() {
            PlayerVersions = new List<PlayerVersion>();
        }

        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<PlayerVersion> PlayerVersions { get; set; } 
    }
}