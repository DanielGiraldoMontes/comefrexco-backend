using System;
using ComeFrexco.Models;
using ComeFrexco.Repositors;
using ComeFrexco.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComeFrexco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DepartamentoController : ControllerBase
    {
        private DepartamentoRepositor departamentoRepositor;
        private readonly IConfig _config;

        public DepartamentoController(IConfig config)
        {
            this._config = config;
        }

        // GET: api/Departamento
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                departamentoRepositor = new DepartamentoRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    departamentos = departamentoRepositor.CargarDepartamento().Tables[0]
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // GET: api/Municipio
        [Route("Municipio")]
        [HttpGet]
        public IActionResult GetMunicipio()
        {
            try
            {
                departamentoRepositor = new DepartamentoRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    municipios = departamentoRepositor.CargarMunicipio().Tables[0]
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // GET: api/Departamento/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                departamentoRepositor = new DepartamentoRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    departamento = departamentoRepositor.CargarMunicipioPorDepartamento(id).Tables[0]
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // POST: api/Departamento
        [HttpPost]
        public IActionResult Post([FromBody] Departamento departamento)
        {
            try
            {
                departamentoRepositor = new DepartamentoRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    departamento = departamentoRepositor.AgregarDepartamento(departamento).Tables[0]
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        // POST: api/Municipio
        [Route("Municipio")]
        [HttpPost]
        public IActionResult PostMunicipio([FromBody] Municipio municipio)
        {
            try
            {
                departamentoRepositor = new DepartamentoRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    municipio = departamentoRepositor.AgregarMunicipio(municipio).Tables[0]
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }
    }
}
