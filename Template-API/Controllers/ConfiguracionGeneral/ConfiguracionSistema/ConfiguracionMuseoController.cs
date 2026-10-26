using Application.ConfiguracionSistema.UseCases.Commands.CreateClosyngDay;
using Application.ConfiguracionSistema.UseCases.Commands.DeleteClosyngDay;
using Application.ConfiguracionSistema.UseCases.Commands.SetMuseumHours;
using Application.ConfiguracionSistema.UseCases.Queries.ClosyngDay;
using Application.ConfiguracionSistema.UseCases.Queries.GetMuseumHours;
using Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.ConfiguracionGeneral.ConfiguracionSistema
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ConfiguracionHorarioMuseoController(ICommandQueryBus commandQueryBus) : ControllerBase
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus;

        [HttpPost("set-museum-hours")]
        public async Task<IActionResult> Create(SetMuseumHoursCommand command)
        {
            if (command is null) return BadRequest();

            var id = await _commandQueryBus.Send(command);

            return Created($"api/[Controller]/{id}", new { Id = id });
        }
        [HttpGet("get-museum-hours")]
        public async Task<IActionResult> GetMuseumHours()
        {


            var entity = await _commandQueryBus.Send(new GetMuseumHoursQuery());

            return Ok(entity);
        }

        [HttpGet("get-closyng-day")]
        public async Task<IActionResult> GetClosyngDay()
        {


            var entity = await _commandQueryBus.Send(new GetClosyngDayQuery());

            return Ok(entity);
        }

        [HttpPost("create-cl-day")]
        public async Task<IActionResult> CreateClDay(
              [FromBody] CreateClosyngDayCommand command)
        {
            if (command is null) return BadRequest();

            var id = await _commandQueryBus.Send(command);

            return Created($"api/[Controller]/{id}", new { Id = id });
        }

        [HttpDelete("delete-closyng-day")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            await _commandQueryBus.Send(new DeleteClosyngDayCommand { Id = id });

            return NoContent();
        }


    }
}
/* [HttpGet("api/v1/[Controller]")]
        public async Task<IActionResult> GetAll(uint pageIndex = 1, uint pageSize = 10)
        {
            var entities = await _commandQueryBus.Send(new GetAllDummyEntitiesQuery() { PageIndex = pageIndex, PageSize = pageSize });

            return Ok(entities);
        }

        [HttpGet("api/v1/[Controller]/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var entity = await _commandQueryBus.Send(new GetDummyEntityByQuery { DummyIdProperty = id });

            return Ok(entity);
        }

        [HttpPost("api/v1/[Controller]")]
        public async Task<IActionResult> Create(CreateDummyEntityCommand command)
        {
            if (command is null) return BadRequest();

            var id = await _commandQueryBus.Send(command);

            return Created($"api/[Controller]/{id}", new { Id = id });
        }

        [HttpPut("api/v1/[Controller]")]
        public async Task<IActionResult> Update(UpdateDummyEntityCommand command)
        {
            if (command is null) return BadRequest();

            await _commandQueryBus.Send(command);

            return NoContent();
        }

        [HttpDelete("api/v1/[Controller]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            await _commandQueryBus.Send(new DeleteDummyEntityCommand { DummyIdProperty = id });

            return NoContent();
        }*/