namespace Sport.ViewModels.Match
{
    public class UmpireResultViewModel
    {
        public int Id { get; set; }

        public int FirstPlayerSets { get; set; }

        public int SecondPlayerSets { get; set; }

        public int FirstPlayerGames { get; set; }

        public int SecondPlayerGames{ get; set; }


        public int FirstPlayerPoints{ get; set; }

        public int SecondPlayerPoints{ get; set; }

        public bool HasTieBreak { get; set; }

    }
}
