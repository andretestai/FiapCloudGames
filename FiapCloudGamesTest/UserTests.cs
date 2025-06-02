using Core.Entities;
using Core.Entities.GetEntities;
using Core.Entities.InsertEntities;
using Core.Entities.UpdateEntity;
using Core.Services;
using FiapCloudGames.Controllers;
using Infrastructure.Logs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FiapCloudGamesTest
{
    public class UserTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<User>> _innerLoggerMock;
        private readonly Mock<ICorrelationIdGenerator> _correlationIdMock;
        private readonly UserController _controller;

        public UserTests()
        {
            _userServiceMock = new Mock<IUserService>();

            _innerLoggerMock = new Mock<ILogger<User>>();
            _correlationIdMock = new Mock<ICorrelationIdGenerator>();
            _correlationIdMock.Setup(c => c.Get()).Returns("test-correlation-id");

            var baseLogger = new BaseLogger<User>(_innerLoggerMock.Object, _correlationIdMock.Object);

            _controller = new UserController(_userServiceMock.Object, baseLogger);
        }

        [Fact]
        public void Cadastrar_DeveRetornarOk_QuandoDadosValidos()
        {
            // Arrange
            var user = new UserInsert
            {
                Nome = "Teste",
                Email = "teste@email.com",
                Password = "Senha12345$#",
                CPF = "00000000000",
                DataNascimento = DateTime.Parse("2000-01-01"),
                Role = "User"
            };

            // Act
            var resultado = _controller.Cadastrar(user);

            // Assert
            Assert.IsType<OkResult>(resultado);
        }

        [Fact]
        public void Cadastrar_DeveRetornarBadRequest_QuandoEmailOuSenhaInvalidos()
        {
            // Arrange
            var user = new UserInsert
            {
                Nome = "Teste",
                Email = "email_invalido",
                Password = "123", // senha fraca
                CPF = "00000000000",
                DataNascimento = DateTime.Parse("2000-01-01"),
                Role = "User"
            };

            // Act
            var resultado = _controller.Cadastrar(user);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact]
        public void ObterTodos_DeveRetornarOk_ComListaDeUsuarios()
        {
            // Arrange
            var usuarios = new List<UserRetorno> 
            { new UserRetorno 
            { 
                Id = 1, 
                Nome = "Andre" ,
                Email = "andre@email.com",
                CPF = "75395185252",
                DataNascimento = DateTime.Now.AddYears(-20),
                Role = "Admin"
            } };
            _userServiceMock.Setup(s => s.ObterTodos()).Returns(usuarios);

            // Act
            var resultado = _controller.ObterTodos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(usuarios, okResult.Value);
        }

        [Fact]
        public void ObterId_DeveRetornarOk_QuandoUsuarioEncontrado()
        {
            // Arrange
            var usuario = new UserRetorno
            {
                Id = 1,
                Nome = "Andre",
                Email = "andre@email.com",
                CPF = "75395185252",
                DataNascimento = DateTime.Now.AddYears(-20),
                Role = "Admin"
            };
            _userServiceMock.Setup(s => s.ObterId(1)).Returns(usuario);

            // Act
            var resultado = _controller.ObterId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(usuario, okResult.Value);
        }

        [Fact]
        public void Alterar_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            var userUpdate = new UserUpdate
            {
                Id = 1,
                Nome = "Atualizado",
                Email = "email@email.com",
                Password = "NovaSenha123",
                CPF = "00000000000",
                DataNascimento = DateTime.Today,
                Token = "token123",
                Role = "Admin"
            };

            // Act
            var resultado = _controller.Alterar(userUpdate);

            // Assert
            Assert.IsType<OkResult>(resultado);
        }

        [Fact]
        public void Deletar_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            int userId = 1;

            // Act
            var resultado = _controller.Deletar(userId);

            // Assert
            Assert.IsType<OkResult>(resultado);
        }

        [Fact]
        public void BuscarJogosUser_DeveRetornarOk_QuandoEncontrado()
        {
            // Arrange
            var user = new UserRetorno
            {
                Id = 5,
                Nome = "Andre",
                UserGames = new List<UserGameRetorno>
                {
                    new UserGameRetorno { GameId = 1 }
                }
            };

            _userServiceMock.Setup(s => s.BuscarJogosUser(5)).Returns(user);

            // Act
            var resultado = _controller.BuscarJogosUser(5);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(user, okResult.Value);
        }

        [Fact]
        public void Logar_DeveRetornarOk_QuandoLoginBemSucedido()
        {
            // Arrange
            _userServiceMock.Setup(s => s.Logar("email@email.com", "senha123", "Andre"))
                .Returns(true);

            // Act
            var resultado = _controller.Logar("email@email.com", "senha123", "Andre");

            // Assert
            Assert.IsType<OkResult>(resultado);
        }

        [Fact]
        public void Logar_DeveRetornarBadRequest_QuandoLoginFalha()
        {
            // Arrange
            _userServiceMock.Setup(s => s.Logar("email@email.com", "senha123", "Andre"))
                .Returns(false);

            // Act
            var resultado = _controller.Logar("email@email.com", "senha123", "Andre");

            // Assert
            Assert.IsType<BadRequestResult>(resultado);
        }
    }
}
