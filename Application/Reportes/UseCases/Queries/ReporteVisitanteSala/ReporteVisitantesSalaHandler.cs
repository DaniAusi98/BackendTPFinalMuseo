using Application.MuseumResources.Repositories;
using Application.Reportes.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Reportes.UseCases.Queries.ReporteVisitanteSala
{
    internal sealed class ReporteVisitantesSalaHandler(
        IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada,
        IRepositorioVisitaGuiada repositorioVisitaGuiada,
        IRepositorioSala repositorioSala)
        : IRequestQueryHandler<ReporteVisitanteSalaQuery, List<ReporteVisitantesporSalaDto>>

    {
        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaGrupalAutoguiada= repositorioVisitaGrupalAutoguiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGrupalAutoguiada));
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        private readonly IRepositorioSala _repositorioSala = repositorioSala ?? throw new ArgumentNullException(nameof(repositorioSala));

        public async Task<List<ReporteVisitantesporSalaDto>> Handle(ReporteVisitanteSalaQuery request, CancellationToken cancellationToken)
        {
            var salas = await _repositorioSala.FindAllAsync();
            var autoguiadas= await _repositorioVisitaGrupalAutoguiada.GetAllGroupVisitAuAsync(request.FechaInicio,request.FechaFin);
            var guiadas = await _repositorioVisitaGuiada.GetAllGroupVisitAsync(request.FechaInicio, request.FechaFin);
            var resultados = new List<ReporteVisitantesporSalaDto>();
            foreach (var sala in salas)
            {
                var cantidadTotalVisitantes = 0;
                var cantidadVisitantesAutoguiadas = autoguiadas
                    .Where(v => v.Salas.Any(s => s.Id == sala.Id))
                    .Sum(v => v.CantidadPersonas?? 0);
                var cantidadVisitantesGuiadas = guiadas
                    .Where(v => v.Salas.Any(s=>s.Id == sala.Id))
                    .Sum(v => v.CantidadPersonas ?? 0);
                cantidadTotalVisitantes = cantidadVisitantesAutoguiadas + cantidadVisitantesGuiadas;
                var reporteDto = new ReporteVisitantesporSalaDto(sala.Id, sala.Nombre, (int)cantidadTotalVisitantes);

                // Aquí puedes agregar el reporteDto a una lista de resultados si lo deseas
                resultados.Add(reporteDto);

            }

            return resultados;
        }
    }
}
