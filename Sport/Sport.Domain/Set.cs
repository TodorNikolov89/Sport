namespace Sport.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class Set
    {
        public Set()
        {
            this.Games = new HashSet<Game>();
            FirstPlayerGames = 0;
            SecondPlayerGames = 0;
            FirsPlayerTieBreakPoints = 0;
            SecondPlayerTieBreakPoints = 0;
        }

        [Key]
        public int Id { get; set; }

        public ICollection<Game> Games { get; set; }

        public string PlayerId { get; set; }
        public User Player { get; set; }

        public Game LastGame => this.Games.ToList().LastOrDefault();

        public int MatchId { get; set; }

        public Match Match { get; set; }

        public TieBreak TieBreak { get; set; }

        public bool IsSetFinished { get; set; }

        public bool HasTieBreak { get; set; }

        public int FirstPlayerGames { get; set; }

        public int SecondPlayerGames { get; set; }

        public int FirsPlayerTieBreakPoints { get; set; }

        public int SecondPlayerTieBreakPoints { get; set; }



    }
}
