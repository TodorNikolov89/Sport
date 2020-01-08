namespace Sport.ViewModels.Game
{
    using Point;
    using User;
    using Set;


    using System.Collections.Generic;
    using System.Linq;


    public class GameViewModel
    {

        public int Id { get; set; }

        public int FirsPlayerPoints { get; set; }

        public int SecondPlayerPoints { get; set; }

        public ICollection<PointViewModel> Points { get; set; }

        public string PlayerId { get; set; }
        public UserDrawViewModel Player { get; set; }

        public int SetId { get; set; }
        public SetViewModel Set { get; set; }

        public bool IsFirstPoint { get; set; }
        public bool IsGameFinished { get; set; }
    }
}
