using Core.Entities;
using Core.Entities.InsertEntities;
using Core.Entities.UpdateEntity;
using Core.Services;
using Infrastructure.Logs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace FiapCloudGames.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UserGameController : ControllerBase
    {
        private readonly IUserGameService _userGameService;
        private readonly BaseLogger<UserGames> _logger;

        public UserGameController(IUserGameService userGameService,
            BaseLogger<UserGames> logger)
        {
            _userGameService = userGameService;
            _logger = logger;
        }

        /// <summary>
        /// Realiza o cadastro de um novo vínculo entre um usuário e um jogo.
        /// </summary>
        /// <param name="userGame">Objeto contendo os dados necessários para o cadastro do vínculo.</param>
        /// <remarks>
        /// Exemplo de JSON a ser enviado:
        /// 
        /// ```json
        /// {
        ///   "userId": 1,
        ///   "gameId": 5
        /// }
        /// ```
        /// </remarks>
        /// <returns>
        /// Um <see cref="IActionResult"/> representando o resultado da operação:
        /// <list type="bullet">
        /// <item><description><see cref="OkResult"/> (200): Cadastro efetuado com sucesso.</description></item>
        /// <item><description><see cref="BadRequestObjectResult"/> (400): Erro ao realizar o cadastro.</description></item>
        /// </list>
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Cadastrar(UserGameInsert userGame)
        {
            try
            {
                _logger.LogInformation("Iniciando cadastro do User Game");

                _userGameService.Cadastrar(userGame);

                _logger.LogInformation("Cadastro Efetuado com sucesso");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao cadastrar o user game {ex.Message}");
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// Retorna todos os vínculos entre usuários e jogos cadastrados no sistema.
        /// </summary>
        /// <returns>
        /// Um <see cref="IActionResult"/> contendo a lista de vínculos encontrados:
        /// <list type="bullet">
        /// <item><description><see cref="OkObjectResult"/> (200): Vínculos encontrados com sucesso.</description></item>
        /// <item><description><see cref="BadRequestObjectResult"/> (400): Erro ao buscar os vínculos.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Exemplo de resposta:
        /// 
        /// ```json
        /// [
        ///   {
        ///     "id": 5,
        ///     "dataCriacao": "0001-01-01T00:00:00",
        ///     "userId": 5,
        ///     "gameId": 1,
        ///     "game": {
        ///       "id": 1,
        ///       "dataCriacao": "0001-01-01T00:00:00",
        ///       "nome": "GTA",
        ///       "genero": "Mundo aberto",
        ///       "descricao": "Joguinho de dirigi e atira",
        ///       "preco": 100,
        ///       "desenvolvedora": "Rockstar",
        ///       "dataLancamento": "2013-09-17T00:00:00",
        ///       "userGames": null
        ///     },
        ///     "user": {
        ///       "id": 5,
        ///       "nome": null,
        ///       "email": "email@email.com",
        ///       "cpf": "78978978",
        ///       "dataNascimento": null,
        ///       "token": null,
        ///       "role": null,
        ///       "userGames": null
        ///     }
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
                _logger.LogInformation("Buscando user games");

                var retorno = _userGameService.ObterTodos();

                _logger.LogInformation("User games encontrados com sucesso");

                return Ok(retorno);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Erro ao encontrar todos user games {ex.Message}");

                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Retorna o vínculo entre usuário e jogo conforme o ID informado.
        /// </summary>
        /// <param name="id">ID do vínculo UserGame.</param>
        /// <returns>
        /// Um <see cref="IActionResult"/> contendo o vínculo encontrado:
        /// <list type="bullet">
        /// <item><description><see cref="OkObjectResult"/> (200): Vínculo encontrado com sucesso.</description></item>
        /// <item><description><see cref="BadRequestObjectResult"/> (400): Erro ao buscar o vínculo.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Exemplo de resposta:
        /// 
        /// ```json
        /// {
        ///   "id": 5,
        ///   "dataCriacao": "0001-01-01T00:00:00",
        ///   "userId": 5,
        ///   "gameId": 1,
        ///   "game": {
        ///     "id": 1,
        ///     "dataCriacao": "0001-01-01T00:00:00",
        ///     "nome": "GTA",
        ///     "genero": "Mundo aberto",
        ///     "descricao": "Joguinho de dirigi e atira",
        ///     "preco": 100,
        ///     "desenvolvedora": "Rockstar",
        ///     "dataLancamento": "2013-09-17T00:00:00",
        ///     "userGames": null
        ///   },
        ///   "user": {
        ///     "id": 5,
        ///     "nome": "Andre Testai Muchao",
        ///     "email": "email@email.com",
        ///     "cpf": "78978978",
        ///     "dataNascimento": null,
        ///     "token": null,
        ///     "role": null,
        ///     "userGames": null
        ///   }
        /// }
        /// ```
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id:int}")]
        public IActionResult ObterId([FromRoute] int id)
        {
            try
            {
                _logger.LogInformation("Buscando user game");

                var retorno = _userGameService.ObterId(id);

                _logger.LogInformation("User game encontrado com sucesso");

                return Ok(retorno);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Erro ao encontrar user game {ex.Message}");

                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Altera os dados de um vínculo UserGame existente.
        /// </summary>
        /// <param name="userGame">Objeto com os dados atualizados do vínculo UserGame.</param>
        /// <returns>
        /// Um <see cref="IActionResult"/> indicando o resultado da operação:
        /// <list type="bullet">
        /// <item><description><see cref="OkResult"/> (200): Alteração realizada com sucesso.</description></item>
        /// <item><description><see cref="BadRequestObjectResult"/> (400): Erro ao tentar alterar o vínculo.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Exemplo de JSON esperado no corpo da requisição:
        /// 
        /// ```json
        /// {
        ///   "id": 5,
        ///   "userId": 5,
        ///   "gameId": 1
        /// }
        /// ```
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        [Authorize(Policy = "Admin")]
        public IActionResult Alterar(UserGameUpdate userGame)
        {
            try
            {
                _logger.LogInformation("Buscando user game");

                _userGameService.Alterar(userGame);

                _logger.LogInformation("Buscando user game");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao alterar user game {ex.Message}");

                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Deleta um vínculo UserGame pelo ID.
        /// </summary>
        /// <param name="id">ID do UserGame a ser deletado.</param>
        /// <returns>
        /// Retorna status 200 (OK) em caso de sucesso ou 400 (BadRequest) em caso de erro.
        /// </returns>
        /// <remarks>
        /// Exemplo de chamada HTTP:
        /// DELETE /api/usergame/5
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Admin")]
        public IActionResult Deletar([FromRoute] int id)
        {
            try
            {
                _logger.LogInformation("Deletando user game");

                _userGameService.Deletar(id);

                _logger.LogInformation("User game deletado com sucesso");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao deletar user game {ex.Message}");

                return BadRequest(ex);
            }
        }
    }
}
