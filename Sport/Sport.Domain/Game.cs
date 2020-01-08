namespace Sport.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class Game
    {
        public Game()
        {
            this.Points = new HashSet<Point>();
            FirsPlayerPoints = 0;
            SecondPlayerPoints = 0;
        }

        [Key]
        public int Id { get; set; }

        public int FirsPlayerPoints { get; set; }

        public int SecondPlayerPoints { get; set; }

        public ICollection<Point> Points { get; set; }

        public Point LastPoint => this.Points.ToList().LastOrDefault();

        public string PlayerId { get; set; }
        public User Player { get; set; }

        public int SetId { get; set; }
        public Set Set { get; set; }

        public bool IsFirstPoint { get; set; }
        public bool IsGameFinished { get; set; }


    }
}
