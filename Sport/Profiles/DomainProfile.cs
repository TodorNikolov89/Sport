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
            CreateMap<Tournament, TournamentFormModel>().ReverseMap();
            CreateMap<Tournament, AllTournamentsViewModel>().ReverseMap();
            CreateMap<TournamentFormModel, Tournament>().ReverseMap();

            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<User, AllPlayersViewModel>().ReverseMap();
            CreateMap<User, PlayerViewModel>().ReverseMap();
            CreateMap<User, UserDrawViewModel>().ReverseMap();

            CreateMap<Match, AllMatchesViewModel>().ReverseMap();
            CreateMap<Match, MatchScoreViewModel>().ReverseMap();
            CreateMap<Match, MatchesViewModel>().ReverseMap();
            CreateMap<Match, LiveMatchesViewModel>()
                .ForMember(dest => dest.FirstPlayerPoints, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().Games.ToList().LastOrDefault().Points.ToList().LastOrDefault().FirstPlayerPoints))
                .ForMember(dest => dest.SecondPlayerPoints, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().Games.ToList().LastOrDefault().Points.ToList().LastOrDefault().SecondPlayerPoints))
                .ForMember(dest => dest.FirstPlayerGames, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().FirstPlayerGames))
                .ForMember(dest => dest.SecondPlayerGames, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().SecondPlayerGames))
                .ForMember(dest => dest.FirstPlayerSets, opt => opt.MapFrom(src => src.FirstPlayerSets))
                .ForMember(dest => dest.SecondPlayerSets, opt => opt.MapFrom(src => src.SecondPlayerSets))
                .ForMember(dest => dest.HasTieBreak, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().HasTieBreak))
                .ForMember(dest => dest.FirstPlayerTieBreakPoints, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().TieBreak.TieBreakPoints.ToList().LastOrDefault().FirstPlayerPoint))
                .ForMember(dest => dest.SecondPlayerTieBreakPoints, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().TieBreak.TieBreakPoints.ToList().LastOrDefault().SecondPlayerPoint))
                .ForMember(dest => dest.Sets, opt => opt.MapFrom(src => src.Sets));

            CreateMap<Match, FinishedMatchDetaisViewModel>()
               .ForMember(dest => dest.FirstPlayerSets, opt => opt.MapFrom(src => src.FirstPlayerSets))
               .ForMember(dest => dest.SecondPlayerSets, opt => opt.MapFrom(src => src.SecondPlayerSets))
               .ForMember(dest => dest.HasTieBreak, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().HasTieBreak))
               .ForMember(dest => dest.Sets, opt => opt.MapFrom(src => src.Sets));



            CreateMap<Match, LiveResultViewModel>()
                .ForMember(dest => dest.FirstPlayerPoints, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().Games.ToList().LastOrDefault().Points.ToList().LastOrDefault().FirstPlayerPoints))
                .ForMember(dest => dest.SecondPlayerPoints, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().Games.ToList().LastOrDefault().Points.ToList().LastOrDefault().SecondPlayerPoints))
                .ForMember(dest => dest.FirstPlayerGames, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().FirstPlayerGames))
                .ForMember(dest => dest.SecondPlayerGames, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().SecondPlayerGames))
                .ForMember(dest => dest.FirstPlayerSets, opt => opt.MapFrom(src => src.FirstPlayerSets))
                .ForMember(dest => dest.SecondPlayerSets, opt => opt.MapFrom(src => src.SecondPlayerSets))
                .ForMember(dest => dest.HasTieBreak, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().HasTieBreak))
                .ForMember(dest => dest.FirstPlayerTieBreakPoints, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().TieBreak.TieBreakPoints.ToList().LastOrDefault().FirstPlayerPoint))
                .ForMember(dest => dest.SecondPlayerTieBreakPoints, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().TieBreak.TieBreakPoints.ToList().LastOrDefault().SecondPlayerPoint))
                .ForMember(dest => dest.Sets, opt => opt.MapFrom(src => src.Sets));



            CreateMap<Match, FinishedMatchesViewModel>()
                 .ForMember(dest => dest.FirstPlayerPoints, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().Games.ToList().LastOrDefault().Points.ToList().LastOrDefault().FirstPlayerPoints))
                .ForMember(dest => dest.SecondPlayerPoints, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().Games.ToList().LastOrDefault().Points.ToList().LastOrDefault().SecondPlayerPoints))
                .ForMember(dest => dest.FirstPlayerGames, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().FirstPlayerGames))
                .ForMember(dest => dest.SecondPlayerGames, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().SecondPlayerGames))
                .ForMember(dest => dest.FirstPlayerSets, opt => opt.MapFrom(src => src.FirstPlayerSets))
                .ForMember(dest => dest.SecondPlayerSets, opt => opt.MapFrom(src => src.SecondPlayerSets))
                .ForMember(dest => dest.HasTieBreak, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().HasTieBreak))
                .ForMember(dest => dest.FirstPlayerTieBreakPoints, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().TieBreak.TieBreakPoints.ToList().LastOrDefault().FirstPlayerPoint))
                .ForMember(dest => dest.SecondPlayerTieBreakPoints, opt => opt.MapFrom(src => src.Sets.ToList().LastOrDefault().TieBreak.TieBreakPoints.ToList().LastOrDefault().SecondPlayerPoint))
                .ForMember(dest => dest.Sets, opt => opt.MapFrom(src => src.Sets));


            //.ForMember(dest => dest.FirstPlayer, opt => opt.MapFrom(src => src.FirstPlayer))
            //.ForMember(dest => dest.SecondPlayer, opt => opt.MapFrom(src => src.SecondPlayer))
            //.ForMember(dest => dest.Sets, opt => opt.MapFrom(src => src.Sets));



            CreateMap<Point, PointViewModel>().ReverseMap();
            CreateMap<Game, GameViewModel>().ReverseMap();
            CreateMap<Set, SetViewModel>().ReverseMap();
            CreateMap<TieBreakPoint, TieBreakPointViewModel>().ReverseMap();
            CreateMap<TieBreak, TieBreakViewModel>().ReverseMap();
        }
    }
}
