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
    public class PlagaController : ControllerBase
    {

        private PlagaRepositor plagas;
        private readonly IConfig _config;

        public PlagaController(IConfig config)
        {
            this._config = config;
        }

        //// GET: api/Plaga
        [HttpGet]
        public IActionResult GetPlagas()
        {
            try
            {
                plagas = new PlagaRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    plagas = plagas.Cargar()
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        //// GET: api/Plaga/5
        [HttpGet("{id}")]
        public IActionResult GetPlaga(int id)
        {
            try
            {
                plagas = new PlagaRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    plaga = plagas.CargarPorId(id)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // POST: api/Plaga
        [HttpPost]
        public IActionResult PostPlaga([FromBody] Plaga plaga)
        {
            try
            {
                plagas = new PlagaRepositor(_config, Request);
                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    plaga = plagas.Agregar(plaga)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // PUT: api/Plaga/5
        [HttpPut("{id}")]
        public IActionResult PutPlaga(int Id, [FromBody] Plaga plaga)
        {
            try
            {
                plagas = new PlagaRepositor(_config, Request);

                if (Id != plaga.id)
                    throw new Exception("La petición no se realizó correctamente");

                if (!plagas.Exist(Id))
                    throw new Exception("La plaga no existe");

                plagas.Actualizar(plaga);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    plaga = plagas.Actualizar(plaga)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult DeletePlaga(int Id)
        {
            try
            {
                plagas = new PlagaRepositor(_config, Request);

                if (!plagas.Exist(Id))
                    throw new Exception("La plaga no existe");

                plagas.Borrar(Id);

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
