using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sport.Domain
{

    public class Point
    {
        public Point()
        {
          
        }

        public int Id { get; set; }

      
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
