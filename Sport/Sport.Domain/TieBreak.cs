using System.Collections.Generic;

namespace Sport.Domain
{
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
