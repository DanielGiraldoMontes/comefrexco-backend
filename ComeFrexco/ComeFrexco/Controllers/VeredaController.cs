using System;
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
    public class VeredaController : ControllerBase
    {
        private FincaRepositor fincaRepositor;
        private readonly IConfig _config;

        public VeredaController(IConfig config)
        {
            this._config = config;
        }

        // GET: api/Vereda
        [HttpGet]
        public IActionResult GetVereda()
        {
            try
            {
                fincaRepositor = new FincaRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    veredas = fincaRepositor.CargarVereda()
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // GET: api/Vereda/5
        [HttpGet("{idVereda}")]
        public IActionResult GetVeredaById(int idVereda)
        {
            try
            {
                fincaRepositor = new FincaRepositor(_config, Request);

                if (!fincaRepositor.ExistVereda(idVereda))
                    throw new Exception("La vereda especificada no existe");

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    veredas = fincaRepositor.CargarVeredaById(idVereda)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // GET: api/Finca/Vereda
        [Route("Municipio/{id}")]
        [HttpGet("Municipio/{id}")]
        public IActionResult GetVeredaByMunicipio(int id)
        {
            try
            {
                fincaRepositor = new FincaRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    veredas = fincaRepositor.CargarVeredaByMunicipio(id)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // POST: api/Vereda
        [HttpPost]
        public IActionResult PostVereda([FromBody] AgregarVereda agregarVereda)
        {
            try
            {
                fincaRepositor = new FincaRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    vereda = fincaRepositor.AgregarVereda(agregarVereda)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // PUT: api/Vereda/5
        [HttpPut("{id}")]
        public IActionResult UpdateVereda(int id, [FromBody] AgregarVereda vereda)
        {
            try
            {
                fincaRepositor = new FincaRepositor(_config, Request);

                if (!fincaRepositor.ExistVereda(id))
                    throw new Exception("La vereda no existe.");

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    vereda = fincaRepositor.ActualizarVereda(vereda, id)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // DELETE: api/Vereda/5
        [HttpDelete("{id}")]
        public IActionResult DeleteVereda(int id)
        {
            try
            {
                fincaRepositor = new FincaRepositor(_config, Request);

                if (!fincaRepositor.ExistVereda(id))
                    throw new Exception("La vereda especificada no existe");

                fincaRepositor.BorrarVereda(id);

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
    }
}
