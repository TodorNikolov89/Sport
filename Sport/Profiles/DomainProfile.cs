namespace Sport.Profiles
{
    using Domain;
    using ViewModels.Point;
    using ViewModels.Match;
    using ViewModels.Player;
    using ViewModels.Tournament;
    using ViewModels.User;
    using ViewModels.TieBreakPoint;
    using ViewModels.Game;
    using ViewModels.Set;
    using ViewModels.TieBreak;

    using AutoMapper;
    using System.Collections.Generic;

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


            CreateMap<Point, PointViewModel>().ReverseMap();
            CreateMap<Game, GameViewModel>().ReverseMap();
            CreateMap<Set, SetViewModel>().ReverseMap();



            CreateMap<Match, MatchesViewModel>().ReverseMap();
            CreateMap<TieBreakPoint, TieBreakPointViewModel>().ReverseMap();
            CreateMap<TieBreak, TieBreakViewModel>().ReverseMap();
        }
    }
}
