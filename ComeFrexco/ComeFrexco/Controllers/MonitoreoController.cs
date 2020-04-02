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
    public class MonitoreoController : ControllerBase
    {

        private MonitoreoRepositor monitoreoRepositor;
        private readonly IConfig _config;

        public MonitoreoController(IConfig config)
        {
            this._config = config;
        }

        // GET: api/Monitoreo
        [HttpGet]
        public IActionResult GetMonitoreos()
        {
            try
            {
                monitoreoRepositor = new MonitoreoRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    monitoreos = monitoreoRepositor.Cargar()
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // GET: api/Monitoreo/Detalle/5
        [Route("Detalle/{id}")]
        [HttpGet("{id}")]
        public IActionResult GetMonitoreoDetalle(int id)
        {
            try
            {
                monitoreoRepositor = new MonitoreoRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    detalle = monitoreoRepositor.CargarDetalle(id)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // GET: api/Monitoreo/5
        [HttpGet("{id}")]
        public IActionResult GetMonitoreo(int id)
        {
            try
            {
                monitoreoRepositor = new MonitoreoRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    monitoreo = monitoreoRepositor.CargarPorId(id)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // POST: api/Monitoreo
        [HttpPost]
        public IActionResult PostMonitoreo([FromBody] Monitoreo monitoreo)
        {
            try
            {
                monitoreoRepositor = new MonitoreoRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    monitoreo = monitoreoRepositor.Agregar(monitoreo)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // PUT: api/Monitoreo/5
        [HttpPut("{id}")]
        public IActionResult PutMonitoreo(int id, [FromBody] List<MonitoreoDetalle> monitoreoDetalle)
        {
            return Ok(new
            {
                statusCode = 200,
                message = "success"
            });
        }

        // POST: api/Monitoreo/5
        [HttpPost("{id}")]
        public IActionResult PostMonitoreoDetalle(int id, [FromBody] List<MonitoreoDetalle> monitoreoDetalle)
        {
            try
            {
                monitoreoRepositor = new MonitoreoRepositor(_config, Request);

                monitoreoRepositor.AgregarDetalle(id, monitoreoDetalle);

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

        // DELETE: api/Monitoreo/5
        [HttpDelete("{id}")]
        public IActionResult DeleteMonitoreo(int id)
        {
            try
            {
                monitoreoRepositor = new MonitoreoRepositor(_config, Request);

                monitoreoRepositor.Borrar(id);

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
