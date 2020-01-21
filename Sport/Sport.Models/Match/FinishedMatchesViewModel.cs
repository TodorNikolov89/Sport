namespace Sport.ViewModels.Match
{
    using System.Collections.Generic;
    using User;
    using Set;

    public class FinishedMatchesViewModel
    {
        public int Id { get; set; }

        public string FirstPlayerId { get; set; }
        public UserViewModel FirstPlayer { get; set; }

        public string SecondPlayerId { get; set; }

        public UserViewModel SecondPlayer { get; set; }
        
        public ICollection<SetViewModel> Sets { get; set; }

        public int FirstPlayerSets { get; set; }

        public int SecondPlayerSets { get; set; }

        public bool IsActive { get; set; }

        public bool IsFinished { get; set; }
    }
}
