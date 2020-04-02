using System;
using System.Collections.Generic;
using ComeFrexco.Models;
using ComeFrexco.Repositors;
using ComeFrexco.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComeFrexco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioController : ControllerBase
    {
        private UsuarioRepositor usuarios;
        private readonly IConfig _config;

        public UsuarioController(IConfig config)
        {
            this._config = config;
        }

        [HttpGet]
        public IActionResult GetUsuarios()
        {
            try
            {
                usuarios = new UsuarioRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    users = usuarios.Cargar().Tables[0]
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        [HttpGet("{userName}", Name = "Get")]
        public IActionResult GetUsuario(string userName)
        {
            try
            {
                usuarios = new UsuarioRepositor(_config, Request);

                if (!usuarios.Exist(userName))
                    throw new Exception("El usuario no existe");

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    user = usuarios.CargarPorId(userName).Tables[0]
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        [HttpPost]
        public IActionResult PostUsuario([FromBody] Usuario usuario)
        {
            try
            {
                if (usuario.userName.Length == 0 || usuario.password.Length == 0 || usuario.isAdmin.Length == 0)
                    throw new Exception("Los datos son obligatorios.");

                usuarios = new UsuarioRepositor(_config, Request);

                if (usuarios.Exist(usuario.userName))
                    throw new Exception(String.Format("El usuario {0} ya se encuentra registrado", usuario.userName));

                return Ok(new
                {
                    statusCode = 200,
                    message = "Success",
                    user = usuarios.Agregar(usuario)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        [HttpPut("{userName}")]
        public IActionResult PutUsuario(string userName, [FromBody] Usuario usuario)
        {
            try
            {
                if (userName != usuario.userName)
                    throw new Exception("Verifique que los datos solicitados esten correctamente diligenciados");

                usuarios = new UsuarioRepositor(_config, Request);

                if (!usuarios.Exist(userName))
                    throw new Exception("El usuario no existe");

                return Ok(new
                {
                    statusCode = 200,
                    message = "Success",
                    user = usuarios.Actualizar(usuario)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        [HttpDelete("{userName}")]
        public IActionResult DeleteUsuario(string userName)
        {
            try
            {
                usuarios = new UsuarioRepositor(_config, Request);

                if (!usuarios.Exist(userName))
                    throw new Exception("El usuario no existe");

                usuarios.Borrar(userName);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success"
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        [Route("Permissions/Grante")]
        [HttpPost]
        public IActionResult GrantePermission([FromBody] List<Permissions> permissions)
        {
            try
            {
                foreach (Permissions permiso in permissions)
                {
                    if (permiso.userName.Length == 0 || permiso.module.Length == 0 || permiso.permission.Length == 0)
                        throw new Exception("Los datos son obligatorios.");
                }

                usuarios = new UsuarioRepositor(_config, Request);

                usuarios.GrantPermission(permissions);

                return Ok(new
                {
                    statusCode = 200,
                    message = "Success"
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        [Route("Permissions/Deny")]
        [HttpPost]
        public IActionResult DenyPermission([FromBody] List<Permissions> permissions)
        {
            try
            {
                foreach (Permissions permiso in permissions)
                {
                    if (permiso.userName.Length == 0 || permiso.module.Length == 0 || permiso.permission.Length == 0)
                        throw new Exception("Los datos son obligatorios.");
                }

                usuarios = new UsuarioRepositor(_config, Request);

                usuarios.DenyPermission(permissions);

                return Ok(new
                {
                    statusCode = 200,
                    message = "Success"
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        [Route("Permisos")]
        [HttpPost]
        public IActionResult Permissions([FromBody] Usuario usuario)
        {
            try
            {
                usuarios = new UsuarioRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "Success",
                    permissions = usuarios.Permissions(usuario).Tables[0]
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }
        [Route("RestablecerClave")]
        [HttpPost]
        public IActionResult RestorePassword([FromBody] Usuario usuario)
        {
            try
            {
                usuarios = new UsuarioRepositor(_config, Request);

                usuarios.RestablecerContraseña(usuario);

                return Ok(new
                {
                    statusCode = 200,
                    message = "El usuario debera cerrar la sesión y volverla a abrir.",
                    user = usuarios.CargarPorId(usuario.userName).Tables[0]
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }
    }
}
