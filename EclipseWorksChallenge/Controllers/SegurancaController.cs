using Application.Dtos;
using Application.Interfaces;
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
        [Consumes(typeof(InputUsuarioDto), MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Ok), StatusCodes.Status200OK, MediaTypeNames.Text.Plain)]
        [ProducesResponseType(typeof(ValidationProblemDetails),
            StatusCodes.Status400BadRequest, MediaTypeNames.Application.ProblemJson)]
        [SwaggerOperation("Recupera um JWT, para uso em um endpoint protegido.",
            @"Fornece a opção de obter um JWT, para experimentos. Não é necessário
            um código de acesso. Informe apenas um nome de usuário e sua função, para
            serem usados na identidade e incorporados no conteúdo do token. As funções possíveis
            (e seus valores correspondentes) são: Estagiario = 0, Contador = 1, Analista = 2, Gerente = 3")]
        public async Task<IActionResult> RecuperarToken(InputUsuarioDto usuarioModel)
        {
            return await Task.FromResult(Ok(
                _myJwtSigningManager.FetchToken(usuarioModel.NomeUsuario,
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<Ok>(StatusCodes.Status200OK, MediaTypeNames.Text.Plain)]
        [SwaggerOperation("Inspeciona os dados do usuário que estão contidos em um JWT válido.",
            @"Fornece a opção de verificar o nome e a função do usuário contidos em JWT(Ecdsa) válido.
            Forneça um token na aba para segurança(icone do cadeado do endpoint atual), a partir de 'RecuperarToken'.")]
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
