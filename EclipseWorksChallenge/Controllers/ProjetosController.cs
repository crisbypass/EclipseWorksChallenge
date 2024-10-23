using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
namespace EclipseWorksChallenge.Controllers
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// Listagem de Projetos - listar todos os projetos do usuário
    /// Visualização de Tarefas - visualizar todas as tarefas de um projeto específico
    /// Criação de Projetos - criar um novo projeto
    /// Criação de Tarefas - adicionar uma nova tarefa a um projeto
    /// Atualização de Tarefas - atualizar o status ou detalhes de uma tarefa
    /// Remoção de Tarefas - remover uma tarefa de um projeto
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetosController(IProjetoService projetoService) : ControllerBase
    {
        private readonly IProjetoService _projetoService = projetoService;

        [HttpGet($"{nameof(Listar)}/{{page=1}}")]
        public async Task<IActionResult> Listar(int page, InputProjetoDto inputUsuarioDto)
        {
            var (HasPreviousPage, HasNextPage, ProjetoDtos) = 
                await _projetoService.ListarProjetosAsync(inputUsuarioDto, page);

            return Ok(new { HasPreviousPage, HasNextPage, Items = ProjetoDtos });
        }

        [HttpPost($"{nameof(Criar)}")]
        public async Task<IActionResult> Criar(InputProjetoDto inputProjetoDto)
        {
            var project = await _projetoService.CriarProjetoAsync(inputProjetoDto);

            return CreatedAtAction(nameof(Criar), project);
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
        public async Task<IActionResult> Excluir(int projetoId)
        {
            var (sucesso, projetoDto, mensagem) = await _projetoService.ExcluirProjetoAsync(projetoId);

            if (sucesso)
            {
                return Ok(projetoDto);
            }
            // Há uma discussão sobre o tipo de código retornado aqui.
            // O fato de um projeto não poder ser excluído, pode não implicar 
            // em questões de validação necessariamente. Nesse caso poderíamos adotar o status 404.
            if (!sucesso && mensagem != null)
            {
                return Problem(mensagem, statusCode: StatusCodes.Status422UnprocessableEntity);
            }

            return NotFound();
        }
    }
}
