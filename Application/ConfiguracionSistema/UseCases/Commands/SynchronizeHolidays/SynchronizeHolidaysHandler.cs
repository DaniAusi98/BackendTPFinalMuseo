
using Application.ConfiguracionSistema.Repositories;
using Core.Application;
using Domain.Common.Entities;
using static Domain.Common.Enums.Enums;

namespace Application.ConfiguracionSistema.UseCases.Commands.SynchronizeHolidays
{
    internal sealed class SynchronizeHolidaysHandler(
        IRepositorioCalendarioMuseo repositorioCalendarioMuseo,
        IFeriadosProvider feriadosProvider)
        : IRequestCommandHandler<SynchronizeHolidaysCommand, string>
    {
        private readonly IRepositorioCalendarioMuseo _repositorioCalendarioMuseo =
            repositorioCalendarioMuseo
            ?? throw new ArgumentNullException(nameof(repositorioCalendarioMuseo));

        private readonly IFeriadosProvider _feriadosProvider =
            feriadosProvider
            ?? throw new ArgumentNullException(nameof(feriadosProvider));

        public async Task<string> Handle(
            SynchronizeHolidaysCommand request,
            CancellationToken cancellationToken)
        {
            var feriados = await _feriadosProvider.ObtenerFeriadosAsync(
                request.Year,
                cancellationToken);

            var calendario = await _repositorioCalendarioMuseo
                .ObtenerCalendarioActivoAsync()
                ?? throw new InvalidOperationException(
                    "No se encontró el calendario del museo.");

            var fechasExistentes = calendario.DiasCierre
                .Where(d => d.Motivo == MotivoCierreMuseo.Feriado)
                .Select(d => d.Fecha.Date)
                .ToHashSet();

            var nuevosCierres = new List<DiaCierreMuseo>();

            foreach (var feriado in feriados)
            {
                var fecha = feriado.Fecha.ToDateTime(TimeOnly.MinValue);

                // Evita duplicados existentes y dentro de la respuesta de la API.
                if (!fechasExistentes.Add(fecha.Date))
                    continue;

                nuevosCierres.Add(new DiaCierreMuseo(
                    fecha,
                    MotivoCierreMuseo.Feriado,
                    feriado.Nombre));
            }

            if (nuevosCierres.Count == 0)
            {
                return "No hay feriados nuevos para sincronizar.";
            }

            calendario.AgregarDiasCierre(nuevosCierres);

            _repositorioCalendarioMuseo.Update(
                calendario.Id,
                calendario);

            return $"Sincronización completada. Se agregaron {nuevosCierres.Count} días de cierre.";
        }
    }
}
