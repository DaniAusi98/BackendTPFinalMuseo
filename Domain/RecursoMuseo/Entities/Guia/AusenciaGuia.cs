using Core.Domain.Entities;

using System;
using Domain.Common.Exceptions;

namespace Domain.RecursoMuseo.Entities.Guia
{
    public class AusenciaGuia : DomainEntity<string>
    {
        public string GuiaId { get; private set; }
        public Guia Guia { get; private set; } = default!;

        public DateTime FechaDesde { get; private set; }
        public DateTime FechaHasta { get; private set; }
        public string Motivo { get; private set; }

        private AusenciaGuia() { }

        public AusenciaGuia(
            DateTime fechaDesde,
            DateTime fechaHasta,
            string motivo)
        {
            Id = Guid.NewGuid().ToString();

            if (fechaHasta < fechaDesde)
                throw new DomainException(
                    "La fecha hasta no puede ser menor a la fecha desde.");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new DomainException(
                    "El motivo de la ausencia no puede ser vacío.");

            FechaDesde = fechaDesde;
            FechaHasta = fechaHasta;
            Motivo = motivo.Trim();
        }

        public void ActualizarFechas(
            DateTime nuevaFechaDesde,
            DateTime nuevaFechaHasta)
        {
            if (nuevaFechaHasta < nuevaFechaDesde)
                throw new DomainException(
                    "La fecha hasta no puede ser menor a la fecha desde.");

            FechaDesde = nuevaFechaDesde;
            FechaHasta = nuevaFechaHasta;
        }
    }
}
