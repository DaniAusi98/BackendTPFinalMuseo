using Application.VisitaGrupal.Repositories;

using Core.Infraestructure.Repositories.Sql;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Microsoft.EntityFrameworkCore;
using static Domain.ActividadMuseo.Enums.Enums;

namespace Infrastructure.Repositories.Sql.VisitaGrupal
{
    internal sealed class RepositorioVisitaGuiada(MuseoDbContext context)
        : BaseRepository<VisitaGrupalGuiada>(context), IRepositorioVisitaGuiada
    {
        public Task<VisitaGrupalGuiada?> FindByIdWithActividadAsync(string id)
        {
            return Repository
                .Include(v => v.Tematicas)
                .Include(v => v.Horario)
                .Include(v => v.Salas)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<List<VisitaGrupalGuiada>> GetAllGroupVisitAsync(
            DateTime fechaDesde,
            DateTime fechaHasta)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("[RepositorioVisitaGuiada] CONSULTANDO");
            Console.WriteLine($"Desde: {fechaDesde:O}");
            Console.WriteLine($"Hasta: {fechaHasta:O}");

            if (fechaDesde == fechaHasta)
            {
                fechaHasta = fechaHasta.Date.AddDays(1).AddSeconds(-1); // Ajusta hasta el final del d�a
            }

            var visitas = await Repository
                .Include(v => v.Tematicas)
                .Include(v => v.Salas)
                .Where(v =>
                    v.Horario.Inicio < fechaHasta &&
                    v.Horario.Fin > fechaDesde)
                .OrderBy(v => v.Horario.Inicio)
                .ToListAsync();

            Console.WriteLine(
                $"[RepositorioVisitaGuiada] Encontradas: {visitas.Count}");

            foreach (var visita in visitas)
            {
                Console.WriteLine(
                    $"Visita {visita.Id} | " +
                    $"Inicio: {visita.Horario.Inicio:O} | " +
                    $"Fin: {visita.Horario.Fin:O}");
            }

            return visitas;
        }

        public async Task<List<VisitaGrupalGuiada>> GetAllGroupVisitCalendarAsync(
            DateTime fechaDesde,
            DateTime fechaHasta)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("[RepositorioVisitaGuiada] CONSULTANDO");
            Console.WriteLine($"Desde: {fechaDesde:O}");
            Console.WriteLine($"Hasta: {fechaHasta:O}");

            if (fechaDesde == fechaHasta)
            {
                fechaHasta = fechaHasta.Date.AddDays(1).AddSeconds(-1); // Ajusta hasta el final del d�a
            }

            var visitas = await Repository
                .Include(v => v.Tematicas)
                .Include(v => v.Salas)
                .Where(v =>
                    v.Horario.Inicio < fechaHasta &&
                    v.Horario.Fin > fechaDesde)
                .Where(v => v.Estado == EstadoActividad.Activa)
                .OrderBy(v => v.Horario.Inicio)
                .ToListAsync();

            Console.WriteLine(
                $"[RepositorioVisitaGuiada] Encontradas: {visitas.Count}");

            foreach (var visita in visitas)
            {
                Console.WriteLine(
                    $"Visita {visita.Id} | " +
                    $"Inicio: {visita.Horario.Inicio:O} | " +
                    $"Fin: {visita.Horario.Fin:O}");
            }

            return visitas;
        }

        public async Task<List<VisitaGrupalGuiada>> ObtenerPorUsuarioIdAsync(
            string usuarioId,
            DateTime fechaActual)
        {
            return await Repository
                .Include(v => v.Tematicas)
                .Include(v => v.Horario)
                .Include(v => v.Salas)
                .Where(v => v.UsuarioVisitanteId == usuarioId)
                .Where(v => v.Horario.Inicio >= fechaActual)
                .Where(v => v.Estado == EstadoActividad.Activa)
                .ToListAsync();
        }

    }
}
