using Application.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorksChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {


        [HttpGet($"{nameof(ListarTarefas)}/{{projetoId}}")]
        public async Task<IActionResult> ListarTarefas(int projetoId)
        {


            return Ok(); //lista de tarefaDto
        }

        [HttpPost($"{nameof(Criar)}/{{projectId}}")] //Precisamos do identificador do projeto.
        public async Task<IActionResult> Criar(int projectId, InputTarefaDto tarefaModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(Criar), new { }); //tarefaDto
        }

        [HttpPut($"{nameof(Editar)}/{{tarefaId}}")]
        public async Task<IActionResult> Editar(int tarefaId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(); //tarefaDto
        }

        [HttpDelete($"{nameof(Excluir)}/{{tarefaId}}")]
        public async Task<IActionResult> Excluir(int tarefaId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (tarefaId != 0)
            {
                return NotFound();
            }

            return Ok(); //tarefaDto
        }

        [HttpPost($"{nameof(Comentar)}/{{projectId}}")] //Precisamos do identificador do projeto.
        public async Task<IActionResult> Comentar(int tarefaId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(Criar), new { }); //tarefaDto
        }
    }
}
