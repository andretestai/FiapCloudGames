using AutoMapper;
using Core.Entities;
using Core.Entities.GetEntities;
using Core.Entities.InsertEntities;
using Core.Entities.UpdateEntity;
using Core.Repository;
using Core.Services;
using Infrastructure.DataAccess.Repository;

namespace Services.Services
{
    public class UserGameService : IUserGameService
    {
        private readonly IUserGameRepository _userGameRepository;
        private readonly IMapper _mapper;

        public UserGameService(
            IUserGameRepository userGameRepository,
            IMapper mapper)
        {
            _userGameRepository = userGameRepository;
            _mapper = mapper;
        }

        public void Alterar(UserGameUpdate userGameUpdate)
        {
            var userGame = _userGameRepository.ObterId(userGameUpdate.Id);

            userGame = _mapper.Map(userGameUpdate, userGame);

            _userGameRepository.Alterar(userGame);
        }

        public void Cadastrar(UserGameInsert userGameInsert)
        {
            var userGame = _mapper.Map<UserGames>(userGameInsert);

            _userGameRepository.Cadastrar(userGame);
        }

        public void Deletar(int id)
        {
            _userGameRepository.Deletar(id);
        }

        public UserGameRetorno ObterId(int id)
        {
            var userGame = _userGameRepository.ObterId(id);

            var userGameRetorno = new UserGameRetorno()
            {
                Id = userGame.Id,
                GameId = userGame.GameId,
                UserId = userGame.UserId,
                Game = new GameRetorno()
                {
                    Id = userGame.Game!.Id,
                    Nome = userGame.Game.Nome,
                    DataLancamento = userGame.Game.DataLancamento,
                    Descricao = userGame.Game.Descricao,
                    Preco = userGame.Game.Preco,
                    Desenvolvedora = userGame.Game.Desenvolvedora,
                    Genero = userGame.Game.Genero,
                },

                User = new UserRetorno()
                {
                    Id = userGame.User!.Id,
                    Nome = userGame.User.Nome,
                    Email = userGame.User.Email,
                    CPF = userGame.User.CPF,
                }
            };

            return userGameRetorno;
        }

        public IList<UserGameRetorno> ObterTodos()
        {
            var userGames = _userGameRepository.ObterTodos();
            var userGameRetornos = new List<UserGameRetorno>();

            foreach (var userGame in userGames)
            {
                var userGameRetorno = new UserGameRetorno()
                {
                    Id = userGame.Id,
                    UserId = userGame.UserId,
                    GameId = userGame.GameId,
                    Game = new GameRetorno()
                    {
                        Id = userGame.Game!.Id,
                        Nome = userGame.Game.Nome,
                        Desenvolvedora = userGame.Game.Desenvolvedora,
                        Genero = userGame.Game.Genero,
                        Preco = userGame.Game.Preco,
                        Descricao = userGame.Game.Descricao,
                        DataLancamento = userGame.Game.DataLancamento,
                        UserGames = null,
                    },
                    User = new UserRetorno()
                    {
                        Id = userGame.User!.Id,
                        Email = userGame.User.Email,
                        CPF = userGame.User.CPF,
                        UserGames= null,
                    }
                };

                userGameRetornos.Add(userGameRetorno);
            }

            return userGameRetornos;
        }

    }
}
