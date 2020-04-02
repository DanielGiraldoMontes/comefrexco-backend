using Microsoft.AspNetCore.Mvc;
using System;

namespace ComeFrexco.Services
{
    public class ResponseContext : ControllerBase
    {
        public IActionResult getFauilureResponse(Exception ex)
        {
            if (ex.Message.Contains("permission"))
            {
                return Conflict(new
                {
                    statusCode = 409,
                    message = "Acceso no permitido",
                });
            }
            else
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    message = ex.Message,
                });
            }
        }
    }
}
