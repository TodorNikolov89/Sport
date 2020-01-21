namespace Sport.ViewModels.Point
{
    using Game;
    public class PointViewModel
    {
        public int Id { get; set; }

        public int FirsPlayerPoints { get; set; }

        public int SecondPlayerPoints { get; set; }

        public int GameId { get; set; }
        public GameViewModel Game { get; set; }
    }
}
