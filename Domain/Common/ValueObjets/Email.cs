using Domain.Common.Exceptions;

using System.Text.RegularExpressions;

namespace Domain.Common.ValueObjets
{
    public class Email : ValueObject
    {
        // Regex compilada normal
        private static readonly Regex _regex = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Valor { get; }

        public Email(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new DomainException("El email es obligatorio.");

            if (!_regex.IsMatch(valor))
                throw new DomainException("El formato del email no es válido.");

            Valor = valor.ToLower();
        }
        

        public Email() { } // Para EF Core


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }

        public override string ToString() => Valor;

        public string Dominio => Valor?.Split('@').Last() ?? "";
        public string Usuario => Valor?.Split('@').First() ?? "";
    }
}
