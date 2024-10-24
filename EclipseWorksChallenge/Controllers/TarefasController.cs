using Application.Dtos;
using Application.Interfaces;
using Application.Messages;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace EclipseWorksChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController(ITarefaService tarefaService) : ControllerBase
    {
        private readonly ITarefaService _tarefaService = tarefaService;

        [HttpGet($"{nameof(Listar)}/{{projetoId}}/{{page}}")]
        [ProducesResponseType<IEnumerable<TarefaDto>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ProblemDetails),
            StatusCodes.Status500InternalServerError, MediaTypeNames.Application.ProblemJson)]
        [SwaggerOperation("Lista as tarefas relacionadas a um determinado projeto.",
            @"Será necessário informar o identificador do projeto para efetuar tarefas relacionadas ao mesmo.
        Recupere o identificador a partir da listagem de projetos, de um determinado usuário.
        O uso de paginação foi aplicado para cada 10 registros, por padrão. Basta informar para qual página ir:
        anterior(page-1), ou próxima(page+1), caso possível a navegação. Cheque HasPreviousPage e HasNextPage.")]
        public async Task<IActionResult> Listar(int projetoId, int page)
        {
            var (HasPreviousPage, HasNextPage, TarefaDtos, IsNotFound) =
                await _tarefaService.BuscarTarefasAsync(projetoId, page);

            if (IsNotFound)
            {
                return NotFound();
            }

            return Ok(new { HasPreviousPage, HasNextPage, Items = TarefaDtos });
        }

        [HttpPost($"{nameof(Criar)}/{{projectId}}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<TarefaDto>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ValidationProblemDetails),
            StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ProblemDetails),
            StatusCodes.Status500InternalServerError, MediaTypeNames.Application.ProblemJson)]
        [SwaggerOperation("Cria uma nova tarefa relacionada a um projeto.",
            @"Será possível informar o mesmo usuário fictício, usado na criação do projeto, ou
            um outro, à sua escolha. Use o identificador do projeto para efetuar tarefas relacionadas ao mesmo.")]
        public async Task<IActionResult> Criar(int projectId, InputTarefaDto inputTarefaDto)
        {
            (bool Sucesso, TarefaDto TarefaDto, string Mensagem) =
                await _tarefaService.CriarTarefaAsync(projectId, inputTarefaDto);

            if (Sucesso)
            {
                return Created(nameof(Listar), TarefaDto);
            }

            if (!Sucesso && Mensagem != Mensagens.ProjetoNaoEncontrado)
            {
                return Problem(Mensagem, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NotFound();
        }

        [HttpPut($"{nameof(Editar)}/{{tarefaId}}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<TarefaDto>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ValidationProblemDetails),
            StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ProblemDetails),
            StatusCodes.Status500InternalServerError, MediaTypeNames.Application.ProblemJson)]
        [SwaggerOperation("Atualiza as informações de tarefa relacionada a um projeto.",
            @"Será possível informar o mesmo usuário fictício, usado na criação do projeto, ou
            um outro, à sua escolha. Use o identificador do projeto para efetuar tarefas relacionadas ao mesmo.")]
        public async Task<IActionResult> Editar(int tarefaId, EditTarefaDto editTarefaDto)
        {
            (bool Sucesso, TarefaDto TarefaDto, string Mensagem) =
                await _tarefaService.AtualizarTarefaAsync(tarefaId, editTarefaDto);

            if (Sucesso)
            {
                return Ok(TarefaDto);
            }

            return Problem(Mensagem, statusCode: StatusCodes.Status500InternalServerError);
        }

        [HttpDelete($"{nameof(Excluir)}/{{tarefaId}}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Exclui uma tarefa.",
            @"Será necessário recuperar o identificador da tarefa, através do endpoint 'Listar'.
            Informe o mesmo usuario fictício, como valor na rota, usado na criação do projeto, 
            ou um outro, à sua escolha.")]
        public async Task<IActionResult> Excluir(int tarefaId)
        {
            var (sucesso, projetoDto) = await _tarefaService.ExcluirTarefaAsync(tarefaId);

            if (sucesso)
            {
                return Ok(projetoDto);
            }

            return NotFound();
        }

        [HttpPost($"{nameof(Comentar)}/{{tarefaId}}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<ComentarioDto>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ValidationProblemDetails),
            StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ProblemDetails),
            StatusCodes.Status500InternalServerError, MediaTypeNames.Application.ProblemJson)]
        [SwaggerOperation("Cria um comentário relacionado a uma tarefa.",
            @"Será possível informar o mesmo usuário fictício, usado na criação do projeto, ou
            um outro, à sua escolha. Use o identificador da tarefa para efetuar alterações relacionadas à mesma.")]
        public async Task<IActionResult> Comentar(int tarefaId, InputComentarioDto inputComentarioDto)
        {
            (bool Sucesso, ComentarioDto ComentarioDto, string Mensagem) =
                await _tarefaService.ComentarAsync(tarefaId, inputComentarioDto);

            if (Sucesso)
            {
                return Created(nameof(Listar), ComentarioDto);
            }

            return Problem(Mensagem, statusCode: StatusCodes.Status400BadRequest);
        }
    }
}
