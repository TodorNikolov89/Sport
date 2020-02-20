using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sport.Domain
{
    public class Point
    {
        public Point()
        {
            FirstPlayerPoints = 0;
            SecondPlayerPoints = 0;
        }

        public int Id { get; set; }

        public int FirstPlayerPoints { get; set; }

        public int SecondPlayerPoints { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
