using Application.Dtos;
using Application.Messages;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
namespace EclipseWorksChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetosController(IProjetoService projetoService) : ControllerBase
    {
        private readonly IProjetoService _projetoService = projetoService;

        [HttpPost($"{nameof(Listar)}/{{page}}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<IEnumerable<ProjetoDto>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ValidationProblemDetails),
            StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ProblemDetails),
            StatusCodes.Status500InternalServerError, MediaTypeNames.Application.ProblemJson)]
        [SwaggerOperation("Lista os projetos de um determinado Usuário.",
            @"Será necessário informar o mesmo usuário fictício, usado na criação do projeto.
        Use o identificador do projeto para efetuar tarefas relacionadas ao mesmo. O uso de paginação
        foi aplicado para cada 10 registros por padrão. Basta informar para qual página ir: anterior(page-1), ou
        próxima(page+1), caso possível a navegação. Cheque HasPreviousPage e HasNextPage.")]
        public async Task<IActionResult> Listar(int page, InputProjetoDto inputUsuarioDto)
        {
            var (HasPreviousPage, HasNextPage, ProjetoDtos, IsNotFound) =
                await _projetoService.ListarProjetosAsync(inputUsuarioDto, page);

            if (IsNotFound) {
                return NotFound();
            }

            return Ok(new { HasPreviousPage, HasNextPage, Items = ProjetoDtos });
        }

        [HttpPost($"{nameof(Criar)}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<ProjetoDto>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ValidationProblemDetails),
            StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ProblemDetails),
            StatusCodes.Status500InternalServerError, MediaTypeNames.Application.ProblemJson)]
        [SwaggerOperation("Cria um projeto.",
            @"Será necessário informar um usuário fictício. Use esse mesmo nome em caso de necessitar
        acessar um endpoint protegido, ao gerar um JWT.")]
        public async Task<IActionResult> Criar(InputProjetoDto inputProjetoDto)
        {
            (bool Sucesso, ProjetoDto ProjectDto, string Mensagem) =
                await _projetoService.CriarProjetoAsync(inputProjetoDto);

            if (Sucesso)
            {
                return Created(nameof(Listar), ProjectDto);
            }

            return Problem(Mensagem, statusCode: StatusCodes.Status500InternalServerError);
        }

        // Ao menos na descrição do desafio, não é mencionado algo sobre a possibilidade de edição de projetos.
        //[HttpPut($"{nameof(EditarProjeto)}/{{id}}")]
        //public async Task<IActionResult> EditarProjeto()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    return Ok(); //projetoDto
        //}

        [HttpDelete($"{nameof(Excluir)}/{{projetoId}}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails),
            StatusCodes.Status422UnprocessableEntity, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Exclui um projeto.", 
            @"Será necessário recuperar o identificador do Projeto, através do endpoint 'Listar'.")]
        public async Task<IActionResult> Excluir(int projetoId)
        {
            var (sucesso, projetoDto, mensagem) = await _projetoService.ExcluirProjetoAsync(projetoId);

            if (sucesso)
            {
                return Ok(projetoDto);
            }
            // Há uma discussão sobre o tipo de código retornado aqui.
            // O fato de um projeto não poder ser excluído no momento, pode não implicar 
            // em questões de validação necessariamente. Caso contrário, poderíamos adotar o status 404.
            if (!sucesso && mensagem == Mensagens.ErroExclusaoProjeto)
            {
                return Problem(mensagem, statusCode: StatusCodes.Status422UnprocessableEntity);
            }

            return NotFound();
        }
    }
}
