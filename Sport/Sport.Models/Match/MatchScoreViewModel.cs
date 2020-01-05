namespace Sport.ViewModels.Match
{
    using Domain;
    public class MatchScoreViewModel
    {
        public int Id { get; set; }

        public string FirstPlayerId { get; set; }
        public User FirstPlayer { get; set; }

        public string SecondPlayerId { get; set; }
        public User SecondPlayer { get; set; }

        public string UmpireId { get; set; }

        public User Umpire { get; set; }

        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        public string FirstPlayerPoints { get; set; }

        public int? FirstPlayerGames { get; set; }

        public int? FirstPlayerSets { get; set; }

        public int? FirstPlayerTieBreakPoints { get; set; }

        public string SecondPlayerPoints { get; set; }

        public int? SecondPlayerGames { get; set; }

        public int? SecondPlayerSets { get; set; }

        public int? SecondPlayerTieBreakPoints { get; set; }
    }
}
