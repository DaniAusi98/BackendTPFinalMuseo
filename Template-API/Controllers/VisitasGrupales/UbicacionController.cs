using Application.ApplicationMuseo.UseCases.Provincia.Queries.GetAllProvincias;
using Application.ApplicationMuseo.UseCases.Provincia.Queries.GetDepartamentosByProvincia;
using Application.ApplicationMuseo.UseCases.Provincia.Queries.GetLocalidadesbyDepartamento;
using Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.VisitasGrupales
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UbicacionController(ICommandQueryBus commandQueryBus) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));

        [HttpGet("provincias")]
        public async Task<IActionResult> GetAlProvincias()
        {
            var entities = await _commandQueryBus.Send(new GetAllProvinciasQuery());

            return Ok(entities);
        }
        [HttpGet("provincia/{provinciaId}/departamentos")]
        // llamada react GET /api/v1/Departamento/provincia/14/departamentos
        public async Task<IActionResult> GetDepartamentosByProvincia(string provinciaId)
        {
            if (string.IsNullOrWhiteSpace(provinciaId))
                return BadRequest();

            var departamentos = await _commandQueryBus.Send(
                new GetDepartamentosByProvinciaQuery
                {
                    ProvinciaId = provinciaId
                });

            return Ok(departamentos);
        }
        // llamada react GET /api/v1/Localidad/departamento/278/localidades 
        [HttpGet("departamento/{departamentoId}/localidades")]
        public async Task<IActionResult> GetLocalidadesByDepartamento(string departamentoId)
        {
            if (string.IsNullOrWhiteSpace(departamentoId))
                return BadRequest();

            var localidades = await _commandQueryBus.Send(
                new GetLocalidadesByDepartamentoQuery
                {
                    DepartamentoId = departamentoId
                });

            return Ok(localidades);
        }

    }
}
/*GET /api/v1/Ubicacion/provincias

GET /api/v1/Ubicacion/provincia/{provinciaId}/departamentos

GET /api/v1/Ubicacion/departamento/{departamentoId}/localidades*/