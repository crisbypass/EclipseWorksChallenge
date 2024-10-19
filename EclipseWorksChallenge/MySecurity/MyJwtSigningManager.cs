using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using static System.Security.Cryptography.ECCurve;
using System.Security.Cryptography;

namespace EclipseWorksChallenge.MySecurity
{
    public class MyJwtSigningManager : IMyJwtSigningManager
    {
        private readonly JsonWebKey _currentJwk;
        private readonly ECDsaSecurityKey _currentEcdsaKey;
        private readonly IConfiguration _configuration;
        public MyJwtSigningManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _currentJwk = FetchJwkCore(NamedCurves.nistP256);
            _currentEcdsaKey = _currentJwk.FetchECSecurityKey(true);
        }
        public string FetchToken(string nomeUsuario, string funcao) => FetchECDsaSignedToken(nomeUsuario, funcao);
        private string FetchECDsaSignedToken(string userName, string userRole)
        {
            var handler = new JsonWebTokenHandler();
            var now = DateTime.UtcNow;

            ClaimsIdentity identity = new([
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, userRole)
                ]);

            string token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = "Me",
                Audience = "Me",
                NotBefore = now,
                Expires = now.Add(TimeSpan.FromHours(2)),
                IssuedAt = now,
                Subject = identity,
                SigningCredentials = new SigningCredentials
                (
                    _currentEcdsaKey,
                    _currentJwk.Alg
                )
            });

            return token;
        }
        public SecurityKey FetchCurrentEcdsaKey() => _currentEcdsaKey;
        public string CreateJwkJson() => CreateJwkCore_(NamedCurves.nistP256);
        private static string CreateJwkCore_(ECCurve namedCurve)
        {
            using var privateEcdsa = ECDsa.Create(namedCurve);
            var keyId = Guid.NewGuid().ToString();
            var eCParameters = privateEcdsa.ExportParameters(true);

            var secKey = new ECDsaSecurityKey(privateEcdsa)
            {
                KeyId = keyId
            };

            var jwk = new JsonWebKey
            {
                Crv = FetchJwkECType(eCParameters.Curve),
                D = Base64UrlEncoder.Encode(eCParameters.D),
                X = Base64UrlEncoder.Encode(eCParameters.Q.X),
                Y = Base64UrlEncoder.Encode(eCParameters.Q.Y),
                Kty = JsonWebAlgorithmsKeyTypes.EllipticCurve,
                Kid = keyId,
                Alg = FetchEcdsaSecurityAlgorithm(eCParameters.Curve),
                Use = JsonWebKeyUseNames.Sig
            };

            var jwkJson = System.Text.Json.JsonSerializer.Serialize(jwk);

            return jwkJson;
        }
        private JsonWebKey FetchJwkCore(ECCurve namedCurve)
        {
            JsonWebKey jwk;

            if (_configuration.GetSection("MyJwk") != null && _configuration.GetSection("MyJwk").Exists())
            {
                jwk = _configuration.GetSection("MyJwk").Get<JsonWebKey>()!;
            }
            else
            {
                using var privateEcdsa = ECDsa.Create(namedCurve);
                var keyId = Guid.NewGuid().ToString();
                var eCParameters = privateEcdsa.ExportParameters(true);

                jwk = new()
                {
                    Crv = FetchJwkECType(eCParameters.Curve),
                    D = Base64UrlEncoder.Encode(eCParameters.D),
                    X = Base64UrlEncoder.Encode(eCParameters.Q.X),
                    Y = Base64UrlEncoder.Encode(eCParameters.Q.Y),
                    Kty = JsonWebAlgorithmsKeyTypes.EllipticCurve,
                    Kid = keyId,
                    Alg = FetchEcdsaSecurityAlgorithm(eCParameters.Curve),
                    Use = JsonWebKeyUseNames.Sig
                };
            }

            return jwk;
        }
        private static string FetchEcdsaSecurityAlgorithm(ECCurve namedCurve)
        {
            if (namedCurve.Oid == null)
            {
                throw new NotSupportedException();
            }

            string result = null!;

            if (string.Equals(namedCurve.Oid.Value, NamedCurves.nistP256.Oid.Value, StringComparison.Ordinal) || string.Equals(namedCurve.Oid.FriendlyName, NamedCurves.nistP256.Oid.FriendlyName, StringComparison.Ordinal))
            {
                result = SecurityAlgorithms.EcdsaSha256;
            }
            if (string.Equals(namedCurve.Oid.Value, NamedCurves.nistP384.Oid.Value, StringComparison.Ordinal) || string.Equals(namedCurve.Oid.FriendlyName, NamedCurves.nistP384.Oid.FriendlyName, StringComparison.Ordinal))
            {
                result = SecurityAlgorithms.EcdsaSha384;
            }
            if (string.Equals(namedCurve.Oid.Value, NamedCurves.nistP521.Oid.Value, StringComparison.Ordinal) || string.Equals(namedCurve.Oid.FriendlyName, NamedCurves.nistP521.Oid.FriendlyName, StringComparison.Ordinal))
            {
                result = SecurityAlgorithms.EcdsaSha512;
            }

            return result;
        }
        private static string FetchJwkECType(ECCurve namedCurve)
        {
            if (namedCurve.Oid == null)
            {
                throw new NotSupportedException();
            }

            string result = null!;

            if (string.Equals(namedCurve.Oid.Value, NamedCurves.nistP256.Oid.Value, StringComparison.Ordinal) || string.Equals(namedCurve.Oid.FriendlyName, NamedCurves.nistP256.Oid.FriendlyName, StringComparison.Ordinal))
            {
                result = JsonWebKeyECTypes.P256;
            }
            if (string.Equals(namedCurve.Oid.Value, NamedCurves.nistP384.Oid.Value, StringComparison.Ordinal) || string.Equals(namedCurve.Oid.FriendlyName, NamedCurves.nistP384.Oid.FriendlyName, StringComparison.Ordinal))
            {
                result = JsonWebKeyECTypes.P384;
            }
            if (string.Equals(namedCurve.Oid.Value, NamedCurves.nistP521.Oid.Value, StringComparison.Ordinal) || string.Equals(namedCurve.Oid.FriendlyName, NamedCurves.nistP521.Oid.FriendlyName, StringComparison.Ordinal))
            {
                result = JsonWebKeyECTypes.P521;
            }

            return result;
        }
    }
    public static class JsonWebKeyExtensions
    {
        public static ECDsaSecurityKey FetchECSecurityKey(this JsonWebKey jsonWebKey, bool includePrivateKey = false)
        {
            var jwkWebKeyPair = jsonWebKey;

            byte[]? keyD = null;

            if (includePrivateKey && !string.IsNullOrEmpty(jwkWebKeyPair.D))
            {
                keyD = Base64UrlEncoder.DecodeBytes(jwkWebKeyPair.D);
            }

            ECParameters parameters = new()
            {
                Curve = FetchNamedECCurve(jwkWebKeyPair.Crv),
                D = keyD,
                Q = new ECPoint
                {
                    X = Base64UrlEncoder.DecodeBytes(jwkWebKeyPair.X),
                    Y = Base64UrlEncoder.DecodeBytes(jwkWebKeyPair.Y)
                }
            };

            var ecdsaCurve = ECDsa.Create(parameters);

            var ecdsaKey = new ECDsaSecurityKey(ecdsaCurve)
            {
                KeyId = jwkWebKeyPair.KeyId,
            };

            return ecdsaKey;
        }
        private static ECCurve FetchNamedECCurve(string jsonWebKeyECType)
        {
            return jsonWebKeyECType switch
            {
                JsonWebKeyECTypes.P256 => NamedCurves.nistP256,
                JsonWebKeyECTypes.P384 => NamedCurves.nistP384,
                JsonWebKeyECTypes.P521 => NamedCurves.nistP521,
                _ => throw new NotSupportedException()
            };
        }
    }
}
