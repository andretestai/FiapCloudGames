using AutoMapper;
using Core.Entities;
using Core.Entities.GetEntities;
using Core.Entities.InsertEntities;
using Core.Entities.UpdateEntity;
using Core.Repository;
using Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IMapper _mapper;

        public GameService(IGameRepository gameRepository, IMapper mapper)
        {
            _gameRepository = gameRepository;
            _mapper = mapper;
        }

        public void Alterar(GameUpdate gameUpdate)
        {
            var game = _gameRepository.ObterId(gameUpdate.Id);

            game = _mapper.Map(gameUpdate, game);

            _gameRepository.Alterar(game);

        }

        public void Apagar(int id)
        {
            _gameRepository.Deletar(id);
        }

        public void Cadastrar(GameInsert gameInsert)
        {
            var game = _mapper.Map<Games>(gameInsert);

            _gameRepository.Cadastrar(game);
        }

        public GameRetorno ObterId(int id)
        {
            var game = _gameRepository.ObterId(id);

            var gamRetorno = new GameRetorno()
            {
                Id = game.Id,
                Nome = game.Nome,
                Descricao = game.Descricao,
                Genero = game.Genero,
                Desenvolvedora = game.Desenvolvedora,
                DataLancamento = game.DataLancamento
            };

            return gamRetorno;
        }

        public IList<GameRetorno> ObterTodos()
        {
            var gameRetorno = new List<GameRetorno>();
            var games = _gameRepository.ObterTodos();

            foreach(var game in games)
            {
                gameRetorno.Add(new GameRetorno()
                {
                    Id = game.Id,
                    Nome = game.Nome,
                    Descricao = game.Descricao,
                    Genero = game.Genero,
                    Desenvolvedora = game.Desenvolvedora,
                    DataLancamento = game.DataLancamento
                });
            }

            return gameRetorno;
        }
    }
}
