using Application.ActividadMuseo.UseCases.ActividadesMuseo.Queries;
using Core.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ActividadesController(ICommandQueryBus commandQueryBus): BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));
           
    


        [HttpGet("calendar")]
        public async Task<IActionResult> GetCalendarActivities(
            DateTime fechaDesde,
            DateTime fechaHasta)
        {
            var activities = await _commandQueryBus.Send(new GetAllActivitiesQuery(fechaDesde,fechaHasta));

            return Ok(activities);
        }







    }
}
