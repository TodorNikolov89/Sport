namespace Sport.ViewModels.TieBreak
{
    using Set;
    using TieBreakPoint;

    using System.Collections.Generic;

    public class TieBreakViewModel
    {
        public int Id { get; set; }

        public ICollection<TieBreakPointViewModel> TieBreakPoints { get; set; }

        public int SetId { get; set; }
        public SetViewModel Set { get; set; }
    }
}
