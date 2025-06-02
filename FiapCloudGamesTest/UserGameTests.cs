using Core.Entities.InsertEntities;
using Core.Entities.UpdateEntity;
using Core.Entities;
using Core.Services;
using FiapCloudGames.Controllers;
using Infrastructure.Logs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Core.Entities.GetEntities;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesTest
{
    public class UserGameTests
    {
        public class UserGameControllerTests
        {
            private readonly Mock<IUserGameService> _userGameServiceMock;
            private readonly Mock<ILogger<UserGames>> _innerLoggerMock;
            private readonly Mock<ICorrelationIdGenerator> _correlationIdMock;
            private readonly UserGameController _controller;

            public UserGameControllerTests()
            {
                _userGameServiceMock = new Mock<IUserGameService>();

                _innerLoggerMock = new Mock<ILogger<UserGames>>();
                _correlationIdMock = new Mock<ICorrelationIdGenerator>();
                _correlationIdMock.Setup(c => c.Get()).Returns("test-correlation-id");

                var baseLogger = new BaseLogger<UserGames>(_innerLoggerMock.Object, _correlationIdMock.Object);

                _controller = new UserGameController(_userGameServiceMock.Object, baseLogger);
            }


            [Fact]
            public void Cadastrar_DeveRetornarOk_QuandoSucesso()
            {
                var userGame = new UserGameInsert { UserId = 1, GameId = 1 };

                var resultado = _controller.Cadastrar(userGame);

                Assert.IsType<OkResult>(resultado);
            }

            [Fact]
            public void Cadastrar_DeveRetornarBadRequest_QuandoExcecao()
            {
                _userGameServiceMock.Setup(s => s.Cadastrar(It.IsAny<UserGameInsert>()))
                                    .Throws(new Exception("Erro"));

                var resultado = _controller.Cadastrar(new UserGameInsert());

                Assert.IsType<BadRequestObjectResult>(resultado);
            }

            [Fact]
            public void ObterTodos_DeveRetornarOk_ComLista()
            {
                var lista = new List<UserGameRetorno>
            {
                new UserGameRetorno { Id = 1, UserId = 1, GameId = 1 }
            };
                _userGameServiceMock.Setup(s => s.ObterTodos()).Returns(lista);

                var resultado = _controller.ObterTodos();

                var okResult = Assert.IsType<OkObjectResult>(resultado);
                Assert.Equal(lista, okResult.Value);
            }

            [Fact]
            public void ObterTodos_DeveRetornarBadRequest_QuandoExcecao()
            {
                _userGameServiceMock.Setup(s => s.ObterTodos())
                                    .Throws(new Exception("Erro"));

                var resultado = _controller.ObterTodos();

                Assert.IsType<BadRequestObjectResult>(resultado);
            }

            [Fact]
            public void ObterId_DeveRetornarOk_QuandoEncontrado()
            {
                var userGame = new UserGameRetorno { Id = 1, UserId = 1, GameId = 1 };
                _userGameServiceMock.Setup(s => s.ObterId(1)).Returns(userGame);

                var resultado = _controller.ObterId(1);

                var okResult = Assert.IsType<OkObjectResult>(resultado);
                Assert.Equal(userGame, okResult.Value);
            }

            [Fact]
            public void ObterId_DeveRetornarBadRequest_QuandoExcecao()
            {
                _userGameServiceMock.Setup(s => s.ObterId(It.IsAny<int>()))
                                    .Throws(new Exception("Erro"));

                var resultado = _controller.ObterId(1);

                Assert.IsType<BadRequestObjectResult>(resultado);
            }

            [Fact]
            public void Alterar_DeveRetornarOk_QuandoSucesso()
            {
                var update = new UserGameUpdate { Id = 1, UserId = 1, GameId = 1 };

                var resultado = _controller.Alterar(update);

                Assert.IsType<OkResult>(resultado);
            }

            [Fact]
            public void Alterar_DeveRetornarBadRequest_QuandoExcecao()
            {
                _userGameServiceMock.Setup(s => s.Alterar(It.IsAny<UserGameUpdate>()))
                                    .Throws(new Exception("Erro"));

                var resultado = _controller.Alterar(new UserGameUpdate());

                Assert.IsType<BadRequestObjectResult>(resultado);
            }

            [Fact]
            public void Deletar_DeveRetornarOk_QuandoSucesso()
            {
                var resultado = _controller.Deletar(1);

                Assert.IsType<OkResult>(resultado);
            }

            [Fact]
            public void Deletar_DeveRetornarBadRequest_QuandoExcecao()
            {
                _userGameServiceMock.Setup(s => s.Deletar(It.IsAny<int>()))
                                    .Throws(new Exception("Erro"));

                var resultado = _controller.Deletar(1);

                Assert.IsType<BadRequestObjectResult>(resultado);
            }
        }
    }
}
