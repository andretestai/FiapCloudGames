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
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly BaseLogger<Games> _logger;


        public GameController(IGameService gameService,
            BaseLogger<Games> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }
        /// <summary>
        /// Cadastra um novo jogo no sistema.
        /// </summary>
        /// <param name="game">Objeto contendo os dados do jogo a ser cadastrado.</param>
        /// <returns>
        /// Um <see cref="IActionResult"/> representando o resultado da operação:
        /// <list type="bullet">
        /// <item><description><see cref="OkResult"/> (200): Jogo cadastrado com sucesso.</description></item>
        /// <item><description><see cref="BadRequestObjectResult"/> (400): Erro ao cadastrar o jogo.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        /// ```json
        /// {
        ///   "nome": "GTA",
        ///   "genero": "Ação",
        ///   "descricao": "Joguinho de dirigi e atira",
        ///   "preco": 199.99,
        ///   "desenvolvedora": "Rockstar Games",
        ///   "dataLancamento": "2013-09-17T00:00:00"
        /// }
        /// ```
        /// </remarks>        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize(Policy ="Admin")]
        public IActionResult Cadastrar (GameInsert game)
        {
            try
            {
                _logger.LogInformation("Iniciando cadastro do game");

                _gameService.Cadastrar(game);

                _logger.LogInformation("Game cadastrado com sucesso");

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Erro ao cadastrar o usuario {ex.Message}");

                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Retorna todos os jogos cadastrados no sistema.
        /// </summary>
        /// <returns>
        /// Um <see cref="IActionResult"/> contendo a lista de jogos:
        /// <list type="bullet">
        /// <item><description><see cref="OkObjectResult"/> (200): Jogos encontrados com sucesso.</description></item>
        /// <item><description><see cref="BadRequestObjectResult"/> (400): Erro ao buscar os jogos.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Exemplo de resposta:
        /// 
        /// ```json
        /// [
        ///   {
        ///     "id": 1,
        ///     "dataCriacao": "2023-01-01T00:00:00",
        ///     "nome": "GTA",
        ///     "genero": "Ação",
        ///     "descricao": "Joguinho de dirigi e atira",
        ///     "preco": 199.99,
        ///     "desenvolvedora": "Rockstar Games",
        ///     "dataLancamento": "2013-09-17T00:00:00",
        ///     "userGames": null
        ///   }
        /// ]
        /// ```
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        [Authorize]
        public IActionResult ObterTodos()
        {
            try
            {
                _logger.LogInformation("Buscando jogos");

                var retorno = _gameService.ObterTodos();

                _logger.LogInformation("Jogos encontrados com sucesso");

                return Ok(retorno);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Erro ao encontrar todos jogos {ex.Message}");

                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Retorna um jogo específico com base no ID informado.
        /// </summary>
        /// <param name="id">ID do jogo.</param>
        /// <returns>
        /// Um <see cref="IActionResult"/> contendo os dados do jogo:
        /// <list type="bullet">
        /// <item><description><see cref="OkObjectResult"/> (200): Jogo encontrado com sucesso.</description></item>
        /// <item><description><see cref="BadRequestObjectResult"/> (400): Erro ao buscar o jogo.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Exemplo de resposta:
        /// 
        /// ```json
        /// {
        ///   "id": 1,
        ///   "dataCriacao": "2023-01-01T00:00:00",
        ///   "nome": "GTA",
        ///   "genero": "Ação",
        ///   "descricao": "Joguinho de dirigi e atira",
        ///   "preco": 199.99,
        ///   "desenvolvedora": "Rockstar Games",
        ///   "dataLancamento": "2013-09-17T00:00:00",
        ///   "userGames": null
        /// }
        /// ```
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id:int}")]
        [Authorize]
        public IActionResult ObterId([FromRoute] int id)
        {
            try
            {
                _logger.LogInformation("Buscando jogo");

                var retorno = _gameService.ObterId(id);

                _logger.LogInformation("Jogo encontrado com sucesso");

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao encontrar jogo {ex.Message}");

                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Altera os dados de um jogo existente.
        /// </summary>
        /// <param name="game">Objeto contendo os dados atualizados do jogo.</param>
        /// <returns>
        /// Um <see cref="IActionResult"/> representando o resultado da operação:
        /// <list type="bullet">
        /// <item><description><see cref="OkResult"/> (200): Jogo alterado com sucesso.</description></item>
        /// <item><description><see cref="BadRequestObjectResult"/> (400): Erro ao alterar o jogo.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        /// ```json
        /// {
        ///   "id": 1,
        ///   "nome": "GTA V",
        ///   "genero": "Ação",
        ///   "descricao": "Atualização da descrição do jogo",
        ///   "preco": 249.99,
        ///   "desenvolvedora": "Rockstar Games",
        ///   "dataLancamento": "2013-09-17T00:00:00"
        /// }
        /// ```
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        [Authorize(Policy = "Admin")]
        public IActionResult Alterar(GameUpdate game)
        {
            try
            {
                _logger.LogInformation("Alterando jogo");

                _gameService.Alterar(game);

                _logger.LogInformation("Jogo alterado com succeso");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao alterar jogo {ex.Message}");

                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Remove um jogo do sistema com base no ID informado.
        /// </summary>
        /// <param name="id">ID do jogo a ser deletado.</param>
        /// <returns>
        /// Um <see cref="IActionResult"/> representando o resultado da operação:
        /// <list type="bullet">
        /// <item><description><see cref="OkResult"/> (200): Jogo deletado com sucesso.</description></item>
        /// <item><description><see cref="BadRequestObjectResult"/> (400): Erro ao deletar o jogo.</description></item>
        /// </list>
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Admin")]
        public IActionResult Apagar([FromRoute] int id)
        {
            try
            {
                _logger.LogInformation("Deletando jogo");

                _gameService.Apagar(id);

                _logger.LogInformation("Jogo deletado com sucesso");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao deletar jogo {ex.Message}");

                return BadRequest(ex);
            }
        }
    }
}
