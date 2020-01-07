namespace Sport.Profiles
{
    using AutoMapper;
    using Domain;
    using ViewModels.Match;
    using ViewModels.Player;
    using ViewModels.Result;
    using ViewModels.Tournament;
    using ViewModels.User;

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
            CreateMap<Match, MatchScoreViewModel>().ReverseMap();
            CreateMap<User, UserDrawViewModel>().ReverseMap();
            CreateMap<Result, ResultViewModel>().ReverseMap();
        }
    }
}
