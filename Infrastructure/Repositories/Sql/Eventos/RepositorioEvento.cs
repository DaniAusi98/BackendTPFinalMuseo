using Application.Eventos.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Eventos.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Sql.Eventos
{
    internal sealed class RepositorioEvento(MuseoDbContext context):BaseRepository<Evento>(context),IRepositorioEvento
    {
         public async Task<List<Evento>> FindAllAsync(
            DateTime fechaDesde,
            DateTime fechaHasta)
        {
            if (fechaDesde == fechaHasta)
            {
                fechaHasta = fechaHasta.Date.AddDays(1).AddSeconds(-1); // Ajusta hasta el final del dia
            }

            // ============================================================
            // CONSULTA OPTIMIZADA: Trae candidatos potenciales
            // ============================================================
            var eventosEnRango = await Repository
                .Include(a => a.Salas)
                .Include(a => a.Recursos)
                    .ThenInclude(r => r.Recurso)
                .Include(a => a.Exceptions)
                .Where(a =>
                    // Caso A: Actividades fijas individuales que caen justo en esta ventana
                    (!string.IsNullOrEmpty(a.RRule) && a.Horario.Inicio < fechaHasta && a.Horario.Fin > fechaDesde) ||

                    // Caso B: Actividades recurrentes que YA empezaron en el pasado o empiezan ahora.
                    // Si su primera cita hist�rica empez� despu�s de 'fechaHasta', es imposible que generen ocurrencias hoy.
                    (string.IsNullOrEmpty(a.RRule) && a.Horario.Inicio <= fechaHasta))
                    
                .OrderByDescending(a => a.Horario.Inicio)
                .ToListAsync();

            // NOTA T�CTICA: Si una actividad recurrente tiene una fecha l�mite de finalizaci�n (UNTIL) 
            // que qued� en el pasado (ej: termin� el a�o pasado), tu 'ActivityAvailabilityFactory' 
            // al ejecutar el Engine e invocar a 'ExpandRule' generar� una lista vac�a de slots (0 elementos),
            // descart�ndola autom�ticamente del c�lculo en memoria de forma ultra r�pida y segura.

            return eventosEnRango;
        }
    }
}
