using AutoMapper;
using Core.Entities;
using Core.Entities.GetEntities;
using Core.Entities.InsertEntities;
using Core.Entities.UpdateEntity;
using Core.Repository;
using Core.Services;
using Services.Services.Criptografia;
using Services.Services.JWT;

namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly JWTGenerator _jwtGenerator;
        private readonly CriptografarSenha _criptografarSenha;

        public UserService(IUserRepository userRepository,
            IMapper mapper,
            JWTGenerator jwtGerenator,
            CriptografarSenha criptografarSenha)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtGenerator = jwtGerenator;
            _criptografarSenha = criptografarSenha;
        }

        public void Alterar(UserUpdate userUpdate)
        {
            var user = _userRepository.ObterId(userUpdate.Id);

            user = _mapper.Map(userUpdate, user);

            _userRepository.Alterar(user);
        }

        public UserRetorno BuscarJogosUser(int id)
        {
            var user = _userRepository.ObterId(id);

            var userRetorno = new UserRetorno()
            {
                Id = user.Id,
                Nome = user.Nome,
                UserGames = user.UserGames!.Select(g => new UserGameRetorno()
                {
                    GameId = g.Game!.Id,
                    Game = new GameRetorno()
                    {
                        Id = g.Game.Id,
                        Nome = g.Game.Nome,
                        DataLancamento = g.Game.DataLancamento,
                        Descricao = g.Game.Descricao
                    }
                    
                }).ToList()
            };

            return userRetorno;
        }

        public void Cadastrar(UserInsert userInsert)
        {
            var user = _mapper.Map<User>(userInsert);

            user.Token = _jwtGenerator.GenerateToken(user.Nome, user.Role, user.CPF);

            user.Password = _criptografarSenha.EncryptPassword(user.Password);

            _userRepository.Cadastrar(user);
        }

        public void Deletar(int id)
        {
            _userRepository.Deletar(id);
        }

        public bool Logar(string email, string password, string nome)
        {
            password = _criptografarSenha.EncryptPassword(password);

            var logado = _userRepository.Logar(email, password, nome);

            return logado;
        }

        public UserRetorno ObterId(int id)
        {
            var user = _userRepository.ObterId(id);

            var userRetorno = new UserRetorno()
            {
                Id = user.Id,
                Nome = user.Nome,
                CPF = user.CPF,
                Email = user.Email,
                DataNascimento = user.DataNascimento,
                Role = user.Role,
                UserGames = null,
            };

            return userRetorno;
        }

        public IList<UserRetorno> ObterTodos()
        {
            var userRetorno = new List<UserRetorno>();
            var users = _userRepository.ObterTodos();

            foreach(var user in users)
            {
                userRetorno.Add(new UserRetorno()
                {
                    Id = user.Id,
                    Nome = user.Nome,
                    Email = user.Email,
                    CPF = user.CPF,
                    DataNascimento = user.DataNascimento,
                    Token = user.Token,
                    Role = user.Role,
                    UserGames = user.UserGames!
                    .Where(userGame => userGame.UserId == user.Id)
                        .Select(userGames => new UserGameRetorno()
                        {
                            UserId = userGames.UserId,
                            GameId = userGames.GameId,
                            Game = new GameRetorno()
                            {
                                Id = userGames.Game!.Id,
                                Nome = userGames.Game.Nome,
                                Desenvolvedora = userGames.Game.Desenvolvedora,
                                Genero =  userGames.Game.Genero,
                                Preco = userGames.Game.Preco,
                                Descricao = userGames.Game.Descricao,
                                DataLancamento = userGames.Game.DataLancamento
                            }
                        }).ToList()
                });
            }

            return userRetorno;
        }
    }
}
