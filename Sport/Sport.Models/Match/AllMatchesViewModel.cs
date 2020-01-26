namespace Sport.ViewModels.Match
{
    using User;
    using Tournament;

    public class AllMatchesViewModel
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
    }
}
