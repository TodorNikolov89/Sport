using System.Collections.Generic;

namespace Sport.Domain
{
    public class Player : User
    {
        public Player()
        {
            this.PlayedTournaments = new HashSet<Tournament>();
        }

        public ICollection<Tournament> PlayedTournaments { get; set; }
    }
}
