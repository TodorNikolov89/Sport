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
    using System.Linq;
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

            CreateMap<Match, LiveResultViewModel>()
                 .ForMember(dest => dest.FirstPlayerPoints, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().Games.ToList().LastOrDefault().Points.ToList().LastOrDefault().FirstPlayerPoints))
                 .ForMember(dest => dest.SecondPlayerPoints, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().Games.ToList().LastOrDefault().Points.ToList().LastOrDefault().SecondPlayerPoints))
                .ForMember(dest => dest.FirstPlayerGames, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().FirstPlayerGames))
                .ForMember(dest => dest.SecondPlayerGames, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().SecondPlayerGames))
                .ForMember(dest => dest.FirstPlayerSets, opt => opt.MapFrom(src => src.FirstPlayerSets))
                .ForMember(dest => dest.SecondPlayerSets, opt => opt.MapFrom(src => src.SecondPlayerSets))
                .ForMember(dest => dest.HasTieBreak, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().HasTieBreak))
                .ForMember(dest => dest.FirstPlayerTieBreakPoints, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().TieBreak.TieBreakPoints.ToList().LastOrDefault().FirstPlayerPoint))
                .ForMember(dest => dest.SecondPlayerTieBreakPoints, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().TieBreak.TieBreakPoints.ToList().LastOrDefault().SecondPlayerpoint));

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
