using Core.Application;

namespace Application.ConfiguracionSistema.UseCases.Commands.SynchronizeHolidays
{
    public class SynchronizeHolidaysCommand : IRequestCommand<string>
    {
        public int Year { get; set; }
        public SynchronizeHolidaysCommand()
        {

        }

    }
}
/*using Core.Application;

namespace Application.ConfiguracionSistema.UseCases
    .Commands.SincronizarFeriados
{
    public sealed class SincronizarFeriadosCommand
        : IRequestCommand<string>
    {
        public int Anio { get; init; }
    }
}*/