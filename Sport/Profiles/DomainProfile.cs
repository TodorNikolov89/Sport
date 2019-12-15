﻿namespace Sport.Profiles
{
    using AutoMapper;
    using Sport.Domain;
    using Sport.ViewModels.Player;
    using Sport.ViewModels.Tournament;

    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<TournamentFormModel, Tournament>().ReverseMap();
            CreateMap<Tournament, TournamentFormModel>().ReverseMap();
            CreateMap<User, AllPlayersViewModel>().ReverseMap();

        }
    }
}
