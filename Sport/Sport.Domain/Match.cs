namespace Sport.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Match
    {
        public Match()
        {
            this.Sets = new HashSet<Set>();
        }

        [Key]
        public int Id { get; set; }

        public string FirstPlayerId { get; set; }
        public User FirstPlayer { get; set; }

        public string SecondPlayerId { get; set; }
        public User SecondPlayer { get; set; }

        public string UmpireId { get; set; }
        public User Umpire { get; set; }

        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }


        public ICollection<Set> Sets { get; set; }

        public int FirstPlayerSets { get; set; }

        public int SecondPlayerSets { get; set; }

        public bool IsActive { get; set; }

        public bool IsFinished { get; set; }

        //TODO Add Foot Faults, Double Faults

    }
}
