using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.ValueObjets
{
    public sealed class DiasLaboralesMuseo
    {
        public IReadOnlyCollection<DayOfWeek> Dias { get; private set; }

        private DiasLaboralesMuseo()
        {
            Dias = Array.Empty<DayOfWeek>();
        }

        public DiasLaboralesMuseo(IEnumerable<DayOfWeek> dias)
        {
            if (dias == null)
                throw new ArgumentNullException(nameof(dias));

            var lista = dias.Distinct().ToList();

            if (!lista.Any())
                throw new ArgumentException("Debe existir al menos un día laboral configurado.");

            Dias = lista.AsReadOnly();
        }
        public bool EsDiaLaboral(DayOfWeek dia)
        {
            return Dias.Contains(dia);
        }
    }

}
