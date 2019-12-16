namespace Sport.Domain
{
    public class UserTournament
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
    }
}
