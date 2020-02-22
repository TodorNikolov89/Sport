namespace Sport.ViewModels.Match
{
    using Set;
    using User;
    using Game;

    using System.Collections.Generic;

    public class FinishedMatchDetaisViewModel
    {
        public int Id { get; set; }

        public string FirstPlayerId { get; set; }
        public UserDrawViewModel FirstPlayer { get; set; }

        public string SecondPlayerId { get; set; }
        public UserDrawViewModel SecondPlayer { get; set; }

        public string UmpireId { get; set; }
        public UserDrawViewModel Umpire { get; set; }

        public IEnumerable<SetViewModel> Sets { get; set; }

        public IEnumerable<GameViewModel> Games { get; set; }

        public int FirstPlayerSets { get; set; }

        public int SecondPlayerSets { get; set; }


        public bool HasTieBreak { get; set; }

        public bool IsFinished { get; set; }
    }
}
