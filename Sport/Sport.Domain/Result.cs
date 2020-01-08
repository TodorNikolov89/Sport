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


        public ICollection<Set> Sets { get; set; }


        public int MatchId { get; set; }

        public Match Match { get; set; }

        public int FirstPlayerSets { get; set; }

        public int SecondPlayerSets { get; set; }
    }

}
