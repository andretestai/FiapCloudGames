using Core.Entities;
using Core.Entities.InsertEntities;
using Core.Entities.UpdateEntity;
using Core.Services;
using Infrastructure.Logs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using Services.Services.Validator;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace FiapCloudGames.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly BaseLogger<User> _logger;
        private readonly UserValidator<UserInsert> _validator;

        public UserController(IUserService userService,
            BaseLogger<User> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Cadastra um novo usuário no sistema.
        /// </summary>
        /// <param name="user">Objeto <see cref="UserInsert"/> contendo os dados do usuário a ser cadastrado.</param>
        /// <returns>
        /// Retorna um <see cref="IActionResult"/> que representa o resultado da operação:
        /// <list type="bullet">
        /// <item><description><see cref="OkResult"/> (200): Cadastro realizado com sucesso.</description></item>
        /// <item><description><see cref="BadRequestObjectResult"/> (400): Erro ao processar o cadastro.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Exemplo do Json a ser enviado:
        /// 
        /// POST /User
        /// ```json
        /// {
        ///   "nome": "Andre Testai",
        ///   "email": "andre@email.com",
        ///   "password": "senha12345",
        ///   "cpf": "00000000000",
        ///   "dataNascimento": "2003-06-02",
        ///   "role": "User"
        /// }
        /// ```
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public IActionResult Cadastrar(UserInsert user)
        {
            try
            {
                _logger.LogInformation("Iniciando cadastro do Usuario");

                var validator = new UserValidator();

                var result = validator.Validate(user);

                if(result.IsValid == false)
                {
                    _logger.LogError($"Erro ao cadastrar o usuario, formato de senha ou email invalido");

                    return BadRequest("Erro com formato da senha ou do email");
                }

                _userService.Cadastrar(user);

                _logger.LogInformation("Cadastro Efetuado com sucesso");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao cadastrar o usuario {ex.Message}");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Retorna a lista de todos os usuários cadastrados. Apenas administradores têm acesso.
        /// </summary>
        /// <returns>
        /// Um <see cref="IActionResult"/> contendo a lista de usuários:
        /// <list type="bullet">
        /// <item><description><see cref="OkObjectResult"/> (200): Lista de usuários retornada com sucesso.</description></item>
        /// <item><description><see cref="BadRequestObjectResult"/> (400): Ocorreu um erro ao buscar os usuários.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Exemplo de resposta:
        /// 
        /// ```json
        /// [
        ///   {
        ///     "id": 7,
        ///     "dataCriacao": "0001-01-01T00:00:00",
        ///     "nome": "Elden Ring",
        ///     "genero": "RPG de Ação",
        ///     "descricao": "Um vasto mundo de fantasia sombria com combate desafiador e uma história coescrita por George R. R. Martin.",
        ///     "preco": 0,
        ///     "desenvolvedora": "FromSoftware",
        ///     "dataLancamento": "2022-02-25T00:00:00",
        ///     "userGames": null
        ///   }
        /// ]
        /// ```
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult ObterTodos()
        {
            try
            {
                _logger.LogInformation("Buscando usuarios");

                var retorno = _userService.ObterTodos();

                _logger.LogInformation("Usuarios encontrados com sucesso");

                return Ok(retorno);

            }
            catch(Exception ex)
            {
                _logger.LogError($"Erro ao encontrar todos usuarios {ex.Message}");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Retorna os dados de um usuário (ou recurso) com base no ID informado. Acesso restrito a administradores.
        /// </summary>
        /// <param name="id">Identificador único do usuário.</param>
        /// <returns>
        /// Um <see cref="IActionResult"/> contendo os dados do usuário:
        /// <list type="bullet">
        /// <item><description><see cref="OkObjectResult"/> (200): Usuário encontrado com sucesso.</description></item>
        /// <item><description><see cref="BadRequestObjectResult"/> (400): Erro ao buscar o usuário.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Exemplo de resposta:
        /// 
        /// ```json
        /// {
        ///   "id": 7,
        ///   "dataCriacao": "0001-01-01T00:00:00",
        ///   "nome": "Elden Ring",
        ///   "genero": "RPG de Ação",
        ///   "descricao": "Um vasto mundo de fantasia sombria com combate desafiador e uma história coescrita por George R. R. Martin.",
        ///   "preco": 0,
        ///   "desenvolvedora": "FromSoftware",
        ///   "dataLancamento": "2022-02-25T00:00:00",
        ///   "userGames": null
        /// }
        /// ```
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id:int}")]
        [Authorize(Policy = "Admin")]
        public IActionResult ObterId([FromRoute] int id)
        {
            try
            {
                _logger.LogInformation("Buscando usuario");

                var retorno = _userService.ObterId(id);

                _logger.LogInformation("Usuario encontrado com sucesso");

                return Ok(retorno);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Erro ao encontrar usuario {ex.Message}");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Altera os dados de um usuário existente. Acesso restrito a administradores.
        /// </summary>
        /// <param name="user">Objeto <see cref="UserUpdate"/> contendo os dados atualizados do usuário.</param>
        /// <returns>
        /// Um <see cref="IActionResult"/> representando o resultado da operação:
        /// <list type="bullet">
        /// <item><description><see cref="OkResult"/> (200): Usuário alterado com sucesso.</description></item>
        /// <item><description><see cref="BadRequestObjectResult"/> (400): Erro ao alterar o usuário.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        /// ```json
        /// {
        ///   "id": 1,
        ///   "nome": "Andre Testai",
        ///   "email": "andre@email.com",
        ///   "password": "novaSenha123",
        ///   "cpf": "00000000000",
        ///   "dataNascimento": "2003-06-02T00:00:00",
        ///   "token": "abc123token",
        ///   "role": "Admin"
        /// }
        /// ```
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        [Authorize(Policy = "Admin")]
        public IActionResult Alterar(UserUpdate user)
        {
            try
            {
                _logger.LogInformation("Alterando usuario");

                _userService.Alterar(user);

                _logger.LogInformation("Usuario alterado com succeso");

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Erro ao alterar usuario {ex.Message}");

                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Remove um usuário do sistema com base no ID informado. Acesso restrito a administradores.
        /// </summary>
        /// <param name="id">Identificador único do usuário a ser deletado.</param>
        /// <returns>
        /// Um <see cref="IActionResult"/> representando o resultado da operação:
        /// <list type="bullet">
        /// <item><description><see cref="OkResult"/> (200): Usuário deletado com sucesso.</description></item>
        /// <item><description><see cref="BadRequestObjectResult"/> (400): Erro ao deletar o usuário.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        /// DELETE /User/1
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Admin")]
        public IActionResult Deletar(int id)
        {
            try
            {
                _logger.LogInformation("Deletando usuario");

                _userService.Deletar(id);

                _logger.LogInformation("Usuario deletado com sucesso");

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Erro ao alterar usuario {ex.Message}");

                return BadRequest(ex);
            }
        }
        /// <summary>
        /// Retorna os jogos associados a um usuário específico com base no ID informado.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <returns>
        /// Um <see cref="IActionResult"/> com os dados do usuário e seus jogos:
        /// <list type="bullet">
        /// <item><description><see cref="OkObjectResult"/> (200): Dados encontrados com sucesso.</description></item>
        /// <item><description><see cref="BadRequestObjectResult"/> (400): Erro ao buscar os dados.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Exemplo de resposta:
        /// 
        /// ```json
        /// {
        ///   "id": 5,
        ///   "nome": "Andre Testai Muchao",
        ///   "email": null,
        ///   "cpf": null,
        ///   "dataNascimento": null,
        ///   "token": null,
        ///   "role": null,
        ///   "userGames": [
        ///     {
        ///       "id": 0,
        ///       "dataCriacao": "0001-01-01T00:00:00",
        ///       "userId": 0,
        ///       "gameId": 1,
        ///       "game": {
        ///         "id": 1,
        ///         "dataCriacao": "0001-01-01T00:00:00",
        ///         "nome": "GTA",
        ///         "genero": null,
        ///         "descricao": "Joguinho de dirigi e atira",
        ///         "preco": 0,
        ///         "desenvolvedora": null,
        ///         "dataLancamento": "2013-09-17T00:00:00",
        ///         "userGames": null
        ///       },
        ///       "user": null
        ///     }
        ///   ]
        /// }
        /// ```
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("BuscarJogosUser/{id:int}")]
        [Authorize]
        public IActionResult BuscarJogosUser([FromRoute] int id)
        {
            try
            {
                _logger.LogInformation("Buscando jogos do usuario");

                var retorno = _userService.BuscarJogosUser(id);

                _logger.LogInformation("Jogos do usuario encontrado com sucesso");

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao encontrar usuario {ex.Message}");

                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Realiza o login do usuário com base no email, senha e nome fornecidos.
        /// </summary>
        /// <param name="email">Email do usuário.</param>
        /// <param name="password">Senha do usuário.</param>
        /// <param name="nome">Nome do usuário.</param>
        /// <returns>
        /// Retorna status 200 (OK) se o login for bem-sucedido.
        /// Retorna status 400 (BadRequest) se o login falhar ou ocorrer algum erro.
        /// </returns>
        /// <remarks>
        /// Exemplo de chamada HTTP:
        /// GET /api/controllername/Logar?email=usuario@email.com&password=senha123&nome=Andre
        /// </remarks>
        [HttpGet("Logar")]
        public IActionResult Logar(string email, string password, string nome)
        {
            try
            {
                _logger.LogInformation("Iniciando login");

                var logado = _userService.Logar(email, password, nome);

                if (logado)
                {
                    _logger.LogInformation("Logado com sucesso");

                    return Ok();
                }
                else
                {
                    _logger.LogError("Erro ao logar");

                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao encontrar logar {ex.Message}");

                return BadRequest(ex);
            }
        }

        [HttpGet("Ambiente")]
        public IActionResult Ambiente() =>
            Ok("Teste da API");

        [HttpPost("bulk")]
        public async Task<IActionResult> CadastrarEmMassa(List<UserInsert> users)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = "rabbitmq"
                };

                await using var connection = await factory.CreateConnectionAsync();
                await using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: "fiap-events",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                foreach (var user in users)
                {
                    var validator = new UserValidator();
                    var result = validator.Validate(user);

                    if (!result.IsValid)
                    {
                        Console.WriteLine($"❌ Usuário inválido: {user.Email}");
                        continue;
                    }

                    var body = Encoding.UTF8.GetBytes(
                        JsonSerializer.Serialize(user)
                    );

                    await channel.BasicPublishAsync(
                        exchange: "",
                        routingKey: "fiap-events",
                        mandatory: false,
                        body: new ReadOnlyMemory<byte>(body)
                    );

                }

                return Ok("Usuários enviados para processamento assíncrono");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
