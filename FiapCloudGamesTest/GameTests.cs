using Core.Entities.InsertEntities;
using Core.Entities.UpdateEntity;
using Core.Entities;
using Core.Services;
using FiapCloudGames.Controllers;
using Infrastructure.Logs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Core.Entities.GetEntities;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesTest
{
    public class GameTests
    {
        private readonly Mock<IGameService> _gameServiceMock;
        private readonly Mock<ILogger<Games>> _innerLoggerMock;
        private readonly Mock<ICorrelationIdGenerator> _correlationIdMock;
        private readonly GameController _controller;

        public GameTests()
        {
            _gameServiceMock = new Mock<IGameService>();

            _innerLoggerMock = new Mock<ILogger<Games>>();
            _correlationIdMock = new Mock<ICorrelationIdGenerator>();
            _correlationIdMock.Setup(c => c.Get()).Returns("test-correlation-id");

            var baseLogger = new BaseLogger<Games>(_innerLoggerMock.Object, _correlationIdMock.Object);

            _controller = new GameController(_gameServiceMock.Object, baseLogger);
        }

        [Fact]
        public void Cadastrar_DeveRetornarOk_QuandoCadastroBemSucedido()
        {
            // Arrange
            var game = new GameInsert
            {
                Nome = "GTA",
                Genero = "Ação",
                Descricao = "Jogo de ação",
                Preco = 199.99m,
                Desenvolvedora = "Rockstar",
                DataLancamento = new DateTime(2013, 9, 17)
            };

            // Act
            var resultado = _controller.Cadastrar(game);

            // Assert
            Assert.IsType<OkResult>(resultado);
        }

        [Fact]
        public void Cadastrar_DeveRetornarBadRequest_QuandoLancarExcecao()
        {
            // Arrange
            var game = new GameInsert { Nome = "GTA" };
            _gameServiceMock.Setup(x => x.Cadastrar(It.IsAny<GameInsert>()))
                            .Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Cadastrar(game);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact]
        public void ObterTodos_DeveRetornarOk_ComListaDeJogos()
        {
            // Arrange
            var jogos = new List<GameRetorno> { new GameRetorno { Id = 1, Nome = "GTA" } };
            _gameServiceMock.Setup(x => x.ObterTodos()).Returns(jogos);

            // Act
            var resultado = _controller.ObterTodos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(jogos, okResult.Value);
        }

        [Fact]
        public void ObterTodos_DeveRetornarBadRequest_QuandoLancarExcecao()
        {
            // Arrange
            _gameServiceMock.Setup(x => x.ObterTodos()).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.ObterTodos();

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact]
        public void ObterId_DeveRetornarOk_QuandoEncontrado()
        {
            // Arrange
            var jogo = new GameRetorno { Id = 1, Nome = "GTA" };
            _gameServiceMock.Setup(x => x.ObterId(1)).Returns(jogo);

            // Act
            var resultado = _controller.ObterId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(jogo, okResult.Value);
        }

        [Fact]
        public void ObterId_DeveRetornarBadRequest_QuandoLancarExcecao()
        {
            // Arrange
            _gameServiceMock.Setup(x => x.ObterId(1)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.ObterId(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact]
        public void Alterar_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            var gameUpdate = new GameUpdate
            {
                Id = 1,
                Nome = "GTA V",
                Genero = "Ação",
                Descricao = "Atualizado",
                Preco = 250.00m,
                Desenvolvedora = "Rockstar",
                DataLancamento = new DateTime(2013, 9, 17)
            };

            // Act
            var resultado = _controller.Alterar(gameUpdate);

            // Assert
            Assert.IsType<OkResult>(resultado);
        }

        [Fact]
        public void Alterar_DeveRetornarBadRequest_QuandoLancarExcecao()
        {
            // Arrange
            _gameServiceMock.Setup(x => x.Alterar(It.IsAny<GameUpdate>()))
                            .Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Alterar(new GameUpdate());

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact]
        public void Apagar_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            int id = 1;

            // Act
            var resultado = _controller.Apagar(id);

            // Assert
            Assert.IsType<OkResult>(resultado);
        }

        [Fact]
        public void Apagar_DeveRetornarBadRequest_QuandoLancarExcecao()
        {
            // Arrange
            _gameServiceMock.Setup(x => x.Apagar(It.IsAny<int>())).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Apagar(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado);
        }
    }
}
