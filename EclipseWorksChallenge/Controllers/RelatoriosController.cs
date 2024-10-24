using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace EclipseWorksChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "GerentePolicy")]
    public class RelatoriosController(ITarefaService tarefaService) : ControllerBase
    {
        private readonly ITarefaService _tarefaService = tarefaService;

        [HttpGet($"{nameof(ListarDesempenho)}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<IEnumerable<RelatorioDesempenhoDto>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ProblemDetails),
            StatusCodes.Status500InternalServerError, MediaTypeNames.Application.ProblemJson)]
        [SwaggerOperation("Lista o número médio de tarefas concluídas por usuário nos últimos 30 dias.",
            @"Será necessário informar um JWT para acessar o endpoint atual. Crie um usuário qualquer,
            com a funçao contemplada pela política de segurança, no endpoint de segurança: RecuperarToken.
            Precisa ser informada a função: Gerente. Não é necessário um código de acesso, ou senha.
            Insira esse token e confirme na aba para segurança(icone do cadeado do endpoint atual).")]
        public async Task<IActionResult> ListarDesempenho()
        {
            var (_, DesempenhoDtos) = await _tarefaService.ListarDesempenhoAsync();
            
            return Ok(DesempenhoDtos);
        }
    }
}
