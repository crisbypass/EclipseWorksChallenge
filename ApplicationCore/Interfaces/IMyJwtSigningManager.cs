using Microsoft.IdentityModel.Tokens;

namespace Application.Interfaces
{
    public interface IMyJwtSigningManager
    {
        public string CreateJwkJson();
        public string FetchToken(string nomeUsuario, string funcao);
        SecurityKey FetchCurrentEcdsaKey();
    }
}
