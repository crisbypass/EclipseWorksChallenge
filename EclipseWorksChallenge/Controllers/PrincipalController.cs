using EclipseWorksChallenge.MyData.MyEntities;
using Microsoft.AspNetCore.Mvc;
namespace EclipseWorksChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrincipalController : ControllerBase
    {
        //[ApiExplorerSettings(IgnoreApi = true)]
        //[HttpGet]
        //public async Task Get()
        //{
        //    await Response.WriteAsync(await _mainService.VersaoDllHtmlText());
        //}

        //[Produces(MediaTypeNames.Application.Json, "application/problem+json")]
        //[ProducesResponseType(typeof(WorkingKeyBitResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        //[HttpPost("ObtemWorkingKeyBit")]

        //Listagem de Projetos - listar todos os projetos do usuário
        //Visualização de Tarefas - visualizar todas as tarefas de um projeto específico
        //Criação de Projetos - criar um novo projeto
        //Criação de Tarefas - adicionar uma nova tarefa a um projeto
        //Atualização de Tarefas - atualizar o status ou detalhes de uma tarefa
        //Remoção de Tarefas - remover uma tarefa de um projeto

        [HttpGet]
        public async Task<IActionResult> ListarProjetos()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(); //lista de projetoDto
        }
        [HttpGet]
        public async Task<IActionResult> ListarTarefas()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(); //lista de tarefaDto
        }
        [HttpPost]
        public async Task<IActionResult> CriarProjeto()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(CriarProjeto), new { }); //projetoDto
        }
        // Ao menos na descrição do desafio, não é mencionado algo sobre a possibilidade de edição de projetos.
        //[HttpPut($"{nameof(EditarProjeto)}/{{id}}")]
        //public async Task<IActionResult> EditarProjeto()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    return Ok(new{}); //projetoDto
        //}

        [HttpDelete($"{nameof(ExcluirProjeto)}/{{id}}")] //Precisamos do identificador do projeto.
        public async Task<IActionResult> ExcluirProjeto(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != 0)
            {
                return NotFound();
            }

            return Ok(new { }); //tarefaDto
        }

        [HttpPost("")] //Precisamos do identificador do projeto.
        public async Task<IActionResult> CriarTarefa()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(CriarTarefa), new { }); //tarefaDto
        }

        [HttpPut($"{nameof(EditarTarefa)}/{{id}}")]
        public async Task<IActionResult> EditarTarefa()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { }); //tarefaDto
        }

        [HttpDelete($"{nameof(ExcluirTarefa)}/{{id}}")] //Precisamos do identificador do projeto.
        public async Task<IActionResult> ExcluirTarefa(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != 0)
            {
                return NotFound();
            }

            return Ok(new { }); //tarefaDto
        }
    }
}
