namespace Sport.ViewModels.Match
{
    using Result;
    using Sport.ViewModels.User;

    public class MatchScoreViewModel
    {
        public int Id { get; set; }

        public string FirstPlayerId { get; set; }
        public UserDrawViewModel FirstPlayer { get; set; }

        public string SecondPlayerId { get; set; }
        public UserDrawViewModel SecondPlayer { get; set; }

        public string UmpireId { get; set; }
        public UserDrawViewModel Umpire { get; set; }

        public int MatchResultId { get; set; }
        public ResultViewModel MatchResult { get; set; }

        public bool IsActive { get; set; }

        public bool IsFinished { get; set; }
    }
}
