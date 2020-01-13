namespace Sport.Domain
{
    public class TieBreakPoint
    {
        public TieBreakPoint()
        {
            this.FirstPlayerPoint = 0;
            this.SecondPlayerpoint = 0;
        }

        public int Id { get; set; }

        public int FirstPlayerPoint { get; set; }

        public int SecondPlayerpoint { get; set; }

        public int TieBreakId { get; set; }
        public TieBreak TieBreak { get; set; }
    }
}
