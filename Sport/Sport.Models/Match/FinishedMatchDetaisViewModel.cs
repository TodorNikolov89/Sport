namespace Sport.ViewModels.Match
{
    using Set;
    using User;
    using Game;
    using Tournament;

    using System.Collections.Generic;

    public class FinishedMatchDetaisViewModel
    {
        public int Id { get; set; }

        public string FirstPlayerId { get; set; }
        public UserViewModel FirstPlayer { get; set; }

        public string SecondPlayerId { get; set; }
        public UserViewModel SecondPlayer { get; set; }

        public string UmpireId { get; set; }
        public UserViewModel Umpire { get; set; }

        public int TournamentId { get; set; }
        public AllTournamentsViewModel Tournament { get; set; }

        public ICollection<SetViewModel> Sets { get; set; }

        public int FirstPlayerSets { get; set; }

        public int SecondPlayerSets { get; set; }

        public bool IsActive { get; set; }

        public bool IsFinished { get; set; }
    }
}
