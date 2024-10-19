using EclipseWorksChallenge.MyDtos;
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
        //public async Task<WorkingKeyBitResponse> ObtemWorkingKeyBit([FromBody] WorkingKeyBitRequest request)
        //{
        //    //throw new Exception("Teste");
        //    //estabelecimento:"000000000019061", bit61:"001011SE3DESv1.00"
        //    return await _mainService.ObtemWorkingKeyBitAsync(request);
        //}
        [HttpPost]
        //[ProducesResponseType<ProjetoDto>(200)]
        public async Task<IActionResult> CreateProject(ProjetoDto projetoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Created("", projetoDto);
        }
    }
}
