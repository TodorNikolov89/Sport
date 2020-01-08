namespace Sport.ViewModels.Set
{
    using Game;
    using User;
    using Match;
    using TieBreak;

    using System.Collections.Generic;
    using System.Linq;

    public class SetViewModel
    {
        public int Id { get; set; }

        public ICollection<GameViewModel> Games { get; set; }

        public string PlayerId { get; set; }
        public UserDrawViewModel Player { get; set; }

        public GameViewModel LastGame => this.Games.ToList().LastOrDefault();

        public int MatchId { get; set; }

        public MatchScoreViewModel Match { get; set; }

        public TieBreakViewModel TieBreak { get; set; }

        public bool IsSetFinished { get; set; }

        public bool HasTieBreak { get; set; }

        public int FirstPlayerGames { get; set; }

        public int SecondPlayerGames { get; set; }

        public int FirsPlayerTieBreakPoints { get; set; }

        public int SecondPlayerTieBreakPoints { get; set; }
    }
}
