namespace Sport.Profiles
{
    using AutoMapper;
    using Sport.Domain;
    using Sport.ViewModels.Match;
    using Sport.ViewModels.Player;
    using Sport.ViewModels.Tournament;
    using Sport.ViewModels.User;

    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<TournamentFormModel, Tournament>().ReverseMap();
            CreateMap<Tournament, TournamentFormModel>().ReverseMap();
            CreateMap<User, AllPlayersViewModel>().ReverseMap();
            CreateMap<Tournament, AllTournamentsViewModel>();
            CreateMap<User, UserViewModel>();
            CreateMap<User, PlayerViewModel>().ReverseMap();
            CreateMap<Match, MatchesViewModel>().ReverseMap();
            CreateMap<User, UserDrawViewModel>().ReverseMap();
        }
    }
}
