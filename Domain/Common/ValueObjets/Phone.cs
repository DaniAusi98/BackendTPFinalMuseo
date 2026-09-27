using Domain.Common.Exceptions;

using System.Text.RegularExpressions;

namespace Domain.Common.ValueObjets
{
    public class Telefono : ValueObject
    {
        public string Valor { get; private set; } = null!;

        public Telefono(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new DomainException("El teléfono es obligatorio.");

            var num = Regex.Replace(input, @"\D", "");

            if (num.StartsWith("00")) num = num[2..];
            if (num.StartsWith("549")) num = num[3..];
            else if (num.StartsWith("54")) num = num[2..];
            else if (num.StartsWith('9') && num.Length > 10) num = num[1..];

            if (num.StartsWith('0')) num = num[1..];

            num = Regex.Replace(num, @"^(\d{2,4})15(\d{6,8})$", "$1$2");

            if (!Regex.IsMatch(num, @"^\d{10}$"))
                throw new DomainException("El número celular debe tener 10 dígitos válidos.");

            Valor = num;
        }

        private Telefono() { } // Para EF Core

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }

        public override string ToString() => Valor;

        public string NumeroInternacional() => $"549{Valor}";
    }
}
