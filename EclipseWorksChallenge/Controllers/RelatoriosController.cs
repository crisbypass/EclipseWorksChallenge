using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorksChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "GerentePolicy")]
    public class RelatoriosController : ControllerBase
    {
        
        [HttpGet($"{nameof(ListarDesempenho)}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListarDesempenho()
        {
            return await Task.FromResult(Ok());
        }
    }
}
