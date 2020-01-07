using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sport.Domain
{
    public class Result
    {
        public Result()
        {
            this.Sets = new HashSet<Set>();
        }

        [Key]
        public int Id { get; set; }

        public int FirstPlayerTieBreakPoints { get; set; }

        public int SecondPlayerTieBreakPoints { get; set; }

        public bool IsTieBreak { get; set; }


        public ICollection<Set> Sets { get; set; }

        public int MatchId { get; set; }

        public Match Match { get; set; }
    }

}
