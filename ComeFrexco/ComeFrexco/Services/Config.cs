using System;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ComeFrexco.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ComeFrexco.Services
{
    public class Config : IConfig
    {

        private SqlConnection conectar { get; set; }
        private ControllerBase controllerBase { get; set; }
        private string conexion { get; set; }
        private string cadena = ";Integrated Security=False;User ID={0};Password={1};Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=true";
        private string catalog = ";Initial Catalog=";
        private string source = "Data Source=";

        public void getControllerBase(ControllerBase controllerBase)
        {
            this.controllerBase = controllerBase;
        }

        public Config(IConfiguration config)
        {
            var settings = config.GetSection("MySettings");
            conexion = source + settings.GetSection("server").Value + catalog + settings.GetSection("database").Value + cadena;
        }

        /// <summary>
        /// Metodo que realiza la conexión a la base de datos.
        /// </summary>
        /// <param name="recuest">Argumento con la petición del cliente para validar los datos de acceso al motor.</param>
        /// <returns name="SqlConnection">Retorna un objeto de tipo conexión Sql.</returns>
        public SqlConnection ConectarSqlServer(HttpRequest recuest)
        {
            var Header = recuest.Headers["Authorization"];
            var CredValue = Header.ToString().Substring("Bearer ".Length).Trim();
            Usuario usuario = ValidateToken(CredValue);

            if (conectar == null)
                conectar = new SqlConnection(string.Format(conexion, usuario.userName, usuario.password));

            return conectar;
        }
        public SqlConnection Login(Usuario usuario)
        {
            if (conectar == null)
                conectar = new SqlConnection(string.Format(conexion, usuario.userName, usuario.password));

            return conectar;
        }
        public ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String(Config.getSecret());
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://ComeFrexco.azurewebsites.net",
                    ValidAudience = "https://ComeFrexco.azurewebsites.net",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.getSecret()))
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Usuario ValidateToken(string token)
        {
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }

            Usuario usuario = new Usuario
            {
                userName = identity.FindFirst("User").Value,
                password = identity.FindFirst("Password").Value,
                isAdmin = identity.FindFirst("Admin").Value
            };

            return usuario;
        }
        public string GenerateToken(Usuario usuario)
        {
            // Este objeto crea los pares clave valor que iran en el body del token
            var claims = new Claim[]
        {
            new Claim("User", usuario.userName),
            new Claim("Password",  usuario.password),
            new Claim("Admin",usuario.isAdmin)

        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.getSecret()));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(4);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "https://ComeFrexco.azurewebsites.net",
                audience: "https://ComeFrexco.azurewebsites.net",
                claims: claims,
                expires: expiration,
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static string getSecret()
        {
            return "XCAP05H6LoKvbRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_cadenaAencriptar"></param>
        /// <returns></returns>
        public string Encriptar(string _cadenaAencriptar)
        {
            byte[] encryted = Encoding.Unicode.GetBytes(_cadenaAencriptar);
            return Convert.ToBase64String(encryted);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_cadenaAdesencriptar"></param>
        /// <returns></returns>
        public string DesEncriptar(string _cadenaAdesencriptar)
        {
            byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
            return Encoding.Unicode.GetString(decryted);
        }
        public IActionResult getFauilureResponse(Exception ex)
        {
            if (ex.Message.Contains("permission"))
            {
                return controllerBase.Conflict((
                    statusCode: 409,
                    message: "Acceso no permitido."
                ));
            }
            else
            {
                return controllerBase.BadRequest((
                    statusCode: 400,
                    message: ex.Message
                ));
            }
        }
    }
}