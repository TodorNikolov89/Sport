namespace Sport.ViewModels.TieBreakPoint
{
    using TieBreak;

    public class TieBreakPointViewModel
    {
        public int Id { get; set; }

        public int FirstPlayerPoint { get; set; }

        public int SecondPlayerpoint { get; set; }
        public int TieBreakId { get; set; }
        public TieBreakViewModel TieBreak { get; set; }
    }
}
