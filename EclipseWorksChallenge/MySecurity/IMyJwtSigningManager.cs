using Microsoft.IdentityModel.Tokens;

namespace EclipseWorksChallenge.MySecurity
{
    public interface IMyJwtSigningManager
    {
        public string CreateJwkJson();
        public string FetchToken(string nomeUsuario, string funcao);
        SecurityKey FetchCurrentEcdsaKey();
    }
}
