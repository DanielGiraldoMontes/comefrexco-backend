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
    public class FincaController : ControllerBase
    {
        private FincaRepositor fincaRepositor;
        private readonly IConfig _config;

        public FincaController(IConfig config)
        {
            this._config = config;
        }

        // GET: api/Finca
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                fincaRepositor = new FincaRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    fincas = fincaRepositor.Cargar()
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // GET: api/Finca/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                fincaRepositor = new FincaRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    finca = fincaRepositor.CargarPorId(id)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // POST: api/Finca
        [HttpPost]
        public IActionResult Post([FromBody] Finca finca)
        {
            try
            {
                fincaRepositor = new FincaRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    finca = fincaRepositor.Agregar(finca)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // PUT: api/Finca/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Finca finca)
        {
            try
            {
                fincaRepositor = new FincaRepositor(_config, Request);

                if (id != finca.fin_id)
                    throw new Exception("La peticion no se realizo correctamente.");

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    finca = fincaRepositor.Actualizar(finca)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // DELETE: api/Finca/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                fincaRepositor = new FincaRepositor(_config, Request);

                fincaRepositor.Borrar(id);

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

        // GET: api/Finca/Propietario/5
        [Route("Propietario/{idPropietario}")]
        [HttpGet("{idPropietario}")]
        public IActionResult GetPerPerson(int idPropietario)
        {
            try
            {
                fincaRepositor = new FincaRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    finca = fincaRepositor.CargarPorPropietario(idPropietario)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }
    }
}
