namespace Sport.Domain
{
    using System.Collections.Generic;

    public class TieBreak
    {
        public TieBreak()
        {
            this.TieBreakPoints = new HashSet<TieBreakPoint>();
        }

        public int Id { get; set; }

        public ICollection<TieBreakPoint> TieBreakPoints { get; set; }

        public int SetId { get; set; }

        public Set Set { get; set; }
    }
}
