using Application.Interfaces;
using EclipseWorksChallenge.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using System.Security.Claims;

namespace EclipseWorksChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegurancaController(IMyJwtSigningManager myJwtSigningManager) : ControllerBase
    {
        private readonly IMyJwtSigningManager _myJwtSigningManager = myJwtSigningManager;

        //[HttpGet(nameof(RecuperarJwkJson))]
        //public async Task<string> RecuperarJwkJson()
        //{
        //    return await Task.FromResult(_myJwtSigningManager.CreateJwkJson());
        //}
        /// <summary>
        /// Recupere o token para usar em um endpoint protegido.
        /// </summary>
        /// <param name="usuarioModel">
        /// Dados usados para simular um usuário. Forneça um nome e sua função.
        /// </param>
        /// <returns>
        /// JWT(Ecdsa).
        /// </returns>
        /// <remarks>
        /// Não há necessidade de informar códigos de acesso(ou senhas).
        /// </remarks>
        
        [HttpPost(nameof(RecuperarToken))]
        [ProducesResponseType<BadRequest>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType<Ok>(StatusCodes.Status200OK, MediaTypeNames.Text.Plain)]
        [SwaggerOperation("Recupera um JWT, para uso em um endpoint protegido.",
            @"Fornece a opção de obter um JWT, para experimentos. Não é necessário
            um código de acesso. Informe apenas um nome de usuário e sua função, para
            serem usados na identidade e incorporados no conteúdo do token.")]
        public async Task<IActionResult> RecuperarToken(UsuarioModel usuarioModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await Task.FromResult(Ok(
                _myJwtSigningManager.FetchToken(usuarioModel.Nome,
                Enum.GetName(usuarioModel.Funcao)!)
                ));
        }
        /// <summary>
        /// Confira os dados do usuário que estão contidos no Token. 
        /// </summary>
        /// <returns>
        /// Nome do usuário e sua função.
        /// </returns>
        [Authorize]
        [ProducesResponseType<UnauthorizedResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<Ok>(StatusCodes.Status200OK, MediaTypeNames.Text.Plain)]
        [SwaggerOperation("Verifica os dados do usuário que estão contidos no JWT.",
            @"Fornece a opção de verificar o nome e a função do usuário contidos no JWT.
            Forneça um token na aba para segurança(icone do cadeado), a partir de 'RecuperarToken'.")]
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
