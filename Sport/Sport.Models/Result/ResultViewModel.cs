namespace Sport.ViewModels.Result
{

    public class ResultViewModel
    {

        public int Id { get; set; }

        public int FirstPlayerPoints { get; set; }

        public int SecondPlayerPoints { get; set; }

        public int FirstPlayerGames { get; set; }

        public int SecondPlayerGames { get; set; }

        public int FirstPlayerSets { get; set; }

        public int SecondPlayerSets { get; set; }

        public int FirstPlayerTieBreakPoints { get; set; }

        public int SecondPlayerTieBreakPoints { get; set; }

        public bool IsTieBreak { get; set; }

    }

}
