namespace Sport.ViewModels.Match
{
    using Set;
    using User;

    using System.Collections.Generic;

    public class LiveMatchesViewModel
    {
        public int Id { get; set; }

        public string FirstPlayerId { get; set; }
        public UserDrawViewModel FirstPlayer { get; set; }

        public string SecondPlayerId { get; set; }
        public UserDrawViewModel SecondPlayer { get; set; }

        public string UmpireId { get; set; }

        public UserDrawViewModel Umpire { get; set; }

        public IEnumerable<SetViewModel> Sets { get; set; }

        public int FirstPlayerPoints { get; set; }

        public int SecondPlayerPoints { get; set; }

        public int FirstPlayerGames { get; set; }

        public int SecondPlayerGames { get; set; }

        public int FirstPlayerSets { get; set; }

        public int SecondPlayerSets { get; set; }

        public bool HasTieBreak { get; set; }

        public int FirstPlayerTieBreakPoints { get; set; }

        public int SecondPlayerTieBreakPoints { get; set; }
    }
}
