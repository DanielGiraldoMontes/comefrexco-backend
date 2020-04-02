using System;
using ComeFrexco.Models;
using ComeFrexco.Repositors;
using ComeFrexco.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComeFrexco.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private LoginRepositor usuarioRepositor;
		private readonly IConfig _config;
		public LoginController(IConfig config)
		{
			this._config = config;
		}

		[Route("Ingresar")]
		[HttpPost]
		public IActionResult Post([FromBody]Usuario usuario)
		{
			try
			{
				usuarioRepositor = new LoginRepositor(_config, usuario);
				Usuario user = usuarioRepositor.Login(usuario);
				return Ok(new
				{
					statusCode = 200,
					message = "success",
					token = _config.GenerateToken(user),
					userData = new { user.userName, user.isAdmin, user.nombre },
					permissions = usuarioRepositor.Permissions(usuario)
				});
			}
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
		}
	}
}