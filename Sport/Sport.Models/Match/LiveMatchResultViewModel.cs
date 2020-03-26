namespace Sport.ViewModels.Match
{
    using Sport.ViewModels.User;

    public class LiveMatchResultViewModel
    {
        public int Id { get; set; }

        public string FirstPlayerId { get; set; }
        public UserDrawViewModel FirstPlayer { get; set; }

        public string SecondPlayerId { get; set; }
        public UserDrawViewModel SecondPlayer { get; set; }

        public int FirstPlayerPoints { get; set; }

        public int SecondPlayerPoints { get; set; }

        public int FirstPlayerGames { get; set; }

        public int SecondPlayerGames { get; set; }

        public int FirstPlayerSets { get; set; }

        public int SecondPlayerSets { get; set; }

        public int FirstPlayerTieBreakPoints { get; set; }

        public int SecondPlayerTieBreakPoints { get; set; }


    }
}
