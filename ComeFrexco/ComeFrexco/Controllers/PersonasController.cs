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
    public class PersonasController : ControllerBase
    {
        private PersonasRepositor personas;
        private readonly IConfig _config;

        public PersonasController(IConfig config)
        {
            this._config = config;
        }

        [HttpGet]
        public IActionResult GetPersonas()
        {
            try
            {
                personas = new PersonasRepositor(_config, Request);

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    personas = personas.Cargar().Tables[0]
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetPersona(string id)
        {
            try
            {
                personas = new PersonasRepositor(_config, Request);

                if (!personas.Exist(id))
                    throw new Exception("La persona no existe");

                return Ok(new
                {
                    statusCode = 200,
                    message = "success",
                    persona = personas.CargarPorId(id).Tables[0]
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutPersona(string id, Personas persona)
        {
            try
            {
                if (id != persona.nit)
                    throw new Exception("Verifique que los datos solicitados esten correctamente diligenciados");

                personas = new PersonasRepositor(_config, Request);

                if (!personas.Exist(persona.nit))
                    throw new Exception("La persona no existe");

                return Ok(new
                {
                    statusCode = 200,
                    message = "Success",
                    persona = personas.Actualizar(persona)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        [HttpPost]
        public IActionResult PostPersona([FromBody]Personas persona)
        {
            try
            {
                if (persona.nit.Length == 0 || persona.nombre1.Length == 0 || persona.apellido1.Length == 0 || persona.telefono.Length == 0)
                    throw new Exception("Los datos son obligatorios.");

                personas = new PersonasRepositor(_config, Request);

                if (personas.Exist(persona.nit))
                    throw new Exception("La persona con ese nit ya esta registrada.");

                return Ok(new
                {
                    statusCode = 200,
                    message = "Success",
                    persona = personas.Agregar(persona)
                });
            }
            catch (Exception ex)
            {
                return new ResponseContext().getFauilureResponse(ex);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePersona(string id)
        {
            try
            {
                personas = new PersonasRepositor(_config, Request);

                if (!personas.Exist(id))
                    throw new Exception("La persona no existe");

                personas.Borrar(id);

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