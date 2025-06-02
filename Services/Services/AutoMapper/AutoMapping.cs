using AutoMapper;
using Core.Entities;
using Core.Entities.GetEntities;
using Core.Entities.InsertEntities;
using Core.Entities.UpdateEntity;

namespace Services.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            ServiceToInfraGames();
            ServiceToInfraUser();
            ServiceToInfraUserGames();
        }

        private void ServiceToInfraGames()
        {
            CreateMap<GameInsert, Games>();
            CreateMap<Games, GameRetorno>();
            CreateMap<GameUpdate, Games>()
                .ForAllMembers(opts => opts.Condition(
                    (gameUpdt, game, prop) => prop != null));
        }

        private void ServiceToInfraUser()
        {
            CreateMap<UserInsert, User>();
            CreateMap<User, UserRetorno>();
            CreateMap<UserUpdate, User>()
                .ForAllMembers(opts => opts.Condition(
                    (userUpdt, user, prop) => prop != null));
        }

        private void ServiceToInfraUserGames()
        {
            CreateMap<UserGameInsert, UserGames>();
            CreateMap<UserGames, UserGameRetorno>();
            CreateMap<UserGameUpdate, UserGames>()
                .ForAllMembers(opts => opts.Condition(
                    (userGamesUpdt, userGames, prop) => prop != null));
        }
    }
}
