namespace Sport.ViewModels.Match
{
    using User;
    using Set;

    using System.Collections.Generic;

    public class MatchScoreViewModel
    {
        public int Id { get; set; }

        public string FirstPlayerId { get; set; }
        public UserDrawViewModel FirstPlayer { get; set; }

        public string SecondPlayerId { get; set; }
        public UserDrawViewModel SecondPlayer { get; set; }

        public string UmpireId { get; set; }
        public UserDrawViewModel Umpire { get; set; }

        public ICollection<SetViewModel> Sets { get; set; }

        public int FirstPlayerSets { get; set; }

        public int SecondPlayerSets { get; set; }

        public bool IsActive { get; set; }

        public bool IsFinished { get; set; }   
     


    }
}
