using EclipseWorksChallenge.MyModels;
using EclipseWorksChallenge.MySecurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EclipseWorksChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegurancaController(IMyJwtSigningManager myJwtSigningManager) : ControllerBase
    {
        private readonly IMyJwtSigningManager _myJwtSigningManager = myJwtSigningManager;

        [HttpGet(nameof(RecuperarJwkJson))]
        public async Task<string> RecuperarJwkJson()
        {
            return await Task.FromResult(_myJwtSigningManager.CreateJwkJson());
        }
        [HttpPost(nameof(RecuperarToken))]
        public async Task<IActionResult> RecuperarToken(UsuarioModel usuarioModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return await Task.FromResult(Ok(
                _myJwtSigningManager.FetchToken(
                    usuarioModel.Nome,
                    Enum.GetName(usuarioModel.Funcao)!)));
        }
        [Authorize]
        [HttpGet(nameof(VerificarInfoUsuario))]
        public async Task<IActionResult> VerificarInfoUsuario()
        {
            var userName = User?.Identity?.Name;
            var role = User?.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .FirstOrDefault()?.Value;

            return await Task.FromResult(Ok($"Olá, {role} {userName}!"));
        }
    }
}
