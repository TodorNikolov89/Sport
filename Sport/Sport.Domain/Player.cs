using System.Collections.Generic;

namespace Sport.Domain
{
    public class Player
    {
        public Player()
        {
           // this.PlayedTournaments = new HashSet<Tournament>();
        }

        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

       // public ICollection<Tournament> PlayedTournaments { get; set; }

    }
}
