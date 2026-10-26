using Application.ConfiguracionSistema.DataTransferObjets;
using Core.Application;

namespace Application.ConfiguracionSistema.UseCases.Commands.SetMuseumHours
{
    public class SetMuseumHoursCommand : IRequestCommand<string>
    {

        public IEnumerable<HorarioAperturaDto> HorariosApertura { get; set; }

    }
}
